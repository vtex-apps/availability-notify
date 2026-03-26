export interface CartSimulationRequest {
  items: CartItem[]
  postalCode: string
  country: string
}

export interface CartItem {
  id: string
  requestIndex: number
  quantity: number
  seller: string
  sellerChain?: string[]
  tax: number
  priceValidUntil?: string
  price: number
  listPrice: number
  rewardValue: number
  sellingPrice: number
  offerings?: unknown[]
  priceTags?: unknown[]
  measurementUnit?: string
  unitMultiplier: number
  parentItemIndex?: unknown
  parentAssemblyBinding?: unknown
  availability?: string
}
