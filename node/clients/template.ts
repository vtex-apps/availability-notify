import type { InstanceOptions, IOContext } from '@vtex/api'
import { JanusClient } from '@vtex/api'

import type { EmailTemplate } from '../typings/emailTemplate'

export class TemplateClient extends JanusClient {
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

  public async createOrUpdateTemplate(
    template: EmailTemplate | string
  ): Promise<boolean> {
    try {
      const body =
        typeof template === 'string' ? JSON.parse(template) : template

      await this.http.post('/api/template-render/pvt/templates', body, {
        metric: 'template-create-or-update',
      })

      return true
    } catch (error) {
      return false
    }
  }

  public async templateExists(templateName: string): Promise<boolean> {
    try {
      await this.http.get(
        `/api/template-render/pvt/templates/${templateName}`,
        {
          metric: 'template-exists',
        }
      )

      return true
    } catch (error) {
      return false
    }
  }
}
