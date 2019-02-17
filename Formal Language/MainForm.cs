using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Formal_Language.UserControls;

namespace Formal_Language
{
    public partial class MainForm : Form
    {
        UserControl[] userControls;
        public MainForm()
        {
            InitializeComponent();

            userControls = new UserControl[4];

            userControls[0] = new StringGenerator();
            userControls[1] = new PushDown();
            userControls[2] = new TableGeneration();

            userControls[3] = new UserControl();

            foreach (UserControl userControl in userControls)
            {
                panel.Controls.Add(userControl);
                userControl.Hide();
            }
        }

        private void radioButtonStringGenerator_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonStringGenerator.Checked)
            {
                userControls[0].Show();
            }
            else
            {
                userControls[0].Hide();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                userControls[1].Show();
            }
            else
            {
                userControls[1].Hide();
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                userControls[2].Show();
            }
            else
            {
                userControls[2].Hide();
            }
        }

        void HideUserControls()
        {

        }

        
    }
}
