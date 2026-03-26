import type { InstanceOptions, IOContext } from '@vtex/api'
import { JanusClient } from '@vtex/api'

import type { ValidatedUser } from '../typings/validatedUser'

export class VtexIdClient extends JanusClient {
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

  public async validateUserToken(
    token: string
  ): Promise<ValidatedUser | null> {
    try {
      return await this.http.post<ValidatedUser>(
        '/api/vtexid/credential/validate',
        { token },
        {
          metric: 'vtexid-validate-token',
        }
      )
    } catch (error) {
      return null
    }
  }
}
