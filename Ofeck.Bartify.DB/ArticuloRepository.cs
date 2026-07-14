using System.Data;
using Dapper;
using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Articulos;
using Ofeck.Bartify.Core.Articulos.DTOs;
using Ofeck.Bartify.Core.Articulos.Requests;

namespace Ofeck.Bartify.DB;

public class ArticuloRepository(IDbConnection db): IArticuloRepository
{
    public async Task Create<Articulo>(Articulo articulo)
    {
        var sql = """
                insert into articulos(id,nombre,descripcion,precio,vendedor_id,categoria_id,trueque,estado_id,ubicacion_id,disponible,created_at,updated_at)
                values(
                    @Id,
                    @Nombre,
                    @Descripcion,
                    @Precio,
                    @VendedorId,
                    @CategoriaId,
                    @EsTrueque,
                    @EstadoId,
                    @UbicacionId,
                    @Disponible,
                    @CreatedAt,
                    @UpdatedAt
                    )
            """;

        await db.ExecuteAsync(sql, articulo);
    }
    public async Task<bool> Update(UpdateArticuloRequest request, Guid id)
    {
        var sql = """
                update articulos
                set
                    nombre = coalesce(@Nombre, nombre),
                    descripcion = coalesce(@Descripcion, descripcion),
                    precio = coalesce(@Precio, precio),
                    trueque = coalesce(@EsTrueque, trueque),
                    categoria_id = coalesce(@CategoriaId, categoria_id),
                    ubicacion_id = coalesce(@UbicacionId, ubicacion_id),
                    estado_id = coalesce(@EstadoId, estado_id)
                where id = @Id
            """;
        
        var parammes = new
        {
            request.Nombre,
            request.Descripcion,
            request.Precio,
            request.EsTrueque,
            request.CategoriaId,
            request.UbicacionId,
            request.EstadoId,
            Id = id.ToString()
        };
        
        var affected = await db.ExecuteAsync(sql, parammes);
        return affected > 0;
    }
    public async Task<bool> Delete(Guid Id)
    {
        var sql = """
                update articulos set activo = 0 where id = @Id
            """;
        
        var affected = await db.ExecuteAsync(sql, new { Id = Id.ToString() });
        return affected > 0;
    }
    public async Task<List<GetArticuloDto>> GetAll()
    {
        var sql = """
                SELECT 
                    a.id            AS Id,
                    a.nombre        AS Nombre,
                    a.descripcion   AS Descripcion,
                    a.precio        AS Precio,
                    a.trueque       AS EsTrueque,
                    a.disponible    AS Disponible,
                    a.created_at    AS CreatedAt,
                    f.url           AS Url,
                    ub.id           AS Id,
                    ub.nombre       AS Nombre,
                    c.id            AS Id,
                    c.nombre        AS Nombre,
                    u.id            AS VendedorId,
                    u.nombre        AS Nombre,
                    u.apellido      AS Apellido
                FROM articulos a
                LEFT JOIN categorias c   ON a.categoria_id  = c.id
                LEFT JOIN usuarios u     ON a.vendedor_id   = u.id
                LEFT JOIN ubicaciones ub ON a.ubicacion_id  = ub.id
                LEFT JOIN fotos f        ON a.id = f.articulo AND f.orden = 0
                ORDER BY a.created_at DESC
            """;

        var articulos =
            await db.QueryAsync<GetArticuloDto, UbicacionArticulo, CategoriaArticulo, Vendedor, GetArticuloDto>(
                sql,
                (articulo, ubicacion, categoria, vendedor) =>
                {
                    return articulo with {
                        Ubicacion = ubicacion,
                        Categoria = categoria,
                        Vendedor = vendedor
                    };
                },
                splitOn: "Id,Id,VendedorId"
            );

        return articulos.ToList();
    }

    public async Task<GetArticuloByIdDto> GetById(Guid id)
    {
        var sql = """
                SELECT 
                    a.id            AS Id,
                    a.nombre        AS Nombre,
                    a.descripcion   AS Descripcion,
                    a.precio        AS Precio,
                    a.trueque       AS EsTrueque,
                    a.disponible    AS Disponible,
                    a.created_at    AS CreatedAt,
                    ub.id           AS Id,
                    ub.nombre       AS Nombre,
                    c.id            AS Id,
                    c.nombre        AS Nombre,
                    u.id            AS VendedorId,
                    u.nombre        AS Nombre,
                    u.apellido      AS Apellido
                FROM articulos a
                LEFT JOIN categorias c   ON a.categoria_id  = c.id
                LEFT JOIN usuarios u     ON a.vendedor_id   = u.id
                LEFT JOIN ubicaciones ub ON a.ubicacion_id  = ub.id
                WHERE a.id = @Id
            """;

        var articulos =
            await db.QueryAsync<GetArticuloByIdDto, UbicacionArticulo, CategoriaArticulo, Vendedor, GetArticuloByIdDto>(
                sql,
                (articulo, ubicacion, categoria, vendedor) =>
                {
                    return articulo with {
                        Ubicacion = ubicacion,
                        Categoria = categoria,
                        Vendedor = vendedor
                    };
                },
                param: new { Id = id.ToString() },
                splitOn: "Id,Id,VendedorId"
            );

        return articulos.FirstOrDefault();
    }

    public async Task<List<GetArticuloDto>> GetByUserId(Guid userId)
    {
        var sql = """
                SELECT 
                a.id            AS Id,
                a.nombre        AS Nombre,
                a.descripcion   AS Descripcion,
                a.precio        AS Precio,
                a.trueque       AS EsTrueque,
                a.disponible    AS Disponible,
                a.created_at    AS CreatedAt,
                f.url           AS Url,
                ub.id           AS Id,
                ub.nombre       AS Nombre,
                c.id            AS Id,
                c.nombre        AS Nombre,
                u.id            AS VendedorId,
                u.nombre        AS Nombre,
                u.apellido      AS Apellido
            FROM articulos a
            LEFT JOIN categorias c   ON a.categoria_id  = c.id
            LEFT JOIN usuarios u     ON a.vendedor_id   = u.id
            LEFT JOIN ubicaciones ub ON a.ubicacion_id  = ub.id
            LEFT JOIN fotos f        ON a.id = f.articulo AND f.orden = 0
            WHERE u.id = @VendedorId
            ORDER BY a.created_at DESC
            """;

        var articulos =
            await db.QueryAsync<GetArticuloDto, UbicacionArticulo, CategoriaArticulo, Vendedor, GetArticuloDto>(
                sql,
                (articulo, ubicacion, categoria, vendedor) =>
                {
                    return articulo with {
                        Ubicacion = ubicacion,
                        Categoria = categoria,
                        Vendedor = vendedor
                    };
                },
                param: new { VendedorId = userId.ToString() },
                splitOn: "Id,Id,VendedorId"
            );

        return articulos.ToList();
    }

    public async Task<List<GetArticuloDto>> GetFiltered(GetFilteredRequest request)
    {
         var sql = """
            SELECT 
                a.id            AS Id,
                a.nombre        AS Nombre,
                a.descripcion   AS Descripcion,
                a.precio        AS Precio,
                a.trueque       AS EsTrueque,
                a.disponible    AS Disponible,
                a.created_at    AS CreatedAt,
                f.url           AS Url,
                ub.id           AS Id,
                ub.nombre       AS Nombre,
                c.id            AS Id,
                c.nombre        AS Nombre,
                u.id            AS VendedorId,
                u.nombre        AS Nombre,
                u.apellido      AS Apellido
            FROM articulos a
            LEFT JOIN categorias c   ON a.categoria_id  = c.id
            LEFT JOIN usuarios u     ON a.vendedor_id   = u.id
            LEFT JOIN ubicaciones ub ON a.ubicacion_id  = ub.id
            LEFT JOIN fotos f        ON a.id = f.articulo AND f.orden = 0
            WHERE (@Nombre IS NULL OR a.nombre LIKE CONCAT('%', @Nombre, '%'))
              AND (@PrecioMin IS NULL OR a.precio >= @PrecioMin)
              AND (@PrecioMax IS NULL OR a.precio <= @PrecioMax)
              AND (@CategoriaId IS NULL OR a.categoria_id = @CategoriaId)
              AND (@EsTrueque IS NULL OR a.trueque = @EsTrueque)
              AND (@UbicacionId IS NULL OR a.ubicacion_id = @UbicacionId)
              AND (@EstadoId IS NULL OR a.estado_id = @EstadoId)
            ORDER BY a.created_at DESC
        """;

    var articulos =
        await db.QueryAsync<GetArticuloDto, UbicacionArticulo, CategoriaArticulo, Vendedor, GetArticuloDto>(
            sql,
            (articulo, ubicacion, categoria, vendedor) =>
            {
                return articulo with {
                    Ubicacion = ubicacion,
                    Categoria = categoria,
                    Vendedor = vendedor
                };
            },
            request,
            splitOn: "Id,Id,VendedorId"
        );

    return articulos.ToList();
    }
}