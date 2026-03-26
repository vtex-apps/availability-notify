export interface InventoryBySku {
  skuId: string
  balance: Balance[]
}

export interface Balance {
  warehouseId: string
  warehouseName: string
  totalQuantity: number
  reservedQuantity: number
  hasUnlimitedQuantity: boolean
  timeToRefill: unknown
  dateOfSupplyUtc: unknown
}
