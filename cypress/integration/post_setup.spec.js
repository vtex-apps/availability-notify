import { loginViaCookies } from '../support/common/support'
import { configureBroadcasterAdapter } from '../support/availability-notify.apis'

const workspace = Cypress.env().workspace.name

const prefix = 'Post setup scenarios'

describe(prefix, () => {
  loginViaCookies()

  configureBroadcasterAdapter(prefix, workspace)
})
