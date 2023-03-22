using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace OtoGaleriOtomasyonDataGridView
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS01;Initial Catalog=galeri;Integrated Security=True");
        private void Form1_Load(object sender, EventArgs e)
        {

            string[] markalar = { "Toyota", "Ford", "Fiat", "Renault", "Hyundai", "Volkswagen" };           
            string[] modelyili = {"2012", "2013", "2014", "2015", "2016", "2017", "2018", "2019", "2020", "2021", "2022", "2023" };
            string[] yakittipi = { "Benzin", "Motorin", "Benzin&LPG" };
            string[] kasatipi = { "Hatchback", "Sedan", "Station Wagon" };
            string[] vites = { "Manuel", "Otomatik", "Yarı Otomatik" };
            string[] renk = { "Sarı", "Lacivert", "Gri", "Siyah", "Beyaz", "Kırmızı", "Mavi" };

            cmbmarka.Items.AddRange(markalar);
            cmbmodyil.Items.AddRange(modelyili);
            cmbyakittur.Items.AddRange(yakittipi);
            cmbkasatipi.Items.AddRange(kasatipi);
            cmbvites.Items.AddRange(vites);
            cmbrenk.Items.AddRange(renk);           
        }

        private void cmbmarka_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbmodel.Items.Clear();
            string marka = cmbmarka.SelectedItem.ToString();
            if (marka == "Toyota")
            {
                string[] toyotamodeller = { "Corolla", "Yaris" };
                cmbmodel.Items.AddRange(toyotamodeller);
            }
            else if (marka == "Ford")
            {
                string[] fordmodeller = { "Focus", "Fiesta", "Transit", "Tourneo" };
                cmbmodel.Items.AddRange(fordmodeller);
            }
            else if (marka == "Fiat")
            {
                string[] fiatmodeller = { "Egea", "Doblo", "500L", "Fiorino" };
                cmbmodel.Items.AddRange(fiatmodeller);
            }
            else if (marka == "Renault")
            {
                string[] renaultmodeller = { "Clio", "Megane", "Symbol", "Talisman", "Kadjar" };
                cmbmodel.Items.AddRange(renaultmodeller);
            }
            else if (marka == "Hyundai")
            {
                string[] hyundaimodeller = { "i10", "i20", "i30", "Accent", "Elantra" };
                cmbmodel.Items.AddRange(hyundaimodeller);
            }
            else if (marka == "Volkswagen")
            {
                string[] volkwagenmodeller = { "Passat", "Jetta", "Caddy", "Golf", "Polo", "Tiguan" };
                cmbmodel.Items.AddRange(volkwagenmodeller);
            }
        }

        private void KayitlariGoster(string veriler)//VERİTABANINDAKİ VERİLERGİ GÖSTERİYORUZ. VE DATAGRİDVİEWE YAZDIRIYORUZ.
        {
            try
            {               
                SqlDataAdapter verilerigetir = new SqlDataAdapter(veriler, baglanti);
                DataSet dataset = new DataSet();
                verilerigetir.Fill(dataset);
                dataGridView1.DataSource = dataset.Tables[0];                 
            }
            catch(Exception mesaj)
            {
                MessageBox.Show(mesaj.Message);
                baglanti.Close();
            }
        }   
        private void Temizle()
        {
            txtruhsat.Clear();
            txtkm.Clear();
            txtfiyat.Clear();
            cmbkasatipi.Text = "";
            cmbmarka.Text = "";
            cmbmodel.Text = "";
            cmbmodyil.Text = "";
            cmbrenk.Text = "";
            cmbvites.Text = "";
            cmbyakittur.Text = "";
        }

        private void bttnkaydet_Click(object sender, EventArgs e)//VERİTABANINDAKİ TABLOMUZA VERİ EKLEME VE DATAGRİDVİEW DA GÖRÜNTÜLEME İŞLEMLERİNE İLİŞKİN KODLAR.
        {
            try
            {
                baglanti.Open();
                string verieklesorgu = $"insert into araclistesi(ruhsatno,marka,model, modelyili,yakittipi,kasatipi,kilometre,fiyat,vitesturu,renk) values (@a,@b,@c,@d,@e,@f,@g,@h,@i,@j)";
                SqlCommand verigetir = new SqlCommand(verieklesorgu,baglanti);
                verigetir.Parameters.AddWithValue("@a", txtruhsat.Text);
                verigetir.Parameters.AddWithValue("@b", cmbmarka.Text);
                verigetir.Parameters.AddWithValue("@c", cmbmodel.Text);
                verigetir.Parameters.AddWithValue("@d", cmbmodyil.Text);
                verigetir.Parameters.AddWithValue("@e", cmbyakittur.Text);
                verigetir.Parameters.AddWithValue("@f", cmbkasatipi.Text);
                verigetir.Parameters.AddWithValue("@g", txtkm .Text);
                verigetir.Parameters.AddWithValue("@h", txtfiyat.Text);
                verigetir.Parameters.AddWithValue("@i", cmbvites.Text);
                verigetir.Parameters.AddWithValue("@j", cmbrenk.Text);
                verigetir.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Araç veritabanına eklendi...");
                Temizle();
                KayitlariGoster("select*from araclistesi");  
            }
            catch (Exception)
            {
                MessageBox.Show("Bu ruhsat numarasına ait araç mevcuttur.");
                Temizle();
                baglanti.Close();
            }
        }
        int ruhsatno=0;
        private void bttnguncel_Click(object sender, EventArgs e)//FİYAT BİLGİSİNİ GÜNCELLEMESİNE İLİŞKİN KODLAR.
        {
            try
            {
                baglanti.Open();
                string guncellemesorgusu = $"update araclistesi set fiyat=@h where ruhsatno={ruhsatno}";
                SqlCommand guncellekomutu = new SqlCommand(guncellemesorgusu, baglanti);
                guncellekomutu.Parameters.AddWithValue("@a", txtruhsat.Text.ToString());
                guncellekomutu.Parameters.AddWithValue("@b", cmbmarka.Text.ToString());
                guncellekomutu.Parameters.AddWithValue("@c", cmbmodel.Text.ToString());
                guncellekomutu.Parameters.AddWithValue("@d", cmbmodyil.Text.ToString());
                guncellekomutu.Parameters.AddWithValue("@e", cmbyakittur.Text.ToString());
                guncellekomutu.Parameters.AddWithValue("@f", cmbkasatipi.Text.ToString());
                guncellekomutu.Parameters.AddWithValue("@g", txtkm.Text.ToString());
                guncellekomutu.Parameters.AddWithValue("@h", txtfiyat.Text.ToString());
                guncellekomutu.Parameters.AddWithValue("@i", cmbvites.Text.ToString());
                guncellekomutu.Parameters.AddWithValue("@j", cmbrenk.Text.ToString());
                guncellekomutu.ExecuteNonQuery();
                KayitlariGoster("select*from araclistesi order by asc");
                baglanti.Close();
                MessageBox.Show("Güncelleme işlemi gerçekleştirildi...");
                Temizle();
            }
            catch (Exception)
            {
                MessageBox.Show("Güncellenecek araç seçiniz...");
            }
        }
        private void bttnsil_Click(object sender, EventArgs e)//VERİTABANINDAN VERİ SİLMEMİZİ VE DATAGRİDVİEW DA GÖRÜNTÜLEME SAĞLAYAN KODLAR.
        {
            try
            {
                baglanti.Open();
                string silmesorgusu = "delete from araclistesi where ruhsatno=@ruhsat";
                SqlCommand silmekomutu = new SqlCommand(silmesorgusu, baglanti);
                silmekomutu.Parameters.AddWithValue("@ruhsat", txtruhsat.Text);
                silmekomutu.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Araç veritabanından silindi");
                Temizle();
                KayitlariGoster("select*from araclistesi");
            }
            catch(Exception)
            {
                MessageBox.Show("Silinecek aracı seçmediniz...");
            }                     
        }   
        private void button1_Click(object sender, EventArgs e)
        {
            KayitlariGoster("select*from araclistesi order by ruhsatno asc");
        }
        private void bttnara_Click(object sender, EventArgs e)//RUHSAT NUMARARISAN GÖRE ARAÇ ARIYORUZ.
        {
            try
            {
            baglanti.Open();
            string aramasorgu = $"Select*from araclistesi where ruhsatno={txtruhsat.Text}";                
            SqlCommand aramakomutu = new SqlCommand(aramasorgu, baglanti);
            SqlDataAdapter da = new SqlDataAdapter(aramakomutu);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];            
            baglanti.Close();
            }
            catch(Exception)
            {
                MessageBox.Show("Lütfen ruhsat numarası ile arama yapınız...");
            }
        }       
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)//DATAGRIDVİEW TABLOSUNDAN SECİLEN ARACLARIN İLGİLİ YERLERİNE YAZILMASI SAĞLANDI.
        {
            try
            {
                int secilenalan = dataGridView1.SelectedCells[0].RowIndex;
                ruhsatno = Convert.ToInt32(dataGridView1.Rows[secilenalan].Cells[0].Value);
                string marka = dataGridView1.Rows[secilenalan].Cells[1].Value.ToString();
                string model = dataGridView1.Rows[secilenalan].Cells[2].Value.ToString();
                string modelyili = dataGridView1.Rows[secilenalan].Cells[3].Value.ToString();
                string yakitturu = dataGridView1.Rows[secilenalan].Cells[4].Value.ToString();
                string kasatipi = dataGridView1.Rows[secilenalan].Cells[5].Value.ToString();
                string km = dataGridView1.Rows[secilenalan].Cells[6].Value.ToString();
                string fiyat = dataGridView1.Rows[secilenalan].Cells[7].Value.ToString();
                string vitesturu = dataGridView1.Rows[secilenalan].Cells[8].Value.ToString();
                string renk = dataGridView1.Rows[secilenalan].Cells[9].Value.ToString();

                txtfiyat.Text = fiyat;
                txtkm.Text = km;
                txtruhsat.Text = ruhsatno.ToString();
                cmbkasatipi.Text = kasatipi;
                cmbmarka.Text = marka;
                cmbmodel.Text = model;
                cmbyakittur.Text = yakitturu;
                cmbvites.Text = vitesturu;
                cmbrenk.Text = renk;
                cmbmodyil.Text = modelyili;
            }
            catch (Exception)
            {
                MessageBox.Show("Lütfen listeden seçim yapınız....");                
            }            
        }
        private void button2_Click(object sender, EventArgs e)
        {
                Temizle();
        }
    }
}