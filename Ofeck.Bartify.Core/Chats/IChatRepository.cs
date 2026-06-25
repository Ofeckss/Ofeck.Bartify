using Ofeck.Bartify.Core.Chats.DTOs;
using Ofeck.Bartify.Core.Models;

namespace Ofeck.Bartify.Core.Chats;

public interface IChatRepository
{
    Task Create(Chat chat);
    Task<string> ChatExists(Guid Id, Guid Articulo);
    Task<List<ChatDTO>> GetByUser(Guid Id);
}