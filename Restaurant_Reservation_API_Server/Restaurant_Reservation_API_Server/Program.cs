using Microsoft.EntityFrameworkCore;
using Restaurant_Reservation_API_Server.Infrastructure.Data;
using Restaurant_Reservation_API_Server.Infrastructure.Repositories;
using Restaurant_Reservation_API_Server.Infrastructure.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ReservationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ReservationDbContext")));

// ¨Ì¿àª`¤J
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IArrivalTimeRepository, ArrivalTimeRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
