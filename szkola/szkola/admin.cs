using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace szkola
{
    public partial class admin : Form
    {
        public admin()
        {
            InitializeComponent();

            for(int i = 0; i < 10; i++)
            {
                listBox1.Items.Add("Item " + i.ToString());

            }
        }
    }
}
