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

describe('Update product as available and validate', () => {
  testSetup(false)

  configureBroadcasterAdapter(workspace)

  updateProductStatus(data1, true)

  triggerBroadCaster(data1.skuId)

  verifyEmail(data1.email)

  preserveCookie()
})
