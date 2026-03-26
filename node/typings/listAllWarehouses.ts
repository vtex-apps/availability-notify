export interface ListAllWarehousesResponse {
  id: string
  name: string
  warehouseDocks: WarehouseDock[]
  pickupPointIds: unknown[]
  priority: number
  isActive: boolean
}

export interface WarehouseDock {
  dockId: string
  time: string
  cost: number
}
