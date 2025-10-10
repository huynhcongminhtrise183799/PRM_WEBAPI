using Microsoft.EntityFrameworkCore;
using PRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Infrastructure
{
	public class PRMDbContext : DbContext
	{
		public PRMDbContext(DbContextOptions<PRMDbContext> options) : base(options)
		{
		}

		public DbSet<User> Users { get; set; }

		public DbSet<Cart> Carts { get; set; }

		public DbSet<CartItem> CartItems { get; set; }

		public DbSet<Category> Categories { get; set; }

		public DbSet<Product> Products { get; set; }

		public DbSet<ProductColors> ProductColors { get; set; }

		public DbSet<ProductImage> ProductImages { get; set; }

		public DbSet<Order> Orders { get; set; }

		public DbSet<OrderItem> OrderItems { get; set; }

		public DbSet<Payments> Payments { get; set; }

		public DbSet<Review> Reviews { get; set; }

		public DbSet<Voucher> Vouchers { get; set; }

		public DbSet<Suppliers> Suppliers { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>(entity =>
			{
				entity.ToTable("Users");
				entity.HasKey(e => e.UserId);
				entity.Property(e => e.UserId).ValueGeneratedOnAdd();
			});

			modelBuilder.Entity<Cart>(entity =>
			{
				entity.ToTable("Carts");
				entity.HasKey(e => e.CartId);
				entity.Property(e => e.CartId).ValueGeneratedOnAdd();
				entity.HasOne(e => e.User)
					  .WithMany(u => u.Carts)
					  .HasForeignKey(e => e.UserId);
			});
			modelBuilder.Entity<CartItem>(entity =>
			{
				entity.ToTable("CartItems");
				entity.HasKey(e => e.CartItemId);
				entity.Property(e => e.CartItemId).ValueGeneratedOnAdd();

				entity.HasOne(e => e.Cart)
					  .WithMany(c => c.CartItems)
					  .HasForeignKey(e => e.CartId);

				entity.HasOne(e => e.ProductColor)
						.WithMany(pc => pc.CartItems)
						.HasForeignKey(e => e.ProductColorId);
			});
			modelBuilder.Entity<Category>(entity =>
			{
				entity.ToTable("Categories");
				entity.HasKey(e => e.CategoryId);
				entity.Property(e => e.CategoryId).ValueGeneratedOnAdd();
			});
			modelBuilder.Entity<Product>(entity =>
			{
				entity.ToTable("Products");
				entity.HasKey(e => e.ProductId);
				entity.Property(e => e.ProductId).ValueGeneratedOnAdd();

				entity.HasOne(e => e.Category)
					  .WithMany(c => c.Products)
					  .HasForeignKey(e => e.CategoryId);

				entity.HasOne(e => e.Supplier)
				.WithMany(s => s.Products)
					  .HasForeignKey(e => e.SupplierId);
			});
			modelBuilder.Entity<ProductColors>(entity =>
			{
				entity.ToTable("ProductColors");
				entity.HasKey(e => e.ProductColorsId);
				entity.Property(e => e.ProductColorsId).ValueGeneratedOnAdd();

				entity.HasOne(e => e.Product)
					  .WithMany(p => p.ProductColors)
					  .HasForeignKey(e => e.ProductId);
			});
			modelBuilder.Entity<ProductImage>(entity =>
			{
				entity.ToTable("ProductImages");
				entity.HasKey(e => e.ProductImageId);
				entity.Property(e => e.ProductImageId).ValueGeneratedOnAdd();

				entity.HasOne(e => e.ProductColor)
					  .WithMany(pc => pc.ProductImages)
					  .HasForeignKey(e => e.ProductColorId);
			});
			modelBuilder.Entity<Order>(entity =>
			{
				entity.ToTable("Orders");
				entity.HasKey(e => e.OrderId);
				entity.Property(e => e.OrderId).ValueGeneratedOnAdd();

				entity.HasOne(e => e.User)
					  .WithMany(u => u.Orders)
					  .HasForeignKey(e => e.UserId);

				entity.HasOne(e => e.Voucher)
					  .WithMany(v => v.Orders)
					  .HasForeignKey(e => e.VoucherId)
					  .IsRequired(false);
			});
			modelBuilder.Entity<OrderItem>(entity =>
			{
				entity.ToTable("OrderItems");
				entity.HasKey(e => e.OrderItemId);
				entity.Property(e => e.OrderItemId).ValueGeneratedOnAdd();

				entity.HasOne(e => e.Order)
					  .WithMany(o => o.OrderItems)
					  .HasForeignKey(e => e.OrderId);

				entity.HasOne(e => e.ProductColor)
					  .WithMany(pc => pc.OrderItems)
					  .HasForeignKey(e => e.ProductColorId);
			});
			modelBuilder.Entity<Payments>(entity =>
			{
				entity.ToTable("Payments");
				entity.HasKey(e => e.PaymentId);
				entity.Property(e => e.PaymentId).ValueGeneratedOnAdd();

				entity.HasOne(e => e.Order)
						.WithOne(o => o.Payments)
						.HasForeignKey<Payments>(e => e.OrderId);
			});
			modelBuilder.Entity<Suppliers>(entity =>
			{
				entity.ToTable("Suppliers");
				entity.HasKey(e => e.SupplierId);
				entity.Property(e => e.SupplierId).ValueGeneratedOnAdd();
			});
			modelBuilder.Entity<Review>(entity =>
			{
				entity.ToTable("Reviews");
				entity.HasKey(e => e.ReviewId);
				entity.Property(e => e.ReviewId).ValueGeneratedOnAdd();

				entity.HasOne(e => e.User)
					  .WithMany(e => e.Reviews)
					  .HasForeignKey(e => e.UserId);

				entity.HasOne(e => e.Product)
					  .WithMany(e => e.Reviews)
					  .HasForeignKey(e => e.ProductId);
			});
			modelBuilder.Entity<Voucher>(entity =>
			{
				entity.ToTable("Vouchers");
				entity.HasKey(e => e.VoucherId);
				entity.Property(e => e.VoucherId).ValueGeneratedOnAdd();

			});

		}
	}
}
