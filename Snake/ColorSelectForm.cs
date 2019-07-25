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
        List<Color> colors = new List<Color>();
        List<Label> labels = new List<Label>();
        List<HScrollBar> scrollBars = new List<HScrollBar>();
        List<PictureBox> previewPBs = new List<PictureBox>();
        Button saveButton = new Button();
        Random rnd = new Random();

        public ColorSelectForm()
        {
            InitializeComponent();

            var colorsTemp = Config.Instance.GetAll("Color");
            for (int i = 0; i < colorsTemp.Count; i++)
            {
                colors.Add(Color.FromArgb(Convert.ToInt32(colorsTemp["Color" + (i + 1)])));
            }

            string[] rgbChar = new string[] { "R", "G", "B" };

            for (int i = 0; i < Convert.ToInt32(Config.Instance.Get("PlayerCount")); i++)
            {
                if (i >= colors.Count)
                {
                    colors.Add(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));
                }
                
                labels.Add(new Label());
                labels[i * 4].Location = new Point(10, 9 + (9 * i) + (17 * 4 * i));
                labels[i * 4].AutoSize = true;
                labels[i * 4].Name = "label" + "Player" + i.ToString();
                labels[i * 4].Text = "Player " + (i + 1);
                labels[i * 4].Visible = true;
                this.Controls.Add(labels[i * 4]);

                for (int j = 0; j < 3; j++)
                {
                    labels.Add(new Label());
                    labels[i * 3 + (i + 1) + j].Location = new Point(10, 9 + 17 + (9 * i) + (17 * 4 * i) + (17 * j));
                    labels[i * 3 + (i + 1) + j].AutoSize = true;
                    labels[i * 3 + (i + 1) + j].Name = "label" + rgbChar[j] + i.ToString();
                    labels[i * 3 + (i + 1) + j].Text = rgbChar[j];
                    labels[i * 3 + (i + 1) + j].Visible = true;
                    this.Controls.Add(labels[i * 3 + (i + 1) + j]);

                    scrollBars.Add(new HScrollBar());
                    scrollBars[i * 3 + j].Location = new Point(25, 9 + 17 + (9 * i) + (17 * 4 * i) + (17 * j));
                    scrollBars[i * 3 + j].Maximum = 255;
                    scrollBars[i * 3 + j].Name = "scrollBar" + rgbChar[j] + i.ToString();
                    scrollBars[i * 3 + j].Size = new Size(80, 17);
                    scrollBars[i * 3 + j].Scroll += new ScrollEventHandler(this.scrollBar_Scroll);
                    this.Controls.Add(scrollBars[i * 3 + j]);
                }

                scrollBars[i * 3].Value = colors[i].R;
                scrollBars[i * 3 + 1].Value = colors[i].G;
                scrollBars[i * 3 + 2].Value = colors[i].B;
                
                int pbSize = (scrollBars[i * 3 + 1].Location.Y - scrollBars[i * 3].Location.Y) * 3;
                previewPBs.Add(new PictureBox());
                ((ISupportInitialize)(previewPBs[i])).BeginInit();
                previewPBs[i].Location = new System.Drawing.Point(scrollBars[i * 3].Location.X + scrollBars[i * 3].Size.Width + 5, scrollBars[i * 3].Location.Y);
                previewPBs[i].Name = "previewPB" + (i + 1);
                previewPBs[i].TabStop = false;
                previewPBs[i].Size = new Size(pbSize, pbSize);
                previewPBs[i].BackColor = colors[i];
                ((ISupportInitialize)(previewPBs[i])).EndInit();
                this.Controls.Add(previewPBs[i]);
            }

            saveButton.Name = "saveButton";
            saveButton.Size = new System.Drawing.Size(75, 23);
            saveButton.Location = new System.Drawing.Point(previewPBs.Last().Location.X + previewPBs.Last().Size.Width - saveButton.Size.Width, previewPBs.Last().Location.Y + previewPBs.Last().Size.Height + 9);
            saveButton.Name = "button1";
            saveButton.Text = "OK";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += new System.EventHandler(this.saveButton_Click);
            this.Controls.Add(saveButton);

            this.ClientSize = new System.Drawing.Size(saveButton.Location.X + saveButton.Size.Width + 10 , saveButton.Location.Y + saveButton.Size.Height + 9);
        }

        private void scrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            HScrollBar sb = (HScrollBar)sender;
            int colorid = Convert.ToInt32(sb.Name.Substring(10));
            colors[colorid] = Color.FromArgb(scrollBars[colorid * 3].Value, scrollBars[colorid * 3 + 1].Value, scrollBars[colorid * 3 + 2].Value);
            previewPBs[colorid].BackColor = colors[colorid];
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < colors.Count; i++)
            {
                if (Config.Instance.GetAll("Color" + (i + 1)).Count != 0)
                {
                    Config.Instance.Set("Color" + (i + 1), colors[i].ToArgb().ToString());
                }
                else
                {
                    Config.Instance.NewID("Color" + (i + 1), colors[i].ToArgb().ToString());
                }
            }
            Config.Instance.StoreCfgFile(); 
            this.Close();
        }
    }
}
