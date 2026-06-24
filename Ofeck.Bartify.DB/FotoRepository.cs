using System.Data;
using Dapper;
using Ofeck.Bartify.Core.Fotos;
using Ofeck.Bartify.Core.Models;

namespace Ofeck.Bartify.DB;

public class FotoRepository(IDbConnection db): IFotoRepository
{
    public async Task Upload(Foto foto)
    {
        var sql = """
            INSERT INTO fotos (id, url, articulo, orden)
            VALUES (@Id, @Url, @ArticuloId, @Orden)
            """;

        await db.ExecuteAsync(sql, foto);
    }

    public async Task<List<Foto>> GetByArticulo(Guid articuloId)
    {
        var sql = """
            SELECT id, url, articulo_id AS ArticuloId, orden
            FROM fotos
            WHERE articulo_id = @ArticuloId
            ORDER BY orden
            """;

        var result = await db.QueryAsync<Foto>(sql, new { ArticuloId = articuloId });
        return result.ToList();
    }
}