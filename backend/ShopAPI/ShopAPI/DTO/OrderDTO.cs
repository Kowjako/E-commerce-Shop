namespace ShopAPI.DTO
{
    public class OrderDTO
    {
        public string BasketId { get; set; }
        public int DeliveryId { get; set; }
        public AddressDTO ShipToAddress { get; set; }
    }
}
