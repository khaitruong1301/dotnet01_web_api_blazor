using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using webapi_blazor.Models.ViewModel;

namespace webapi_blazor.models.EbayDB;

public partial class EbayContext : DbContext
{

    public virtual DbSet<ProductDetailVM> ProductDetailVM { get; set; }
    
}