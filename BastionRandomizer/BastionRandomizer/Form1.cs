using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        bool randomizeMap = true;
        bool randomizeWeapons;
        Random rand;
        Randomizer randomizer;
        string folderPath;

        public Form1()
        {
            InitializeComponent();

            rand = new Random();
            randomizer = new Randomizer();
            randomizer.Init();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;
            folderBrowserDialog1.SelectedPath = AppDomain.CurrentDomain.BaseDirectory;
            richTextBox1.Text = folderBrowserDialog1.SelectedPath;
            folderPath = richTextBox1.Text;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                randomizeMap = true;
            }
            else
            {
                randomizeMap = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                randomizeWeapons = true;
            }
            else
            {
                randomizeWeapons = false;
            }
        }

        private void Randomize_Click(object sender, EventArgs e)
        {
            if (randomizeMap == false && randomizeWeapons == false)
                return;

            randomizer.BackupFiles(folderPath);

            if(randomizeMap)
                randomizer.RandomizeLevelOrder(rand, folderPath);
        }

        private void Unrandomize_Click(object sender, EventArgs e)
        {
            randomizer.RestoreFiles(folderPath);
        }

        private void FolderSelect_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Text = folderBrowserDialog1.SelectedPath;
                folderPath = richTextBox1.Text;
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            folderPath = richTextBox1.Text;
        }
    }
}
