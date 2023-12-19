using Microsoft.AspNetCore.Identity;
using MetricsApp;

var builder = WebApplication.CreateBuilder(args);

builder.AddNpgsqlDataSource("sqldata");
builder.AddNpgsqlDbContext<AppDbContext>("sqldata");

builder.AddServiceDefaults();

builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorizationBuilder();



builder.Services.AddIdentityCore<MyUser>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddApiEndpoints();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapFallbackToFile("index.html");

var api = app.MapGroup("/api");
api.MapIdentityApi<MyUser>();
api.MapClientApi(app.Configuration);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging();
}

app.UseStaticFiles();
app.UseBlazorFrameworkFiles();

await Auth.InitializeTestUserAsync(app.Services);

app.Run();

