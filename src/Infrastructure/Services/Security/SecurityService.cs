using System.Text;
using Application.Common.Interfaces;
using Konscious.Security.Cryptography;

namespace Infrastructure.Services.Security;

public class SecurityService : ISecurityService {
    public SecurityService() {}

    public byte[] Hash(string password, string salt, string username) {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var saltBytes = Convert.FromBase64String(salt);
        var usernameBytes = Encoding.UTF8.GetBytes(username);

        var argon2 = new Argon2id(passwordBytes)
        {
            DegreeOfParallelism = 2,
            MemorySize = 19456,
            Iterations = 2,
            Salt = saltBytes,
            AssociatedData = usernameBytes
        };

        var bytes = argon2.GetBytes(64);

        return bytes;
    }
}
