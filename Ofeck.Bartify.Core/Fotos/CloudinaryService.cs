using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Ofeck.Bartify.Core.Fotos.Requests;

namespace Ofeck.Bartify.Core.Fotos;

public class CloudinaryService(IOptions<CloudinaryRequest> options)
{
    private readonly Cloudinary cloudinary = new(
            new Account(options.Value.CloudName, options.Value.ApiKey, options.Value.ApiSecret))
        {Api = {Secure = true}};

    public async Task<List<string>> UploadImages(IFormFileCollection files)
    {
        var urls = new List<string>();

        foreach (var file in files)
        {
            using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File           = new FileDescription(file.FileName, stream),
                Folder         = "bartify",
                Transformation = new Transformation().Quality("auto").FetchFormat("auto")
            };

            var result = await this.cloudinary.UploadAsync(uploadParams);

            if (result.Error is not null)
                throw new Exception($"Error de Cloudinary: {result.Error.Message}");

            urls.Add(result.SecureUrl.ToString());
        }

        return urls;
    }
}