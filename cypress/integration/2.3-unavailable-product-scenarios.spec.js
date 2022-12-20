import availabilityNotifySelectors from '../support/selectors'
import { testSetup, updateRetry } from '../support/common/support'
import { testCase1 } from '../support/outputvalidation'
import {
  generateEmailId,
  updateProductStatus,
} from '../support/availability-notify.apis'
import { updateProductAsUnavailable } from '../support/availability-notify'
import { MESSAGES } from '../support/utils'

const { data1, name } = testCase1
const product = 'weber spirit'
const prefix = 'Update product as unavailable'

describe('Updating product as unavailable', () => {
  // Load test setup
  testSetup(false)
  const email = generateEmailId()

  updateProductStatus(prefix, data1, false)

  updateProductAsUnavailable({ prefix, product, email, name })

  it(
    `${prefix} - Verify with same email address and check if we are getting error`,
    updateRetry(3),
    () => {
      cy.subscribeToProduct({ email, name })
      cy.get(availabilityNotifySelectors.AvailabilityNotifyAlert).should(
        'have.text',
        MESSAGES.EmailAlreadyExist
      )
    }
  )
})
