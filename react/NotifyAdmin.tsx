/* eslint-disable no-await-in-loop */
/* eslint-disable no-loop-func */
import type { FC } from 'react'
import React, { useState } from 'react'
import { injectIntl, defineMessages } from 'react-intl'
import {
  Layout,
  PageBlock,
  PageHeader,
  ButtonWithIcon,
  IconDownload,
} from 'vtex.styleguide'
import XLSX from 'xlsx'

const NotifyAdmin: FC<any> = ({ intl }) => {
  const [state, setState] = useState<any>({
    loading: false,
    processing: false,
  })

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

    const data: any = []

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

      data.push(requestData)
    }

    const ws = XLSX.utils.json_to_sheet(data, { header })
    const wb = XLSX.utils.book_new()

    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1')
    const exportFileName = `requests.xls`

    XLSX.writeFile(wb, exportFileName)
  }

  const processRequestsResults = (processedRequests: any) => {
    const header = ['Sku Id', 'Quantity Available', 'Email', 'Sent', 'Updated']

    const data: any = []

    for (const request of processedRequests) {
      const requestData = {
        'Sku Id': request.skuId,
        'Quantity Available': request.quantityAvailable,
        Email: request.email,
        Sent: request.notificationSent,
        Updated: request.updated,
      }

      data.push(requestData)
    }

    const ws = XLSX.utils.json_to_sheet(data, { header })
    const wb = XLSX.utils.book_new()

    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1')
    const exportFileName = `processedRequests.xls`

    XLSX.writeFile(wb, exportFileName)
  }

  const getAllRequests = async () => {
    setState({ ...state, loading: true })

    const data: any = await fetch(
      `/_v/availability-notify/list-requests`
    ).then(response => response.json())

    const requestArr = data

    downloadRequests(requestArr)
    setState({ ...state, loading: false })
  }

  const processUnsentRequests = async () => {
    setState({ ...state, processing: true })

    const data: any = await fetch(
      `/_v/availability-notify/process-unsent-requests`
    ).then(response => response.json())

    const requestArr = data

    processRequestsResults(requestArr)
    setState({ ...state, processing: false })
  }

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
  })

  const download = <IconDownload />

  return (
    <Layout
      pageHeader={<PageHeader title={intl.formatMessage(messages.title)} />}
    >
      <PageBlock variation="full">
        <ButtonWithIcon
          icon={download}
          isLoading={loading}
          onClick={() => {
            getAllRequests()
          }}
        >
          {intl.formatMessage(messages.download)}
        </ButtonWithIcon>
        <ButtonWithIcon
          icon={download}
          isLoading={processing}
          onClick={() => {
            processUnsentRequests()
          }}
        >
          {intl.formatMessage(messages.processUnsent)}
        </ButtonWithIcon>
      </PageBlock>
    </Layout>
  )
}

export default injectIntl(NotifyAdmin)
