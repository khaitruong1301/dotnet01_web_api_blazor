using webapi_blazor.models.EbayDB;

public  class OrderItemVM{
     public int Id { get; set; }

    public DateTime? CreatedAt { get; set; }

    public List<ItemOrderVM> lstOrderDetail { get; set; } = new List<ItemOrderVM>();
}
public  class ItemOrderVM
{
      public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
}