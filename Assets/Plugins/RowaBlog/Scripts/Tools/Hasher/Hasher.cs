using System;
using System.Security.Cryptography;
using System.Text;

namespace Rowa.Blog.Tools.Hasher
{
    public class Hasher : IHasher
    {
        public string ToHash(string value)
        {
            MD5 MD5Hash = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(value);
            byte[] hash = MD5Hash.ComputeHash(inputBytes);
            string output = BitConverter.ToString(hash).Replace("-", String.Empty); ;
            return output;
        }
    }
}
