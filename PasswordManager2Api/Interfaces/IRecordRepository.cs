using PasswordManager2Api.Models;

namespace PasswordManager2Api.Interfaces
{
    public interface IRecordRepository
    {
        Task<List<Record>> GetUserRecords(int userId);
        Task<Record> GetRecordById(int id);
        Task<Record> Create(Record record);
        Task<Record> Update(int id, Record record);
        Task<Record?> Delete(int id);
    }
}
