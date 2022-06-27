using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvailabilityNotify.Models
{
    public class CartSimulationResponse
{
        [JsonProperty("items")]
        public CartResponseItem[] Items { get; set; }

        [JsonProperty("ratesAndBenefitsData")]
        public RatesAndBenefitsData RatesAndBenefitsData { get; set; }

        [JsonProperty("paymentData")]
        public PaymentData PaymentData { get; set; }

        [JsonProperty("selectableGifts")]
        public object[] SelectableGifts { get; set; }

        [JsonProperty("marketingData")]
        public object MarketingData { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("logisticsInfo")]
        public LogisticsInfo[] LogisticsInfo { get; set; }

        [JsonProperty("messages")]
        public object[] Messages { get; set; }

        [JsonProperty("purchaseConditions")]
        public PurchaseConditions PurchaseConditions { get; set; }

        [JsonProperty("pickupPoints")]
        public object[] PickupPoints { get; set; }

        [JsonProperty("subscriptionData")]
        public object SubscriptionData { get; set; }

        [JsonProperty("totals")]
        public Total[] Totals { get; set; }

        [JsonProperty("itemMetadata")]
        public object ItemMetadata { get; set; }
    }

    public class CartResponseItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("requestIndex")]
        public string RequestIndex { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("seller")]
        public string Seller { get; set; }

        [JsonProperty("sellerChain")]
        public string[] SellerChain { get; set; }

        [JsonProperty("tax")]
        public string Tax { get; set; }

        [JsonProperty("priceValidUntil")]
        public string PriceValidUntil { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("listPrice")]
        public string ListPrice { get; set; }

        [JsonProperty("rewardValue")]
        public string RewardValue { get; set; }

        [JsonProperty("sellingPrice")]
        public string SellingPrice { get; set; }

        [JsonProperty("offerings")]
        public object[] Offerings { get; set; }

        [JsonProperty("priceTags")]
        public object[] PriceTags { get; set; }

        [JsonProperty("measurementUnit")]
        public string MeasurementUnit { get; set; }

        [JsonProperty("unitMultiplier")]
        public string UnitMultiplier { get; set; }

        [JsonProperty("parentItemIndex")]
        public object ParentItemIndex { get; set; }

        [JsonProperty("parentAssemblyBinding")]
        public object ParentAssemblyBinding { get; set; }

        [JsonProperty("availability")]
        public string Availability { get; set; }

        [JsonProperty("catalogProvider")]
        public string CatalogProvider { get; set; }

        [JsonProperty("priceDefinition")]
        public PriceDefinition PriceDefinition { get; set; }
    }

    public class PriceDefinition
    {
        [JsonProperty("calculatedSellingPrice")]
        public string CalculatedSellingPrice { get; set; }

        [JsonProperty("total")]
        public string Total { get; set; }

        [JsonProperty("sellingPrices")]
        public SellingPrice[] SellingPrices { get; set; }
    }

    public class SellingPrice
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("quantity")]
        public string Quantity { get; set; }
    }

    public class LogisticsInfo
    {
        [JsonProperty("itemIndex")]
        public long ItemIndex { get; set; }

        [JsonProperty("addressId")]
        public object AddressId { get; set; }

        [JsonProperty("selectedSla")]
        public object SelectedSla { get; set; }

        [JsonProperty("selectedDeliveryChannel")]
        public object SelectedDeliveryChannel { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("shipsTo")]
        public string[] ShipsTo { get; set; }

        [JsonProperty("slas")]
        public Sla[] Slas { get; set; }

        [JsonProperty("deliveryChannels")]
        public DeliveryChannel[] DeliveryChannels { get; set; }
    }

    public class DeliveryChannel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class Sla
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("deliveryChannel")]
        public string DeliveryChannel { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("deliveryIds")]
        public DeliveryId[] DeliveryIds { get; set; }

        [JsonProperty("shippingEstimate")]
        public string ShippingEstimate { get; set; }

        [JsonProperty("shippingEstimateDate")]
        public object ShippingEstimateDate { get; set; }

        [JsonProperty("lockTTL")]
        public object LockTtl { get; set; }

        [JsonProperty("availableDeliveryWindows")]
        public object[] AvailableDeliveryWindows { get; set; }

        [JsonProperty("deliveryWindow")]
        public object DeliveryWindow { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("listPrice")]
        public string ListPrice { get; set; }

        [JsonProperty("tax")]
        public string Tax { get; set; }

        [JsonProperty("pickupStoreInfo")]
        public PickupStoreInfo PickupStoreInfo { get; set; }

        [JsonProperty("pickupPointId")]
        public object PickupPointId { get; set; }

        [JsonProperty("pickupDistance")]
        public string PickupDistance { get; set; }

        [JsonProperty("polygonName")]
        public object PolygonName { get; set; }

        [JsonProperty("transitTime")]
        public string TransitTime { get; set; }
    }

    public class DeliveryId
    {
        [JsonProperty("courierId")]
        public string CourierId { get; set; }

        [JsonProperty("warehouseId")]
        public string WarehouseId { get; set; }

        [JsonProperty("dockId")]
        public string DockId { get; set; }

        [JsonProperty("courierName")]
        public string CourierName { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("kitItemDetails")]
        public object[] KitItemDetails { get; set; }
    }

    public class PickupStoreInfo
    {
        [JsonProperty("isPickupStore")]
        public bool IsPickupStore { get; set; }

        [JsonProperty("friendlyName")]
        public object FriendlyName { get; set; }

        [JsonProperty("address")]
        public object Address { get; set; }

        [JsonProperty("additionalInfo")]
        public object AdditionalInfo { get; set; }

        [JsonProperty("dockId")]
        public object DockId { get; set; }
    }

    public class PaymentData
    {
        [JsonProperty("installmentOptions")]
        public InstallmentOption[] InstallmentOptions { get; set; }

        [JsonProperty("paymentSystems")]
        public PaymentSystem[] PaymentSystems { get; set; }

        [JsonProperty("payments")]
        public object[] Payments { get; set; }

        [JsonProperty("giftCards")]
        public object[] GiftCards { get; set; }

        [JsonProperty("giftCardMessages")]
        public object[] GiftCardMessages { get; set; }

        [JsonProperty("availableAccounts")]
        public object[] AvailableAccounts { get; set; }

        [JsonProperty("availableTokens")]
        public object[] AvailableTokens { get; set; }
    }

    public class InstallmentOption
    {
        [JsonProperty("paymentSystem")]
        public string PaymentSystem { get; set; }

        [JsonProperty("bin")]
        public object Bin { get; set; }

        [JsonProperty("paymentName")]
        public string PaymentName { get; set; }

        [JsonProperty("paymentGroupName")]
        public string PaymentGroupName { get; set; }

        [JsonProperty("value")]
        public long Value { get; set; }

        [JsonProperty("installments")]
        public Installment[] Installments { get; set; }
    }

    public class Installment
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("hasInterestRate")]
        public bool? HasInterestRate { get; set; }

        [JsonProperty("interestRate")]
        public long InterestRate { get; set; }

        [JsonProperty("value")]
        public long Value { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("sellerMerchantInstallments", NullValueHandling = NullValueHandling.Ignore)]
        public Installment[] SellerMerchantInstallments { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string? Id { get; set; }
    }

    public class PaymentSystem
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("groupName")]
        public string GroupName { get; set; }

        [JsonProperty("validator")]
        public object Validator { get; set; }

        [JsonProperty("stringId")]
        public string StringId { get; set; }

        [JsonProperty("template")]
        public string Template { get; set; }

        [JsonProperty("requiresDocument")]
        public bool RequiresDocument { get; set; }

        [JsonProperty("isCustom")]
        public bool IsCustom { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("requiresAuthentication")]
        public bool RequiresAuthentication { get; set; }

        [JsonProperty("dueDate")]
        public DateTimeOffset DueDate { get; set; }

        [JsonProperty("availablePayments")]
        public object AvailablePayments { get; set; }
    }

    public class PurchaseConditions
    {
        [JsonProperty("itemPurchaseConditions")]
        public ItemPurchaseCondition[] ItemPurchaseConditions { get; set; }
    }

    public class ItemPurchaseCondition
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("seller")]
        public string Seller { get; set; }

        [JsonProperty("sellerChain")]
        public string[] SellerChain { get; set; }

        [JsonProperty("slas")]
        public Sla[] Slas { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("listPrice")]
        public string ListPrice { get; set; }
    }

    public class RatesAndBenefitsData
    {
        [JsonProperty("rateAndBenefitsIdentifiers")]
        public object[] RateAndBenefitsIdentifiers { get; set; }

        [JsonProperty("teaser")]
        public object[] Teaser { get; set; }
    }

    public class Total
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public long Value { get; set; }
    }
}
