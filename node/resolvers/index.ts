import {
  listNotifyRequests,
  availabilitySubscribe,
  isValidAuthUser,
  processUnsentRequests,
} from '../middlewares/vtexApiService'

export const resolvers = {
  Query: {
    listRequests: async (_: unknown, __: unknown, ctx: Context) => {
      const notifyRequests = await listNotifyRequests(ctx)

      return notifyRequests
    },
  },
  Mutation: {
    availabilitySubscribe: async (
      _: unknown,
      args: {
        name: string
        email: string
        skuId: string
        locale: string
        sellerObj: {
          sellerId: string
          sellerName: string
          addToCartLink: string
          sellerDefault: boolean
        }
      },
      ctx: Context
    ) => {
      const { name, email, skuId, locale, sellerObj } = args

      ctx.vtex.logger.debug({
        message: `GraphQL AvailabilitySubscribe: '${name}' '${email}' '${skuId}' '${locale}'`,
      })

      return availabilitySubscribe(ctx, email, skuId, name, locale, sellerObj)
    },
    deleteRequest: async (
      _: unknown,
      args: { id: string },
      ctx: Context
    ) => {
      const authStatus = await isValidAuthUser(ctx)

      if (authStatus !== 200) {
        throw new Error(`Unauthorized: ${authStatus}`)
      }

      const { id } = args

      return ctx.clients.mdClient.deleteNotifyRequest(id)
    },
    processUnsentRequests: async (
      _: unknown,
      __: unknown,
      ctx: Context
    ) => {
      const authStatus = await isValidAuthUser(ctx)

      if (authStatus !== 200) {
        throw new Error(`Unauthorized: ${authStatus}`)
      }

      return processUnsentRequests(ctx)
    },
  },
}
