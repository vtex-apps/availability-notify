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
} from '../support/availability-notify.graphql'
import { testSetup } from '../support/common/support'
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

  it('List Requests', () => {
    graphql(listRequests(), (response) => {
      validateListRequestResponse(response)
      cy.setavailabilitySubscribeId(response.body.data.listRequests)
    })
  })

  it('Delete Request', () => {
    cy.setDeleteId().then((deleteId) => {
      graphql(deleteRequest(deleteId.id), validateDeleteRequestResponse)
    })
  })
})
