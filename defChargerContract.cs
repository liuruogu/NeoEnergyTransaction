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
    public partial class defChargerContract : Form
    {
        public defChargerContract()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            chargerView charger = new chargerView();
            charger.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            chargerView charger = new chargerView();
            charger.ShowDialog();
        }

        private void Signup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }
    }
}
