using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake_Game
{
    public partial class Form3 : Form
    {
        PictureBox[] snakeParts;
        int snakeSize = 5;
        Point location = new Point(120, 120);
        string direction = "Right";
        bool chngingDirection = false;

        //Food Defaults
        PictureBox food = new PictureBox();
        Point foodLocation = new Point(0, 0);

        //Database Variables
        DataSet ds = new DataSet();
        SqlDataAdapter sda;

        public string Name;
        public int speed;
        
        public Form3( String n,int val)
        {
            InitializeComponent();
            Name = n;
            speed = val;
        }
        
        private void Form3_Load(object sender, EventArgs e)
        {
            System.Media.SoundPlayer sp = new System.Media.SoundPlayer("peritune-spook4.wav");

            //sp.PlayLooping();

            sp.Play();
            this.label3.Text = Name;
            timer.Interval = 501 - (5 * speed);
            { 
                pnlGame.Controls.Clear();
                snakeParts = null;
                lblScoreCount.Text = "0";
                snakeSize = 5;
                direction = "Right";
                location = new Point(120, 120);

                //Start Game
                drawSnake();
                drawFood();

                timer.Start();

                btnStop.Enabled = true;

            }
        }
        
        private void btnStop_Click(object sender, EventArgs e)
        {
            stopGame();
        }

        private void drawSnake()
        {
            snakeParts = new PictureBox[snakeSize];

            for (int i = 0; i < snakeSize; i++)
            {
                snakeParts[i] = new PictureBox();
                snakeParts[i].Size = new Size(15, 15);
                snakeParts[i].BackColor = Color.Red;
                snakeParts[i].BorderStyle = BorderStyle.FixedSingle;
                snakeParts[i].Location = new Point(location.X - (15 * i), location.Y);
                pnlGame.Controls.Add(snakeParts[i]);


            }

        }

        private void drawFood()
        {
            Random rnd = new Random();
            int xRand = rnd.Next(30) * 15;
            int yRand = rnd.Next(30) * 15;

            bool isOnSnake = true;
            //Check if food is on snake body

            while (isOnSnake)
            {
                for (int i = 0; i < snakeSize; i++)
                {
                    if (snakeParts[i].Location == new Point(xRand, yRand))
                    {
                        xRand = rnd.Next(30) * 15;
                        yRand = rnd.Next(30) * 15;
                        i = 0;
                    }
                    else
                    {
                        isOnSnake = false;
                    }
                }
            }

            //Draw Food
            if (isOnSnake == false)
            {
                foodLocation = new Point(xRand, yRand);
                food.Size = new Size(15, 15);
                food.BackColor = Color.Yellow;
                food.Location = foodLocation;
                pnlGame.Controls.Add(food);

            }

        }

        /*private void btnStart_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "Name")
            {
                MessageBox.Show("Enter Your Name", "Here Is A Message For You");
            }
            else
            {
                pnlGame.Controls.Clear();
                snakeParts = null;
                lblScoreCount.Text = "0";
                snakeSize = 5;
                direction = "Right";
                location = new Point(120, 120);

                //Start Game
                drawSnake();
                drawFood();

                timer.Start();

                //Disable Some Controls
                trackBar1.Enabled = false;
                btnStart.Enabled = false;
                txtName.Enabled = false;
                dataGridView1.Enabled = false;


                btnStop.Enabled = true;
            }
        }*/

        private void eatFood()
        {
            snakeSize++;

            //save old snake and use it to create new bigger snake with incremented snake part
            PictureBox[] oldSnake = snakeParts;
            pnlGame.Controls.Clear();
            snakeParts = new PictureBox[snakeSize];

            for (int i = 0; i < snakeSize; i++)
            {
                snakeParts[i] = new PictureBox();
                snakeParts[i].Size = new Size(15, 15);
                snakeParts[i].BackColor = Color.Red;
                snakeParts[i].BorderStyle = BorderStyle.FixedSingle;

                if (i == 0)
                {

                    System.Media.SoundPlayer player = new System.Media.SoundPlayer();
                    player.SoundLocation = "food.wav";
                    player.Play();
                    snakeParts[i].Location = foodLocation;
                }
                else
                {
                    snakeParts[i].Location = oldSnake[i - 1].Location;
                }
                pnlGame.Controls.Add(snakeParts[i]);
            }

            //UPdate Score
            int score = Int32.Parse(lblScoreCount.Text);
            int newScore = score + 10;
            lblScoreCount.Text = newScore + "";


        }

        private void stopGame()
        {
            timer.Stop();
            btnStop.Enabled = false;


            //Game Over Label
            Label over = new Label();
            over.Text = "Game\nOver";
            over.ForeColor = Color.LightBlue;
            over.Font = new Font("Arial", 100, FontStyle.Bold);
            over.TextAlign = ContentAlignment.MiddleCenter;
            over.Size = over.PreferredSize;



            int x = pnlGame.Width / 2 - over.Width / 2;
            int y = pnlGame.Height / 2 - over.Height / 2;
            over.Location = new Point(x, y);

            pnlGame.Controls.Add(over);
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = "gameover.wav";
            player.Play();
            button1.Enabled = true;


            over.BringToFront();


            //Add Current Score And Update Current Score Board
            //AddCurrentScoresToDatabase();
            updateScoreBoard();

        }

        private void move()
        {
            Point point = new Point(0, 0);

            //Loop for moving each part of snake in given direction
            for (int i = 0; i < snakeSize; i++)
            {
                if (i == 0)
                {
                    point = snakeParts[i].Location;
                    if (direction == "Left")
                        snakeParts[i].Location = new Point(snakeParts[i].Location.X - 15, snakeParts[i].Location.Y);
                    if (direction == "Right")
                        snakeParts[i].Location = new Point(snakeParts[i].Location.X + 15, snakeParts[i].Location.Y);
                    if (direction == "Top")
                        snakeParts[i].Location = new Point(snakeParts[i].Location.X, snakeParts[i].Location.Y - 15);
                    if (direction == "Down")
                        snakeParts[i].Location = new Point(snakeParts[i].Location.X, snakeParts[i].Location.Y + 15);
                }
                else
                {
                    Point newPoint = snakeParts[i].Location;
                    snakeParts[i].Location = point;
                    point = newPoint;
                }


            }

            //When Snake Hits Food
            if (snakeParts[0].Location == foodLocation)
            {
                eatFood();
                drawFood();
            }

            //When Snake Hits Any Wall
            if (snakeParts[0].Location.X < 0 || snakeParts[0].Location.X >= 592 || snakeParts[0].Location.Y < 0 || snakeParts[0].Location.Y >= 450)
            {
                stopGame();
            }

            //When Sanke Hits Itself
            for (int i = 3; i < snakeSize; i++)
            {
                if (snakeParts[0].Location == snakeParts[i].Location)
                    stopGame();

            }

            chngingDirection = false;
        }



        private void timer_Tick(object sender, EventArgs e)
        {
            move();
        }

        //Now Handle User Input TO control Snake
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Up && direction != "Down" && chngingDirection != true)
            {
                direction = "Top";
                chngingDirection = true;
            }

            if (keyData == Keys.Down && direction != "Up" && chngingDirection != true)
            {
                direction = "Down";
                chngingDirection = true;
            }

            if (keyData == Keys.Left && direction != "Right" && chngingDirection != true)
            {
                direction = "Left";
                chngingDirection = true;
            }

            if (keyData == Keys.Right && direction != "Left" && chngingDirection != true)
            {
                direction = "Right";
                chngingDirection = true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void AddCurrentScoresToDatabase()
        {
            //Insert Score Label Value With Name And DateTime in Database
            string conString = System.Configuration.ConfigurationManager.ConnectionStrings["cAString"].ConnectionString;
            string query = "Select * from Score";
            using (SqlConnection con = new SqlConnection(conString))
            {
                sda = new SqlDataAdapter(query, con);
                sda.Fill(ds, "Score");
                DataRow dr = ds.Tables["Score"].NewRow();
                dr[0] = DateTime.Now;
                dr[1] = Name;
                dr[2] = lblScoreCount.Text;

                ds.Tables[0].Rows.Add(dr);

                SqlCommandBuilder scb = new SqlCommandBuilder(sda);
                sda.Update(ds, "Score");
            }

        }

        private void updateScoreBoard()
        {
            //Get Data From Database And Show In DataGridView
             string conString = ConfigurationManager.ConnectionStrings["cAString"].ConnectionString;
             string query = "Select * from Score";
             using (SqlConnection con = new SqlConnection(conString))
             {
                 sda = new SqlDataAdapter(query, con);

                 sda.Fill(ds, "Score");
           /* Form2 frm2 = new Form2();
            frm2.dataGridView1.DataSource = ds.Tables["Score"];
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridView1.Sort(this.dataGridView1.Columns[0], ListSortDirection.Descending);
           */
            



        }
             }

        private void pnlGame_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnStart_Click_1(object sender, EventArgs e)
        { 
        
        }
      /* {
            if (txtName.Text == "Name")
            {
                MessageBox.Show("Enter Your Name", "Here Is A Message For You");
            }
            else
            {
                pnlGame.Controls.Clear();
                snakeParts = null;
                lblScoreCount.Text = "0";
                snakeSize = 5;
                direction = "Right";
                location = new Point(120, 120);

                //Start Game
                drawSnake();
                drawFood();

                timer.Start();

                //Disable Some Controls
                trackBar1.Enabled = false;
                btnStart.Enabled = false;
                txtName.Enabled = false;
                dataGridView1.Enabled = false;


                btnStop.Enabled = true;
            }*/
        


            
        private void timer_Tick_2(object sender, EventArgs e)
        {
            move();
        }

        private void btnStop_Click_2(object sender, EventArgs e)
        {
            stopGame();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2(ds);
            f2.Show();
        }

       
    }
    }
