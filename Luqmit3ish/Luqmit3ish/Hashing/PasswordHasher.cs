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
            return true;
        }
    }
}
