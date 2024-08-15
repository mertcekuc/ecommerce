namespace EcommerceApp.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public AspNetUser User { get; set; }
        public Cart cart { get; set; }
        public decimal CartTotal { get; set; }
        public List<Address> Addresses { get; set; }
    }
}
