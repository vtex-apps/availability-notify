/* eslint-disable no-await-in-loop */
/* eslint-disable no-loop-func */
import type { FC } from 'react'
import React, { useState, useEffect } from 'react'
import { injectIntl, defineMessages } from 'react-intl'
import {
  ToastProvider,
  ToastConsumer,
  Layout,
  PageBlock,
  PageHeader,
  ButtonWithIcon,
  IconDownload,
  Input,
  Toggle,
  Button,
} from 'vtex.styleguide'
import XLSX from 'xlsx'
import { useMutation, useQuery } from 'react-apollo'

import AppSettings from './queries/appSettings.gql'
import SaveAppSettings from './mutations/saveAppSettings.gql'

interface Props {
  intl: any
}

const NotifyAdmin: FC<any> = ({ intl }: Props) => {
  const [state, setState] = useState<any>({
    loading: false,
    processing: false,
  })

  const messages = defineMessages({
    title: {
      id: 'admin/request.menu.label',
      defaultMessage: 'Requests',
    },
    download: {
      id: 'admin/settings.download',
      defaultMessage: 'Download Requests',
    },
    processUnsent: {
      id: 'admin/settings.process-unsent',
      defaultMessage: 'Process Unsent',
    },
    verifyAvailability: {
      id: 'admin/settings.verify-availability',
      defaultMessage: 'Verify Availability',
    },
    marketplaceToNotify: {
      id: 'admin/settings.marketplace-to-notify',
      defaultMessage: 'Marketplace To Notify',
    },
    saveSettingsSuccess: {
      id: 'admin/settings.saveSettings.success',
      defaultMessage: 'Settings Saved',
    },
    saveSettingsFailure: {
      id: 'admin/settings.saveSettings.failure',
      defaultMessage: 'Did not save settings',
    },
    saveSettingsButton: {
      id: 'admin/settings.saveSettings.button',
      defaultMessage: 'Save',
    },
  })

  const { data } = useQuery(AppSettings, {
    variables: {
      version: process.env.VTEX_APP_VERSION,
    },
    ssr: false,
  })

  const [saveSettings] = useMutation(SaveAppSettings)
  const [settingsLoading, setSettingsLoading] = useState(false)

  const [settingsState, setSettingsState] = useState({
    doShippingSim: false,
    marketplaceToNotify: '',
  })

  const handleSaveSettings = async (showToast: any) => {
    setSettingsLoading(true)

    try {
      await saveSettings({
        variables: {
          version: process.env.VTEX_APP_VERSION,
          settings: JSON.stringify(settingsState),
        },
      }).then(() => {
        showToast({
          message: intl.formatMessage(messages.saveSettingsSuccess),
          duration: 5000,
        })
        setSettingsLoading(false)
      })
    } catch (error) {
      console.error(error)
      showToast({
        message: intl.formatMessage(messages.saveSettingsFailure),
        duration: 5000,
      })
      setSettingsLoading(false)
    }
  }

  useEffect(() => {
    if (!data?.appSettings?.message) return

    const parsedSettings: any = JSON.parse(data.appSettings.message)

    setSettingsState(parsedSettings)
  }, [data])

  const { loading } = state
  const { processing } = state

  const downloadRequests = (allRequests: any) => {
    const header = [
      'Id',
      'Name',
      'Email',
      'Sku Id',
      'Requested At',
      'Sent',
      'Sent At',
    ]

    const result: any = []

    for (const request of allRequests) {
      const requestData = {
        Id: request.id,
        Name: request.name,
        Email: request.email,
        'Sku Id': request.skuId,
        'Requested At': request.requestedAt,
        Sent: request.notificationSent,
        'Sent At': request.notificationSentAt,
      }

      result.push(requestData)
    }

    const ws = XLSX.utils.json_to_sheet(result, { header })
    const wb = XLSX.utils.book_new()

    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1')
    const exportFileName = `requests.xls`

    XLSX.writeFile(wb, exportFileName)
  }

  const processRequestsResults = (processedRequests: any) => {
    const header = ['Sku Id', 'Quantity Available', 'Email', 'Sent', 'Updated']

    const result: any = []

    for (const request of processedRequests) {
      const requestData = {
        'Sku Id': request.skuId,
        'Quantity Available': request.quantityAvailable,
        Email: request.email,
        Sent: request.notificationSent,
        Updated: request.updated,
      }

      result.push(requestData)
    }

    const ws = XLSX.utils.json_to_sheet(result, { header })
    const wb = XLSX.utils.book_new()

    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1')
    const exportFileName = `processedRequests.xls`

    XLSX.writeFile(wb, exportFileName)
  }

  const getAllRequests = async () => {
    setState({ ...state, loading: true })

    const result: any = await fetch(
      `/_v/availability-notify/list-requests`
    ).then(response => response.json())

    const requestArr = result

    downloadRequests(requestArr)
    setState({ ...state, loading: false })
  }

  const processUnsentRequests = async () => {
    setState({ ...state, processing: true })

    const result: any = await fetch(
      `/_v/availability-notify/process-unsent-requests`
    ).then(response => response.json())

    const requestArr = result

    processRequestsResults(requestArr)
    setState({ ...state, processing: false })
  }

  const download = <IconDownload />

  return (
    <ToastProvider positioning="window">
      <ToastConsumer>
        {({ showToast }: { showToast: any }) => (
          <Layout
            pageHeader={
              <PageHeader title={intl.formatMessage(messages.title)} />
            }
          >
            <PageBlock variation="full">
              <div>
                <Toggle
                  label={intl.formatMessage(messages.verifyAvailability)}
                  size="large"
                  checked={settingsState.doShippingSim}
                  onChange={() => {
                    setSettingsState({
                      ...settingsState,
                      doShippingSim: !settingsState.doShippingSim,
                    })
                  }}
                />
              </div>

              <div>
                <Input
                  label={intl.formatMessage(messages.marketplaceToNotify)}
                  onChange={(e: React.FormEvent<HTMLInputElement>) =>
                    setSettingsState({
                      ...settingsState,
                      marketplaceToNotify: e.currentTarget.value,
                    })
                  }
                  value={settingsState.marketplaceToNotify}
                />
              </div>
              <section className="pt4">
                <Button
                  variation="primary"
                  onClick={() => handleSaveSettings(showToast)}
                  isLoading={settingsLoading}
                >
                  {intl.formatMessage(messages.saveSettingsButton)}
                </Button>
              </section>
            </PageBlock>
            <PageBlock variation="half">
              <div>
                <ButtonWithIcon
                  icon={download}
                  isLoading={loading}
                  onClick={() => {
                    getAllRequests()
                  }}
                >
                  {intl.formatMessage(messages.download)}
                </ButtonWithIcon>
              </div>
              <div>
                <ButtonWithIcon
                  icon={download}
                  isLoading={processing}
                  onClick={() => {
                    processUnsentRequests()
                  }}
                >
                  {intl.formatMessage(messages.processUnsent)}
                </ButtonWithIcon>
              </div>
            </PageBlock>
          </Layout>
        )}
      </ToastConsumer>
    </ToastProvider>
  )
}

export default injectIntl(NotifyAdmin)
