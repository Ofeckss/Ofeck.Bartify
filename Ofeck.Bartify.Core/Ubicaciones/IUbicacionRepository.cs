using Ofeck.Bartify.Core.Models;

namespace Ofeck.Bartify.Core.Ubicaciones;

public interface IUbicacionRepository
{
    public Task<List<Ubicacion>> GetAll();
}