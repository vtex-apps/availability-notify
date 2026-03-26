export interface EmailTemplate {
  Name: string
  FriendlyName: string
  Description: string
  IsDefaultTemplate: boolean
  AccountId: unknown
  AccountName: unknown
  ApplicationId: unknown
  IsPersisted: boolean
  IsRemoved: boolean
  Type: string
  Templates: Templates
}

export interface Templates {
  email: Email
  sms: Sms
}

export interface Email {
  To: string
  CC: unknown
  BCC: unknown
  Subject: string
  Message: string
  Type: string
  ProviderId: unknown
  ProviderName: unknown
  IsActive: boolean
  withError: boolean
}

export interface Sms {
  Type: string
  ProviderId: unknown
  ProviderName: unknown
  IsActive: boolean
  withError: boolean
  Parameters: unknown[]
}
