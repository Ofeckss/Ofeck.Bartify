using System.Data;
using Dapper;
using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Categorias;

namespace Ofeck.Bartify.DB;

public class CategoriaRepository(IDbConnection db): ICategoriaRepository
{
    public async Task<List<Categoria>> GetParents()
    {
        var sql = """
                select id as Id, nombre as Nombre, padre_id as PadreId from categorias where padre_id is null
            """;
        
        var result = await db.QueryAsync<Categoria>(sql);

        return result.ToList();
    }

    public async Task<List<Categoria>> GetSons(int id)
    {
        var sql = """
                select id as Id, nombre as Nombre, padre_id as PadreId from categorias where padre_id = @Id
            """;

        var result = await db.QueryAsync<Categoria>(sql,new { Id = id });

        return result.ToList();
    }
}