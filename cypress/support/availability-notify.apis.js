/* eslint-disable */
import { updateProductStatusAPI } from './product.api'
import { VTEX_AUTH_HEADER, FAIL_ON_STATUS_CODE } from './common/constants'
import { updateRetry } from './common/support'

const config = Cypress.env()

// Constants
const { account } = config.base.vtex
const WIPE_ENV = 'wipe'
const { name } = config.workspace

export function processUnsentRequest() {
  it('verify the unsend request', updateRetry(3), () => {
    cy.addDelayBetweenRetries(2000)
    cy.getVtexItems((vtex) => {
      cy.request({
        method: 'GET',
        url: `https://${vtex.account}.myvtex.com/availability-notify/process-unsent-requests`,
        headers: {
          VtexIdclientAutCookie: vtex.userAuthCookieValue,
        },
        ...FAIL_ON_STATUS_CODE,
      }).then((response) => {
        expect(response.status).to.equal(200)
      })
    })
  })
}

export function processAllRequest() {
  it('process all the requests', updateRetry(3), () => {
    cy.addDelayBetweenRetries(2000)
    cy.getVtexItems((vtex) => {
      cy.request({
        method: 'GET',
        url: `https://${vtex.account}.myvtex.com/availability-notify/process-all-requests`,
        ...FAIL_ON_STATUS_CODE,
      }).then((response) => {
        expect(response.status).to.equal(200)
      })
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
        url: `https://${vtex.account}.myvtex.com/api/dataentities/notify/search?_schema=reviewsSchema&_fields=email,skuId,name,createdAt`,
        headers: VTEX_AUTH_HEADER(vtex.apiKey, vtex.apiToken),
        ...FAIL_ON_STATUS_CODE,
      }).then((response) => {
        expect(response.status).to.equal(200)
      })
    })
  })
}

export function configureTargetWorkspace(app, version, doShippingSim = false) {
  it(`Configuring target workspace in ${app}`, updateRetry(2), () => {
    cy.getVtexItems().then((vtex) => {
      // Define constants
      const APP_NAME = 'vtex.apps-graphql'
      const APP_VERSION = '3.x'
      const APP = `${APP_NAME}@${APP_VERSION}`
      const CUSTOM_URL = `${vtex.baseUrl}/_v/private/admin-graphql-ide/v0/${APP}`

      const GRAPHQL_MUTATION =
        'mutation' +
        '($app:String,$version:String,$settings:String)' +
        '{saveAppSettings(app:$app,version:$version,settings:$settings){message}}'

      const QUERY_VARIABLES = {
        app,
        version,
        settings: `{\"doShippingSim\":${doShippingSim},\"notifyMarketplace\":\"productusqaseller\"}`,
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
      }).its('body.data.saveAppSettings.message', { timeout: 10000 })
    })
  })
}

export function configureBroadcasterAdapter(app, version, workspace = name) {
  it(
    `Configuring target workspace as ${workspace} in ${app}`,
    updateRetry(2),
    () => {
      cy.getOrderItems().then((order) => {
        if (order[WIPE_ENV]) {
          // Define constants
          const APP_NAME = 'vtex.apps-graphql'
          const APP_VERSION = '3.x'
          const APP = `${APP_NAME}@${APP_VERSION}`
          const CUSTOM_URL = `${order.baseUrl}.myvtex.com/_v/private/admin-graphql-ide/v0/${APP}`

          const GRAPHQL_MUTATION =
            'mutation' +
            '($app:String,$version:String,$settings:String)' +
            '{saveAppSettings(app:$app,version:$version,settings:$settings){message}}'

          const QUERY_VARIABLES = {
            app,
            version,
            settings: `{"targetWorkspace":"${workspace}"}`,
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
            .should('contain', workspace)
        } else {
          cy.log('Tax configuration is configured with another workspace')
        }
      })
    }
  )
}


export function generateEmailId() {
  return `shashi+${(Math.random() + 1).toString(36).substring(7)}@bitcot.com`
}