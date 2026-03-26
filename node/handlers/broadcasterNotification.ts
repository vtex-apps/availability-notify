import type { EventContext } from '@vtex/api'

import type { Clients } from '../clients'
import { AvailabilityVBase } from '../clients/vbase'
import type { BroadcastNotification } from '../typings/broadcastNotification'
import { processNotificationBroadcast } from '../middlewares/vtexApiService'

let throttleCounter = 0

export async function broadcasterNotification(
  ctx: EventContext<Clients>
): Promise<void> {
  throttleCounter++

  if (throttleCounter > 10) {
    throttleCounter--
    // Throttling — event system will retry the event later
    throw new Error('Throttled')
  }

  try {
    let notification: BroadcastNotification

    try {
      notification = (ctx as any).body as BroadcastNotification
    } catch (error) {
      ctx.vtex.logger.error({
        message: 'broadcasterNotification: Error reading Notification',
        error,
      })
      throttleCounter--

      return
    }

    const skuId = notification.IdSku

    if (!skuId) {
      ctx.vtex.logger.warn({
        message: 'broadcasterNotification: Empty Sku',
      })
      throttleCounter--

      return
    }

    const isActive = notification.IsActive
    const inventoryUpdated = notification.StockModified

    if (!isActive || !inventoryUpdated) {
      throttleCounter--

      return
    }

    const vbaseHelper = new AvailabilityVBase(ctx.clients.vbase as any)
    const processingStarted = await vbaseHelper.checkImportLock(skuId)
    const elapsedMs = Date.now() - processingStarted.getTime()

    if (elapsedMs < 60000) {
      ctx.vtex.logger.warn({
        message: `broadcasterNotification: Sku ${skuId} blocked by lock. Processing started: ${processingStarted.toISOString()}`,
      })
      throttleCounter--

      return
    }

    await vbaseHelper.setImportLock(skuId)

    const processed = await processNotificationBroadcast(
      ctx as any,
      notification
    )

    ctx.vtex.logger.info({
      message: `broadcasterNotification: Processed? ${processed} : ${JSON.stringify(notification)}`,
    })
  } catch (error) {
    ctx.vtex.logger.error({
      message: 'broadcasterNotification: Error processing Notification',
      error,
    })
    throttleCounter--
    throw error
  }

  throttleCounter--
}
