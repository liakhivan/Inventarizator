using InventarizatorLI.Model;
using InventarizatorLI.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventarizatorUI.Forms
{
    public partial class ChangeWithDeleting : Form
    {
        private Ingredient ingredient;
        public ChangeWithDeleting(Ingredient igredient)
        {
            IngredientRepository ingredientRepository = new IngredientRepository();
            InitializeComponent();
            comboBox1.DataSource = ingredientRepository.GetDataSource().SkipWhile(n => n.Id == ingredient.Id).ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
