using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace KursovyiProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void AttachDatabaseIfNeeded()
        {
            string mdfPath = System.IO.Path.Combine(Application.StartupPath, "VetClinicDB.mdf");
            string ldfPath = System.IO.Path.Combine(Application.StartupPath, "VetClinicDB_log.ldf");

            string masterConnection = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;";

            using (SqlConnection conn = new SqlConnection(masterConnection))
            {
                conn.Open();
                string checkQuery = "SELECT COUNT(*) FROM sys.databases WHERE name = 'VetClinicDB'";
                using (SqlCommand cmd = new SqlCommand(checkQuery, conn))
                {
                    int count = (int)cmd.ExecuteScalar();
                    if (count == 0)
                    {
                        string attachQuery = $"CREATE DATABASE VetClinicDB ON (FILENAME='{mdfPath}'), (FILENAME='{ldfPath}') FOR ATTACH;";
                        using (SqlCommand attachCmd = new SqlCommand(attachQuery, conn))
                        {
                            attachCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Чи дійсно ви хочете закрити та вийти з вікна входу в програму?", "Запит", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void linkLabel_Login_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string loginUser = textBox_Login.Text.Trim();
            string passUser = textBox_Password.Text.Trim();

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=VetClinicDB;Integrated Security=True;Connect Timeout=30;";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();

                    string query = "SELECT PasswordHash, Role FROM Users WHERE Username = @login";

                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@login", loginUser);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string dbPassword = reader["PasswordHash"].ToString().Trim();
                                string userRole = reader["Role"].ToString();

                                if (PasswordHasher.Verify(passUser, dbPassword))
                                {
                                    MessageBox.Show("Авторизація успішна!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    PetHealth mainForm = new PetHealth(userRole);
                                    mainForm.FormClosed += (s, args) => this.Show();
                                    mainForm.Show();
                                    this.Hide();
                                }
                                else
                                {
                                    MessageBox.Show("Невірний пароль!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Користувача з таким логіном не знайдено!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка підключення до бази даних: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AttachDatabaseIfNeeded();
            textBox_Password.MaxLength = 80;
            textBox_Password.UseSystemPasswordChar = true;
            pictureBox_dontShowPass.Visible = false;
        }

        private void pictureBox_showPass_Click(object sender, EventArgs e)
        {
            textBox_Password.UseSystemPasswordChar = false;
            pictureBox_showPass.Visible = false;
            pictureBox_dontShowPass.Visible = true;
        }

        private void pictureBox_dontShowPass_Click(object sender, EventArgs e)
        {
            textBox_Password.UseSystemPasswordChar = true;
            pictureBox_dontShowPass.Visible = false;
            pictureBox_showPass.Visible = true;
        }
    }
}