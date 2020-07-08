namespace BastionRandomiztion
{
    partial class BastionRandomizer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.RandomizeLevelOrder = new System.Windows.Forms.CheckBox();
            this.Randomize = new System.Windows.Forms.Button();
            this.RandomizeLoot = new System.Windows.Forms.CheckBox();
            this.Unrandomize = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.FolderSelect = new System.Windows.Forms.Button();
            this.PathTextBox = new System.Windows.Forms.RichTextBox();
            this.SeedTextBox = new System.Windows.Forms.TextBox();
            this.SeedLabel = new System.Windows.Forms.Label();
            this.RemoveCutscenes = new System.Windows.Forms.CheckBox();
            this.RemoveHub = new System.Windows.Forms.CheckBox();
            this.LootOptions = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // RandomizeLevelOrder
            // 
            this.RandomizeLevelOrder.AutoSize = true;
            this.RandomizeLevelOrder.Checked = true;
            this.RandomizeLevelOrder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RandomizeLevelOrder.Location = new System.Drawing.Point(12, 12);
            this.RandomizeLevelOrder.Name = "RandomizeLevelOrder";
            this.RandomizeLevelOrder.Size = new System.Drawing.Size(137, 17);
            this.RandomizeLevelOrder.TabIndex = 0;
            this.RandomizeLevelOrder.Text = "Randomize Level Order";
            this.RandomizeLevelOrder.UseVisualStyleBackColor = true;
            this.RandomizeLevelOrder.CheckedChanged += new System.EventHandler(this.RandomizeLevelOrder_CheckedChanged);
            // 
            // Randomize
            // 
            this.Randomize.Location = new System.Drawing.Point(12, 149);
            this.Randomize.Name = "Randomize";
            this.Randomize.Size = new System.Drawing.Size(91, 23);
            this.Randomize.TabIndex = 1;
            this.Randomize.Text = "Randomize";
            this.Randomize.UseVisualStyleBackColor = true;
            this.Randomize.Click += new System.EventHandler(this.Randomize_Click);
            // 
            // RandomizeLoot
            // 
            this.RandomizeLoot.AutoSize = true;
            this.RandomizeLoot.Checked = true;
            this.RandomizeLoot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RandomizeLoot.Location = new System.Drawing.Point(170, 12);
            this.RandomizeLoot.Name = "RandomizeLoot";
            this.RandomizeLoot.Size = new System.Drawing.Size(103, 17);
            this.RandomizeLoot.TabIndex = 2;
            this.RandomizeLoot.Text = "Randomize Loot";
            this.RandomizeLoot.UseVisualStyleBackColor = true;
            this.RandomizeLoot.CheckedChanged += new System.EventHandler(this.RandomizeLoot_CheckedChanged);
            // 
            // Unrandomize
            // 
            this.Unrandomize.Location = new System.Drawing.Point(116, 149);
            this.Unrandomize.Name = "Unrandomize";
            this.Unrandomize.Size = new System.Drawing.Size(91, 23);
            this.Unrandomize.TabIndex = 3;
            this.Unrandomize.Text = "Unrandomize";
            this.Unrandomize.UseVisualStyleBackColor = true;
            this.Unrandomize.Click += new System.EventHandler(this.Unrandomize_Click);
            // 
            // FolderSelect
            // 
            this.FolderSelect.Location = new System.Drawing.Point(12, 178);
            this.FolderSelect.Name = "FolderSelect";
            this.FolderSelect.Size = new System.Drawing.Size(91, 23);
            this.FolderSelect.TabIndex = 4;
            this.FolderSelect.Text = "Select Folder";
            this.FolderSelect.UseVisualStyleBackColor = true;
            this.FolderSelect.Click += new System.EventHandler(this.FolderSelect_Click);
            // 
            // PathTextBox
            // 
            this.PathTextBox.Location = new System.Drawing.Point(116, 178);
            this.PathTextBox.Multiline = false;
            this.PathTextBox.Name = "PathTextBox";
            this.PathTextBox.Size = new System.Drawing.Size(190, 23);
            this.PathTextBox.TabIndex = 5;
            this.PathTextBox.Text = "";
            this.PathTextBox.TextChanged += new System.EventHandler(this.PathTextBox_TextChanged);
            // 
            // SeedTextBox
            // 
            this.SeedTextBox.Location = new System.Drawing.Point(47, 113);
            this.SeedTextBox.MaxLength = 9;
            this.SeedTextBox.Name = "SeedTextBox";
            this.SeedTextBox.Size = new System.Drawing.Size(93, 20);
            this.SeedTextBox.TabIndex = 8;
            this.SeedTextBox.TextChanged += new System.EventHandler(this.SeedTextBox_TextChanged);
            this.SeedTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SeedTextBox_KeyPress);
            // 
            // SeedLabel
            // 
            this.SeedLabel.AutoSize = true;
            this.SeedLabel.Location = new System.Drawing.Point(9, 116);
            this.SeedLabel.Name = "SeedLabel";
            this.SeedLabel.Size = new System.Drawing.Size(32, 13);
            this.SeedLabel.TabIndex = 9;
            this.SeedLabel.Text = "Seed";
            // 
            // RemoveCutscenes
            // 
            this.RemoveCutscenes.AutoSize = true;
            this.RemoveCutscenes.Location = new System.Drawing.Point(12, 58);
            this.RemoveCutscenes.Name = "RemoveCutscenes";
            this.RemoveCutscenes.Size = new System.Drawing.Size(119, 17);
            this.RemoveCutscenes.TabIndex = 10;
            this.RemoveCutscenes.Text = "Remove Cutscenes";
            this.RemoveCutscenes.UseVisualStyleBackColor = true;
            this.RemoveCutscenes.CheckedChanged += new System.EventHandler(this.RemoveCutscenes_CheckedChanged);
            // 
            // RemoveHub
            // 
            this.RemoveHub.AutoSize = true;
            this.RemoveHub.Location = new System.Drawing.Point(12, 81);
            this.RemoveHub.Name = "RemoveHub";
            this.RemoveHub.Size = new System.Drawing.Size(118, 17);
            this.RemoveHub.TabIndex = 11;
            this.RemoveHub.Text = "Remove Hub Level";
            this.RemoveHub.UseVisualStyleBackColor = true;
            this.RemoveHub.CheckedChanged += new System.EventHandler(this.RemoveHub_CheckedChanged);
            // 
            // LootOptions
            // 
            this.LootOptions.BackColor = System.Drawing.SystemColors.Control;
            this.LootOptions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LootOptions.CheckOnClick = true;
            this.LootOptions.FormattingEnabled = true;
            this.LootOptions.Items.AddRange(new object[] {
            "Weapons",
            "Abilities",
            "Upgrades",
            "Loot"});
            this.LootOptions.Location = new System.Drawing.Point(188, 35);
            this.LootOptions.Name = "LootOptions";
            this.LootOptions.Size = new System.Drawing.Size(70, 60);
            this.LootOptions.TabIndex = 12;
            this.LootOptions.SelectedIndexChanged += new System.EventHandler(this.LootOptions_SelectedIndexChanged);
            // 
            // BastionRandomizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 213);
            this.Controls.Add(this.LootOptions);
            this.Controls.Add(this.RemoveHub);
            this.Controls.Add(this.RemoveCutscenes);
            this.Controls.Add(this.SeedLabel);
            this.Controls.Add(this.SeedTextBox);
            this.Controls.Add(this.PathTextBox);
            this.Controls.Add(this.FolderSelect);
            this.Controls.Add(this.Unrandomize);
            this.Controls.Add(this.RandomizeLoot);
            this.Controls.Add(this.Randomize);
            this.Controls.Add(this.RandomizeLevelOrder);
            this.Name = "BastionRandomizer";
            this.Text = "Bastion Randomizer";
            this.Load += new System.EventHandler(this.BastionRandomizer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox RandomizeLevelOrder;
        private System.Windows.Forms.Button Randomize;
        private System.Windows.Forms.CheckBox RandomizeLoot;
        private System.Windows.Forms.Button Unrandomize;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button FolderSelect;
        private System.Windows.Forms.RichTextBox PathTextBox;
        private System.Windows.Forms.TextBox SeedTextBox;
        private System.Windows.Forms.Label SeedLabel;
        private System.Windows.Forms.CheckBox RemoveCutscenes;
        private System.Windows.Forms.CheckBox RemoveHub;
        private System.Windows.Forms.CheckedListBox LootOptions;
    }
}

