namespace Connect_The_Chips
{
    partial class AITrain_Form
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label7;
            this.TrainingSave_NUD = new System.Windows.Forms.NumericUpDown();
            this.TrainedSave_NUD = new System.Windows.Forms.NumericUpDown();
            this.AIsCount_NUD = new System.Windows.Forms.NumericUpDown();
            this.Repiats_NUD = new System.Windows.Forms.NumericUpDown();
            this.Start_B = new System.Windows.Forms.Button();
            this.Abort_B = new System.Windows.Forms.Button();
            this.Stop_B = new System.Windows.Forms.Button();
            this.Result_RTB = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.IterationsToEnd_TB = new System.Windows.Forms.TextBox();
            this.CurentIteration_TB = new System.Windows.Forms.TextBox();
            this.InfinityRepiats_CB = new System.Windows.Forms.CheckBox();
            this.RoundIterations_NUD = new System.Windows.Forms.NumericUpDown();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.TrainingSave_NUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrainedSave_NUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AIsCount_NUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Repiats_NUD)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RoundIterations_NUD)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 21);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(73, 13);
            label1.TabIndex = 1;
            label1.Text = "Training Save";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 66);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(71, 13);
            label2.TabIndex = 3;
            label2.Text = "Trained Save";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(158, 21);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(76, 13);
            label3.TabIndex = 5;
            label3.Text = "Training Count";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(158, 66);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(74, 13);
            label4.TabIndex = 7;
            label4.Text = "Repiats Count";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(13, 21);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(82, 13);
            label5.TabIndex = 12;
            label5.Text = "Curent Iteration:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(13, 47);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(82, 13);
            label6.TabIndex = 14;
            label6.Text = "Iteration to End:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(304, 21);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(85, 13);
            label7.TabIndex = 16;
            label7.Text = "Round Iterations";
            // 
            // TrainingSave_NUD
            // 
            this.TrainingSave_NUD.Location = new System.Drawing.Point(12, 37);
            this.TrainingSave_NUD.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.TrainingSave_NUD.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TrainingSave_NUD.Name = "TrainingSave_NUD";
            this.TrainingSave_NUD.Size = new System.Drawing.Size(120, 20);
            this.TrainingSave_NUD.TabIndex = 0;
            this.TrainingSave_NUD.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // TrainedSave_NUD
            // 
            this.TrainedSave_NUD.Location = new System.Drawing.Point(12, 82);
            this.TrainedSave_NUD.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.TrainedSave_NUD.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TrainedSave_NUD.Name = "TrainedSave_NUD";
            this.TrainedSave_NUD.Size = new System.Drawing.Size(120, 20);
            this.TrainedSave_NUD.TabIndex = 2;
            this.TrainedSave_NUD.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // AIsCount_NUD
            // 
            this.AIsCount_NUD.Location = new System.Drawing.Point(158, 37);
            this.AIsCount_NUD.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.AIsCount_NUD.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.AIsCount_NUD.Name = "AIsCount_NUD";
            this.AIsCount_NUD.Size = new System.Drawing.Size(120, 20);
            this.AIsCount_NUD.TabIndex = 4;
            this.AIsCount_NUD.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // Repiats_NUD
            // 
            this.Repiats_NUD.Location = new System.Drawing.Point(158, 82);
            this.Repiats_NUD.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Repiats_NUD.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Repiats_NUD.Name = "Repiats_NUD";
            this.Repiats_NUD.Size = new System.Drawing.Size(120, 20);
            this.Repiats_NUD.TabIndex = 6;
            this.Repiats_NUD.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // Start_B
            // 
            this.Start_B.Location = new System.Drawing.Point(12, 415);
            this.Start_B.Name = "Start_B";
            this.Start_B.Size = new System.Drawing.Size(75, 23);
            this.Start_B.TabIndex = 8;
            this.Start_B.Text = "Start";
            this.Start_B.UseVisualStyleBackColor = true;
            this.Start_B.Click += new System.EventHandler(this.Start_B_Click);
            // 
            // Abort_B
            // 
            this.Abort_B.Enabled = false;
            this.Abort_B.Location = new System.Drawing.Point(434, 415);
            this.Abort_B.Name = "Abort_B";
            this.Abort_B.Size = new System.Drawing.Size(75, 23);
            this.Abort_B.TabIndex = 9;
            this.Abort_B.Text = "Abort";
            this.Abort_B.UseVisualStyleBackColor = true;
            this.Abort_B.Click += new System.EventHandler(this.Abort_B_Click);
            // 
            // Stop_B
            // 
            this.Stop_B.Enabled = false;
            this.Stop_B.Location = new System.Drawing.Point(93, 415);
            this.Stop_B.Name = "Stop_B";
            this.Stop_B.Size = new System.Drawing.Size(75, 23);
            this.Stop_B.TabIndex = 10;
            this.Stop_B.Text = "Stop";
            this.Stop_B.UseVisualStyleBackColor = true;
            this.Stop_B.Click += new System.EventHandler(this.Stop_B_Click);
            // 
            // Result_RTB
            // 
            this.Result_RTB.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Result_RTB.Location = new System.Drawing.Point(0, 82);
            this.Result_RTB.Name = "Result_RTB";
            this.Result_RTB.Size = new System.Drawing.Size(262, 368);
            this.Result_RTB.TabIndex = 11;
            this.Result_RTB.Text = "";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.IterationsToEnd_TB);
            this.panel1.Controls.Add(label6);
            this.panel1.Controls.Add(this.CurentIteration_TB);
            this.panel1.Controls.Add(this.Result_RTB);
            this.panel1.Controls.Add(label5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(538, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(262, 450);
            this.panel1.TabIndex = 13;
            // 
            // IterationsToEnd_TB
            // 
            this.IterationsToEnd_TB.Location = new System.Drawing.Point(117, 44);
            this.IterationsToEnd_TB.Name = "IterationsToEnd_TB";
            this.IterationsToEnd_TB.ReadOnly = true;
            this.IterationsToEnd_TB.Size = new System.Drawing.Size(133, 20);
            this.IterationsToEnd_TB.TabIndex = 15;
            // 
            // CurentIteration_TB
            // 
            this.CurentIteration_TB.Location = new System.Drawing.Point(117, 18);
            this.CurentIteration_TB.Name = "CurentIteration_TB";
            this.CurentIteration_TB.ReadOnly = true;
            this.CurentIteration_TB.Size = new System.Drawing.Size(133, 20);
            this.CurentIteration_TB.TabIndex = 13;
            // 
            // InfinityRepiats_CB
            // 
            this.InfinityRepiats_CB.AutoSize = true;
            this.InfinityRepiats_CB.Location = new System.Drawing.Point(158, 108);
            this.InfinityRepiats_CB.Name = "InfinityRepiats_CB";
            this.InfinityRepiats_CB.Size = new System.Drawing.Size(55, 17);
            this.InfinityRepiats_CB.TabIndex = 14;
            this.InfinityRepiats_CB.Text = "infinity";
            this.InfinityRepiats_CB.UseVisualStyleBackColor = true;
            // 
            // RoundIterations_NUD
            // 
            this.RoundIterations_NUD.Location = new System.Drawing.Point(304, 37);
            this.RoundIterations_NUD.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.RoundIterations_NUD.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.RoundIterations_NUD.Name = "RoundIterations_NUD";
            this.RoundIterations_NUD.Size = new System.Drawing.Size(120, 20);
            this.RoundIterations_NUD.TabIndex = 15;
            this.RoundIterations_NUD.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // AITrain_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(label7);
            this.Controls.Add(this.RoundIterations_NUD);
            this.Controls.Add(this.InfinityRepiats_CB);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Stop_B);
            this.Controls.Add(this.Abort_B);
            this.Controls.Add(this.Start_B);
            this.Controls.Add(label4);
            this.Controls.Add(this.Repiats_NUD);
            this.Controls.Add(label3);
            this.Controls.Add(this.AIsCount_NUD);
            this.Controls.Add(label2);
            this.Controls.Add(this.TrainedSave_NUD);
            this.Controls.Add(label1);
            this.Controls.Add(this.TrainingSave_NUD);
            this.Name = "AITrain_Form";
            this.Text = "AITrain_Form";
            this.Load += new System.EventHandler(this.AITrain_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TrainingSave_NUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrainedSave_NUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AIsCount_NUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Repiats_NUD)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RoundIterations_NUD)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown TrainingSave_NUD;
        private System.Windows.Forms.NumericUpDown TrainedSave_NUD;
        private System.Windows.Forms.NumericUpDown AIsCount_NUD;
        private System.Windows.Forms.NumericUpDown Repiats_NUD;
        private System.Windows.Forms.Button Start_B;
        private System.Windows.Forms.Button Abort_B;
        private System.Windows.Forms.Button Stop_B;
        private System.Windows.Forms.RichTextBox Result_RTB;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox CurentIteration_TB;
        private System.Windows.Forms.CheckBox InfinityRepiats_CB;
        private System.Windows.Forms.TextBox IterationsToEnd_TB;
        private System.Windows.Forms.NumericUpDown RoundIterations_NUD;
    }
}