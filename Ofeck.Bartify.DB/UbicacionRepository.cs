using System.Data;
using Dapper;
using Ofeck.Bartify.Core.Ubicaciones;

namespace Ofeck.Bartify.DB;

public class UbicacionRepository(IDbConnection db): IUbicacionRepository
{
    public async Task<List<string>> GetAll()
    {
        var sql = """
                select nombre as Nombre from ubicaciones
            """;

        var result = await db.QueryAsync<string>(sql);

        return result.ToList();
    }
}