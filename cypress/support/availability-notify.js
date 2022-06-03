import { updateRetry } from './common/support'
import { getEmailContent } from './extract'

export function verifyEmail() {
  it(`Verifying email`, updateRetry(5), () => {
    cy.wait(15000)
    cy.getGmailItems().then(async (gmail) => {
      const gmailCreds = {
        clientId: gmail.clientId,
        clientSecret: gmail.clientSecret,
        refreshToken: gmail.refreshToken,
      }

      const accessToken = await getEmailContent('shashi@bitcot.com', gmailCreds)
      cy.log(accessToken)
    })
  })
}
