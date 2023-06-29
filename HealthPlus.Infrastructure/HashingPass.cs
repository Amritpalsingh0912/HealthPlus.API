using Konscious.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPlus.Infrastructure
{
    public class HashingPass
    {
        public static string CreateHash(string password, string salt)
        {
            var aragon2 = new Argon2id(Encoding.UTF8.GetBytes(password)); //Convert String password to bytes
            aragon2.Salt = Encoding.UTF8.GetBytes(salt);                  //Convert string salt to bytes
            aragon2.DegreeOfParallelism = 8;                              //Define No of Threads use by algoritham  //1 degreeofparllelism 4kb required
            aragon2.Iterations = 30;                                      //No of Repitations as required req/Complaxity
            aragon2.MemorySize = 4 * 8;                                   //memory size depend on degreeofparllelism 32kb

            return Convert.ToBase64String(aragon2.GetBytes(16));
        }
        public static string CreateSalt(string emailAddress)
        {
            var bytes = Encoding.UTF8.GetBytes(emailAddress);
            return Convert.ToBase64String(bytes);
        }
        public static bool VerifyHash(string password, string salt, string hash)
        {
            string encodedStr = Convert.ToBase64String(Encoding.UTF8.GetBytes(salt));
            var newHash = CreateHash(password, encodedStr);
            return hash.SequenceEqual(newHash);
        }
    }
}
