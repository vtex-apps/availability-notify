export interface ResponseWrapper {
  responseText: string
  message: string
  isSuccess: boolean
  masterDataToken?: string
  total?: string
  from?: string
  to?: string
}
