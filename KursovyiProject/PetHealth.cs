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
    public partial class PetHealth : Form
    {
        private string currentUserRole;
        private int selectedPetIdForEdit = 0;

        public PetHealth(string role)
        {
            InitializeComponent();
            currentUserRole = role;
        }

        private void textBox_SearchPet_TextChanged(object sender, EventArgs e)
        {
            string searchWord = textBox_SearchPet.Text;
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=VetClinicDB;Integrated Security=True;Connect Timeout=30;";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM Pets WHERE Name LIKE @search OR OwnerName LIKE @search";

                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@search", "%" + searchWord + "%");

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            dataGridView_Pets.DataSource = dataTable;
                            FormatPetsGrid();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка пошуку: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void PetHeath_Load(object sender, EventArgs e)
        {
            if (currentUserRole != "SuperAdmin")
            {
                tabControl1.TabPages.Remove(Адмінпанель);
            }

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=VetClinicDB;Integrated Security=True;Connect Timeout=30;";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM Pets";

                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            dataGridView_Pets.DataSource = dataTable;
                            FormatPetsGrid();

                            LoadPetsToComboBox();

                            LoadPetsToVacComboBox();

                            LoadUsers();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка завантаження даних: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void pictureBox_Exit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Чи дійсно ви хочете закрити програму?", "Запит", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button_AddPet_Click(object sender, EventArgs e)
        {
            string petName = textBox_PetName.Text;
            string species = comboBox_Species.Text;
            string breed = textBox_Breed.Text;
            string ownerName = textBox_OwnerName.Text;

            maskedTextBox_OwnerPhone.TextMaskFormat = MaskFormat.IncludePromptAndLiterals;
            string ownerPhone = maskedTextBox_OwnerPhone.Text;

            if (string.IsNullOrWhiteSpace(petName) || string.IsNullOrWhiteSpace(ownerName))
            {
                MessageBox.Show("Будь ласка, введіть хоча б кличку тварини та ім'я власника!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=VetClinicDB;Integrated Security=True;Connect Timeout=30;";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();

                    string query = @"INSERT INTO Pets (Name, Species, Breed, OwnerName, OwnerPhone, RegistrationDate, Photo) 
                                     VALUES (@Name, @Species, @Breed, @OwnerName, @OwnerPhone, GETDATE(), @Photo)";

                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@Name", petName);
                        command.Parameters.AddWithValue("@Species", species);
                        command.Parameters.AddWithValue("@Breed", breed);
                        command.Parameters.AddWithValue("@OwnerName", ownerName);
                        command.Parameters.AddWithValue("@OwnerPhone", ownerPhone);

                        byte[] photoBytes = ConvertImageToBytes(pictureBox_PetPhoto.Image);
                        if (photoBytes != null)
                        {
                            command.Parameters.Add("@Photo", SqlDbType.VarBinary, -1).Value = photoBytes;
                        }
                        else
                        {
                            command.Parameters.Add("@Photo", SqlDbType.VarBinary, -1).Value = DBNull.Value;
                        }

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Нового пацієнта успішно зареєстровано!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    textBox_PetName.Clear();
                    comboBox_Species.SelectedIndex = -1;
                    textBox_Breed.Clear();
                    textBox_OwnerName.Clear();
                    maskedTextBox_OwnerPhone.Clear();
                    pictureBox_PetPhoto.Image = null;

                    PetHeath_Load(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при додаванні в базу: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FormatPetsGrid()
        {
            dataGridView_Pets.AllowUserToAddRows = false;
            dataGridView_Pets.ReadOnly = true;

            if (dataGridView_Pets.Columns["PetId"] != null)
                dataGridView_Pets.Columns["PetId"].HeaderText = "№";

            if (dataGridView_Pets.Columns["Name"] != null)
                dataGridView_Pets.Columns["Name"].HeaderText = "Кличка";

            if (dataGridView_Pets.Columns["Species"] != null)
                dataGridView_Pets.Columns["Species"].HeaderText = "Вид";

            if (dataGridView_Pets.Columns["Breed"] != null)
                dataGridView_Pets.Columns["Breed"].HeaderText = "Порода";

            if (dataGridView_Pets.Columns["OwnerName"] != null)
                dataGridView_Pets.Columns["OwnerName"].HeaderText = "Власник";

            if (dataGridView_Pets.Columns["OwnerPhone"] != null)
                dataGridView_Pets.Columns["OwnerPhone"].HeaderText = "Телефон";

            if (dataGridView_Pets.Columns["RegistrationDate"] != null)
                dataGridView_Pets.Columns["RegistrationDate"].HeaderText = "Дата реєстрації";

            if (dataGridView_Pets.Columns["PhotoPath"] != null)
                dataGridView_Pets.Columns["PhotoPath"].Visible = false;

            dataGridView_Pets.BackgroundColor = Color.White;
            dataGridView_Pets.EnableHeadersVisualStyles = false;
            dataGridView_Pets.ColumnHeadersDefaultCellStyle.BackColor = Color.SeaGreen;
            dataGridView_Pets.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView_Pets.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            if (dataGridView_Pets.Columns["Photo"] != null)
                dataGridView_Pets.Columns["Photo"].Visible = false;
        }

        private void button_EditPet_Click(object sender, EventArgs e)
        {
            if (selectedPetIdForEdit == 0)
            {
                MessageBox.Show("Спочатку оберіть пацієнта в таблиці для редагування!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string petName = textBox_PetName.Text;
            string species = comboBox_Species.Text;
            string breed = textBox_Breed.Text;
            string ownerName = textBox_OwnerName.Text;

            maskedTextBox_OwnerPhone.TextMaskFormat = MaskFormat.IncludePromptAndLiterals;
            string ownerPhone = maskedTextBox_OwnerPhone.Text;

            if (string.IsNullOrWhiteSpace(petName) || string.IsNullOrWhiteSpace(ownerName))
            {
                MessageBox.Show("Кличка та ПІБ власника не можуть бути порожніми!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=VetClinicDB;Integrated Security=True;Connect Timeout=30;";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();

                    string query = @"UPDATE Pets 
                                     SET Name = @Name, 
                                         Species = @Species, 
                                         Breed = @Breed, 
                                         OwnerName = @OwnerName, 
                                         OwnerPhone = @OwnerPhone,
                                         Photo = @Photo
                                     WHERE PetId = @PetId";

                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@Name", petName);
                        command.Parameters.AddWithValue("@Species", species);
                        command.Parameters.AddWithValue("@Breed", breed);
                        command.Parameters.AddWithValue("@OwnerName", ownerName);
                        command.Parameters.AddWithValue("@OwnerPhone", ownerPhone);
                        command.Parameters.AddWithValue("@PetId", selectedPetIdForEdit);

                        byte[] photoBytes = ConvertImageToBytes(pictureBox_PetPhoto.Image);
                        if (photoBytes != null)
                        {
                            command.Parameters.Add("@Photo", SqlDbType.VarBinary, -1).Value = photoBytes;
                        }
                        else
                        {
                            command.Parameters.Add("@Photo", SqlDbType.VarBinary, -1).Value = DBNull.Value;
                        }

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Дані пацієнта успішно оновлено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    PetHeath_Load(null, null);

                    selectedPetIdForEdit = 0;
                    textBox_PetName.Clear();
                    comboBox_Species.SelectedIndex = -1;
                    textBox_Breed.Clear();
                    textBox_OwnerName.Clear();
                    maskedTextBox_OwnerPhone.Clear();
                    pictureBox_PetPhoto.Image = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при оновленні: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView_Pets_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView_Pets.Rows[e.RowIndex];

                selectedPetIdForEdit = Convert.ToInt32(row.Cells["PetId"].Value);

                textBox_PetName.Text = row.Cells["Name"].Value.ToString();
                comboBox_Species.Text = row.Cells["Species"].Value.ToString();
                textBox_Breed.Text = row.Cells["Breed"].Value.ToString();
                textBox_OwnerName.Text = row.Cells["OwnerName"].Value.ToString();

                string phone = row.Cells["OwnerPhone"].Value.ToString();

                phone = phone.Replace("+", "").Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");

                if (phone.StartsWith("380"))
                {
                    phone = phone.Substring(3);
                }
                else if (phone.StartsWith("0") && phone.Length == 10)
                {
                    phone = phone.Substring(1);
                }

                maskedTextBox_OwnerPhone.Text = phone;

                pictureBox_PetPhoto.Image = null;

                if (row.Cells["Photo"].Value != DBNull.Value && row.Cells["Photo"].Value != null)
                {
                    byte[] photoBytes = (byte[])row.Cells["Photo"].Value;

                    using (MemoryStream ms = new MemoryStream(photoBytes))
                    {
                        using (Image tempImg = Image.FromStream(ms))
                        {
                            pictureBox_PetPhoto.Image = new Bitmap(tempImg);
                        }
                    }
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            textBox_PetName.Text = "";
            comboBox_Species.SelectedIndex = -1;
            textBox_Breed.Text = "";
            textBox_OwnerName.Text = "";
            maskedTextBox_OwnerPhone.Text = "";
            pictureBox_PetPhoto.Image = null;

            textBox_PetName.Focus();
        }

        private void ApplyDateFilter()
        {
            DateTime dateFrom = dtp_From.Value.Date;
            DateTime dateTo = dtp_To.Value.Date.AddDays(1).AddSeconds(-1);

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=VetClinicDB;Integrated Security=True;Connect Timeout=30;";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM Pets WHERE RegistrationDate BETWEEN @from AND @to";

                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@from", dateFrom);
                        command.Parameters.AddWithValue("@to", dateTo);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            dataGridView_Pets.DataSource = dataTable;
                            FormatPetsGrid();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка фільтрації: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dtp_From_ValueChanged(object sender, EventArgs e)
        {
            ApplyDateFilter();
        }

        private void dtp_To_ValueChanged(object sender, EventArgs e)
        {
            ApplyDateFilter();
        }

        private void button_ResetFilter_Click(object sender, EventArgs e)
        {
            PetHeath_Load(null, null);
        }

        private void pictureBox_Backward_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Чи дійсно ви бажаєте повернутися на форму авторизації?", "Запит",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Form1 mainForm = new Form1();
                mainForm.Show();
                this.Hide();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (dataGridView_Pets.CurrentRow == null)
            {
                MessageBox.Show("Будь ласка, оберіть пацієнта в таблиці для видалення!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedPetId = Convert.ToInt32(dataGridView_Pets.CurrentRow.Cells["PetId"].Value);
            string selectedPetName = dataGridView_Pets.CurrentRow.Cells["Name"].Value.ToString();

            DialogResult result = MessageBox.Show($"Ви дійсно хочете видалити пацієнта '{selectedPetName}'?\nЦю дію неможливо скасувати.",
                                                  "Підтвердження видалення",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=VetClinicDB;Integrated Security=True;Connect Timeout=30;";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();

                    string query = @"
                    DELETE FROM Vaccinations WHERE PetId = @id;
                    DELETE FROM HealthRecords WHERE PetId = @id;
                    DELETE FROM Pets WHERE PetId = @id;";

                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@id", selectedPetId);
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Пацієнта успішно видалено.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    PetHeath_Load(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при видаленні з бази: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button_UploadPhoto_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Зображення (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
                ofd.Title = "Оберіть фото пацієнта";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    using (Image tempImg = Image.FromFile(ofd.FileName))
                    {
                        pictureBox_PetPhoto.Image = new Bitmap(tempImg);
                    }
                }
            }
        }

        private byte[] ConvertImageToBytes(Image img)
        {
            if (img == null) return null;

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (Bitmap bmp = new Bitmap(img))
                    {
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    return ms.ToArray();
                }
            }
            catch
            {
                return null; // Запобіжник на випадок битого файлу
            }
        }

        private void LoadPetsToComboBox()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=VetClinicDB;Integrated Security=True;Connect Timeout=30;";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT PetId, CONCAT(Name, ' (', OwnerName, ')') AS DisplayName FROM Pets";

                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            comboBox_SelectPet.DataSource = dataTable;

                            comboBox_SelectPet.DisplayMember = "DisplayName";

                            comboBox_SelectPet.ValueMember = "PetId";

                            comboBox_SelectPet.SelectedIndex = -1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка завантаження списку пацієнтів: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button_AddRecord_Click(object sender, EventArgs e)
        {
            if (comboBox_SelectPet.SelectedIndex == -1)
            {
                MessageBox.Show("Будь ласка, оберіть пацієнта зі списку!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedPetId = Convert.ToInt32(comboBox_SelectPet.SelectedValue);

            string weightText = textBox_Weight.Text.Replace(".", ",");
            string tempText = textBox_Temperature.Text.Replace(".", ",");
            string notes = textBox_VetNotes.Text;

            if (string.IsNullOrWhiteSpace(weightText) || string.IsNullOrWhiteSpace(tempText) || string.IsNullOrWhiteSpace(notes))
            {
                MessageBox.Show("Будь ласка, заповніть усі поля (вага, температура та нотатки) перед додаванням запису!", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal weight = 0;
            decimal temperature = 0;
            decimal.TryParse(weightText, out weight);
            decimal.TryParse(tempText, out temperature);

            if (weight <= 0 || weight > 200)
            {
                MessageBox.Show("Введіть реальну вагу пацієнта (від 0,1 до 200 кг)!", "Некоректні дані", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (temperature < 10 || temperature > 45)
            {
                MessageBox.Show("Введіть реальну температуру тіла (від 10 до 45 °C)!", "Некоректні дані", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=VetClinicDB;Integrated Security=True;Connect Timeout=30;";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();

                    string query = @"INSERT INTO HealthRecords (PetId, ExaminationDate, Weight, Temperature, VetNotes) 
                                     VALUES (@PetId, GETDATE(), @Weight, @Temperature, @VetNotes)";

                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@PetId", selectedPetId);

                        if (weight > 0) command.Parameters.AddWithValue("@Weight", weight);
                        else command.Parameters.AddWithValue("@Weight", DBNull.Value);

                        if (temperature > 0) command.Parameters.AddWithValue("@Temperature", temperature);
                        else command.Parameters.AddWithValue("@Temperature", DBNull.Value);

                        command.Parameters.AddWithValue("@VetNotes", string.IsNullOrWhiteSpace(notes) ? (object)DBNull.Value : notes);

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Медичний запис успішно додано!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    textBox_Weight.Clear();
                    textBox_Temperature.Clear();
                    textBox_VetNotes.Clear();

                    comboBox_SelectPet_SelectedIndexChanged(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при додаванні запису: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void comboBox_SelectPet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_SelectPet.SelectedIndex == -1 || comboBox_SelectPet.SelectedValue == null)
            {
                dataGridView_HealthHistory.DataSource = null;
                chart_Weight.Series[0].Points.Clear();
                return;
            }

            int selectedPetId;
            if (!int.TryParse(comboBox_SelectPet.SelectedValue.ToString(), out selectedPetId))
            {
                return;
            }

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=VetClinicDB;Integrated Security=True;Connect Timeout=30;";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    string query = @"SELECT ExaminationDate, Weight, Temperature, VetNotes 
                                     FROM HealthRecords 
                                     WHERE PetId = @PetId 
                                     ORDER BY ExaminationDate ASC";

                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@PetId", selectedPetId);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            dataGridView_HealthHistory.DataSource = dataTable;
                            FormatHealthGrid();

                            chart_Weight.Series[0].Points.Clear();
                            chart_Weight.Series[0].XValueMember = "ExaminationDate";
                            chart_Weight.Series[0].YValueMembers = "Weight";
                            chart_Weight.DataSource = dataTable;
                            chart_Weight.Series[0].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Date;
                            chart_Weight.DataBind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка завантаження медичної історії: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FormatHealthGrid()
        {
            if (dataGridView_HealthHistory.Columns["ExaminationDate"] != null)
                dataGridView_HealthHistory.Columns["ExaminationDate"].HeaderText = "Дата огляду";

            if (dataGridView_HealthHistory.Columns["Weight"] != null)
                dataGridView_HealthHistory.Columns["Weight"].HeaderText = "Вага (кг)";

            if (dataGridView_HealthHistory.Columns["Temperature"] != null)
                dataGridView_HealthHistory.Columns["Temperature"].HeaderText = "Температура (°C)";

            if (dataGridView_HealthHistory.Columns["VetNotes"] != null)
                dataGridView_HealthHistory.Columns["VetNotes"].HeaderText = "Нотатки ветеринара";

            dataGridView_HealthHistory.BackgroundColor = Color.White;
            dataGridView_HealthHistory.EnableHeadersVisualStyles = false;
            dataGridView_HealthHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.SeaGreen;
            dataGridView_HealthHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView_HealthHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dataGridView_HealthHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_HealthHistory.AllowUserToAddRows = false;
            dataGridView_HealthHistory.ReadOnly = true;

            dataGridView_HealthHistory.RowHeadersVisible = false;

            dataGridView_HealthHistory.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridView_HealthHistory.AlternatingRowsDefaultCellStyle.BackColor = Color.Honeydew;
        }

        private void LoadPetsToVacComboBox()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=VetClinicDB;Integrated Security=True;Connect Timeout=30;";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT PetId, CONCAT(Name, ' (', OwnerName, ')') AS DisplayName FROM Pets";

                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            comboBox_VacPet.DataSource = dataTable;
                            comboBox_VacPet.DisplayMember = "DisplayName";
                            comboBox_VacPet.ValueMember = "PetId";
                            comboBox_VacPet.SelectedIndex = -1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка завантаження списку для вакцинації: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button_AddVaccine_Click(object sender, EventArgs e)
        {
            if (comboBox_VacPet.SelectedIndex == -1)
            {
                MessageBox.Show("Будь ласка, оберіть пацієнта!", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string vaccineName = comboBox_VaccineName.Text;
            string vacNotes = textBox_VacNotes.Text;

            if (string.IsNullOrWhiteSpace(vaccineName))
            {
                MessageBox.Show("Будь ласка, вкажіть або оберіть назву вакцини!", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedPetId = Convert.ToInt32(comboBox_VacPet.SelectedValue);
            DateTime dateAdministered = dtp_VacDate.Value.Date;
            DateTime nextDueDate = dtp_NextVacDate.Value.Date;

            if (nextDueDate <= dateAdministered)
            {
                MessageBox.Show("Дата наступної вакцинації повинна бути пізнішою за дату поточного щеплення!", "Помилка даних", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=VetClinicDB;Integrated Security=True;Connect Timeout=30;";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    string query = @"INSERT INTO Vaccinations (PetId, VaccineName, DateAdministered, NextDueDate, Notes) 
                                     VALUES (@PetId, @VaccineName, @DateAdministered, @NextDueDate, @Notes)";

                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@PetId", selectedPetId);
                        command.Parameters.AddWithValue("@VaccineName", vaccineName);
                        command.Parameters.AddWithValue("@DateAdministered", dateAdministered);
                        command.Parameters.AddWithValue("@NextDueDate", nextDueDate);
                        command.Parameters.AddWithValue("@Notes", string.IsNullOrWhiteSpace(vacNotes) ? (object)DBNull.Value : vacNotes);

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Запис про вакцинацію успішно додано!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    comboBox_VaccineName.SelectedIndex = -1;
                    comboBox_VaccineName.Text = "";
                    textBox_VacNotes.Clear();

                    comboBox_VacPet_SelectedIndexChanged(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при збереженні вакцинації: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void comboBox_VacPet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_VacPet.SelectedIndex == -1 || comboBox_VacPet.SelectedValue == null)
            {
                dataGridView_Vaccinations.DataSource = null;
                return;
            }

            int selectedPetId;
            if (!int.TryParse(comboBox_VacPet.SelectedValue.ToString(), out selectedPetId)) return;

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=VetClinicDB;Integrated Security=True;Connect Timeout=30;";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    string query = @"SELECT VaccineName, DateAdministered, NextDueDate, Notes 
                                     FROM Vaccinations 
                                     WHERE PetId = @PetId 
                                     ORDER BY DateAdministered DESC";

                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@PetId", selectedPetId);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            dataGridView_Vaccinations.DataSource = dataTable;
                            FormatVaccinationsGrid();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка завантаження історії щеплень: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FormatVaccinationsGrid()
        {
            if (dataGridView_Vaccinations.Columns["VaccineName"] != null)
                dataGridView_Vaccinations.Columns["VaccineName"].HeaderText = "Назва вакцини";

            if (dataGridView_Vaccinations.Columns["DateAdministered"] != null)
                dataGridView_Vaccinations.Columns["DateAdministered"].HeaderText = "Дата щеплення";

            if (dataGridView_Vaccinations.Columns["NextDueDate"] != null)
                dataGridView_Vaccinations.Columns["NextDueDate"].HeaderText = "Планова дата";

            if (dataGridView_Vaccinations.Columns["Notes"] != null)
                dataGridView_Vaccinations.Columns["Notes"].HeaderText = "Серія / Нотатки";

            dataGridView_Vaccinations.BackgroundColor = Color.White;
            dataGridView_Vaccinations.EnableHeadersVisualStyles = false;
            dataGridView_Vaccinations.ColumnHeadersDefaultCellStyle.BackColor = Color.SeaGreen; 
            dataGridView_Vaccinations.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView_Vaccinations.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dataGridView_Vaccinations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_Vaccinations.RowHeadersVisible = false;
            dataGridView_Vaccinations.AllowUserToAddRows = false;
            dataGridView_Vaccinations.ReadOnly = true;

            dataGridView_Vaccinations.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridView_Vaccinations.AlternatingRowsDefaultCellStyle.BackColor = Color.Honeydew;
        }

        private void dataGridView_HealthHistory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string date = dataGridView_HealthHistory.Rows[e.RowIndex].Cells["ExaminationDate"].Value.ToString();
                string notes = dataGridView_HealthHistory.Rows[e.RowIndex].Cells["VetNotes"].Value.ToString();

                MessageBox.Show(notes, $"Деталі огляду за {date}", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dataGridView_Vaccinations_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string date = dataGridView_Vaccinations.Rows[e.RowIndex].Cells["DateAdministered"].Value.ToString();
                string notes = dataGridView_Vaccinations.Rows[e.RowIndex].Cells["Notes"].Value.ToString();

                if (string.IsNullOrWhiteSpace(notes)) notes = "Нотаток немає";

                MessageBox.Show(notes, $"Деталі вакцинації за {date}", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LoadReminders()
        {
            string filter = comboBox_ReminderFilter.Text;
            string dateCondition = "";

            if (filter == "Протерміновані")
                dateCondition = "AND v.NextDueDate < GETDATE()";
            else if (filter == "Наступні 7 днів")
                dateCondition = "AND v.NextDueDate BETWEEN GETDATE() AND DATEADD(day, 7, GETDATE())";
            else if (filter == "Наступні 30 днів")
                dateCondition = "AND v.NextDueDate BETWEEN GETDATE() AND DATEADD(day, 30, GETDATE())";

            string query = $@"
        SELECT p.Name AS PetName, p.OwnerName, p.OwnerPhone AS Phone, v.VaccineName, v.NextDueDate
        FROM Vaccinations v 
        JOIN Pets p ON v.PetId = p.PetId 
        WHERE v.NextDueDate IS NOT NULL {dateCondition}
        ORDER BY v.NextDueDate ASC";

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=VetClinicDB;Integrated Security=True;Connect Timeout=30;";
            using (System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                try
                {
                    System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(query, sqlConnection);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    adapter.Fill(dt);
                    dataGridView_Reminders.DataSource = dt;

                    if (dataGridView_Reminders.Columns["PetName"] != null)
                    {
                        dataGridView_Reminders.Columns["PetName"].HeaderText = "Кличка";
                        dataGridView_Reminders.Columns["OwnerName"].HeaderText = "Власник";
                        dataGridView_Reminders.Columns["Phone"].HeaderText = "Телефон";
                        dataGridView_Reminders.Columns["VaccineName"].HeaderText = "Вакцина";
                        dataGridView_Reminders.Columns["NextDueDate"].HeaderText = "Планова дата";

                        dataGridView_Reminders.AllowUserToAddRows = false;
                        dataGridView_Reminders.ReadOnly = true;
                        dataGridView_Reminders.RowHeadersVisible = false;
                        dataGridView_Reminders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }

                    foreach (DataGridViewRow row in dataGridView_Reminders.Rows)
                    {
                        if (row.Cells["NextDueDate"].Value != null && row.Cells["NextDueDate"].Value.ToString() != "")
                        {
                            DateTime dueDate = Convert.ToDateTime(row.Cells["NextDueDate"].Value);
                            if (dueDate < DateTime.Now)
                                row.DefaultCellStyle.BackColor = Color.LightCoral;
                            else if (dueDate <= DateTime.Now.AddDays(7))
                                row.DefaultCellStyle.BackColor = Color.LightYellow;
                            else
                                row.DefaultCellStyle.BackColor = Color.White;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void comboBox_ReminderFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReminders();
        }

        private void button_SendReminders_Click(object sender, EventArgs e)
        {
            if (dataGridView_Reminders.Rows.Count == 0)
            {
                MessageBox.Show("Немає кому відправляти нагадування.");
                return;
            }
            MessageBox.Show($"Імітація відправки повідомлень...\nУспішно надіслано: {dataGridView_Reminders.Rows.Count} шт.");
        }

        private void LoadUsers()
        {
            string query = "SELECT UserId, Username, Role FROM Users";
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=VetClinicDB;Integrated Security=True;Connect Timeout=30;";
            using (System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                try
                {
                    System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(query, sqlConnection);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    adapter.Fill(dt);
                    dataGridView_Users.DataSource = dt;
                    dataGridView_Users.AllowUserToAddRows = false;
                    dataGridView_Users.ReadOnly = true;
                    dataGridView_Users.RowHeadersVisible = false;
                    dataGridView_Users.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
        }

        private void button_AddUser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_Login.Text) || string.IsNullOrWhiteSpace(textBox_Password.Text) || comboBox_Role.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show("Заповніть всі поля!");
                return;
            }

            string selectedRoleUA = comboBox_Role.SelectedItem.ToString();
            string dbRole = "";

            if (selectedRoleUA == "Головний лікар") dbRole = "SuperAdmin";
            else if (selectedRoleUA == "Ветеринар") dbRole = "Vet";
            else if (selectedRoleUA == "Адміністратор") dbRole = "Admin";
            else dbRole = "Vet";

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=VetClinicDB;Integrated Security=True;Connect Timeout=30;";

            using (System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();

                    if (dbRole == "SuperAdmin")
                    {
                        string checkQuery = "SELECT COUNT(*) FROM Users WHERE Role = 'SuperAdmin'";
                        using (System.Data.SqlClient.SqlCommand checkCommand = new System.Data.SqlClient.SqlCommand(checkQuery, sqlConnection))
                        {
                            int adminCount = (int)checkCommand.ExecuteScalar();
                            if (adminCount >= 1)
                            {
                                System.Windows.Forms.MessageBox.Show("В системі може бути лише один Головний лікар!", "Блокування", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }

                    string hashedPassword = PasswordHasher.Hash(textBox_Password.Text);
                    string query = "INSERT INTO Users (Username, PasswordHash, Role) VALUES (@username, @hash, @role)";

                    using (System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@username", textBox_Login.Text);
                        command.Parameters.AddWithValue("@hash", hashedPassword);
                        command.Parameters.AddWithValue("@role", dbRole);

                        command.ExecuteNonQuery();
                        System.Windows.Forms.MessageBox.Show("Користувача успішно додано!");

                        textBox_Login.Clear();
                        textBox_Password.Clear();
                        comboBox_Role.SelectedIndex = -1;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
            LoadUsers();
        }

        private void button_DeleteUser_Click(object sender, EventArgs e)
        {
            if (dataGridView_Users.CurrentRow == null)
            {
                System.Windows.Forms.MessageBox.Show("Оберіть користувача для видалення!");
                return;
            }

            string roleToDelete = dataGridView_Users.CurrentRow.Cells["Role"].Value.ToString();
            if (roleToDelete == "SuperAdmin")
            {
                System.Windows.Forms.MessageBox.Show("Критична помилка: неможливо видалити обліковий запис Головного лікаря! Система повинна мати хоча б одного адміністратора.", "Захист системи", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int userId = Convert.ToInt32(dataGridView_Users.CurrentRow.Cells["UserId"].Value);
            string query = "DELETE FROM Users WHERE UserId = @id";
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=VetClinicDB;Integrated Security=True;Connect Timeout=30;";

            using (System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                using (System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(query, sqlConnection))
                {
                    command.Parameters.AddWithValue("@id", userId);
                    try
                    {
                        sqlConnection.Open();
                        command.ExecuteNonQuery();
                        System.Windows.Forms.MessageBox.Show("Користувача видалено!");
                        LoadUsers();
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Ви дійсно хочете перейти на форму створення медичної картки тварини?", "Запит", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ReportForm reportForm = new ReportForm();
                reportForm.ShowDialog();
            }
        }
    }
}