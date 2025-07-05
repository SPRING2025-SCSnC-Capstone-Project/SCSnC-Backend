using System.Security.Cryptography;
using System.Text;

namespace Application.Common.Helpers;

public class StringUtils {
    private static RandomNumberGenerator rng = RandomNumberGenerator.Create();
    public static string RandomString(int length)
    {
        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        byte[] res = new byte[length];
        rng.GetBytes(res);

        return Convert.ToBase64String(res);
    }
}
