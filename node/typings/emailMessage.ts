import { GetSkuContextResponse } from './getSkuContext'
import { NotifyRequest } from './notifyRequest'

export interface JsonData {
  SkuContext: GetSkuContextResponse
  NotifyRequest: NotifyRequest
}

export interface EmailMessage {
  ProviderName: unknown
  TemplateName: string
  JsonData: JsonData
}
