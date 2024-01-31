﻿using Blog.Data;
using Blog.Models;
using Blog.ViewModels;
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
        try
        {
            var category = await context
            .Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(category);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, "05XE9 - Não foi possível buscar a categoria");
        }
        catch (FileNotFoundException ex)
        {
            return StatusCode(404, "05XE13 - Não foi possível buscar a categoria");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "05XE10 - Falha interna no servidor");
        }
    }

    [HttpPost("v1/categories")]
    public async Task<IActionResult> PostAsync(
        [FromServices] BlogDataContext context,
        [FromBody] CreateCategoryViewModel model)
    {
        try
        {
            var category = new Category
            {
                Id = 0,
                Name = model.Name,
                Slug = model.Name.ToLower()
            };
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return Created($"v1/categories/{category.Id}", category);
        }
        catch (DbUpdateException ex)
        {
            //colocar uma numeração de código para achar onde o exception ocorreu com mais facilidade
            return StatusCode(500, "05XE9 - Não foi possível incluir a categoria");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "05XE10 - Falha interna no servidor");
        }
    }

    [HttpPut("v1/categories/{id:int}")]
    public async Task<IActionResult> PutAsync(
        [FromServices] BlogDataContext context,
        [FromRoute] int id,
        CreateCategoryViewModel model)
    {
        try
        {
            var category = await context
            .Categories
            .FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound();

            category.Name = model.Name ?? category.Name;
            category.Slug = model.Slug == "" ? category.Name.ToLower() : model.Slug;

            context.Categories.Update(category);
            await context.SaveChangesAsync();
            return Ok(category);
        }
        catch (DbUpdateException ex)
        {
            //colocar uma numeração de código para achar onde o exception ocorreu com mais facilidade
            return StatusCode(500, "05XE14 - Não foi possível atualizar a categoria");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "05XE15 - Falha interna no servidor");
        }
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