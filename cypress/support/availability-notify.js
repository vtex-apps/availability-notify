import {
  listRequests,
  validateListRequestResponse,
} from './availability-notify.graphql'
import { graphql } from './common/graphql_utils'
import { updateRetry } from './common/support'
import { getEmailContent } from './extract'
import availabilityNotifySelectors from './selectors'
import { MESSAGES } from './utils'
import { AVAILABILITY_NOTIFY_APP } from './graphql_apps'

export function verifyEmail(prefix) {
  it(`${prefix} - Verifying email`, updateRetry(5), () => {
    cy.addDelayBetweenRetries(30000)
    cy.getEmailItems().then((e) => {
      graphql(AVAILABILITY_NOTIFY_APP, listRequests(), (response) => {
        validateListRequestResponse(response)
        const list = response.body.data.listRequests
        const request = list.filter((req) => req.email === e.email)
        const notification = request[0].notificationSent !== 'false'

        expect(notification).to.be.true
      })
      cy.getGmailItems().then(async (gmail) => {
        const gmailCreds = {
          clientId: gmail.clientId,
          clientSecret: gmail.clientSecret,
          refreshToken: gmail.refreshToken,
        }

        const after = new Date().toISOString().slice(0, 10)
        const before = new Date(new Date().getTime() + 60 * 60 * 24 * 1000)
          .toISOString()
          .slice(0, 10)

        const emailContent = await getEmailContent({
          email: e.email,
          gmailCreds,
          after,
          before,
        })

        expect(emailContent).to.not.equal('Email not received')
      })
    })
  })
}

export function subscribeToProductAlerts(data) {
  it(`${data.prefix} - Open product`, updateRetry(3), () => {
    cy.openStoreFront()
    cy.openProduct(data.product, true)
  })

  it(
    `${data.prefix} - Verify product should not available and subscribe to product alerts`,
    updateRetry(2),
    () => {
      cy.subscribeToProduct({ email: data.email, name: data.name })
      cy.get(availabilityNotifySelectors.AvailabilityNotifyAlert).should(
        'have.text',
        MESSAGES.EmailRegistered
      )
    }
  )
}
