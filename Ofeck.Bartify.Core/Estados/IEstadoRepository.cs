using Ofeck.Bartify.Core.Models;

namespace Ofeck.Bartify.Core.Estados;

public interface IEstadoRepository
{
    public Task<List<Estado>> GetAll();
}