import { processAllRequests as processAll } from '../middlewares/vtexApiService'

export async function prcocessAllRequests(
  ctx: Context,
  next: () => Promise<void>
) {
  try {
    const results = await processAll(ctx)

    ctx.vtex.logger.info({
      message: 'prcocessAllRequests',
      results: results.join(', '),
    })

    ctx.status = 200
    ctx.body = 'OK'
  } catch (error) {
    ctx.vtex.logger.error({ message: 'prcocessAllRequests handler error', error })
    ctx.status = 500
    ctx.body = 'Internal Server Error'
  }

  await next()
}
