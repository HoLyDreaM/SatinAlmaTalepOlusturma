using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using System.Windows.Forms;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Drawing;

public partial class TalepFormu : System.Web.UI.Page
{

    #region Bağlantı Ayarları

    SqlConnection DbConnUser = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnizinAyari = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnDepartmanKodu = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnYetkiKoduBul = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnYetkiTamamla = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnTalepAkisi = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnLink = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnLink"].ToString());
    SqlCommand cmd, cmdMalKodu, cmdMalAdi, cmdEvrakNo, cmdBirimveStokKodu, cmdTalepID,
        cmdTalepAkisi, cmdTalepEkle, cmdEvrakDurum, cmdDepartmanKodu, cmdDepartmanAdi, cmdStoksuzMal, cmdizinDurum;
    SqlDataReader dr;
    int Durum, EvrakNo;
    string Yetkimiz, DepartmanID, DepartmanAdi, DepartmanKodu, KullaniciKodu, KullaniciName,
    AltDepartmanID, AltDepartmanAdi, AltDepartmanKodu, GidecekDepartman, StokluDepartmanAdi, OnayDurumu, KayitYetki,
    StokluDepartmanKodu, StokluDepartmanID, StoksuzDepartmanAdi, StoksuzDepartmanKodu, StoksuzDepartmanID;



    #endregion

    string StokKodu, StokAciklama, SQLSorgu, sonuc, sonuc2, MenuDurumKontrol, MasrafYerimiz, MasrafMerkeziAdimiz, MaxYetki, MinYetki;
    int DepartmanCountDurum, ToplamMasrafMerkeziSayimiz;
    bool KullaniciAnaDepartmani, AnaDepartmanimiz, SatinAlmaButonu;

    #region İnsert Parametreleri

    string TalepEvrakNumarasi, MalKodu, MalAdi, Birim, Kod1, Kod2, Kod3, Kod4, Kod5, Kod6, Kod7, Kod8, Kod9, Kod10, Aciklama, Aciklama2, Kaydeden,
        Degistiren, DegistirenKodu, Departman, KullanilacakDepartman, TalepisleyisSorgu, Aciklama3,
        YetkiUnvani, AdSoyad, OnayDurum, InsertSorgu, Depo, IhtiyacNedeni, TeminYeri, IhtiyacYeri;
    decimal Miktar, OnayMiktar, StokMiktar;
    int Kod11, Kod12, TalepDurum, TarihRakam, KayitTarihRakam, evrakNomuz, EvrakKontrol, TalepID;
    double Tarih, KayitTarih;
    bool izinDurum;

    #endregion

    public static DataTable datadoldur(string Sql)
    {
        SqlConnection conn = new SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DbConnUser"].ToString());

        DataTable dt = new DataTable();
        try
        {
            conn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(Sql, conn);
            adapter.Fill(dt);
        }
        finally
        {
            conn.Close();
        }
        return dt;
    }

    public static DataTable datadoldurLink(string Sql)
    {
        SqlConnection conn = new SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DbConnLink"].ToString());

        DataTable dt = new DataTable();
        try
        {
            conn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(Sql, conn);
            adapter.Fill(dt);
        }
        finally
        {
            conn.Close();
        }
        return dt;
    }

    private string KullaniciKoduDoldur(string KullaniciAdi)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string Sorgumuz = "SELECT KullaniciKodu FROM Kullanicilar WHERE KullaniciAdi='" + KullaniciAdi.ToString() + "'";
        cmd = new SqlCommand(Sorgumuz, DbConnUser);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();
    }

    private string KullaniciAdiDoldur(string KullaniciAdi)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string Sorgumuz = "SELECT AdSoyad FROM Kullanicilar WHERE KullaniciAdi='" + KullaniciAdi.ToString() + "'";
        cmd = new SqlCommand(Sorgumuz, DbConnUser);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["Giris"] != "Evet")
            {
                Response.Redirect("Login.Aspx");
            }
            else
            {
                Session["StokDurum"] = 0;
                Session["Sorgu"] = "";
                DepartmanID = Session["DepartmanID"].ToString();
                DepartmanAdi = Session["DepartmanAdi"].ToString();
                DepartmanKodu = Session["DepartmanKodu"].ToString();

                AltDepartmanID = Session["AltDepartmanID"].ToString();
                AltDepartmanAdi = Session["AltDepartmanAdi"].ToString();
                AltDepartmanKodu = Session["AltDepartmanKodu"].ToString();

                //StokluDepartmanAdi = Session["StokluDepartmanAdi"].ToString();
                //StokluDepartmanKodu = Session["StokluDepartmanKodu"].ToString();
                //StokluDepartmanID = Session["StokluDepartmanID"].ToString();

                //StoksuzDepartmanAdi = Session["StoksuzDepartmanAdi"].ToString();
                //StoksuzDepartmanKodu = Session["StoksuzDepartmanKodu"].ToString();
                //StoksuzDepartmanID = Session["StoksuzDepartmanID"].ToString();

                KullaniciKodu = Session["KullaniciKodu"].ToString();
                KullaniciName = Session["Name"].ToString();

                txtDepartmanKodu.Text = DepartmanKodu;
                txtDepartmanAdi.Text = DepartmanAdi;
                txtDepKodu.Text = txtDepartmanKodu.Text;
                txtDepAdi.Text = txtDepartmanAdi.Text;

                Yetkimiz = Session["Yetki"].ToString();

                SatinAlmaButonu = SatinAlmaButonDurumu(KullaniciKodu);

                txtTalepEdenAdi.Text = KullaniciName;
                txtTalepEdenKodu.Text = KullaniciKodu;
                txtTalepAdi.Text = KullaniciName;
                txtTalepKodu.Text = KullaniciKodu;
                //StoksuzMalKodlari();

                bool masrafMerkeziDurum = Convert.ToBoolean(Session["MasrafMerkeziDurumu"].ToString());
                string MasrafMerkeziKodu = Session["MasrafMerkeziKodu"].ToString();

                if (masrafMerkeziDurum == true)
                {
                    DataSet ds = new DataSet();
                    string sorgu = "SELECT TOP(1) '1' AS MasrafMerkeziKodu,'Seçiniz' AS MasrafMerkeziAdi " +
                                   "UNION ALL " +
                                   "SELECT MasrafMerkeziKodu,MasrafMerkeziAdi FROM MasrafMerkezi";
                    SqlDataAdapter adr;
                    adr = new SqlDataAdapter(sorgu, DbConnUser);
                    adr.Fill(ds, "MasrafMerkezi");

                    ToplamMasrafMerkeziSayimiz = ds.Tables["MasrafMerkezi"].Rows.Count;
                    int MMKodu = Convert.ToInt32(ds.Tables["MasrafMerkezi"].Rows[0]["MasrafMerkeziKodu"].ToString());

                    for (int i = 0; i < ToplamMasrafMerkeziSayimiz; i++)
                    {
                        MasrafMerkeziAdimiz = ds.Tables["MasrafMerkezi"].Rows[i]["MasrafMerkeziAdi"].ToString();
                        MasrafMerkeziKodu = ds.Tables["MasrafMerkezi"].Rows[i]["MasrafMerkeziKodu"].ToString();

                        drpKullDepartman.Items.Add(new ListItem(MasrafMerkeziAdimiz, Convert.ToString(MMKodu)));
                        drpKullDepartman1.Items.Add(new ListItem(MasrafMerkeziAdimiz, Convert.ToString(MMKodu)));
                        MMKodu++;
                    }
                }
                else
                {
                    DataSet ds = new DataSet();
                    string sorgu = "SELECT TOP(1) '1' AS MasrafMerkeziKodu,'Seçiniz' AS MasrafMerkeziAdi " +
                                   "UNION ALL " +
                                   "SELECT MasrafMerkeziKodu,MasrafMerkeziAdi FROM MasrafMerkezi WHERE MasrafMerkeziKodu='" + MasrafMerkeziKodu + "'";
                    SqlDataAdapter adr;
                    adr = new SqlDataAdapter(sorgu, DbConnUser);
                    adr.Fill(ds, "MasrafMerkezi");

                    ToplamMasrafMerkeziSayimiz = ds.Tables["MasrafMerkezi"].Rows.Count;
                    int MMKodu = Convert.ToInt32(ds.Tables["MasrafMerkezi"].Rows[0]["MasrafMerkeziKodu"].ToString());

                    for (int i = 0; i < ToplamMasrafMerkeziSayimiz; i++)
                    {
                        MasrafMerkeziAdimiz = ds.Tables["MasrafMerkezi"].Rows[i]["MasrafMerkeziAdi"].ToString();
                        MasrafMerkeziKodu = ds.Tables["MasrafMerkezi"].Rows[i]["MasrafMerkeziKodu"].ToString();

                        drpKullDepartman.Items.Add(new ListItem(MasrafMerkeziAdimiz, Convert.ToString(MMKodu)));
                        drpKullDepartman1.Items.Add(new ListItem(MasrafMerkeziAdimiz, Convert.ToString(MMKodu)));
                        MMKodu++;
                    }
                }

                //string sorgu3 = "SELECT STK006_Row_ID,STK006_ReferansTuru, STK006_ReferansKodu, STK006_ReferansAciklamasi " +
                //               "FROM STK006 WHERE STK006_ReferansTuru=96";

                //drpihtiyacYeri.DataTextField = "STK006_ReferansAciklamasi";
                //drpihtiyacYeri.DataValueField = "STK006_ReferansKodu";
                //drpihtiyacYeri.DataSource = datadoldurLink(sorgu3);
                //drpihtiyacYeri.DataBind();

                string sorgu3 = "SELECT TOP(1) 1 AS TeminID,'Seçiniz' AS TeminAdi,'Seçiniz' AS TeminKodu FROM TeminYeri " +
                                "UNION ALL " +
                                "SELECT TeminID,TeminAdi,TeminKodu FROM TeminYeri " +
                                "ORDER BY TeminID ";

                drpTeminYeri.DataTextField = "TeminAdi";
                drpTeminYeri.DataValueField = "TeminKodu";
                drpTeminYeri.DataSource = datadoldur(sorgu3);
                drpTeminYeri.DataBind();

                drpTeminYeri1.DataTextField = "TeminAdi";
                drpTeminYeri1.DataValueField = "TeminKodu";
                drpTeminYeri1.DataSource = datadoldur(sorgu3);
                drpTeminYeri1.DataBind();

                //string sorguMasrafYeri = "SELECT MasrafMerkeziKodu , MasrafMerkeziAdi FROM MasrafMerkezi ORDER BY MasrafMerkeziAdi ASC";

                //drpKullDepartman.DataTextField = "MasrafMerkeziAdi";
                //drpKullDepartman.DataValueField = "MasrafMerkeziKodu";
                //drpKullDepartman.DataSource = datadoldur(sorguMasrafYeri);
                //drpKullDepartman.DataBind();

                //drpKullDepartman1.DataTextField = "MasrafMerkeziAdi";
                //drpKullDepartman1.DataValueField = "MasrafMerkeziKodu";
                //drpKullDepartman1.DataSource = datadoldur(sorguMasrafYeri);
                //drpKullDepartman1.DataBind();

                string sorgu4 = "SELECT IhtiyacID, IhtiyacAdi, IhtiyacKodu FROM Ihtiyaclar Order BY IhtiyacKodu ASC";
                drpihtiyacNedeni.DataTextField = "IhtiyacAdi";
                drpihtiyacNedeni.DataValueField = "IhtiyacKodu";
                drpihtiyacNedeni.DataSource = datadoldur(sorgu4);
                drpihtiyacNedeni.DataBind();

                drpIhtNedeni.DataTextField = "IhtiyacAdi";
                drpIhtNedeni.DataValueField = "IhtiyacKodu";
                drpIhtNedeni.DataSource = datadoldur(sorgu4);
                drpIhtNedeni.DataBind();

                dtIhtTarih.Date = DateTime.Now;
                dtTalepTarihi.Date = DateTime.Now;

                DateTime SeriNoTarih = DateTime.Now;
                string SeriNo = Convert.ToString(SeriNoTarih.Year);
                EvrakNoBul();

                if (EvrakNo == 1)
                {

                    SeriNo = SeriNo + Convert.ToString(EvrakNo).PadLeft(6, '0');
                    int SeriNoInt = Convert.ToInt32(SeriNo);
                    evrakNomuz = SeriNoInt + EvrakNo;
                }
                else
                {
                    SeriNo = SeriNo + Convert.ToString(EvrakNo).PadLeft(6, '0');
                    int SeriNoInt = Convert.ToInt32(SeriNo);
                    evrakNomuz = SeriNoInt;
                    SeriNo = Convert.ToString(evrakNomuz);
                }

                txtTalepSeriNo.Text = SeriNo;
                txtIhtTalepSeriNo.Text = txtTalepSeriNo.Text;

                if (Session["StokDurum"].ToString() == "0")
                {
                    AnaEkranPenceresi.ActiveViewIndex = Convert.ToInt32(Session["StokDurum"]);
                }
                else if (Session["StokDurum"].ToString() == "1")
                {
                    AnaEkranPenceresi.ActiveViewIndex = Convert.ToInt32(Session["StokDurum"]);
                }
                else if (Session["StokDurum"].ToString() == "2")
                {
                    AnaEkranPenceresi.ActiveViewIndex = Convert.ToInt32(Session["StokDurum"]);
                }
            }
        }
    }

    private bool SatinAlmaButonDurumu(string KullaniciKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT SatinAlma FROM Kullanicilar " +
                       "WHERE KullaniciKodu='" + KullaniciKodu.ToString() + "'";
        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (bool)cmd.ExecuteScalar();
    }

    protected void grdLookUpMalKodu_ValueChanged(object sender, EventArgs e)
    {
        string MalKodu = drpStokKodlari.SelectedItem.Value.ToString();
        Durum = 1;
        BirimveStokMiktariMalKodu(MalKodu);
        MalKodu = MalAdiBul(MalKodu);
        drpStokAdi.SelectedItem.Value = MalKodu;
    }

    protected void grdLookUpMalAdlari_ValueChanged(object sender, EventArgs e)
    {
        string MalAdi = drpStokAdi.SelectedItem.Value.ToString();
        Durum = 1;
        BirimveStokMiktariMalAdi(MalAdi);
        MalAdi = MalKoduBul(MalAdi);
        drpStokKodlari.SelectedItem.Value = MalAdi;
    }

    private string MalAdiBul(string MalKodu)
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        string sorgu = "SELECT STK004_Aciklama AS Aciklama FROM STK004 " +
                       " WHERE STK004_MalKodu='" + MalKodu.ToString() + "'";
        cmdMalAdi = new SqlCommand(sorgu, DbConnLink);
        cmdMalAdi.CommandTimeout = 120;
        return (string)cmdMalAdi.ExecuteScalar();
    }

    private string MalKoduBul(string MalAdi)
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        string sorgu = "SELECT STK004_MalKodu AS Aciklama FROM  STK004 " +
                    "WHERE STK004_Aciklama='" + MalAdi.ToString() + "'";
        cmdMalKodu = new SqlCommand(sorgu, DbConnLink);
        cmdMalKodu.CommandTimeout = 120;
        return (string)cmdMalKodu.ExecuteScalar();
    }

    private void BirimveStoksuzMiktarMalKodu(string MalKodu)
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        string sorgu = "SELECT STK004_Birim1 AS Birim,CONVERT(NUMERIC(18,3),((STK004_GirisMiktari+STK004_DevirMiktari)-STK004_CikisMiktari)) AS StokMiktari FROM STK004 " +
                     "WHERE STK004_MalKodu='" + MalKodu.ToString() + "'";
        cmdBirimveStokKodu = new SqlCommand(sorgu, DbConnLink);
        cmdBirimveStokKodu.CommandTimeout = 120;
        dr = cmdBirimveStokKodu.ExecuteReader();

        if (dr.Read())
        {
            if (Durum == 1)
            {
                txtStoksuzBirim.Text = dr["Birim"].ToString();
                txtStoksuzMiktar.Text = dr["StokMiktari"].ToString();
                txtStoksuzTalepMiktar.Text = "0";

                if (txtStoksuzMiktar.Text.ToString().Substring(0, 1) == "-")
                {
                    txtStoksuzMiktar.BackColor = Color.Red;
                    txtStoksuzMiktar.ForeColor = Color.White;
                }
                else
                {
                    txtStoksuzMiktar.BackColor = Color.White;
                    txtStoksuzMiktar.ForeColor = Color.Black;
                }
            }
        }

        dr.Dispose();
        dr.Close();
    }

    private void BirimveStoksuzMiktarMalAdi(string MalAdi)
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        string sorgu = "SELECT STK004_Birim1 AS Birim,CONVERT(NUMERIC(18,3),((STK004_GirisMiktari+STK004_DevirMiktari)-STK004_CikisMiktari)) AS StokMiktari FROM STK004 " +
                     "WHERE STK004_Aciklama='" + MalAdi.ToString() + "'";
        cmdBirimveStokKodu = new SqlCommand(sorgu, DbConnLink);
        cmdBirimveStokKodu.CommandTimeout = 120;
        dr = cmdBirimveStokKodu.ExecuteReader();

        if (dr.Read())
        {
            if (Durum == 1)
            {
                txtStoksuzBirim.Text = dr["Birim"].ToString();
                txtStoksuzMiktar.Text = dr["StokMiktari"].ToString();
                txtStoksuzTalepMiktar.Text = "0";

                if (txtStoksuzMiktar.Text.ToString().Substring(0, 1) == "-")
                {
                    txtStoksuzMiktar.BackColor = Color.Red;
                    txtStoksuzMiktar.ForeColor = Color.White;
                }
                else
                {
                    txtStoksuzMiktar.BackColor = Color.White;
                    txtStoksuzMiktar.ForeColor = Color.Black;
                }
            }
        }

        dr.Dispose();
        dr.Close();
    }

    private void BirimveStokMiktariMalKodu(string MalKodu)
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        string sorgu = "SELECT STK004_Birim1 AS Birim,CONVERT(NUMERIC(18,3),((STK004_GirisMiktari+STK004_DevirMiktari)-STK004_CikisMiktari)) AS StokMiktari FROM STK004 " +
                     "WHERE STK004_MalKodu='" + MalKodu.ToString() + "'";
        cmdBirimveStokKodu = new SqlCommand(sorgu, DbConnLink);
        cmdBirimveStokKodu.CommandTimeout = 120;
        dr = cmdBirimveStokKodu.ExecuteReader();

        if (dr.Read())
        {
            if (Durum == 1)
            {
                txtBirim.Text = dr["Birim"].ToString();
                txtStokMiktari.Text = dr["StokMiktari"].ToString();

                if (txtStokMiktari.Text.ToString().Substring(0, 1) == "-")
                {
                    txtStokMiktari.BackColor = Color.Red;
                    txtStokMiktari.ForeColor = Color.White;
                }
                else
                {
                    txtStokMiktari.BackColor = Color.White;
                    txtStokMiktari.ForeColor = Color.Black;
                }
            }
        }

        dr.Dispose();
        dr.Close();
    }

    private void BirimveStokMiktariMalAdi(string MalAdi)
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        string sorgu = "SELECT STK004_Birim1 AS Birim,(STK004_GirisMiktari-STK004_CikisMiktari) AS StokMiktari FROM STK004 " +
                     "WHERE STK004_Aciklama='" + MalAdi.ToString() + "'";
        cmdBirimveStokKodu = new SqlCommand(sorgu, DbConnLink);
        cmdBirimveStokKodu.CommandTimeout = 120;
        dr = cmdBirimveStokKodu.ExecuteReader();

        if (dr.Read())
        {
            if (Durum == 1)
            {
                txtBirim.Text = dr["Birim"].ToString();
                txtStokMiktari.Text = dr["StokMiktari"].ToString();

                if (txtStokMiktari.Text.ToString().Substring(0, 1) == "-")
                {
                    txtStokMiktari.BackColor = Color.Red;
                    txtStokMiktari.ForeColor = Color.White;
                }
                else
                {
                    txtStokMiktari.BackColor = Color.White;
                    txtStokMiktari.ForeColor = Color.Black;
                }
            }
        }

        dr.Dispose();
        dr.Close();
    }

    private void EvrakNoBul()
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        DateTime SeriNoTarih = DateTime.Now;
        int SeriNo = Convert.ToInt32(SeriNoTarih.Year);

        string sorgu = "SELECT (CASE ISNULL(MAX(EvrakNo),0) " +
                       "WHEN 0 THEN 1 ELSE MAX(EvrakNo)+1 END) AS EvrakNo FROM Tlp " +
                       "WHERE EvrakNoTarih='" + Convert.ToString(SeriNo.ToString()) + "'";
        cmdEvrakNo = new SqlCommand(sorgu, DbConnUser);
        cmdEvrakNo.CommandTimeout = 120;
        dr = cmdEvrakNo.ExecuteReader();

        if (dr.Read())
        {
            EvrakNo = Convert.ToInt32(dr["EvrakNo"].ToString());
        }
        dr.Dispose();
        dr.Close();
    }

    protected void btnTalepGonder_Click(object sender, EventArgs e)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        if (DbConnTalepAkisi.State == ConnectionState.Closed)
            DbConnTalepAkisi.Open();

        EvrakKontrol = EvrakNoDurum(txtTalepSeriNo.Text.ToString().Substring(4, 6));

        if (EvrakKontrol > 0)
        {
            int tlpNo = Convert.ToInt32(txtTalepSeriNo.Text);
            tlpNo = tlpNo + 1;
            TalepEvrakNumarasi = Convert.ToString(tlpNo).Substring(4, 6);
        }
        else
        {
            TalepEvrakNumarasi = txtTalepSeriNo.Text;
            TalepEvrakNumarasi = TalepEvrakNumarasi.ToString().Substring(4, 6);
        }

        TeminYeri = drpTeminYeri1.SelectedValue.ToString();

        string KullDept = drpKullDepartman1.SelectedItem.Text.ToString();

        if (TeminYeri == "Seçiniz")
        {
            Alert.Show("Lütfen Temin Yerini Seçiniz");
            return;
        }
        if (KullDept == "Seçiniz")
        {
            Alert.Show("Lütfen Kullanılacak Yeri Seçiniz");
            return;
        }

        Aciklama2 = "";
        Departman = txtDepartmanKodu.Text;
        YetkiUnvani = Session["Yetki"].ToString();
        AnaDepartmanimiz = Convert.ToBoolean(Session["AnaDepartman"].ToString());

        DepartmanID = Session["DepartmanID"].ToString();
        DepartmanAdi = Session["DepartmanAdi"].ToString();
        DepartmanKodu = Session["DepartmanKodu"].ToString();

        AltDepartmanID = Session["AltDepartmanID"].ToString();
        AltDepartmanAdi = Session["AltDepartmanAdi"].ToString();
        AltDepartmanKodu = Session["AltDepartmanKodu"].ToString();
        string BagliOlduguDepartman = Session["BagliOlduguBirim"].ToString();

        KullaniciKodu = Session["KullaniciKodu"].ToString();
        KullaniciName = Session["Name"].ToString();
        Yetkimiz = Session["Yetki"].ToString();

        DateTime obj = new DateTime();
        obj = Convert.ToDateTime(dtTalepTarihi.Date);
        string str;
        Tarih = 0;
        Tarih = obj.ToOADate();
        str = Tarih.ToString().Substring(0, 5);
        TarihRakam = Convert.ToInt32(str);

        DateTime obj2 = new DateTime();
        obj2 = DateTime.Now;
        //string obs = obj2.ToString("dd-MM-yyyy");
        //obj2 = Convert.ToDateTime(obj2);
        string str2;
        KayitTarih = 0;
        KayitTarih = obj2.ToOADate();
        str2 = KayitTarih.ToString().Substring(0, 5);
        KayitTarihRakam = Convert.ToInt32(str2);

        IhtiyacNedeni = drpihtiyacNedeni.SelectedValue.ToString();

        string KulDeptVal = drpKullDepartman1.SelectedItem.Text.ToString();
        string KulDepValint = MasrafMerkeziKoduBul(KulDeptVal);

        KullanilacakDepartman = KulDepValint;

        //IhtiyacYeri = drpihtiyacYeri.SelectedValue.ToString();
        DateTime SeriNoTarih = DateTime.Now;
        string SeriNo = Convert.ToString(SeriNoTarih.Year);
        string TalepAkisDepartmanKodu = DepartmanKodu;
        KullaniciAnaDepartmani = AnaDepartmanKontrolEdiyoruz(KullaniciKodu);
        string TalepBirimDurum = drpStokluBirim.SelectedItem.Value.ToString();

        if (TalepBirimDurum == "Seçiniz")
        {
            TalepBirimDurum = "";
        }

        string GitDepBul = "";

        //DateTime Saat = new DateTime();
        string Saat = DateTime.Now.ToString("hh:mm:ss");


        DateTime SeriNoTarih1 = DateTime.Now;
        string TalepTarihi = Convert.ToString(SeriNoTarih.Year);
        EvrakNoBul();

        string TalepEvrakNumaramiz = "";
        int SeriNoInt2 = 0;

        if (EvrakNo == 1)
        {

            TalepEvrakNumaramiz = Convert.ToString(EvrakNo).PadLeft(6, '0');
            SeriNoInt2 = Convert.ToInt32(TalepTarihi);
        }
        else
        {
            TalepEvrakNumaramiz = Convert.ToString(EvrakNo).PadLeft(6, '0');
            SeriNoInt2 = Convert.ToInt32(TalepTarihi);
        }

        if (!string.IsNullOrEmpty(drpStokKodlari.SelectedItem.Value.ToString()))
        {
            MalKodu = drpStokKodlari.SelectedItem.Value.ToString();
            MalAdi = MalAdiniAliyoruz(MalKodu);
            Birim = txtBirim.Text;
            Miktar = Convert.ToDecimal(txtMiktar.Text.Replace(',', '.'));
            Aciklama = txtAciklama.Text;
            Aciklama2 = txtAciklama2.Text;
            Aciklama3 = txtAciklama3.Text;
            StokluDepartmanKodu = "";

            StokMiktar = Convert.ToDecimal(txtStokMiktari.Text);
            GitDepBul = GidecekDepartmanBul(MalKodu);

            string izinayari = "";
            MasrafYerimiz = drpKullDepartman1.SelectedItem.Text.ToString();

            if (BagliOlduguDepartman == "0")
            {
                YetkiKoduBul(DepartmanKodu, KullaniciKodu.ToString(), Yetkimiz, AltDepartmanID);
            }
            else
            {
                YetkiKoduBul(DepartmanKodu, KullaniciKodu.ToString(), Yetkimiz, BagliOlduguDepartman);
            }

            if (Convert.ToInt32(KayitYetki) == 1 || Convert.ToInt32(KayitYetki) == 2 || Convert.ToInt32(KayitYetki) == 3)
            {
                if (BagliOlduguDepartman == "0")
                {
                    izinayari = izinDurumu(Session["DepartmanKodu"].ToString(), Session["KullaniciKodu"].ToString(), Yetkimiz, Session["AltDepartmanID"].ToString());
                }
                else
                {
                    izinayari = izinDurumu(Session["DepartmanKodu"].ToString(), Session["KullaniciKodu"].ToString(), Yetkimiz, BagliOlduguDepartman);

                }
            }
            else
            {
                izinayari = "İzinde Değil";
            }

            if (izinayari == "İzne Çıkmış")
            {
                if (BagliOlduguDepartman == "0")
                {
                    YetkiKoduBul(DepartmanKodu, KullaniciKodu.ToString(), KayitYetki, AltDepartmanID);
                }
                else
                {
                    YetkiKoduBul(DepartmanKodu, KullaniciKodu.ToString(), KayitYetki, BagliOlduguDepartman);
                }

                string DurumKullaniciAdi = KullaniciAdiBul(DepartmanKodu, Convert.ToInt32(KayitYetki));

                if (Convert.ToInt32(KayitYetki) == 0)
                {
                    TalepAkisDepartmanKodu = StokluDepartmanKodu.ToString();
                    OnayDurumu = "3";

                    KayitYetki = "4";
                    DepartmanCountDurum = GidecekDeptDurum(GidecekDepartmanBul(MalKodu));

                    if (GitDepBul == "HAMMADDE" || GitDepBul == "YARDMADDE")
                    {
                        StokluDepartmanKodu = HammaddeDepartmanKodu(GitDepBul);
                    }
                    else
                    {
                        StokluDepartmanKodu = AnaDepartmanKodu();
                    }
                }
                else
                {
                    int YeniYetki = Convert.ToInt32(KayitYetki) - Convert.ToInt32(Yetkimiz);
                    string YenYetki = "";
                    string UnvanimiziAliyoruz = "";

                    if (YeniYetki == 2)
                    {
                        YenYetki = Convert.ToString(YeniYetki);
                    }
                    else
                    {
                        UnvanimiziAliyoruz = YetkiUnvanimiz(Convert.ToInt32(Yetkimiz));

                        if (UnvanimiziAliyoruz == "Memur")
                        {
                            YenYetki = "1";
                        }
                        else
                        {
                            YenYetki = Yetkimiz;
                        }
                    }
                    OnayDurumu = OnayDurumBelirliyoruz(Convert.ToInt32(YenYetki));
                }
            }
            else if (izinayari == "İzinde Değil")
            {
                if (BagliOlduguDepartman == "0")
                {
                    YetkiKoduBul(DepartmanKodu, KullaniciKodu.ToString(), Yetkimiz, AltDepartmanID);
                }
                else
                {
                    YetkiKoduBul(DepartmanKodu, KullaniciKodu.ToString(), Yetkimiz, BagliOlduguDepartman);

                }

                if (Convert.ToInt32(KayitYetki) == 0)
                {
                    OnayDurumu = "3";
                    DepartmanCountDurum = GidecekDeptDurum(GidecekDepartmanBul(MalKodu));
                    KayitYetki = "4";

                    if (GitDepBul == "HAMMADDE" || GitDepBul == "YARDMADDE")
                    {
                        StokluDepartmanKodu = HammaddeDepartmanKodu(GitDepBul);
                    }
                    else
                    {
                        StokluDepartmanKodu = AnaDepartmanKodu();
                    }
                }
                else
                {
                    int YeniYetki = Convert.ToInt32(KayitYetki) - Convert.ToInt32(Yetkimiz);
                    string YenYetki = "";
                    string UnvanimiziAliyoruz = "";

                    if (YeniYetki == 2)
                    {
                        YenYetki = Convert.ToString(YeniYetki);
                    }
                    else
                    {
                        UnvanimiziAliyoruz = YetkiUnvanimiz(Convert.ToInt32(Yetkimiz));

                        if (UnvanimiziAliyoruz == "Memur")
                        {
                            YenYetki = "1";
                        }
                        else
                        {
                            YenYetki = Yetkimiz;
                        }
                    }

                    OnayDurumu = OnayDurumBelirliyoruz(Convert.ToInt32(YenYetki));
                }
            }

            if (KullaniciAnaDepartmani == true)
            {
                StokluDepartmanKodu = TeminYeri;
                KayitYetki = "5";
                OnayDurumu = "5";
            }

            if (SatinAlmaButonu == true)
            {
                StokluDepartmanKodu = SatinAlmaGidecekDepartmanBul(MalKodu);
                OnayDurumu = "5";
                KayitYetki = "5";
            }

            InsertSorgu = "INSERT INTO Tlp(EvrakNoTarih, EvrakNo, Tarih, Saat, MalKodu, MalAdi, Birim, Miktar, " +
                            "Aciklama, Aciklama2, Aciklama3, Kaydeden, AdSoyad, DepartmanID, DepartmanAdi, DepartmanKodu, " +
                            "AltDepartmanID, AltDepartmanAdi, AltDepartmanKodu, KayitTarihi, SiparisNo, " +
                            "SiparisDurum, GidecekDepartman, KullanilacakDepartman, MasrafMerkeziAdi, TeminYeri, TalepBirim, ihtiyacNedeni, Onaylayan, OnayMiktar, " +
                            "OnayDurumu, TalepKapatan, TalepDurum, ilkIslem,CikisHareketNo,CikisHareketDepo,SipMiktar,IrsaliyeDurum,idariKisim, " +
                            "TeknikKisim, ihtiyacPusulasi, StokCikisHareketi, StokCikisFormu, Fax, SiparisFormu, RedNedeni,DegisiklikNedeni,Yetki,OnaylayanYetki ) " +
                            "VALUES ('" + Convert.ToString(SeriNoInt2) + "','" + TalepEvrakNumaramiz.ToString() + "'," + TarihRakam + ", '" + Saat.ToString() + "' " +
                            ",'" + MalKodu.ToString() + "','" + MalAdi.ToString().Replace("'", "''") + "','" + Birim.ToString() + "' " +
                            ",'" + Miktar.ToString() + "','" + Aciklama.ToString().Replace("'", "''") + "','" + Aciklama2.ToString().Replace("'", "''") + "', '" + Aciklama3.ToString().Replace("'", "''") + "', " +
                            "'" + KullaniciKodu.ToString() + "','" + KullaniciName.ToString() + "', " + Convert.ToInt32(DepartmanID.ToString()) + "," +
                            "'" + DepartmanAdi.ToString() + "','" + DepartmanKodu.ToString() + "'," + Convert.ToInt32(AltDepartmanID.ToString()) + ",'" + AltDepartmanAdi.ToString() + "', " +
                            "'" + AltDepartmanKodu.ToString() + "'," + KayitTarihRakam + ",N'', 0,'" + StokluDepartmanKodu.ToString() + "', " +
                            "'" + KullanilacakDepartman.ToString() + "','" + MasrafYerimiz.ToString() + "', '" + TeminYeri.ToString() + "','" + TalepBirimDurum + "','" + IhtiyacNedeni.ToString() + "', " +
                            "'" + KullaniciKodu.ToString() + "',0,'" + OnayDurumu.ToString() + "','" + KullaniciKodu.ToString() + "', 0, 0, " +
                            "N'',N'',0,0,0,0,0,0,0,1,0,N'',N''," + Yetkimiz + "," + Convert.ToInt32(KayitYetki) + ")";

            cmdTalepEkle = new SqlCommand(InsertSorgu, DbConnUser);
            cmdTalepEkle.ExecuteNonQuery();

            //TalepID = TalepIDBul();

            ////DepartmanKoduBul(Session["KullAdi"].ToString());

            //TalepisleyisSorgu = "INSERT INTO TalepAkisi(TalepID, OnaylayanYetki, EkleyenYetki, TalepDurumu, Departman, Onay, OnayTarih) " +
            //                     "VALUES (" + TalepID + "," + Convert.ToInt32(KayitYetki) + ", " + Convert.ToInt32(Yetkimiz) + ", " +
            //                     "'" + TalepAkisDepartmanKodu.ToString() + "','" + DepartmanKodu.ToString() + "', 'False', GETDATE())";
            //cmdTalepAkisi = new SqlCommand(TalepisleyisSorgu, DbConnTalepAkisi);
            //cmdTalepAkisi.ExecuteNonQuery();
        }

        EvrakNoBul();

        if (EvrakNo == 1)
        {

            SeriNo = SeriNo + Convert.ToString(EvrakNo).PadLeft(6, '0');
            int SeriNoInt = Convert.ToInt32(SeriNo);
            evrakNomuz = SeriNoInt + EvrakNo;
        }
        else
        {
            evrakNomuz = EvrakNo;
        }

        txtTalepSeriNo.Text = SeriNo + Convert.ToString(evrakNomuz).PadLeft(6, '0');
        txtIhtTalepSeriNo.Text = txtTalepSeriNo.Text;

        Session["StokDurum"] = "0";
        AnaEkranPenceresi.ActiveViewIndex = 0;
        rdTalepDurumu.ClearSelection();

        #region Temizlik

        txtAciklama.Text = null;
        txtAciklama2.Text = null;
        txtAciklama3.Text = null;
        txtBirim.Text = null;

        txtStokMiktari.Text = null;
        txtStokMiktari.BackColor = Color.White;

        txtMiktar.Text = null;

        //grdLookUpMalKodu.Value = null;
        //grdLookUpMalKodu.Text = null;
        //grdLookUpMalAdlari.Text = null;
        //grdLookUpMalAdlari.Value = null;

        #endregion

        ConnectionKapat();

        Alert.Show("İhtiyaç Talebiniz Eklenmiştir.");
    }

    private int EvrakNoDurum(string EvrakDurum)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        DateTime SeriNoTarih = DateTime.Now;
        string SeriNo = Convert.ToString(SeriNoTarih.Year);

        string sorgu = "SELECT COUNT(EvrakNo) AS Durum FROM Tlp " +
                       "WHERE EvrakNo='" + EvrakDurum.ToString() + "' AND EvrakNoTarih=" + Convert.ToInt32(SeriNo) + "";
        cmdEvrakDurum = new SqlCommand(sorgu, DbConnUser);
        cmdEvrakDurum.CommandTimeout = 120;
        return (int)cmdEvrakDurum.ExecuteScalar();
    }

    protected void rdTalepDurumu_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdTalepDurumu.SelectedIndex == 0)
        {
            Session["StokDurum"] = "2";

            MenuDurumKontrol = Session["MenuKodu"].ToString();

            if (MenuDurumKontrol == "Ambar")
            {
                StoksuzBirimDurumu.Visible = true;
                StokluBirimDurumu.Visible = true;
            }
            else
            {
                StoksuzBirimDurumu.Visible = false;
                StokluBirimDurumu.Visible = false;
            }

            AnaEkranPenceresi.ActiveViewIndex = Convert.ToInt32(Session["StokDurum"]);
        }
        if (rdTalepDurumu.SelectedIndex == 1)
        {
            Session["StokDurum"] = "1";

            if (MenuDurumKontrol == "Ambar")
            {
                StoksuzBirimDurumu.Visible = true;
                StokluBirimDurumu.Visible = true;
            }
            else
            {
                StoksuzBirimDurumu.Visible = false;
                StokluBirimDurumu.Visible = false;
            }

            AnaEkranPenceresi.ActiveViewIndex = Convert.ToInt32(Session["StokDurum"]);
        }
    }

    protected void btnStoksuzTalepGonder_Click(object sender, EventArgs e)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        EvrakKontrol = EvrakNoDurum(txtTalepSeriNo.Text.ToString().Substring(4, 6));

        if (EvrakKontrol > 0)
        {
            int tlpNo = Convert.ToInt32(txtTalepSeriNo.Text);
            tlpNo = tlpNo + 1;
            TalepEvrakNumarasi = Convert.ToString(tlpNo).Substring(4, 6);
        }
        else
        {
            TalepEvrakNumarasi = txtTalepSeriNo.Text;
            TalepEvrakNumarasi = TalepEvrakNumarasi.ToString().Substring(4, 6);
        }

        TeminYeri = drpTeminYeri.SelectedValue.ToString();
        string KullDept = drpKullDepartman.SelectedItem.Text.ToString();

        if (TeminYeri == "Seçiniz")
        {
            Alert.Show("Lütfen Temin Yerini Seçiniz");
            return;
        }
        if (KullDept == "Seçiniz")
        {
            Alert.Show("Lütfen Kullanılacak Yeri Seçiniz");
            return;
        }

        Aciklama2 = "";

        Departman = txtDepartmanKodu.Text;
        YetkiUnvani = Session["Yetki"].ToString();

        DepartmanID = Session["DepartmanID"].ToString();
        DepartmanAdi = Session["DepartmanAdi"].ToString();
        DepartmanKodu = Session["DepartmanKodu"].ToString();

        AltDepartmanID = Session["AltDepartmanID"].ToString();
        AltDepartmanAdi = Session["AltDepartmanAdi"].ToString();
        AltDepartmanKodu = Session["AltDepartmanKodu"].ToString();
        string BagliOlduguDepartman = Session["BagliOlduguBirim"].ToString();

        KullaniciKodu = Session["KullaniciKodu"].ToString();
        KullaniciName = Session["Name"].ToString();
        Yetkimiz = Session["Yetki"].ToString();
        //Depo = Session["Depo"].ToString();

        DateTime obj = new DateTime();
        obj = Convert.ToDateTime(dtTalepTarihi.Date);
        string str;
        Tarih = 0;
        Tarih = obj.ToOADate();
        str = Tarih.ToString().Substring(0, 5);
        TarihRakam = Convert.ToInt32(str);

        DateTime obj2 = new DateTime();
        obj2 = Convert.ToDateTime(DateTime.Now);
        //string obs = obj2.ToString("dd-MM-yyyy");
        obj2 = Convert.ToDateTime(obj2);
        string str2;
        KayitTarih = 0;
        KayitTarih = obj2.ToOADate();
        str2 = KayitTarih.ToString().Substring(0, 5);
        KayitTarihRakam = Convert.ToInt32(str2);

        IhtiyacNedeni = drpIhtNedeni.SelectedValue.ToString();

        string KulDeptVal = drpKullDepartman.SelectedItem.Text.ToString();
        string KulDepValint = MasrafMerkeziKoduBul(KulDeptVal);

        KullanilacakDepartman = KulDepValint;

        string GitDepBul = "";
        //IhtiyacYeri = drpihtiyacYeri.SelectedValue.ToString();
        DateTime SeriNoTarih = DateTime.Now;
        string SeriNo = Convert.ToString(SeriNoTarih.Year);
        MalKodu = drpMasrafKodlari.SelectedItem.Value.ToString();
        MalAdi = MalAdiniAliyoruz(MalKodu);
        Birim = txtStoksuzBirim.Text;
        KullaniciAnaDepartmani = AnaDepartmanKontrolEdiyoruz(KullaniciKodu);
        string TalepAkisDepartmanKodu = DepartmanKodu;
        Miktar = Convert.ToDecimal(txtStoksuzTalepMiktar.Text.Replace(',', '.'));
        Aciklama = txtStoksuzAciklama.Text;
        Aciklama2 = txtStoksuzAciklama2.Text;
        Aciklama3 = txtStoksuzAciklama3.Text;
        string TalepBirimDurum = drpStoksuzBirim.SelectedItem.Value.ToString();

        if (TalepBirimDurum == "Seçiniz")
        {
            TalepBirimDurum = "";
        }

        StoksuzDepartmanKodu = AnaDepartmanKodu();
        StokluDepartmanKodu = StoksuzDepartmanKodu;

        string Saat = DateTime.Now.ToString("hh:mm:ss");

        GitDepBul = GidecekDepartmanBul(MalKodu);
        

        DateTime SeriNoTarih1 = DateTime.Now;
        string TalepTarihi = Convert.ToString(SeriNoTarih.Year);
        EvrakNoBul();

        string TalepEvrakNumaramiz = "";
        int SeriNoInt2 = 0;

        if (EvrakNo == 1)
        {

            TalepEvrakNumaramiz = Convert.ToString(EvrakNo).PadLeft(6, '0');
            SeriNoInt2 = Convert.ToInt32(TalepTarihi);
        }
        else
        {
            TalepEvrakNumaramiz = Convert.ToString(EvrakNo).PadLeft(6, '0');
            SeriNoInt2 = Convert.ToInt32(TalepTarihi);
        }

        MasrafYerimiz = drpKullDepartman.SelectedItem.Text.ToString();

        string izinayari = "";

        if (BagliOlduguDepartman == "0")
        {
            YetkiKoduBul(DepartmanKodu, KullaniciKodu.ToString(), Yetkimiz, AltDepartmanID);
        }
        else
        {
            YetkiKoduBul(DepartmanKodu, KullaniciKodu.ToString(), Yetkimiz, BagliOlduguDepartman);
        }

        if (Convert.ToInt32(KayitYetki) == 1 || Convert.ToInt32(KayitYetki) == 2 || Convert.ToInt32(KayitYetki) == 3)
        {
            if (BagliOlduguDepartman == "0")
            {
                izinayari = izinDurumu(Session["DepartmanKodu"].ToString(), Session["KullaniciKodu"].ToString(), Yetkimiz, Session["AltDepartmanID"].ToString());
            }
            else
            {
                izinayari = izinDurumu(Session["DepartmanKodu"].ToString(), Session["KullaniciKodu"].ToString(), Yetkimiz, BagliOlduguDepartman);

            }
        }
        else
        {
            izinayari = "İzinde Değil";
        }

        if (izinayari == "İzne Çıkmış")
        {
            if (BagliOlduguDepartman == "0")
            {
                YetkiKoduBul(DepartmanKodu, KullaniciKodu.ToString(), KayitYetki, AltDepartmanID);
            }
            else
            {
                YetkiKoduBul(DepartmanKodu, KullaniciKodu.ToString(), KayitYetki, BagliOlduguDepartman);
            }

            string DurumKullaniciAdi = KullaniciAdiBul(DepartmanKodu, Convert.ToInt32(KayitYetki));

            if (Convert.ToInt32(KayitYetki) == 0)
            {
                TalepAkisDepartmanKodu = StokluDepartmanKodu.ToString();
                OnayDurumu = "3";

                KayitYetki = "4";
                DepartmanCountDurum = GidecekDeptDurum(GidecekDepartmanBul(MalKodu));

                if (GitDepBul == "HAMMADDE" || GitDepBul == "YARDMADDE")
                {
                    StokluDepartmanKodu = HammaddeDepartmanKodu(GitDepBul);
                }
                else
                {
                    StokluDepartmanKodu = AnaDepartmanKodu();
                }
            }
            else
            {
                int YeniYetki = Convert.ToInt32(KayitYetki) - Convert.ToInt32(Yetkimiz);
                string YenYetki = "";
                string UnvanimiziAliyoruz = "";

                if (YeniYetki == 2)
                {
                    YenYetki = Convert.ToString(YeniYetki);
                }
                else
                {
                    UnvanimiziAliyoruz = YetkiUnvanimiz(Convert.ToInt32(Yetkimiz));

                    if (UnvanimiziAliyoruz == "Memur")
                    {
                        YenYetki = "1";
                    }
                    else
                    {
                        YenYetki = Yetkimiz;
                    }
                }
                OnayDurumu = OnayDurumBelirliyoruz(Convert.ToInt32(YenYetki));
            }
        }
        else if (izinayari == "İzinde Değil")
        {
            if (BagliOlduguDepartman == "0")
            {
                YetkiKoduBul(DepartmanKodu, KullaniciKodu.ToString(), Yetkimiz, AltDepartmanID);
            }
            else
            {
                YetkiKoduBul(DepartmanKodu, KullaniciKodu.ToString(), Yetkimiz, BagliOlduguDepartman);

            }

            if (Convert.ToInt32(KayitYetki) == 0)
            {
                OnayDurumu = "3";
                DepartmanCountDurum = GidecekDeptDurum(GidecekDepartmanBul(MalKodu));
                KayitYetki = "4";

                if (GitDepBul == "HAMMADDE" || GitDepBul == "YARDMADDE")
                {
                    StokluDepartmanKodu = HammaddeDepartmanKodu(GitDepBul);
                }
                else
                {
                    StokluDepartmanKodu = AnaDepartmanKodu();
                }
            }
            else
            {
                int YeniYetki = Convert.ToInt32(KayitYetki) - Convert.ToInt32(Yetkimiz);
                string YenYetki = "";
                string UnvanimiziAliyoruz = "";

                if (YeniYetki == 2)
                {
                    YenYetki = Convert.ToString(YeniYetki);
                }
                else
                {
                    UnvanimiziAliyoruz = YetkiUnvanimiz(Convert.ToInt32(Yetkimiz));

                    if (UnvanimiziAliyoruz == "Memur")
                    {
                        YenYetki = "1";
                    }
                    else
                    {
                        YenYetki = Yetkimiz;
                    }
                }

                OnayDurumu = OnayDurumBelirliyoruz(Convert.ToInt32(YenYetki));
            }
        }

        if (KullaniciAnaDepartmani == true)
        {
            StoksuzDepartmanKodu = TeminYeri;
            KayitYetki = "5";
            OnayDurumu = "5";
        }

        InsertSorgu = "INSERT INTO Tlp(EvrakNoTarih, EvrakNo, Tarih, Saat, MalKodu, MalAdi, Birim, Miktar, " +
                    "Aciklama, Aciklama2, Aciklama3, Kaydeden, AdSoyad, DepartmanID, DepartmanAdi, DepartmanKodu, " +
                    "AltDepartmanID, AltDepartmanAdi, AltDepartmanKodu, KayitTarihi, SiparisNo, " +
                    "SiparisDurum, GidecekDepartman, KullanilacakDepartman, MasrafMerkeziAdi, TeminYeri, TalepBirim, ihtiyacNedeni, Onaylayan, OnayMiktar, " +
                    "OnayDurumu, TalepKapatan, TalepDurum, ilkIslem,CikisHareketNo,CikisHareketDepo,SipMiktar,IrsaliyeDurum,idariKisim, " +
                    "TeknikKisim, ihtiyacPusulasi, StokCikisHareketi, StokCikisFormu, Fax, SiparisFormu, RedNedeni,DegisiklikNedeni,Yetki,OnaylayanYetki ) " +
                    "VALUES ('" + Convert.ToString(SeriNoInt2) + "','" + TalepEvrakNumaramiz.ToString() + "'," + TarihRakam + ", '" + Saat.ToString() + "' " +
                    ",'" + MalKodu.ToString() + "','" + MalAdi.ToString().Replace("'", "''") + "','" + Birim.ToString() + "' " +
                    ",'" + Miktar.ToString() + "','" + Aciklama.ToString().Replace("'", "''") + "','" + Aciklama2.ToString().Replace("'", "''") + "', '" + Aciklama3.ToString().Replace("'", "''") + "', " +
                    "'" + KullaniciKodu.ToString() + "','" + KullaniciName.ToString() + "', " + Convert.ToInt32(DepartmanID.ToString()) + "," +
                    "'" + DepartmanAdi.ToString() + "','" + DepartmanKodu.ToString() + "'," + Convert.ToInt32(AltDepartmanID.ToString()) + ",'" + AltDepartmanAdi.ToString() + "', " +
                    "'" + AltDepartmanKodu.ToString() + "'," + KayitTarihRakam + ",N'', 0,'" + StoksuzDepartmanKodu.ToString() + "', " +
                    "'" + KullanilacakDepartman.ToString() + "','" + MasrafYerimiz.ToString() + "', '" + TeminYeri.ToString() + "','" + TalepBirimDurum.ToString() + "','" + IhtiyacNedeni.ToString() + "', " +
                    "'" + KullaniciKodu.ToString() + "',0,'" + OnayDurumu.ToString() + "','" + KullaniciKodu.ToString() + "', 0, 0, " +
                    "N'',N'',0,0,0,0,0,0,0,1,0,N'',N''," + Yetkimiz + "," + Convert.ToInt32(KayitYetki) + ")";

        cmdTalepEkle = new SqlCommand(InsertSorgu, DbConnUser);
        cmdTalepEkle.ExecuteNonQuery();

        EvrakNoBul();

        if (EvrakNo == 1)
        {

            SeriNo = SeriNo + Convert.ToString(EvrakNo).PadLeft(6, '0');
            int SeriNoInt = Convert.ToInt32(SeriNo);
            evrakNomuz = SeriNoInt + EvrakNo;
        }
        else
        {
            evrakNomuz = EvrakNo;
        }

        txtTalepSeriNo.Text = SeriNo + Convert.ToString(evrakNomuz).PadLeft(6, '0');
        txtIhtTalepSeriNo.Text = txtTalepSeriNo.Text;

        Session["StokDurum"] = "0";
        AnaEkranPenceresi.ActiveViewIndex = 0;
        rdTalepDurumu.ClearSelection();

        txtStoksuzAciklama.Text = null;

        ConnectionKapat();

        Alert.Show("Stoksuz İhtiyaç Talebiniz Eklenmiştir.");
    }

    private int TalepIDBul()
    {
        if (DbConnUser.State == ConnectionState.Connecting)
            DbConnUser.Open();

        string sorgu = "SELECT  dbo.fn_TalepID() AS TalepID";
        cmdTalepID = new SqlCommand(sorgu, DbConnUser);
        cmdTalepID.CommandTimeout = 120;
        return (int)cmdTalepID.ExecuteScalar();
    }

    private void DepartmanKoduBul(string KullaniciKodu)
    {
        if (DbConnDepartmanKodu.State == ConnectionState.Closed)
            DbConnDepartmanKodu.Open();

        string sorgu = "SELECT Departman,Yetki FROM Kullanicilar " +
                       "WHERE KullaniciAdi='" + KullaniciKodu + "'";
        cmd = new SqlCommand(sorgu, DbConnDepartmanKodu);
        cmd.CommandTimeout = 120;
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        if (dr.Read())
        {
            //DepartmanKodumuz = dr["Departman"].ToString();
            Yetkimiz = dr["Yetki"].ToString();
        }

        dr.Dispose();
        dr.Close();
    }

    private void YetkiKoduBul(string DepartmanKodu, string KullaniciKodu, string YetkiKodumuz, string AltDepartmanKodu)
    {
        if (DbConnYetkiKoduBul.State == ConnectionState.Closed)
            DbConnYetkiKoduBul.Open();

        string sorgu = "SELECT ISNULL(MIN(Yetki),0) AS Yetki FROM Kullanicilar AS K " +
                        "INNER JOIN Departmanlar AS D ON K.DepartmanID=D.DepartmanID " +
                        "WHERE D.DepartmanKodu='" + DepartmanKodu.ToString() + "'  AND (AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " OR AltDepartmanID=0) " +
                        "AND KullaniciAdi <> '" + KullaniciKodu.ToString() + "' " +
                        "AND Yetki > " + Convert.ToInt32(YetkiKodumuz) + "  AND K.Durum=1 " +
                        "AND (MenuKodu<>'Satınalma' AND MenuKodu<>'Ambar') AND K.KullaniciAdi<>'Admin' ";

        cmd = new SqlCommand(sorgu, DbConnYetkiKoduBul);
        cmd.CommandTimeout = 120;
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        if (dr.Read())
        {
            KayitYetki = dr["Yetki"].ToString();
        }

        dr.Dispose();
        dr.Close();
    }

    private void YetkiKoduBulMax(string DepartmanKodu, string KullaniciKodu, string YetkiKodumuz)
    {
        if (DbConnYetkiKoduBul.State == ConnectionState.Closed)
            DbConnYetkiKoduBul.Open();

        string sorgu = "SELECT ISNULL(MAX(Yetki),0) AS Yetki FROM Kullanicilar AS K " +
                        "INNER JOIN Departmanlar AS D ON K.DepartmanID=D.DepartmanID " +
                        "WHERE D.DepartmanKodu='" + DepartmanKodu.ToString() + "' " +
                        "AND KullaniciAdi <> '" + KullaniciKodu.ToString() + "' " +
                        "AND Yetki > " + Convert.ToInt32(YetkiKodumuz) + "  AND K.Durum=1 " +
                        "AND (MenuKodu<>'Satınalma' AND MenuKodu<>'Ambar') AND K.KullaniciAdi<>'Admin' ";

        cmd = new SqlCommand(sorgu, DbConnYetkiKoduBul);
        cmd.CommandTimeout = 120;
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        if (dr.Read())
        {
            KayitYetki = dr["Yetki"].ToString();
            MaxYetki = dr["Yetki"].ToString();
        }

        dr.Dispose();
        dr.Close();
    }

    private void YetkiTamamla(string Departman)
    {
        if (DbConnYetkiTamamla.State == ConnectionState.Closed)
            DbConnYetkiTamamla.Open();

        string sorgu = "SELECT ISNULL(MIN(Yetki),0) AS Yetki FROM Kullanicilar AS K " +
                    "INNER JOIN Departmanlar AS D ON K.DepartmanID=D.DepartmanID " +
                    "WHERE D.DepartmanKodu ='" + Departman.ToString() + "'";

        cmd = new SqlCommand(sorgu, DbConnYetkiTamamla);
        cmd.CommandTimeout = 120;
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        if (dr.Read())
        {
            KayitYetki = dr["Yetki"].ToString();
        }

        dr.Dispose();
        dr.Close();
    }

    private string OnayDurumKontrol(int YetkiKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string OnaySorgu = "SELECT Yetki_Unvani+' Onayı Bekliyor' AS OnayDurum FROM Yetki_Unvanlari " +
                        "WHERE YetkiKodu=" + YetkiKodu + "";

        cmd = new SqlCommand(OnaySorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();
    }

    private string DepartmanOnayKontrol(string DepartmanKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string DepartmanSorgu = "SELECT DepartmanAdi+' Onayı Bekliyor' AS OnayDurum FROM Departmanlar " +
                            "WHERE DepartmanKodu='" + DepartmanKodu.ToString() + "'";
        cmd = new SqlCommand(DepartmanSorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();
    }

    private string KullaniciAdiBul(string DepartmanKodu, int Yetki)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT K.KullaniciAdi FROM Kullanicilar AS K " +
                        "INNER JOIN Departmanlar AS D ON K.DepartmanID=D.DepartmanID " +
                        "WHERE D.DepartmanKodu='" + DepartmanKodu.ToString() + "'" +
                        "AND Yetki = " + Yetki + "";

        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();

    }

    //private bool izinDurum(string KullaniciAdi)
    //{
    //    if (DbConnUser.State == ConnectionState.Closed)
    //        DbConnUser.Open();

    //    string sorgu = "SELECT (CASE " +
    //                "WHEN izin_BitTarih > GetDate() THEN 'True' " +
    //                "ELSE 'False' END) AS izin_Durum  FROM Kullanicilar " +
    //                "WHERE KullaniciAdi='" + KullaniciAdi.ToString() +"'";
    //    cmd = new SqlCommand(sorgu, DbConnUser);
    //    cmd.CommandTimeout = 120;
    //    return (bool)cmd.ExecuteScalar();
    //}

    private void ConnectionKapat()
    {
        DbConnUser.Close();
        DbConnDepartmanKodu.Close();
        DbConnYetkiKoduBul.Close();
        DbConnLink.Close();
        DbConnYetkiTamamla.Close();
        DbConnTalepAkisi.Close();
        DbConnLink.Close();
        //DbConnYetkilendir.Close();
        //DbConnTalepIrsaliyeGuncelle.Close();
        //DbConnTalepAkisiIrsaliyeGuncelle.Close();
        //DbConnSiparisBul.Close();
        //DbConnIrsaliyeDurumu.Close();
    }

    protected void btnStokArama_Click(object sender, EventArgs e)
    {
        StokKodu = txtStokKodu.Text;
        StokAciklama = txtStokAdi.Text;

        if (string.IsNullOrEmpty(StokKodu))
        {
            SQLSorgu = "SELECT * FROM StokKodlari " +
                    "WHERE StokAciklama LIKE'%" + StokAciklama + "%' AND LEFT(STOKKODU,1)<>'M'";
        }
        else if (string.IsNullOrEmpty(StokAciklama))
        {
            SQLSorgu = "SELECT * FROM StokKodlari " +
                    "WHERE StokKodu LIKE '%" + StokKodu + "%' AND LEFT(STOKKODU,1)<>'M'";
        }

        if (!string.IsNullOrEmpty(StokKodu) && !string.IsNullOrEmpty(StokAciklama))
        {
            SQLSorgu = "SELECT * FROM StokKodlari " +
                       "WHERE StokKodu LIKE '%" + StokKodu + "%' AND LEFT(STOKKODU,1)<>'M'";
        }

        DatasetDoldur baglan = new DatasetDoldur();
        DataSet ds = baglan.StokKartlarimiz(SQLSorgu);
        drpStokKodlari.DataSource = ds;
        drpStokKodlari.DataBind();
        drpStokAdi.DataSource = ds;
        drpStokAdi.DataBind();

        string MalKodu = "";
        string MalKoduKontrol = drpStokKodlari.SelectedValue as string;

        if (string.IsNullOrEmpty(MalKoduKontrol))
        {
            MalKodu = "1";
        }
        else
        {
            MalKodu = drpStokKodlari.SelectedValue.ToString();
        }

        txtRezervMiktari.Text = Convert.ToString(RezervMiktariBul(MalKodu));

        string RezervAltKategori = Session["AltDepartmanID"].ToString();
        string RezervDepartmanKodu = Session["DepartmanKodu"].ToString();

        string Linkimiz = "<a target=\"_self\" onClick=\"MM_openBrWindow('UserControl/RezervKontrol.aspx?MalKodu=" + MalKodu + "&AltKategori=" + RezervAltKategori + "&DepartmanKodu=" + RezervDepartmanKodu + "','','toolbar=no,location=yes,status=yes,resizable=yes,scrollbars=yes,width=650,height=400')\" href=\"#\"><img src=\"iconlar/Rezerv.png\" width=\"20\" height=\"20\" alt=\"Rezerv Miktarı\" title=\"Rezerv Miktarı\" /></a>";
        ltdLink.Text = Linkimiz;

        Durum = 1;
        BirimveStokMiktariMalKodu(MalKodu);
        MalKodu = MalAdiBul(MalKodu);

        if (!string.IsNullOrEmpty(MalKodu))
        {
            drpStokAdi.SelectedItem.Value = MalKodu;
        }

        txtStokKodu.Text = null;
        txtStokAdi.Text = null;
    }

    protected void drpStokKodlari_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string MalKodu = drpStokKodlari.SelectedItem.Value.ToString();
        txtRezervMiktari.Text = Convert.ToString(RezervMiktariBul(MalKodu.ToString().Replace("'", "''")));
        string RezervAltKategori = Session["AltDepartmanID"].ToString();
        string RezervDepartmanKodu = Session["DepartmanKodu"].ToString();

        string Linkimiz = "<a target=\"_self\" onClick=\"MM_openBrWindow('UserControl/RezervKontrol.aspx?MalKodu=" + MalKodu.ToString().Replace("'", "''") + "&AltKategori=" + RezervAltKategori + "&DepartmanKodu=" + RezervDepartmanKodu + "','','toolbar=no,location=yes,status=yes,resizable=yes,scrollbars=yes,width=650,height=400')\" href=\"#\"><img src=\"iconlar/Rezerv.png\" width=\"20\" height=\"20\" alt=\"Rezerv Miktarı\" title=\"Rezerv Miktarı\" /></a>";
        ltdLink.Text = Linkimiz;

        Durum = 1;
        BirimveStokMiktariMalKodu(MalKodu.ToString().Replace("'", "''"));
        MalKodu = MalAdiBul(MalKodu.ToString().Replace("'", "''"));
        drpStokAdi.SelectedItem.Value = MalKodu;

        StokKodu = drpStokKodlari.SelectedValue.ToString();
        SQLSorgu = "SELECT * FROM StokKodlari " +
                    "WHERE StokKodu LIKE '%" + StokKodu + "%'";

        cmd = new SqlCommand(SQLSorgu, DbConnUser);
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        if (dr.Read())
        {
            drpStokAdi.SelectedItem.Text = dr["StokAciklama"].ToString();
        }

        dr.Dispose();
        dr.Close();
        DbConnUser.Dispose();
        DbConnUser.Close();
    }

    protected void drpStokAdi_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string MalAdi = drpStokAdi.SelectedItem.Value.ToString();
        Durum = 1;
        BirimveStokMiktariMalAdi(MalAdi.ToString().Replace("'", "''"));
        MalAdi = MalKoduBul(MalAdi.ToString().Replace("'", "''"));
        txtRezervMiktari.Text = Convert.ToString(RezervMiktariBul(MalAdi.ToString().Replace("'", "''")));

        string RezervAltKategori = Session["AltDepartmanID"].ToString();
        string RezervDepartmanKodu = Session["DepartmanKodu"].ToString();

        //string MalKodB = MalKoduBul(MalAdi.ToString().Replace("'", "''"));
        //Alert.Show(MalKodB);

        string Linkimiz = "<a target=\"_self\" onClick=\"MM_openBrWindow('UserControl/RezervKontrol.aspx?MalKodu=" + MalAdi + "&AltKategori=" + RezervAltKategori + "&DepartmanKodu=" + RezervDepartmanKodu + "','','toolbar=no,location=yes,status=yes,resizable=yes,scrollbars=yes,width=650,height=400')\" href=\"#\"><img src=\"iconlar/Rezerv.png\" width=\"20\" height=\"20\" alt=\"Rezerv Miktarı\" title=\"Rezerv Miktarı\" /></a>";
        ltdLink.Text = Linkimiz;
        //drpStokKodlari.SelectedItem.Value = MalAdi;

        StokKodu = drpStokAdi.SelectedValue.ToString();
        SQLSorgu = "SELECT * FROM StokKodlari " +
                    "WHERE StokAciklama LIKE '%" + StokKodu.ToString().Replace("'", "''") + "%'";

        cmd = new SqlCommand(SQLSorgu, DbConnUser);
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        if (dr.Read())
        {
            drpStokKodlari.Text = dr["StokKodu"].ToString();
            drpStokKodlari.SelectedItem.Value = dr["StokKodu"].ToString();
        }

        dr.Dispose();
        dr.Close();
        DbConnUser.Dispose();
        DbConnUser.Close();
    }

    private string GidecekDepartmanBul(string MalKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT ISNULL(dbo.fn_GidecekDepartmanBul('" + MalKodu + "'),'') AS GidecekDepartman";

        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();
    }

    private int GidecekDeptDurum(string StokDurum)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT COUNT(GidecekKod) AS Durum FROM GidecekDepartman " +
                        "WHERE GidecekDepartman='" + StokDurum + "'";

        cmd = new SqlCommand(sorgu, DbConnUser);
        return (int)Convert.ToInt32(cmd.ExecuteScalar());
    }

    private string GidecekDepartmanKodu(string MalKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT TOP(1) GidecekKod FROM GidecekDepartman " +
                    "WHERE GidecekDepartman='" + MalKodu + "' ";

        cmd = new SqlCommand(sorgu, DbConnUser);
        return (string)cmd.ExecuteScalar();
    }

    private string GidecekDepartmanKodu2(string MalKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT GidecekKod FROM GidecekDepartman " +
                    "WHERE GidecekDepartman='" + MalKodu + "' ";

        cmd = new SqlCommand(sorgu, DbConnUser);
        return (string)cmd.ExecuteScalar();
    }

    private string OnayDurumKontrolEdiyoruz(int YetkiDurumu, string OnayDurumu)
    {
        if (YetkiDurumu == 1)
        {
            switch (OnayDurumu)
            {
                case "1": sonuc = "Şeften Onay Bekleniyor"; break;
                case "2": sonuc = "Müdürden Onay Bekleniyor"; break;
                case "3": sonuc = "Ambara Gönderildi"; break;
                case "4": sonuc = "Temine Hazır"; break;
                case "5": sonuc = "Satın Alma Onayı Bekleniyor"; break;
                case "6": sonuc = "Teklif Aşamasında"; break;
                case "7": sonuc = "Sipariş Açıldı"; break;
                case "8": sonuc = "Satın Alma Tarafından Beklemeye Alındı"; break;
                case "9": sonuc = "Şef Onaylamadı"; break;
                case "10": sonuc = "Müdür Onaylamadı"; break;
                case "11": sonuc = "Satın Alma Onaylamadı"; break;
                case "12": sonuc = "Ambar Onaylamadı"; break;
                case "13": sonuc = "İhtiyaç Kapandı"; break;
            }
        }
        if (YetkiDurumu == 2)
        {
            switch (OnayDurumu)
            {
                case "1": sonuc = "Onay Bekleniyor"; break;
                case "2": sonuc = "Müdürden Onay Bekleniyor"; break;
                case "3": sonuc = "Ambara Gönderildi"; break;
                case "4": sonuc = "Temine Hazır"; break;
                case "5": sonuc = "Satın Alma Onayı Bekleniyor"; break;
                case "6": sonuc = "Teklif Aşamasında"; break;
                case "7": sonuc = "Sipariş Açıldı"; break;
                case "8": sonuc = "Satın Alma Tarafından Beklemeye Alındı"; break;
                case "9": sonuc = "Tarafınızdan Onaylamadı"; break;
                case "10": sonuc = "Müdür Onaylamadı"; break;
                case "11": sonuc = "Satın Alma Onaylamadı"; break;
                case "12": sonuc = "Ambar Onaylamadı"; break;
                case "13": sonuc = "İhtiyaç Kapandı"; break;
            }
        }
        if (YetkiDurumu == 3)
        {
            switch (OnayDurumu)
            {
                case "1": sonuc = "Şef Onayı Bekleniyor"; break;
                case "2": sonuc = "Onay Bekleniyor"; break;
                case "3": sonuc = "Ambara Gönderildi"; break;
                case "4": sonuc = "Temine Hazır"; break;
                case "5": sonuc = "Satın Alma Onayı Bekleniyor"; break;
                case "6": sonuc = "Teklif Aşamasında"; break;
                case "7": sonuc = "Sipariş Açıldı"; break;
                case "8": sonuc = "Satın Alma Tarafından Beklemeye Alındı"; break;
                case "9": sonuc = "Şef Onaylamadı"; break;
                case "10": sonuc = "Tarafınızdan Onaylamadı"; break;
                case "11": sonuc = "Satın Alma Onaylamadı"; break;
                case "12": sonuc = "Ambar Onaylamadı"; break;
                case "13": sonuc = "İhtiyaç Kapandı"; break;
            }
        }
        if (YetkiDurumu == 4)
        {
            switch (OnayDurumu)
            {
                case "1": sonuc = "Şeften Onay Bekleniyor"; break;
                case "2": sonuc = "Müdürden Onay Bekleniyor"; break;
                case "3": sonuc = "Tarafınızdan Onay Bekleniyor"; break;
                case "4": sonuc = "Temine Hazır"; break;
                case "5": sonuc = "Satın Alma Onayı Bekleniyor"; break;
                case "6": sonuc = "Teklif Aşamasında"; break;
                case "7": sonuc = "Sipariş Açıldı"; break;
                case "8": sonuc = "Satın Alma Tarafından Beklemeye Alındı"; break;
                case "9": sonuc = "Şef Onaylamadı"; break;
                case "10": sonuc = "Müdür Onaylamadı"; break;
                case "11": sonuc = "Satın Alma Onaylamadı"; break;
                case "12": sonuc = "Tarafınızdan Onaylanmadı"; break;
                case "13": sonuc = "İhtiyaç Kapandı"; break;
            }
        }
        if (YetkiDurumu == 5)
        {
            switch (OnayDurumu)
            {
                case "1": sonuc = "Şeften Onay Bekleniyor"; break;
                case "2": sonuc = "Müdürden Onay Bekleniyor"; break;
                case "3": sonuc = "Ambara Gönderildi"; break;
                case "4": sonuc = "Temine Hazır"; break;
                case "5": sonuc = "Tarafınızdan Onay Bekleniyor"; break;
                case "6": sonuc = "Teklif Aşamasında"; break;
                case "7": sonuc = "Sipariş Açıldı"; break;
                case "8": sonuc = "Tarafınızdan Beklemeye Alındı"; break;
                case "9": sonuc = "Şef Onaylamadı"; break;
                case "10": sonuc = "Müdür Onaylamadı"; break;
                case "11": sonuc = "Tarafınızdan Onaylanmadı"; break;
                case "12": sonuc = "Ambar Onaylamadı"; break;
                case "13": sonuc = "İhtiyaç Kapandı"; break;
            }
        }
        string Yeniislem = sonuc;
        return Yeniislem;
    }

    private string OnayDurumBelirliyoruz(int YetkiDurumu)
    {
        switch (YetkiDurumu)
        {
            case 1: sonuc2 = "1"; break;
            case 2: sonuc2 = "2"; break;
            case 3: sonuc2 = "3"; break;
            case 4: sonuc2 = "4"; break;
        }
        string Yeniislem = sonuc2;
        return Yeniislem;
    }

    private string AnaDepartmanKodu()
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT AnaDepartmanKodu AS GidecekDepartman FROM AnaDepartman";
        cmd = new SqlCommand(sorgu, DbConnUser);

        return (string)cmd.ExecuteScalar();
    }

    private decimal RezervMiktariBul(string MalKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string Sorgu = "SELECT ISNULL(SUM(Miktar),0) AS RezervMiktari FROM Tlp " +
                    "WHERE MalKodu='" + MalKodu.ToString() + "' AND (OnayDurumu IN(1,2,3,4,5,6,7,14)) ";

        cmd = new SqlCommand(Sorgu, DbConnUser);

        return (decimal)Convert.ToDecimal(cmd.ExecuteScalar());
    }

    private bool AnaDepartmanKontrolEdiyoruz(string KullaniciKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT AnaDepartman FROM Kullanicilar WHERE KullaniciKodu='" + KullaniciKodu + "'";

        cmd = new SqlCommand(sorgu, DbConnUser);

        return (bool)Convert.ToBoolean(cmd.ExecuteScalar());
    }

    private string SatinAlmaGidecekDepartmanBul(string MalKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT GidecekKod FROM GidecekDepartman " +
                       "WHERE GidecekDepartman=(SELECT DepartmanDurumu FROM StokKodlari " +
                       "WHERE StokKodu='" + MalKodu + "') AND ID=2";

        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();
    }

    private string HammaddeDepartmanKodu(string DepartmanDurum)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT GidecekKod from GidecekDepartman " +
                        "WHERE GidecekDepartman='" + DepartmanDurum + "' AND ID=1 ";

        cmd = new SqlCommand(sorgu, DbConnUser);

        return (string)cmd.ExecuteScalar();
    }

    protected void drpKullDepartman_SelectedIndexChanged(object sender, EventArgs e)
    {
        for (int i = 0; i < ToplamMasrafMerkeziSayimiz; i++)
        {
            if (drpKullDepartman.SelectedItem.Text == drpKullDepartman.SelectedItem.Text.ToString())
            {
                drpKullDepartman.Items[i].Selected = true;

            }
        }
    }

    protected void drpKullDepartman1_SelectedIndexChanged(object sender, EventArgs e)
    {
        for (int i = 0; i < ToplamMasrafMerkeziSayimiz; i++)
        {
            if (drpKullDepartman1.SelectedItem.Text == drpKullDepartman1.SelectedItem.Text.ToString())
            {
                drpKullDepartman1.Items[i].Selected = true;

            }
        }
    }

    private string MasrafMerkeziKoduBul(string MasrafMerkeziAdi)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT MasrafMerkeziKodu FROM MasrafMerkezi WHERE MasrafMerkeziAdi='" + MasrafMerkeziAdi + "'";

        cmd = new SqlCommand(sorgu, DbConnUser);

        return (string)cmd.ExecuteScalar();
    }

    private string YetkiUnvanimiz(int YetkiKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT Yetki_Unvani FROM Yetki_Unvanlari WHERE YetkiKodu=" + YetkiKodu + "";

        cmd = new SqlCommand(sorgu, DbConnUser);

        return (string)cmd.ExecuteScalar();
    }

    private string MalAdiniAliyoruz(string MalKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT StokAciklama FROM StokKodlari " +
                    "WHERE StokKodu='" + MalKodu.ToString() + "'";

        cmd = new SqlCommand(sorgu, DbConnUser);

        return (string)cmd.ExecuteScalar();
    }

    protected void btnStoksuzAra_Click(object sender, EventArgs e)
    {
        StokKodu = txtStokKodu1.Text;
        StokAciklama = txtStokAdi1.Text;

        if (string.IsNullOrEmpty(StokKodu))
        {
            SQLSorgu = "SELECT * FROM StokKodlari " +
                    "WHERE StokAciklama LIKE'%" + StokAciklama + "%' AND LEFT(StokKodu,1)='M' ";
        }
        else if (string.IsNullOrEmpty(StokAciklama))
        {
            SQLSorgu = "SELECT * FROM StokKodlari " +
                    "WHERE StokKodu LIKE '%" + StokKodu + "%' AND LEFT(StokKodu,1)='M' ";
        }

        if (!string.IsNullOrEmpty(StokKodu) && !string.IsNullOrEmpty(StokAciklama))
        {
            SQLSorgu = "SELECT * FROM StokKodlari " +
                       "WHERE StokKodu LIKE '%" + StokKodu + "%'  AND LEFT(StokKodu,1)='M' ";
        }

        DatasetDoldur baglan = new DatasetDoldur();
        DataSet ds = baglan.StokKartlarimiz(SQLSorgu);
        drpMasrafKodlari.DataSource = ds;
        drpMasrafKodlari.DataBind();
        drpMasrafisimleri.DataSource = ds;
        drpMasrafisimleri.DataBind();

        string MalKodu = "";
        string MalKoduKontrol = drpMasrafKodlari.SelectedValue as string;

        if (string.IsNullOrEmpty(MalKoduKontrol))
        {
            MalKodu = "1";
        }
        else
        {
            MalKodu = drpMasrafKodlari.SelectedValue.ToString();
        }

        Durum = 1;
        BirimveStoksuzMiktarMalKodu(MalKodu);
        MalKodu = MalAdiBul(MalKodu);

        if (!string.IsNullOrEmpty(MalKodu))
        {
            drpMasrafisimleri.SelectedItem.Value = MalKodu;
        }

        txtStokKodu1.Text = null;
        txtStokAdi1.Text = null;
    }

    protected void drpMasrafKodlari_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string MalKodu = drpMasrafKodlari.SelectedItem.Value.ToString();
        Durum = 1;
        BirimveStoksuzMiktarMalKodu(MalKodu.ToString().Replace("'", "''"));
        MalKodu = MalAdiBul(MalKodu.ToString().Replace("'", "''"));
        drpMasrafisimleri.SelectedItem.Value = MalKodu;

        StokKodu = drpMasrafKodlari.SelectedValue.ToString();
        SQLSorgu = "SELECT * FROM StokKodlari " +
                    "WHERE StokKodu LIKE '%" + StokKodu + "%'";

        cmd = new SqlCommand(SQLSorgu, DbConnUser);
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        if (dr.Read())
        {
            drpMasrafisimleri.SelectedItem.Text = dr["StokAciklama"].ToString();
        }

        dr.Dispose();
        dr.Close();
        DbConnUser.Dispose();
        DbConnUser.Close();
    }

    protected void drpMasrafisimleri_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string MalAdi = drpMasrafisimleri.SelectedItem.Value.ToString();
        Durum = 1;
        BirimveStoksuzMiktarMalAdi(MalAdi.ToString().Replace("'", "''"));
        MalAdi = MalKoduBul(MalAdi.ToString().Replace("'", "''"));

        StokKodu = drpMasrafisimleri.SelectedValue.ToString();
        SQLSorgu = "SELECT * FROM StokKodlari " +
                    "WHERE StokAciklama LIKE '%" + StokKodu.ToString().Replace("'", "''") + "%'";

        cmd = new SqlCommand(SQLSorgu, DbConnUser);
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        if (dr.Read())
        {
            drpMasrafKodlari.Text = dr["StokKodu"].ToString();
            drpMasrafKodlari.SelectedItem.Value = dr["StokKodu"].ToString();
        }

        dr.Dispose();
        dr.Close();
        DbConnUser.Dispose();
        DbConnUser.Close();
    }

    private string izinDurumu(string DepartmanKodu, string KullaniciKodu, string YetkiKodumuz, string AltDepartmanKodu)
    {
        if (DbConnizinAyari.State == ConnectionState.Closed)
            DbConnizinAyari.Open();

        string sorgu = "SELECT (CASE " +
                    "WHEN GETDATE() >= izin_Bastarih AND izin_BitTarih >= GETDATE() THEN 'İzne Çıkmış' " +
                    "ELSE 'İzinde Değil' " +
                    "END) AS izinDurumu FROM Kullanicilar AS K " +
                    "INNER JOIN Departmanlar AS D ON K.DepartmanID=D.DepartmanID " +
                    "WHERE D.DepartmanKodu='" + DepartmanKodu.ToString() + "' AND (AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " OR AltDepartmanID = 0) " +
                    "AND Yetki =(SELECT ISNULL(MIN(Yetki),0) AS Yetki FROM Kullanicilar AS K " +
                    "INNER JOIN Departmanlar AS D ON K.DepartmanID=D.DepartmanID " +
                    "WHERE D.DepartmanKodu='" + DepartmanKodu.ToString() + "'  AND (AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " OR AltDepartmanID = 0) " +
                    "AND KullaniciAdi <> '" + KullaniciKodu.ToString() + "' " +
                    "AND Yetki > " + Convert.ToInt32(YetkiKodumuz) + "  AND K.Durum=1 " +
                    "AND (MenuKodu<>'Satınalma' AND MenuKodu<>'Ambar') AND K.KullaniciAdi<>'Admin' )";

        cmdizinDurum = new SqlCommand(sorgu, DbConnizinAyari);
        cmdizinDurum.CommandTimeout = 120;
        return (string)cmdizinDurum.ExecuteScalar();
    }
}
