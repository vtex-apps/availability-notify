import type { EventContext } from '@vtex/api'

import type { Clients } from '../clients'
import type { AllStatesNotification } from '../typings/allStatesNotification'
import { processNotificationAllStates } from '../middlewares/vtexApiService'

export async function allStates(ctx: EventContext<Clients>): Promise<void> {
  try {
    const notification = (ctx as any).body as AllStatesNotification

    await processNotificationAllStates(ctx as any, notification)
  } catch (error) {
    ctx.vtex.logger.warn({
      message: `allStates: Error processing Orders Broadcaster Notification: ${
        error instanceof Error ? error.message : String(error)
      }`,
    })
  }
}
