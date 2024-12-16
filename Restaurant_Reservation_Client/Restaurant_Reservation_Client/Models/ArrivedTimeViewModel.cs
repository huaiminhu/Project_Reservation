namespace Restaurant_Reservation_Client.Models
{
    public class ArrivedTimeViewModel
    {
        public int Id { get; set; }
        public string Period { get; set; }

        public ICollection<ReservationViewModel>? Reservations { get; set; }
    }
}
