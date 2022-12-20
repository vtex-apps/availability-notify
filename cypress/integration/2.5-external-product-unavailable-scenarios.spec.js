import { loginViaCookies } from '../support/common/support'
import { testCase1 } from '../support/outputvalidation'
import {
  generateEmailId,
  updateProductStatus,
} from '../support/availability-notify.apis'
import { updateProductAsUnavailable } from '../support/availability-notify'

const { data1, name } = testCase1
const product = 'weber spirit'
const prefix = 'Update product as unavailable'

describe('Test external product unavailable scenarios', () => {
  loginViaCookies()

  const email = generateEmailId()

  updateProductStatus(prefix, data1, false)

  updateProductAsUnavailable({ prefix, product, email, name })
})
