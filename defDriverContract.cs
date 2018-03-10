using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Neo.Core;
using Neo.Cryptography.ECC;
using Neo.SmartContract;
using Neo.VM;
using Neo.Wallets;
using System.Globalization;
using Neo;

namespace WindowsFormsApp1
{
    public partial class defDriverContract : Form

    {
        //Define the contract
        //The cost of using the EV per hour
        public static float rentPrice = 0.3f;
        public static Int16 numOfCar = 30000;
        //public static UInt160 Owner = Wallet.ToScriptHash("AUawydn3Adz88nvj3C3JiCY2KNNnc7CcQy");
        public static string Owner = "AUawydn3Adz88nvj3C3JiCY2KNNnc7CcQy";
        private int[] startTime = new int[] { 8, 10, 12, 14 };

        public defDriverContract()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            //chargeCost.Text = Owner.ToString(); 
            userView user = new userView();
            user.ShowDialog();
        }

        private void Signup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void defDriverContract_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            //chargeCost.Text = Owner.ToString(); 
            userView user = new userView();
            user.ShowDialog();
        }
    }
}