import products from './products'

const config = Cypress.env()

// Constants
const { id } = config.base.gmail

export default {
  testCase1: {
    skuId: '880210',
    name: 'Shashi',
    email: id,
    sellerObj: {
      sellerId: '',
    },
    warehouseId: '1_1',
    product: products.Weber.name,
  },
  testCase2: {
    data: {
      skuId: '880160',
      warehouseId: '1_1',
    },
    name: 'Shashi',
    email: id,
    product: products.Lenovo.name,
  },
  appDetails: {
    app: 'vtex.availability-notify',
    version: '1.7.3',
  },
}
