using Microsoft.EntityFrameworkCore;
using PasswordManager2Api.Data;
using PasswordManager2Api.Dtos;
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

        public async Task<Record> Create(string userId, RecordDto recordDto)
        {
            var record = new Record
            {
                AccountId = userId,
                Title = recordDto.Title,
                Username = recordDto.Username,
                Website = recordDto.Website,
                Password = recordDto.Password,
                IV = recordDto.IV,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.Records.AddAsync(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<Record?> Delete(int id, string userId)
        {
            var record = await _context.Records.FirstOrDefaultAsync(x => x.Id == id);

            if (record == null || record.AccountId != userId)
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

        public async Task<List<Record>> GetUserRecords(string userId)
        {
            return await _context.Records.Where(i => i.AccountId == userId).ToListAsync();
        }

        public async Task<Record> Update(int id, string userId, RecordDto recordDto)
        {
            var existingRecord = await _context.Records.FirstOrDefaultAsync(i => i.Id == id);
            if (existingRecord == null || existingRecord.AccountId != userId)
            {
                return null;
            }

            existingRecord.Website = recordDto.Website;
            existingRecord.Password = recordDto.Password;
            existingRecord.IV = recordDto.IV;
            existingRecord.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingRecord;
        }
    }
}
