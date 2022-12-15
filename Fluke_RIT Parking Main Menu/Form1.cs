using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fluke_RIT_Parking_Main_Menu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.mainMenuButton.Enabled = true;
            this.displayLotsbutton.Enabled = true;
            this.optimalLotButton.Enabled = true;
            this.ritDiningButton.Enabled = true;
            this.exitButton.Enabled = true;

            this.mainMenuButton.Click += new EventHandler(MainMenuButton__Click);
            this.displayLotsbutton.Click += new EventHandler(DisplayLotsButton__Click);
            this.optimalLotButton.Click += new EventHandler(OptimalLotButton__Click);
            this.ritDiningButton.Click += new EventHandler(RITDiningButton__Click);
            this.exitButton.Click += new EventHandler(ExitButton__Click);
        }

        private void MainMenuButton__Click(object sender, EventArgs e)
        {
            groupBox.Text = "RIT Interactive Map";
            webBrowser1.Navigate("https://maps.rit.edu/");
        }

        private void DisplayLotsButton__Click(object sender, EventArgs e)
        {
            var myForm = new lotViewForm();
            myForm.Show();
        }
        private void OptimalLotButton__Click(object sender, EventArgs e)
        {
            var myForm = new optimalLotForm();
            myForm.Show();
        }
        private void RITDiningButton__Click(object sender, EventArgs e)
        {
            groupBox.Text = "RIT Dining Hours and Availability";
            webBrowser1.Navigate("https://www.rit.edu/fa/diningservices/places-to-eat/hours");
        }
        private void ExitButton__Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
