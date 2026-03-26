import type { ServiceContext } from '@vtex/api'

import type { Clients } from '../clients'
import { AvailabilityVBase } from '../clients/vbase'
import type { AffiliateNotification } from '../typings/affiliateNotification'
import type { BroadcastNotification } from '../typings/broadcastNotification'
import type { CartSimulationRequest, CartItem } from '../typings/cartSimulation'
import type { EmailMessage, JsonData } from '../typings/emailMessage'
import type { EmailTemplate } from '../typings/emailTemplate'
import type { GetSkuContextResponse } from '../typings/getSkuContext'
import type { NotifyRequest } from '../typings/notifyRequest'
import type { ProcessingResult } from '../typings/processingResult'
import type { RequestContext } from '../typings/requestContext'
import type { SellerObj } from '../typings/sellerObj'
import type { ShopperAddress } from '../typings/shopperAddress'
import type { ShopperRecord } from '../typings/shopperRecord'
import {
  Availability,
  DEFAULT_TEMPLATE_NAME,
  VtexOrderStatus,
} from '../utils/constants'

type Ctx = ServiceContext<Clients>

export async function verifySchema(ctx: Ctx): Promise<boolean> {
  try {
    return await ctx.clients.mdClient.verifySchema()
  } catch (error) {
    ctx.vtex.logger.error({ message: 'verifySchema error', error })

    return false
  }
}

export async function createDefaultTemplate(ctx: Ctx): Promise<boolean> {
  let templateExists = false
  const templateName = DEFAULT_TEMPLATE_NAME

  try {
    templateExists = await ctx.clients.template.templateExists(templateName)

    if (!templateExists) {
      const templateBody = await ctx.clients.github.getDefaultTemplate(
        templateName
      )

      if (!templateBody) {
        ctx.vtex.logger.warn({
          message: `createDefaultTemplate: Failed to load template ${templateName}`,
        })
      } else {
        const emailTemplate: EmailTemplate = JSON.parse(templateBody)

        emailTemplate.Templates.email.Message =
          emailTemplate.Templates.email.Message.replace(/\\r/g, '')
        emailTemplate.Templates.email.Message =
          emailTemplate.Templates.email.Message.replace(/\\n/g, '\n')
        templateExists = await ctx.clients.template.createOrUpdateTemplate(
          emailTemplate
        )
      }
    }
  } catch (error) {
    ctx.vtex.logger.error({ message: 'createDefaultTemplate error', error })
  }

  return templateExists
}

export async function getTotalAvailableForSku(
  ctx: Ctx,
  sku: string,
  _requestContext: RequestContext
): Promise<number> {
  let totalAvailable = 0

  const listAllWarehouses = await ctx.clients.logistics.listAllWarehouses()
  const inventoryBySku = await ctx.clients.logistics.listInventoryBySku(sku)

  if (inventoryBySku?.balance) {
    try {
      const activeWarehouseIds = (listAllWarehouses ?? [])
        .filter((w) => w.isActive)
        .map((w) => w.id)

      const hasUnlimited = inventoryBySku.balance.some(
        (i) =>
          i.hasUnlimitedQuantity && activeWarehouseIds.includes(i.warehouseId)
      )

      if (hasUnlimited) {
        totalAvailable = 1
        ctx.vtex.logger.debug({
          message: `getTotalAvailableForSku: Sku '${sku}' Has Unlimited Quantity.`,
        })
      } else {
        const totalQuantity = inventoryBySku.balance
          .filter((i) => activeWarehouseIds.includes(i.warehouseId))
          .reduce((sum, i) => sum + i.totalQuantity, 0)

        const totalReserved = inventoryBySku.balance
          .filter((i) => activeWarehouseIds.includes(i.warehouseId))
          .reduce((sum, i) => sum + i.reservedQuantity, 0)

        totalAvailable = totalQuantity - totalReserved
        ctx.vtex.logger.debug({
          message: `getTotalAvailableForSku: Sku '${sku}' ${totalQuantity} - ${totalReserved} = ${totalAvailable}`,
        })
      }
    } catch (error) {
      ctx.vtex.logger.error({
        message: `getTotalAvailableForSku: Error calculating for sku '${sku}'`,
        error,
      })
    }
  }

  // if marketplace inventory is zero, check seller inventory
  if (totalAvailable === 0) {
    const skuContextResponse = await ctx.clients.catalog.getSkuContext(sku)

    if (skuContextResponse?.SkuSellers) {
      const cartSimulationRequest: CartSimulationRequest = {
        items: [],
        postalCode: '',
        country: '',
      }

      for (const skuSeller of skuContextResponse.SkuSellers) {
        cartSimulationRequest.items.push({
          id: sku,
          quantity: 1,
          seller: skuSeller.SellerId,
        } as CartItem)
      }

      try {
        const cartSimulationResponse = await ctx.clients.checkout.cartSimulation(
          cartSimulationRequest
        )

        if (cartSimulationResponse?.items) {
          const availableItems = cartSimulationResponse.items.filter(
            (i) => i.availability === Availability.Available
          )

          totalAvailable += availableItems.reduce(
            (sum, i) => sum + i.quantity,
            0
          )
        }
      } catch (error) {
        ctx.vtex.logger.error({
          message: `getTotalAvailableForSku: Error from seller(s) for sku '${sku}'`,
          error,
        })
      }
    }
  }

  return totalAvailable
}

export async function sendEmail(
  ctx: Ctx,
  notifyRequest: NotifyRequest,
  skuContext: GetSkuContextResponse,
  requestContext: RequestContext
): Promise<boolean> {
  const templateName = DEFAULT_TEMPLATE_NAME

  const emailMessage: EmailMessage = {
    TemplateName: templateName,
    ProviderName: requestContext.account,
    JsonData: {
      SkuContext: skuContext,
      NotifyRequest: notifyRequest,
    } as JsonData,
  }

  try {
    const success = await ctx.clients.mail.sendMail(
      emailMessage,
      requestContext.account
    )

    ctx.vtex.logger.debug({
      message: `sendEmail: ${JSON.stringify(emailMessage)}`,
      success,
    })

    return success
  } catch (error) {
    ctx.vtex.logger.error({ message: 'sendEmail: Failure', error })

    return false
  }
}

export async function getShopperByEmail(
  ctx: Ctx,
  email: string
): Promise<ShopperRecord[] | null> {
  try {
    const url = `/api/dataentities/CL/search?email=${email}`

    const result = await (ctx.clients as any).mdClient.http.get(url, {
      metric: 'masterdata-search-shopper',
      headers: {
        VtexIdclientAutCookie: ctx.vtex.authToken,
        'X-Vtex-Use-Https': 'true',
      },
    }) as ShopperRecord[]

    return result
  } catch (error) {
    ctx.vtex.logger.error({
      message: `getShopperByEmail: Error for '${email}'`,
      error,
    })

    return null
  }
}

export async function getShopperAddressById(
  ctx: Ctx,
  id: string
): Promise<ShopperAddress[] | null> {
  const searchFields = 'country,postalCode'

  try {
    const url = `/api/dataentities/AD/search?userId=${id}&_fields=${searchFields}`

    const result = await (ctx.clients as any).mdClient.http.get(url, {
      metric: 'masterdata-search-address',
      headers: {
        VtexIdclientAutCookie: ctx.vtex.authToken,
        'X-Vtex-Use-Https': 'true',
      },
    }) as ShopperAddress[]

    return result
  } catch (error) {
    ctx.vtex.logger.error({
      message: `getShopperAddressById: Error for '${id}'`,
      error,
    })

    return null
  }
}

export async function canShipToShopper(
  ctx: Ctx,
  notifyRequest: NotifyRequest,
  _requestContext: RequestContext
): Promise<boolean> {
  let canSend = false
  const shopperRecord = await getShopperByEmail(ctx, notifyRequest.email)

  if (shopperRecord && shopperRecord.length > 0) {
    const matchingRecord = shopperRecord.find(
      (sr) => sr.accountName === ctx.vtex.account
    )

    const shopperAddresses = matchingRecord
      ? await getShopperAddressById(ctx, matchingRecord.id)
      : null

    if (shopperAddresses && shopperAddresses.length > 0) {
      let sellerId = ''

      if (notifyRequest.seller?.sellerId) {
        sellerId = notifyRequest.seller.sellerId
      }

      const cartSimulationRequest: CartSimulationRequest = {
        items: [
          {
            id: notifyRequest.skuId,
            quantity: 1,
            seller: sellerId,
          } as CartItem,
        ],
        postalCode: '',
        country: '',
      }

      // Deduplicate addresses
      const uniqueAddresses = shopperAddresses.filter(
        (addr, index, self) =>
          index ===
          self.findIndex(
            (a) =>
              a.postalCode === addr.postalCode && a.country === addr.country
          )
      )

      for (const shopperAddress of uniqueAddresses) {
        cartSimulationRequest.postalCode = shopperAddress.postalCode
        cartSimulationRequest.country = shopperAddress.country

        const cartSimulationResponse = await ctx.clients.checkout.cartSimulation(
          cartSimulationRequest
        )

        if (
          cartSimulationResponse?.items &&
          cartSimulationResponse.items.length > 0 &&
          cartSimulationResponse.items[0].availability ===
            Availability.Available
        ) {
          canSend = true
          break
        }
      }
    }
  }

  return canSend
}

async function processNotificationInternal(
  ctx: Ctx,
  requestContext: RequestContext,
  isActive: boolean,
  inventoryUpdated: boolean,
  skuId: string
): Promise<boolean> {
  let success = false

  if (isActive && inventoryUpdated) {
    const merchantSettings = await ctx.clients.appsClient.getMerchantSettings()
    const requestsToNotify = await ctx.clients.mdClient.searchRequests(
      requestContext.account,
      `notificationSend=false&skuId=${skuId}`
    )

    if (requestsToNotify) {
      const seen = new Set<string>()
      const distinct = requestsToNotify.filter((r) => {
        if (seen.has(r.email)) return false
        seen.add(r.email)

        return true
      })

      if (distinct.length > 0) {
        const available = await getTotalAvailableForSku(
          ctx,
          skuId,
          requestContext
        )

        if (available > 0) {
          const skuContextResponse = await ctx.clients.catalog.getSkuContext(
            skuId
          )

          if (skuContextResponse) {
            for (const requestToNotify of distinct) {
              let doSendMail = true

              if (merchantSettings.doShippingSim) {
                doSendMail = await canShipToShopper(
                  ctx,
                  requestToNotify,
                  requestContext
                )
              }

              if (doSendMail) {
                const mailSent = await sendEmail(
                  ctx,
                  requestToNotify,
                  skuContextResponse,
                  requestContext
                )

                if (mailSent) {
                  requestToNotify.notificationSend = 'true'
                  requestToNotify.sendAt = new Date().toISOString()

                  const updatedRequest = await ctx.clients.mdClient.saveNotifyRequest(
                    requestToNotify,
                    requestContext.account
                  )

                  success = updatedRequest

                  if (!updatedRequest) {
                    ctx.vtex.logger.error({
                      message: `processNotificationInternal: Mail sent but failed to update: ${JSON.stringify(requestToNotify)}`,
                    })
                  }
                }
              } else {
                ctx.vtex.logger.debug({
                  message: `processNotificationInternal: SkuId '${skuId}' cannot ship to '${requestToNotify.email}'`,
                })
              }
            }
          } else {
            ctx.vtex.logger.warn({
              message: `processNotificationInternal: Null SkuContext for skuId ${skuId}`,
            })
          }
        } else {
          ctx.vtex.logger.debug({
            message: `processNotificationInternal: SkuId '${skuId}' ${available} available`,
          })
        }
      }
    } else {
      ctx.vtex.logger.debug({
        message: `processNotificationInternal: Request returned NULL for ${skuId}`,
      })
    }
  }

  return success
}

export async function forwardNotification(
  ctx: Ctx,
  notification: BroadcastNotification,
  accountName: string,
  _requestContext: RequestContext
): Promise<boolean> {
  if (!accountName) {
    ctx.vtex.logger.warn({ message: 'forwardNotification: Account name is empty.' })

    return false
  }

  const trimmedAccount = accountName.trim()

  if (ctx.vtex.account.toLowerCase() === trimmedAccount.toLowerCase()) {
    ctx.vtex.logger.warn({
      message: 'forwardNotification: Skipping self reference.',
    })

    return true
  }

  const affiliateNotification: AffiliateNotification = {
    An: notification.An,
    HasStockKeepingUnitRemovedFromAffiliate: notification.HasStockKeepingUnitRemovedFromAffiliate,
    IdAffiliate: notification.IdAffiliate,
    IsActive: notification.IsActive,
    DateModified: notification.DateModified,
    HasStockKeepingUnitModified: notification.HasStockKeepingUnitModified,
    IdSku: notification.IdSku,
    PriceModified: notification.PriceModified,
    ProductId: notification.ProductId,
    StockModified: notification.StockModified,
    Version: notification.Version,
  }

  const appMajor = process.env.VTEX_APP_VERSION?.split('.')[0] ?? '1'

  try {
    return await ctx.clients.availability.forwardNotification(
      affiliateNotification,
      trimmedAccount,
      appMajor
    )
  } catch (error) {
    ctx.vtex.logger.warn({
      message: `forwardNotification: Error forwarding to '${trimmedAccount}'`,
      error,
    })

    return false
  }
}

export async function processNotificationAffiliate(
  ctx: Ctx,
  notification: AffiliateNotification
): Promise<boolean> {
  let success = true
  const requestContext: RequestContext = {
    account: ctx.vtex.account,
    authToken: ctx.vtex.authToken,
  }

  if (notification.An !== requestContext.account) {
    const getSkuSellerResponse = await ctx.clients.catalog.getSkuSeller(
      notification.An,
      notification.IdSku
    )

    if (getSkuSellerResponse) {
      notification.IdSku = getSkuSellerResponse.StockKeepingUnitId.toString()
    } else {
      ctx.vtex.logger.warn({ message: 'processNotificationAffiliate: SKU NOT FOUND' })
    }
  }

  const { IsActive: isActive, StockModified: inventoryUpdated, IdSku: skuId } = notification

  ctx.vtex.logger.debug({
    message: `processNotificationAffiliate: Sku:${skuId} Active?${isActive} Inventory Changed?${inventoryUpdated}`,
  })

  success = await processNotificationInternal(
    ctx,
    requestContext,
    isActive,
    inventoryUpdated,
    skuId
  )

  if (isActive && inventoryUpdated) {
    const merchantSettings = await ctx.clients.appsClient.getMerchantSettings()

    if (merchantSettings.notifyMarketplace) {
      const results: string[] = []
      const broadcastNotification: BroadcastNotification = {
        An: notification.An,
        HasStockKeepingUnitRemovedFromAffiliate: notification.HasStockKeepingUnitRemovedFromAffiliate,
        IdAffiliate: notification.IdAffiliate,
        IsActive: notification.IsActive,
        DateModified: notification.DateModified,
        HasStockKeepingUnitModified: notification.HasStockKeepingUnitModified,
        IdSku: notification.IdSku,
        PriceModified: notification.PriceModified,
        ProductId: notification.ProductId,
        StockModified: notification.StockModified,
        Version: notification.Version,
      }

      const marketplaces = merchantSettings.notifyMarketplace.split(',')

      for (const marketplace of marketplaces) {
        const successThis = await forwardNotification(
          ctx,
          broadcastNotification,
          marketplace,
          requestContext
        )

        results.push(`'${marketplace}' ${successThis}`)
        success = success && successThis
      }

      ctx.vtex.logger.info({
        message: `processNotificationAffiliate: Sku:${skuId}`,
        accounts: results.join('\n'),
      })
    }
  }

  return success
}

export async function processNotificationBroadcast(
  ctx: Ctx,
  notification: BroadcastNotification
): Promise<boolean> {
  let success = false
  const requestContext: RequestContext = {
    account: ctx.vtex.account,
    authToken: ctx.vtex.authToken,
  }

  const { IsActive: isActive, StockModified: inventoryUpdated, IdSku: skuId } = notification

  success = await processNotificationInternal(
    ctx,
    requestContext,
    isActive,
    inventoryUpdated,
    skuId
  )

  if (isActive && inventoryUpdated) {
    const merchantSettings = await ctx.clients.appsClient.getMerchantSettings()

    if (merchantSettings.notifyMarketplace) {
      const results: string[] = []
      const marketplaces = merchantSettings.notifyMarketplace.split(',')

      for (const marketplace of marketplaces) {
        const successThis = await forwardNotification(
          ctx,
          notification,
          marketplace,
          requestContext
        )

        results.push(`'${marketplace}' ${successThis}`)
      }

      ctx.vtex.logger.info({
        message: `processNotificationBroadcast: Sku:${skuId}`,
        accounts: results.join('\n'),
      })
    }
  }

  return success
}

export async function processAllRequests(ctx: Ctx): Promise<string[]> {
  const results: string[] = []
  const requestContext: RequestContext = {
    account: ctx.vtex.account,
    authToken: ctx.vtex.authToken,
  }

  const allRequests = await ctx.clients.mdClient.scrollRequests(
    requestContext.account
  )

  if (allRequests && allRequests.length > 0) {
    for (const requestToNotify of allRequests) {
      let doSendMail = false
      let updatedRecord = false
      const { skuId } = requestToNotify

      if (requestToNotify.notificationSend === 'true') {
        results.push(
          `${skuId} ${requestToNotify.email} Sent at ${requestToNotify.sendAt}`
        )
      } else {
        const available = await getTotalAvailableForSku(
          ctx,
          skuId,
          requestContext
        )

        if (available > 0) {
          let canSend = true
          const merchantSettings = await ctx.clients.appsClient.getMerchantSettings()

          if (merchantSettings.doShippingSim) {
            canSend = await canShipToShopper(ctx, requestToNotify, requestContext)
          }

          if (canSend) {
            const skuContextResponse = await ctx.clients.catalog.getSkuContext(skuId)

            if (skuContextResponse) {
              doSendMail = await sendEmail(ctx, requestToNotify, skuContextResponse, requestContext)

              if (doSendMail) {
                requestToNotify.notificationSend = 'true'
                requestToNotify.sendAt = new Date().toISOString()
                updatedRecord = await ctx.clients.mdClient.saveNotifyRequest(
                  requestToNotify,
                  requestContext.account
                )
              }
            }
          }
        }

        results.push(
          `${skuId} Qnty:${available} '${requestToNotify.email}' Sent? ${doSendMail} Updated? ${updatedRecord}`
        )
      }
    }
  } else {
    results.push('No requests to notify.')
  }

  return results
}

export async function processUnsentRequests(ctx: Ctx): Promise<ProcessingResult[]> {
  const results: ProcessingResult[] = []
  const requestContext: RequestContext = {
    account: ctx.vtex.account,
    authToken: ctx.vtex.authToken,
  }

  const allRequests = await ctx.clients.mdClient.searchRequests(
    requestContext.account,
    'notificationSend=false'
  )

  if (allRequests && allRequests.length > 0) {
    for (const requestToNotify of allRequests) {
      let doSendMail = false
      let updatedRecord = false
      const { skuId } = requestToNotify

      const available = await getTotalAvailableForSku(ctx, skuId, requestContext)

      if (available > 0) {
        let canSend = true
        const merchantSettings = await ctx.clients.appsClient.getMerchantSettings()

        if (merchantSettings.doShippingSim) {
          canSend = await canShipToShopper(ctx, requestToNotify, requestContext)
        }

        if (canSend) {
          const skuContextResponse = await ctx.clients.catalog.getSkuContext(skuId)

          if (skuContextResponse) {
            doSendMail = await sendEmail(ctx, requestToNotify, skuContextResponse, requestContext)

            if (doSendMail) {
              requestToNotify.notificationSend = 'true'
              requestToNotify.sendAt = new Date().toISOString()
              updatedRecord = await ctx.clients.mdClient.saveNotifyRequest(
                requestToNotify,
                requestContext.account
              )
            }
          }
        }
      }

      results.push({
        quantityAvailable: available.toString(),
        email: requestToNotify.email,
        sent: doSendMail,
        skuId,
        updated: updatedRecord,
      })
    }
  } else {
    results.push({} as ProcessingResult)
  }

  return results
}

export async function availabilitySubscribe(
  ctx: Ctx,
  email: string,
  sku: string,
  name: string,
  locale: string,
  sellerObj: SellerObj
): Promise<boolean> {
  const requestContext: RequestContext = {
    account: ctx.vtex.account,
    authToken: ctx.vtex.authToken,
  }

  const requestsToNotify = await ctx.clients.mdClient.searchRequests(
    requestContext.account,
    `notificationSend=false&skuId=${sku}`
  )

  if (requestsToNotify.some((x) => x.email === email)) {
    return false
  }

  const notifyRequest: NotifyRequest = {
    createdAt: new Date().toISOString(),
    email,
    skuId: sku,
    name,
    notificationSend: 'false',
    locale,
    seller: sellerObj,
  }

  return ctx.clients.mdClient.saveNotifyRequest(
    notifyRequest,
    requestContext.account
  )
}

export async function checkUnsentNotifications(ctx: Ctx): Promise<void> {
  const windowInMinutes = 100
  const vbaseHelper = new AvailabilityVBase(ctx.clients.vbase as any)
  const lastCheck = await vbaseHelper.getLastUnsentCheck()

  const now = new Date()
  const checkThreshold = new Date(
    lastCheck.getTime() + windowInMinutes * 60 * 1000
  )

  if (checkThreshold < now) {
    const processingResults = await processUnsentRequests(ctx)

    ctx.vtex.logger.info({
      message: 'checkUnsentNotifications',
      results: JSON.stringify(processingResults),
    })

    await vbaseHelper.setLastUnsentCheck(now)
  }
}

export async function isValidAuthUser(ctx: Ctx): Promise<number> {
  const adminToken = ctx.vtex.adminUserAuthToken

  if (!adminToken) {
    return 401
  }

  try {
    const validatedUser = await ctx.clients.vtexId.validateUserToken(adminToken)

    const hasPermission =
      validatedUser != null && validatedUser.authStatus === 'Success'

    if (!hasPermission) {
      ctx.vtex.logger.warn({ message: 'isValidAuthUser: User Does Not Have Permission' })

      return 403
    }

    return 200
  } catch (error) {
    ctx.vtex.logger.error({ message: 'isValidAuthUser: Error fetching user', error })

    return 400
  }
}

export async function listNotifyRequests(ctx: Ctx): Promise<NotifyRequest[]> {
  try {
    return await ctx.clients.mdClient.scrollRequests(ctx.vtex.account)
  } catch (error) {
    ctx.vtex.logger.error({ message: 'listNotifyRequests error', error })

    return []
  }
}

export async function processNotificationAllStates(
  ctx: Ctx,
  notification: { currentState: string }
): Promise<void> {
  switch (notification.currentState) {
    case VtexOrderStatus.StartHanding:
      await checkUnsentNotifications(ctx)
      break
    default:
      break
  }
}
