import { updateProductStatusAPI } from './product.api'
import { VTEX_AUTH_HEADER, FAIL_ON_STATUS_CODE } from './common/constants'
import { updateRetry } from './common/support'
const config = Cypress.env()

// Constants
const { account } = config.base.vtex

export function processUnsentRequest() {
  it('verify the unsend request', updateRetry(3), () => {
    cy.addDelayBetweenRetries(2000)
    cy.request({
      method: 'GET',
      url: 'https://sandboxusdev.myvtex.com/availability-notify/process-unsent-requests',
      headers: {
        VtexIdclientAutCookie:
          'eyJhbGciOiJFUzI1NiIsImtpZCI6IjNCQ0ZFRjdDQjlFMTQ5Qzc1NjRCNDkwNDJDMzlBNjc4QjYzMEI2OUMiLCJ0eXAiOiJqd3QifQ.eyJzdWIiOiJzaGFzaGlkaGFyLnJlZGR5QHZ0ZXguY29tLmJyIiwiYWNjb3VudCI6InNhbmRib3h1c2RldiIsImF1ZGllbmNlIjoiYWRtaW4iLCJzZXNzIjoiMzEzNGRhMzgtYTkxMC00MGIwLTg3MjEtODRjMGEzYTU5OGUzIiwiZXhwIjoxNjU0MjQ4MzE5LCJ1c2VySWQiOiI4ZGFjYThmYi1mMmYwLTQyYTItODJkMC01N2IyZDQwYTllMTQiLCJpYXQiOjE2NTQxNjE5MTksImlzcyI6InRva2VuLWVtaXR0ZXIiLCJqdGkiOiIzYTdhZmJmNy03ZmU3LTQwYTItYTQ0Zi0wZTYwODk3NGQyMTcifQ.26PsE-wtJDDSopbzzS7nHHwVHu27eRe24SfyU56gontvycYrWVXm86irnRL1qpU0POx4gP2kc0F1BovIkwkqEA',
      },
      ...FAIL_ON_STATUS_CODE,
    }).then((response) => {
      cy.log(response)
      expect(response.status).to.equal(200)
    })
  })
}

export function processAllRequest() {
  it('process all the requests', updateRetry(3), () => {
    cy.addDelayBetweenRetries(2000)
    cy.request({
      method: 'GET',
      url: 'https://sandboxusdev.myvtex.com/availability-notify/process-all-requests',
      ...FAIL_ON_STATUS_CODE,
    }).then((response) => {
      cy.log(response)
      expect(response.status).to.equal(200)
    })
  })
}

export function updateProductStatus(data1, unlimited = false) {
  it('update the product status', updateRetry(3), () => {
    cy.addDelayBetweenRetries(2000)
    cy.getVtexItems().then((vtex) => {
      cy.request({
        method: 'PUT',
        url: updateProductStatusAPI(data1),
        headers: VTEX_AUTH_HEADER(vtex.apiKey, vtex.apiToken),
        ...FAIL_ON_STATUS_CODE,
        body: { unlimitedQuantity: unlimited, quantity: 0 },
      }).then((response) => {
        expect(response.body).to.be.true
      })
    })
  })
}

export function notifySearch() {
  it('Notify search', updateRetry(3), () => {
    cy.addDelayBetweenRetries(2000)

    cy.getVtexItems().then((vtex) => {
      cy.request({
        method: 'GET',
        url: 'https://sandboxusdev.myvtex.com/api/dataentities/notify/search?_schema=reviewsSchema&_fields=email,skuId,name,createdAt',
        headers: VTEX_AUTH_HEADER(vtex.apiKey, vtex.apiToken),
        ...FAIL_ON_STATUS_CODE,
      }).then((response) => {
        expect(response.status).to.equal(200)
      })
    })
  })
}

export function configureTargetWorkspace(app, version, doShippingSim = false) {
  it(
    `Configuring target workspace in ${app}`,
    updateRetry(2),
    () => {
      cy.getVtexItems().then((vtex) => {
        // Define constants
        const APP_NAME = 'vtex.apps-graphql'
        const APP_VERSION = '3.x'
        const APP = `${APP_NAME}@${APP_VERSION}`
        const CUSTOM_URL = `https://${account}.myvtex.com/_v/private/admin-graphql-ide/v0/${APP}`

        const GRAPHQL_MUTATION =
          'mutation' +
          '($app:String,$version:String,$settings:String)' +
          '{saveAppSettings(app:$app,version:$version,settings:$settings){message}}'

        const QUERY_VARIABLES = {
          app,
          version,
          settings: `{\"doShippingSim\":${doShippingSim},\"notifyMarketplace\":\"sandboxusdevseller\"}`,
        }
        // Mutating it to the new workspace
        cy.request({
          method: 'POST',
          url: CUSTOM_URL,
          ...FAIL_ON_STATUS_CODE,
          body: {
            query: GRAPHQL_MUTATION,
            variables: QUERY_VARIABLES,
          },
        })
          .its('body.data.saveAppSettings.message', { timeout: 10000 })
      })
    }
  )
}