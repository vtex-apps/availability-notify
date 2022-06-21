import { FAIL_ON_STATUS_CODE } from './common/constants'
import { updateRetry } from './common/support'

export function triggerBroadCaster(skuid) {
  it('Triggering broadcatser api', updateRetry(3), () => {
    cy.addDelayBetweenRetries(2000)
    cy.getVtexItems().then((vtex) => {
      cy.request({
        method: 'POST',
        url: `http://app.io.vtex.com/vtex.broadcaster/v0/${vtex.account}/${
          Cypress.env().workspace.name
        }/notify`,
        headers: {
          VtexIdclientAutCookie: vtex.userAuthCookieValue,
        },
        body: {
          HasStockKeepingUnitModified: true,
          IdSku: skuid,
          StockModified: true,
        },
        ...FAIL_ON_STATUS_CODE,
      }).then((response) => {
        expect(response.status).to.equal(200)
      })
    })
  })
}
