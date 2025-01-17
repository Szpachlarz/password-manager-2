using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PasswordManager2Api.Interfaces;
using PasswordManager2Api.Models;

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
        public async Task<IActionResult> Create([FromBody] Record record)
        {
            if (record == null)
            {
                return BadRequest("Record is null.");
            }

            var createdRecord = await _recordRepository.Create(record);
            return CreatedAtAction(nameof(GetRecordById), new { id = createdRecord.Id }, createdRecord);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Record record)
        {
            if (record == null || id != record.Id)
            {
                return BadRequest("Invalid record data.");
            }

            var updatedRecord = await _recordRepository.Update(id, record);
            if (updatedRecord == null)
            {
                return NotFound();
            }

            return Ok(updatedRecord);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedRecord = await _recordRepository.Delete(id);
            if (deletedRecord == null)
            {
                return NotFound();
            }

            return Ok(deletedRecord);
        }
    }
}
