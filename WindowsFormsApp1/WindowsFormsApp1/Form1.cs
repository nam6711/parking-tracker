using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//hooray collaboration
using PathFinder;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        //globals
        List<Node> allNodes = new List<Node>();
        static PathFinder.PathFinder pathFinder = new PathFinder.PathFinder("nodeList (1)");
        static SortedList<int, Node> path = new SortedList<int, Node>();
        bool firstClick = false;
        Node node1 = null;
        Node node2 = null;
        public Form1()
        {
            InitializeComponent();
            //allNodes = new List<Node>(pathFinder.nodes.Values);
            path = pathFinder.FindPath(1, 10);


            this.pictureBox1.Click += PictureBox1__Click;
        }

        private void PictureBox1__Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            foreach(Node node in allNodes)
            {
                //if the user clicks in a 15 pixels radius of the building it fires
                if(node.X + 15 > Cursor.Position.X && node.X - 15 < Cursor.Position.X && node.Y - 15 < Cursor.Position.Y && node.Y + 15 > Cursor.Position.Y)
                {
                    FindPath(node);
                }
                else
                {
                    continue;
                }
            }
        }
        //If the node name includes "lot" save it as node 1, if it doesn't find "lot", assume it is a building and wait for a lot
        private void FindPath(Node node)
        {
            SortedList<int,Node> path = null;
            if (node1 != null && node2 != null)
            {
                path = pathFinder.FindPath(node1.ID, node2.ID);
                Form2 form2 = new Form2(path);
                node1 = null;
                node2 = null;
            }

            if (node.Name.Contains("lot") && node1==null)
            {
                node1 = node;
                return;
            }else if (node2 == null)
            {
                node2 = node;
                return;
            }
        }
    }
}
