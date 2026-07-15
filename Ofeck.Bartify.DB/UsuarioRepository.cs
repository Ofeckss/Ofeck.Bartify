using System.Data;
using Dapper;
using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Usuarios;
using Ofeck.Bartify.Core.Usuarios.DTOs;
using Ofeck.Bartify.Core.Usuarios.Requests;

namespace Ofeck.Bartify.DB;

public class UsuarioRepository(IDbConnection db): IUsuarioRepository
{
    public async Task Create(Usuario usuario)
    {
        var sql = """
                insert into usuarios(id,nombre,correo,password,activo,created_at,rol_id)
                values (
                    @Id,
                    @Nombre,
                    @Email,
                    @Password,
                    @Activo,
                    @FechaCreacion,
                    @Rol
                    )
            """;
        
            await db.ExecuteAsync(sql, usuario);
    }

    public async Task<bool> Update(UpdateUserRequest request, Guid Id)
    {
        var sql = """
                update usuarios
                set
                    nombre = coalesce(@Nombre, nombre),
                    apellido = coalesce(@Apellido, apellido),
                    fecha_nacimiento = coalesce(@FechaNacimiento, fecha_nacimiento),
                    numero_cel = coalesce(@NumeroCel, numero_cel),
                    password = coalesce(@Password, password)
                where id = @Id
            """;

        var parammes = new {
            request.Nombre,
            request.Apellido,
            FechaNacimiento = request.FechaNacimiento?.ToDateTime(TimeOnly.MinValue),
            request.NumeroCel,
            request.Password,
            Id = Id.ToString()
        };
        
        var affected = await db.ExecuteAsync(sql, parammes);
        return affected > 0;
    }
    public async Task<bool> Delete(Guid id)
    {
        var sql = """
                update usuarios set activo = 0 where id = @Id
            """;
        
        var succesfulDelete = await db.ExecuteAsync(sql, new { Id = id.ToString() });
        return succesfulDelete > 0;
    }

    public async Task<UsuarioDTO> GetById(Guid id)
    {
        var sql = """
                select id as Id, nombre as Nombre, apellido as Apellido, fecha_nacimiento as FechaNacimiento, numero_cel as NumeroCel, rating as Rating, activo as Activo, rol as Rol
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
    try
    {
        var sql = """
                select id as Id, nombre as Nombre, apellido as Apellido, correo as Email, password as Password, fecha_nacimiento as FechaNacimiento, numero_cel as NumeroCel, rol_id as Rol, activo as Activo from usuarios where correo = @Email
            """;
        
        return await db.QuerySingleOrDefaultAsync<LoginDTO>(sql, new { Email = email });
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
        throw;
    }
}

    public async Task<bool> Rate(Guid id, double rating)
    {
        var sql = """
                update usuarios
                set
                    rating = ((rating * numero_ratings) + @Rating) / (numero_ratings + 1),
                    numero_ratings = numero_ratings + 1
                where id = @Id
            """;
        
        var affected = await db.ExecuteAsync(sql, new { Id = id.ToString(), Rating = rating });

        return affected > 0;
    }
}
