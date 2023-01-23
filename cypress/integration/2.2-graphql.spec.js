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

  it('Process Unsent Requests', updateRetry(3), () => {
    graphql(
      AVAILABILITY_NOTIFY_APP,
      processUnsentRequest(),
      validateProcessUnsentRequestResponse
    )
  })

  it('List Requests', updateRetry(3), () => {
    graphql(AVAILABILITY_NOTIFY_APP, listRequests(), response => {
      validateListRequestResponse(response)
      const subscribedRequest = response.body.data.listRequests.filter(
        r => r.email === testCase1.email
      )

      cy.setavailabilitySubscribeId('subscribed_request', subscribedRequest[0])
    })
  })

  it('Delete Request', () => {
    cy.getRequests().then(request => {
      graphql(
        AVAILABILITY_NOTIFY_APP,
        deleteRequest(request.subscribed_request.id),
        validateDeleteRequestResponse
      )
    })
  })
})
