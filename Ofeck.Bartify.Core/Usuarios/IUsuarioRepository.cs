using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Usuarios.DTOs;
using Ofeck.Bartify.Core.Usuarios.Requests;

namespace Ofeck.Bartify.Core.Usuarios;

public interface IUsuarioRepository 
{
    public Task Create(Usuario user);
    public Task<bool> Update(UpdateUserRequest request, Guid Id);
    public Task<bool> Delete(Guid id);
    public Task<UsuarioDTO> GetById(Guid id);
    public Task<bool> ExistByEmail(string email);
    public Task<LoginDTO> GetByEmail(string email);
    public Task<bool> Rate(Guid id, double rating);
}