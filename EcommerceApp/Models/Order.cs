using System;
using System.Collections.Generic;

namespace EcommerceApp.Models;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int StatusId { get; set; }

    public int AddressId { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual OrderStatus Status { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
