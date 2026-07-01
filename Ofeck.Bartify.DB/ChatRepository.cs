using System.Data;
using Dapper;
using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Chats;
using Ofeck.Bartify.Core.Chats.DTOs;

namespace Ofeck.Bartify.DB;

public class ChatRepository(IDbConnection db): IChatRepository
{
    public async Task Create(Chat chat)
    {
        var sql = """
            insert into chats(id, articulo_principal, comprador_id, vendedor_id, url, created_at) values(
                @Id,
                @ArticuloPrincipal,
                @Comprador,
                @Vendedor,
                @Url,
                @CreatedAt)
            """;
        await db.ExecuteAsync(sql, chat);
    }

    public async Task<string> ChatExists(Guid Id, Guid Articulo)
    {
        var sql = """
            select url as Url from chats where comprador_id = @Id and articulo_principal = @Articulo limit 1
            """;
        
        return await db.QueryFirstOrDefaultAsync<string>(sql, new {Id, Articulo});
    }
    public async Task<List<ChatDTO>> GetByUser(Guid id)
    {
        var sql = """
                select 
                chats.id as Id, 
                chats.articulo_principal as ArticuloPrincipal, 
                articulos.nombre as NombreArticulo,
                fotos.url as UrlArticulo,
                chats.url as Url
                from chats 
                inner join articulos on chats.articulo_principal = articulos.id
                inner join fotos on articulos.id = fotos.articulo and fotos.orden = 0
                where chats.comprador_id = @Id or chats.vendedor_id = @Id
            """;

        var chats = await db.QueryAsync<ChatDTO>(sql, new { Id = id });

        return chats.ToList();
    }
}