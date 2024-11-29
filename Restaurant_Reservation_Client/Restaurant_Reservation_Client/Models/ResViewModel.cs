using Reservation_Client.Models;

namespace Restaurant_Reservation_Client.Models
{
    public class ResViewModel
    {
        public List<ArrivedTime> Periods { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}
