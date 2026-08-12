namespace KursovyiProject
{
    partial class ReportForm
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
            this.comboBox_Pets = new System.Windows.Forms.ComboBox();
            this.button_LoadReport = new System.Windows.Forms.Button();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // comboBox_Pets
            // 
            this.comboBox_Pets.BackColor = System.Drawing.Color.Honeydew;
            this.comboBox_Pets.FormattingEnabled = true;
            this.comboBox_Pets.Location = new System.Drawing.Point(13, 13);
            this.comboBox_Pets.Name = "comboBox_Pets";
            this.comboBox_Pets.Size = new System.Drawing.Size(343, 24);
            this.comboBox_Pets.TabIndex = 0;
            // 
            // button_LoadReport
            // 
            this.button_LoadReport.BackColor = System.Drawing.Color.SeaGreen;
            this.button_LoadReport.FlatAppearance.BorderColor = System.Drawing.Color.Green;
            this.button_LoadReport.FlatAppearance.BorderSize = 2;
            this.button_LoadReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_LoadReport.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_LoadReport.ForeColor = System.Drawing.Color.White;
            this.button_LoadReport.Location = new System.Drawing.Point(920, 5);
            this.button_LoadReport.Name = "button_LoadReport";
            this.button_LoadReport.Size = new System.Drawing.Size(221, 40);
            this.button_LoadReport.TabIndex = 38;
            this.button_LoadReport.Text = "Завантажити звіт";
            this.button_LoadReport.UseVisualStyleBackColor = false;
            this.button_LoadReport.Click += new System.EventHandler(this.button_LoadReport_Click);
            // 
            // reportViewer1
            // 
            this.reportViewer1.Location = new System.Drawing.Point(13, 51);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(1128, 606);
            this.reportViewer1.TabIndex = 39;
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Honeydew;
            this.ClientSize = new System.Drawing.Size(1145, 669);
            this.Controls.Add(this.reportViewer1);
            this.Controls.Add(this.button_LoadReport);
            this.Controls.Add(this.comboBox_Pets);
            this.Name = "ReportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ReportForm";
            this.Load += new System.EventHandler(this.ReportForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_Pets;
        private System.Windows.Forms.Button button_LoadReport;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
    }
}