namespace Application.Common.Interfaces;

public interface ISecurityService {
    public byte[] Hash(string password, string salt, string email);
}
