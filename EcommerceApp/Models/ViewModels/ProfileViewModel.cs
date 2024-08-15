namespace EcommerceApp.Models.ViewModels
{
    public class ProfileViewModel
    {
        public List<Address> Addresses { get; set; }
        public AspNetUser User { get; set; }
        public List<Order> Orders { get; set; }
    }
}
