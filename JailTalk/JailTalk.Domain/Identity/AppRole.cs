﻿using Microsoft.AspNetCore.Identity;

namespace JailTalk.Domain.Identity;
public class AppRole : IdentityRole
{
    public string Description { get; set; }
}
