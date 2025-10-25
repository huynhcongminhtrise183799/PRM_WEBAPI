using Microsoft.EntityFrameworkCore;
using PRM.API.ChatHubs;
using PRM.Application.Interfaces;
using PRM.Application.Interfaces.Repositories;
using PRM.Application.IService;
using PRM.Application.Service;
using PRM.Application.Services;
using PRM.Domain.IRepository;
using PRM.Infrastructure;
using PRM.Infrastructure.Repositories;
using PRM.API.Middleware;
using PRM.Infrastructure.Repository;

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
			builder.Services.AddSingleton<IChatDbContext>(sp =>
			{
				var connectionString = builder.Configuration.GetConnectionString("MongoDb");
				var dbName = builder.Configuration["MongoDbSettings:DatabaseName"];
				return new ChatDbContext(connectionString, dbName);
			});
			builder.Services.AddSignalR();
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
			builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ISupplierService, SupplierService>();
            builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
            builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
            builder.Services.AddScoped<IVoucherService, VoucherService>();
			builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
			builder.Services.AddScoped<IMessageRepository, MessageRepository>();
			builder.Services.AddScoped<IConversationService, ConversationService>();
			builder.Services.AddScoped<IChatNotifier, SignalRChatNotifier>();
			builder.Services.AddScoped<IChatService , ChatService>();
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IProductService, ProductService>();
			builder.Services.AddScoped<IReviewService, ReviewService>();
			builder.Services.AddScoped<IUserDeviceTokenService, UserDeviceTokenService>();		
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAll", policy =>
				{
					policy.AllowAnyHeader()
						  .AllowAnyMethod()
						  .AllowCredentials()
						  .SetIsOriginAllowed(_ => true);
				});
			});
			var app = builder.Build();
			app.UseCors("AllowAll");

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
			app.MapHub<ChatHub>("/chatHub");

			app.Run();
		}
	}
}
