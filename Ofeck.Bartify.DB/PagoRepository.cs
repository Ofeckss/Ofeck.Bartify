using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Pagos;

namespace Ofeck.Bartify.DB;

public class PagoRepository : IPagoRepository
{
    private readonly string _connectionString;

    public PagoRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task InsertarAsync(Pago pago)
    {
        using var connection = new MySqlConnection(_connectionString);
        const string sql = @"
            INSERT INTO pagos (id, transaccion_id, stripe_session_id, monto, moneda, estado, fecha_creacion)
            VALUES (@Id, @TransaccionId, @StripeSessionId, @Monto, @Moneda, @Estado, @FechaCreacion)";

        await connection.ExecuteAsync(sql, new
        {
            Id = pago.Id.ToString(),
            TransaccionId = pago.TransaccionId.ToString(),
            pago.StripeSessionId,
            pago.Monto,
            pago.Moneda,
            pago.Estado,
            pago.FechaCreacion
        });
    }

    public async Task<Pago> ObtenerPorSessionIdAsync(string stripeSessionId)
    {
        using var connection = new MySqlConnection(_connectionString);
        const string sql = @"
            SELECT id, transaccion_id, stripe_session_id, monto, moneda, estado, fecha_creacion, fecha_actualizacion
            FROM pagos
            WHERE stripe_session_id = @StripeSessionId";

        var pago = await connection.QueryFirstOrDefaultAsync<Pago>(sql, new { StripeSessionId = stripeSessionId });

        if (pago.StripeSessionId is null)
            throw new KeyNotFoundException($"Pago con session {stripeSessionId} no encontrado");

        return pago;
    }

    public async Task ActualizarEstadoAsync(string stripeSessionId, string nuevoEstado)
    {
        using var connection = new MySqlConnection(_connectionString);
        const string sql = @"
            UPDATE pagos
            SET estado = @Estado, fecha_actualizacion = @FechaActualizacion
            WHERE stripe_session_id = @StripeSessionId";

        await connection.ExecuteAsync(sql, new
        {
            Estado = nuevoEstado,
            FechaActualizacion = DateTime.UtcNow,
            StripeSessionId = stripeSessionId
        });
    }

    public async Task ActualizarPrecio(Guid id, decimal monto)
    {
        using var connection = new MySqlConnection(_connectionString);
        const string sql = """
                update transacciones set precio_final = @Monto where id = @Id
            """;
        
        await connection.ExecuteAsync(sql, new { Id = id, Monto = monto });
    }
}