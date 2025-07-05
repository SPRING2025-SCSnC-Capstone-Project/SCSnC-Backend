using System.Text;
using Application.Common.Interfaces;
using Konscious.Security.Cryptography;

namespace Infrastructure.Services.Security;

public class SecurityService : ISecurityService {
    public SecurityService() {}

    public byte[] Hash(string password, string salt, string email) {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var saltBytes = Convert.FromBase64String(salt);
        var emailBytes = Encoding.UTF8.GetBytes(email);

        var argon2 = new Argon2id(passwordBytes)
        {
            DegreeOfParallelism = 2,
            MemorySize = 19456,
            Iterations = 2,
            Salt = saltBytes,
            AssociatedData = emailBytes
        };

        var bytes = argon2.GetBytes(64);

        return bytes;
    }
}
