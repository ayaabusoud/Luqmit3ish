using Luqmit3ish.Hashing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Luqmit3ish.Hashing
{
    public class PasswordHasher : IHasher
    {

       public bool VerifyPassword(string password, string savedPasswordHash)
        {
            // Retrieve the salt value from the stored hash
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Generate a hash value using the retrieved salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            // Compare the generated hash value with the stored hash value
            bool passwordMatches = hash.SequenceEqual(hashBytes.Skip(16).ToArray());
            return passwordMatches;
        }
    }
}
