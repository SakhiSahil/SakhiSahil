using Dental.Forms;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dental
{
    public partial class Form1 : Form
    {

        private List<Guna2Button> radioButtonGroup;

        public Form1()
        {
            InitializeComponent();
            InitializeRadioButtonGroup();
        }

        private void guna2Button1_Click(object sender, EventArgs e) => this.Close();

        private void guna2Button2_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;

        private void InitializeRadioButtonGroup()
        {
            radioButtonGroup = new List<Guna2Button>
            {
                btnDashboard,
                btnDentiest,
                btnBilling,
                btnAppointments,
                btnPatients,
                btnReports,
                btnSettings
            };

            foreach (var button in radioButtonGroup)
            {
                button.Click += RadioButton_Click;
            }
        }

        private void RadioButton_Click(object sender, EventArgs e)
        {
            var clickedButton = (Guna2Button)sender;
            labelDisplay.Text = "You selected: " + clickedButton.Text;

            foreach (var button in radioButtonGroup)
            {
                if (button == clickedButton)
                {
                    button.Checked = true;
                }
                else
                {
                    button.Checked = false;
                }
            }
        }

        private void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            guna2Panel2.Controls.Clear();
            LoadDashboardForm();
        }
        private void LoadDashboardForm()
        {
            DashboardForm dashboard = new DashboardForm();
            dashboard.TopLevel = false;
            dashboard.AutoScroll = true;
            guna2Panel2.Controls.Add(dashboard);
            dashboard.Show();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDashboardForm();
        }
    }
}
