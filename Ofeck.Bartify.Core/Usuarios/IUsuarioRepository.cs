using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Usuarios.DTOs;

namespace Ofeck.Bartify.Core.Usuarios;

public interface IUsuarioRepository 
{
    public Task Create(Usuario user);
    public Task<bool> Update(Usuario user);
    public Task<bool> Delete(Guid id);
    public Task<UsuarioDTO> GetById(Guid id);
    public Task<bool> ExistByEmail(string email);
    public Task<LoginDTO> GetByEmail(string email);
}