using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Econtact
{
    public partial class Econtact : Form
    {
        private MySqlCommand cmd;
        private MySqlDataAdapter dataAdapter;
        private DataSet dataSet;
        public Econtact()
        {
            InitializeComponent();
            InitializeData();
        }
        private void InitializeData()
        {
            // Initialize the data adapter and dataset
            dataAdapter = new MySqlDataAdapter();
            dataSet = new DataSet();

            // Assuming conn is your MySqlConnection instance
            MySqlConnection conn = new MySqlConnection("server=127.0.0.1;uid=root;database=econtact");
            conn.Open();

            // Assuming cmd is your MySqlCommand instance
            cmd = new MySqlCommand("SELECT * FROM tbl_contact", conn);

            // Set the SelectCommand for the data adapter
            dataAdapter.SelectCommand = cmd;

            // Fill the dataset with data from the database
            dataAdapter.Fill(dataSet);

            // Set the DataSource for the DataGridView
            dataGridView1.DataSource = dataSet.Tables[0];
        }
        private void LoadData()
        {
            // Clear the dataset
            dataSet.Clear();

            // Fill the dataset with updated data from the database
            dataAdapter.Fill(dataSet);
        }
        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex =e.RowIndex;
            textBox1.Text = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.Rows[rowIndex].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.Rows[rowIndex].Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.Rows[rowIndex].Cells[3].Value.ToString();
            textBox5.Text = dataGridView1.Rows[rowIndex].Cells[4].Value.ToString();
            cmbGender.Text = dataGridView1.Rows[rowIndex].Cells[5].Value.ToString();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Ask the user if they really want to exit
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // If the user clicks Yes, then close the application
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
            // If the user clicks No, do nothing (you can handle it differently if needed)
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Assuming conn is your MySqlConnection instance
            using (MySqlConnection conn = new MySqlConnection("server=127.0.0.1;uid=root;database=econtact"))
            {
                try
                {
                    conn.Open();

                    // Assuming textBox1 and textBox2 are your TextBox controls for input
                    string Firstname = textBox2.Text;
                    string Lastrname = textBox3.Text;
                    string Contact = textBox4.Text;
                    string Address = textBox5.Text;
                    string Gender = cmbGender.Text;

                    // Use parameterized query to prevent SQL injection
                    string query = "INSERT INTO tbl_contact (Firstname,Lastrname,Contact,Address,Gender) VALUES (@Firstname, @Lastrname, @Contact ,@Address ,@Gender)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("@Firstname", Firstname);
                        cmd.Parameters.AddWithValue("@Lastrname", Lastrname);
                        cmd.Parameters.AddWithValue("@Contact", Contact);
                        cmd.Parameters.AddWithValue("@Address", Address);
                        cmd.Parameters.AddWithValue("@Gender", Gender);
                        // Execute the query
                        cmd.ExecuteNonQuery();
                    }
                    // Reload the data
                    LoadData();
                    MessageBox.Show("Data added to the database successfully");
                    
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            
        }

        private void del_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection("server=127.0.0.1;uid=root;database=econtact"))
            {
                try
                {
                    conn.Open();
                    // Get the ID to be deleted from textBox1
                    int idToDelete;
                    if (int.TryParse(textBox1.Text, out idToDelete))
                    {
                        // Use parameterized query to prevent SQL injection
                        string query = "DELETE FROM tbl_contact WHERE ContactID = @ContactID";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            // Add parameter
                            cmd.Parameters.AddWithValue("@ContactID", idToDelete);
                            // Execute the query
                            cmd.ExecuteNonQuery();
                        }
                        // Reload the data
                        LoadData();
                        MessageBox.Show("Data deleted from the database successfully");
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid ID for deletion");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void update_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection("server=127.0.0.1;uid=root;database=econtact"))
            {
                try
                {
                    conn.Open();

                    // Get the current row being edited or clicked
                    DataGridViewRow currentRow = dataGridView1.CurrentRow;

                    // Check if there is a current row
                    if (currentRow != null)
                    {
                        // Assuming the ID column is the first column (index 0) in your DataGridView
                        int contactIDToUpdate = Convert.ToInt32(currentRow.Cells[0].Value);

                        // Assuming textBox2, textBox3, textBox4, textBox5, cmbGender are your TextBox and ComboBox controls for input
                        string firstName = textBox2.Text;
                        string lastName = textBox3.Text;
                        string contact = textBox4.Text;
                        string address = textBox5.Text;
                        string Gender = cmbGender.Text;

                        // Use parameterized query to prevent SQL injection
                        string query = "UPDATE tbl_contact SET Firstname = @Firstname, Lastrname = @Lastrname, Contact = @Contact, Address = @Address, Gender = @Gender WHERE ContactID = @ContactID";

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Firstname", firstName);
                            cmd.Parameters.AddWithValue("@Lastrname", lastName);
                            cmd.Parameters.AddWithValue("@Contact", contact);
                            cmd.Parameters.AddWithValue("@Address", address);
                            cmd.Parameters.AddWithValue("@Gender", Gender);
                            cmd.Parameters.AddWithValue("@ContactID", contactIDToUpdate);

                            cmd.ExecuteNonQuery();
                        }

                        // Reload the data
                        LoadData();

                        MessageBox.Show("Data updated in the database successfully");
                    }
                    else
                    {
                        MessageBox.Show("No row is currently selected.");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void clear_Click(object sender, EventArgs e)
        {
            // Assuming textBox2, textBox3, textBox4, textBox5, cmbGender are your TextBox and ComboBox controls
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty; // Clear the text in textBox2
            textBox3.Text = string.Empty; // Clear the text in textBox3
            textBox4.Text = string.Empty; // Clear the text in textBox4
            textBox5.Text = string.Empty; // Clear the text in textBox5
            cmbGender.SelectedIndex = -1; // Clear the selected index in cmbGender (if applicable)
        }
    }
}
