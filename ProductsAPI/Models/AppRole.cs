﻿using Microsoft.AspNetCore.Identity;

namespace ProductsAPI.Models
{
    public class AppRole : IdentityRole<int>
    {
        public int MyProperty { get; set; }
    }
}
