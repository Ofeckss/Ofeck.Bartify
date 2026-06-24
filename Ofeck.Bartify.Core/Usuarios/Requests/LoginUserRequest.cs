namespace Ofeck.Bartify.Core.Usuarios.Requests;

public readonly record struct LoginUserRequest
(
    string Email,
    string Password
);