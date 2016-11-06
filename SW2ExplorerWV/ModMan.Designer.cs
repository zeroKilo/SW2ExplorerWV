namespace SW2ExplorerWV
{
    partial class ModMan
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.modsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.saveModJobsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadModJobsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addModJobsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jobsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executeSelectedJobToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportJobDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executeAllJobsForThisFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executeAllJobsForAllFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modsToolStripMenuItem,
            this.jobsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(788, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listBox2);
            this.splitContainer1.Size = new System.Drawing.Size(788, 470);
            this.splitContainer1.SplitterDistance = 379;
            this.splitContainer1.TabIndex = 1;
            // 
            // modsToolStripMenuItem
            // 
            this.modsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadModJobsToolStripMenuItem,
            this.saveModJobsToolStripMenuItem,
            this.addModJobsToolStripMenuItem});
            this.modsToolStripMenuItem.Name = "modsToolStripMenuItem";
            this.modsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.modsToolStripMenuItem.Text = "Mods";
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.listBox1.FormattingEnabled = true;
            this.listBox1.IntegralHeight = false;
            this.listBox1.ItemHeight = 14;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(379, 470);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // listBox2
            // 
            this.listBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox2.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.listBox2.FormattingEnabled = true;
            this.listBox2.IntegralHeight = false;
            this.listBox2.ItemHeight = 14;
            this.listBox2.Location = new System.Drawing.Point(0, 0);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(405, 470);
            this.listBox2.TabIndex = 0;
            // 
            // saveModJobsToolStripMenuItem
            // 
            this.saveModJobsToolStripMenuItem.Name = "saveModJobsToolStripMenuItem";
            this.saveModJobsToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.saveModJobsToolStripMenuItem.Text = "Save Mod Jobs";
            this.saveModJobsToolStripMenuItem.Click += new System.EventHandler(this.saveModJobsToolStripMenuItem_Click);
            // 
            // loadModJobsToolStripMenuItem
            // 
            this.loadModJobsToolStripMenuItem.Name = "loadModJobsToolStripMenuItem";
            this.loadModJobsToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.loadModJobsToolStripMenuItem.Text = "Clear and Load Mod Jobs";
            this.loadModJobsToolStripMenuItem.Click += new System.EventHandler(this.loadModJobsToolStripMenuItem_Click);
            // 
            // addModJobsToolStripMenuItem
            // 
            this.addModJobsToolStripMenuItem.Name = "addModJobsToolStripMenuItem";
            this.addModJobsToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.addModJobsToolStripMenuItem.Text = "Add Mod Jobs";
            this.addModJobsToolStripMenuItem.Click += new System.EventHandler(this.addModJobsToolStripMenuItem_Click);
            // 
            // jobsToolStripMenuItem
            // 
            this.jobsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportJobDataToolStripMenuItem,
            this.executeSelectedJobToolStripMenuItem,
            this.executeAllJobsForThisFileToolStripMenuItem,
            this.executeAllJobsForAllFilesToolStripMenuItem});
            this.jobsToolStripMenuItem.Name = "jobsToolStripMenuItem";
            this.jobsToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.jobsToolStripMenuItem.Text = "Jobs";
            // 
            // executeSelectedJobToolStripMenuItem
            // 
            this.executeSelectedJobToolStripMenuItem.Name = "executeSelectedJobToolStripMenuItem";
            this.executeSelectedJobToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.executeSelectedJobToolStripMenuItem.Text = "Execute Selected Job";
            this.executeSelectedJobToolStripMenuItem.Click += new System.EventHandler(this.executeSelectedJobToolStripMenuItem_Click);
            // 
            // exportJobDataToolStripMenuItem
            // 
            this.exportJobDataToolStripMenuItem.Name = "exportJobDataToolStripMenuItem";
            this.exportJobDataToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.exportJobDataToolStripMenuItem.Text = "Export Job Data";
            // 
            // executeAllJobsForThisFileToolStripMenuItem
            // 
            this.executeAllJobsForThisFileToolStripMenuItem.Name = "executeAllJobsForThisFileToolStripMenuItem";
            this.executeAllJobsForThisFileToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.executeAllJobsForThisFileToolStripMenuItem.Text = "Execute All Jobs for selected File";
            this.executeAllJobsForThisFileToolStripMenuItem.Click += new System.EventHandler(this.executeAllJobsForThisFileToolStripMenuItem_Click);
            // 
            // executeAllJobsForAllFilesToolStripMenuItem
            // 
            this.executeAllJobsForAllFilesToolStripMenuItem.Name = "executeAllJobsForAllFilesToolStripMenuItem";
            this.executeAllJobsForAllFilesToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.executeAllJobsForAllFilesToolStripMenuItem.Text = "Execute All Jobs for All Files";
            this.executeAllJobsForAllFilesToolStripMenuItem.Click += new System.EventHandler(this.executeAllJobsForAllFilesToolStripMenuItem_Click);
            // 
            // ModMan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 494);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ModMan";
            this.Text = "Mod Manager";
            this.Load += new System.EventHandler(this.ModMan_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem modsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadModJobsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveModJobsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addModJobsToolStripMenuItem;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.ToolStripMenuItem jobsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportJobDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem executeSelectedJobToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem executeAllJobsForThisFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem executeAllJobsForAllFilesToolStripMenuItem;
    }
}