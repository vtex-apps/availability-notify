import { updateRetry } from './common/support'
import { updateProductStatusAPI } from './product.api'
import { VTEX_AUTH_HEADER, FAIL_ON_STATUS_CODE } from './common/constants'

export function updateProductStatus(data1, data2) {
    it('update the product status', updateRetry(3), () => {
        cy.addDelayBetweenRetries(2000)
        cy.getVtexItems().then((vtex) => {
            cy.request({
                method: 'PUT',
                url: updateProductStatusAPI(data1),
                headers: VTEX_AUTH_HEADER(vtex.apiKey, vtex.apiToken),
                ...FAIL_ON_STATUS_CODE,
                body: data2

            }).then((response) => {
                expect(response.body).to.be.true
            })
        })

    })
}