namespace Ofeck.Bartify.Core.Sendbird;

public interface ISendbirdRepository
{
    Task CreateUser(Guid id, string nombre);
    Task<string> CreateChannel(Guid Comprador, Guid Vendedor, Guid Articulo, string NombreArticulo);
}