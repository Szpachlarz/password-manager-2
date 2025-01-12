using Microsoft.EntityFrameworkCore;
using PasswordManager2Api.Data;
using PasswordManager2Api.Interfaces;
using PasswordManager2Api.Models;

namespace PasswordManager2Api.Repositories
{
    public class RecordRepository : IRecordRepository
    {
        private readonly ApplicationDbContext _context;
        public RecordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Record> Create(Record record)
        {
            await _context.Records.AddAsync(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<Record?> Delete(int id)
        {
            var record = await _context.Records.FirstOrDefaultAsync(x => x.Id == id);

            if (record == null)
            {
                return null;
            }

            _context.Records.Remove(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<Record?> GetRecordById(int id)
        {
            return await _context.Records.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<List<Record>> GetUserRecords(int userId)
        {
            return await _context.Records.Where(i => i.AccountId == userId).ToListAsync();
        }

        public async Task<Record> Update(int id, Record record)
        {
            var existingRecord = await _context.Records.FirstOrDefaultAsync(i => i.Id == id);
            if (existingRecord == null)
            {
                return null;
            }

            existingRecord.ServiceName = record.ServiceName;
            existingRecord.Password = record.Password;
            existingRecord.IV = record.IV;
            existingRecord.UpdatedAt = record.UpdatedAt;

            await _context.SaveChangesAsync();
            return existingRecord;
        }
    }
}
