import { json } from 'co-body'

import type { AffiliateNotification } from '../typings/affiliateNotification'
import { processNotificationAffiliate } from '../middlewares/vtexApiService'

export async function processNotification(
  ctx: Context,
  next: () => Promise<void>
) {
  ctx.set('Cache-Control', 'private')

  try {
    if (ctx.method.toLowerCase() !== 'post') {
      ctx.status = 400
      await next()

      return
    }

    const notification = (await json(ctx.req)) as AffiliateNotification
    const sent = await processNotificationAffiliate(ctx, notification)

    ctx.status = sent ? 200 : 400
    ctx.body = sent ? 'OK' : 'Bad Request'
  } catch (error) {
    ctx.vtex.logger.error({ message: 'processNotification handler error', error })
    ctx.status = 400
    ctx.body = 'Bad Request'
  }

  await next()
}
