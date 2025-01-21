using Restaurant_Reservation_API_Server.Models;

namespace Restaurant_Reservation_API_Server.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        // 讀取所有會員資訊
        Task<IEnumerable<Member>> AllMembers();

        // 使用ID尋找會員資訊
        Task<Member?> GetMember(int id);

        // 新增會員
        Task Create(Member member);

        // 更新會員資訊
        Task Update(int id);

        // 刪除會員
        Task Delete(int id);
    }
}
