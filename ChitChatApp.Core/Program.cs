using ChitChatApp.Core.Application.Interfaces;
using ChitChatApp.Core.Application.Services;
using ChitChatApp.Core.Infrastructure.Persistence.Repositories;
using ChitChatApp.Core.Infrastructure.Supabase;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Get Supabase configuration
var supabaseUrl = builder.Configuration["Supabase:Url"];
var supabaseAnonKey = builder.Configuration["Supabase:AnonKey"];

if (string.IsNullOrEmpty(supabaseUrl) || string.IsNullOrEmpty(supabaseAnonKey))
{
    throw new InvalidOperationException("Supabase URL or AnonKey is missing in configuration.");
}

// Configure application services
builder.Services.AddSingleton<SupabaseService>(sp =>
    new SupabaseService(supabaseUrl, supabaseAnonKey));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IChatRoomRepository, ChatRoomRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline
// Enable Swagger in all environments for testing purposes
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChitChat API V1");
    c.RoutePrefix = string.Empty; // Makes Swagger UI available at root
});

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

Console.WriteLine("ChitChat API is starting...");
Console.WriteLine("Swagger UI will be available at the root URL");

app.Run();
