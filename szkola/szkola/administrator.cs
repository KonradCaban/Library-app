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
    public partial class administrator : Form
    {
        public class ComboBoxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }
            public override string ToString()
            {
                return Text;
            }
        }
        //dane usr
        int ID;
        SqlConnection cnn;

        //dodawanie Ksiażki
        string tytul, wydawca, opis;
        int rokwydania;

        //zmiamna ksiazki
        string tytulZ, rokwydaniaZ, wydawcaZ, opisZ, kategoriaZ;

        //dodawanie autora
        string imie, nazwisko, uwagiAutora;

        //dodawanie autora
        string imieZ, nazwiskoZ, uwagiAutoraZ;

        //panel kategorii
        string nowaKategoria;
        int IDkategorii=0;

        //panel wydawcow
        string nowyWydawca;
        int IDwydawcy=0;

        //panel egzemplarzu
        int uszkodzenie=0, IDKsiazki=0, zmienStan=0, nowyStatus=0;

        //wykres1 zmienne seri
        int OSY;
        string OSX;

        //wykres2 zmienne seri
        int osy;
        string osx;


        private void rokWydania_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }




        public administrator(int _id, SqlConnection _cnn)
        {
            this.ID = _id;
            this.cnn = _cnn;
            InitializeComponent();
            load_autorzy();
            uzupelnijAutorow();
            uzupelnijKsiazki();
            uzupelnijwyswietlkategorie();
            uzupelnijpanelWydawcy_Lista();
            uzupelnijpanelEgzemplarzy_Lista1();
            uzupelnijPanelWydawcow();
            uzupelnijPanelKategori();
            uzupelnijPanelEgzemplarzy_Lista2();
        }

        private void uzupelnijPanelWydawcow()
        {
   
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select nazwa from [wykaz wydawcow]";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            wszysycyWydawcy.DataSource = dt;
           
        }

        private void uzupelnijPanelEgzemplarzy_Lista2()
        {
            panelEgzemplarzy_Lista2.Items.Clear();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select tytul, idegzemplarza from [wykaz publikacji ] as wp join [wykaz egzemplarzy] as we on wp.IDksiazki = we.IDksiazki";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            panelEgzemplarzy_Lista2.DisplayMember="tytul";
            panelEgzemplarzy_Lista2.ValueMember = "ID";
            foreach (DataRow dr in dt.Rows)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Text = dr["tytul"].ToString()+ " | egzemplarz: " + dr["idegzemplarza"].ToString();
                item.Value = dr["idegzemplarza"].ToString();
               
                panelEgzemplarzy_Lista2.Items.Add(item);
            }
        }

        private void uzupelnijPanelKategori()
        {

            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select kategoria from kategorie";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            wszystkieKategorie.DataSource = dt;
        }

        private void uzupelnijpanelEgzemplarzy_Lista1()
        {
            panelEgzemplarzy_Lista1.Items.Clear();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select tytul from [wykaz publikacji]";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                panelEgzemplarzy_Lista1.Items.Add(dr["tytul"].ToString());
            }
        }


        private void uzupelnijKsiazki()
        {
            zmienKsiazke.Items.Clear();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select tytul from [wykaz publikacji]";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                zmienKsiazke.Items.Add(dr["tytul"].ToString());
            }
        }

        private void uzupelnijAutorow()
        {
            autorzyzmiana.Items.Clear();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select nazwisko from autorzy";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                autorzyzmiana.Items.Add(dr["nazwisko"].ToString());
            }


        }

        private void uzupelnijwyswietlkategorie()
        {
            wyswietlkategorie.Items.Clear();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select kategoria from kategorie";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                wyswietlkategorie.Items.Add(dr["kategoria"].ToString());
            }
        }

        private void uzupelnijpanelWydawcy_Lista()
        {
            panelWydawcy_Lista.Items.Clear();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select nazwa from [wykaz wydawcow]";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                panelWydawcy_Lista.Items.Add(dr["nazwa"].ToString());
            }
        }




        private void wydawnictwozmien_SelectedIndexChanged(object sender, EventArgs e)
        {
            wydawcaZ = wydawnictwozmien.SelectedItem.ToString();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select IDwydawcy from [wykaz wydawcow] where nazwa = '"+ wydawcaZ + "'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                wydawcaZ = dr["IDwydawcy"].ToString();
            }
        }

        private void kategoriazmien_SelectedIndexChanged(object sender, EventArgs e)
        {
            kategoriaZ = kategoriazmien.SelectedItem.ToString();

            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select IDkategorii from kategorie where kategoria = '" + kategoriaZ + "'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                kategoriaZ = dr["IDkategorii"].ToString();
            }
        }


        private void zmienKsiazke_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                kategoriazmien.Items.Clear();
                wydawnictwozmien.Items.Clear();
                SqlCommand cmd1 = cnn.CreateCommand();
                SqlCommand cmd2 = cnn.CreateCommand();
                SqlCommand cmd3 = cnn.CreateCommand();
                cmd1.CommandType = CommandType.Text;
                cmd2.CommandType = CommandType.Text;
                cmd3.CommandType = CommandType.Text;
                cmd1.CommandText = "select tytul, rok, słowa_kluczowe from [wykaz publikacji] where tytul ='" + zmienKsiazke.SelectedItem.ToString() + " ' ";
                cmd2.CommandText = "select kategoria from kategorie ";
                cmd3.CommandText = "select nazwa from [wykaz wydawcow] ";
                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                cmd3.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    tytulzmien.Text = dr["tytul"].ToString();
                    tytulZ = dr["tytul"].ToString();
                    rokzmien.Text = dr["rok"].ToString();
                    rokwydaniaZ = dr["rok"].ToString();

                    tagizmien.Text = dr["słowa_kluczowe"].ToString();
                    opisZ = dr["słowa_kluczowe"].ToString();
                }
                dt.Clear();
                da = new SqlDataAdapter(cmd3);
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    wydawnictwozmien.Items.Add(dr["nazwa"].ToString());


                }
                dt.Clear();
                da = new SqlDataAdapter(cmd2);
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    kategoriazmien.Items.Add(dr["kategoria"].ToString());

                }
            }
            catch (Exception ex)
            {
                zmienKsiazke.SelectedIndex = 0;
            }

            
        }


        private void zmianyksiazki_Click(object sender, EventArgs e)
        {

            if (!String.IsNullOrWhiteSpace(tytulzmien.Text.ToString()) &&
                   !String.IsNullOrWhiteSpace(rokzmien.Text.ToString()) &&
                   !String.IsNullOrWhiteSpace(wydawnictwozmien.Text.ToString()) &&
                   
                   !String.IsNullOrWhiteSpace(tagizmien.Text.ToString()) &&
                   !String.IsNullOrWhiteSpace(kategoriazmien.Text.ToString()) 
)
            {
                
                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update [wykaz publikacji] set " +
                    "tytul=@tytul, " +
                    "IDwydawcy=@IDwydawcy," +
                    "rok=@rok, " +
                    "słowa_kluczowe=@tagi," +
                    "IDkategorii=@kategoria " +
                    " where tytul='" 
                    + zmienKsiazke.SelectedItem.ToString() + "'";
                cmd.Parameters.Add("@tytul", tytulzmien.Text.ToString());
                cmd.Parameters.Add("@IDwydawcy", Convert.ToInt32(wydawcaZ));
                cmd.Parameters.Add("@rok", Convert.ToInt32(rokzmien.Text.ToString()));
                cmd.Parameters.Add("@tagi", tagizmien.Text.ToString());
                cmd.Parameters.Add("@kategoria", Convert.ToInt32(kategoriaZ));           
                cmd.ExecuteNonQuery();
                MessageBox.Show("Dane zostały zmienione!");
                uzupelnijKsiazki();

            }
            else
            {
                MessageBox.Show("Wszystkie pola są wymagane!");
            }

        }


        private void zmienAutora_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(zmienimie.Text.ToString()) &&
               !String.IsNullOrWhiteSpace(zmiennazwisko.Text.ToString()) &&
               !String.IsNullOrWhiteSpace(zmienuwagi.Text.ToString())
           )
            {
                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update autorzy set imie=@name, nazwisko=@lastname, uwagi=@uwagi where nazwisko='" + autorzyzmiana.SelectedItem.ToString() + "'";
                cmd.Parameters.Add("name", zmienimie.Text.ToString());
                cmd.Parameters.Add("lastname", zmiennazwisko.Text.ToString());
                cmd.Parameters.Add("uwagi", zmienuwagi.Text.ToString());
                cmd.ExecuteNonQuery();
                MessageBox.Show("Dane zostały zmienione!");
                uzupelnijAutorow();
                
            }
            else
            {
                MessageBox.Show("Wszystkie pola są wymagane!");
            }


        }


        private void autorzyzmiana_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from autorzy where nazwisko ='" + autorzyzmiana.SelectedItem.ToString() + " ' ";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach(DataRow dr in dt.Rows)
            {
                zmienimie.Text = dr["imie"].ToString();
                zmiennazwisko.Text = dr["nazwisko"].ToString();
                zmienuwagi.Text = dr["uwagi"].ToString();
                imieZ = dr["imie"].ToString();
                nazwiskoZ = dr["nazwisko"].ToString();
                uwagiAutoraZ = dr["uwagi"].ToString();
            }

        }

        private void load_autorzy()
        {
            kategoriaKsiazki.Items.Clear();
            wydawnictwoComboBox.Items.Clear();
            autorzy.Items.Clear();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from autorzy";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);


            foreach (DataRow dr in dt.Rows)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Text = dr["imie"].ToString() + " " + dr["nazwisko"].ToString();
                item.Value = dr["IDautora"].ToString();
                autorzy.Items.Add(item);
            }
            cmd.CommandText = "Select * from [Wykaz wydawcow]";
            cmd.ExecuteNonQuery();
            da = new SqlDataAdapter(cmd);
            dt.Clear();
            da.Fill(dt);
            wydawnictwoComboBox.DisplayMember = "NazwaWydawnictwa";
            wydawnictwoComboBox.ValueMember = "ID";
            foreach (DataRow dr in dt.Rows)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Text = dr["NAZWA"].ToString();
                item.Value = dr["IDwydawcy"].ToString();
                wydawnictwoComboBox.Items.Add(item);
            }

            cmd.CommandText = "Select * from [KATEGORIE]";
            cmd.ExecuteNonQuery();
            da = new SqlDataAdapter(cmd);
            dt.Clear();
            da.Fill(dt);
            kategoriaKsiazki.DisplayMember = "NazwaKategorii";
            kategoriaKsiazki.ValueMember = "ID";
            foreach (DataRow dr in dt.Rows)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Text = dr["KATEGORIA"].ToString();
                item.Value = dr["IDkategorii"].ToString();
                kategoriaKsiazki.Items.Add(item);
            }

        }



        private void ksiazki_Click(object sender, EventArgs e)
        {
            panelKsiazek.BringToFront();
            load_autorzy();
        }


        private void dodajKsiazke_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(tytulKsiazki.Text.ToString()) &&
               !String.IsNullOrWhiteSpace(rokWydania.Text.ToString()) &&
               !String.IsNullOrWhiteSpace(wydawnictwoComboBox.Text.ToString()) &&
               !String.IsNullOrWhiteSpace(tagi.Text.ToString()) &&
               kategoriaKsiazki.SelectedIndex > -1 &&
               wydawnictwoComboBox.SelectedIndex > -1 &&
               autorzy.SelectedItems.Count > 0
               )
            {

                tytul = tytulKsiazki.Text.ToString();
                rokwydania = Convert.ToInt32(rokWydania.Text.ToString());
                wydawca = wydawnictwoComboBox.Text.ToString();
                opis = tagi.Text.ToString();
                MessageBox.Show(((ComboBoxItem)kategoriaKsiazki.SelectedItem).Value.ToString());
                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into [Wykaz Publikacji](IDkategorii,TYTUL,IDwydawcy,ROK,SŁOWA_KLUCZOWE) output INSERTED.IDksiazki values(@IDkat,@tytul,@IDwydawcy,@rok,@tagi)";
                cmd.Parameters.Add("@IDkat", SqlDbType.Int);
                cmd.Parameters.Add("@tytul", SqlDbType.NVarChar, 200);
                cmd.Parameters.Add("@IDwydawcy", SqlDbType.Int);
                cmd.Parameters.Add("@rok", SqlDbType.Int);
                cmd.Parameters.Add("@tagi", SqlDbType.NVarChar, 200);
                cmd.Parameters["@IDkat"].Value = ((ComboBoxItem)kategoriaKsiazki.SelectedItem).Value;
                cmd.Parameters["@tytul"].Value = tytul;
                cmd.Parameters["@IDwydawcy"].Value = ((ComboBoxItem)wydawnictwoComboBox.SelectedItem).Value;
                cmd.Parameters["@rok"].Value = rokwydania;
                cmd.Parameters["@tagi"].Value = opis;
                try
                {
                    long id = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.CommandText = "insert into [posrednia_autor_ksiazka] Values (@idaut,@idks)";
                    cmd.Parameters.Add(new SqlParameter("@idaut", 0));
                    cmd.Parameters.Add(new SqlParameter("@idks", 0));
                    foreach (object _item in autorzy.CheckedItems)
                    {
                        var item = (ComboBoxItem)_item;
                        cmd.Parameters["@idaut"].Value = item.Value;
                        cmd.Parameters["@idks"].Value = id;
                        cmd.ExecuteNonQuery();

                    }
                }catch(SqlException ex)
                {
                    MessageBox.Show("Podales zly rok wydania ksiazki.");
                }



                uzupelnijKsiazki();
            }
            else
            {
                MessageBox.Show("Wszystkie pola są wymagane!");
            }
        }


        private void rokzmien_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void dodajAutora_Click(object sender, EventArgs e)
        {
            panelAutorow.BringToFront();
        }


        private void dodoajautora_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(imieautora.Text.ToString()) &&
               !String.IsNullOrWhiteSpace(nazwiskoautora.Text.ToString()) &&
               !String.IsNullOrWhiteSpace(uwagi.Text.ToString()) 
               )
            {
                imie = imieautora.Text.ToString();
                nazwisko = nazwiskoautora.Text.ToString();
                uwagiAutora = uwagi.Text.ToString();

                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into Autorzy (imie, nazwisko, uwagi) values(@imie,@nazwisko,@uwagi)";
                cmd.Parameters.AddWithValue("@imie", imie);
                cmd.Parameters.AddWithValue("@nazwisko", nazwisko);
                cmd.Parameters.AddWithValue("@uwagi", uwagiAutora);
                cmd.ExecuteNonQuery();
                uzupelnijAutorow();

            }
            else
            {
                MessageBox.Show("Wszystkie pola są wymagane!");
            }
        }

        private void wyswietlStatystyki_Click(object sender, EventArgs e)
        {
            wykres2.BringToFront();
            statystykiWykres.Series["Wypozyczone"].Points.Clear();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select tytul, count(*) as suma from [rejestr wypozyczen] as rw join [wykaz egzemplarzy] as we on rw.IDegzemplarza = we.IDegzemplarza join[wykaz publikacji] as wp on we.IDksiazki = wp.IDksiazki group by tytul";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                osy = Convert.ToInt32(dr["suma"].ToString());
                osx = dr["TYTUL"].ToString() + " " +dr["suma"].ToString();
                statystykiWykres.Series["Wypozyczone"].Points.AddXY(osx, osy);

            }

        }

        private void button13_Click(object sender, EventArgs e)
        {
            wykres1.BringToFront();
            wykresEgzemplarzy.Series["Egzemplarze"].Points.Clear();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select TYTUL,count(*) as liczba from [WYKAZ PUBLIKACJI] as p join[WYKAZ EGZEMPLARZY] as e on e.IDksiazki = p.IDksiazki group by TYTUL";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                OSY = Convert.ToInt32(dr["liczba"].ToString());
                OSX = dr["TYTUL"].ToString();
                wykresEgzemplarzy.Series["Egzemplarze"].Points.AddXY(OSX, OSY);

            }


        }
        private void load_gridview2()
        {
            DataTable table = new DataTable();
            using (var command = new SqlCommand("SELECT TYTUL,IDegzemplarza FROM [WYKAZ EGZEMPLARZY] as we join [WYKAZ PUBLIKACJI] as wp on wp.IDksiazki=We.IDksiazki", cnn))
            {
                // Loads the query results into the table
                table.Load(command.ExecuteReader());
            }
            dataGridView2.DataSource = table;
        }


        private void otworzEgzemplarze_Click(object sender, EventArgs e)
        {
            panelEgzemplarzy.BringToFront();
            uzupelnijpanelEgzemplarzy_Lista1();
            uzupelnijPanelEgzemplarzy_Lista2();

            load_gridview2();
          
        }

        private void panelEgzemplarzy_Ubytki_CheckedChanged(object sender, EventArgs e)
        {
            if (panelEgzemplarzy_Ubytki.Checked)
                uszkodzenie = 1;
            else
                uszkodzenie = 0;
        }

        private void panelEgzemplarzy_checkbox2_CheckedChanged(object sender, EventArgs e)
        {
            if (panelEgzemplarzy_checkbox2.Checked)
                nowyStatus= 1;
            else
                nowyStatus = 0;
        }

        private void GridWypozyczone_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                int index = e.RowIndex;
                DataGridViewRow selectedRow = GridWypozyczone.Rows[index];
                DialogResult dialogResult = MessageBox.Show("Czy na pewno chcesz zwrocic książke pod tytułem: " + selectedRow.Cells[1].Value.ToString()+ ", którą wypożyczył "+selectedRow.Cells[0].Value.ToString()+"?", "Zwrot", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    String tm = DateTime.Now.ToString("yyyy-MM-dd");
                    SqlCommand cmd = new SqlCommand("update[Rejestr WYPOZYCZEN] set Zwrotprzed = @dt where IDegzemplarza=@idegz AND Datawyp=@datawyp AND nrkarty=@nrk", cnn);
                    cmd.Parameters.AddWithValue("@idegz", selectedRow.Cells[2].Value.ToString());
                    cmd.Parameters.AddWithValue("@dt", tm);
                    cmd.Parameters.AddWithValue("@datawyp", selectedRow.Cells[3].Value.ToString());
                    cmd.Parameters.AddWithValue("@nrk", selectedRow.Cells[4].Value.ToString());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Zwrocono ksiazke.");
                    load_wyp();
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }
            }
        }

        private void panelEgzemplarzy_button2_Click(object sender, EventArgs e)
        {

                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update [wykaz egzemplarzy] set ubytki=@ubytki where idksiazki='" + zmienStan + "'";
                cmd.Parameters.Add("ubytki", nowyStatus);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Dane zostały zmienione!");
            load_gridview2();

        }

        private void logsButton_Click(object sender, EventArgs e)
        {
            panelLogow.BringToFront();
            DataTable table = new DataTable();
            using (var command = new SqlCommand("SELECT * FROM logi", cnn))
            {
                // Loads the query results into the table
                table.Load(command.ExecuteReader());
            }
            dataGridView1.DataSource = table;
        }

        private void panelEgzemplarzy_Lista2_SelectedIndexChanged(object sender, EventArgs e)
        {
            zmienStan = Convert.ToInt32(((ComboBoxItem)panelEgzemplarzy_Lista2.SelectedItem).Value.ToString());
        }

        private void panelEgzemplarzy_Lista1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select IDksiazki from [wykaz publikacji] where tytul ='" + panelEgzemplarzy_Lista1.SelectedItem.ToString() + " ' ";
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    IDKsiazki = Convert.ToInt32(dr["IDksiazki"].ToString());

                }
            }
            catch(Exception ex)
            {
                panelEgzemplarzy_Lista1.SelectedIndex = 0;
            }
        }

        private void panelEgzemplarzy_Button1_Click(object sender, EventArgs e)
        {
            if (
                    IDKsiazki != 0
                )
            {

                nowyWydawca = panelWydawcow_nowyWydawca.Text.ToString();
                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into [wykaz egzemplarzy] (IDksiazki, ubytki) values(@IDksiazki,@ubytki)";
                cmd.Parameters.AddWithValue("@IDksiazki", IDKsiazki);
                cmd.Parameters.AddWithValue("@ubytki", uszkodzenie);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Dodano dane!");
                uzupelnijPanelEgzemplarzy_Lista2();
                load_gridview2();

            }
            else
                MessageBox.Show("Uzupełnij wszystkie pola");

            IDKsiazki = 0;
        }

        private void load_wyp()
        {
            GridWypozyczone.DataSource = null;
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select Imie+' ' +NAZWISKO as Czytelnik, TYTUL, rw.IDegzemplarza ,Datawyp,c.Nrkarty from [REJESTR WYPOZYCZEN] as rw join CZYTELNICY as c on c.NRkarty=rw.NRkarty join [WYKAZ EGZEMPLARZY] as we on we.IDegzemplarza=rw.IDegzemplarza join [WYKAZ PUBLIKACJI] as wp on wp.IDksiazki=we.IDksiazki where Zwrotprzed is null ";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            GridWypozyczone.DataSource = dt;
        }

        private void przyciskZwrotu_Click(object sender, EventArgs e)
        {
            panelZwrotu.BringToFront();
            load_wyp();
        }

        private void otworzPanelWydawcow_Click(object sender, EventArgs e)
        {
            panelWydawcow.BringToFront();
            uzupelnijPanelWydawcow();
        }

        private void panelWydawcy_Button2_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(panelWydawcy_zmienNazwe.Text.ToString()) &&
                IDwydawcy !=0
            )
            {

                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update [wykaz wydawcow] set nazwa=@nazwa where IDwydawcy='" + IDwydawcy + "'";
                cmd.Parameters.Add("nazwa", panelWydawcy_zmienNazwe.Text.ToString());
                cmd.ExecuteNonQuery();
                MessageBox.Show("Dane zostały zmienione!");
                uzupelnijpanelWydawcy_Lista();

            }
            else
                MessageBox.Show("Uzupełnij wszystkie pola");
            IDwydawcy = 0;
            uzupelnijPanelWydawcow();
        }

        private void panelWydawcy_Lista_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from [wykaz wydawcow] where nazwa ='" + panelWydawcy_Lista.SelectedItem.ToString() + " ' ";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                panelWydawcy_zmienNazwe.Text = dr["nazwa"].ToString();
                IDwydawcy = Convert.ToInt32(dr["IDwydawcy"].ToString());

            }
        }

        private void panelWydawcow_Button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(panelWydawcow_nowyWydawca.Text.ToString())
    )
            {
                nowyWydawca = panelWydawcow_nowyWydawca.Text.ToString();
                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into [wykaz wydawcow] (nazwa) values(@nazwa)";
                cmd.Parameters.AddWithValue("@nazwa", nowyWydawca);
                cmd.ExecuteNonQuery();
                uzupelnijpanelWydawcy_Lista();
            }
            else
                MessageBox.Show("Wpisz nazwę nowej kategori!");
            uzupelnijPanelWydawcow();
        }

        private void otworzPanelKategori_Click(object sender, EventArgs e)
        {
            kategoriapanel.BringToFront();
        }

        private void zmienkategorieprzycisk_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(kategoriapanel_nowanazwa.Text.ToString()) &&
            IDkategorii !=0
            )
            {

                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update kategorie set kategoria=@nazwa where IDkategorii='" + IDkategorii + "'";
                cmd.Parameters.Add("nazwa", kategoriapanel_nowanazwa.Text.ToString());
                cmd.ExecuteNonQuery();
                MessageBox.Show("Dane zostały zmienione!");
                uzupelnijwyswietlkategorie();

            }
            else
                MessageBox.Show("Uzupełnij wszystkie pola");
            IDkategorii = 0;
            uzupelnijPanelKategori();
        }

        private void wyswietlkategorie_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from kategorie where kategoria ='" + wyswietlkategorie.SelectedItem.ToString() + " ' ";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                kategoriapanel_nowanazwa.Text = dr["kategoria"].ToString();
                IDkategorii = Convert.ToInt32(dr["IDkategorii"].ToString());

            }
        }

        private void wyswietlkategorie_SelectedIndexChanged(object sender, EventArgs e)
        {

            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from kategorie where kategoria ='" + wyswietlkategorie.SelectedItem.ToString() + " ' ";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                kategoriapanel_nowanazwa.Text = dr["kategoria"].ToString();
                IDkategorii = Convert.ToInt32(dr["IDkategorii"].ToString());

            }
        }


        private void dodajkategorieprzycisk_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(nazwanowejkategori.Text.ToString())
                )
            {
                nowaKategoria = nazwanowejkategori.Text.ToString();
                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into kategorie (kategoria) values(@kategoria)";
                cmd.Parameters.AddWithValue("@kategoria", nowaKategoria);
                cmd.ExecuteNonQuery();
                uzupelnijwyswietlkategorie();
            }
            else
                MessageBox.Show("Wpisz nazwę nowej kategori!");
            uzupelnijPanelKategori();
        }


        private void wyolgujAdmina_Click(object sender, EventArgs e)
        {
            this.Close();
            cnn.Close();
            Form1 f = new Form1();
            f.Show();
        }

    }
}
