import {
  notifySearch,
  processAllRequest,
  updateProductStatus,
} from '../support/availability-notify.apis'
import { loginViaCookies } from '../support/common/support'
import { testCase1 } from '../support/outputvalidation'

const { skuId, warehouseId } = testCase1
const prefix = 'Rest API'

describe('Rest api', () => {
  loginViaCookies()

  processAllRequest()
  updateProductStatus({
    prefix,
    warehouseId,
    skuId,
    unlimited: true,
  })
  notifySearch(prefix)
})
