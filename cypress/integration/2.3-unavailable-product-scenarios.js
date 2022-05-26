import availabilityNotifySelectors from '../support/availability-notify.selectors'
import { testSetup, updateRetry } from '../support/common/support'
import { testCase1 } from '../support/availability-notify.outputvalidation'
import availabilityNotifyConstants from '../support/availability-notify.constants'

describe('Testing', () => {
  // Load test setup
  testSetup(false)

  const { name, email } = testCase1

  it('Open product', updateRetry(3), () => {
    cy.openStoreFront()
    cy.openProduct('weber spirit', true)
  })

  it(
    'verify product should not available and subscribe to product alerts',
    updateRetry(3),
    () => {
      // cy.get('.vtex-add-to-cart-button-0-x-buttonText').should('be.visible')
      cy.get(availabilityNotifySelectors.InputName).type(name)
      cy.get(availabilityNotifySelectors.InputEmail).type(email)
      cy.get(availabilityNotifySelectors.AvailabilityNotifySubmitButton)
        .should('not.be.disabled')
        .click()
      // operationName: "AvailabilitySubscribe"
      cy.get(availabilityNotifySelectors.AvailabilityNotifyAlert).should(
        'have.text',
        availabilityNotifyConstants.EmailRegistered
      )
    }
  )

  it(
    'Verify with same email address and check if we are getting error',
    updateRetry(3),
    () => {
      // cy.get('.vtex-add-to-cart-button-0-x-buttonText').should('be.visible')
      cy.get(availabilityNotifySelectors.InputName).clear().type(name)
      cy.get(availabilityNotifySelectors.InputEmail).clear().type(email)
      cy.get(availabilityNotifySelectors.AvailabilityNotifySubmitButton)
        .should('not.be.disabled')
        .click()
      // operationName: "AvailabilitySubscribe"
      cy.get(availabilityNotifySelectors.AvailabilityNotifyAlert).should(
        'have.text',
        availabilityNotifyConstants.EmailAlreadyExist
      )
    }
  )
})
