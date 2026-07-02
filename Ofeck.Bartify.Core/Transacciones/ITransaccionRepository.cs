using Ofeck.Bartify.Core.Articulos.DTOs;
using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Transacciones.Responses;

namespace Ofeck.Bartify.Core.Transacciones;

public interface ITransaccionRepository
{
    public Task CreateTransaccion (Transaccion transaccion);
    public Task CreateDetalle (DetalleTransaccion detalleTransaccion);
    public Task<Guid> GetTransId(Guid chatId);
    public Task<bool> EsVendedor (Guid chatId, Guid usuarioId);
    public Task ConfirmarComprador (Guid chatId, double? precio);
    public Task ConfirmarDetComprador(Guid chatId, Guid articuloId);
    public Task ConfirmarVendedor (Guid chatId);
    public Task ConfirmarDetVendedor(Guid chatId, Guid articuloId);
    public Task RevertirComprador (Guid chatId);
    public Task RevertirDetComprador(Guid chatId);
    public Task RevertirVendedor (Guid chatId);
    public Task RevertirDetVendedor(Guid chatId);
    public Task<List<GetDetalleResponse>> GetDetalle(Guid chatId);
    public Task<List<GetAllTransaccionesResponse>> GetAll (Guid usuarioId);
    public Task<GetMiniTransaccionResponse> GetSemiById (Guid id);
    public Task<List<MiniArticuloDTO>> GetListById(Guid transaccionId, bool esVendedor);
    public Task<GetStatusResponse> GetStatus(Guid chatId);
    public Task<bool> ExistByChatId(Guid chatId);
    public Task Terminar(Guid chatId);

}