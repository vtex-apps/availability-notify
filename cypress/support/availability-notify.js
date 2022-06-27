import {
  graphql,
  listRequests,
  validateListRequestResponse,
} from './availability-notify.graphql'
import { updateRetry } from './common/support'
import { getEmailContent } from './extract'

export function verifyEmail() {
  it(`Verifying email`, updateRetry(5), () => {
    cy.addDelayBetweenRetries(30000)
    cy.getEmailItems().then((e) => {
      graphql(listRequests(), (response) => {
        validateListRequestResponse(response)
        const list = response.body.data.listRequests
        const request = list.filter((req) => req.email === e.email)
        const notification = request[0].notificationSent

        expect(notification).to.be(true)
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

        expect(emailContent).to.not.equal('0')
      })
    })
  })
}
