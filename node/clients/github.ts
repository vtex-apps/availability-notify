import type { InstanceOptions, IOContext } from '@vtex/api'
import { ExternalClient } from '@vtex/api'

import {
  REPOSITORY,
  TEMPLATE_FOLDER,
  TEMPLATE_FILE_EXTENSION,
} from '../utils/constants'

export class GitHubClient extends ExternalClient {
  constructor(context: IOContext, options?: InstanceOptions) {
    super('https://raw.githubusercontent.com', context, {
      ...options,
      headers: {
        ...options?.headers,
        'X-Vtex-Use-Https': 'true',
      },
    })
  }

  public async getDefaultTemplate(templateName: string): Promise<string> {
    try {
      const response = await this.http.get<string>(
        `/${REPOSITORY}/${TEMPLATE_FOLDER}/${templateName}.${TEMPLATE_FILE_EXTENSION}`,
        {
          metric: 'github-get-template',
        }
      )

      return typeof response === 'string' ? response : JSON.stringify(response)
    } catch (error) {
      return ''
    }
  }
}
