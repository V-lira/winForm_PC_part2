using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace WinFormsApp17
{
    public partial class Form1 : Form
    {
        private DomainUpDown dud;
        private CheckedListBox clist_box;
        private NumericUpDown num_ud;
        private CheckBox c_box;
        private TextBox t_Box;
        private Button btn_click;
        private Label cpu_lbl;
        private Label choose;
        private Label lbl_count;
        private PictureBox pBox;
        private ListBox list;
        private Label lbl;
        private const string LogFileName = "log.txt";
        public Form1()
        {
            InitializeComponents();
            elementS_in_form();
            load_from_file();
        }
        private void InitializeComponents()
        {
            this.Text = "PC configurator";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Padding = new Padding(10);
            this.Size = new Size(700, 600);
            this.MinimumSize = new Size(700, 600);

            cpu_lbl = new Label()
            {
                Location = new Point(20, 20),
                Size = new Size(150, 20),
                Text = "Choose CPU:",
                Font = new Font("Arial", 9, FontStyle.Regular),
            };
            dud = new DomainUpDown()
            {
                Location = new Point(20, 45),
                Size = new Size(150, 23),
            };
            dud.SelectedItemChanged += dud_selected;
            pBox = new PictureBox()
            {
                Location = new Point(180, 20),
                Size = new Size(120, 60),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle
            };
            choose = new Label()
            {
                Location = new Point(20, 80),
                Size = new Size(200, 20),
                Text = "Choose system components:",
                Font = new Font("Arial", 9, FontStyle.Regular)
            };
            clist_box = new CheckedListBox()
            {
                Location = new Point(20, 105),
                Size = new Size(250, 140),
            };
            lbl_count = new Label()
            {
                Location = new Point(20, 255),
                Size = new Size(150, 20),
                Text = "How many?",
                Font = new Font("Arial", 9, FontStyle.Regular)
            };
            num_ud = new NumericUpDown()
            {
                Location = new Point(20, 280),
                Size = new Size(100, 23),
            };
            c_box = new CheckBox()
            {
                Location = new Point(20, 320),
                Size = new Size(200, 25),
                Text = "building a PC (+5000 money)"
            };
            btn_click = new Button()
            {
                Location = new Point(20, 360),
                Size = new Size(100, 30),
                Text = "Calculate",
            };
            btn_click.Click += button_Click;
            t_Box = new TextBox()
            {
                Location = new Point(350, 45),
                Size = new Size(300, 120),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true
            };
            Label resultLabel = new Label()
            {
                Location = new Point(350, 20),
                Size = new Size(200, 20),
                Text = "Calculation result:",
                Font = new Font("Arial", 9, FontStyle.Regular)
            };
            lbl = new Label()
            {
                Location = new Point(350, 180),
                Size = new Size(200, 20),
                Text = "Order history:",
                Font = new Font("Arial", 9, FontStyle.Regular)
            };
            list = new ListBox()
            {
                Location = new Point(350, 205),
                Size = new Size(300, 250),
                ScrollAlwaysVisible = true
            };
            //||||||||||||||||||||||||||||
            this.Controls.Add(cpu_lbl);
            this.Controls.Add(dud);
            this.Controls.Add(choose);
            this.Controls.Add(clist_box);
            this.Controls.Add(lbl_count);
            this.Controls.Add(num_ud);
            this.Controls.Add(c_box);
            this.Controls.Add(btn_click);
            this.Controls.Add(t_Box);
            this.Controls.Add(lbl);
            this.Controls.Add(pBox);
            this.Controls.Add(list);
            this.Controls.Add(resultLabel);
            //||||||||||||||||||||||||||||
        }
        private void elementS_in_form()
        {
            dud.Items.Add("Intel");
            dud.Items.Add("AMD");
            dud.Items.Add("Apple");
            //default -> 0
            dud.SelectedIndex = 0;
            //i ADDED hdd
            clist_box.Items.Add("GPU");
            clist_box.Items.Add("RAM");
            clist_box.Items.Add("SSD");
            clist_box.Items.Add("HDD");
            clist_box.Items.Add("PC case");
            clist_box.Items.Add("PSU");
            clist_box.Items.Add("monitor");

            //NumericUpDown (num of systems)
            num_ud.Minimum = 1;
            num_ud.Maximum = 100;
            num_ud.Value = 1;
            t_Box.ReadOnly = true;
            t_Box.Multiline = true;
        }

        private void dud_selected(object sender, EventArgs e)
        {
            update_logo();
        }
        private void update_logo()
        {
            string imageName = "";
            switch (dud.Text)
            {
                case 
                "Intel": imageName = "C:\\Users\\a\\Downloads\\intel.jpg"; 
                    break;
                case 
                "AMD": imageName = "C:\\Users\\a\\Downloads\\amd.jpg"; 
                    break;
                case 
                "Apple": imageName = "C:\\Users\\a\\Downloads\\apple.jpg"; 
                    break;
            }
            try
            {
                if (File.Exists(imageName))
                {
                    pBox.Image = Image.FromFile(imageName);
                }
                else
                {
                    pBox.Image = create_logo_img(dud.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"error with loading file: {ex.Message}");
                pBox.Image = create_logo_img(dud.Text);
            }
        }
        private Image create_logo_img(string processorName)
        {
            Bitmap bmp = new Bitmap(120, 60);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.LightGray);
                using (Font font = new Font("Arial", 8, FontStyle.Bold))
                {
                    g.DrawString(processorName, font, Brushes.Black, 10, 20);
                }
                g.DrawRectangle(Pens.Black, 0, 0, 119, 59);
            }
            return bmp;
        }
        private void load_from_file()
        {
            if (File.Exists(LogFileName))
            {
                try
                {
                    string[] lines = File.ReadAllLines(LogFileName);
                    list.Items.Clear();
                    list.Items.AddRange(lines);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"error with lodaing log: {ex.Message}");
                }
            }
        }
        private void dave_file(string orderInfo)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(LogFileName))
                {
                    sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {orderInfo}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"error with saving log: {ex.Message}");
            }
        }
        private void button_Click(object sender, EventArgs e)
        {
            int cpuPrice = 0;
            switch (dud.Text)
            {
                case 
                "Intel": cpuPrice = 12000; 
                    break;
                case 
                "AMD": cpuPrice = 6000;
                    break;
                case 
                "Apple": cpuPrice = 45000; 
                    break;
            }
            int totalParts = 0;
            foreach (var item in clist_box.CheckedItems)
            {
                switch (item.ToString())
                {
                    case 
                    "GPU": totalParts += 16000; 
                        break;
                    case 
                    "RAM": totalParts += 8000; 
                        break;
                    case 
                    "SSD": totalParts += 5000;
                        break;
                    case 
                    "HDD": totalParts += 6000; 
                        break;
                    case
                    "PC case": totalParts += 3000;
                        break;
                    case 
                    "PSU": totalParts += 5000; 
                        break;
                    case 
                    "monitor": totalParts += 12000;
                        break;
                }
            }
            int assemblyCost = c_box.Checked 
                ? 5000 
                : 0;
            int count = (int)num_ud.Value;
            int total = (cpuPrice + totalParts + assemblyCost) * count;
            //info about your buying
            string orderInfo = $"{dud.Text} PC: {total} money (Qty: {count})";
            t_Box.Text = $"Processor: {dud.Text} ({cpuPrice} money)\r\n" +$"Components: {totalParts} money\r\n" + $"Building PC: {(c_box.Checked ? "Yes (+5000)" : "No")}\r\n" + $"Quantity: {count}\r\n\r\n" + $"TOTAL: {total} money";
            list.Items.Add($"{DateTime.Now:HH:mm:ss} - {orderInfo}");
            dave_file(orderInfo);
            list.TopIndex = list.Items.Count - 1;
        }
    }

}
