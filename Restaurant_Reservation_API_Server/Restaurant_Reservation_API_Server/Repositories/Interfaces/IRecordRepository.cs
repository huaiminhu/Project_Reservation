using Restaurant_Reservation_API_Server.Models;

namespace Restaurant_Reservation_API_Server.Repositories.Interfaces
{
    public interface IRecordRepository
    {
        // 讀取所有訂位紀錄
        Task<IEnumerable<Record>> AllRecords();

        // 使用ID尋找訂位紀錄
        Task<Record?> GetRecord(int id);

        // 新增訂位紀錄
        Task Create(Record record);

        // 更新訂位紀錄
        Task Update(int id);

        // 刪除訂位紀錄
        Task Delete(int id);
    }
}
