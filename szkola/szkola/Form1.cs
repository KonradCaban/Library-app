using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Data.SqlClient;

namespace szkola
{
    public partial class Form1 : Form
    {
        SqlConnection cnn;
        String msg;
        string connstring;
        
        public Form1()
        {
            connstring = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\lifetecc\\Documents\\Visual Studio 2015\\Projects\\szkola\\szkola\\biblioteka.mdf\";Integrated Security=True;Connect Timeout=30";
                      cnn = new SqlConnection(connstring);
            cnn.Open();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
       
            if (String.IsNullOrWhiteSpace(textBox1.Text) || String.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Nie wprowadzono loginu i/bądź hasła.", "BŁĄD");
                textBox2.Text = "";
            }
            else
            {
                SqlCommand cmd = new SqlCommand("dbo.LoginInto", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pLoginName", SqlDbType.NVarChar, 254);
                cmd.Parameters.Add("@pPassword", SqlDbType.NVarChar, 50);
                cmd.Parameters.Add("@responseMessage", SqlDbType.NVarChar, 250).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@usid", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@rola", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                cmd.Parameters["@pLoginName"].Value = textBox1.Text;
                cmd.Parameters["@pPassword"].Value = textBox2.Text;
                cmd.ExecuteNonQuery();
                msg = Convert.ToString(cmd.Parameters["@responseMessage"].Value);
               if(msg.Equals("Correct"))
                {
                    if (Convert.ToString(cmd.Parameters["@rola"].Value).Equals("uzytkownik"))
                    {
                        this.Hide();
                        user mm = new user(Convert.ToInt32(cmd.Parameters["@usid"].Value),cnn);
                        mm.Show();
                    }
                    else
                    {
                        this.Hide();
                        administrator adm = new administrator(Convert.ToInt32(cmd.Parameters["@usid"].Value),cnn);
                        adm.Show();
                    }
                }
               else
                {
                    MessageBox.Show(msg);
                    textBox2.Text = "";
                }
            }
            
            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void rejestracja_Click(object sender, EventArgs e)
        {
            this.Hide();
            rejestracja form = new rejestracja(cnn);
            form.Show();
        }
    }
}
