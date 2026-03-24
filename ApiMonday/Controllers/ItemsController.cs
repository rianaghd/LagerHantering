using Microsoft.AspNetCore.Mvc;
using ApiMonday.DTOs.Item;
using ApiMonday.Exceptions;
using ApiMonday.Services.Interfaces;

namespace ApiMonday.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IItemService _service;
    public ItemsController(IItemService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try   { return Ok(await _service.GetByIdAsync(id)); }
        catch (NotFoundException ex) { return NotFound(new { error = ex.Message }); }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateItemDto dto)
    {
        try
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (Exception ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateItemDto dto)
    {
        try   { return Ok(await _service.UpdateAsync(id, dto)); }
        catch (NotFoundException ex) { return NotFound(new { error = ex.Message }); }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try   { await _service.DeleteAsync(id); return NoContent(); }
        catch (NotFoundException ex) { return NotFound(new { error = ex.Message }); }
    }

    // --- Specialendpoints ---

    [HttpGet("expiring")]
    public async Task<IActionResult> GetExpiring([FromQuery] int days = 7) =>
        Ok(await _service.GetExpiringSoonAsync(days));

    [HttpGet("by-category/{categoryId}")]
    public async Task<IActionResult> GetByCategory(int categoryId) =>
        Ok(await _service.GetByCategoryAsync(categoryId));

    [HttpDelete("cleanup/expired")]
    public async Task<IActionResult> DeleteExpired()
    {
        var count = await _service.DeleteExpiredAsync();
        return Ok(new { message = $"{count} utgångna varor raderades." });
    }
}