import type {
  ClientsConfig,
  ServiceContext,
  RecorderState,
  ParamsContext,
  EventContext,
} from '@vtex/api'
import { Service, method } from '@vtex/api'

import { Clients } from './clients'
import { processNotification } from './handlers/processNotification'
import { initialize } from './handlers/initialize'
import { prcocessAllRequests } from './handlers/processAllRequests'
import { processUnsentRequests } from './handlers/processUnsentRequests'
import { listNotifyRequests } from './handlers/listNotifyRequests'
import { broadcasterNotification } from './handlers/broadcasterNotification'
import { OnAppInstalled } from './handlers/onAppInstalled'
import { allStates } from './handlers/allStates'
import { resolvers } from './resolvers'

const TIMEOUT_MS = 10000

const clients: ClientsConfig<Clients> = {
  implementation: Clients,
  options: {
    default: {
      retries: 2,
      timeout: TIMEOUT_MS,
    },
  },
}

declare global {
  type Context = ServiceContext<Clients, RecorderState>
}

export default new Service<Clients, RecorderState, ParamsContext>({
  clients,
  routes: {
    processNotification: method({
      POST: [processNotification],
    }),
    initialize: method({
      GET: [initialize],
    }),
    prcocessAllRequests: method({
      GET: [prcocessAllRequests],
      POST: [prcocessAllRequests],
    }),
    processUnsentRequests: method({
      GET: [processUnsentRequests],
      POST: [processUnsentRequests],
    }),
    listNotifyRequests: method({
      GET: [listNotifyRequests],
    }),
  },
  events: {
    broadcasterNotification,
    OnAppInstalled,
    allStates,
  },
  graphql: {
    resolvers,
  },
})
