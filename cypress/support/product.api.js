export default {
  getallProcessUnsentRequestAPI: (baseUrl) => {
    return `${baseUrl}.myvtex.com/availability-notify/process-unsent-requests`
  },
  getProcessAllRequestAPI: (baseUrl) => {
    return `${baseUrl}.myvtex.com/availability-notify/process-all-requests`
  },
  updateProductStatusAPI: (data) => {
    return `https://sandboxusdev.vtexcommercestable.com.br/api/logistics/pvt/inventory/skus/${data.skuId}/warehouses/${data.warehouseId}`
  },
}
