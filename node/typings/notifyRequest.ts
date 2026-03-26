export interface NotifyRequest {
  id?: string
  name: string
  email: string
  skuId: string
  createdAt: string
  notificationSend: string
  sendAt?: string
  locale?: string
  seller?: SellerObj
}

import { SellerObj } from './sellerObj'
