namespace Application.DTOS.Auth;

public record RestablecerPasswordDto(
    string Token,
    string NuevoPassword,
    string ConfirmarPassword
);
