export function version() {
  cy.qe(`Get version via graphql`)
  const query = 'query' + '{version}'

  cy.addGraphqlLogs(query)

  return {
    query: 'query' + '{version}',
    queryVariables: {},
  }
}

export function deleteRequest(deleteId) {
  const query = 'mutation' + '($id: String)' + '{deleteRequest(id: $id)}'

  cy.addGraphqlLogs(query, deleteId)

  return {
    query,
    queryVariables: {
      id: deleteId,
    },
  }
}

export function listRequests() {
  const query =
    'query' +
    '{listRequests{id,name,email,skuId, notificationSent, notificationSentAt}}'

  cy.addGraphqlLogs(query)

  return {
    query:
      'query' +
      // '($name: String, $email: String, $skuId: String,$id: String)' +
      '{listRequests{id,name,email,skuId, notificationSent, notificationSentAt}}',
    queryVariables: {},
  }
}

export function processUnsentRequest() {
  const query = 'mutation' + '{processUnsentRequests{email,skuId,sent}}'

  cy.addGraphqlLogs(query)

  return {
    query: 'mutation' + '{processUnsentRequests{email,skuId,sent}}',
    queryVariables: {},
  }
}

export function availabilitySubscribe(availabilityData) {
  const data = {
    name: availabilityData.name,
    email: availabilityData.email,
    skuId: availabilityData.skuId,
    sellerObj: {
      sellerId: '',
    },
  }

  cy.setavailabilitySubscribeId('subscribed_email', availabilityData.email)
  const query =
    'mutation' +
    '($name: String, $email: String, $skuId: String, $sellerObj: SellerObjInputType!)' +
    '{availabilitySubscribe(name: $name, email: $email, skuId: $skuId, sellerObj: $sellerObj)}'

  cy.addGraphqlLogs(query, data)

  return {
    query,
    queryVariables: data,
  }
}

export function validateGetVersionResponse(response) {
  expect(response.body.data).to.not.equal(null)
}

export function validateDeleteRequestResponse(response) {
  expect(response.body.data).to.not.equal(null)
  expect(response.body.data.deleteRequest).to.not.equal(false)
}

export function validateAvailabilitySubscribeRequestResponse(response) {
  expect(response.body.data).to.not.equal(null)
  expect(response.body.data.availabilitySubscribe).to.not.equal(false)
}

export function validateListRequestResponse(response) {
  expect(response.body.data).to.not.equal(null)
}

export function validateProcessUnsentRequestResponse(response) {
  expect(response.body.data).to.not.equal(null)
  expect(response.body.data.processUnsentRequests).to.be.an('array')
}
