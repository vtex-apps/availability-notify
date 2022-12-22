import { loginViaCookies } from '../support/common/support.js'
import {
  deleteRequest,
  listRequests,
  validateDeleteRequestResponse,
  validateListRequestResponse,
} from '../support/availability-notify.graphql.js'
import { graphql } from '../support/common/graphql_utils.js'
import { AVAILABILITY_NOTIFY_APP } from '../support/graphql_apps'

describe('Wipe the registered emails', () => {
  loginViaCookies()

  it('Delete registered email requests', () => {
    graphql(AVAILABILITY_NOTIFY_APP, listRequests(), response => {
      validateListRequestResponse(response)
      // eslint-disable-next-line array-callback-return
      response.body.data.listRequests.map(res => {
        graphql(deleteRequest(res.id), validateDeleteRequestResponse)
      })
    })
  })
})
