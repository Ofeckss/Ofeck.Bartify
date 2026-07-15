using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ofeck.Bartify.APIEndpoints.Auth;
using Ofeck.Bartify.Core.Usuarios;
using Ofeck.Bartify.Core.Usuarios.DTOs;

namespace Ofeck.Bartify.APIEndpoints.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController: ControllerBase
{
    private readonly UsuarioService usuarioService;
    public AuthController(UsuarioService usuarioService)
    {
        this.usuarioService = usuarioService;
    }
    
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (idClaim is null)
            return Unauthorized();
        
        var id = Guid.Parse(idClaim);
        
        var account = await this.usuarioService.GetById(id);
        
        DateOnly? fecha = account.FechaNacimiento is DateTime dt
            ? DateOnly.FromDateTime(dt)
            : null;
        
        return this.Ok(new {
            id = account.Id,
            nombre = account.Nombre,
            apellido = account.Apellido,
            fecha_nacimiento = fecha,
            numero_cel = account.NumeroCel,
            rol = account.Rol,
            activo = account.Activo
        });
    }
}