using System.Text.RegularExpressions;
using CloudinaryDotNet.Actions;
using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Chats;
using Ofeck.Bartify.Core.Chats.DTOs;
using Ofeck.Bartify.Core.Chats.Requests;
using Ofeck.Bartify.Core.Sendbird;

namespace Ofeck.Bartify.Core.Chats;

public class ChatService
{
    private readonly IChatRepository repository;
    private readonly ISendbirdRepository sendbird;

    public ChatService(IChatRepository repository, ISendbirdRepository sendbird)
    {
        this.repository = repository;
        this.sendbird = sendbird;
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
        return nuevoCanal;
    }

    public async Task<List<ChatDTO>> GetByUser(Guid Id)
    {
        var list = await this.repository.GetByUser(Id);

        return list;
    }
}