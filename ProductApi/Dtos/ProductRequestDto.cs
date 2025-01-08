namespace ProductApi.Dtos
{
    public class ProductRequestDto
    {
        public required string Name {  get; set; }
        public required decimal Price {  get; set; }
        public required int Stock {  get; set; }
    }
}
