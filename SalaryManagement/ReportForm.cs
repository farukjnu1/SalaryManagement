using SalaryManagement.reports;
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
    public partial class ReportForm : Form
    {
        const string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='E:\my doc\Nodi\ado.net\SalaryManagement\SalaryManagement\DB\SalaryMgtDb.mdf';Integrated Security=True";
        public ReportForm()
        {
            InitializeComponent();
        }

        private void ReportForm_Load(object sender, EventArgs e)
        {
            ReportWithSqlConn();
        }

        private void ReportWithSqlConn()
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
                //adap.Fill(ds, "Employee");
                EmployeeReport cr2 = new EmployeeReport();
                cr2.SetDataSource(dsData);
                crystalReportViewer1.ReportSource = cr2;
                crystalReportViewer1.Refresh();
            }
            //return dsData.Tables[0]
        
            
        }

    }
}
