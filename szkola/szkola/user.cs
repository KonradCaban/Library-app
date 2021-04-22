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
    public partial class user : Form
    {
        SqlConnection cnn;
        int id;

        public user(int _id, SqlConnection _cnn)
        {
            id = _id;
            InitializeComponent();
            cnn = _cnn;
            SqlCommand cmd = new SqlCommand("select LoginName,IMIE, NAZWISKO,DataUr,miasto,ulica,telefon from [CZYTELNICY] where NRkarty=@id ", cnn);
            cmd.Parameters.AddWithValue("@id", id);
            SqlCommand cmd2 = new SqlCommand("select count(*) from [REJESTR WYPOZYCZEN] where nrkarty = @ID", cnn);
            cmd2.Parameters.AddWithValue("@id", id);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    DateTime dt = (DateTime)reader["DataUr"];
                    labelImie.Text = Convert.ToString(reader["imie"]);
                    labelNazwisko.Text = Convert.ToString(reader["nazwisko"]);
                    labelData.Text = Convert.ToString(dt.ToString("yyyy/MM/dd"));
                    labelAdres.Text = Convert.ToString(reader["miasto"]) + ", " + Convert.ToString(reader["ulica"]);
                    labelTelefon.Text = Convert.ToString(reader["telefon"]);

                }
            }
            labelCount.Text = Convert.ToString(cmd2.ExecuteScalar());
            // this.pictureBox1.Image = grafika.jpg;


        }

        private void load_historia()
        {
            SqlCommand sqlCmd = new SqlCommand("select stuff((SELECT DISTINCT ', ' + x.IMIE + ' ' + x.NAZWISKo FROM posrednia_autor_ksiazka as p join AUTORZY as x on x.IDautora = p.idautora WHERE idksiazki = t.idksiazki FOR XML PATH('')), 1, 2, '') as Autorzy, d.TYTUL as Tytuł,rw.Datawyp as [Data wypożyczenia],rw.Zwrotprzed as [Data wczesniejszego zwrotu],rw.DataZwrotu as [Końcowy termin zwrotu],k.KATEGORIA as Kategoria FROM (SELECT DISTINCT idksiazki FROM posrednia_autor_ksiazka) t join[WYKAZ PUBLIKACJI] as d on d.IDksiazki = t.idksiazki join[WYKAZ EGZEMPLARZY] as w on w.IDksiazki = d.IDksiazki join[REJESTR WYPOZYCZEN] as rw on rw.IDegzemplarza = w.IDegzemplarza join Kategorie as k on k.IDkategorii=d.IDkategorii where rw.NRkarty = @id");
            sqlCmd.Connection = cnn;
            sqlCmd.Parameters.AddWithValue("@id", id);
            sqlCmd.CommandType = CommandType.Text;

            SqlDataAdapter sqlDataAdap = new SqlDataAdapter(sqlCmd);



           



            DataTable dtRecord = new DataTable();
            sqlDataAdap.Fill(dtRecord);
            dataGridView2.DataSource = dtRecord;
            foreach (DataGridViewColumn column in dataGridView2.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panelHistorii.BringToFront();
            load_historia();

        }

        private void loadgrid()
        {

            szukaj.BringToFront();
            SqlCommand cmd = new SqlCommand("select * from(select Distinct d.TYTUL as Tytuł, stuff((SELECT DISTINCT ', ' + x.IMIE + ' ' + x.NAZWISKo FROM posrednia_autor_ksiazka as p join AUTORZY as x on x.IDautora = p.idautora WHERE idksiazki = t.idksiazki FOR XML PATH('')), 1, 2, '') as Autorzy, k.KATEGORIA as Kategoria, ww.NAZWA as wydawca FROM(SELECT DISTINCT idksiazki FROM posrednia_autor_ksiazka) t join[WYKAZ PUBLIKACJI] as d on d.IDksiazki = t.idksiazki join[KATEGORIE] as k on k.IDkategorii = d.IDkategorii join[Wykaz Wydawcow] as ww on ww.IDwydawcy = d.IDwydawcy) as innertab where Autorzy Like @autor AND Tytuł Like @tytul AND Kategoria Like @kategoria AND wydawca Like @wydawca ");
            cmd.Connection = cnn;
            cmd.Parameters.Add("@autor", SqlDbType.NVarChar, 254);
            cmd.Parameters.Add("@tytul", SqlDbType.NVarChar, 254);
            cmd.Parameters.Add("@kategoria", SqlDbType.NVarChar, 35);
            cmd.Parameters.Add("@wydawca", SqlDbType.NVarChar, 254);
            cmd.Parameters["@autor"].Value = "%" + imieAutora.Text + "%";
            cmd.Parameters["@tytul"].Value = "%" + tytulKsiazki.Text + "%";
            cmd.Parameters["@kategoria"].Value = "%" + kategoriaTextBox.Text + "%";
            cmd.Parameters["@wydawca"].Value = "%" + labelWydawca.Text + "%";
            cmd.CommandType = CommandType.Text;

            SqlDataAdapter sqlDataAdap = new SqlDataAdapter(cmd);

            DataTable dtRecord = new DataTable();
            sqlDataAdap.Fill(dtRecord);
            dataGridView3.DataSource = dtRecord;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            loadgrid();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            cnn.Close();
            Form1 f = new Form1();
            f.Show();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            loadgrid();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SqlCommand cmd2 = new SqlCommand("select count(*) from [REJESTR WYPOZYCZEN] where nrkarty = @ID", cnn);
            cmd2.Parameters.AddWithValue("@id", id);
            labelCount.Text = Convert.ToString(cmd2.ExecuteScalar());
            panelUzytkownika.BringToFront();
        }

        private void admin_Load(object sender, EventArgs e)
        {

        }

        private void passChangeButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(oldPass.Text) || String.IsNullOrWhiteSpace(newPass.Text) || String.IsNullOrWhiteSpace(newPass2.Text))
            {
                MessageBox.Show("Nie uzupełniłeś wszystkich pól.");
            }
            else
            {
                if (!newPass.Text.Equals(newPass2.Text))
                {
                    MessageBox.Show("Podałeś różne hasła.");
                    newPass.Text = "";
                    newPass2.Text = "";
                }
                else
                {
                    if (newPass.Text.Equals(oldPass.Text))
                    {
                        MessageBox.Show("Nowe hasło nie może być takie samo jak stare.");
                        newPass.Text = "";
                        newPass2.Text = "";

                    }
                    else
                    {
                        SqlCommand cmd = new SqlCommand("Select dbo.pass(@newpass,@id)", cnn);
                        cmd.Parameters.Add("@newpass", SqlDbType.NVarChar, 250);
                        cmd.Parameters.Add("@id", SqlDbType.Int);
                        cmd.Parameters["@newpass"].Value = oldPass.Text;
                        cmd.Parameters["@id"].Value = id;
                        if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                        {
                            MessageBox.Show("Podales zle stare haslo.");
                        }
                        else
                        {
                            cmd = new SqlCommand("Update [Czytelnicy] SET PasswordHash = HASHBYTES('SHA2_512', @pPassword) WHERE NRkarty = @id", cnn);
                            cmd.Parameters.Add("@pPassword", SqlDbType.NVarChar, 250);
                            cmd.Parameters.Add("@id", SqlDbType.Int);
                            cmd.Parameters["@pPassword"].Value = newPass.Text;
                            cmd.Parameters["@id"].Value = id;
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Haslo zostalo zmienione.");
                        }

                    }
                }
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            panelHistorii.BringToFront();
            SqlCommand sqlCmd = new SqlCommand("select stuff((SELECT DISTINCT ', ' + x.IMIE + ' ' + x.NAZWISKo FROM posrednia_autor_ksiazka as p join AUTORZY as x on x.IDautora = p.idautora WHERE idksiazki = t.idksiazki FOR XML PATH('')), 1, 2, '') as Autorzy, d.TYTUL as Tytuł,rw.Datawyp as [Data wypożyczenia],rw.Zwrotprzed as [Data wczesniejszego zwrotu],rw.DataZwrotu as [Końcowy termin zwrotu],k.KATEGORIA as Kategoria FROM (SELECT DISTINCT idksiazki FROM posrednia_autor_ksiazka) t join[WYKAZ PUBLIKACJI] as d on d.IDksiazki = t.idksiazki join[WYKAZ EGZEMPLARZY] as w on w.IDksiazki = d.IDksiazki join[REJESTR WYPOZYCZEN] as rw on rw.IDegzemplarza = w.IDegzemplarza join Kategorie as k on k.IDkategorii=d.IDkategorii where rw.NRkarty = @id order by 1");
            sqlCmd.Connection = cnn;
            sqlCmd.Parameters.AddWithValue("@id", id);
            sqlCmd.CommandType = CommandType.Text;

            SqlDataAdapter sqlDataAdap = new SqlDataAdapter(sqlCmd);

            DataTable dtRecord = new DataTable();
            sqlDataAdap.Fill(dtRecord);
            dataGridView2.DataSource = dtRecord;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            panelHistorii.BringToFront();
            SqlCommand sqlCmd = new SqlCommand("select stuff((SELECT DISTINCT ', ' + x.IMIE + ' ' + x.NAZWISKo FROM posrednia_autor_ksiazka as p join AUTORZY as x on x.IDautora = p.idautora WHERE idksiazki = t.idksiazki FOR XML PATH('')), 1, 2, '') as Autorzy, d.TYTUL as Tytuł,rw.Datawyp as [Data wypożyczenia],rw.Zwrotprzed as [Data wczesniejszego zwrotu],rw.DataZwrotu as [Końcowy termin zwrotu],k.KATEGORIA as Kategoria FROM (SELECT DISTINCT idksiazki FROM posrednia_autor_ksiazka) t join[WYKAZ PUBLIKACJI] as d on d.IDksiazki = t.idksiazki join[WYKAZ EGZEMPLARZY] as w on w.IDksiazki = d.IDksiazki join[REJESTR WYPOZYCZEN] as rw on rw.IDegzemplarza = w.IDegzemplarza join Kategorie as k on k.IDkategorii=d.IDkategorii where rw.NRkarty = @id order by 3");
            sqlCmd.Connection = cnn;
            sqlCmd.Parameters.AddWithValue("@id", id);
            sqlCmd.CommandType = CommandType.Text;

            SqlDataAdapter sqlDataAdap = new SqlDataAdapter(sqlCmd);

            DataTable dtRecord = new DataTable();
            sqlDataAdap.Fill(dtRecord);
            dataGridView2.DataSource = dtRecord;
        }

        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {

        }



        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView3_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                SqlCommand sqlCmd = new SqlCommand("select IDegzemplarza, TYTUL from[WYKAZ EGZEMPLARZY] as we join[WYKAZ PUBLIKACJI] as wp on wp.IDksiazki = we.IDksiazki where UBYTKI = 0 AND tytul Like @tytul EXCEPT select we.IDegzemplarza,wp.TYTUL from[WYKAZ EGZEMPLARZY] as we join[WYKAZ PUBLIKACJI] as wp on wp.IDksiazki = we.IDksiazki join[REJESTR WYPOZYCZEN] as rw on rw.IDegzemplarza = we.IDegzemplarza where rw.Zwrotprzed is null and tytul Like @tytul");
                sqlCmd.Connection = cnn;
                int index = e.RowIndex;
                sqlCmd.Parameters.Add("@tytul", SqlDbType.NVarChar, 254);
                DataGridViewRow selectedRow = dataGridView3.Rows[index];
                sqlCmd.Parameters["@tytul"].Value = "%" + selectedRow.Cells[0].Value.ToString() + "%";
                sqlCmd.CommandType = CommandType.Text;

                SqlDataAdapter sqlDataAdap = new SqlDataAdapter(sqlCmd);

                DataTable dtRecord = new DataTable();
                sqlDataAdap.Fill(dtRecord);
                dataGridView4.DataSource = dtRecord;
            }
        }

        private void dataGridView4_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                int index = e.RowIndex;
                DataGridViewRow selectedRow = dataGridView4.Rows[index];
                DialogResult dialogResult = MessageBox.Show("Czy na pewno chcesz wypożyczyć książke pod tytułem: " + selectedRow.Cells[1].Value.ToString(), "Wypozyczenie", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand("insert into [Rejestr WYPOZYCZEN](NRkarty,IDegzemplarza) values(@id,@egz)", cnn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@egz", selectedRow.Cells[0].Value.ToString());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Wypozyczyles ksiazke. Masz na jej zwrot 3 miesiące.");
                    panelHistorii.BringToFront();
                    load_historia();
                    dataGridView3.DataSource = "";
                    dataGridView4.DataSource = "";
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }
            }
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void panelHistorii_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
