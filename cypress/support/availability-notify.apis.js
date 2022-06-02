import { updateProductStatusAPI } from './product.api'
import { VTEX_AUTH_HEADER, FAIL_ON_STATUS_CODE } from './common/constants'
import { updateRetry } from './common/support'

export function processUnsentRequest() {
  it('verify the unsend request', updateRetry(3), () => {
    cy.addDelayBetweenRetries(2000)
    cy.request({
      method: 'GET',
      url: 'https://productusqa.myvtex.com/availability-notify/process-unsent-requests',
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
      url: 'https://productusqa.myvtex.com/availability-notify/process-all-requests',
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
