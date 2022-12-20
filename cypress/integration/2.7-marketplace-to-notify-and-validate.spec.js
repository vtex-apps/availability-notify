import {
  loginViaCookies,
  updateRetry,
  preserveCookie,
} from '../support/common/support'
import { testCase1 } from '../support/outputvalidation'
import { triggerBroadCaster } from '../support/broadcaster.api'
import { verifyEmail } from '../support/availability-notify'
import {
  updateProductStatus,
  configureTargetWorkspace,
  configureBroadcasterAdapter,
} from '../support/availability-notify.apis'
import availabilityNotifySelectors from '../support/selectors'
import { MESSAGES } from '../support/utils'

const { data1, name, email } = testCase1
const workspace = Cypress.env().workspace.name
const prefix = 'Marketplace to notify'

describe('Testing market place to notify', () => {
  loginViaCookies()

  configureBroadcasterAdapter(prefix, workspace)
  configureTargetWorkspace(prefix, true)

  updateProductStatus(prefix, data1, false)

  it(`${prefix} - Open product`, updateRetry(3), () => {
    cy.openStoreFront()
    cy.openProduct('weber spirit', true)
  })

  it(
    `${prefix} - Enable marketplace to notify and validate`,
    updateRetry(3),
    () => {
      cy.subscribeToProduct({ email, name })
      cy.get(availabilityNotifySelectors.AvailabilityNotifyAlert).should(
        'have.text',
        MESSAGES.EmailRegistered
      )
    }
  )

  configureTargetWorkspace(prefix, false)
  updateProductStatus(prefix, data1, true)

  triggerBroadCaster(prefix, data1.skuId)

  verifyEmail(prefix)

  preserveCookie()
})
