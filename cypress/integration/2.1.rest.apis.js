import {
  processAllRequest,
  processUnsentRequest,
} from '../support/availability-notify.apis'
import { testSetup } from '../support/common/support'

describe('Rest api', () => {
  testSetup()

  processUnsentRequest()
  processAllRequest()
})
