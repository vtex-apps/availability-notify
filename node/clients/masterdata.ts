import type { InstanceOptions, IOContext } from '@vtex/api'
import { JanusClient } from '@vtex/api'

import {
  DATA_ENTITY,
  SCHEMA,
  SCHEMA_JSON,
  FIELDS,
} from '../utils/constants'
import type { NotifyRequest } from '../typings/notifyRequest'

export class MasterDataClient extends JanusClient {
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

  public async verifySchema(): Promise<boolean> {
    const url = `/api/dataentities/${DATA_ENTITY}/schemas/${SCHEMA}`

    try {
      const response = await this.http.getRaw(url, {
        metric: 'masterdata-get-schema',
      })

      const responseText =
        typeof response.data === 'string'
          ? response.data
          : JSON.stringify(response.data)

      if (responseText !== SCHEMA_JSON) {
        await this.http.putRaw(url, SCHEMA_JSON, {
          metric: 'masterdata-put-schema',
          headers: {
            'Content-Type': 'application/json',
          },
        })
      }

      return true
    } catch (error) {
      return false
    }
  }

  public async saveNotifyRequest(
    notifyRequest: NotifyRequest,
    account: string
  ): Promise<boolean> {
    const url = `/api/dataentities/${DATA_ENTITY}/documents`

    try {
      await this.http.put(url, notifyRequest, {
        metric: 'masterdata-save-notify',
        headers: {
          'X-Vtex-Account': account,
        },
      })

      return true
    } catch (error) {
      return false
    }
  }

  public async deleteNotifyRequest(documentId: string): Promise<boolean> {
    const url = `/api/dataentities/${DATA_ENTITY}/documents/${documentId}`

    try {
      await this.http.delete(url, {
        metric: 'masterdata-delete-notify',
      })

      return true
    } catch (error) {
      return false
    }
  }

  public async searchRequests(
    account: string,
    searchString: string,
    searchFrom?: number
  ): Promise<NotifyRequest[]> {
    const allRequests: NotifyRequest[] = []
    const from = searchFrom ?? 0
    const to = from + 99

    const url = `/api/dataentities/${DATA_ENTITY}/search?_fields=${FIELDS}&_schema=${SCHEMA}&${searchString}`

    try {
      const response = await this.http.getRaw(url, {
        metric: 'masterdata-search-requests',
        headers: {
          'REST-Range': `resources=${from}-${to}`,
          'X-Vtex-Account': account,
        },
      })

      const requests: NotifyRequest[] = response.data
      allRequests.push(...requests)

      const contentRange = response.headers?.['rest-content-range']

      if (contentRange) {
        // format: "resources 0-99/168"
        const parts = contentRange.split(' ')
        const ranges = parts[1].split('/')
        const responseTotal = parseInt(ranges[1], 10)
        const responseTo = parseInt(ranges[0].split('-')[1], 10)

        if (responseTo < responseTotal) {
          const nextFrom = responseTo + 1
          const moreRequests = await this.searchRequests(
            account,
            searchString,
            nextFrom
          )

          allRequests.push(...moreRequests)
        }
      }
    } catch (error) {
      // Return whatever we've gathered so far
    }

    return allRequests
  }

  public async scrollRequests(account: string): Promise<NotifyRequest[]> {
    const allRequests: NotifyRequest[] = []
    let url = `/api/dataentities/${DATA_ENTITY}/scroll?_fields=${FIELDS}`

    try {
      let response = await this.http.getRaw(url, {
        metric: 'masterdata-scroll-requests',
        headers: {
          'X-Vtex-Account': account,
        },
      })

      let requests: NotifyRequest[] = response.data
      allRequests.push(...requests)
      let returnedRecords = requests.length

      let mdToken = response.headers?.['x-vtex-md-token']

      while (mdToken && returnedRecords > 0) {
        url = `/api/dataentities/${DATA_ENTITY}/scroll?_token=${mdToken}`

        response = await this.http.getRaw(url, {
          metric: 'masterdata-scroll-requests',
          headers: {
            'X-Vtex-Account': account,
          },
        })

        requests = response.data
        returnedRecords = requests.length

        if (returnedRecords > 0) {
          allRequests.push(...requests)
        }

        mdToken = response.headers?.['x-vtex-md-token']
      }
    } catch (error) {
      // Return whatever we've gathered so far
    }

    return allRequests
  }
}
