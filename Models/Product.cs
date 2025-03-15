using System.ComponentModel.DataAnnotations;

public class ProductStore {
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string Image { get; set; }
}