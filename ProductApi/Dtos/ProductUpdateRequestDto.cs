namespace ProductApi.Dtos
{
    public class ProductUpdateRequestDto
    {
        public string? Name {  get; set; }
        public decimal Price {  get; set; }
        public int Stock {  get; set; }
    }
}
