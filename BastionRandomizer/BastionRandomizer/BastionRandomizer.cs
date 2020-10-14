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
using BastionRandomiztion.Properties;

namespace BastionRandomiztion
{
    public partial class BastionRandomizer : Form
    {
        int seed;
        Randomizer randomizer;
        string folderPath;

        public BastionRandomizer()
        {
            InitializeComponent();
            
            randomizer = new Randomizer();
        }

        private void BastionRandomizer_Load(object sender, EventArgs e)
        {
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;

            if (Settings.Default.SavedFolder == null)
            {
                folderBrowserDialog1.SelectedPath = AppDomain.CurrentDomain.BaseDirectory;
            }
            else
            {
                folderBrowserDialog1.SelectedPath = Settings.Default.SavedFolder;
            }

            PathTextBox.Text = folderBrowserDialog1.SelectedPath;
            folderPath = PathTextBox.Text;
            
            randomizer.randomizeLevels = RandomizeLevelOrder.Checked;
            randomizer.randomizeLoot = RandomizeLoot.Checked;
            randomizer.randomizeEnemies = RandomizeEnemies.Checked;
        }

        // Levels
        private void RandomizeLevelOrder_CheckedChanged(object sender, EventArgs e)
        {
            randomizer.randomizeLevels = RandomizeLevelOrder.Checked;
        }

        // Loot
        private void RandomizeLoot_CheckedChanged(object sender, EventArgs e)
        {
            randomizer.randomizeLoot = RandomizeLoot.Checked;
            LootOptions.Enabled = RandomizeLoot.Checked;
            GuaranteeWeapon.Enabled = RandomizeLoot.Checked;
            RandomizeHopscotch.Enabled = RandomizeLoot.Checked;
        }

        private void LootOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(LootOptions.SelectedIndex == 0)
            {
                randomizer.weapons = LootOptions.GetItemChecked(0);
            }
            else if(LootOptions.SelectedIndex == 1)
            {
                randomizer.abilities = LootOptions.GetItemChecked(1);
            }
            else if (LootOptions.SelectedIndex == 2)
            {
                randomizer.upgrades = LootOptions.GetItemChecked(2);
            }
            else if (LootOptions.SelectedIndex == 3)
            {
                randomizer.loot = LootOptions.GetItemChecked(3);
            }
        }

        private void GuaranteeWeapon_CheckedChanged(object sender, EventArgs e)
        {
            randomizer.guaranteeWeapon = GuaranteeWeapon.Checked;
        }

        private void RandomizeHopscotch_CheckedChanged(object sender, EventArgs e)
        {
            randomizer.randomizeHopscotch = RandomizeHopscotch.Checked;
        }

        // Enemies
        private void RandomizeEnemies_CheckedChanged(object sender, EventArgs e)
        {
            randomizer.randomizeEnemies = RandomizeEnemies.Checked;
        }

        // Set Seed
        private void SeedTextBox_TextChanged(object sender, EventArgs e)
        {
            if (SeedTextBox.Text != "")
                seed = int.Parse(SeedTextBox.Text);
        }

        private void SeedTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // Randomize
        private void Randomize_Click(object sender, EventArgs e)
        {
            if (randomizer.randomizeLevels == false && randomizer.randomizeLoot == false && randomizer.randomizeEnemies == false)
                return;

            if (SeedTextBox.Text == "")
                randomizer.rand = new Random();
            else
                randomizer.rand = new Random(seed);

            randomizer.BackupFiles(folderPath);

            if (randomizer.randomizeLevels)
                randomizer.RandomizeLevelOrder(folderPath);

            if (randomizer.randomizeLoot)
                randomizer.RandomizeLoot();

            if (randomizer.randomizeEnemies)
                randomizer.SetEnemyPackages(folderPath);

            randomizer.EditLevelScripts(folderPath);
        }

        // Unrandomize
        private void Unrandomize_Click(object sender, EventArgs e)
        {
            randomizer.RestoreFiles(folderPath);
        }

        // Game Data Folder
        private void FolderSelect_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.SavedFolder = folderBrowserDialog1.SelectedPath;
                PathTextBox.Text = folderBrowserDialog1.SelectedPath;
                folderPath = PathTextBox.Text;
            }
        }

        private void PathTextBox_TextChanged(object sender, EventArgs e)
        {
            folderPath = PathTextBox.Text;
        }

    }
}
