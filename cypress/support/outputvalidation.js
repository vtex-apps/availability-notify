import { generateEmailId } from './availability-notify.apis'
import products from './products'

export default {
  testCase1: {
    skuId: '880210',
    name: 'Shashi',
    email: generateEmailId(),
    warehouseId: '1_1',
    product: products.Weber.name,
  },
  testCase2: {
    skuId: '880160',
    warehouseId: '1_1',
    name: 'Shashi',
    email: generateEmailId(),
    product: products.Lenovo.name,
  },
  appDetails: {
    app: 'vtex.availability-notify',
    version: '1.7.3',
  },
}
