export interface GetSkuContextResponse {
  Id: number | null
  ProductId: number | null
  NameComplete: string
  ProductName: string
  ProductDescription: string
  ProductRefId: string
  TaxCode: string
  SkuName: string
  IsActive: boolean
  IsTransported: boolean
  IsInventoried: boolean
  IsGiftCardRecharge: boolean
  ImageUrl: string
  DetailUrl: string
  CSCIdentification: unknown
  BrandId: string
  BrandName: string
  IsBrandActive: boolean
  Dimension: Dimension
  RealDimension: RealDimension
  ManufacturerCode: string
  IsKit: boolean
  KitItems: unknown[]
  Services: unknown[]
  Categories: unknown[]
  CategoriesFullPath: string[]
  Attachments: unknown[]
  Collections: unknown[]
  SkuSellers: SkuSeller[]
  SalesChannels: number[]
  Images: Image[]
  Videos: unknown[]
  SkuSpecifications: Specification[]
  ProductSpecifications: Specification[]
  ProductClustersIds: string
  PositionsInClusters: PositionsInClusters
  ProductCategoryIds: string
  IsDirectCategoryActive: boolean
  ProductGlobalCategoryId: number | null
  ProductCategories: unknown
  CommercialConditionId: unknown
  RewardValue: number
  AlternateIds: AlternateIds
  AlternateIdValues: string[]
  EstimatedDateArrival: string
  MeasurementUnit: string
  UnitMultiplier: number | null
  InformationSource: string
  ModalType: unknown
  KeyWords: string
  ReleaseDate: string
  ProductIsVisible: boolean
  ShowIfNotAvailable: boolean
  IsProductActive: boolean
  ProductFinalScore: number | null
}

export interface AlternateIds {
  RefId: string
}

export interface Dimension {
  cubicweight: number | null
  height: number | null
  length: number | null
  weight: number | null
  width: number | null
}

export interface Image {
  ImageUrl: string
  ImageName: string
  FileId: number | null
}

export interface PositionsInClusters {
  [key: string]: unknown
}

export interface Specification {
  FieldId: number | null
  FieldName: string
  FieldValueIds: number[]
  FieldValues: string[]
  IsFilter: boolean
  FieldGroupId: number | null
  FieldGroupName: string
}

export interface RealDimension {
  realCubicWeight: number | null
  realHeight: number | null
  realLength: number | null
  realWeight: number | null
  realWidth: number | null
}

export interface SkuSeller {
  SellerId: string
  StockKeepingUnitId: number | null
  SellerStockKeepingUnitId: string
  IsActive: boolean
  FreightCommissionPercentage: number | null
  ProductCommissionPercentage: number | null
}
