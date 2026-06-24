namespace Ofeck.Bartify.APIEndpoints.Auth;

public interface ITokenService
{
    string CreateToken(Guid Id, string Email);
}