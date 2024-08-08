using System;
using System.Collections.Generic;

namespace EcommerceApp.Models;

public partial class Address
{
    public string Id { get; set; } = null!;

    public int UserId { get; set; }

    public string Street { get; set; } = null!;

    public int CityId { get; set; }

    public int CountryId { get; set; }

    public int PostalCode { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime ModifiedDate { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual Country Country { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
