using System.Text.RegularExpressions;
using CloudinaryDotNet.Actions;
using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Chats;
using Ofeck.Bartify.Core.Chats.DTOs;
using Ofeck.Bartify.Core.Chats.Requests;
using Ofeck.Bartify.Core.Sendbird;
using Ofeck.Bartify.Core.Transacciones;
using Ofeck.Bartify.Core.Transacciones.Requests;

namespace Ofeck.Bartify.Core.Chats;

public class ChatService
{
    private readonly IChatRepository repository;
    private readonly ISendbirdRepository sendbird;
    private readonly TransaccionService service;

    public ChatService(IChatRepository repository, ISendbirdRepository sendbird, TransaccionService service)
    {
        this.repository = repository;
        this.sendbird = sendbird;
        this.service = service;
    }

    public async Task<string> Create(CreateChatRequest request, Guid Comprador)
    {
        var exists = await repository.ChatExists(Comprador, request.Articulo);

        if (exists is not null) return exists;
        
        var nuevoCanal = await this.sendbird.CreateChannel(Comprador, request.Vendedor, request.Articulo, request.Nombre);

        var chat = new Chat(
            Guid.CreateVersion7(),
            request.Articulo,
            Comprador,
            request.Vendedor,
            nuevoCanal,
            DateTime.Now
        );

        await this.repository.Create(chat);

        var transRequest = new CreateTransaccionRequest(
            chat.Id,
            request.EsTrueque
        );
        
        await this.service.CreateTrans(transRequest);

        var detalleRequest = new CreateDetalleRequest(
            chat.Id,
            chat.ArticuloPrincipal
        );
        
        await this.service.CreateDetalle(detalleRequest, request.Vendedor);
        
        return nuevoCanal;
    }

    public async Task<List<ChatDTO>> GetByUser(Guid Id)
    {
        var list = await this.repository.GetByUser(Id);

        return list;
    }
}