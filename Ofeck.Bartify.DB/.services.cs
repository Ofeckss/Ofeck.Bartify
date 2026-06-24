using System.Data;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using Ofeck.Bartify.Core.Articulos;
using Ofeck.Bartify.Core.Categorias;
using Ofeck.Bartify.Core.Estados;
using Ofeck.Bartify.Core.Fotos;
using Ofeck.Bartify.Core.Ubicaciones;
using Ofeck.Bartify.Core.Usuarios;
namespace Ofeck.Bartify.DB;

public static class ServiceContainer
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, string connectionString) {
        
        services.AddScoped<IDbConnection>(sp => new MySqlConnection(connectionString));

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<IEstadoRepository, EstadoRepository>();
        services.AddScoped<IUbicacionRepository, UbicacionRepository>();
        services.AddScoped<IArticuloRepository, ArticuloRepository>();
        services.AddScoped<IFotoRepository, FotoRepository>();

        return services;
    }
}