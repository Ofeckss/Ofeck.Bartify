using System.Text.RegularExpressions;
using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Usuarios.Requests;
using Ofeck.Bartify.Core.Usuarios.DTOs;

namespace Ofeck.Bartify.Core.Usuarios;

public class UsuarioService
{
    private readonly IUsuarioRepository repository;
    public UsuarioService(IUsuarioRepository repository)
    {
        this.repository = repository;
    }

    public async Task<bool> Register(RegisterUserRequest request)
    {
        if(!Regex.IsMatch(request.Nombre, @"^\p{L}+(?:[-' ]\p{L}+)*$"))
            throw new ArgumentException("Nombre inválido.", nameof(request.Nombre));
        
        if(!Regex.IsMatch(request.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("Correo inválido.", nameof(request.Email));
        
        //if (!Regex.IsMatch(request.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$"))
        //  throw new ArgumentException("Invalid password.", nameof(request.Password));
        
        var emailExist = await this.repository.ExistByEmail(request.Email);
        
        if (emailExist)
            return false;
        
        var hashedPass = BCrypt.Net.BCrypt.HashPassword(request.Password);
        
        var usuario = new Usuario(
            Guid.CreateVersion7(),
            request.Nombre,
            null,
            null,
            request.Email,
            null,
            null,
            hashedPass,
            true,
            null,
            DateTime.Now
        );
        
        await this.repository.Create(usuario);
        return true;
    }

    public async Task<Token> Login(LoginUserRequest request)
    {
        var account = await this.repository.GetByEmail(request.Email);

        if (account == null) throw new ArgumentException("Cuenta no existe.", request.Email);
        
        if (account.Activo == false) throw new ArgumentException("Cuenta desactivada.", request.Email);

        var validPass = BCrypt.Net.BCrypt.Verify(request.Password, account.Password);
        
        if(!validPass)
            throw new UnauthorizedAccessException("Credenciales inválidas.");

        return new Token {
            Id = account.Id,
            Email = account.Email,
        };
    }

    public async Task<UsuarioDTO> GetById(Guid id)
    {
        var usuario = await this.repository.GetById(id);

        if (usuario == null) throw new KeyNotFoundException("Cuenta no existe.");

        return usuario;
    }
}