using System;
using System.Collections.Generic;
using System.Text;

namespace Luqmit3ish.Hashing
{
    interface IHasher
    {
         bool VerifyPassword(string password, string savedPasswordHash);
    }
}
