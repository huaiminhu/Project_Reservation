using Microsoft.EntityFrameworkCore;
using Restaurant_Reservation_API_Server.Data;
using Restaurant_Reservation_API_Server.Repositories.Interfaces;
using Restaurant_Reservation_API_Server.Repositories.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ReservationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ReservationDbContext")));
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IarrivalTimeRepository, arrivalTimeRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
