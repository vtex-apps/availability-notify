import availabilityNotifySelectors from '../support/availability-notify.selectors'
import { testSetup, updateRetry, preserveCookie } from '../support/common/support'
import { testCase1 } from '../support/availablity-notify.outputvalidation'

describe('Update product as available and validate', () => {
    testSetup()
    const { name, email } = testCase1
    it(
        'update external seller product as unavailable',
        updateRetry(3),
        () => {
            cy.openStoreFront()
            cy.openProduct('weber spirit', true)
        }
    )

    it(
        'Add email address and on save button click success message should be displayed',
        updateRetry(3),
        () => {
            cy.get('.vtex-add-to-cart-button-0-x-buttonText').should('be.visible')
            cy.wait(5000)
            cy.get(availabilityNotifySelectors.name).clear().type(name)
            cy.get(availabilityNotifySelectors.email).clear().type(email)
            cy.get('.vtex-availability-notify-1-x-submit > .vtex-button').click()
            cy.get(availabilityNotifySelectors.AvailabilityNotifySubmitButton)
                .should('not.be.disabled')
                .click()
        }
    )
    preserveCookie()
})