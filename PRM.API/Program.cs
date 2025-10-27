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
using PRM.Infrastructure.ExternalService; 
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Messaging;

namespace PRM.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Cấu hình logging (nên đặt sớm)
			builder.Logging.ClearProviders();
			builder.Logging.AddConsole(); // Hoặc các provider khác

			// Add services to the container.
			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			// DbContexts
			builder.Services.AddDbContext<PRMDbContext>(opt =>
				opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
			builder.Services.AddSingleton<IChatDbContext>(sp =>
			{
				var connectionString = builder.Configuration.GetConnectionString("MongoDb");
				var dbName = builder.Configuration["MongoDbSettings:DatabaseName"];

				Console.WriteLine($"🔍 Mongo connection string: {connectionString}");
				Console.WriteLine($"🔍 Mongo database: {dbName}");

				if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(dbName))
					throw new InvalidOperationException("MongoDB connection string or database name is not configured properly.");

				return new ChatDbContext(connectionString, dbName);
			});


			// SignalR
			builder.Services.AddSignalR();

			// Unit of Work & Repositories
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
			builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
			builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
			builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
			builder.Services.AddScoped<IMessageRepository, MessageRepository>();
			builder.Services.AddScoped<IProductRepository, ProductRepository>();
			builder.Services.AddScoped<IReviewRepository, ReviewRepository>();


			// Application Services
			builder.Services.AddScoped<ICategoryService, CategoryService>();
			builder.Services.AddScoped<ISupplierService, SupplierService>();
			builder.Services.AddScoped<IVoucherService, VoucherService>();
			builder.Services.AddScoped<IConversationService, ConversationService>();
			builder.Services.AddScoped<IChatNotifier, SignalRChatNotifier>();
			builder.Services.AddScoped<IChatService, ChatService>();
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IProductService, ProductService>();
			builder.Services.AddScoped<IReviewService, ReviewService>();
			builder.Services.AddScoped<IUserDeviceTokenService, UserDeviceTokenService>();

			// External Services (Firebase)
			builder.Services.AddScoped<IFirebaseService, FirebaseService>();

			// CORS
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAll", policy =>
				{
					policy.AllowAnyHeader()
						  .AllowAnyMethod()
						  .AllowCredentials()
						  .SetIsOriginAllowed(_ => true); // Cho phép mọi origin
				});
			});

			// --- KHỞI TẠO FIREBASE ADMIN SDK ---
			try
			{
				var firebaseKeyPath = builder.Configuration["Firebase:AdminSdkKeyPath"] ?? "prmFirebase.json"; // Lấy từ config hoặc mặc định
				if (!File.Exists(firebaseKeyPath))
				{
					Console.WriteLine($"Firebase Admin SDK key file not found at: {Path.GetFullPath(firebaseKeyPath)}");
					throw new FileNotFoundException("Firebase Admin SDK key file not found.", firebaseKeyPath);
				}

				// Chỉ tạo nếu chưa tồn tại
				if (FirebaseApp.DefaultInstance == null)
				{
					FirebaseApp.Create(new AppOptions()
					{
						Credential = GoogleCredential.FromFile(firebaseKeyPath)
					});
					Console.WriteLine("Firebase Admin SDK initialized successfully.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error initializing Firebase Admin SDK: {ex.Message}");
				throw;
			}
			builder.Services.AddSingleton(FirebaseMessaging.DefaultInstance);
			var app = builder.Build();
			try
			{
				using (var scope = app.Services.CreateScope())
				{
					var context = scope.ServiceProvider.GetRequiredService<PRMDbContext>();
					context.Database.Migrate();
					Console.WriteLine("Database migration applied successfully.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred during database migration: {ex.Message}");
				// Xử lý lỗi migration
			}


			// Configure Swagger
			// if (app.Environment.IsDevelopment()) // Nên chỉ bật Swagger trong Development
			// {
			app.UseSwagger();
			app.UseSwaggerUI();
			// }

			// app.UseHttpsRedirection(); // Bật lại nếu bạn dùng HTTPS

			// CORS phải đặt trước UseAuthorization và MapControllers/MapHub
			app.UseCors("AllowAll");

			// Middleware xử lý lỗi tùy chỉnh
			app.UseMiddleware<ExceptionMiddleware>();

			app.UseAuthorization();

			app.MapControllers();
			app.MapHub<ChatHub>("/chatHub");

			app.Run();
		}
	}
}
