﻿using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public class DiscountContext : DbContext
    {
        public DbSet<Coupon> Coupons { get; set; } = default!;
        public DiscountContext(DbContextOptions<DiscountContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Coupon>().HasData(
                    new Coupon { Id = 1, ProductName="IPhone X", Description="IPhone description", Amount=299 },
                    new Coupon { Id = 2, ProductName = "Samsung 10", Description = "Samsung description", Amount = 299 }
            );
        }
    }
}
