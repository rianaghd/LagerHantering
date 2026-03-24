using Microsoft.AspNetCore.Mvc;
using ApiMonday.DTOs.Category;
using ApiMonday.Exceptions;
using ApiMonday.Services.Interfaces;

namespace ApiMonday.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _service;
    public CategoriesController(ICategoryService service) => _service = service;

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
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
    {
        try   { return Ok(await _service.CreateAsync(dto)); }
        catch (Exception ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto dto)
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
}