using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Collections;
using Microsoft.VisualBasic;


using System.Runtime.InteropServices;

namespace Snake_Game
{
    public partial class frmGame : Form
    {
        public string name;
        Point location = new Point(120, 120);
      

        //Food Defaults
        PictureBox food = new PictureBox();
        Point foodLocation = new Point(0, 0);

        //Database Variables
        DataSet ds = new DataSet();
        SqlDataAdapter sda;
        public int speed;
        public frmGame()
        {
            InitializeComponent();
            
        }

        private void frmGame_Load(object sender, EventArgs e)
        {
            
         
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //Change the value of timer with the value of trackbar
            //   timer.Interval = 501 - (5 * trackBar1.Value);
        }

       
        private void btnStart_Click_1(object sender, EventArgs e)
        {
            if (txtName.Text == "Name")
            {
                MessageBox.Show("Enter Your Name", "Here Is A Message For You");
                
               
            }
            else
            {
                name = txtName.Text;
                speed = trackBar1.Value;
                Form3 f3 = new Form3(name,speed);
                f3.Show();
            }
        }

    }
             

     
        
    
}

