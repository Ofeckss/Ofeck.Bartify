using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Transacciones.Requests;
using Ofeck.Bartify.Core.Transacciones.Responses;

namespace Ofeck.Bartify.Core.Transacciones;

public class TransaccionService
{
    private ITransaccionRepository repository;
    
    public TransaccionService(ITransaccionRepository repository)
    {
        this.repository = repository;
    }

    public async Task CreateTrans(CreateTransaccionRequest request)
    {
        var trans = new Transaccion(
            Guid.CreateVersion7(),
            request.ChatId,
            request.EsTrueque,
            null,
            false,
            false,
            DateTime.Now,
            DateTime.Now
        );
        
        await this.repository.CreateTransaccion(trans);
    }

    public async Task CreateDetalle(CreateDetalleRequest request, Guid userId)
    {
        var transaccion = await this.repository.GetTransId(request.ChatId);
        
        var esVendedor = await this.repository.EsVendedor(request.ChatId,  userId);

        if (esVendedor)
        {
            var detalle = new DetalleTransaccion(
                    Guid.CreateVersion7(),
                    transaccion,
                    request.ArticuloId,
                    true,
                    false
                );
            await this.repository.CreateDetalle(detalle);
            
        } else
        {
            var detalle = new DetalleTransaccion(
                Guid.CreateVersion7(),
                transaccion,
                request.ArticuloId,
                false,
                false
            );
            
            await this.repository.CreateDetalle(detalle);
        }
    }

    public async Task Confirmar(ConfirmarRequest request, Guid id)
    {
        var esVendedor = await this.repository.EsVendedor(request.ChatId, id);

        if (esVendedor)
        {
            await this.repository.ConfirmarVendedor(request.ChatId);
            
            for (int i = 0; i < request.Articulos.Count; i++)
            {
                await this.repository.ConfirmarDetVendedor(request.ChatId, request.Articulos[i]);
            }
        } else
        {
            await this.repository.ConfirmarComprador(request.ChatId, request.Precio);
            
            for (int i = 0; i < request.Articulos.Count; i++)
            {
                await this.repository.ConfirmarDetComprador(request.ChatId, request.Articulos[i]);
            }
        }
    }

    public async Task Cancelar(Guid chatId, Guid id)
    {
        var esVendedor = await this.repository.EsVendedor(chatId, id);

        if (esVendedor)
        {
            await this.repository.RevertirVendedor(chatId);
            await this.repository.RevertirDetVendedor(chatId);
        } else
        {
            await this.repository.RevertirComprador(chatId);
            await this.repository.RevertirDetComprador(chatId);
        }
    }

    public async Task<bool> GetStatus(Guid chatId)
    {
        var status = await this.repository.GetStatus(chatId);
        return status;
    }

    public async Task<List<GetAllTransaccionesResponse>> GetAllByUser(Guid userId)
    {
        var lista = await this.repository.GetAll(userId);

        return lista;
    }

    public async Task<GetTransaccionResponse> GetById(Guid chatId)
    {
        var info = await this.repository.GetSemiById(chatId);

        var listaV = await this.repository.GetListById(info.Id, true);
        var listaC = await this.repository.GetListById(info.Id, false);

        var response = new GetTransaccionResponse(
            info.Id,
            info.ChatId,
            info.EsTrueque,
            info.PrecioFinal,
            info.CreatedAt,
            info.UpdatedAt,
            listaV,
            listaC
        );

        return response;
    }

    public async Task<List<GetDetalleResponse>> GetDetalle(Guid chatId)
    {
        return await this.repository.GetDetalle(chatId);
    }
}