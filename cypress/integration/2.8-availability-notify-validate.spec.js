import { loginViaCookies, preserveCookie } from '../support/common/support'
import { testCase2 } from '../support/outputvalidation'
import {
  subscribeToProductAlerts,
  verifyEmail,
} from '../support/availability-notify'
import {
  updateProductStatus,
  updateAppSettings,
  generateEmailId,
} from '../support/availability-notify.apis'
import availbalityNotifyProducts from '../support/products'

const { name, warehouseId, skuId } = testCase2
const workspace = Cypress.env().workspace.name
const prefix = 'Availability notify'

describe('Test availability notify scenarios', () => {
  loginViaCookies()

  const email = generateEmailId()

  updateAppSettings(prefix, true)

  updateProductStatus({ prefix, warehouseId, skuId, unlimited: false })

  subscribeToProductAlerts({
    prefix,
    product: availbalityNotifyProducts.Lenovo.name,
    email,
    name,
  })

  updateAppSettings(prefix, false)

  updateProductStatus({ prefix, warehouseId, skuId, unlimited: true })

  verifyEmail(prefix)

  preserveCookie()
})
