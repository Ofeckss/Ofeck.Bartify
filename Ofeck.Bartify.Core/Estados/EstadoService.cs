namespace Ofeck.Bartify.Core.Estados;
using Ofeck.Bartify.Core.Models;

public class EstadoService
{
    private readonly IEstadoRepository repository;

    public EstadoService(IEstadoRepository repository)
    {
        this.repository = repository;
    }

    public async Task<List<Estado>> GetEstados()
    {
        var estados = await this.repository.GetAll();
        
        if(estados == null || estados.Count == 0)
            throw new KeyNotFoundException("No se encontraron estados.");
        
        return estados;
    }
}