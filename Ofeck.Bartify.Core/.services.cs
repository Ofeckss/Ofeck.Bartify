using Microsoft.Extensions.DependencyInjection;
using Ofeck.Bartify.Core.Articulos;
using Ofeck.Bartify.Core.Categorias;
using Ofeck.Bartify.Core.Chats;
using Ofeck.Bartify.Core.Estados;
using Ofeck.Bartify.Core.Fotos;
using Ofeck.Bartify.Core.Integrations.Sendbird;
using Ofeck.Bartify.Core.Ubicaciones;
using Ofeck.Bartify.Core.Usuarios;

namespace Ofeck.Bartify.Core;

public static class ServiceContainer
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<UsuarioService>();
        services.AddScoped<CategoriaService>();
        services.AddScoped<EstadoService>();
        services.AddScoped<UbicacionService>();
        services.AddScoped<ArticuloService>();
        services.AddScoped<FotoService>();
        services.AddScoped<CloudinaryService>(); 
        services.AddScoped<ChatService>();
        
        return services;
    }
}