using System;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Ghostware.GPS.NET.Encryption
{
    public static class StringEncryption
    {
        public static string EncryptString(SecureString input, byte[] salt)
        {
            var encryptedData = ProtectedData.Protect(Encoding.Unicode.GetBytes(ToInsecureString(input)), salt,
                DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        public static SecureString DecryptString(string encryptedData, byte[] salt)
        {
            try
            {
                var decryptedData = ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), salt,
                    DataProtectionScope.CurrentUser);
                return ToSecureString(Encoding.Unicode.GetString(decryptedData));
            }
            catch
            {
                return new SecureString();
            }
        }

        public static SecureString ToSecureString(string input)
        {
            var secure = new SecureString();
            foreach (var c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }

        public static string ToInsecureString(SecureString input)
        {
            string returnValue;
            var ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input);
            try
            {
                returnValue = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
            return returnValue;
        }
    }
}