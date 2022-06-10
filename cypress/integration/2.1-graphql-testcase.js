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
} from '../support/availability.graphql'
import { testSetup } from '../support/common/support'
import { availabilityDatas } from '../support/availability.outputvalidation'

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
      validateListRequestResponse
      cy.setavailabilitySubscribeId(response.body.data.listRequests[0])
    })
  })

  it('Delete Request', () => {
    cy.setDeleteId().then((deleteId) => {
      graphql(deleteRequest(deleteId.id), validateDeleteRequestResponse)
    })
  })
})
