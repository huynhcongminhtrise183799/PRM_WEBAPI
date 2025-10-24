using Microsoft.EntityFrameworkCore;
using PRM.Application.IService;         
using PRM.Application.Service;          
using PRM.Application.Interfaces;
using PRM.Application.Interfaces.Repositories;
using PRM.Application.Services;
using PRM.Infrastructure;
using PRM.Infrastructure.Repositories;
using PRM.API.Middleware;            

namespace PRM.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddDbContext<PRMDbContext>(opt =>
				opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));


			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

			// Đăng ký Repo
			builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
			builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
			builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();

			// Đăng ký Service
			builder.Services.AddScoped<ICategoryService, CategoryService>();
			builder.Services.AddScoped<ISupplierService, SupplierService>();
			builder.Services.AddScoped<IVoucherService, VoucherService>();
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IProductService, ProductService>();
			builder.Services.AddScoped<IReviewService, ReviewService>();
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAll", policy =>
					policy.AllowAnyOrigin()
						  .AllowAnyMethod()
						  .AllowAnyHeader());
			});

			var app = builder.Build();

			using (var scope = app.Services.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<PRMDbContext>();
				context.Database.Migrate();
			}


			//if (app.Environment.IsDevelopment())
			//{
				app.UseSwagger();
				app.UseSwaggerUI();
			//}

		//	app.UseHttpsRedirection();

			app.UseMiddleware<ExceptionMiddleware>();

			app.UseCors("AllowAll");

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
