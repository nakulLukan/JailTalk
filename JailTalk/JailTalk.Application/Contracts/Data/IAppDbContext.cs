﻿using JailTalk.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JailTalk.Application.Contracts.Data;

public interface IAppDbContext
{
    public DbSet<AppUser> AspNetUsers { get; set; }
}