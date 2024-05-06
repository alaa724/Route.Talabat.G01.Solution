namespace Route.Talabat.APIs.DTO
{
	public class OrderItemDto
	{
        public int Id { get; set; }
		public int ProductId { get; set; }
		public string ProductName { get; set; } = null!;
		public string ProductUrl { get; set; } = null!;
		public decimal Price { get; set; }

		public int Quntity { get; set; }
	}
}