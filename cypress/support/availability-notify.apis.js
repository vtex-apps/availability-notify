import { updateRetry } from './common/support'
import { FAIL_ON_STATUS_CODE } from './common/constants'

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
