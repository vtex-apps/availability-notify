// Header constants
export const APP_TOKEN = 'X-VTEX-API-AppToken'
export const APP_KEY = 'X-VTEX-API-AppKey'
export const END_POINT_KEY = 'availability-notify'
export const APP_NAME = 'availability-notify'

export const FORWARDED_HEADER = 'X-Forwarded-For'
export const FORWARDED_HOST = 'X-Forwarded-Host'
export const APPLICATION_JSON = 'application/json'
export const HEADER_VTEX_CREDENTIAL = 'X-Vtex-Credential'
export const AUTHORIZATION_HEADER_NAME = 'Authorization'
export const PROXY_AUTHORIZATION_HEADER_NAME = 'Proxy-Authorization'
export const USE_HTTPS_HEADER_NAME = 'X-Vtex-Use-Https'
export const PROXY_TO_HEADER_NAME = 'X-Vtex-Proxy-To'
export const VTEX_ACCOUNT_HEADER_NAME = 'X-Vtex-Account'
export const ENVIRONMENT = 'vtexcommercestable'
export const LOCAL_ENVIRONMENT = 'myvtex'
export const VTEX_ID_HEADER_NAME = 'VtexIdclientAutCookie'
export const HEADER_VTEX_WORKSPACE = 'X-Vtex-Workspace'
export const APP_SETTINGS = 'vtex.availability-notify'
export const ACCEPT = 'Accept'
export const CONTENT_TYPE = 'Content-Type'
export const MINICART = 'application/vnd.vtex.checkout.minicart.v1+json'
export const HTTP_FORWARDED_HEADER = 'HTTP_X_FORWARDED_FOR'
export const API_VERSION_HEADER = 'x-api-version'

// VBase bucket and lock constants
export const BUCKET = 'availability-notify'
export const LOCK = 'availability-notify-lock'
export const UNSENT_CHECK = 'check-unsent'

// MasterData entity and schema
export const DATA_ENTITY = 'notify'
export const SCHEMA = 'notify'
export const SCHEMA_JSON =
  '{"name":"notify","properties":{"skuId":{"type":"string","title":"skuId"},"sendAt":{"type":"string","title":"sendAt"},"name":{"type":"string","title":"name"},"email":{"type":"string","title":"email"},"createdAt":{"type":"string","title":"createdAt"},"notificationSend":{"type":"string","title":"notificationSend"},"locale":{"type":"string","title":"locale"},"seller":{"type":"object","title":"seller"}},"v-indexed":["skuId","notificationSend"],"v-security":{"allowGetAll":true}}'
export const FIELDS =
  'id,email,skuId,notificationSend,sendAt,name,createdAt,locale,seller'

// Mail service
export const MAIL_SERVICE =
  'http://mailservice.vtex.com.br/api/mail-service/pvt/sendmail'
export const ACQUIRER = 'AvailabilityNotify'

// GitHub template constants
export const GITHUB_URL = 'https://raw.githubusercontent.com'
export const REPOSITORY = 'vtex-apps/availability-notify/master'
export const TEMPLATE_FOLDER = 'templates'
export const TEMPLATE_FILE_EXTENSION = 'json'
export const DEFAULT_TEMPLATE_NAME = 'back-in-stock'

// Availability statuses
export const Availability = {
  CannotBeDelivered: 'cannotBeDelivered',
  Available: 'available',
}

// Domain constants
export const Domain = {
  Fulfillment: 'Fulfillment',
  Marketplace: 'Marketplace',
}

// VTEX Order Statuses
export const VtexOrderStatus = {
  OrderCreated: 'order-created',
  OrderCompleted: 'order-completed',
  OnOrderCompleted: 'on-order-completed',
  PaymentPending: 'payment-pending',
  WaitingForOrderAuthorization: 'waiting-for-order-authorization',
  ApprovePayment: 'approve-payment',
  PaymentApproved: 'payment-approved',
  PaymentDenied: 'payment-denied',
  RequestCancel: 'request-cancel',
  WaitingForSellerDecision: 'waiting-for-seller-decision',
  AuthorizeFullfilment: 'authorize-fulfillment',
  OrderCreateError: 'order-create-error',
  OrderCreationError: 'order-creation-error',
  WindowToCancel: 'window-to-cancel',
  ReadyForHandling: 'ready-for-handling',
  StartHanding: 'start-handling',
  Handling: 'handling',
  InvoiceAfterCancellationDeny: 'invoice-after-cancellation-deny',
  OrderAccepted: 'order-accepted',
  Invoice: 'invoice',
  Invoiced: 'invoiced',
  Replaced: 'replaced',
  CancellationRequested: 'cancellation-requested',
  Cancel: 'cancel',
  Canceled: 'canceled',
  Cancelled: 'cancelled',
}
