import {
  updateProductStatus,
  configureBroadcasterAdapter,
} from '../support/availability-notify.apis'
import { preserveCookie, loginViaAPI } from '../support/common/support'
import { testCase1 } from '../support/outputvalidation'
import { triggerBroadCaster } from '../support/broadcaster.api'
import { verifyEmail } from '../support/availability-notify'

const { skuId, warehouseId } = testCase1
const workspace = Cypress.env().workspace.name
const prefix = 'Update product as available'

describe('Update product as available and validate', () => {
  loginViaAPI()

  afterEach(() => {
    cy.getCookies().then(cookies => {
      const userAuthCookie = cookies.filter(
        c => c.name === 'VtexIdclientAutCookie_productusqa'
      )

      cy.setOrderItem('userAuthCookieValue', userAuthCookie[0].value)
    })
  })

  configureBroadcasterAdapter(prefix, workspace)

  updateProductStatus({ prefix, warehouseId, skuId, unlimited: true })

  triggerBroadCaster(prefix, skuId)

  verifyEmail(prefix)

  preserveCookie()
})
