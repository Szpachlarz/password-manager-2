using PasswordManager2.Models.Password;
using PasswordManager2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager2.Mappers
{
    public static class PasswordMapper
    {
        public static PasswordEntry ToPasswordEntry(this PasswordDto dto)
        {
            return new PasswordEntry
            {
                Id = dto.Id,
                Title = dto.Title,
                Username = dto.Username,
                Password = dto.Password,
                Website = dto.Website,
                LastModified = dto.LastModified
            };
        }

        public static PasswordDto ToPasswordDto(this PasswordEntry entry)
        {
            return new PasswordDto
            {
                Id = entry.Id,
                Title = entry.Title,
                Username = entry.Username,
                Password = entry.Password,
                Website = entry.Website,
                LastModified = entry.LastModified
            };
        }
    }
}
