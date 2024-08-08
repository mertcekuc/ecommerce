using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace EcommerceApp.Models;

public partial class User : IdentityUser<int>
{
    public string LastName { get; set; }

	public string FirstName { get; set; } = null!;

	public DateTime CreatedDate { get; set; }
}
