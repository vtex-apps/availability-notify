import availabilityNotifySelectors from './selectors'
import selectors from './common/selectors'
import { generateAddtoCartCardSelector } from './common/utils'

const availabilityJson = '.availability.json'
const emailsJson = '.emails.json'

Cypress.Commands.add('gotoProductDetailPage', () => {
  cy.get(selectors.ProductAnchorElement)
    .should('have.attr', 'href')
    .then(href => {
      cy.get(generateAddtoCartCardSelector(href)).first().click()
    })
})

Cypress.Commands.add('openStoreFront', (login = false) => {
  cy.qe(`Adding intercept for events API before visiting the hompage`)
  cy.intercept('**/rc.vtex.com.br/api/events').as('events')
  cy.visit('/')
  if (login === true) {
    cy.qe(
      'Verifying Profile label should be visible in the homepage and it should contain Hello'
    )
    cy.get(selectors.ProfileLabel, { timeout: 20000 })
      .should('be.visible')
      .should('have.contain', `Hello,`)
  }

  cy.qe(`Waiting for the events API to be complted`)
  cy.wait('@events')
})

Cypress.Commands.add('openProduct', (product, detailPage = false) => {
  // Search product in search bar
  cy.qe(
    `Verifying that search bar shouldn't be disabled and it should be visible`
  )
  cy.get(selectors.Search).should('be.not.disabled').should('be.visible')
  cy.qe(`Searching the ${product} in search bar`)

  cy.get(selectors.Search)
    .should('be.visible')
    .clear()
    .type(product)
    .type('{enter}')
  // Page should load successfully now Filter should be visible
  cy.qe(`Search result should have the text ${product.toLowerCase()}`)
  cy.get(selectors.searchResult).should('have.text', product.toLowerCase())
  cy.qe(`Verifying that FilterHeading should be visible before the timeout`)
  cy.get(selectors.FilterHeading, { timeout: 30000 }).should('be.visible')

  if (detailPage) {
    cy.gotoProductDetailPage()
  } else {
    cy.log('Visiting detail page is disabled')
  }
})

Cypress.Commands.add('setavailabilitySubscribeId', (item, itemValue) => {
  cy.readFile(availabilityJson).then(items => {
    items[item] = itemValue
    cy.writeFile(availabilityJson, items)
  })
})

Cypress.Commands.add('saveEmailId', email => {
  cy.writeFile(emailsJson, { email })
})

Cypress.Commands.add('getGmailItems', () => {
  return cy.wrap(Cypress.env().base.gmail, { log: false })
})

Cypress.Commands.add('getEmailItems', () => {
  cy.readFile(emailsJson).then(email => {
    return email
  })
})

Cypress.Commands.add('subscribeToProduct', data => {
  cy.qe('Saving email')
  cy.saveEmailId(data.email)
  cy.qe('Entering the name')
  cy.get(availabilityNotifySelectors.name).type(data.name)
  cy.qe('Entering the email')
  cy.get(availabilityNotifySelectors.email).type(data.email)
  cy.qe(
    `Verifying submit button should not be disabled and clicking on the submit button`
  )
  cy.get(availabilityNotifySelectors.AvailabilityNotifySubmitButton)
    .should('not.be.disabled')
    .click()
})

Cypress.Commands.add('getRequests', () => {
  cy.readFile(availabilityJson).then(items => {
    return items
  })
})
