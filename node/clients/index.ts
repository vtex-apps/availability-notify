import { IOClients } from '@vtex/api'

import { MasterDataClient } from './masterdata'
import { LogisticsClient } from './logistics'
import { CatalogClient } from './catalog'
import { CheckoutClient } from './checkout'
import { MailClient } from './mail'
import { TemplateClient } from './template'
import { GitHubClient } from './github'
import { AppsClient } from './apps'
import { VtexIdClient } from './vtexid'
import { AvailabilityClient } from './availability'

export class Clients extends IOClients {
  public get mdClient() {
    return this.getOrSet('mdClient', MasterDataClient)
  }

  public get logistics() {
    return this.getOrSet('logistics', LogisticsClient)
  }

  public get catalog() {
    return this.getOrSet('catalog', CatalogClient)
  }

  public get checkout() {
    return this.getOrSet('checkout', CheckoutClient)
  }

  public get mail() {
    return this.getOrSet('mail', MailClient)
  }

  public get template() {
    return this.getOrSet('template', TemplateClient)
  }

  public get github() {
    return this.getOrSet('github', GitHubClient)
  }

  public get appsClient() {
    return this.getOrSet('appsClient', AppsClient)
  }

  public get vtexId() {
    return this.getOrSet('vtexId', VtexIdClient)
  }

  public get availability() {
    return this.getOrSet('availability', AvailabilityClient)
  }
}
