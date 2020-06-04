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
        int seed;
        Random rand;
        Randomizer randomizer;
        string folderPath;

        public Form1()
        {
            InitializeComponent();
            
            randomizer = new Randomizer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;
            folderBrowserDialog1.SelectedPath = AppDomain.CurrentDomain.BaseDirectory;
            PathTextBox.Text = folderBrowserDialog1.SelectedPath;
            folderPath = PathTextBox.Text;
            WeaponComboBox.Text = "Fully Random";
            randomizer.randomizeLevels = checkBox1.Checked;
            randomizer.randomizeWeapons = checkBox2.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                randomizer.randomizeLevels = true;
            }
            else
            {
                randomizer.randomizeLevels = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                randomizer.randomizeWeapons = true;
                WeaponComboBox.Enabled = true;
            }
            else
            {
                randomizer.randomizeWeapons = false;
                WeaponComboBox.Enabled = false;
            }
        }

        private void Randomize_Click(object sender, EventArgs e)
        {
            if (randomizer.randomizeLevels == false && randomizer.randomizeWeapons == false)
                return;

            if (SeedTextBox.Text == "")
                rand = new Random();
            else
                rand = new Random(seed);

            randomizer.BackupFiles(folderPath);

            if (randomizer.randomizeLevels)
                randomizer.RandomizeLevelOrder(rand, folderPath);

            if (randomizer.randomizeWeapons)
                randomizer.RandomizeWeapons(rand, folderPath);

            randomizer.EditLevelScripts(folderPath);
        }

        private void Unrandomize_Click(object sender, EventArgs e)
        {
            randomizer.RestoreFiles(folderPath);
        }

        private void FolderSelect_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                PathTextBox.Text = folderBrowserDialog1.SelectedPath;
                folderPath = PathTextBox.Text;
            }
        }

        private void PathTextBox_TextChanged(object sender, EventArgs e)
        {
            folderPath = PathTextBox.Text;
        }

        private void WeaponComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(WeaponComboBox.Text == "Fully Randomized")
            {
                randomizer.weaponRandomizationType = WeaponRandomization.FullyRandom;
            }
            else if (WeaponComboBox.Text == "Weapons & Abilities Split")
            {
                randomizer.weaponRandomizationType = WeaponRandomization.WeaponAbilitySplit;
            }
        }

        private void SeedTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void SeedTextBox_TextChanged(object sender, EventArgs e)
        {
            if (SeedTextBox.Text != "")
                seed = int.Parse(SeedTextBox.Text);
        }
    }
}
