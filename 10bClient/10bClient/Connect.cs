using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _10bClient
{
    public partial class Connect : Form
    {
        public Connect()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart((Action)(()=>
            { Application.Run(new Main(txtHost.Text, Convert.ToInt32(txtPort.Text), txtUser.Text, txtPass.Text)); })));
            t.Start();
            this.Hide();
        }
    }
}
