namespace CleverMaple
{
    partial class Form1
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
            this.toggleButton = new System.Windows.Forms.Button();
            this.HpChbox = new System.Windows.Forms.CheckBox();
            this.MpChbox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.HealTimerNum = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.HealTimer = new System.Windows.Forms.Timer(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.HealTimerNum)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toggleButton
            // 
            this.toggleButton.Location = new System.Drawing.Point(197, 226);
            this.toggleButton.Name = "toggleButton";
            this.toggleButton.Size = new System.Drawing.Size(75, 23);
            this.toggleButton.TabIndex = 0;
            this.toggleButton.Text = "Start";
            this.toggleButton.UseVisualStyleBackColor = true;
            this.toggleButton.Click += new System.EventHandler(this.buttonToggle_Click);
            // 
            // HpChbox
            // 
            this.HpChbox.AutoSize = true;
            this.HpChbox.Checked = true;
            this.HpChbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HpChbox.Font = new System.Drawing.Font("Microsoft JhengHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.HpChbox.Location = new System.Drawing.Point(9, 21);
            this.HpChbox.Name = "HpChbox";
            this.HpChbox.Size = new System.Drawing.Size(43, 20);
            this.HpChbox.TabIndex = 1;
            this.HpChbox.Text = "HP";
            this.HpChbox.UseVisualStyleBackColor = true;
            // 
            // MpChbox
            // 
            this.MpChbox.AutoSize = true;
            this.MpChbox.Checked = true;
            this.MpChbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MpChbox.Font = new System.Drawing.Font("Microsoft JhengHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.MpChbox.Location = new System.Drawing.Point(9, 49);
            this.MpChbox.Name = "MpChbox";
            this.MpChbox.Size = new System.Drawing.Size(46, 20);
            this.MpChbox.TabIndex = 2;
            this.MpChbox.Text = "MP";
            this.MpChbox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft JhengHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(6, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Interval :";
            // 
            // HealTimerNum
            // 
            this.HealTimerNum.Font = new System.Drawing.Font("Microsoft JhengHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.HealTimerNum.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.HealTimerNum.Location = new System.Drawing.Point(9, 92);
            this.HealTimerNum.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.HealTimerNum.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.HealTimerNum.Name = "HealTimerNum";
            this.HealTimerNum.Size = new System.Drawing.Size(59, 23);
            this.HealTimerNum.TabIndex = 9;
            this.HealTimerNum.Value = new decimal(new int[] {
            750,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.HealTimerNum);
            this.groupBox1.Controls.Add(this.HpChbox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.MpChbox);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft JhengHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(77, 123);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Healing";
            // 
            // HealTimer
            // 
            this.HealTimer.Interval = 1000;
            this.HealTimer.Tick += new System.EventHandler(this.HealTimer_Tick);
            // 
            // timer1
            // 
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 180000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 168);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "label1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 189);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "label3";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toggleButton);
            this.Controls.Add(this.groupBox1);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Clever Maple";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.HealTimerNum)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button toggleButton;
        private System.Windows.Forms.CheckBox HpChbox;
        private System.Windows.Forms.CheckBox MpChbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown HealTimerNum;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Timer HealTimer;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
    }
}

