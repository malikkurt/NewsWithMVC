using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsWithMVC.Models;

namespace NewsWithMVC.Data
{
    public class NewsWithMVCContext : DbContext
    {
        public NewsWithMVCContext (DbContextOptions<NewsWithMVCContext> options)
            : base(options)
        {
        }
    }
}
