using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using InventarizatorLI.Model;

namespace InventarizatorUI
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (StorageDbContext sd = new StorageDbContext())
            {
                sd.Ingredients.Add(new Ingredient("Milk", 25));
                sd.Ingredients.Add(new Ingredient("Sugar", 15));
                sd.SaveChanges();
            }
            //sd.Products.Add(new Product("Plombir", new Dictionary<Ingredient, double> { new Ingredient()}))
            Application.Run(new Form1());
        }
    }
}
