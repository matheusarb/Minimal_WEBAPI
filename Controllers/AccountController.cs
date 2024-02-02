using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.Services;
using Blog.ViewModels;
using Blog.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;
using System.Text.RegularExpressions;

namespace Blog.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
    private readonly TokenService _tokenService;
    private readonly BlogDataContext _context;

    public AccountController(TokenService tokenService, BlogDataContext context)
    {
        _tokenService = tokenService;
        _context = context;
    }

    [HttpPost("v1/accounts/")]
    public async Task<IActionResult> Post(
        [FromBody]RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var newUser = new User
        {
            Name = model.Name,
            Email = model.Email,
            Slug = model.Email.Replace("@", "-").Replace(".", "-")
        };

        var password = PasswordGenerator.Generate(25, true, false);
        newUser.PasswordHash = PasswordHasher.Hash(password);

        try
        {
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
        
            return Ok(new ResultViewModel<dynamic>(new
            {
                newUser = newUser.Email,
                password
            }));
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(400, new ResultViewModel<string>("05XE20 - Este email já está cadastrado"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<User>("05XE21 - Falha interna do servidor."));
        }
    }

    [HttpPost("v1/accounts/login")]
    public async Task<IActionResult> Login(
        [FromBody]LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var user = await _context
            .Users
            .AsNoTracking()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Email == model.Email);
        
        if (user == null)
            return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos."));
        if(!PasswordHasher.Verify(user.PasswordHash, model.Password))
            return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos."));

        try
        {
            var token = _tokenService.GenerateToken(user);
            return Ok(new ResultViewModel<string>(token, null));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("05X23 - Falha interna no servidor"));
        }     
    }

    [Authorize]
    [HttpPost("v1/accounts/upload-image")]
    public async Task<IActionResult> UploadProfileImage(
        [FromBody] UploadImageViewModel model)
    {
        var fileName = $"{Guid.NewGuid().ToString()}.jpg";
        var data = new Regex(@"^data:image\/[a-z]+;base64, ")
            .Replace(model.Base64Image, "");
        var bytes = Convert.FromBase64String(data);

        try
        {
            await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{fileName}", bytes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<string>("05X30 - Falha interna no servidor"));
        }

        var user = await _context.
            Users
            .FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

        if (user == null)
            return NotFound(new ResultViewModel<User>("User not found."));

        user.Image = $"https://localhost:0000/images/{fileName}";

        try
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {

            throw;
        }
    }
}
