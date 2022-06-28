import availabilityNotifySelectors from '../support/availability-notify.selectors'
import { testSetup, updateRetry } from '../support/common/support'
import { testCase1 } from '../support/availability-notify.outputvalidation'
import availabilityNotifyConstants from '../support/availability-notify.constants'
import {
  generateEmailId,
  updateProductStatus,
} from '../support/availability-notify.apis'

const { data1, name } = testCase1
const prefix = 'Update product as unavailable'

describe('Test external product unavailable scenarios', () => {
  // Load test setup
  testSetup(false)

  const email = generateEmailId()

  updateProductStatus(prefix, data1, false)

  it(`${prefix} - Open product`, updateRetry(3), () => {
    cy.openStoreFront()
    cy.openProduct('weber spirit', true)
  })

  it(
    `${prefix} - Verify product should not available and subscribe to product alerts`,
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
