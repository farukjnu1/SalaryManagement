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

namespace SalaryManagement
{
    public partial class Form1 : Form
    {
        //const string connString = "Data Source=FARUK-PC;User Id=sa;Password=123;DataBase=TestDB";
        const string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='E:\my doc\Nodi\ado.net\SalaryManagement\SalaryManagement\DB\SalaryMgtDb.mdf';Integrated Security=True";
        
        public Form1()
        {
            InitializeComponent();
        }

        int designationid = 0;
        public DataTable GetDesignations()
        {
            // example of disconnected architechture
            DataSet dsData = new DataSet();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT [DesignationID],[DesignationName] FROM [Designation]";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dsData);
            }
            return dsData.Tables[0];
        }

        private void ResetDesignation()
        {
            btnInsertDesig.Show();
            btnUpdateDesig.Hide();
            txtDesig.Text = "";
            dgvDesig.DataSource = GetDesignations();
            designationid = 0;
        }

        private void btnInsertDesig_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = null;
                conn.Open();
                string insertQ = "INSERT INTO [Designation]([DesignationName]) VALUES('" + txtDesig.Text.Trim() + "')";
                cmd = new SqlCommand(insertQ, conn);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("INSERT Perform Succesfully.");
                }
                else
                {
                    MessageBox.Show("Operation failed");
                }
            }

            ResetDesignation();
        }

        private void btnUpdateDesig_Click(object sender, EventArgs e)
        {
            // example of connected architechture
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string selectQ = "SELECT [DesignationID],[DesignationName] FROM [Designation] WHERE [DesignationID] = " + designationid;
                SqlCommand cmd = new SqlCommand(selectQ, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        designationid = Convert.ToInt32(reader["DesignationID"]);
                    }
                    reader.Close();

                    string updateQ = "UPDATE [Designation] SET [DesignationName] = '" + txtDesig.Text.Trim() + "' WHERE [DesignationID] = " + designationid;
                    cmd = new SqlCommand(updateQ, conn);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("UPDATE Perform Succesfully.");
                    }
                    else
                    {
                        MessageBox.Show("Operation failed.");
                    }
                }
                else
                {
                    MessageBox.Show("No data found.");
                }
            }

            ResetDesignation();
        }

        private void btnResetDesig_Click(object sender, EventArgs e)
        {
            ResetDesignation();
        }

        private void dgvDesig_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0) // edit button
            {
                btnInsertDesig.Hide();
                btnUpdateDesig.Show();
                string DesignationID = dgvDesig.Rows[e.RowIndex].Cells["DesignationID"].Value.ToString();
                string DesignationName = dgvDesig.Rows[e.RowIndex].Cells["DesignationName"].Value.ToString();
                Int32.TryParse(DesignationID, out designationid);
                txtDesig.Text = DesignationName;
            }

            if (e.ColumnIndex == 1) // delete button
            {
                string DesignationID = dgvDesig.Rows[e.RowIndex].Cells["DesignationID"].Value.ToString();
                string DesignationName = dgvDesig.Rows[e.RowIndex].Cells["DesignationName"].Value.ToString();
                Int32.TryParse(DesignationID, out designationid);

                // example of connected architechture
                if (MessageBox.Show("Are you sure to delete '" + DesignationName + "'", "Delete confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        string selectQ = "SELECT [DesignationID],[DesignationName] FROM [Designation] WHERE [DesignationID] = " + designationid;
                        SqlCommand cmd = new SqlCommand(selectQ, conn);
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                designationid = Convert.ToInt32(reader["DesignationID"]);
                            }
                            reader.Close();

                            string deleteQ = "DELETE FROM [Designation] WHERE [DesignationID] = " + designationid;
                            cmd = new SqlCommand(deleteQ, conn);
                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("DELETE Perform Succesfully.");
                            }
                            else
                            {
                                MessageBox.Show("Operation failed.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("No data found.");
                        }


                    }

                    ResetDesignation();

                }

            }
        }

        int empid = 0;
        public DataTable GetEmployees()
        {
            // example of disconnected architechture
            DataSet dsData = new DataSet();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT e.[EmployeeID],e.[EmployeeName],d.[DesignationName] FROM [Employee] e JOIN [Designation] d ON e.DesignationID = d.DesignationID";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dsData);
            }
            return dsData.Tables[0];
        }

        private void ResetEmployee()
        {
            btnInsertEmp.Show();
            btnUpdateEmp.Hide();
            txtEmp.Text = "";
            dgvEmp.DataSource = GetEmployees();
            empid = 0;
        }

        private void btnInsertEmp_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = null;
                conn.Open();
                string insertQ = "INSERT INTO [Employee]([EmployeeName],[DesignationID]) VALUES('" + txtEmp.Text.Trim() + "','" + cmbDesig.SelectedValue.ToString() + "')";
                cmd = new SqlCommand(insertQ, conn);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("INSERT Perform Succesfully.");
                }
                else
                {
                    MessageBox.Show("Operation failed");
                }
            }

            ResetEmployee();
        }

        private void btnUpdateEmp_Click(object sender, EventArgs e)
        {
            // example of connected architechture
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string selectQ = "SELECT [EmployeeID],[EmployeeName],[DesignationID] FROM [Employee] WHERE [EmployeeID] = " + empid;
                SqlCommand cmd = new SqlCommand(selectQ, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        empid = Convert.ToInt32(reader["EmployeeID"]);
                    }
                    reader.Close();

                    string updateQ = "UPDATE [Employee] SET [EmployeeName] = '" + txtEmp.Text.Trim() + "',[DesignationID] = '" + cmbDesig.SelectedValue.ToString() + "' WHERE [EmployeeID] = " + empid;
                    cmd = new SqlCommand(updateQ, conn);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("UPDATE Perform Succesfully.");
                    }
                    else
                    {
                        MessageBox.Show("Operation failed.");
                    }
                }
                else
                {
                    MessageBox.Show("No data found.");
                }
            }

            ResetEmployee();
        }

        private void btnResetEmp_Click(object sender, EventArgs e)
        {
            ResetEmployee();
            FillCmbDesignation();
        }

        private void dgvEmp_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0) // edit button
            {
                btnInsertEmp.Hide();
                btnUpdateEmp.Show();
                string EmployeeID = dgvEmp.Rows[e.RowIndex].Cells["EmployeeID"].Value.ToString();
                string EmployeeName = dgvEmp.Rows[e.RowIndex].Cells["EmployeeName"].Value.ToString();
                string DesignationID = dgvEmp.Rows[e.RowIndex].Cells["DesignationID"].Value.ToString();
                Int32.TryParse(EmployeeID, out empid);
                txtEmp.Text = EmployeeName;
                cmbDesig.SelectedValue = DesignationID;
            }

            if (e.ColumnIndex == 1) // delete button
            {
                string EmployeeID = dgvEmp.Rows[e.RowIndex].Cells["EmployeeID"].Value.ToString();
                string EmployeeName = dgvEmp.Rows[e.RowIndex].Cells["EmployeeName"].Value.ToString();
                string DesignationID = dgvEmp.Rows[e.RowIndex].Cells["DesignationID"].Value.ToString();
                Int32.TryParse(EmployeeID, out empid);

                // example of connected architechture
                if (MessageBox.Show("Are you sure to delete '" + Name + "'", "Delete confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        string selectQ = "SELECT [EmployeeID],[EmployeeName],[DesignationID] FROM [Employee] WHERE [EmployeeID] = " + empid;
                        SqlCommand cmd = new SqlCommand(selectQ, conn);
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                empid = Convert.ToInt32(reader["EmployeeID"]);
                            }
                            reader.Close();

                            string deleteQ = "DELETE FROM [Employee] WHERE [EmployeeID] = " + empid;
                            cmd = new SqlCommand(deleteQ, conn);
                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("DELETE Perform Succesfully.");
                            }
                            else
                            {
                                MessageBox.Show("Operation failed.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("No data found.");
                        }


                    }

                    ResetEmployee();

                }

            }
        }

        private void FillCmbDesignation()
        {
            var designations = GetDesignations();
            cmbDesig.DataSource = designations;
            cmbDesig.DisplayMember = "DesignationName";
            cmbDesig.ValueMember = "DesignationID";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ResetDesignation();
            ResetEmployee();
            FillCmbDesignation();
            ResetSalaryRule();
        }

        private void ResetSalaryRule()
        {
            FillCmbDesignationSalary();
        }

        private void FillCmbDesignationSalary()
        {
            var designations = GetDesignations();
            cmbDesigSalary.DataSource = designations;
            cmbDesigSalary.DisplayMember = "DesignationName";
            cmbDesigSalary.ValueMember = "DesignationID";
        }

        private void txtGrossSalary_TextChanged(object sender, EventArgs e)
        {
            string strGrossSalary = txtGrossSalary.Text.Trim();
            double nGrossSalary = 0;
            if (strGrossSalary != "")
            {
                double.TryParse(strGrossSalary, out nGrossSalary);
                txtBasicSalary.Text = Convert.ToString(nGrossSalary * 0.50);
                txtHouseRent.Text = Convert.ToString(nGrossSalary * 0.30);
                txtMedicalAllowance.Text = Convert.ToString(nGrossSalary * 0.10);
                txtConveyanceAllowance.Text = Convert.ToString(nGrossSalary * 0.10);
                txtIncomeTax.Text = Convert.ToString(nGrossSalary * 0.03);
                double nIncomeTax = 0;
                if (nGrossSalary >= 15000)
                {
                    nIncomeTax = nGrossSalary * 0.03;
                    txtIncomeTax.Text = nIncomeTax.ToString();
                }
                txtNetSalary.Text = Convert.ToString(nGrossSalary - nIncomeTax);
            }
        }

        private void btnReportEmp_Click(object sender, EventArgs e)
        {
            ReportForm frm2 = new ReportForm();
            frm2.Show();
        }
    }
}
