using Microsoft.AspNetCore.Mvc;
using Ofeck.Bartify.APIEndpoints.Auth;
using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Usuarios;
using Ofeck.Bartify.Core.Usuarios.Requests;
using Ofeck.Bartify.APIEndpoints.Auth;

namespace Ofeck.Bartify.APIEndpoints.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsuarioController: ControllerBase
{
    private readonly UsuarioService usuarioService;
    private readonly ITokenService tokenService;
    public UsuarioController(UsuarioService usuarioService,  ITokenService TokenService)
    {
        this.usuarioService = usuarioService;
        this.tokenService = TokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterUsuario([FromBody] RegisterUserRequest request)
    {
        try
        {
            var newEmail = await this.usuarioService.Register(request);

            if (!newEmail) return this.Conflict("Email ya está registrado.");

            return this.Created();
        } catch (ArgumentException ae)
        {
            return this.UnprocessableEntity(ae.Message);
        } catch (Exception e)
        {
            return this.Problem(e.StackTrace, title: "Ha ocurrido un error inesperado.");
        }
    }
    
    [HttpPost("login")]
    public async Task<ActionResult> LoginUsuario([FromBody] LoginUserRequest request) 
    {
        try
        {
            var login = await this.usuarioService.Login(request);
            var token = this.tokenService.CreateToken(login.Id, login.Email, login.Nombre);

            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            });

            return this.Ok(new { id = login.Id, nombre = login.Nombre, email = login.Email });
        } 
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Contraseña incorrecta");
        }
        catch (ArgumentException ae)
        {
            return this.UnprocessableEntity(ae.Message);
        } 
        catch (Exception e)
        {
            return this.Problem(e.StackTrace, title: "Ha ocurrido un error inesperado.");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var usuario = await this.usuarioService.GetById(id);

            return this.Ok(usuario);
        } catch (KeyNotFoundException knfe)
        {
            return this.NotFound(knfe.Message);
        } catch (Exception e)
        {
            return this.Problem(e.StackTrace, title: "Ha ocurrido un error inesperado.");
        }
    }
}