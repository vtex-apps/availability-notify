import { VBase } from '@vtex/api'

import { BUCKET, LOCK, UNSENT_CHECK } from '../utils/constants'
import type { Lock } from '../typings/lock'

const APP_NAME = 'vtex.availability-notify'

export class AvailabilityVBase {
  private vbase: VBase

  constructor(vbase: VBase) {
    this.vbase = vbase
  }

  public async setImportLock(sku: string): Promise<void> {
    const lock: Lock = {
      processing_started: new Date().toISOString(),
    }

    try {
      await this.vbase.saveJSON<Lock>(BUCKET, `${LOCK}-${sku}`, lock)
    } catch (error) {
      // Silently fail on lock setting
    }
  }

  public async checkImportLock(sku: string): Promise<Date> {
    try {
      const lock = await this.vbase.getJSON<Lock>(
        BUCKET,
        `${LOCK}-${sku}`,
        true
      )

      if (lock?.processing_started) {
        return new Date(lock.processing_started)
      }
    } catch (error) {
      // Return default date if lock not found
    }

    return new Date(0)
  }

  public async clearImportLock(sku: string): Promise<void> {
    try {
      await this.vbase.deleteFile(BUCKET, `${LOCK}-${sku}`)
    } catch (error) {
      // Silently fail on lock clearing
    }
  }

  public async getLastUnsentCheck(): Promise<Date> {
    try {
      const lastCheck = await this.vbase.getJSON<string>(
        BUCKET,
        UNSENT_CHECK,
        true
      )

      if (lastCheck) {
        return new Date(lastCheck)
      }

      // If file exists but is empty, set and return now
      const now = new Date()
      await this.setLastUnsentCheck(now)

      return now
    } catch (error) {
      // If file doesn't exist, set 7 days ago and return
      const sevenDaysAgo = new Date()
      sevenDaysAgo.setDate(sevenDaysAgo.getDate() - 7)
      await this.setLastUnsentCheck(sevenDaysAgo)

      return sevenDaysAgo
    }
  }

  public async setLastUnsentCheck(lastCheck: Date): Promise<void> {
    try {
      await this.vbase.saveJSON(
        BUCKET,
        UNSENT_CHECK,
        lastCheck.toISOString()
      )
    } catch (error) {
      // Silently fail
    }
  }
}

export { APP_NAME }
