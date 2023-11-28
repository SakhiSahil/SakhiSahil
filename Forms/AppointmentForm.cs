using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dental.Forms
{
    public partial class AppointmentForm : Form
    {
        public AppointmentForm()
        {
            InitializeComponent();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            guna2ShadowPanel1.Visible = false;

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            guna2ShadowPanel1.Location = new System.Drawing.Point(100, 100);
            guna2ShadowPanel1.Visible = true;
            guna2ShadowPanel1.BringToFront();


        }

        private void guna2ShadowPanel1_Paint(object sender, PaintEventArgs e)
        {
                   }
    }
}
