namespace Restaurant_Reservation_Client.Models
{
    public class DisplayViewModel   // 用於呈現訂位時段下拉式選單資料
    {
        public int Id { get; set; }

        // 最後呈現的資料: 時段、目前剩餘空位數
        public string Display { get; set; }   
    }
}
