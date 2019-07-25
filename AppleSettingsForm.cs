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
    public partial class AppleSettingsForm : Form
    {
        GameplayForm _gameplayForm;

        public AppleSettingsForm(GameplayForm gameplayForm)
        {
            InitializeComponent();

            _gameplayForm = gameplayForm;

            textBox1.Text = gameplayForm.appleCount.ToString();
            hScrollBar1.Value = gameplayForm.growAppleChance;
            hScrollBar2.Value = gameplayForm.doubleGrowAppleChance;
            hScrollBar3.Value = gameplayForm.changeControlAppleChance;
            hScrollBar4.Value = gameplayForm.speedBoostAppleChance;
            hScrollBar5.Value = gameplayForm.blackAppleChance;

            refreshLabels();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.Type.Equals(ScrollEventType.EndScroll))
            {
                int delta = (hScrollBar1.Value - _gameplayForm.growAppleChance);
                int sum = hScrollBar2.Value + hScrollBar3.Value + hScrollBar4.Value + hScrollBar5.Value;

                if (sum != 0)
                {
                    hScrollBar2.Value -= delta * hScrollBar2.Value / sum;
                    hScrollBar3.Value -= delta * hScrollBar3.Value / sum;
                    hScrollBar4.Value -= delta * hScrollBar4.Value / sum;
                    hScrollBar5.Value -= delta * hScrollBar5.Value / sum;
                }
                else
                {
                    hScrollBar2.Value -= delta / 4;
                    hScrollBar3.Value -= delta / 4;
                    hScrollBar4.Value -= delta / 4;
                    hScrollBar5.Value -= delta / 4;
                }
                    setValues();
                refreshLabels();
            }
        }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.Type.Equals(ScrollEventType.EndScroll))
            {
                int delta = (hScrollBar2.Value - _gameplayForm.doubleGrowAppleChance);
                int sum = hScrollBar1.Value + hScrollBar3.Value + hScrollBar4.Value + hScrollBar5.Value;

                if (sum != 0)
                {
                    hScrollBar1.Value -= delta * hScrollBar1.Value / sum;
                    hScrollBar3.Value -= delta * hScrollBar3.Value / sum;
                    hScrollBar4.Value -= delta * hScrollBar4.Value / sum;
                    hScrollBar5.Value -= delta * hScrollBar5.Value / sum;
                }
                else
                {
                    hScrollBar1.Value -= delta / 4;
                    hScrollBar3.Value -= delta / 4;
                    hScrollBar4.Value -= delta / 4;
                    hScrollBar5.Value -= delta / 4;
                }
                        setValues();
                refreshLabels();
            }
        }

        private void hScrollBar3_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.Type.Equals(ScrollEventType.EndScroll))
            {
                int delta = (hScrollBar3.Value - _gameplayForm.changeControlAppleChance);
                int sum = hScrollBar1.Value + hScrollBar2.Value + hScrollBar4.Value + hScrollBar5.Value;

                if (sum != 0)
                {
                    hScrollBar1.Value -= delta * hScrollBar1.Value / sum;
                    hScrollBar2.Value -= delta * hScrollBar2.Value / sum;
                    hScrollBar4.Value -= delta * hScrollBar4.Value / sum;
                    hScrollBar5.Value -= delta * hScrollBar5.Value / sum;
                }
                else
                {
                    hScrollBar1.Value -= delta / 4;
                    hScrollBar2.Value -= delta / 4;
                    hScrollBar4.Value -= delta / 4;
                    hScrollBar5.Value -= delta / 4;
                }
                            setValues();
                refreshLabels();
            }
        }

        private void hScrollBar4_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.Type.Equals(ScrollEventType.EndScroll))
            {
                int delta = (hScrollBar4.Value - _gameplayForm.speedBoostAppleChance);
                int sum = hScrollBar1.Value + hScrollBar2.Value + hScrollBar3.Value + hScrollBar5.Value;

                if (sum != 0)
                {
                    hScrollBar1.Value -= delta * hScrollBar1.Value / sum;
                    hScrollBar2.Value -= delta * hScrollBar2.Value / sum;
                    hScrollBar3.Value -= delta * hScrollBar3.Value / sum;
                    hScrollBar5.Value -= delta * hScrollBar5.Value / sum;
                }
                else
                {
                    hScrollBar1.Value -= delta / 4;
                    hScrollBar2.Value -= delta / 4;
                    hScrollBar3.Value -= delta / 4;
                    hScrollBar5.Value -= delta / 4;
                }

                setValues();
                refreshLabels();
            }
        }

        private void hScrollBar5_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.Type.Equals(ScrollEventType.EndScroll))
            {
                int delta = (hScrollBar5.Value - _gameplayForm.speedBoostAppleChance);
                int sum = hScrollBar1.Value + hScrollBar2.Value + hScrollBar3.Value + hScrollBar4.Value;

                if (sum != 0)
                {
                    hScrollBar1.Value -= delta * hScrollBar1.Value / sum;
                    hScrollBar2.Value -= delta * hScrollBar2.Value / sum;
                    hScrollBar3.Value -= delta * hScrollBar3.Value / sum;
                    hScrollBar4.Value -= delta * hScrollBar4.Value / sum;
                }
                else
                {
                    hScrollBar1.Value -= delta / 4;
                    hScrollBar2.Value -= delta / 4;
                    hScrollBar3.Value -= delta / 4;
                    hScrollBar4.Value -= delta / 4;
                }

                setValues();
                refreshLabels();
            }
        }

        private void refreshLabels()
        {
            label6.Text = hScrollBar1.Value.ToString();
            label7.Text = hScrollBar2.Value.ToString();
            label8.Text = hScrollBar3.Value.ToString();
            label9.Text = hScrollBar4.Value.ToString();
            label10.Text = hScrollBar5.Value.ToString();
        }

        private void setValues()
        {
            _gameplayForm.growAppleChance = hScrollBar1.Value;
            _gameplayForm.doubleGrowAppleChance = hScrollBar2.Value;
            _gameplayForm.changeControlAppleChance = hScrollBar3.Value;
            _gameplayForm.speedBoostAppleChance = hScrollBar4.Value;
            _gameplayForm.blackAppleChance = hScrollBar5.Value;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int temp;
            if (textBox1.Text.Trim() == "")
            {

            }
            else if (!int.TryParse(textBox1.Text.Trim(), out temp))
            {
                MessageBox.Show("Invalid value");
            }
            else
            {
                _gameplayForm.appleCount = temp;
            }
        }
    }
}
