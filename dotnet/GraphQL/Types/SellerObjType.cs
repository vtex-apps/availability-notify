using AvailabilityNotify.Models;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvailabilityNotify.GraphQL.Types
{
    [GraphQLMetadata("SellerObjInputType")]
    public class SellerObjInputType : InputObjectGraphType<SellerObj>
    {
        public SellerObjInputType()
        {
            Name = "SellerObjInputType";
            Field(s => s.addToCartLink, type: typeof(StringGraphType)).Description("Add To Cart Link");
            Field(s => s.sellerDefault, type: typeof(BooleanGraphType)).Description("Seller Default");
            Field(s => s.sellerId, type: typeof(StringGraphType)).Description("Seller Id");
            Field(s => s.sellerName, type: typeof(StringGraphType)).Description("Seller Name");
        }
    }
}
