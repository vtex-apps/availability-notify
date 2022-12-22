import { loginViaCookies, preserveCookie } from '../support/common/support'
import { testCase1 } from '../support/outputvalidation'
import { triggerBroadCaster } from '../support/broadcaster.api'
import {
  subscribeToProductAlerts,
  verifyEmail,
} from '../support/availability-notify'
import {
  updateProductStatus,
  updateAppSettings,
  configureBroadcasterAdapter,
} from '../support/availability-notify.apis'
import availbalityNotifyProducts from '../support/products'

const { data1, name, email } = testCase1
const workspace = Cypress.env().workspace.name
const prefix = 'Availability notify'

describe('Test availability notify scenarios', () => {
  loginViaCookies()

  updateAppSettings(prefix, true)

  updateProductStatus(prefix, data1, false)

  subscribeToProductAlerts({
    prefix,
    product: availbalityNotifyProducts.Lenovo.name,
    email,
    name,
  })

  configureBroadcasterAdapter(prefix, workspace)

  updateAppSettings(prefix, false)

  updateProductStatus(prefix, data1, true)

  triggerBroadCaster(prefix, data1.skuId)

  verifyEmail(prefix)

  preserveCookie()
})
