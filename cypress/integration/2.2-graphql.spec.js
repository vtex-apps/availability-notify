import {
  graphql,
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
import { availabilityDatas } from '../support/outputvalidation'

describe('Graphql queries', () => {
  loginViaCookies()

  it('Get Version', () => {
    graphql(version(), validateGetVersionResponse)
  })

  it('Availability Subscriber Request', () => {
    graphql(
      availabilitySubscribe(availabilityDatas),
      validateAvailabilitySubscribeRequestResponse
    )
  })

  it('List Requests', updateRetry(3), () => {
    graphql(listRequests(), response => {
      validateListRequestResponse(response)
      cy.setavailabilitySubscribeId(response.body.data.listRequests)
    })
  })

  it('Process Unsent Requests', updateRetry(3), () => {
    graphql(processUnsentRequest(), validateProcessUnsentRequestResponse)
  })

  it('Delete Request', () => {
    cy.setDeleteId().then(deleteId => {
      graphql(deleteRequest(deleteId.id), validateDeleteRequestResponse)
    })
  })
})
