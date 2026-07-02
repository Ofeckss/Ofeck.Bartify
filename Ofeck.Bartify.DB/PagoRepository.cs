using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Pagos;

namespace Ofeck.Bartify.DB;

public class PagoRepository(IDbConnection db) : IPagoRepository
{
     public async Task InsertarAsync(Pago pago)
    {
        var sql = @"
            INSERT INTO pagos (id, transaccion_id, stripe_session_id, monto, moneda, estado, fecha_creacion)
            VALUES (@Id, @TransaccionId, @StripeSessionId, @Monto, @Moneda, @Estado, @FechaCreacion)";

        var parametros = new
        {
            Id = pago.Id.ToString(),
            TransaccionId = pago.TransaccionId.ToString(),
            pago.StripeSessionId,
            pago.Monto,
            pago.Moneda,
            pago.Estado,
            pago.FechaCreacion
        };

        logger.LogInformation(
            "Insertando Pago: Id={Id}, TransaccionId={TransaccionId}, StripeSessionId={StripeSessionId}, Monto={Monto}, Moneda={Moneda}, Estado={Estado}",
            parametros.Id, parametros.TransaccionId, parametros.StripeSessionId, parametros.Monto, parametros.Moneda, parametros.Estado);

        try
        {
            await db.ExecuteAsync(sql, parametros);
        }
        catch (MySqlException ex)
        {
            logger.LogError(ex,
                "Error MySQL insertando Pago. Number={Number}, SqlState={SqlState}, Message={Message}. Parametros: {@Parametros}",
                ex.Number, ex.SqlState, ex.Message, parametros);
            throw;
        }
    }


    public async Task<Pago> ObtenerPorSessionIdAsync(string stripeSessionId)
    {
        var sql = @"
            SELECT id, transaccion_id, stripe_session_id, monto, moneda, estado, fecha_creacion, fecha_actualizacion
            FROM pagos
            WHERE stripe_session_id = @StripeSessionId";

        var pago = await db.QueryFirstOrDefaultAsync<Pago>(sql, new { StripeSessionId = stripeSessionId });

        if (pago.StripeSessionId is null)
            throw new KeyNotFoundException($"Pago con session {stripeSessionId} no encontrado");

        return pago;
    }

    public async Task ActualizarEstadoAsync(string stripeSessionId, string nuevoEstado)
    {
        var sql = @"
            UPDATE pagos
            SET estado = @Estado, fecha_actualizacion = @FechaActualizacion
            WHERE stripe_session_id = @StripeSessionId";

        await db.ExecuteAsync(sql, new
        {
            Estado = nuevoEstado,
            FechaActualizacion = DateTime.UtcNow,
            StripeSessionId = stripeSessionId
        });
    }

    public async Task ActualizarPrecio(Guid id, decimal monto)
    {
        const string sql = """
                update transacciones set precio_final = @Monto where id = @Id
            """;
        
        await db.ExecuteAsync(sql, new { Id = id, Monto = monto });
    }
}
