import { testSetup, updateRetry } from '../support/common/support'

describe('Testing', () => {
  // Load test setup
  testSetup(false)

  it('Open product', updateRetry(3), () => {
    cy.openStoreFront()
    cy.openProduct('weber spirit', true)
  })
})
