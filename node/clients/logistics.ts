import type { InstanceOptions, IOContext } from '@vtex/api'
import { JanusClient } from '@vtex/api'

import type { InventoryBySku } from '../typings/inventoryBySku'
import type { ListAllWarehousesResponse } from '../typings/listAllWarehouses'

export class LogisticsClient extends JanusClient {
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

  public async listInventoryBySku(sku: string): Promise<InventoryBySku | null> {
    try {
      return await this.http.get<InventoryBySku>(
        `/api/logistics/pvt/inventory/skus/${sku}`,
        {
          metric: 'logistics-inventory-by-sku',
        }
      )
    } catch (error) {
      return null
    }
  }

  public async listAllWarehouses(): Promise<ListAllWarehousesResponse[] | null> {
    try {
      return await this.http.get<ListAllWarehousesResponse[]>(
        '/api/logistics/pvt/configuration/warehouses',
        {
          metric: 'logistics-list-warehouses',
        }
      )
    } catch (error) {
      return null
    }
  }
}
