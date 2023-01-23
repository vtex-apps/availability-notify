import availabilityNotifySelectors from '../support/selectors'
import { loginViaAPI, updateRetry } from '../support/common/support'
import { testCase1 } from '../support/outputvalidation'
import {
  generateEmailId,
  updateProductStatus,
  configureBroadcasterAdapter,
} from '../support/availability-notify.apis'
import { subscribeToProductAlerts } from '../support/availability-notify'
import { MESSAGES } from '../support/utils'

const { name, product, warehouseId, skuId } = testCase1
const workspace = Cypress.env().workspace.name

const prefix = 'Update product as unavailable'

describe('Updating product as unavailable', () => {
  loginViaAPI()

  const email = generateEmailId()

  updateProductStatus({ prefix, warehouseId, skuId, unlimited: false })

  configureBroadcasterAdapter(prefix, workspace)

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
