import {
  updateProductStatus,
  configureBroadcasterAdapter,
} from '../support/availability-notify.apis'
import { preserveCookie, loginViaCookies } from '../support/common/support'
import { testCase1 } from '../support/outputvalidation'
import { triggerBroadCaster } from '../support/broadcaster.api'
import { verifyEmail } from '../support/availability-notify'

const { skuId, warehouseId } = testCase1
const workspace = Cypress.env().workspace.name
const prefix = 'Update product as available'

describe('Update product as available and validate', () => {
  loginViaCookies()

  configureBroadcasterAdapter(prefix, workspace)

  updateProductStatus({ prefix, warehouseId, skuId, unlimited: true })

  triggerBroadCaster(prefix, skuId)

  verifyEmail(prefix)

  preserveCookie()
})
