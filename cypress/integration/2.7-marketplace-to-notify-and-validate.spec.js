import { loginViaCookies, preserveCookie } from '../support/common/support'
import { testCase1 } from '../support/outputvalidation'
import {
  subscribeToProductAlerts,
  verifyEmail,
} from '../support/availability-notify'
import {
  updateProductStatus,
  updateAppSettings,
  generateEmailId,
} from '../support/availability-notify.apis'

const { name, product, warehouseId, skuId } = testCase1
const workspace = Cypress.env().workspace.name
const prefix = 'Marketplace to notify'

describe('Testing market place to notify', () => {
  loginViaCookies()

  const email = generateEmailId()

  updateAppSettings(prefix, true)

  updateProductStatus({ prefix, warehouseId, skuId, unlimited: false })

  subscribeToProductAlerts({ prefix, product, email, name })

  updateAppSettings(prefix, false)

  updateProductStatus({ prefix, warehouseId, skuId, unlimited: true })

  verifyEmail(prefix)

  preserveCookie()
})
