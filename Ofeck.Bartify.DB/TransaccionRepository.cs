using System.Data;
using Dapper;
using Ofeck.Bartify.Core.Articulos.DTOs;
using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Transacciones;
using Ofeck.Bartify.Core.Transacciones.Responses;
using Ofeck.Bartify.Core.Usuarios.DTOs;

namespace Ofeck.Bartify.DB;

public class TransaccionRepository(IDbConnection db) : ITransaccionRepository
{
    public async Task CreateTransaccion(Transaccion transaccion)
    {
        var sql = """
                insert into transacciones(id,chat_id,trueque,precio_final,created_at,updated_at,confirmado_comprador,confirmado_vendedor)
                values(
                    @Id,
                    @ChatId,
                    @EsTrueque,
                    @PrecioFinal,
                    @CreatedAt,
                    @UpdatedAt,
                    @ConfirmadoComprador,
                    @ConfirmadoVendedor
                )
            """;
        
        await db.ExecuteAsync(sql, transaccion);
    }
    public async Task CreateDetalle(DetalleTransaccion detalleTransaccion)
    {
        var sql = """
                insert into detalles_transaccion(id,transaccion_id,articulo_id,ofrecido_vendedor,final)
                values(
                       @Id,
                       @TransaccionId,
                       @ArticuloId,
                       @OfrecidoVendedor,
                       @Final
                    )
            """;
        
        await db.ExecuteAsync(sql, detalleTransaccion);
    }
    public async Task<Guid> GetTransId(Guid chatId)
    {
        var sql = """
                select id from transacciones where chat_id = @ChatId
            """;
        
        return await db.QuerySingleAsync<Guid>(sql,new {ChatId = chatId.ToString()});
    }
    public async Task<bool> EsVendedor(Guid chatId, Guid usuarioId)
    {
        var sql = """
                select exists(select 1 from chats where id = @ChatId and vendedor_id = @VendedorId)
            """;
        
        return await db.ExecuteScalarAsync<bool>(sql, new {ChatId = chatId.ToString(), VendedorId = usuarioId.ToString()});
    }
    public async Task ConfirmarComprador(Guid chatId, double? precio)
    {
        var sql = """
                update transacciones set confirmado_comprador = 1, precio_final = COALESCE(@Precio, precio_final), updated_at = NOW() where chat_id = @ChatId
            """;
        
        await db.ExecuteAsync(sql, new { ChatId = chatId.ToString(), Precio = precio });
    }

    public async Task ConfirmarDetComprador(Guid chatId, Guid articuloId)
    {
        var sql = """
                update detalles_transaccion dt inner join transacciones t on t.id = dt.transaccion_id
                set dt.final = 1 
                where t.chat_id = @ChatId and dt.articulo_id = @ArticuloId and dt.ofrecido_vendedor = 0
            """;
        
        await db.ExecuteAsync(sql, new { ChatId = chatId.ToString(), ArticuloId = articuloId.ToString() });
    }

    public async Task ConfirmarVendedor(Guid chatId)
    {
        var sql = """
                update transacciones set confirmado_vendedor = 1, updated_at = NOW() where chat_id = @ChatId
            """;
        
        await db.ExecuteAsync(sql, new { ChatId = chatId.ToString() });
    }

    public async Task ConfirmarDetVendedor(Guid chatId, Guid articuloId)
    {
        var sql = """
                update detalles_transaccion dt inner join transacciones t on t.id = dt.transaccion_id
                set dt.final = 1
                where t.chat_id = @ChatId and dt.articulo_id = @ArticuloId and ofrecido_vendedor = 1
            """;
        
        await db.ExecuteAsync(sql, new { ChatId = chatId.ToString(), ArticuloId = articuloId.ToString() });
    }
    
    public async Task RevertirComprador(Guid chatId)
    {
        var sql = """
                update transacciones set confirmado_comprador = 0, updated_at = NOW() where chat_id = @ChatId
            """;
        
        await db.ExecuteAsync(sql, new { ChatId = chatId.ToString() });
    }
    public async Task RevertirDetComprador(Guid chatId)
    {
        var sql = """
                update detalles_transaccion dt inner join transacciones t on t.id = dt.transaccion_id
                set dt.final = 0
                where t.chat_id = @ChatId and ofrecido_vendedor = 0
            """;
        
        await db.ExecuteAsync(sql, new { ChatId = chatId.ToString() });
    }
    public async Task RevertirVendedor(Guid chatId)
    {
        var sql = """
                update transacciones set confirmado_vendedor = 0, updated_at = NOW() where chat_id = @ChatId
            """;
        
        await db.ExecuteAsync(sql, new { ChatId = chatId.ToString() });
    }

    public async Task RevertirDetVendedor(Guid chatId)
    {
        var sql = """
                update detalles_transaccion dt inner join transacciones t on t.id = dt.transaccion_id
                set dt.final = 0
                where t.chat_id = @ChatId and ofrecido_vendedor = 1
            """;
        
        await db.ExecuteAsync(sql, new { ChatId = chatId.ToString() });
    }
    public async Task<List<GetDetalleResponse>> GetDetalle(Guid chatId)
    {
        var sql = """
                select dt.id as Id, dt.ofrecido_vendedor as OfrecidoVendedor, a.id as Id, a.nombre as Nombre, f.url as Url
                from  detalles_transaccion dt
                inner join transacciones t on t.id = dt.transaccion_id
                inner join articulos a on a.id = dt.articulo_id
                left join fotos f on f.articulo = a.id and orden = 0
                where t.chat_id = @ChatId
                order by dt.ofrecido_vendedor
            """;
        
        var detalles = await db.QueryAsync<GetDetalleResponse, MiniArticuloDTO, GetDetalleResponse>(sql,
            (detalle, articulo) =>
            {
                return detalle with {
                    Articulo = articulo
                };
            },
            param: new { ChatId = chatId.ToString() },
            splitOn: "Id"
        );
        
        return detalles.ToList();
    }
    public async Task<List<GetAllTransaccionesResponse>> GetAll(Guid usuarioId)
    {
        const string sql = """
            SELECT 
                t.id                AS Id,
                t.trueque           AS EsTrueque,
                t.precio_final      AS PrecioFinal,
                t.created_at        AS CreatedAt,
                a.nombre            AS NombreMainArticulo,
                f.url               AS UrlMainArticulo,
                u.id                AS Id,
                u.nombre            AS Nombre,
                u.apellido          AS Apellido,
                u.fecha_nacimiento  AS FechaNacimiento,
                u.numero_cel        AS NumeroCel,
                u.rating            AS Rating,
                u.activo            AS Activo
            FROM transacciones t
            INNER JOIN chats c      ON c.id = t.chat_id
            LEFT JOIN articulos a   ON a.id = c.articulo_principal
            LEFT JOIN fotos f       ON f.articulo = a.id AND f.orden = 0
            LEFT JOIN usuarios u    ON u.id = CASE
                                        WHEN c.comprador_id = @UsuarioId THEN c.vendedor_id
                                        ELSE c.comprador_id
                                      END
            WHERE c.comprador_id = @UsuarioId OR c.vendedor_id = @UsuarioId
            ORDER BY t.created_at DESC
            """;

        var result = await db.QueryAsync<GetAllTransaccionesResponse, UsuarioDTO, GetAllTransaccionesResponse>(
            sql,
            (transaccion, otroUsuario) => transaccion with { OtroUsuario = otroUsuario },
            param: new { UsuarioId = usuarioId.ToString() },
            splitOn: "Id"
        );

        return result.ToList();
    }
    public async Task<GetMiniTransaccionResponse> GetSemiById(Guid chatId)
    {
        var sql = """
                select id as Id, chat_id as ChatId, trueque as EsTrueque, precio_final as PrecioFinal, created_at as CreatedAt, updated_at as UpdatedAt
                from transacciones 
                where chat_id = @ChatId
            """;
        
        var result = await db.QuerySingleOrDefaultAsync<GetMiniTransaccionResponse>(sql, new { ChatId = chatId.ToString() });
        return result;
    }

    public async Task<List<MiniArticuloDTO>> GetListById(Guid transaccionId, bool esVendedor)
    {
        var sql = """
                select d.articulo_id as Id, a.nombre as Nombre, f.url as Url
                from detalles_transaccion d 
                inner join articulos a on a.id = d.articulo_id
                left join fotos f on f.articulo = a.id and f.orden = 0
                where d.transaccion_id = @TransaccionId and ofrecido_vendedor = @EsVendedor
            """;
        
        var result = await db.QueryAsync<MiniArticuloDTO>(sql, new { TransaccionId = transaccionId.ToString(), EsVendedor    = esVendedor });

        return result.ToList();
    }
    public async Task<bool> GetStatus(Guid chatId)
    {
        var sql = """
                select confirmado_vendedor as ConfirmadoV, confirmado_comprador as ConfirmadoC
                from transacciones
                where  chat_id = @ChatId
            """;
        
        var checker = await db.QuerySingleOrDefaultAsync<Confirmador>(sql, new { ChatId = chatId.ToString() });

        return checker.ConfirmadoC && checker.ConfirmadoV;
    }
}
