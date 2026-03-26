import type { EventContext } from '@vtex/api'

import type { Clients } from '../clients'
import type { AppInstalledEvent } from '../typings/appInstalledEvent'
import { APP_SETTINGS } from '../utils/constants'
import {
  verifySchema,
  createDefaultTemplate,
} from '../middlewares/vtexApiService'

export async function OnAppInstalled(
  ctx: EventContext<Clients>
): Promise<void> {
  const event = (ctx as any).body as AppInstalledEvent

  if (event?.To?.Id?.includes(APP_SETTINGS)) {
    await verifySchema(ctx as any)
    await createDefaultTemplate(ctx as any)
  }
}
