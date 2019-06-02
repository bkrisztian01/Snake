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
    public partial class ColorSelectForm : Form
    {
        GameplayForm _gameplayForm;

        public ColorSelectForm(GameplayForm gameplayForm)
        {
            InitializeComponent();
            this._gameplayForm = gameplayForm;
            pictureBox2.BackColor = gameplayForm.player1;
            pictureBox3.BackColor = gameplayForm.player2;
            hScrollBar1.Value = gameplayForm.player1.R;
            hScrollBar2.Value = gameplayForm.player1.G;
            hScrollBar3.Value = gameplayForm.player1.B;
            hScrollBar6.Value = gameplayForm.player2.R;
            hScrollBar5.Value = gameplayForm.player2.G;
            hScrollBar4.Value = gameplayForm.player2.B;
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
            _gameplayForm.player1 = Color.FromArgb(hScrollBar1.Value, hScrollBar2.Value, hScrollBar3.Value);
            _gameplayForm.player2 = Color.FromArgb(hScrollBar6.Value, hScrollBar5.Value, hScrollBar4.Value);
        }
    }
}
