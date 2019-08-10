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

namespace Snake
{
    public partial class ControlSettingsForm : Form
    {
        Dictionary<string, string> backupControls = new Dictionary<string, string>();
        List<TextBox> textBoxes = new List<TextBox>();
        List<Label> labels = new List<Label>();
        Button saveButton = new Button();
        string[] binds = new string[]
             {
                "Up", "Down", "Left", "Right"
             };

        public ControlSettingsForm()
        {
            InitializeComponent();

            int rowCounter = 0;
            int leftMargin = 10;
            int topMargin = 9;
            int playerSpacing = 9;
            int textBoxSpacing = 22;
            int bindLabelTextBoxSpacing = 52;

            for (int i = 0; i < Convert.ToInt32(Config.Instance.Get("PlayerCount")); i++)
            {
                Label playerLabel = new Label();
                playerLabel.Location = new Point(leftMargin, topMargin + (playerSpacing * i) + (textBoxSpacing * rowCounter));
                playerLabel.AutoSize = true;
                playerLabel.Name = "label" + "Player" + (i + 1).ToString();
                playerLabel.Text = "Player " + (i + 1);
                playerLabel.Visible = true;
                this.Controls.Add(playerLabel);
                labels.Add(playerLabel);

                rowCounter++;

                for (int j = 0; j < 4; j++)
                {
                    string directionBind = binds[j];

                    Label directionBindLabel = new Label();
                    directionBindLabel.Location = new Point(leftMargin, topMargin + (playerSpacing * i) + (textBoxSpacing * rowCounter));
                    directionBindLabel.AutoSize = true;
                    directionBindLabel.Name = "label" + directionBind + (i + 1).ToString();
                    directionBindLabel.Text = directionBind + (i + 1);
                    directionBindLabel.Visible = true;
                    this.Controls.Add(directionBindLabel);
                    labels.Add(directionBindLabel);

                    TextBox directionBindTextBox = new TextBox();
                    directionBindTextBox.Location = new Point(bindLabelTextBoxSpacing, topMargin + (playerSpacing * i) + (textBoxSpacing * rowCounter));
                    directionBindTextBox.Name = "tb" + directionBind + (i + 1).ToString();
                    directionBindTextBox.Size = new Size(100, 20);
                    directionBindTextBox.Visible = true;
                    this.Controls.Add(directionBindTextBox);
                    textBoxes.Add(directionBindTextBox);

                    rowCounter++;
                }
            }

            saveButton.Name = "saveButton";
            saveButton.Size = new System.Drawing.Size(75, 23);
            saveButton.Location = new System.Drawing.Point(textBoxes.Last().Location.X + textBoxes.Last().Size.Width - saveButton.Size.Width, textBoxes.Last().Location.Y + textBoxes.Last().Size.Height + 9);
            saveButton.Name = "button1";
            saveButton.Text = "OK";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += new System.EventHandler(this.saveButton_Click);
            this.Controls.Add(saveButton);

            this.ClientSize = new System.Drawing.Size(saveButton.Location.X + saveButton.Size.Width + 10, saveButton.Location.Y + saveButton.Size.Height + 9);

            for (int i = 0; i < Convert.ToInt32(Config.Instance.Get("PlayerCount")); i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Config.Instance.IDExists(binds[j] + (i + 1)))
                    {
                        textBoxes[i * 4 + j].Text = Config.Instance.Get(binds[j] + (i + 1));
                    }
                    else
                    {
                        Config.Instance.NewID(binds[j] + (i + 1), "");
                    }

                    backupControls.Add(binds[j] + (i + 1), Config.Instance.Get(binds[j] + (i + 1)));
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            bool storeSuccesful = true;

            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (storeControl(textBoxes[i]) == false)
                {
                    storeSuccesful = false;
                }
            }

            if (storeSuccesful)
            {
                Config.Instance.StoreCfgFile();

                this.Close();
            }
            else
            {
                MessageBox.Show("Key(s) are invalid");
                restoreControls();
            }
        }

        private string firstCharToUpper(string input)
        {
            if (input.Length > 0)
            {
                return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
            }
            else
            {
                return "";
            }
        }

        private bool storeControl(TextBox tb)
        {
            Keys i;
            string c = firstCharToUpper(tb.Text);
            if (Enum.TryParse(c, out i)) //c == "" || 
            {
                Config.Instance.Set(tb.Name.Substring(2), c);
                return true;
            }
            else
            {
                return false;
            }
        }
        private void restoreControls()
        {
            foreach (var i in backupControls)
            {
                Config.Instance.Set(i.Key, i.Value);
            }
        }
    }
}
