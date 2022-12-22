export default {
  getProcessAllRequestAPI: () => {
    return `https://app.io.vtex.com/vtex.availability-notify/v1`
  },
  updateProductStatusAPI: (warehouseId, skuId) => {
    return `https://productusqa.vtexcommercestable.com.br/api/logistics/pvt/inventory/skus/${skuId}/warehouses/${warehouseId}`
  },
}
