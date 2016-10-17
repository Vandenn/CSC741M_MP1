using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSC741M_MP1
{
    public partial class AboutView : Form
    {
        public AboutView()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {

        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
