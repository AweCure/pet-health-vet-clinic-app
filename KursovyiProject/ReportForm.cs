using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace KursovyiProject
{
    public partial class ReportForm : Form
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=VetClinicDB;Integrated Security=True;Connect Timeout=30;";

        public ReportForm()
        {
            InitializeComponent();
        }

        private void ReportForm_Load(object sender, EventArgs e)
        {
            LoadPetsComboBox();
            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
        }

        private void LoadPetsComboBox()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT PetId, Name + ' (' + OwnerName + ')' AS DisplayText FROM Pets";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    comboBox_Pets.DataSource = dt;
                    comboBox_Pets.DisplayMember = "DisplayText";
                    comboBox_Pets.ValueMember = "PetId";
                    comboBox_Pets.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка завантаження списку: " + ex.Message);
                }
            }
        }

        private void button_LoadReport_Click(object sender, EventArgs e)
        {
            if (comboBox_Pets.SelectedValue == null)
            {
                MessageBox.Show("Оберіть пацієнта зі списку!");
                return;
            }

            int selectedPetId = Convert.ToInt32(comboBox_Pets.SelectedValue);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string petQuery = "SELECT Name AS PetName, OwnerName, Species, Breed FROM Pets WHERE PetId = @id";
                    SqlCommand cmdPet = new SqlCommand(petQuery, conn);
                    cmdPet.Parameters.AddWithValue("@id", selectedPetId);
                    SqlDataAdapter daPet = new SqlDataAdapter(cmdPet);
                    DataTable dtPetInfo = new DataTable();
                    daPet.Fill(dtPetInfo);

                    string healthQuery = "SELECT CONVERT(varchar, ExaminationDate, 104) AS ExamDate, Weight, Temperature, VetNotes FROM HealthRecords WHERE PetId = @id";
                    SqlCommand cmdHealth = new SqlCommand(healthQuery, conn);
                    cmdHealth.Parameters.AddWithValue("@id", selectedPetId);
                    SqlDataAdapter daHealth = new SqlDataAdapter(cmdHealth);
                    DataTable dtHealthInfo = new DataTable();
                    daHealth.Fill(dtHealthInfo);

                    string vacQuery = "SELECT VaccineName, DateAdministered, NextDueDate, Notes AS VacNotes FROM Vaccinations WHERE PetId = @id";
                    SqlCommand cmdVac = new SqlCommand(vacQuery, conn);
                    cmdVac.Parameters.AddWithValue("@id", selectedPetId);
                    SqlDataAdapter daVac = new SqlDataAdapter(cmdVac);
                    DataTable dtVaccineInfo = new DataTable();
                    daVac.Fill(dtVaccineInfo);

                    reportViewer1.LocalReport.ReportPath = "PetReport.rdlc";
                    reportViewer1.LocalReport.DataSources.Clear();

                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_PetInfo", dtPetInfo));
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_HealthInfo", dtHealthInfo));
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_VaccineInfo", dtVaccineInfo));

                    reportViewer1.RefreshReport();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка генерації звіту: " + ex.Message);
                }
            }
        }
    }
}