namespace Restaurant_Reservation_Client.Models
{
    public class SeatsViewModel   // 用於呈現首頁即時時段及空位數資料
    {
        public string Period { get; set; }   // 訂位時段
        public int RemainSeats { get; set; }   // 目前空位數
    }
}
