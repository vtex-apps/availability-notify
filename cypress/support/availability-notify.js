import {
  graphql,
  listRequests,
  validateListRequestResponse,
} from './availability-notify.graphql'
import { updateRetry } from './common/support'
import { getEmailContent } from './extract'

export function verifyEmail(email) {
  it(`Verifying email`, updateRetry(5), () => {
    graphql(listRequests(), (response) => {
      validateListRequestResponse(response)
      const request = response.body.data.listRequests.filter(
        (req) => req.email === email
      )

      expect(request[0].notificationSent).to.be(true)
    })
    /* eslint-disable cypress/no-unnecessary-waiting */
    cy.wait(15000)
    cy.getGmailItems().then(async (gmail) => {
      const gmailCreds = {
        clientId: gmail.clientId,
        clientSecret: gmail.clientSecret,
        refreshToken: gmail.refreshToken,
      }

      const emailContent = await getEmailContent(
        'shashi@bitcot.com',
        gmailCreds
      )

      expect(emailContent).to.equal(1)
    })
  })
}
