using PathFinder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2(SortedList<int, Node> optiPath)
        {
            InitializeComponent();
            //outputs the path that is passed in as the names of the nodes
            string pathString = "";
            foreach (KeyValuePair<int, Node> kvp in optiPath)
            {
                pathString = $"{pathString}{kvp.Value.Name},";
            }
            //this.textBox1.Text
        }
    }
}
