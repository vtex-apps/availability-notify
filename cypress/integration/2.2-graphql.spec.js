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
import { testSetup, updateRetry } from '../support/common/support'
import { availabilityDatas } from '../support/availability-notify.outputvalidation'

describe('Graphql queries', () => {
  testSetup(false)

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
