using Ofeck.Bartify.Core.Models;

namespace Ofeck.Bartify.Core.Fotos;

public interface IFotoRepository
{
    public Task Upload(Foto foto);
    public Task<List<Foto>> GetByArticulo(Guid articuloId);
}