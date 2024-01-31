using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
public class CategoryController : ControllerBase
{
    [HttpGet("v1/categories")]
    public async Task<IActionResult> GetAsync(
        [FromServices]BlogDataContext context)
    {
        var categories = await context
            .Categories
            .AsNoTracking()
            .ToListAsync();

        return Ok(categories);
    }

    [HttpGet("v1/categories/{id:int}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromServices]BlogDataContext context,
        [FromRoute]int id)
    {
        var category = await context
            .Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (category == null)
            return NotFound();

        return Ok(category);
    }

    [HttpPost("v1/categories")]
    public async Task<IActionResult> PostAsync(
        [FromServices] BlogDataContext context,
        [FromBody] Category model)
    {
        try
        {
            await context.Categories.AddAsync(model);
            await context.SaveChangesAsync();

            return Created($"v1/categories/{model.Id}", model);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, "Não foi possível incluir a categoria");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Falha interna no servidor");
        }
    }

    [HttpPut("v1/categories/{id:int}")]
    public async Task<IActionResult> PutAsync(
        [FromServices] BlogDataContext context,
        [FromRoute] int id,
        Category model)
    {
        var category = await context
            .Categories
            .FirstOrDefaultAsync(x => x.Id == id);
        if(category==null)
            return NotFound();

        category.Name = model.Name ?? category.Name;
        category.Slug = model.Slug ?? category.Slug;

        context.Categories.Update(category);
        await context.SaveChangesAsync();

        return Ok(category);
    }

    [HttpDelete("v1/categories/{id:int}")]
    public async Task<IActionResult> DeleteAsync(
        [FromServices] BlogDataContext context,
        [FromRoute] int id)
    {
        var category = await context
            .Categories
            .FirstOrDefaultAsync(x => x.Id == id);
        if (category == null)
            return NotFound();

        context.Categories.Remove(category);
        await context.SaveChangesAsync();

        return Ok(category);
    }
}