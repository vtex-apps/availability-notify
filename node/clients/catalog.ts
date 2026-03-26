import type { InstanceOptions, IOContext } from '@vtex/api'
import { JanusClient } from '@vtex/api'

import type { GetSkuContextResponse } from '../typings/getSkuContext'
import type { GetSkuSellerResponse } from '../typings/getSkuSeller'

export class CatalogClient extends JanusClient {
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

  public async getSkuContext(
    skuId: string
  ): Promise<GetSkuContextResponse | null> {
    try {
      return await this.http.get<GetSkuContextResponse>(
        `/api/catalog_system/pvt/sku/stockkeepingunitbyid/${skuId}`,
        {
          metric: 'catalog-get-sku-context',
        }
      )
    } catch (error) {
      return null
    }
  }

  public async getSkuSeller(
    sellerId: string,
    skuId: string
  ): Promise<GetSkuSellerResponse | null> {
    try {
      return await this.http.get<GetSkuSellerResponse>(
        `/api/catalog_system/pvt/skuseller/${sellerId}/${skuId}`,
        {
          metric: 'catalog-get-sku-seller',
        }
      )
    } catch (error) {
      return null
    }
  }
}
