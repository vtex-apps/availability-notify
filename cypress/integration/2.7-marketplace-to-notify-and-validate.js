import {
  testSetup,
  updateRetry,
  preserveCookie,
} from '../support/common/support'
import { testCase1 } from '../support/availability-notify.outputvalidation'
import { triggerBroadCaster } from '../support/broadcaster.api'
import { verifyEmail } from '../support/availability-notify'
import {
  updateProductStatus,
  configureTargetWorkspace,
} from '../support/availability-notify.apis'
import availabilityNotifySelectors from '../support/availability-notify.selectors'
import availabilityNotifyConstants from '../support/availability-notify.constants'

const { data1, name, email, appDetails } = testCase1

describe('Testing', () => {
  // Load test setup
  testSetup()

  configureTargetWorkspace(appDetails.app, appDetails.version, true)

  updateProductStatus(data1, false)

  it('Open product', updateRetry(3), () => {
    cy.openStoreFront()
    cy.openProduct('weber spirit', true)
  })

  it('Enable marketplace to notify and validate', updateRetry(3), () => {
    cy.subscribeToProduct({ email, name })
    cy.get(availabilityNotifySelectors.AvailabilityNotifyAlert).should(
      'have.text',
      availabilityNotifyConstants.EmailRegistered
    )
  })

  configureTargetWorkspace('vtex.availability-notify', '1.7.3', false)
  updateProductStatus(data1, true)

  triggerBroadCaster(data1.skuId)

  verifyEmail()

  preserveCookie()
})
