using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class PlayerCountForm : Form
    {
        public PlayerCountForm()
        {
            InitializeComponent();
            numericUpDown1.Value = Convert.ToInt32(Config.Instance.Get("PlayerCount"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Config.Instance.Set("PlayerCount", numericUpDown1.Value.ToString());
            Config.Instance.StoreCfgFile();
            this.Close();
        }
    }
}
