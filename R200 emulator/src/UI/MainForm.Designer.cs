namespace remu
{
    partial class MainForm
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
            this.flpRamPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.flpROM = new System.Windows.Forms.FlowLayoutPanel();
            this.flpConst = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flpRamPanel
            // 
            this.flpRamPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpRamPanel.Location = new System.Drawing.Point(0, 0);
            this.flpRamPanel.Name = "flpRamPanel";
            this.flpRamPanel.Size = new System.Drawing.Size(361, 168);
            this.flpRamPanel.TabIndex = 0;
            // 
            // flpROM
            // 
            this.flpROM.Location = new System.Drawing.Point(361, 0);
            this.flpROM.Name = "flpROM";
            this.flpROM.Size = new System.Drawing.Size(491, 473);
            this.flpROM.TabIndex = 1;
            // 
            // flpConst
            // 
            this.flpConst.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpConst.Location = new System.Drawing.Point(0, 174);
            this.flpConst.Name = "flpConst";
            this.flpConst.Size = new System.Drawing.Size(361, 299);
            this.flpConst.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 485);
            this.Controls.Add(this.flpConst);
            this.Controls.Add(this.flpROM);
            this.Controls.Add(this.flpRamPanel);
            this.DoubleBuffered = true;
            this.Name = "MainForm";
            this.Text = "R200";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpRamPanel;
        private System.Windows.Forms.FlowLayoutPanel flpROM;
        private System.Windows.Forms.FlowLayoutPanel flpConst;
    }
}