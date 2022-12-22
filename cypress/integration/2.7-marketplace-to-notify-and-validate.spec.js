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

const { data1, name, email, product } = testCase1
const workspace = Cypress.env().workspace.name
const prefix = 'Marketplace to notify'

describe('Testing market place to notify', () => {
  loginViaCookies()

  configureBroadcasterAdapter(prefix, workspace)

  updateAppSettings(prefix, true)

  updateProductStatus(prefix, data1, false)

  subscribeToProductAlerts({ prefix, product, email, name })

  updateAppSettings(prefix, false)

  updateProductStatus(prefix, data1, true)

  triggerBroadCaster(prefix, data1.skuId)

  verifyEmail(prefix)

  preserveCookie()
})
