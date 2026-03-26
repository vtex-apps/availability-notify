import type { InstanceOptions, IOContext } from '@vtex/api'
import { ExternalClient } from '@vtex/api'

import type { AffiliateNotification } from '../typings/affiliateNotification'

export class AvailabilityClient extends ExternalClient {
  constructor(context: IOContext, options?: InstanceOptions) {
    super('http://app.io.vtex.com', context, {
      ...options,
      headers: {
        ...options?.headers,
        VtexIdclientAutCookie: context.authToken,
        'X-Vtex-Use-Https': 'true',
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
    })
  }

  public async forwardNotification(
    notification: AffiliateNotification,
    accountName: string,
    appMajor: string
  ): Promise<boolean> {
    try {
      await this.http.post(
        `/vtex.availability-notify/v${appMajor}/${accountName}/master/_v/availability-notify/notify`,
        notification,
        {
          metric: 'availability-forward-notification',
        }
      )

      return true
    } catch (error) {
      return false
    }
  }
}
