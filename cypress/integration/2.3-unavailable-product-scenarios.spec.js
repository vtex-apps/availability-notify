import availabilityNotifySelectors from '../support/selectors'
import { loginViaCookies, updateRetry } from '../support/common/support'
import { testCase1 } from '../support/outputvalidation'
import {
  generateEmailId,
  updateProductStatus,
} from '../support/availability-notify.apis'
import { subscribeToProductAlerts } from '../support/availability-notify'
import { MESSAGES } from '../support/utils'

const { data1, name, product } = testCase1
const prefix = 'Update product as unavailable'

describe('Updating product as unavailable', () => {
  loginViaCookies()

  const email = generateEmailId()

  updateProductStatus(prefix, data1, false)

  subscribeToProductAlerts({ prefix, product, email, name })

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
