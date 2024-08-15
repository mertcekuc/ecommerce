namespace EcommerceApp.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public List<CartDetail> CartItems { get; set; }
        public decimal CartTotal { get; set; }
        public List<Address> Addresses { get; set; }
        public int SelectedAddressId { get; set; }
        public string PaymentMethod { get; set; }
        public string CreditCardNumber { get; set; }
        public string CreditCardExpiry { get; set; }
        public string CreditCardCvc { get; set; }
    }
}
