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
    public partial class Eve : Form
    {
        public Eve()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            string userName = textBox1.Text;
            string passWord = textBox2.Text;

            if(userName == "roger"&&passWord == "roger")
            {
            
            this.Hide();

                string accountType = comboBox1.SelectedItem.ToString();

                //if(accountType == "Tenant Driver")
                //{
                //    userView user = new userView();
                //    user.ShowDialog();
                //}

                switch (accountType)
                {
                    case "Tenant Driver":

                        userView user = new userView();
                        user.ShowDialog();
                        break;

                    case "Private Charger Opeartor":
                        userView user1 = new userView();
                        user1.ShowDialog();
                        break;

                    case "Local Energy ProviderTenant Driver":
                        userView user2 = new userView();
                        user2.ShowDialog();
                        break;

                    case "EV Fleet":
                        userView user3 = new userView();
                        user3.ShowDialog();
                        break;
                }

                this.Close();
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }
    }
}
