namespace Backend_Dis_App.Services.Interfaces
{
    public interface ISecurePassword
    {
        (byte[], byte[]) EncryptPassword(string password);
        byte[] DecryptPassword(string password, string salt);
    }
}
