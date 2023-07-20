using System.Security.Cryptography;
using System.Text;
using API.Dtos;
using API.Entities;

namespace API.Helpers;

public static class Password
{
    public static void HashPassword(AppUser user, string password)
    {
        using var hmac = new HMACSHA512();

        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        user.PasswordSalt = hmac.Key;
    }

    public static bool PasswordValid(AppUser user, string password)
    {
        using var hmac = new HMACSHA512(user.PasswordSalt);
        
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
            {
                return false;
            }
        }

        return true;
    }
}