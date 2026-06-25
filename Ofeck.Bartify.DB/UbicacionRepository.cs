using System.Data;
using Dapper;
using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Ubicaciones;

namespace Ofeck.Bartify.DB;

public class UbicacionRepository(IDbConnection db): IUbicacionRepository
{
    public async Task<List<Ubicacion>> GetAll()
    {
        var sql = """
                select id as Id, nombre as Nombre from ubicaciones
            """;

        var result = await db.QueryAsync<Ubicacion>(sql);

        return result.ToList();
    }
}