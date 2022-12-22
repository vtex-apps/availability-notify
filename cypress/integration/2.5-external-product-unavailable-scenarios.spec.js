import { loginViaCookies } from '../support/common/support'
import { testCase1 } from '../support/outputvalidation'
import {
  generateEmailId,
  updateProductStatus,
} from '../support/availability-notify.apis'
import { subscribeToProductAlerts } from '../support/availability-notify'

const { data1, name, product } = testCase1
const prefix = 'Update product as unavailable'

describe('Test external product unavailable scenarios', () => {
  loginViaCookies()

  const email = generateEmailId()

  updateProductStatus(prefix, data1, false)

  subscribeToProductAlerts({ prefix, product, email, name })
})
