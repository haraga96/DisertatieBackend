using System;
using System.Security.Cryptography;
using System.Text;
using Backend_Dis_App.Services.Interfaces;

namespace Backend_Dis_App.Services.Implementation
{
    public class SecurePassword: ISecurePassword
    {
        public (byte[], byte[]) EncryptPassword(string password)
        {

            var salt = new byte[8];
            new RNGCryptoServiceProvider().GetBytes(salt);

            var keyDerivation = new Rfc2898DeriveBytes(Encoding.ASCII.GetBytes(password), salt, 10000);

            var computedHash = keyDerivation.GetBytes(32);
            return (computedHash, salt);
        }

        public byte[] DecryptPassword(string password, string salt)
        {
            var keyDerivation = new Rfc2898DeriveBytes(Encoding.ASCII.GetBytes(password),
                                                       Convert.FromBase64String(salt),
                                                       10000);

            var computedHash = keyDerivation.GetBytes(32);
            return computedHash;
        }
    }
}
