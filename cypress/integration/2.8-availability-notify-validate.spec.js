import { loginViaCookies, preserveCookie } from '../support/common/support'
import { testCase2 } from '../support/outputvalidation'
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

const { name, email, warehouseId, skuId } = testCase2
const workspace = Cypress.env().workspace.name
const prefix = 'Availability notify'

describe('Test availability notify scenarios', () => {
  loginViaCookies()

  updateAppSettings(prefix, true)

  updateProductStatus({ prefix, warehouseId, skuId, unlimited: false })

  subscribeToProductAlerts({
    prefix,
    product: availbalityNotifyProducts.Lenovo.name,
    email,
    name,
  })

  configureBroadcasterAdapter(prefix, workspace)

  updateAppSettings(prefix, false)

  updateProductStatus({ prefix, warehouseId, skuId, unlimited: true })

  triggerBroadCaster(prefix, skuId)

  verifyEmail(prefix)

  preserveCookie()
})
