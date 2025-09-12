using Microsoft.Data.SqlClient;
using System.Data;


using WLSPL_CRM_2.repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register IDbConnection for SQL Server
builder.Services.AddSingleton<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("Conn_String"))
);

// Add session support
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "MySession";
});

// Add your repositories
builder.Services.AddScoped<IUserRegistrationRepo, UserRegistration>();
builder.Services.AddScoped<IleadRepo, LeadRepo>();
builder.Services.AddScoped<IcomapnymasterRepo, CompanymasterRepo>();
builder.Services.AddScoped<Iprospectrepo, prospectrepo>();
builder.Services.AddScoped<IQuotationRepo, Quotationrepo>();
builder.Services.AddScoped<IServicesRepo, ServicesRepo>();
builder.Services.AddScoped<IDealRepo, DealRepo>();
builder.Services.AddScoped<IProformaInvoiceRepo, ProformaInvoiceRepo>();
builder.Services.AddScoped<IWorkorderRepo, WorkorderRepo>();
builder.Services.AddScoped<IchatRepo, ChatRepo>();

builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();

// Add authentication & authorization
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Login/Index";
        options.AccessDeniedPath = "/Login/AccessDenied";
    });

builder.Services.AddAuthorization();

// Add SignalR support
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Enable session before routing
app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Map controller routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

// Map the SignalR hub endpoint
//app.MapHub<ChatHub>("/chatHub");

app.Run();
