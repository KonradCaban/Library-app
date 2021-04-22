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

namespace szkola
{
    public partial class rejestracja : Form
    {
        SqlConnection cnn;
        string imie1, nazwisko1, miasto1, kod1, ulica1, login, haslo, hasloDwa, komunikat, nr_telefonu;
        DateTime data1;

        private void telefonTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void rejestracja_Load(object sender, EventArgs e)
        {

        }

        Image avatarUsera;



        private void data_ValueChanged(object sender, EventArgs e)
        {
            data1 = this.data.Value.Date;
        }



        private void anuluj_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 f = new Form1();
            f.Show();
        }

        private void wyczysc_Click(object sender, EventArgs e)
        {
            groupBox2.Controls.OfType<TextBox>().ToList().ForEach(t => t.Clear());
            groupBox1.Controls.OfType<TextBox>().ToList().ForEach(t => t.Clear());
        }


        private void zdj_Click(object sender, EventArgs e)
        {
            OpenFileDialog zdj = new OpenFileDialog();
            zdj.Filter = "Bitmaps|*.jpg|*.gif|*.bmp|*.png|";
            if (zdj.ShowDialog() == DialogResult.OK)
            {
                avatarUsera = Image.FromFile(zdj.FileName);
                avatar.Image = avatarUsera;
            }
        }

        private void zamknij_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 f = new Form1();
            f.Show();
        }

        public rejestracja(SqlConnection _cnn)
        {
            cnn = _cnn;
            InitializeComponent();
            data1 = this.data.Value.Date;
        }

        private void rejestruj_Click(object sender, EventArgs e)
        {
            komunikat = "Wartości pól: ";
            if (!String.IsNullOrWhiteSpace(imie.Text.ToString()) &&
                !String.IsNullOrWhiteSpace(nazwisko.Text.ToString()) &&
                !String.IsNullOrWhiteSpace(miasto.Text.ToString()) &&
                !String.IsNullOrWhiteSpace(kodPocztowy.Text.ToString()) &&
                !String.IsNullOrWhiteSpace(logi.Text.ToString()) &&
                !String.IsNullOrWhiteSpace(haslo1.Text.ToString()) &&
                !String.IsNullOrWhiteSpace(haslo2.Text.ToString()) &&
                !String.IsNullOrWhiteSpace(ulica.Text.ToString()) &&
                !String.IsNullOrWhiteSpace(telefonTextBox.Text.ToString()) &&
                data1 != null
                )
            {
                imie1 = imie.Text.ToString();
                nazwisko1 = nazwisko.Text.ToString();
                miasto1 = miasto.Text.ToString();
                kod1 = kodPocztowy.Text.ToString();
                login = logi.Text.ToString();
                haslo = haslo1.Text.ToString();
                hasloDwa = haslo2.Text.ToString();
                ulica1 = ulica.Text.ToString();
                nr_telefonu = telefonTextBox.Text.ToString();

                if (haslo.Equals(hasloDwa, StringComparison.Ordinal) == true)
                {
                    SqlCommand cmd = new SqlCommand("dbo.AddUser", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pLogin", SqlDbType.NVarChar, 254);
                    cmd.Parameters.Add("@pPassword", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@pImie", SqlDbType.NVarChar, 250);
                    cmd.Parameters.Add("@pNazwisko", SqlDbType.NVarChar, 250);
                    cmd.Parameters.Add("@pDataUr", SqlDbType.Date);
                    cmd.Parameters.Add("@pkod", SqlDbType.NVarChar, 7);
                    cmd.Parameters.Add("@pmiasto", SqlDbType.NVarChar, 250);
                    cmd.Parameters.Add("@pUlica", SqlDbType.NVarChar, 250);
                    cmd.Parameters.Add("@pTelefon", SqlDbType.Int);
                    cmd.Parameters.Add("@responseMessage", SqlDbType.NVarChar, 250).Direction = ParameterDirection.Output;
                    cmd.Parameters["@pLogin"].Value = login;
                    cmd.Parameters["@pPassword"].Value = haslo;
                    cmd.Parameters["@pImie"].Value = imie1;
                    cmd.Parameters["@pNazwisko"].Value = nazwisko1;
                    cmd.Parameters["@pDataUr"].Value = data1;
                    cmd.Parameters["@pkod"].Value = kod1;
                    cmd.Parameters["@pmiasto"].Value = miasto1;
                    cmd.Parameters["@pUlica"].Value = ulica1;
                    cmd.Parameters["@pTelefon"].Value = Convert.ToInt32(nr_telefonu);
                    cmd.ExecuteNonQuery();
                    String msg = Convert.ToString(cmd.Parameters["@responseMessage"].Value);

                    if (msg.Equals("Success"))
                    {
                        MessageBox.Show("Zarejestrowano nowe konto. Nastąpi przjescie do menu głównego.");
                        this.Close();
                        Form1 f = new Form1();
                        f.Show();
                    }
                    else
                    {
                        if (msg.Contains("CK_C_KOD"))
                        {
                            MessageBox.Show("Podales zły kod pocztowy.");

                        }
                        else
                        {
                            if (msg.Contains("UNIQUE KEY"))
                                MessageBox.Show("Konto o podanym loginie już istnieje w bazie danych. Wybierz inny login.");
                            else
                                MessageBox.Show(msg);
                        }

                    }


                }
                else
                    MessageBox.Show("Hasła muszą być takie same!");

            }
            else
                MessageBox.Show("Wszystkie pola są wymagane!");




        }
    }
}
