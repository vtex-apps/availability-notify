using System;
using System.Collections.Generic;
using System.Text;

namespace AvailabilityNotify.Models
{
	public class SellerObj
	{
		public string sellerId { get; set; }
		public string sellerName { get; set; }
		public string addToCartLink { get; set; }
		public bool sellerDefault { get; set; }
	}
}
