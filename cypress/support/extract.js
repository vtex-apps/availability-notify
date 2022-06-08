const GmailAPI = require('./gmail')

const delay = (ms) => new Promise((resolve) => setTimeout(resolve, ms))

function extractContent(message) {
  if (message) {
    return message.includes('The wait is over') ? message : '0'
  }

  return '0'
}

export async function getEmailContent(
  email,
  gmailCreds,
  currentContent = null
) {
  const gmail = new GmailAPI(gmailCreds)
  const ToEmail = email.replace('+', '%2B')

  let content
  const totalRetry = !currentContent ? 0 : 8

  /* eslint-disable no-await-in-loop */
  for (let currentRetry = 0; currentRetry <= totalRetry; currentRetry++) {
    content = extractContent(
      await gmail.readInboxContent(
        new URLSearchParams(
          `from:noreply@vtexcommerce.com.br+to:${ToEmail}`
        ).toString(),
        gmailCreds
      )
    )
    if (currentContent === null) {
      return content
    }

    if (currentContent) {
      return content
    }

    await delay(5000)
  }
}
