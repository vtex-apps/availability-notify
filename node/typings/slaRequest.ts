import { Dimension } from './getSkuContext'

export interface SlaRequest {
  items: SlaItem[]
  location: Location
  salesChannel: string
}

export interface SlaItem {
  id: string
  groupItemId: unknown
  kitItem: unknown[]
  quantity: number
  price: number
  additionalHandlingTime: string
  dimension: Dimension
}

export interface Location {
  zipCode: string
  country: string
  instore: Instore
}

export interface Instore {
  isCheckedIn: boolean
  storeId: string
}
