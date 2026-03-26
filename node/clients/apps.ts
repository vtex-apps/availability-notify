import type { InstanceOptions, IOContext } from '@vtex/api'
import { JanusClient } from '@vtex/api'

import { APP_SETTINGS } from '../utils/constants'
import type { MerchantSettings } from '../typings/merchantSettings'

export class AppsClient extends JanusClient {
  constructor(context: IOContext, options?: InstanceOptions) {
    super(context, {
      ...options,
      headers: {
        ...options?.headers,
        VtexIdclientAutCookie: context.authToken,
        'X-Vtex-Use-Https': 'true',
      },
    })
  }

  public async getMerchantSettings(): Promise<MerchantSettings> {
    try {
      const region = process.env.VTEX_REGION ?? ''
      const account = this.context.account
      const workspace = this.context.workspace

      return await this.http.get<MerchantSettings>(
        `http://apps.${region}.vtex.io/${account}/${workspace}/apps/${APP_SETTINGS}/settings`,
        {
          metric: 'apps-get-settings',
        }
      )
    } catch (error) {
      return {} as MerchantSettings
    }
  }

  public async setMerchantSettings(
    merchantSettings: MerchantSettings
  ): Promise<void> {
    try {
      const region = process.env.VTEX_REGION ?? ''
      const account = this.context.account
      const workspace = this.context.workspace

      await this.http.put(
        `http://apps.${region}.vtex.io/${account}/${workspace}/apps/${APP_SETTINGS}/settings`,
        merchantSettings,
        {
          metric: 'apps-set-settings',
        }
      )
    } catch (error) {
      // Silently fail
    }
  }
}
