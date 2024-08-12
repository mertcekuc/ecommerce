using System;
using System.Collections.Generic;

namespace EcommerceApp.Models;

public partial class Comment
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public string Comment1 { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
