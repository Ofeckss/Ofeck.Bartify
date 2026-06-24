using Ofeck.Bartify.Core.Models;

namespace Ofeck.Bartify.Core.Categorias;

public interface ICategoriaRepository
{
    public Task<List<Categoria>> GetParents();
    public Task<List<Categoria>> GetSons(int id);
}