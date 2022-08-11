export default {
  getProcessAllRequestAPI: () => {
    return `https://app.io.vtex.com/vtex.availability-notify/v1`
  },
  updateProductStatusAPI: data => {
    return `https://productusqa.vtexcommercestable.com.br/api/logistics/pvt/inventory/skus/${data.skuId}/warehouses/${data.warehouseId}`
  },
}
