using System.Text.RegularExpressions;
using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Categorias.Responses;

namespace Ofeck.Bartify.Core.Categorias;

public class CategoriaService
{
    private readonly ICategoriaRepository repository;

    public CategoriaService(ICategoriaRepository repository)
    {
        this.repository = repository;
    }

    public async Task<List<Categoria>> GetParents()
    {
        var categorias = await this.repository.GetParents();

        if (categorias == null || categorias.Count == 0) throw new KeyNotFoundException("No se encontraron categorias.");
        
        return categorias;
    }

    public async Task<List<Categoria>> GetSons(int id)
    {
        var hijos = await this.repository.GetSons(id);

        if (hijos == null || hijos.Count == 0) throw new KeyNotFoundException("No se encontraron subcategorias");
        
        return hijos;
    }
}