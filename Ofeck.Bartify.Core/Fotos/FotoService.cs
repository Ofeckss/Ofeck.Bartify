using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Fotos.Requests;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;


namespace Ofeck.Bartify.Core.Fotos;

public class FotoService
{
    private readonly IFotoRepository fotoRepository;
    private readonly CloudinaryService cloudinaryService;

    public FotoService(IFotoRepository fotoRepository, CloudinaryService cloudinaryService)
    {
        this.fotoRepository = fotoRepository;
        this.cloudinaryService = cloudinaryService;
    }

    public async Task<Foto> Upload(IFormFileCollection files, Guid articuloId)
    {
        var urls = await cloudinaryService.UploadImages(files);
        Foto primeraFoto = default;

        for (int i = 0; i < urls.Count; i++)
        {
            var foto = new Foto(Guid.CreateVersion7(), urls[i], articuloId, i);
            await fotoRepository.Upload(foto);
            if (i == 0) primeraFoto = foto;
        }

        return primeraFoto;
    }
    
    public async Task<List<Foto>> GetByArticulo(Guid articuloId)
    {
        return await fotoRepository.GetByArticulo(articuloId);
    }
    
    
}