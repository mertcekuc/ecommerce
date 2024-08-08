using System;
using System.Collections.Generic;

namespace EcommerceApp.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateUserId { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
