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
        string[] rgbChar = new string[] 
            {
                "R", "G", "B"
            };

        public ColorSelectForm()
        {
            InitializeComponent();

            var colorsTemp = Config.Instance.GetAll("Color");
            for (int i = 0; i < colorsTemp.Count; i++)
            {
                colors.Add(Color.FromArgb(Convert.ToInt32(colorsTemp["Color" + (i + 1)])));
            }

            int rowCounter = 0;
            int leftMargin = 10;
            int topMargin = 9;
            int playerSpacing = 9;
            int scrollBarSpacing = 17;
            int rgbLabelTextBoxSpacing = 25;

            for (int i = 0; i < Convert.ToInt32(Config.Instance.Get("PlayerCount")); i++)
            {
                if (i >= colors.Count)
                {
                    colors.Add(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));
                }

                Label playerLabel = new Label();
                playerLabel.Location = new Point(leftMargin, topMargin + (playerSpacing * i) + (scrollBarSpacing * rowCounter));
                playerLabel.AutoSize = true;
                playerLabel.Name = "label" + "Player" + i.ToString();
                playerLabel.Text = "Player " + (i + 1);
                playerLabel.Visible = true;
                this.Controls.Add(playerLabel);
                labels.Add(playerLabel);
                rowCounter++;

                for (int j = 0; j < 3; j++)
                {
                    Label colorLabel = new Label();
                    colorLabel.Location = new Point(leftMargin, topMargin + (playerSpacing * i) + (scrollBarSpacing * rowCounter));
                    colorLabel.AutoSize = true;
                    colorLabel.Name = "label" + rgbChar[j] + i.ToString();
                    colorLabel.Text = rgbChar[j];
                    colorLabel.Visible = true;
                    this.Controls.Add(colorLabel);
                    labels.Add(colorLabel);

                    HScrollBar colorScrollBar = new HScrollBar();
                    colorScrollBar.Location = new Point(rgbLabelTextBoxSpacing, topMargin + (playerSpacing * i) + (scrollBarSpacing * rowCounter));
                    colorScrollBar.Maximum = 255;
                    colorScrollBar.Name = "scrollBar" + rgbChar[j] + i.ToString();
                    colorScrollBar.Size = new Size(80, 17);
                    colorScrollBar.Scroll += new ScrollEventHandler(this.scrollBar_Scroll);
                    this.Controls.Add(colorScrollBar);
                    scrollBars.Add(colorScrollBar);

                    rowCounter++;
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
