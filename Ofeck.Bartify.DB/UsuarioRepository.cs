using System.Data;
using Dapper;
using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Usuarios;
using Ofeck.Bartify.Core.Usuarios.DTOs;

namespace Ofeck.Bartify.DB;

public class UsuarioRepository(IDbConnection db): IUsuarioRepository
{
    public async Task Create(Usuario usuario)
    {
        var sql = """
                insert into usuarios(id,nombre,correo,password,activo,created_at)
                values (
                    @Id,
                    @Nombre,
                    @Email,
                    @Password,
                    @Activo,
                    @FechaCreacion
                    )
            """;
        
            await db.ExecuteAsync(sql, usuario);
    }
    
    public async Task<bool> Update(Usuario usuario) {
        throw new NotImplementedException();
    }

    public async Task<bool> Delete(Guid id)
    {
        var sql = """
                update usuarios set activo = false where id = @Id
            """;
        
        var succesfulDelete = await db.ExecuteAsync(sql, new { Id = id });
        return succesfulDelete > 0;
    }

    public async Task<UsuarioDTO> GetById(Guid id)
    {
        var sql = """
                select id as Id, nombre as Nombre, apellido as Apellido, fecha_nacimiento as FechaNacimiento, numero_cel as NumeroCel, rating as Rating, activo as Activo
                from usuarios
                where id = @Id
            """;
        
        return await db.QuerySingleOrDefaultAsync<UsuarioDTO>(sql, new { Id = id });
    }

    public async Task<bool> ExistByEmail(string email) {
        
        var sql = """
                select exists(select 1 from usuarios where correo = @Email)
            """;

        return await db.ExecuteScalarAsync<bool>(sql, new { Email = email});
    }

    public async Task<LoginDTO> GetByEmail(string email)
    {
        var sql = """
                select id as Id, nombre as Nombre, correo as Email, password, activo as Activo from usuarios where correo = @Email
            """;
        
        return await db.QuerySingleOrDefaultAsync<LoginDTO>(sql, new { Email = email });
    }
}
