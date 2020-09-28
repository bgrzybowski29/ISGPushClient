using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ISGPushClient
{
    public partial class Form1 : Form
    {
        public static JObject appConfig = null;
        string myConfig=null;
       
        public Form1()
        {
            InitializeComponent();
            myConfig = File.ReadAllText("oedotnet.json");
            appConfig = JObject.Parse(myConfig);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lstMessages.Items.Add("form Loading");
            appConfig = JObject.Parse(myConfig);
            var regions =
                from p in appConfig["Application"]["Regions"]
                select ((Newtonsoft.Json.Linq.JProperty)p).Name;
            foreach (var item in regions)
            {
                cboEnvironment.Items.Add(item);
                //Console.WriteLine(item);
            }
            lstMessages.Items.Add("regions loaded");
        }

        private void cboEnvironment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedItem != null)
            {
                lstMessages.Items.Add("region: " + ((ComboBox)sender).SelectedItem + " selected");
                Console.WriteLine(((ComboBox)sender).SelectedItem);
                cboTopic.SelectedItem = null;
                cboTopic.Items.Clear();
                var topics =
                    from p in appConfig["Application"]["Regions"][((ComboBox)sender).SelectedItem]["topics"]
                    select (string)p["topic"];
                foreach (var item in topics)
                {
                    cboTopic.Items.Add(item);
                }
                lstMessages.Items.Add("topics loaded");
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Browse Text Files",
                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtInputFromFile.Text = openFileDialog1.FileName;
            }
            lstMessages.Items.Add("file: " + openFileDialog1.FileName + " selected");
        }

        private void cboTopic_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstMessages.Items.Add("topic: " + ((ComboBox)sender).SelectedItem + " selected");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lstMessages.Items.Clear();
        }

        private void btnPush_Click(object sender, EventArgs e)
        {
            if (txtInput.Text!="" && txtInputFromFile.Text!="") MessageBox.Show("You can select either a message or input file.");
        }

        private void btnClearForm_Click(object sender, EventArgs e)
        {
            cboEnvironment.SelectedItem = null;
            cboTopic.Items.Clear();
            cboTopic.SelectedItem = null;
            cboTopic.Text = "";
            txtInput.Text = "";
            txtInputFromFile.Text = "";
            lstMessages.Items.Clear();
        }
    }
}
