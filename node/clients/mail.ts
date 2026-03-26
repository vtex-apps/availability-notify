import type { InstanceOptions, IOContext } from '@vtex/api'
import { ExternalClient } from '@vtex/api'

import type { EmailMessage } from '../typings/emailMessage'

export class MailClient extends ExternalClient {
  constructor(context: IOContext, options?: InstanceOptions) {
    super('http://mailservice.vtex.com.br', context, {
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

  public async sendMail(
    message: EmailMessage,
    accountName: string
  ): Promise<boolean> {
    try {
      await this.http.post(
        `/api/mail-service/pvt/sendmail?an=${accountName}`,
        message,
        {
          metric: 'mail-send',
        }
      )

      return true
    } catch (error) {
      return false
    }
  }
}
