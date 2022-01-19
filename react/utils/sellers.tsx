import type { Seller as SellerFromContext } from 'vtex.product-context'

import type { SellerObj } from '../AvailabilityNotifier'

export type Seller = SellerFromContext & SellerObj

export function getDefaultSeller(sellers?: SellerFromContext[]) {
  if (!sellers || sellers.length === 0) {
    return null
  }

  const defaultSeller = sellers.find(seller => seller.sellerDefault)

  if (defaultSeller) {
    return defaultSeller
  }

  return sellers[0]
}
