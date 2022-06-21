import availabilityNotifySelectors from '../support/availability-notify.selectors'
import { testSetup, updateRetry } from '../support/common/support'
import { testCase1 } from '../support/availability-notify.outputvalidation'
import availabilityNotifyConstants from '../support/availability-notify.constants'
import { updateProductStatus } from '../support/availability-notify.apis'

const { data1, name, email } = testCase1

describe('Test external product unavailable scenarios', () => {
  // Load test setup
  testSetup(false)

  updateProductStatus(data1, false)

  it('Open product', updateRetry(3), () => {
    cy.openStoreFront()
    cy.openProduct('weber spirit', true)
  })

  it(
    'verify product should not available and subscribe to product alerts',
    updateRetry(3),
    () => {
      cy.subscribeToProduct({ email, name })
      cy.get(availabilityNotifySelectors.AvailabilityNotifyAlert).should(
        'have.text',
        availabilityNotifyConstants.EmailRegistered
      )
    }
  )
})
