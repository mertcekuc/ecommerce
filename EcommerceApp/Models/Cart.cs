using System;
using System.Collections.Generic;

namespace EcommerceApp.Models;

public partial class Cart
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();

    public virtual AspNetUser User { get; set; } = null!;
}
