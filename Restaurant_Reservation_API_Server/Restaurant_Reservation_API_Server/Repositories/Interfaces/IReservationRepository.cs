﻿using Microsoft.AspNetCore.Mvc;
using Restaurant_Reservation_API_Server.Models;

namespace Restaurant_Reservation_API_Server.Repositories.Interfaces
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Reservation>> AllReservations();
        Task<Reservation?> FindReservation(int id);
        Task Create(Reservation reservation);
        Task Update(Reservation reservation);
        Task Delete(Reservation reservation);
        Reservation? FindByPhone(string phone);
    }
}
