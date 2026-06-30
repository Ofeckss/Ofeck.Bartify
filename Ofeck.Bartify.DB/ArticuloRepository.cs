using System.Data;
using Dapper;
using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Articulos;
using Ofeck.Bartify.Core.Articulos.DTOs;

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

    public async Task<bool> Update(Articulo articulo)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Delete(Guid id)
    {
        throw new NotImplementedException();
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
                splitOn: "Id,Id,Id"
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
            ORDER BY a.created_at DESC
                Where u.id = @VendedorId
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
}
