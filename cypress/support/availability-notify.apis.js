/* eslint-disable */
import { updateProductStatusAPI, getProcessAllRequestAPI } from './product.api'
import {
  VTEX_AUTH_HEADER,
  FAIL_ON_STATUS_CODE,
  FAIL_ON_STATUS_CODE_STRING,
} from './common/constants'
import { updateRetry } from './common/support'

const config = Cypress.env()

// Constants
const { name } = config.workspace

const app = 'vtex.availability-notify'
const version = '1.x'

export function processAllRequest() {
  it('Process all the requests', updateRetry(3), () => {
    cy.addDelayBetweenRetries(2000)
    cy.getVtexItems(vtex => {
      cy.request({
        method: 'GET',
        url: `${getProcessAllRequestAPI()}/${
          vtex.account
        }/${name}/_v/availability-notify/process-all-requests`,
        ...FAIL_ON_STATUS_CODE,
      }).then(response => {
        expect(response.status).to.equal(200)
      })
    })
  })
}

export function updateProductStatus({
  prefix,
  warehouseId,
  skuId,
  unlimited = false,
}) {
  it(`${prefix} - Update the product status`, updateRetry(3), () => {
    cy.qe(`Updating the product status`)
    cy.addDelayBetweenRetries(2000)
    cy.getVtexItems().then(vtex => {
      cy.addLogsForRestAPI({
        method: 'PUT',
        url: updateProductStatusAPI(warehouseId, skuId),
        headers: VTEX_AUTH_HEADER(vtex.apiKey, vtex.apiToken),
        body: { unlimitedQuantity: unlimited, quantity: 0 },
      }).then(response => {
        cy.qe('Verifying response.body to be true')
        expect(response.body).to.be.true
      })
    })
  })
}

export function notifySearch(prefix) {
  it(`${prefix} - Notify search`, updateRetry(3), () => {
    cy.addDelayBetweenRetries(2000)

    cy.getVtexItems().then(vtex => {
      cy.getAPI(
        `https://${vtex.account}.myvtex.com/api/dataentities/notify/search?_schema=reviewsSchema&_fields=email,skuId,name,createdAt`,
        VTEX_AUTH_HEADER(vtex.apiKey, vtex.apiToken)
      ).then(response => {
        cy.qe(`Verifying the status to be ${response.status} `)
        expect(response.status).to.equal(200)
      })
    })
  })
}

export function updateAppSettings(prefix, doShippingSim = false) {
  it(`${prefix} - Configuring app settings`, updateRetry(2), () => {
    cy.getVtexItems().then(vtex => {
      // Define constants
      const APP_NAME = 'vtex.apps-graphql'
      const APP_VERSION = '3.x'
      const APP = `${APP_NAME}@${APP_VERSION}`
      const CUSTOM_URL = `${vtex.baseUrl}/_v/private/admin-graphql-ide/v0/${APP}`

      cy.qe('Configuting app settings')

      const GRAPHQL_MUTATION =
        'mutation' +
        '($app:String,$version:String,$settings:String)' +
        '{saveAppSettings(app:$app,version:$version,settings:$settings){message}}'

      const QUERY_VARIABLES = {
        app,
        version,
        settings: `{\"doShippingSim\":${doShippingSim},\"notifyMarketplace\":\"productusqaseller\"}`,
      }
      cy.addGraphqlLogs({ query: GRAPHQL_MUTATION, QUERY_VARIABLES })

      // Mutating it to the new workspace

      cy.addLogsForRestAPI({
        method: 'POST',
        url: CUSTOM_URL,
        body: {
          query: GRAPHQL_MUTATION,
          variables: QUERY_VARIABLES,
        },
      })
      .its('body.data.saveAppSettings.message', { timeout: 10000 })
    })
  })
}

export function configureBroadcasterAdapter(prefix, workspace = 'master') {
  const BROADCASTER_APP = 'vtex.broadcaster'
  it(
    `${prefix} - Register target workspace as ${workspace} in ${BROADCASTER_APP}`,
    updateRetry(2),
    () => {
      cy.qe(`Register target workspace as ${workspace} in ${BROADCASTER_APP}`)
      cy.getVtexItems().then(vtex => {
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
          app: BROADCASTER_APP,
          version: '0.x',
          settings: `{"targetWorkspace":"${workspace}"}`,
        }
        cy.addGraphqlLogs({ query: GRAPHQL_MUTATION, QUERY_VARIABLES })

        cy.addLogsForRestAPI({
          method: 'POST',
          url: CUSTOM_URL,
          body: {
            query: GRAPHQL_MUTATION,
            variables: QUERY_VARIABLES,
          },
        })
          .its('body.data.saveAppSettings.message', { timeout: 10000 })
          .should('contain', workspace)
      })
    }
  )
}

export function generateEmailId() {
  return `shashi+${Date.now().toString().substring(7)}@bitcot.com`
}
