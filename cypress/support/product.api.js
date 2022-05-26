export default {
  getallProcessUnsentRequestAPI: (baseUrl) => {
    return `${baseUrl}/availability-notify/process-unsent-requests`
  },
  getProcessAllRequestAPI: (baseUrl) => {
    return `${baseUrl}/availability-notify/process-all-requests`
  },
}
