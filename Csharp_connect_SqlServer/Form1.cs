using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient; //จำเป็น อย่าลืม เพราะเชื่อมฐานข้อมูล SQL
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Csharp_connect_SqlServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        DataSet ds = new DataSet(); //สร้าง dataset

        //string cn = "Data Source=.; Initial Catalog=Employee; User ID=sa; Password=123456"; //ที่อยู่ของฐานข้อมูล
        string cn = @"Data Source=DESKTOP-OS3KIOS\SQLEXPRESS;Initial Catalog=Employee;User ID=sa;Password=123456"; //ที่อยู่ของฐานข้อมูล
        

        private void Form1_Load(object sender, EventArgs e)
        {// เมื่อเปิดโปรแกรม
            string sql = "SELECT * FROM tblEmployee";
            SqlDataAdapter da = new SqlDataAdapter(sql, cn);
            da.Fill(ds, "Employee");
            dataGridView1.DataSource = ds.Tables["Employee"]; //นำข้อมูลใน ds มาใส่ gridview

            sql = "SELECT * FROM tblDepartment";
            da = new SqlDataAdapter(sql, cn);
            da.Fill(ds, "Department");
            comboBoxDept.DisplayMember = "deptName"; //เลือกว่าจะเอา field ไหนไปโชว์
            comboBoxDept.ValueMember = "deptID"; // เอาค่าไปเก็บแอบไว้ เพื่อบันทึกลง database
            comboBoxDept.DataSource = ds.Tables["Department"];

        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        { //เมื่อเลือกข้อมูลใน grid จะให้ไปแสดงข้อมูลใน text และ combobox

            /*if(e.RowIndex == -1) // ไม่เลือก หรือเลือกส่วนอื่นที่ไม่ใช่ข้อมูล
            {
                return;
            }
            else
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                //วิธีที่ 1 ดึงข้อมูลจาก DataTable
                DataRow dr = ds.Tables["Employee"].Rows[e.RowIndex];
                textBoxID.Text = dr["empID"].ToString();
                textBoxName.Text = dr["empName"].ToString();
                comboBoxDept.SelectedValue = dr["empDeptID"].ToString();

                //วิธีที่ 2 ดึงข้อมูลจาก DataGridView
            }*/

        }

        private void DataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1) // ไม่เลือก หรือเลือกส่วนอื่นที่ไม่ใช่ข้อมูล
            {
                return;
            }
            else
            {
                dataGridView1.Rows[e.RowIndex].Selected = true; //เลือกข้อมูลทั้งแถว

                //วิธีที่ 1 ดึงข้อมูลจาก DataTable
                DataRow dr = ds.Tables["Employee"].Rows[e.RowIndex];
                textBoxID.Text = dr["empID"].ToString();
                textBoxName.Text = dr["empName"].ToString();
                comboBoxDept.SelectedValue = dr["empDeptID"].ToString();

                //วิธีที่ 2 ดึงข้อมูลจาก DataGridView
                /*DataGridViewRow dgr = dataGridView1.Rows[e.RowIndex];
                textBoxID.Text = dgr.Cells[0].Value.ToString();
                textBoxName.Text = dgr.Cells[1].Value.ToString();
                comboBoxDept.SelectedValue = dgr.Cells[2].Value.ToString();*/
            }
        }

        private void ButtonUpdate_Click(object sender, EventArgs e)
        { //ไว้ insert/update
            DataRow[] drs = ds.Tables["Employee"].Select("empID='"+textBoxID.Text+"' ");

            if (drs.Length == 0) //ถ้าไม่มีข้อมูล ให้ insert
            {
                DataRow dr = ds.Tables["Employee"].NewRow(); //สร้างแถวใหม่

                dr["empID"] = textBoxID.Text; // ใส่ข้อมูลในแถวที่สร้าง
                dr["empName"] = textBoxName.Text;
                dr["empDeptID"] = comboBoxDept.SelectedValue;

                ds.Tables["Employee"].Rows.Add(dr); // เอาแภวที่สร้าง ไปเก็บใน table
            }
            else // ถ้ามีข้อมูลให้ update
            {
                drs[0]["empName"] = textBoxName.Text;
                drs[0]["empDeptID"] = comboBoxDept.SelectedValue;
            }

            dataGridView1.DataSource = ds.Tables["Employee"]; //อัพเดทค่าบน datagridview

        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        { //ลบข้อมูล
            DataRow[] drs = ds.Tables["Employee"].Select("empID='"+textBoxID.Text+"' ");
            if (drs.Length == 0) //หาข้อมูลไม่เจอ
            {
                MessageBox.Show("ไม่พบข้อมูล");
            }
            else //เจอข้อมูลให้ทำการลบ
            {
                drs[0].Delete(); //ลบข้อมูลในแถว *แต่แถวยังอยู่*

                //ปรับปรุงฐานข้อมูล ตรงนี้เลย !!!
                string sql = "SELECT * FROM tblEmployee";
                SqlDataAdapter da = new SqlDataAdapter(sql, cn);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                da.Update(ds, "Employee");

                ds.Tables["Employee"].AcceptChanges(); //จะทำให้แถวที่ว่างหายไป
            }
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        { //บันทึกลง database
            string sql = "SELECT * FROM tblEmployee";
            SqlDataAdapter da = new SqlDataAdapter(sql, cn);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            da.Update(ds, "Employee");
        }
    }
}
