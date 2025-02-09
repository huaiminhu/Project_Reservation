using Restaurant_Reservation_Client.Service.Services.IServices;
using Restaurant_Reservation_Client.Service.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// ���USESSION�A��, �]�m60�����Ჾ��
builder.Services.AddSession(options => 
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// �̿�`�J
builder.Services.AddScoped<IReservationApiConsuming, ReservationApiConsuming>();
builder.Services.AddScoped<IArrivalTimeApiConsuming, ArrivalTimeApiConsuming>();
builder.Services.AddScoped<IShowPeriods, ShowPeriods>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Reservation}/{action=Index}/{id?}");

app.Run();
