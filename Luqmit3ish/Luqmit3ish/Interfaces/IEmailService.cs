using System;
using System.Threading.Tasks;

namespace Luqmit3ish.Interfaces
{
    public interface IEmailService
    {
        Task<string> SendVerificationCode(string recipientName, string recipientEmail);
    }
}