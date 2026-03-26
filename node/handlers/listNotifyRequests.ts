import {
  isValidAuthUser,
  listNotifyRequests as listRequests,
} from '../middlewares/vtexApiService'

export async function listNotifyRequests(
  ctx: Context,
  next: () => Promise<void>
) {
  try {
    const authStatus = await isValidAuthUser(ctx)

    if (authStatus === 200) {
      const results = await listRequests(ctx)

      ctx.status = 200
      ctx.body = results
    } else {
      ctx.status = 401
      ctx.body = 'Unauthorized'
    }
  } catch (error) {
    ctx.vtex.logger.error({ message: 'listNotifyRequests handler error', error })
    ctx.status = 500
    ctx.body = 'Internal Server Error'
  }

  await next()
}
