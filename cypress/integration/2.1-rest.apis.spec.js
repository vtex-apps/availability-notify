import {
  notifySearch,
  updateProductStatus,
} from '../support/availability-notify.apis'
import { testSetup } from '../support/common/support'
import { testCase1 } from '../support/availability-notify.outputvalidation'

const { data1 } = testCase1

describe('Rest api', () => {
  testSetup()

  // processUnsentRequest()
  // processAllRequest()
  updateProductStatus(data1, true)
  notifySearch()
})
