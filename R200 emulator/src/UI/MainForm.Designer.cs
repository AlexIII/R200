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
            this.components = new System.ComponentModel.Container();
            this.flpROM = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flpRamPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.flpConst = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.flpRegisters = new System.Windows.Forms.FlowLayoutPanel();
            this.repaintTimer = new System.Windows.Forms.Timer(this.components);
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnStep = new System.Windows.Forms.ToolStripButton();
            this.btnRun = new System.Windows.Forms.ToolStripButton();
            this.btnRun10x = new System.Windows.Forms.ToolStripButton();
            this.btnRunOnMaximumSpeed = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPause = new System.Windows.Forms.ToolStripButton();
            this.btnStop = new System.Windows.Forms.ToolStripButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtSourceCode = new ScrollbarPosition.MultilineScrollableTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpROM
            // 
            this.flpROM.Location = new System.Drawing.Point(683, 245);
            this.flpROM.Name = "flpROM";
            this.flpROM.Size = new System.Drawing.Size(359, 67);
            this.flpROM.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.flpRamPanel);
            this.groupBox1.Location = new System.Drawing.Point(12, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(343, 206);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "RAM";
            // 
            // flpRamPanel
            // 
            this.flpRamPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpRamPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpRamPanel.Location = new System.Drawing.Point(3, 16);
            this.flpRamPanel.Name = "flpRamPanel";
            this.flpRamPanel.Size = new System.Drawing.Size(337, 187);
            this.flpRamPanel.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.flpConst);
            this.groupBox2.Location = new System.Drawing.Point(361, 33);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(684, 206);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "CONST";
            // 
            // flpConst
            // 
            this.flpConst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpConst.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpConst.Location = new System.Drawing.Point(3, 16);
            this.flpConst.Name = "flpConst";
            this.flpConst.Size = new System.Drawing.Size(678, 187);
            this.flpConst.TabIndex = 3;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.flpRegisters);
            this.groupBox3.Location = new System.Drawing.Point(12, 245);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(654, 70);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "REGS";
            // 
            // flpRegisters
            // 
            this.flpRegisters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpRegisters.Location = new System.Drawing.Point(3, 16);
            this.flpRegisters.Name = "flpRegisters";
            this.flpRegisters.Size = new System.Drawing.Size(648, 51);
            this.flpRegisters.TabIndex = 0;
            // 
            // repaintTimer
            // 
            this.repaintTimer.Enabled = true;
            this.repaintTimer.Tick += new System.EventHandler(this.repaintTimer_Tick);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnStep,
            this.btnRun,
            this.btnRun10x,
            this.btnRunOnMaximumSpeed,
            this.toolStripSeparator1,
            this.btnPause,
            this.btnStop});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1057, 25);
            this.toolStrip.TabIndex = 6;
            this.toolStrip.Text = "toolStrip1";
            // 
            // btnStep
            // 
            this.btnStep.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnStep.Image = global::remu.Properties.Resources.Step;
            this.btnStep.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStep.Name = "btnStep";
            this.btnStep.Size = new System.Drawing.Size(23, 22);
            this.btnStep.Text = "Step by Step Execution";
            this.btnStep.Click += new System.EventHandler(this.btnStep_Click);
            // 
            // btnRun
            // 
            this.btnRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRun.Image = global::remu.Properties.Resources.Play;
            this.btnRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(23, 22);
            this.btnRun.Tag = "450";
            this.btnRun.Text = "Run at Real Speed";
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnRun10x
            // 
            this.btnRun10x.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRun10x.Image = global::remu.Properties.Resources.Play10;
            this.btnRun10x.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRun10x.Name = "btnRun10x";
            this.btnRun10x.Size = new System.Drawing.Size(23, 22);
            this.btnRun10x.Tag = "45";
            this.btnRun10x.Text = "Run at x10 Speed";
            this.btnRun10x.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnRunOnMaximumSpeed
            // 
            this.btnRunOnMaximumSpeed.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRunOnMaximumSpeed.Image = global::remu.Properties.Resources.PlayInf;
            this.btnRunOnMaximumSpeed.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRunOnMaximumSpeed.Name = "btnRunOnMaximumSpeed";
            this.btnRunOnMaximumSpeed.Size = new System.Drawing.Size(23, 22);
            this.btnRunOnMaximumSpeed.Tag = "0";
            this.btnRunOnMaximumSpeed.Text = "Run at Maximum Speed";
            this.btnRunOnMaximumSpeed.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnPause
            // 
            this.btnPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPause.Image = global::remu.Properties.Resources.Pause;
            this.btnPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(23, 22);
            this.btnPause.Text = "Pause";
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStop
            // 
            this.btnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnStop.Image = global::remu.Properties.Resources.Stop;
            this.btnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(23, 22);
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox4.Controls.Add(this.txtSourceCode);
            this.groupBox4.Location = new System.Drawing.Point(15, 321);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1027, 152);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Source code";
            // 
            // txtSourceCode
            // 
            this.txtSourceCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSourceCode.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtSourceCode.Location = new System.Drawing.Point(3, 16);
            this.txtSourceCode.Multiline = true;
            this.txtSourceCode.Name = "txtSourceCode";
            this.txtSourceCode.ReadOnly = true;
            this.txtSourceCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSourceCode.Size = new System.Drawing.Size(1021, 133);
            this.txtSourceCode.TabIndex = 8;
            this.txtSourceCode.ScrollPositionChanged += new System.Windows.Forms.ScrollEventHandler(this.txtSourceCode_ScrollPositionChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 485);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.flpROM);
            this.DoubleBuffered = true;
            this.Name = "MainForm";
            this.Text = "R200";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flpROM;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.FlowLayoutPanel flpRamPanel;
        private System.Windows.Forms.FlowLayoutPanel flpConst;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.FlowLayoutPanel flpRegisters;
        private System.Windows.Forms.Timer repaintTimer;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnRun;
        private System.Windows.Forms.ToolStripButton btnRun10x;
        private System.Windows.Forms.ToolStripButton btnRunOnMaximumSpeed;
        private System.Windows.Forms.ToolStripButton btnPause;
        private System.Windows.Forms.ToolStripButton btnStop;
        private System.Windows.Forms.ToolStripButton btnStep;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.GroupBox groupBox4;
        private ScrollbarPosition.MultilineScrollableTextBox txtSourceCode;
    }
}