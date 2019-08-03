using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Snake
{
    public partial class AppleSettingsForm : Form
    {
        List<HScrollBar> scrollBars = new List<HScrollBar>();
        List<float> appleChances = new List<float>();

        public AppleSettingsForm()
        {
            InitializeComponent();

            appleChances.Add(float.Parse(Config.Instance.Get("GrowAppleChance")));
            appleChances.Add(float.Parse(Config.Instance.Get("DoubleGrowAppleChance")));
            appleChances.Add(float.Parse(Config.Instance.Get("ChangeControlAppleChance")));
            appleChances.Add(float.Parse(Config.Instance.Get("SpeedBoostAppleChance")));
            appleChances.Add(float.Parse(Config.Instance.Get("BlackAppleChance")));

            numericUpDown1.Value = Convert.ToInt32(Config.Instance.Get("AppleCount"));
            
            scrollBars.Add(hScrollBar1);
            scrollBars.Add(hScrollBar2);
            scrollBars.Add(hScrollBar3);
            scrollBars.Add(hScrollBar4);
            scrollBars.Add(hScrollBar5);

            for (int i = 0; i < 5; i++)
            {
                scrollBars[i].Value = (int)appleChances[i];
            }

            refreshLabels();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            HScrollBar s = (HScrollBar)sender;
            int index = scrollBars.IndexOf(s);
            float delta = s.Value - appleChances[index];
            float other_sum_previous = appleChances.Sum() - appleChances[index];

            //other_sum_previous = chances.sum() - chance(i)
            //other_sum_new = other_sum_previous - delta

            appleChances[index] = scrollBars[index].Value;

            if (other_sum_previous != 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (s != scrollBars[i])
                    {
                        appleChances[i] -= delta * appleChances[i] / other_sum_previous;
                        //new_chance[j] = chance[j] * other_sum_new / other_sum_previous = chance[j] * (other_sum_previous - delta) / other_sum_previous
                        //= chance[j] * (other_sum_previous - delta) / other_sum_previous = 
                        //= chance[j] * other_sum_previous / other_sum_previous - chance[j] * delta / other_sum_previous
                    }
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    if (s != scrollBars[i])
                    {
                        appleChances[i] -= delta / 4;
                    }
                }
            }

            for (int i = 0; i < 5; i++)
            {
                if (appleChances[i] < 0)
                    appleChances[i] = 0;
                scrollBars[i].Value = (int)appleChances[i];
            }

            refreshLabels();
        }

        private void refreshLabels()
        {
            label6.Text = hScrollBar1.Value.ToString();
            label7.Text = hScrollBar2.Value.ToString();
            label8.Text = hScrollBar3.Value.ToString();
            label9.Text = hScrollBar4.Value.ToString();
            label10.Text = hScrollBar5.Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //int appleCount;
            //if (!int.TryParse(numericUpDown1.Text.Trim(), out appleCount))
            //{
            //    MessageBox.Show("Invalid value");
            //    return;
            //}

            Config.Instance.Set("GrowAppleChance", Convert.ToInt32(appleChances[0]).ToString());
            Config.Instance.Set("DoubleGrowAppleChance", Convert.ToInt32(appleChances[1]).ToString());
            Config.Instance.Set("ChangeControlAppleChance", Convert.ToInt32(appleChances[2]).ToString());
            Config.Instance.Set("SpeedBoostAppleChance", Convert.ToInt32(appleChances[3]).ToString());
            Config.Instance.Set("BlackAppleChance", Convert.ToInt32(appleChances[4]).ToString());

            Config.Instance.Set("AppleCount", numericUpDown1.Value.ToString());

            Config.Instance.StoreCfgFile();

            this.Close();
        }
    }
}
