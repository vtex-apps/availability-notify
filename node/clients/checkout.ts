import type { InstanceOptions, IOContext } from '@vtex/api'
import { JanusClient } from '@vtex/api'

import type { CartSimulationRequest } from '../typings/cartSimulation'
import type { CartSimulationResponse } from '../typings/cartSimulationResponse'

export class CheckoutClient extends JanusClient {
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

  public async cartSimulation(
    request: CartSimulationRequest
  ): Promise<CartSimulationResponse | null> {
    try {
      return await this.http.post<CartSimulationResponse>(
        '/api/checkout/pub/orderForms/simulation',
        request,
        {
          metric: 'checkout-cart-simulation',
        }
      )
    } catch (error) {
      return null
    }
  }
}
