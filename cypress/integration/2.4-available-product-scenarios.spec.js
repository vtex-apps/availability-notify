import {
  updateProductStatus,
  configureBroadcasterAdapter,
} from '../support/availability-notify.apis'
import { preserveCookie, testSetup } from '../support/common/support'
import { testCase1 } from '../support/availability-notify.outputvalidation'
import { triggerBroadCaster } from '../support/broadcaster.api'
import { verifyEmail } from '../support/availability-notify'

const { data1 } = testCase1
const workspace = Cypress.env().workspace.name
const prefix = 'Update product as available'

describe('Update product as available and validate', () => {
  testSetup(false)

  configureBroadcasterAdapter(prefix, workspace)

  updateProductStatus(prefix, data1, true)

  triggerBroadCaster(prefix, data1.skuId)

  verifyEmail(prefix)

  preserveCookie()
})