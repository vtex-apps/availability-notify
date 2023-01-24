import { updateProductStatus } from '../support/availability-notify.apis'
import { preserveCookie, loginViaCookies } from '../support/common/support'
import { testCase1 } from '../support/outputvalidation'
import { verifyEmail } from '../support/availability-notify'

const { skuId, warehouseId } = testCase1
const prefix = 'Update product as available'

describe('Update product as available and validate', () => {
  loginViaCookies()

  updateProductStatus({ prefix, warehouseId, skuId, unlimited: true })

  verifyEmail(prefix)

  preserveCookie()
})
