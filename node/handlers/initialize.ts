import {
  verifySchema,
  createDefaultTemplate,
} from '../middlewares/vtexApiService'

export async function initialize(ctx: Context, next: () => Promise<void>) {
  ctx.set('Cache-Control', 'private')

  try {
    const schema = await verifySchema(ctx)
    const template = await createDefaultTemplate(ctx)

    if (schema && template) {
      ctx.status = 200
      ctx.body = 'OK'
    } else {
      ctx.status = 400
      ctx.body = 'Bad Request'
    }
  } catch (error) {
    ctx.vtex.logger.error({ message: 'initialize handler error', error })
    ctx.status = 400
    ctx.body = 'Bad Request'
  }

  await next()
}
