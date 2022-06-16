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
    // cy.wait(15000)
    cy.getGmailItems().then(async (gmail) => {
      const gmailCreds = {
        clientId: gmail.clientId,
        clientSecret: gmail.clientSecret,
        refreshToken: gmail.refreshToken,
      }

      const after = new Date().toISOString().slice(0, 10)
      const before = new Date(new Date() - 15000 * 60)
        .toISOString()
        .slice(0, 10)

      const emailContent = await getEmailContent(
        'shashi@bitcot.com',
        gmailCreds,
        after,
        before,
        4
      )
      cy.log(emailContent)
      expect(emailContent).to.not.equal(0)
    })
  })
}
