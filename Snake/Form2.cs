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
    public partial class Form2 : Form
    {
        Form1 form1;

        public Form2(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;
            pictureBox2.BackColor = form1.player1;
            pictureBox3.BackColor = form1.player2;
            hScrollBar1.Value = form1.player1.R;
            hScrollBar2.Value = form1.player1.G;
            hScrollBar3.Value = form1.player1.B;
            hScrollBar6.Value = form1.player2.R;
            hScrollBar5.Value = form1.player2.G;
            hScrollBar4.Value = form1.player2.B;
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            pictureBox2.BackColor = Color.FromArgb(hScrollBar1.Value, hScrollBar2.Value, hScrollBar3.Value);
        }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            pictureBox3.BackColor = Color.FromArgb(hScrollBar6.Value, hScrollBar5.Value, hScrollBar4.Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            form1.player1 = Color.FromArgb(hScrollBar1.Value, hScrollBar2.Value, hScrollBar3.Value);
            form1.player2 = Color.FromArgb(hScrollBar6.Value, hScrollBar5.Value, hScrollBar4.Value);
        }
    }
}
