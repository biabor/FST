namespace FingerSearchTreeForm
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.delete_ = new System.Windows.Forms.Button();
            this.add_ = new System.Windows.Forms.Button();
            this.search_ = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.test_ = new System.Windows.Forms.CheckBox();
            this.only_ = new System.Windows.Forms.CheckBox();
            this.random_ = new System.Windows.Forms.Button();
            this.remove_ = new System.Windows.Forms.Button();
            this.insert_ = new System.Windows.Forms.Button();
            this.inputOutput_ = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.delete_);
            this.groupBox1.Controls.Add(this.add_);
            this.groupBox1.Controls.Add(this.search_);
            this.groupBox1.Location = new System.Drawing.Point(10, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(408, 55);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Single Operations";
            // 
            // delete_
            // 
            this.delete_.Location = new System.Drawing.Point(288, 19);
            this.delete_.Name = "delete_";
            this.delete_.Size = new System.Drawing.Size(75, 23);
            this.delete_.TabIndex = 2;
            this.delete_.Text = "Delete";
            this.delete_.UseVisualStyleBackColor = true;
            this.delete_.Click += new System.EventHandler(this.delete__Click);
            // 
            // add_
            // 
            this.add_.Location = new System.Drawing.Point(144, 19);
            this.add_.Name = "add_";
            this.add_.Size = new System.Drawing.Size(75, 23);
            this.add_.TabIndex = 1;
            this.add_.Text = "Insert";
            this.add_.UseVisualStyleBackColor = true;
            this.add_.Click += new System.EventHandler(this.add__Click);
            // 
            // search_
            // 
            this.search_.Location = new System.Drawing.Point(8, 19);
            this.search_.Name = "search_";
            this.search_.Size = new System.Drawing.Size(75, 23);
            this.search_.TabIndex = 0;
            this.search_.Text = "Search";
            this.search_.UseVisualStyleBackColor = true;
            this.search_.Click += new System.EventHandler(this.search__Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.test_);
            this.groupBox2.Controls.Add(this.only_);
            this.groupBox2.Controls.Add(this.random_);
            this.groupBox2.Controls.Add(this.remove_);
            this.groupBox2.Controls.Add(this.insert_);
            this.groupBox2.Location = new System.Drawing.Point(10, 105);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(408, 72);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Bulk Testing Operations";
            // 
            // test_
            // 
            this.test_.AutoSize = true;
            this.test_.Location = new System.Drawing.Point(8, 48);
            this.test_.Name = "test_";
            this.test_.Size = new System.Drawing.Size(79, 19);
            this.test_.TabIndex = 3;
            this.test_.Text = "Test Order";
            this.test_.UseVisualStyleBackColor = true;
            // 
            // only_
            // 
            this.only_.AutoSize = true;
            this.only_.Location = new System.Drawing.Point(144, 47);
            this.only_.Name = "only_";
            this.only_.Size = new System.Drawing.Size(132, 19);
            this.only_.TabIndex = 4;
            this.only_.Text = "Only When Finished";
            this.only_.UseVisualStyleBackColor = true;
            // 
            // random_
            // 
            this.random_.Location = new System.Drawing.Point(287, 19);
            this.random_.Name = "random_";
            this.random_.Size = new System.Drawing.Size(75, 23);
            this.random_.TabIndex = 2;
            this.random_.Text = "Random";
            this.random_.UseVisualStyleBackColor = true;
            this.random_.Click += new System.EventHandler(this.random__Click);
            // 
            // remove_
            // 
            this.remove_.Location = new System.Drawing.Point(119, 19);
            this.remove_.Name = "remove_";
            this.remove_.Size = new System.Drawing.Size(132, 23);
            this.remove_.TabIndex = 1;
            this.remove_.Text = "Insert and Remove";
            this.remove_.UseVisualStyleBackColor = true;
            this.remove_.Click += new System.EventHandler(this.remove__Click);
            // 
            // insert_
            // 
            this.insert_.Location = new System.Drawing.Point(8, 19);
            this.insert_.Name = "insert_";
            this.insert_.Size = new System.Drawing.Size(75, 23);
            this.insert_.TabIndex = 0;
            this.insert_.Text = "Insert";
            this.insert_.UseVisualStyleBackColor = true;
            this.insert_.Click += new System.EventHandler(this.insert__Click);
            // 
            // inputOutput_
            // 
            this.inputOutput_.Location = new System.Drawing.Point(10, 76);
            this.inputOutput_.Name = "inputOutput_";
            this.inputOutput_.Size = new System.Drawing.Size(408, 23);
            this.inputOutput_.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 183);
            this.Controls.Add(this.inputOutput_);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "User Interface";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GroupBox groupBox1;
        private Button delete_;
        private Button add_;
        private Button search_;
        private GroupBox groupBox2;
        private Button random_;
        private Button remove_;
        private Button insert_;
        private TextBox inputOutput_;
        private CheckBox test_;
        private CheckBox only_;
    }
}