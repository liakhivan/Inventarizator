using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
 

namespace InventarizatorLI.Model
{
    public class Security
    {
        public int Id { get; set; }
        public string Pass { get; set; }


        private static string salt = "J7%5hu9mM3kZ4^XY8r+d#7@5(Zcc!P";


        public Security() 
        {
            Pass = CreateMD5Hash("1111" + salt);
        }
        public Security(string pasStr)
        {
            if (pasStr != null)
            {
                throw new Exception("Пароль не задано.");
            }
            if (pasStr.Length < 4)
                throw new Exception("Заданий пароль занадто короткий.\nПароль повинен бути не менше 4 символів.");

            Pass = CreateMD5Hash(pasStr + salt);
        }

        public static string GetHashPass(string pass)
        {
            return CreateMD5Hash(pass + salt);
        }

        private static string CreateMD5Hash(string text)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(text);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
