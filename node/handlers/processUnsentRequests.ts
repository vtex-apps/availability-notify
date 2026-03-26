import { processUnsentRequests as processUnsent } from '../middlewares/vtexApiService'

export async function processUnsentRequests(
  ctx: Context,
  next: () => Promise<void>
) {
  try {
    const results = await processUnsent(ctx)

    ctx.vtex.logger.info({
      message: 'processUnsentRequests',
      results: JSON.stringify(results),
    })

    ctx.status = 200
    ctx.body = results
  } catch (error) {
    ctx.vtex.logger.error({ message: 'processUnsentRequests handler error', error })
    ctx.status = 500
    ctx.body = 'Internal Server Error'
  }

  await next()
}
