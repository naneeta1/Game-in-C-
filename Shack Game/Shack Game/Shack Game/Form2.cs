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
using System.Data.SqlClient;
using System.Configuration;

namespace Snake_Game
{
    public partial class Form2 : Form
    {
        DataSet ds;
        public Form2(DataSet ds)
        {
            InitializeComponent();
                this.ds = ds;
         }
        SqlDataAdapter sda;

    private void Form2_Load(object sender, EventArgs e)
    {

            string conString = ConfigurationManager.ConnectionStrings["cAString"].ConnectionString;
            string query = "SELECT * from Score ORDER BY Scores";
            using (SqlConnection con = new SqlConnection(conString))
            {
                sda = new SqlDataAdapter(query, con);

                sda.Fill(ds, "Score");
            }
            dataGridView1.DataSource = ds.Tables["Score"];
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridView1.Sort(this.dataGridView1.Columns[0], ListSortDirection.Descending);

        }


    }
        
    }

