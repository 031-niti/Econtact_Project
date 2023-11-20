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
            //สร้าง DataAdapter และ DataSet โดยการสร้าง MySqlDataAdapter และ DataSet เพื่อใช้เก็บข้อมูลที่ดึงมาจากฐานข้อมูล MySQL
            dataAdapter = new MySqlDataAdapter();
            dataSet = new DataSet();
            //ชื่อมต่อกับฐานข้อมูล ทำการสร้างสร้าง MySqlConnection เพื่อเชื่อมต่อกับฐานข้อมูล MySQL ที่มีข้อมูลที่ต้องการดึง
            MySqlConnection conn = new MySqlConnection("server=127.0.0.1;uid=root;database=econtact");
            conn.Open();
            //MySqlCommand เพื่อสร้างคำสั่ง SQL ที่ต้องการดึงข้อมูล (SELECT * FROM tbl_contact) และกำหนด Connection ให้กับคำสั่ง
            cmd = new MySqlCommand("SELECT * FROM tbl_contact", conn);
            //ตั้งค่า SelectCommand สำหรับ dataAdapter
            dataAdapter.SelectCommand = cmd;
            //ใช้ Fill เพื่อดึงข้อมูลจากฐานข้อมูลและเก็บลงใน DataSet
            dataAdapter.Fill(dataSet);
            //กำหนด DataSource ของ DataGridView เพื่อให้แสดงข้อมูลที่ถูกดึงมาจากฐานข้อมูล
            dataGridView1.DataSource = dataSet.Tables[0];
        }
        private void LoadData()
        {
            //ลบข้อมูลทั้งหมดที่อยู่ใน dataSet. เมื่อเรียกใช้ Clear() ของ DataSet จะทำให้ข้อมูลทั้งหมดใน DataSet ถูกลบทิ้ง
            dataSet.Clear();
            //ใช้ Fill ของ dataAdapter เพื่อดึงข้อมูลใหม่จากฐานข้อมูลและเติมข้อมูลลงใน DataSet
            //Fill จะทำการดึงข้อมูลจาก dataAdapter.SelectCommand(ที่ได้รับค่าที่กำหนดไว้ใน InitializeData)
            dataAdapter.Fill(dataSet);
        }
        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex =e.RowIndex;
            //กำหนดค่าใน TextBoxes และ ComboBox
            textBox1.Text = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.Rows[rowIndex].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.Rows[rowIndex].Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.Rows[rowIndex].Cells[3].Value.ToString();
            textBox5.Text = dataGridView1.Rows[rowIndex].Cells[4].Value.ToString();
            cmbGender.Text = dataGridView1.Rows[rowIndex].Cells[5].Value.ToString();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //กำหนดให้ MessageBox แสดงข้อความ และตัวเลือก "Yes" และ "No"
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //ตรวจสอบว่าผู้ใช้คลิกที่ปุ่ม "Yes" หรือไม่
            //ถ้าผู้ใช้คลิกที่ "Yes", ฟังก์ชันจะทำการปิดแอปพลิเคชันโดยใช้ Application.Exit();
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
            //ถ้าผู้ใช้คลิกที่ "No", ฟังก์ชันจะจบการทำงานโดยไม่ทำอะไรเพิ่มเติม
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //เชื่อมต่อกับฐานข้อมูล MySQL โดยใช้ MySqlConnection เพื่อเชื่อมต่อกับฐานข้อมูล MySQL
            using (MySqlConnection conn = new MySqlConnection("server=127.0.0.1;uid=root;database=econtact"))
            {
                try
                {
                    conn.Open();
                    //ทำการอ่านข้อมูลที่ผู้ใช้ป้อนใน TextBoxes และ ComboBox เพื่อนำมาใช้ในการเพิ่มข้อมูล
                    string Firstname = textBox2.Text;
                    string Lastrname = textBox3.Text;
                    string Contact = textBox4.Text;
                    string Address = textBox5.Text;
                    string Gender = cmbGender.Text;
                    //สร้าง Parameterized Query เพื่อป้องกันการโจมตี SQL Injection
                    string query = "INSERT INTO tbl_contact (Firstname,Lastrname,Contact,Address,Gender) VALUES (@Firstname, @Lastrname, @Contact ,@Address ,@Gender)";
                    //ใช้ฟังก์ชัน MySqlCommand และกำหนดคำสั่ง SQL ที่ใช้ในการเพิ่มข้อมูล
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("@Firstname", Firstname);
                        cmd.Parameters.AddWithValue("@Lastrname", Lastrname);
                        cmd.Parameters.AddWithValue("@Contact", Contact);
                        cmd.Parameters.AddWithValue("@Address", Address);
                        cmd.Parameters.AddWithValue("@Gender", Gender);
                        //ฟังก์ชันนี้จะทำการ execute query ที่สร้างไว้ ซึ่งในที่นี้คือการเพิ่มข้อมูลลงในฐานข้อมูล
                        cmd.ExecuteNonQuery();
                    }
                    //ใช้ LoadData เพื่อโหลดข้อมูลใหม่จากฐานข้อมูล
                    LoadData();
                    MessageBox.Show("Data added to the database successfully");
                }
                //ถ้ามี error จะแสดง MessageBox
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void del_Click(object sender, EventArgs e) 
        
        {
            //เชื่อมต่อกับฐานข้อมูล MySQL โดยใช้ MySqlConnection เพื่อเชื่อมต่อกับฐานข้อมูล MySQL
            using (MySqlConnection conn = new MySqlConnection("server=127.0.0.1;uid=root;database=econtact"))
            {
                try
                {
                    conn.Open();
                    //จะทำการอ่านค่าที่รับใน textBox1 และแปลงเป็นตัวเลข ถ้าการแปลงสำเร็จค่าจะถูกเก็บไว้ใน idToDelete
                    int idToDelete;
                    if (int.TryParse(textBox1.Text, out idToDelete))
                    {
                        //คำสั่ง SQL สำหรับลบข้อมูลจากตาราง tbl_contact โดยใช้ Parameterized Query เพื่อป้องกัน SQL Injection
                        string query = "DELETE FROM tbl_contact WHERE ContactID = @ContactID";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            //โดยจะนำ ContactID ที่ต้องการลบมาเป็นพารามิเตอร์
                            cmd.Parameters.AddWithValue("@ContactID", idToDelete);
                            cmd.ExecuteNonQuery();
                        }
                        //ใช้ LoadData เพื่อโหลดข้อมูลใหม่จากฐานข้อมูล
                        LoadData();
                        MessageBox.Show("Data deleted from the database successfully");
                    }
                    else //ถ้าข้อมูลถูกลบสำเร็จ จะแสดงข้อความ 
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
            //เชื่อมต่อกับฐานข้อมูล MySQL โดยใช้ MySqlConnection เพื่อเชื่อมต่อกับฐานข้อมูล MySQL
            using (MySqlConnection conn = new MySqlConnection("server=127.0.0.1;uid=root;database=econtact"))
            {
                try
                {
                    conn.Open();
                    //จะอ่านข้อมูลจากแถวที่ถูกคลิกใน DataGridView
                    DataGridViewRow currentRow = dataGridView1.CurrentRow;
                    if (currentRow != null)
                    {
                        //เอา ContactID ที่อยู่ในคอลัมน์ที่ 0 ของแถวนั้นมาเก็บไว้ใน contactIDToUpdate
                        int contactIDToUpdate = Convert.ToInt32(currentRow.Cells[0].Value);
                        //ทำการอ่านข้อมูลที่ผู้ใช้ป้อนใน TextBoxes และ ComboBox เพื่อนำมาใช้ในการอัปเดตข้อมูล
                        string firstName = textBox2.Text;
                        string lastName = textBox3.Text;
                        string contact = textBox4.Text;
                        string address = textBox5.Text;
                        string Gender = cmbGender.Text;
                        //สร้าง Parameterized Query เพื่อป้องกันการโจมตี SQL Injection
                        string query = "UPDATE tbl_contact SET Firstname = @Firstname, Lastrname = @Lastrname, Contact = @Contact, Address = @Address, Gender = @Gender WHERE ContactID = @ContactID";
                        //สร้าง MySqlCommand ที่มีคำสั่ง SQL สำหรับอัปเดตข้อมูลในตาราง tbl_contact
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            //กำหนดข้อมูลที่ต้องการอัปเดตมาเป็นพารามิเตอร์
                            cmd.Parameters.AddWithValue("@Firstname", firstName);
                            cmd.Parameters.AddWithValue("@Lastrname", lastName);
                            cmd.Parameters.AddWithValue("@Contact", contact);
                            cmd.Parameters.AddWithValue("@Address", address);
                            cmd.Parameters.AddWithValue("@Gender", Gender);
                            cmd.Parameters.AddWithValue("@ContactID", contactIDToUpdate);
                            cmd.ExecuteNonQuery();
                        }
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
            //ฟังก์ชันนี้จะกำหนดค่าของ TextBoxes ให้เป็นค่าว่าง string.Empty
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty; 
            textBox3.Text = string.Empty; 
            textBox4.Text = string.Empty; 
            textBox5.Text = string.Empty; 
            //ถ้า cmbGender ถูกใช้ ฟังก์ชันนี้จะกำหนด SelectedIndex ใน -1 เพื่อล้างการเลือกที่อาจจะถูกทำไว้
            cmbGender.SelectedIndex = -1; 
        }
    }
}
