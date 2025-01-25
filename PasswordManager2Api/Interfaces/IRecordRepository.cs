using PasswordManager2Api.Dtos;
using PasswordManager2Api.Models;

namespace PasswordManager2Api.Interfaces
{
    public interface IRecordRepository
    {
        Task<List<Record>> GetUserRecords(string userId);
        Task<Record> GetRecordById(int id);
        Task<Record> Create(string userId, RecordDto recordDto);
        Task<Record> Update(int id, string userId, RecordDto recordDto);
        Task<Record?> Delete(int id, string userId);
    }
}
