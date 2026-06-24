using System.Data;
using Dapper;
using Ofeck.Bartify.Core.Estados;
using Ofeck.Bartify.Core.Models;

namespace Ofeck.Bartify.DB;

public class EstadoRepository(IDbConnection db): IEstadoRepository
{
    public async Task<List<Estado>> GetAll()
    {
        var sql = """
                select id as Id, nombre as Nombre from estados
            """;

        var result = await db.QueryAsync<Estado>(sql);
        
        return result.ToList();
    }
}