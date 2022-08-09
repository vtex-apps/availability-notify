import {
  notifySearch,
  processAllRequest,
  updateProductStatus,
} from '../support/availability-notify.apis'
import { testSetup } from '../support/common/support'
import { testCase1 } from '../support/availability-notify.outputvalidation'

const { data1 } = testCase1
const prefix = 'Rest API'

describe('Rest api', () => {
  testSetup()

  processAllRequest()
  updateProductStatus(prefix, data1, true)
  notifySearch(prefix)
})
