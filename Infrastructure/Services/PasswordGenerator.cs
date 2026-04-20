using Application.Ports.Driven;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services;

public class PasswordGenerator : IPasswordGenerator{
    private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
    private const string Numbers = "0123456789";
    private const string Special = "!@#$%^&*()-_=+[]{}|;:,.<>?";

    public string Generate(int length = 12) {
        if(length < 4)
            throw new ArgumentException("Mínimo 4 caracteres");

        var passwordChars = new char[length];
        var allChars = Uppercase + Lowercase + Numbers + Special;

        using(var rng = RandomNumberGenerator.Create()) {
            // Asegurar al menos uno de cada tipo
            passwordChars[0] = Uppercase[GetRandom(rng, Uppercase.Length)];
            passwordChars[1] = Lowercase[GetRandom(rng, Lowercase.Length)];
            passwordChars[2] = Numbers[GetRandom(rng, Numbers.Length)];
            passwordChars[3] = Special[GetRandom(rng, Special.Length)];

            // Rellenar el resto
            for(int i = 4; i < length; i++) {
                passwordChars[i] = allChars[GetRandom(rng, allChars.Length)];
            }

            // Mezclar (shuffle)
            return new string(passwordChars.OrderBy(x => GetRandom(rng, int.MaxValue)).ToArray());
        }
    }

    private static int GetRandom(RandomNumberGenerator rng, int max) {
        var bytes = new byte[4];
        rng.GetBytes(bytes);
        return Math.Abs(BitConverter.ToInt32(bytes, 0)) % max;
    }
}
