import {
  validateGetVersionResponse,
  validateDeleteRequestResponse,
  validateListRequestResponse,
  validateAvailabilitySubscribeRequestResponse,
  version,
  deleteRequest,
  listRequests,
  availabilitySubscribe,
  processUnsentRequest,
  validateProcessUnsentRequestResponse,
} from '../support/availability-notify.graphql'
import { loginViaCookies, updateRetry } from '../support/common/support'
import { testCase1 } from '../support/outputvalidation'
import { graphql } from '../support/common/graphql_utils'
import { AVAILABILITY_NOTIFY_APP } from '../support/graphql_apps'

describe('Graphql queries', () => {
  loginViaCookies()

  it('Get Version', () => {
    graphql(version(), validateGetVersionResponse)
  })

  it('Availability Subscriber Request', () => {
    graphql(
      AVAILABILITY_NOTIFY_APP,
      availabilitySubscribe(testCase1),
      validateAvailabilitySubscribeRequestResponse
    )
  })

  it('List Requests', updateRetry(3), () => {
    graphql(AVAILABILITY_NOTIFY_APP, listRequests(), response => {
      validateListRequestResponse(response)
      cy.setavailabilitySubscribeId(response.body.data.listRequests)
    })
  })

  it('Process Unsent Requests', updateRetry(3), () => {
    graphql(
      AVAILABILITY_NOTIFY_APP,
      processUnsentRequest(),
      validateProcessUnsentRequestResponse
    )
  })

  it('Delete Request', () => {
    cy.getRequests().then(request => {
      graphql(
        AVAILABILITY_NOTIFY_APP,
        deleteRequest(request[0].id),
        validateDeleteRequestResponse
      )
    })
  })
})
