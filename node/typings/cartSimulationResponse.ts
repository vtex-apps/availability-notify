export interface CartSimulationResponse {
  items: CartResponseItem[]
  ratesAndBenefitsData: RatesAndBenefitsData
  paymentData: PaymentData
  selectableGifts: unknown[]
  marketingData: unknown
  postalCode: string
  country: string
  logisticsInfo: LogisticsInfo[]
  messages: unknown[]
  purchaseConditions: PurchaseConditions
  pickupPoints: unknown[]
  subscriptionData: unknown
  totals: Total[]
  itemMetadata: unknown
}

export interface CartResponseItem {
  id: string
  requestIndex: string
  quantity: number
  seller: string
  sellerChain: string[]
  tax: string
  priceValidUntil: string
  price: string
  listPrice: string
  rewardValue: string
  sellingPrice: string
  offerings: unknown[]
  priceTags: unknown[]
  measurementUnit: string
  unitMultiplier: string
  parentItemIndex: unknown
  parentAssemblyBinding: unknown
  availability: string
  catalogProvider: string
  priceDefinition: PriceDefinition
}

export interface PriceDefinition {
  calculatedSellingPrice: string
  total: string
  sellingPrices: SellingPrice[]
}

export interface SellingPrice {
  value: string
  quantity: string
}

export interface LogisticsInfo {
  itemIndex: number
  addressId: unknown
  selectedSla: unknown
  selectedDeliveryChannel: unknown
  quantity: number
  shipsTo: string[]
  slas: Sla[]
  deliveryChannels: DeliveryChannel[]
}

export interface DeliveryChannel {
  id: string
}

export interface Sla {
  id: string
  deliveryChannel: string
  name: string
  deliveryIds: DeliveryId[]
  shippingEstimate: string
  shippingEstimateDate: unknown
  lockTTL: unknown
  availableDeliveryWindows: unknown[]
  deliveryWindow: unknown
  price: string
  listPrice: string
  tax: string
  pickupStoreInfo: PickupStoreInfo
  pickupPointId: unknown
  pickupDistance: string
  polygonName: unknown
  transitTime: string
}

export interface DeliveryId {
  courierId: string
  warehouseId: string
  dockId: string
  courierName: string
  quantity: number
  kitItemDetails: unknown[]
}

export interface PickupStoreInfo {
  isPickupStore: boolean
  friendlyName: unknown
  address: unknown
  additionalInfo: unknown
  dockId: unknown
}

export interface PaymentData {
  installmentOptions: InstallmentOption[]
  paymentSystems: PaymentSystem[]
  payments: unknown[]
  giftCards: unknown[]
  giftCardMessages: unknown[]
  availableAccounts: unknown[]
  availableTokens: unknown[]
}

export interface InstallmentOption {
  paymentSystem: string
  bin: unknown
  paymentName: string
  paymentGroupName: string
  value: number
  installments: Installment[]
}

export interface Installment {
  count: number
  hasInterestRate?: boolean
  interestRate?: number
  value?: number
  total?: number
  sellerMerchantInstallments?: Installment[]
  id?: string
}

export interface PaymentSystem {
  id: string
  name: string
  groupName: string
  validator: unknown
  stringId: string
  template: string
  requiresDocument: boolean
  isCustom: boolean
  description: string
  requiresAuthentication: boolean
  dueDate: string
  availablePayments: unknown
}

export interface PurchaseConditions {
  itemPurchaseConditions: ItemPurchaseCondition[]
}

export interface ItemPurchaseCondition {
  id: string
  seller: string
  sellerChain: string[]
  slas: Sla[]
  price: string
  listPrice: string
}

export interface RatesAndBenefitsData {
  rateAndBenefitsIdentifiers: unknown[]
  teaser: unknown[]
}

export interface Total {
  id: string
  name: string
  value: string
}
