import {
  testSetup,
  updateRetry,
  preserveCookie,
} from '../support/common/support'
import {
  testCase1,
  appDetails,
} from '../support/availability-notify.outputvalidation'
import { triggerBroadCaster } from '../support/broadcaster.api'
import { verifyEmail } from '../support/availability-notify'
import {
  updateProductStatus,
  configureTargetWorkspace,
  configureBroadcasterAdapter,
} from '../support/availability-notify.apis'
import availabilityNotifySelectors from '../support/availability-notify.selectors'
import availabilityNotifyConstants from '../support/availability-notify.constants'
import availbalityNotifyProducts from '../support/availability-notify.products'
const { data1, name, email } = testCase1
const { app, version } = appDetails

describe('Testing', () => {
  // Load test setup
  testSetup()
  configureTargetWorkspace('vtex.availability-notify', '1.7.3', true)
  updateProductStatus(data1, false)
  it('Open product', updateRetry(3), () => {
    cy.openStoreFront()
    cy.openProduct(availbalityNotifyProducts.Lenovo.name, true)
  })
  it('Enable marketplace to notify and validate', updateRetry(3), () => {
    cy.subscribeToProduct({ email, name })
    cy.get(availabilityNotifySelectors.AvailabilityNotifyAlert).should(
      'have.text',
      availabilityNotifyConstants.EmailRegistered
    )
  })
  configureBroadcasterAdapter(app, version)
  configureTargetWorkspace('vtex.availability-notify', '1.7.3', false)
  updateProductStatus(data1, true)
  triggerBroadCaster(data1.skuId)
  verifyEmail()
  preserveCookie()
})
