import { loginViaCookies } from '../support/common/support.js'
import {
  deleteRequest,
  graphql,
  listRequests,
  validateDeleteRequestResponse,
  validateListRequestResponse,
} from '../support/availability-notify.graphql.js'

describe('Wipe the registered emails', () => {
  loginViaCookies()

  it('Delete registered email requests', () => {
    graphql(listRequests(), response => {
      validateListRequestResponse(response)
      // eslint-disable-next-line array-callback-return
      response.body.data.listRequests.map(res => {
        graphql(deleteRequest(res.id), validateDeleteRequestResponse)
      })
    })
  })
})
