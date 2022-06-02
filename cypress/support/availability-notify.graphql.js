import { FAIL_ON_STATUS_CODE } from './common/constants'

const path = require('path')

const config = Cypress.env()

// Constants
const { vtex } = config.base

export function graphql(getQuery, validateResponseFn = null) {
  const { query, queryVariables } = getQuery

  // Define constants
  const manifestFile = path.join('..', 'manifest.json')
  const APP_VERSION = manifestFile.version
  const APP_NAME = 'vtex.wish-list'
  const APP = `${APP_NAME}@${APP_VERSION}`
  const CUSTOM_URL = `${vtex.baseUrl}/_v/private/admin-graphql-ide/v0/${APP}`

  cy.request({
    method: 'POST',
    url: CUSTOM_URL,
    ...FAIL_ON_STATUS_CODE,
    body: {
      query,
      variables: queryVariables,
    },
  }).as('RESPONSE')

  if (validateResponseFn) {
    cy.get('@RESPONSE').then((response) => {
      expect(response.status).to.equal(200)
      expect(response.body.data).to.not.equal(null)
      validateResponseFn(response)
    })
  } else {
    return cy.get('@RESPONSE')
  }
}

export function version() {
  return {
    query: 'query' + '{version}',
    queryVariables: {},
  }
}

export function deleteRequest(deleteId) {
  const query = 'mutation' + '($id: String)' + '{deleteRequest(id: $id)}'

  return {
    query,
    queryVariables: {
      id: deleteId,
    },
  }
}

export function listRequests() {
  return {
    query:
      'query' +
      // '($name: String, $email: String, $skuId: String,$id: String)' +
      '{listRequests{id,name,email,skuId}}',
    queryVariables: {},
  }
}

export function availabilitySubscribe(availabilityDatas) {
  const query =
    'mutation' +
    '($name: String, $email: String, $skuId: String, $sellerObj: SellerObjInputType!)' +
    '{availabilitySubscribe(name: $name, email: $email, skuId: $skuId, sellerObj: $sellerObj)}'

  return {
    query,
    queryVariables: availabilityDatas,
  }
}

export function validateGetVersionResponse(response) {
  expect(response.body.data).to.not.equal(null)
}

export function validateDeleteRequestResponse(response) {
  expect(response.body.data).to.not.equal(null)
}

export function validateAvailabilitySubscribeRequestResponse(response) {
  expect(response.body.data).to.not.equal(null)
}

export function validateListRequestResponse(response) {
  expect(response.body.data).to.not.equal(null)
}