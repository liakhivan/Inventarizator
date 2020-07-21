using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarizatorLI.Model
{
    public class Security
    {
        public int Id { get; set; }
        public int Pass { get; set; }


        public Security() 
        {
            Pass = "1111".GetHashCode();
        }
        public Security(string pasStr)
        {
            if (pasStr != null)
            {
                throw new Exception("Пароль не задано.");
            }
            if (pasStr.Length < 4)
                throw new Exception("Заданий пароль занадто короткий.\nПароль повинен бути не менше 4 символів.");

            Pass = pasStr.GetHashCode();
        }
    }
}
