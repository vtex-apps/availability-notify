export interface AppInstalledEvent {
  To: InstalledApp
}

export interface InstalledApp {
  Id: string
  Registry: string
}
