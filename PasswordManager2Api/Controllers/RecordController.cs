using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager2Api.Dtos;
using PasswordManager2Api.Interfaces;
using PasswordManager2Api.Models;
using System.Security.Claims;

namespace PasswordManager2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordController : ControllerBase
    {
        private readonly IRecordRepository _recordRepository;

        public RecordController(IRecordRepository recordRepository)
        {
            _recordRepository = recordRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetRecordById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var record = await _recordRepository.GetRecordById(id);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(record);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserRecords(string userId)
        {
            var records = await _recordRepository.GetUserRecords(userId);
            return Ok(records);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RecordDto recordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var createdRecord = await _recordRepository.Create(userId, recordDto);
            return CreatedAtAction(nameof(GetRecordById), new { id = createdRecord.Id }, createdRecord);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RecordDto recordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingRecord = await _recordRepository.GetRecordById(id);

            var updatedRecord = await _recordRepository.Update(id, userId, recordDto);
            if (updatedRecord == null)
                return NotFound();

            return Ok(updatedRecord);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var deletedRecord = await _recordRepository.Delete(id, userId);
            if (deletedRecord == null)
            {
                return NotFound();
            }

            return Ok(deletedRecord);
        }
    }
}
