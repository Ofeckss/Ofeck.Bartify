namespace Ofeck.Bartify.Core.Ubicaciones;

public class UbicacionService
{
    private readonly IUbicacionRepository repository;
    
    public UbicacionService(IUbicacionRepository repository)
    {
        this.repository = repository;
    }

    public async Task<List<string>> GetUbicaciones()
    {
        var ubicaciones = await this.repository.GetAll();

        if (ubicaciones == null || ubicaciones.Count == 0)
            throw new KeyNotFoundException("No se encontraron ubicaciones.");
        
        return ubicaciones;
    }
}