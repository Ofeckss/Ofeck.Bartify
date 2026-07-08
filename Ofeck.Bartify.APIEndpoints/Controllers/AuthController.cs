using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ofeck.Bartify.Core.Usuarios.DTOs;

namespace Ofeck.Bartify.APIEndpoints.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController: ControllerBase
{
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (idClaim is null)
            return Unauthorized();
        
        var id = Guid.Parse(idClaim);
        var email = User.FindFirstValue(ClaimTypes.Email);
        var nombre = User.FindFirstValue(ClaimTypes.Name);

        var info = new Token(
            id,
            nombre,
            email
        );
        
        return this.Ok(info);
    }
}
