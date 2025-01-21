using Restaurant_Reservation_API_Server.Models;

namespace Restaurant_Reservation_API_Server.Repositories.Interfaces
{
    public interface IActivityRepository
    {
        // 讀取所有優惠項目
        Task<IEnumerable<Activity>> AllActivities();

        // 使用ID尋找優惠項目
        Task<Activity?> GetActivity(int id);

        // 新增優惠項目
        Task Create(Activity activity);

        // 更新優惠項目
        Task Update(int id);

        // 刪除優惠項目
        Task Delete(int id);
    }
}
