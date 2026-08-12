namespace KursovyiProject
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_Password = new System.Windows.Forms.TextBox();
            this.linkLabel_Login = new System.Windows.Forms.LinkLabel();
            this.textBox_Login = new System.Windows.Forms.TextBox();
            this.pictureBox_showPass = new System.Windows.Forms.PictureBox();
            this.pictureBox_dontShowPass = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_showPass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_dontShowPass)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.PaleGreen;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(596, 52);
            this.panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(540, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(53, 46);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label1.Location = new System.Drawing.Point(105, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Логін";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label2.Location = new System.Drawing.Point(105, 214);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Пароль";
            // 
            // textBox_Password
            // 
            this.textBox_Password.Location = new System.Drawing.Point(202, 214);
            this.textBox_Password.Name = "textBox_Password";
            this.textBox_Password.Size = new System.Drawing.Size(197, 22);
            this.textBox_Password.TabIndex = 4;
            // 
            // linkLabel_Login
            // 
            this.linkLabel_Login.AutoSize = true;
            this.linkLabel_Login.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.linkLabel_Login.Location = new System.Drawing.Point(198, 276);
            this.linkLabel_Login.Name = "linkLabel_Login";
            this.linkLabel_Login.Size = new System.Drawing.Size(70, 20);
            this.linkLabel_Login.TabIndex = 5;
            this.linkLabel_Login.TabStop = true;
            this.linkLabel_Login.Text = "Увійти";
            this.linkLabel_Login.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_Login_LinkClicked);
            // 
            // textBox_Login
            // 
            this.textBox_Login.Location = new System.Drawing.Point(202, 136);
            this.textBox_Login.Name = "textBox_Login";
            this.textBox_Login.Size = new System.Drawing.Size(197, 22);
            this.textBox_Login.TabIndex = 6;
            // 
            // pictureBox_showPass
            // 
            this.pictureBox_showPass.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_showPass.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_showPass.Image")));
            this.pictureBox_showPass.Location = new System.Drawing.Point(405, 214);
            this.pictureBox_showPass.Name = "pictureBox_showPass";
            this.pictureBox_showPass.Size = new System.Drawing.Size(36, 22);
            this.pictureBox_showPass.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_showPass.TabIndex = 7;
            this.pictureBox_showPass.TabStop = false;
            this.pictureBox_showPass.Click += new System.EventHandler(this.pictureBox_showPass_Click);
            // 
            // pictureBox_dontShowPass
            // 
            this.pictureBox_dontShowPass.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_dontShowPass.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_dontShowPass.Image")));
            this.pictureBox_dontShowPass.Location = new System.Drawing.Point(405, 214);
            this.pictureBox_dontShowPass.Name = "pictureBox_dontShowPass";
            this.pictureBox_dontShowPass.Size = new System.Drawing.Size(36, 22);
            this.pictureBox_dontShowPass.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_dontShowPass.TabIndex = 8;
            this.pictureBox_dontShowPass.TabStop = false;
            this.pictureBox_dontShowPass.Click += new System.EventHandler(this.pictureBox_dontShowPass_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 366);
            this.Controls.Add(this.pictureBox_dontShowPass);
            this.Controls.Add(this.pictureBox_showPass);
            this.Controls.Add(this.textBox_Login);
            this.Controls.Add(this.linkLabel_Login);
            this.Controls.Add(this.textBox_Password);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_showPass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_dontShowPass)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_Password;
        private System.Windows.Forms.LinkLabel linkLabel_Login;
        private System.Windows.Forms.TextBox textBox_Login;
        private System.Windows.Forms.PictureBox pictureBox_showPass;
        private System.Windows.Forms.PictureBox pictureBox_dontShowPass;
    }
}

