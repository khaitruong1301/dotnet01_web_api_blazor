using System;
using System.Collections.Generic;

namespace webapi_blazor.models.EbayDB;

public partial class Bid
{
    public int Id { get; set; }

    public int ListingId { get; set; }

    public int BidderId { get; set; }

    public decimal BidAmount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? Deleted { get; set; }

    public virtual User Bidder { get; set; } = null!;

    public virtual Listing Listing { get; set; } = null!;
}
