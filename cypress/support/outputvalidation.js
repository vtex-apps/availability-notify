const config = Cypress.env()

// Constants
const { id } = config.base.gmail

export default {
  data1: {
    skuId: '880223',
    name: 'shashi',
    email: id,
    sellerObj: {
      sellerId: '24',
      sellerName: 'shashi',
    },
  },
  data2: {
    name: 'Shashi',
    email: id,
  },
  testCase1: {
    data1: {
      skuId: '880210',
      warehouseId: '1_1',
    },
    name: 'Shashi',
    email: id,
    product: 'weber spirit',
  },
  appDetails: {
    app: 'vtex.availability-notify',
    version: '1.7.3',
  },
}
