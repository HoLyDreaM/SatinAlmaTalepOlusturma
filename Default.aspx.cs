﻿
using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using System.Windows.Forms;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.IO;
using DevExpress.Web;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Web.Configuration;

public partial class _Default : System.Web.UI.Page
{

    #region Bağlantı Ayarları

    SqlConnection DbConnUser = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnEvrakTarihUps = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection Kullanicilar = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConMaxYetki = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnLink = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnLink"].ToString());
    SqlConnection DbConnLinkSorgu = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnLinkSorgu"].ToString());

    SqlConnection DbConnDepartmanKodu = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnYetkiKoduBul = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnYetkiTamamla = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnYetkilendir = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());

    SqlConnection DbConnTalepIrsaliyeGuncelle = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnTalepAkisiIrsaliyeGuncelle = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());

    SqlConnection DbConnSiparisBul = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnIrsaliyeDurumu = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnLink"].ToString());

    SqlCommand cmd, cmdEvrakTarihUps, cmdYetki, cmdTalepAkisi, cmdTalepAkisDuzenle, MaxYetkiBul,
        cmdSiparisBul, cmdIrsaliyeDurumu, cmdTalepIrsaliyeGuncelle, cmdTalepAkisiIrsaliyeGuncelle;
    SqlDataReader dr, drYetki;
    string Datasource, dsSorgu, DepoKodu, TalepDurum, DepartmanKodumuz, Yetkimiz, AltDepartmanKodumuz, StokluDepartman,
        GelenKullanici, KayitYetki, SipNoBulduk;
    bool Ekle, Guncelle, Sil, Onay, OnayDurum;
    int TalepID, OnaylayanYetki, DepartmanYetkiDurum, SiparisDurumKontrolu, TalepIrsaliyeID,
        SiparisDurumumumuz;
    bool HomePage, StokKartiSorgulama, IhtiyacArama, Talepler, Formlar, Tanimlar, KullaniciTanimi, DigerTanimlar, SiparisButonu, SatinAlmaButonu, AmbarButonu;

    #endregion

    string ihtiyacNo2, MalKodumuz2, MalAdlari2, Miktar2, Kdv2, TalepID2, OnayDurumu, sonuc, GitDeptKod2, GitDeptKod3,
        AmbarKodu, SatinAlmaKodu, WhereSorgu, sonuc2, AciklamaSatirimiz2;
    int Sayfaliyoruz, YetkiKodu, OnayYetkiDurumu, DepartmanCountDurum;
    decimal Miktar;
    bool DurumKontrolu, AnaDepartmanimiz;
    string[] LinkDb, GitDeptKod;
    string Linkdb2, SifDb, Dizimiz;
    string[] evrakNumaramiz, MalKodlarimiz, MalAdlarimiz, Miktarlarimiz, TalepIDlerimiz, AciklamaSat;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Giris"] != "Evet")
        {
            Response.Redirect("Login.Aspx");
        }
        else
        {
            try
            {
                #region Saat ve İp yi Güncelliyoruz

                if (Kullanicilar.State == ConnectionState.Closed)
                    Kullanicilar.Open();

                //string ip = Request.ServerVariables["REMOTE_ADDR"].ToString();

                cmd = new SqlCommand("UPDATE Kullanicilar SET LastLogin = GETDATE() WHERE USERID= '" + Session["ID"].ToString() + "'", Kullanicilar);

                cmd.ExecuteNonQuery();

                #endregion

                int EvrakNumarasiTarihKontrolu = Convert.ToInt32(DateTime.Now.Year);
                string WebConfigEvrakYili = ConfigurationManager.AppSettings["EvrakNumaraTarihi"];

                if (Convert.ToString(EvrakNumarasiTarihKontrolu) != WebConfigEvrakYili)
                {
                    EvrakNumaralariniSifirla(EvrakNumarasiTarihKontrolu);

                    Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                    config.AppSettings.Settings.Remove("EvrakNumaraTarihi");
                    config.AppSettings.Settings.Add("EvrakNumaraTarihi", Convert.ToString(EvrakNumarasiTarihKontrolu));
                    config.Save();
                }

                Session["Sorgu"] = "";

                grdTaleplerimiz.PagerSettings.Mode = PagerButtons.NumericFirstLast;

                int DepartmanID = Convert.ToInt32(Session["DepartmanID"].ToString());
                DepartmanYetkiDurum = YetkiDurumu(DepartmanID);
                string Yetkili = Session["Yetki"].ToString();
                string Kaydeden = Session["DepartmanKodu"].ToString();
                AltDepartmanKodumuz = Session["AltDepartmanKodu"].ToString();
                DepoKodu = Session["DepartmanKodu"].ToString();
                //StokluDepartman = Session["StokluDepartmanKodu"].ToString();
                AnaDepartmanimiz = Convert.ToBoolean(Session["AnaDepartman"].ToString());

                string LinkVeritabanı = ConfigurationManager.ConnectionStrings["DbConnLinkSorgu"].ToString();
                string SifVeritabani = ConfigurationManager.ConnectionStrings["DbConnUser"].ToString();

                string[] SifDizi = new string[6];
                string[] SifDizi2 = new string[3];
                string[] LinkDizi = new string[6];
                string[] LinkDizi2 = new string[3];
                LinkDizi = LinkVeritabanı.Split('=');
                LinkVeritabanı = LinkDizi[2].ToString();
                LinkDizi2 = LinkVeritabanı.Split(';');
                Linkdb2 = LinkDizi2[0].ToString();

                SifDizi = SifVeritabani.Split('=');
                SifVeritabani = SifDizi[2].ToString();
                SifDizi2 = SifVeritabani.Split(';');
                SifDb = SifDizi2[0].ToString();

                GelenKullanici = Session["KullaniciKodu"].ToString();
                SiparisButonu = SiparisButonDurumu(GelenKullanici);
                SatinAlmaButonu = SatinAlmaButonDurumu(GelenKullanici);
                btnSatinAlmayaGonder.Visible = SatinAlmaButonu;
                btnSiparisKapat.Visible = SiparisButonu;
                btnBeklemeyeAl.Visible = SiparisButonu;
                btnAmbaraGonder.Visible = SiparisButonu;
                btnDagit.Visible = AnaDepartmanimiz;

                string BagliOlduguDepartman = Session["BagliOlduguBirim"].ToString();

                //grdTalepler.Columns[10].Visible = false;
                grdTaleplerimiz.Columns[14].Visible = false;
                imgParcaliSiparisGonder.Visible = false;
                imgTopluSiparis.Visible = false;

                if (BagliOlduguDepartman == "0")
                {
                    YetkiKoduBul2(DepoKodu, GelenKullanici.ToString(), Convert.ToString(Yetkili));
                }
                else
                {
                    YetkiKoduBul(DepoKodu, GelenKullanici.ToString(), Convert.ToString(Yetkili), BagliOlduguDepartman);
                }

                if (Convert.ToInt32(KayitYetki) == 0)
                {
                    OnayYetkiDurumu = 4;
                }
                else
                {
                    OnayYetkiDurumu = Convert.ToInt32(KayitYetki);
                }

                if (!IsPostBack)
                {
                    if (SatinAlmaButonu == true)
                    {
                        rdTalepler.Items[0].Enabled = true;
                        rdTalepler.Items[2].Enabled = true;
                        rdTalepler.Items[7].Enabled = true;
                        rdTalepler.Items[8].Enabled = true;
                        rdTalepler.Items[6].Enabled = true;

                        rdTalepler.Items[1].Enabled = false;
                        rdTalepler.Items[3].Enabled = false;
                        rdTalepler.Items[4].Enabled = false;
                        rdTalepler.Items[5].Enabled = false;
                        rdTalepler.Items[9].Enabled = false;

                        if (AnaDepartmanimiz == false)
                        {
                            AramaDepartmanlari(DepoKodu);
                            AramaDepartmanlari2(GitDeptKod2);
                            int son = GitDeptKod3.ToString().Length - 1;
                            GitDeptKod3 = GitDeptKod3.ToString().Substring(0, son);
                            AmbarKodu = GitDeptKod3.ToString();

                            string[] Amb2 = AmbarKodu.Split(',');
                            AmbarKodu = null;

                            for (int i = 0; i < Amb2.Length; i++)
                            {
                                AmbarKodu += "'" + Amb2[i].ToString() + "',";
                            }

                            int Amb3 = AmbarKodu.ToString().Length - 1;
                            AmbarKodu = AmbarKodu.ToString().Substring(0, Amb3);
                            Session["AmbarKodu"] = AmbarKodu.ToString();
                            Session["SatinAlmaKodu"] = "";
                        }
                        else
                        {
                            Session["AmbarKodu"] = AnaDepartmanKodu();
                            Session["SatinAlmaKodu"] = "";
                        }
                        rdTalepler.SelectedIndex = 2;
                        rdTalepler_SelectedIndexChanged(sender, e);

                        //

                        if (DbConnLinkSorgu.State == ConnectionState.Closed)
                            DbConnLinkSorgu.Open();

                        DataTable dt = new DataTable();

                        string sorgu = "SELECT T.TalepID,T.EvrakNoTarih+T.EvrakNo AS EvrakNo, " +
                                        "CONVERT(VARCHAR(10),CONVERT(DATETIME,T.Tarih-2),120) AS Tarih, " +
                                        "T.MalKodu,T.MalAdi,T.Aciklama,Miktar " +
                                        "FROM " + SifDb + ".dbo.Tlp AS T " +
                                        "INNER JOIN " + Linkdb2.ToString() + ".STK005 AS S ON S.STK005_MalSeriNo=T.EvrakNoTarih+T.EvrakNo " +
                                        "WHERE (T.OnayDurumu = 7) AND (T.SiparisDurum = 2) AND (T.IrsaliyeDurum = 0)";

                        SqlDataAdapter da = new SqlDataAdapter(sorgu, DbConnLinkSorgu);
                        da.Fill(dt);

                        foreach (DataRow dr in dt.Rows)
                        {
                            string ihtiyacNumaramiz = dr["EvrakNo"].ToString();
                            SipNoBulduk = SiparisNumarasiBul(ihtiyacNumaramiz);

                            if (!string.IsNullOrEmpty(SipNoBulduk))
                            {
                                SiparisDurumKontrolu = SiparisDurum(ihtiyacNumaramiz);
                                TalepIrsaliyeID = Convert.ToInt32(dr["TalepID"].ToString());

                                if (SiparisDurumKontrolu > 0)
                                {
                                    TalepIrsaliyeDurumGuncelle(GelenKullanici, ihtiyacNumaramiz);
                                    //TalepAkisiIrsaliyeDurumGuncelle(TalepIrsaliyeID);
                                }
                            }
                        }
                    }
                    else if (SiparisButonu == true)
                    {
                        rdTalepler.Items[0].Enabled = true;
                        rdTalepler.Items[1].Enabled = true;
                        rdTalepler.Items[3].Enabled = true;
                        rdTalepler.Items[4].Enabled = true;
                        rdTalepler.Items[9].Enabled = true;

                        rdTalepler.Items[2].Enabled = false;
                        rdTalepler.Items[5].Enabled = false;
                        rdTalepler.Items[6].Enabled = false;
                        rdTalepler.Items[7].Enabled = false;
                        rdTalepler.Items[8].Enabled = false;

                        AramaDepartmanlari(DepoKodu);
                        AramaDepartmanlari3(GitDeptKod2);
                        int son = GitDeptKod3.ToString().Length - 1;
                        GitDeptKod3 = GitDeptKod3.ToString().Substring(0, son);
                        SatinAlmaKodu = GitDeptKod3.ToString();

                        string[] Sat2 = SatinAlmaKodu.Split(',');
                        SatinAlmaKodu = null;

                        for (int i = 0; i < Sat2.Length; i++)
                        {
                            SatinAlmaKodu += "'" + Sat2[i].ToString() + "',";
                        }

                        int Sat3 = SatinAlmaKodu.ToString().Length - 1;
                        SatinAlmaKodu = SatinAlmaKodu.ToString().Substring(0, Sat3);
                        Session["SatinAlmaKodu"] = SatinAlmaKodu.ToString();
                        Session["AmbarKodu"] = "";

                        rdTalepler.SelectedIndex = 1;
                        grdTaleplerimiz.Columns[14].Visible = SiparisButonu;
                        imgParcaliSiparisGonder.Visible = SiparisButonu;
                        btnSiparisKapat.Visible = SiparisButonu;
                        imgTopluSiparis.Visible = SiparisButonu;
                        rdTalepler_SelectedIndexChanged(sender, e);
                    }
                    else if (SatinAlmaButonu == true)
                    {
                        rdTalepler.SelectedIndex = 2;
                        rdTalepler_SelectedIndexChanged(sender, e);
                    }
                    else
                    {
                        Session["SatinAlmaKodu"] = "";
                        Session["AmbarKodu"] = "";

                        rdTalepler.SelectedIndex = 5;
                        rdTalepler_SelectedIndexChanged(sender, e);
                    }

                    if (SiparisButonu == true)
                    {
                        rdTalepler.SelectedIndex = 1;
                        grdTaleplerimiz.Columns[14].Visible = SiparisButonu;
                        imgParcaliSiparisGonder.Visible = SiparisButonu;
                        btnSiparisKapat.Visible = SiparisButonu;
                        imgTopluSiparis.Visible = SiparisButonu;
                        rdTalepler_SelectedIndexChanged(sender, e);
                    }
                    else if (SatinAlmaButonu == true)
                    {
                        rdTalepler.SelectedIndex = 2;
                        rdTalepler_SelectedIndexChanged(sender, e);
                    }
                    else
                    {
                        rdTalepler.SelectedIndex = 5;
                        rdTalepler_SelectedIndexChanged(sender, e);
                    }

                    if (SatinAlmaButonu != true && SiparisButonu != true)
                    {
                        rdTalepler.Items[0].Enabled = true;
                        rdTalepler.Items[1].Enabled = true;
                        rdTalepler.Items[2].Enabled = true;
                        rdTalepler.Items[3].Enabled = true;
                        rdTalepler.Items[4].Enabled = true;
                        rdTalepler.Items[5].Enabled = true;
                        rdTalepler.Items[6].Enabled = true;
                        rdTalepler.Items[7].Enabled = true;
                        rdTalepler.Items[8].Enabled = true;
                        rdTalepler.Items[9].Enabled = true;

                        NormalDepartmanSatinAlma();
                        NormalDepartmanAmbar();

                        string[] Sat1 = SatinAlmaKodu.Split(',');
                        string[] Amb1 = AmbarKodu.Split(',');

                        SatinAlmaKodu = null;
                        AmbarKodu = null;

                        for (int i = 0; i < Sat1.Length - 1; i++)
                        {
                            SatinAlmaKodu += "'" + Sat1[i].ToString() + "',";
                        }

                        for (int i = 0; i < Amb1.Length - 1; i++)
                        {
                            AmbarKodu += "'" + Amb1[i].ToString() + "',";
                        }

                        int sat = SatinAlmaKodu.ToString().Length - 1;
                        int Amb = AmbarKodu.ToString().Length - 1;

                        SatinAlmaKodu = SatinAlmaKodu.ToString().Substring(0, sat);
                        AmbarKodu = AmbarKodu.ToString().Substring(0, Amb);

                        NormalDepartmanSatinAlma2(SatinAlmaKodu);
                        NormalDepartmanAmbar2(AmbarKodu);

                        string[] Sat2 = SatinAlmaKodu.Split(',');
                        string[] Amb2 = AmbarKodu.Split(',');

                        SatinAlmaKodu = null;
                        AmbarKodu = null;

                        for (int i = 0; i < Sat2.Length - 1; i++)
                        {
                            SatinAlmaKodu += "'" + Sat2[i].ToString() + "',";
                        }

                        for (int i = 0; i < Amb2.Length - 1; i++)
                        {
                            AmbarKodu += "'" + Amb2[i].ToString() + "',";
                        }

                        int sat3 = SatinAlmaKodu.ToString().Length - 1;
                        int Amb3 = AmbarKodu.ToString().Length - 1;

                        SatinAlmaKodu = SatinAlmaKodu.ToString().Substring(0, sat3);
                        AmbarKodu = AmbarKodu.ToString().Substring(0, Amb3);

                        Session["SatinAlmaKodu"] = SatinAlmaKodu.ToString();
                        Session["AmbarKodu"] = AmbarKodu.ToString();
                    }
                }

            }
            catch (Exception ex)
            {

                Alert.Show(ex.Message.ToString());
            }
        }
    }

    private int YetkiDurumu(int DepartmanID)
    {
        if (DbConMaxYetki.State == ConnectionState.Closed)
        {
            DbConMaxYetki.Open();
        }

        string Sorgu = "SELECT Max(Yetki) AS Yetki FROM Kullanicilar " +
                        "WHERE DepartmanID=" + DepartmanID + "";

        MaxYetkiBul = new SqlCommand(Sorgu, DbConMaxYetki);
        MaxYetkiBul.CommandTimeout = 120;
        return Convert.ToInt32(MaxYetkiBul.ExecuteScalar());
    }

    protected void rdTalepler_SelectedIndexChanged(object sender, EventArgs e)
    {
        AnaDepartmanimiz = Convert.ToBoolean(Session["AnaDepartman"].ToString());

        string Yetkili = Session["Yetki"].ToString();
        string YetkiUnvanimiz = YetkiUnvaniBul(Convert.ToInt32(Yetkili));

        string Kaydeden = Session["DepartmanKodu"].ToString();
        string KullaniciKodumuz = Session["KullaniciKodu"].ToString();
        string OnayDurum = "";
        int DepartmanID = Convert.ToInt32(Session["DepartmanID"].ToString());
        DepartmanYetkiDurum = YetkiDurumu(DepartmanID);
        DepoKodu = Session["DepartmanKodu"].ToString();
        string AltDepartmanKodu = Session["AltDepartmanID"].ToString();
        string BagliOlduguDepartman = Session["BagliOlduguBirim"].ToString();
        string KullaniciKodu = Session["KullaniciKodu"].ToString();
        int DepartmanYetkilisi = DepartmanYetkisiBul(DepoKodu);

        string LinkVeritabanı = ConfigurationManager.ConnectionStrings["DbConnLink"].ToString();
        LinkDb = LinkVeritabanı.Split(';');
        LinkVeritabanı = LinkDb[1];
        LinkVeritabanı = LinkVeritabanı.ToString().Substring(16, 8);

        if (SiparisButonu == true)
        {
            grdTaleplerimiz.Columns[8].Visible = SiparisButonu;
        }
        else
        {
            grdTaleplerimiz.Columns[9].Visible = SiparisButonu;
        }

        if (rdTalepler.SelectedIndex == 0)
        {
            Sayfaliyoruz = 0;
            Session["SayfaDegeri"] = "0";

            btnBeklemeyeAl.Visible = false;
            btnSatinAlmayaGonder.Visible = false;
            btnTumunuOnayla.Visible = false;
            grdTaleplerimiz.Columns[1].Visible = false;
            btnDagit.Visible = false;
            btnAmbaraGonder.Visible = false;
            btnSiparisKapat.Visible = false;

            OnayDurum = Session["SatinAlmaKodu"].ToString();
            string OnayDurum2 = Session["AmbarKodu"].ToString();

            if (SiparisButonu == false && SatinAlmaButonu == false)
            {
                if (DepartmanYetkilisi == Convert.ToInt32(Yetkili))
                {
                    WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnayDurumu IN(1,2,3,4,5,6,7,8,9,10,11,12,14))" +
                    "AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  ";
                }
                else
                {
                    if (Convert.ToInt32(AltDepartmanKodu) == 0)
                    {
                        WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                                "AND K.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                                "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ") ";
                    }
                    else
                    {
                        if (Convert.ToInt32(BagliOlduguDepartman) == 0)
                        {
                            WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                            "AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                            "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  " +
                            "AND (K.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " OR K.BagliAltDepartman=" + Convert.ToInt32(AltDepartmanKodu) + ") " +
                            "AND (T.OnayDurumu IN(1,2,3,4,5,6,7,8,9,10,11,12,14)) ";
                        }
                        else
                        {
                            WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                                "AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                                "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  " +
                                "AND T.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " AND (T.OnayDurumu IN(1,2,3,4,5,6,7,8,9,10,11,12,14)) ";
                        }
                    }
                }
            }
            else if (SiparisButonu == true)
            {
                WhereSorgu = "(T.GidecekDepartman IN (" + OnayDurum.ToString() + "))  AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + ") " +
                    "AND (T.OnayDurumu IN(5,6,7,8,11,14)) ";
            }
            else if (SatinAlmaButonu == true)
            {//AMBAR TARAFINA AYARLANACAK
                WhereSorgu = "(T.GidecekDepartman IN (" + OnayDurum2.ToString() + "))  AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + ") " +
                    "AND (T.OnayDurumu IN(3,4,12)) ";
            }

            #region Tümü

            Datasource = "SELECT T.TalepID,T.EvrakNoTarih+T.EvrakNo AS EvrakNo, " +
                    "CONVERT(VARCHAR(10),CONVERT(DATETIME,Tarih-2),104) AS Tarih, " +
                    "T.Aciklama,T.MalKodu,T.MalAdi,CONVERT(NUMERIC(18,2),T.Miktar) AS Miktar,T.Birim, " +
                    "ISNULL((SELECT ((STK004_GirisMiktari+STK004_DevirMiktari)-STK004_CikisMiktari) AS StokMiktar " +
                    "FROM  " + LinkVeritabanı.ToString() + "." + LinkVeritabanı.ToString() + ".STK004  " +
                    "WHERE STK004_MalKodu=T.MalKodu),'0') AS StokMiktari, " +
                    "T.SipMiktar,TM.TeminAdi AS TeminYeri,T.OnayDurumu,T.AdSoyad,K.Yetki,T.MasrafMerkeziAdi AS KullanilacakYer, " +
                    "(CASE WHEN T.Kaydeden = '" + KullaniciKodumuz.ToString() + "' THEN " +
                    "((CASE T.ilkIslem WHEN 1 THEN 0 " +
                    "WHEN 0 THEN 1 END)) ELSE 0 END) AS ilkIslem, " +
                    "(CASE  " +
                    "WHEN T.OnaylayanYetki = " + Convert.ToInt32(Yetkili) + " THEN 1  " +
                    "ELSE 0 " +
                    "END) AS OnayDurum " +
                    "FROM Tlp AS T " +
                    "INNER JOIN TeminYeri AS TM ON TM.TeminKodu=T.TeminYeri " +
                    "INNER JOIN Kullanicilar AS K ON T.Kaydeden=K.KullaniciKodu " +
                    "WHERE " + WhereSorgu +
                    "ORDER BY T.TalepID DESC";

            #endregion

        }
        else if (rdTalepler.SelectedIndex == 1)
        {
            //OnayDurum = AramaDurumDepartmani(Kaydeden) + " Onayı Bekliyor";
            OnayDurum = Session["SatinAlmaKodu"].ToString();
            btnDagit.Visible = false;
            btnSiparisKapat.Visible = false;

            if (SiparisButonu == true)
            {
                btnBeklemeyeAl.Visible = true;
                btnTumunuOnayla.Visible = true;
                btnAmbaraGonder.Visible = true;
                grdTaleplerimiz.Columns[1].Visible = true;
            }
            else
            {
                btnAmbaraGonder.Visible = false;
                btnBeklemeyeAl.Visible = false;
                grdTaleplerimiz.Columns[1].Visible = false;
                btnTumunuOnayla.Visible = false;
            }

            if (SiparisButonu == false)
            {
                if (DepartmanYetkilisi == Convert.ToInt32(Yetkili))
                {
                    WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ")  AND (T.OnayDurumu IN(5)) " +
                    "AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  ";
                }
                else
                {
                    if (Convert.ToInt32(AltDepartmanKodu) == 0)
                    {
                        WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                                "AND K.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                                "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ") ";
                    }
                    else
                    {
                        if (Convert.ToInt32(BagliOlduguDepartman) == 0)
                        {
                            WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                            "AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                            "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  " +
                            "AND (K.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " OR K.BagliAltDepartman=" + Convert.ToInt32(AltDepartmanKodu) + ") " +
                            "AND (T.OnayDurumu IN(5)) ";
                        }
                        else
                        {
                            WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                                "AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                                "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  " +
                                "AND T.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " AND (T.OnayDurumu IN(5)) ";
                        }
                    }
                }
            }
            else
            {
                WhereSorgu = "(T.GidecekDepartman IN (" + OnayDurum.ToString() + ")) AND (T.OnayDurumu IN(5)) ";
            }

            Sayfaliyoruz = 1;
            Session["SayfaDegeri"] = "1";
            //btnBeklemeyeAl.Visible = true;
            //btnTumunuOnayla.Visible = true;
            grdTaleplerimiz.Columns[14].Visible = SiparisButonu;
            imgParcaliSiparisGonder.Visible = SiparisButonu;
            imgTopluSiparis.Visible = SiparisButonu;

            #region Satın Alma Onayı

            Datasource = "SELECT T.TalepID,T.EvrakNoTarih+T.EvrakNo AS EvrakNo, " +
                    "CONVERT(VARCHAR(10),CONVERT(DATETIME,Tarih-2),104) AS Tarih, " +
                    "T.Aciklama,T.MalKodu,T.MalAdi,CONVERT(NUMERIC(18,2),T.Miktar) AS Miktar,T.Birim, " +
                    "ISNULL((SELECT ((STK004_GirisMiktari+STK004_DevirMiktari)-STK004_CikisMiktari) AS StokMiktar " +
                    "FROM  " + LinkVeritabanı.ToString() + "." + LinkVeritabanı.ToString() + ".STK004  " +
                    "WHERE STK004_MalKodu=T.MalKodu),'0') AS StokMiktari, " +
                    "T.SipMiktar,TM.TeminAdi AS TeminYeri,T.OnayDurumu,T.AdSoyad,K.Yetki,T.MasrafMerkeziAdi AS KullanilacakYer, " +
                    "(CASE WHEN T.Kaydeden = '" + KullaniciKodumuz.ToString() + "' THEN " +
                    "((CASE T.ilkIslem WHEN 1 THEN 0 " +
                    "WHEN 0 THEN 1 END)) ELSE 0 END) AS ilkIslem, " +
                    "(CASE  " +
                    "WHEN T.OnaylayanYetki = " + Convert.ToInt32(Yetkili) + " THEN 1  " +
                    "ELSE 0 " +
                    "END) AS OnayDurum " +
                    "FROM Tlp AS T " +
                    "INNER JOIN TeminYeri AS TM ON TM.TeminKodu=T.TeminYeri " +
                    "INNER JOIN Kullanicilar AS K ON T.Kaydeden=K.KullaniciKodu " +
                    "WHERE " + WhereSorgu +
                    "ORDER BY T.TalepID DESC";

            #endregion

        }
        else if (rdTalepler.SelectedIndex == 2)
        {
            Sayfaliyoruz = 2;
            Session["SayfaDegeri"] = "2";
            OnayDurum = Session["AmbarKodu"].ToString();
            btnAmbaraGonder.Visible = SiparisButonu;
            btnSiparisKapat.Visible = false;
            btnDagit.Visible = AnaDepartmanimiz;

            if (SatinAlmaButonu == true)
            {
                btnBeklemeyeAl.Visible = false;
                btnTumunuOnayla.Visible = true;
                btnSatinAlmayaGonder.Visible = true;

                grdTaleplerimiz.Columns[1].Visible = true;
            }
            else
            {
                btnSatinAlmayaGonder.Visible = false;
                btnDagit.Visible = false;
                btnBeklemeyeAl.Visible = false;
                grdTaleplerimiz.Columns[1].Visible = false;
                btnTumunuOnayla.Visible = false;
            }

            if (SatinAlmaButonu == false)
            {
                if (DepartmanYetkilisi == Convert.ToInt32(Yetkili))
                {
                    WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnayDurumu IN(3)) " +
                    "AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  ";
                }
                else
                {
                    if (Convert.ToInt32(AltDepartmanKodu) == 0)
                    {
                        WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                                "AND K.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                                "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ") ";
                    }
                    else
                    {
                        if (Convert.ToInt32(BagliOlduguDepartman) == 0)
                        {
                            WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                            "AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                            "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  " +
                            "AND (K.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " OR K.BagliAltDepartman=" + Convert.ToInt32(AltDepartmanKodu) + ") " +
                            "AND (T.OnayDurumu IN(3)) ";
                        }
                        else
                        {
                            WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                                "AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                                "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  " +
                                "AND T.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " AND (T.OnayDurumu IN(3)) ";
                        }
                    }
                }
            }
            else
            {
                WhereSorgu = "(T.GidecekDepartman IN (" + OnayDurum.ToString() + "))  AND (T.OnayDurumu IN(3)) ";
            }

            #region Ambar Onayı

            Datasource = "SELECT T.TalepID,T.EvrakNoTarih+T.EvrakNo AS EvrakNo, " +
                "CONVERT(VARCHAR(10),CONVERT(DATETIME,Tarih-2),104) AS Tarih, " +
                "T.Aciklama,T.MalKodu,T.MalAdi,CONVERT(NUMERIC(18,2),T.Miktar) AS Miktar,T.Birim, " +
                "ISNULL((SELECT ((STK004_GirisMiktari+STK004_DevirMiktari)-STK004_CikisMiktari) AS StokMiktar " +
                "FROM  " + LinkVeritabanı.ToString() + "." + LinkVeritabanı.ToString() + ".STK004  " +
                "WHERE STK004_MalKodu=T.MalKodu),'0') AS StokMiktari, " +
                "T.SipMiktar,TM.TeminAdi AS TeminYeri,T.OnayDurumu,T.AdSoyad,K.Yetki,T.MasrafMerkeziAdi AS KullanilacakYer, " +
                "(CASE WHEN T.Kaydeden = '" + KullaniciKodumuz.ToString() + "' THEN " +
                "((CASE T.ilkIslem WHEN 1 THEN 0 " +
                "WHEN 0 THEN 1 END)) ELSE 0 END) AS ilkIslem, " +
                "(CASE  " +
                "WHEN T.OnaylayanYetki = " + Convert.ToInt32(Yetkili) + " THEN 1  " +
                "ELSE 0 " +
                "END) AS OnayDurum " +
                "FROM Tlp AS T " +
                "INNER JOIN TeminYeri AS TM ON TM.TeminKodu=T.TeminYeri " +
                "INNER JOIN Kullanicilar AS K ON T.Kaydeden=K.KullaniciKodu " +
                "WHERE " + WhereSorgu +
                "ORDER BY T.TalepID DESC";

            #endregion

        }
        else if (rdTalepler.SelectedIndex == 3)
        {
            OnayDurum = Session["SatinAlmaKodu"].ToString();

            if (SiparisButonu == true)
            {
                btnBeklemeyeAl.Visible = true;
                btnTumunuOnayla.Visible = true;
                btnAmbaraGonder.Visible = false;
                grdTaleplerimiz.Columns[1].Visible = true;
            }
            else
            {
                btnAmbaraGonder.Visible = false;
                btnBeklemeyeAl.Visible = false;
                grdTaleplerimiz.Columns[1].Visible = false;
                btnTumunuOnayla.Visible = false;
            }

            if (SiparisButonu == false)
            {
                if (DepartmanYetkilisi == Convert.ToInt32(Yetkili))
                {
                    WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnayDurumu IN(6)) " +
                    "AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  ";
                }
                else
                {
                    if (Convert.ToInt32(AltDepartmanKodu) == 0)
                    {
                        WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                                "AND K.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                                "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ") ";
                    }
                    else
                    {
                        if (Convert.ToInt32(BagliOlduguDepartman) == 0)
                        {
                            WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                            "AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                            "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  " +
                            "AND (K.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " OR K.BagliAltDepartman=" + Convert.ToInt32(AltDepartmanKodu) + ") " +
                            "AND (T.OnayDurumu IN(6)) ";
                        }
                        else
                        {
                            WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                                "AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                                "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  " +
                                "AND T.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " AND (T.OnayDurumu IN(6)) ";
                        }
                    }
                }
            }
            else
            {
                WhereSorgu = "(T.GidecekDepartman IN (" + OnayDurum.ToString() + ")) AND (T.OnayDurumu=6)";
            }

            Sayfaliyoruz = 3;
            Session["SayfaDegeri"] = "3";
            grdTaleplerimiz.Columns[14].Visible = SiparisButonu;
            imgParcaliSiparisGonder.Visible = SiparisButonu;
            imgTopluSiparis.Visible = SiparisButonu;
            btnBeklemeyeAl.Visible = true;
            btnTumunuOnayla.Visible = false;
            btnDagit.Visible = false;
            btnAmbaraGonder.Visible = SiparisButonu;
            btnSiparisKapat.Visible = false;

            #region Teklif Bekleyenler

            Datasource = "SELECT T.TalepID,T.EvrakNoTarih+T.EvrakNo AS EvrakNo, " +
                    "CONVERT(VARCHAR(10),CONVERT(DATETIME,Tarih-2),104) AS Tarih, " +
                    "T.Aciklama,T.MalKodu,T.MalAdi,CONVERT(NUMERIC(18,2),T.Miktar) AS Miktar,T.Birim, " +
                    "ISNULL((SELECT ((STK004_GirisMiktari+STK004_DevirMiktari)-STK004_CikisMiktari) AS StokMiktar " +
                    "FROM  " + LinkVeritabanı.ToString() + "." + LinkVeritabanı.ToString() + ".STK004  " +
                    "WHERE STK004_MalKodu=T.MalKodu),'0') AS StokMiktari, " +
                    "T.SipMiktar,TM.TeminAdi AS TeminYeri,T.OnayDurumu,T.AdSoyad,K.Yetki,T.MasrafMerkeziAdi AS KullanilacakYer, " +
                    "(CASE WHEN T.Kaydeden = '" + KullaniciKodumuz.ToString() + "' THEN " +
                    "((CASE T.ilkIslem WHEN 1 THEN 0 " +
                    "WHEN 0 THEN 1 END)) ELSE 0 END) AS ilkIslem, " +
                    "(CASE  " +
                    "WHEN T.OnaylayanYetki = " + Convert.ToInt32(Yetkili) + " THEN 1  " +
                    "ELSE 0 " +
                    "END) AS OnayDurum " +
                    "FROM Tlp AS T " +
                    "INNER JOIN TeminYeri AS TM ON TM.TeminKodu=T.TeminYeri " +
                    "INNER JOIN Kullanicilar AS K ON T.Kaydeden=K.KullaniciKodu " +
                    "WHERE " + WhereSorgu +
                    "ORDER BY T.TalepID DESC";

            #endregion

        }
        else if (rdTalepler.SelectedIndex == 4)
        {
            Sayfaliyoruz = 4;
            Session["SayfaDegeri"] = "4";

            OnayDurum = Session["SatinAlmaKodu"].ToString();
            btnDagit.Visible = false;
            btnAmbaraGonder.Visible = false;
            btnSiparisKapat.Visible = false;

            if (SiparisButonu == false)
            {
                if (DepartmanYetkilisi == Convert.ToInt32(Yetkili))
                {
                    WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnayDurumu IN(7,14)) " +
                    "AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  ";
                }
                else
                {
                    if (Convert.ToInt32(AltDepartmanKodu) == 0)
                    {
                        WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                                "AND K.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                                "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ") ";
                    }
                    else
                    {
                        if (Convert.ToInt32(BagliOlduguDepartman) == 0)
                        {
                            WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                            "AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                            "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  " +
                            "AND (K.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " OR K.BagliAltDepartman=" + Convert.ToInt32(AltDepartmanKodu) + ") " +
                            "AND (T.OnayDurumu IN(7,14)) ";
                        }
                        else
                        {
                            WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                                "AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                                "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  " +
                                "AND T.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " AND (T.OnayDurumu IN(7,14)) ";
                        }
                    }
                }
            }
            else
            {
                WhereSorgu = "(T.GidecekDepartman IN (" + OnayDurum.ToString() + ")) AND (T.OnayDurumu IN(7,14))";
            }

            if (SiparisButonu == true)
            {
                btnBeklemeyeAl.Visible = false;
                imgParcaliSiparisGonder.Visible = true;
                imgTopluSiparis.Visible = true;
                grdTaleplerimiz.Columns[1].Visible = false;
                btnTumunuOnayla.Visible = false;
                btnSiparisKapat.Visible = true;
            }
            else
            {
                imgParcaliSiparisGonder.Visible = false;
                btnSiparisKapat.Visible = false;
                imgTopluSiparis.Visible = false;
                btnBeklemeyeAl.Visible = false;
                btnTalepGuncelle.Visible = true;
                btnTumunuOnayla.Visible = false;
            }

            #region Sipariş Açılmış

            Datasource = "SELECT T.TalepID,T.EvrakNoTarih+T.EvrakNo AS EvrakNo, " +
                    "CONVERT(VARCHAR(10),CONVERT(DATETIME,Tarih-2),104) AS Tarih, " +
                    "T.Aciklama,T.MalKodu,T.MalAdi,CONVERT(NUMERIC(18,2),T.Miktar) AS Miktar,T.Birim, " +
                    "ISNULL((SELECT ((STK004_GirisMiktari+STK004_DevirMiktari)-STK004_CikisMiktari) AS StokMiktar " +
                    "FROM  " + LinkVeritabanı.ToString() + "." + LinkVeritabanı.ToString() + ".STK004  " +
                    "WHERE STK004_MalKodu=T.MalKodu),'0') AS StokMiktari, " +
                    "T.SipMiktar,TM.TeminAdi AS TeminYeri,T.OnayDurumu,T.AdSoyad,K.Yetki,T.MasrafMerkeziAdi AS KullanilacakYer, " +
                    "(CASE WHEN T.Kaydeden = '" + KullaniciKodumuz.ToString() + "' THEN " +
                    "((CASE T.ilkIslem WHEN 1 THEN 0 " +
                    "WHEN 0 THEN 1 END)) ELSE 0 END) AS ilkIslem, " +
                    "(CASE  " +
                    "WHEN T.OnaylayanYetki = " + Convert.ToInt32(Yetkili) + " THEN 1  " +
                    "ELSE 0 " +
                    "END) AS OnayDurum " +
                    "FROM Tlp AS T " +
                    "INNER JOIN TeminYeri AS TM ON TM.TeminKodu=T.TeminYeri " +
                    "INNER JOIN Kullanicilar AS K ON T.Kaydeden=K.KullaniciKodu " +
                    "WHERE " + WhereSorgu +
                    "ORDER BY T.TalepID DESC";

            #endregion
        }
        else if (rdTalepler.SelectedIndex == 5)
        {
            Sayfaliyoruz = 5;
            Session["SayfaDegeri"] = "5";

            OnayDurum = Session["SatinAlmaKodu"].ToString();
            string OnayDurum2 = Session["AmbarKodu"].ToString();
            btnDagit.Visible = AnaDepartmanimiz;
            btnAmbaraGonder.Visible = SiparisButonu;
            btnSiparisKapat.Visible = false;

            if (SiparisButonu == false)
            {
                btnTalepGuncelle.Visible = true;
                btnTumunuOnayla.Visible = true;
                grdTaleplerimiz.Columns[1].Visible = true;
            }
            else
            {
                btnTalepGuncelle.Visible = true;
                btnTumunuOnayla.Visible = true;
                grdTaleplerimiz.Columns[1].Visible = true;
            }

            if (SiparisButonu == false && SatinAlmaButonu == false)
            {
                if (DepartmanYetkilisi == Convert.ToInt32(Yetkili))
                {
                    if (YetkiUnvanimiz == "Şef")
                    {
                        WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") " +
                                            "AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                                            "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ") AND (T.OnayDurumu IN(1)) ";
                    }
                    else if (YetkiUnvanimiz == "Müdür")
                    {
                        WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") " +
                    "AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                    "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ") AND (T.OnayDurumu IN(2)) ";
                    }
                }
                else
                {
                    if (Convert.ToInt32(AltDepartmanKodu) == 0)
                    {
                        WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                                "AND K.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                                "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ") AND (T.OnayDurumu IN(1,2)) ";
                    }
                    else
                    {
                        if (Convert.ToInt32(BagliOlduguDepartman) == 0)
                        {
                            WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                            "AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                            "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  " +
                            "AND (K.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " OR K.BagliAltDepartman=" + Convert.ToInt32(AltDepartmanKodu) + ") " +
                            "AND (T.OnayDurumu IN(1,2)) ";
                        }
                        else
                        {
                            WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                                "AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                                "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  " +
                                "AND (T.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + ") AND (T.OnayDurumu IN(1,2)) ";
                        }
                    }
                }
            }
            else if (SiparisButonu == true)
            {
                WhereSorgu = "(T.GidecekDepartman IN (" + OnayDurum.ToString() + ")) ";
            }
            else if (SatinAlmaButonu == true)
            {
                WhereSorgu = "(T.GidecekDepartman IN (" + OnayDurum2.ToString() + ")) ";
            }

            #region Beklemede

            Datasource = "SELECT T.TalepID,T.EvrakNoTarih+T.EvrakNo AS EvrakNo, " +
                    "CONVERT(VARCHAR(10),CONVERT(DATETIME,Tarih-2),104) AS Tarih, " +
                    "T.Aciklama,T.MalKodu,T.MalAdi,CONVERT(NUMERIC(18,2),T.Miktar) AS Miktar,T.Birim, " +
                    "ISNULL((SELECT ((STK004_GirisMiktari+STK004_DevirMiktari)-STK004_CikisMiktari) AS StokMiktar " +
                    "FROM  " + LinkVeritabanı.ToString() + "." + LinkVeritabanı.ToString() + ".STK004  " +
                    "WHERE STK004_MalKodu=T.MalKodu),'0') AS StokMiktari, " +
                    "T.SipMiktar,TM.TeminAdi AS TeminYeri,T.OnayDurumu,T.AdSoyad,K.Yetki,T.MasrafMerkeziAdi AS KullanilacakYer, " +
                    "(CASE WHEN T.Kaydeden = '" + KullaniciKodumuz.ToString() + "' THEN " +
                    "((CASE T.ilkIslem WHEN 1 THEN 0 " +
                    "WHEN 0 THEN 1 END)) ELSE 0 END) AS ilkIslem, " +
                    "(CASE  " +
                    "WHEN T.OnaylayanYetki = " + Convert.ToInt32(Yetkili) + " THEN 1  " +
                    "ELSE 0 " +
                    "END) AS OnayDurum " +
                    "FROM Tlp AS T " +
                    "INNER JOIN TeminYeri AS TM ON TM.TeminKodu=T.TeminYeri " +
                    "INNER JOIN Kullanicilar AS K ON T.Kaydeden=K.KullaniciKodu " +
                    "WHERE " + WhereSorgu + " " +
                    "ORDER BY T.TalepID DESC";


            #endregion
        }
        else if (rdTalepler.SelectedIndex == 6)
        {
            Sayfaliyoruz = 6;
            Session["SayfaDegeri"] = "6";

            OnayDurum = Session["SatinAlmaKodu"].ToString();
            string OnayDurum2 = Session["AmbarKodu"].ToString();

            btnDagit.Visible = false;
            btnSatinAlmayaGonder.Visible = false;
            btnTumunuOnayla.Visible = false;
            btnBeklemeyeAl.Visible = false;
            btnTalepGuncelle.Visible = true;
            grdTaleplerimiz.Columns[1].Visible = false;
            btnAmbaraGonder.Visible = false;
            btnSiparisKapat.Visible = false;

            if (SiparisButonu == false && SatinAlmaButonu == false)
            {
                if (DepartmanYetkilisi == Convert.ToInt32(Yetkili))
                {
                    if (YetkiUnvanimiz == "Şef")
                    {
                        WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") " +
                                            "AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                                            "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ") AND (T.OnayDurumu IN(9)) ";
                    }
                    else if (YetkiUnvanimiz == "Müdür")
                    {
                        WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") " +
                    "AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                    "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ") AND (T.OnayDurumu IN(10,11,12)) ";
                    }
                }
                else
                {
                    if (Convert.ToInt32(AltDepartmanKodu) == 0)
                    {
                        WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                                "AND K.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                                "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ") ";
                    }
                    else
                    {
                        if (Convert.ToInt32(BagliOlduguDepartman) == 0)
                        {
                            WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                            "AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                            "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  " +
                            "AND (K.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " OR K.BagliAltDepartman=" + Convert.ToInt32(AltDepartmanKodu) + ") " +
                            "AND (T.OnayDurumu IN(11,12)) ";
                        }
                        else
                        {
                            WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                                "AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                                "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  " +
                                "AND T.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " AND (T.OnayDurumu IN(11,12)) ";
                        }
                    }
                }
            }
            else if (SiparisButonu == true)
            {
                WhereSorgu = "(T.GidecekDepartman IN (" + OnayDurum.ToString() + ")) AND (T.OnayDurumu IN (11))";
            }
            else if (SatinAlmaButonu == true)
            {
                WhereSorgu = "(T.GidecekDepartman IN (" + OnayDurum2.ToString() + ")) AND (T.OnayDurumu IN (12))";
            }

            #region Red Edilenler

            Datasource = "SELECT T.TalepID,T.EvrakNoTarih+T.EvrakNo AS EvrakNo, " +
                    "CONVERT(VARCHAR(10),CONVERT(DATETIME,Tarih-2),104) AS Tarih, " +
                    "T.Aciklama,T.MalKodu,T.MalAdi,CONVERT(NUMERIC(18,2),T.Miktar) AS Miktar,T.Birim, " +
                    "ISNULL((SELECT ((STK004_GirisMiktari+STK004_DevirMiktari)-STK004_CikisMiktari) AS StokMiktar " +
                    "FROM  " + LinkVeritabanı.ToString() + "." + LinkVeritabanı.ToString() + ".STK004  " +
                    "WHERE STK004_MalKodu=T.MalKodu),'0') AS StokMiktari, " +
                    "T.SipMiktar,TM.TeminAdi AS TeminYeri,T.OnayDurumu,T.AdSoyad,K.Yetki,T.MasrafMerkeziAdi AS KullanilacakYer, " +
                    "(CASE WHEN T.Kaydeden = '" + KullaniciKodumuz.ToString() + "' THEN " +
                    "((CASE T.ilkIslem WHEN 1 THEN 0 " +
                    "WHEN 0 THEN 1 END)) ELSE 0 END) AS ilkIslem, " +
                    "(CASE  " +
                    "WHEN T.OnaylayanYetki = " + Convert.ToInt32(Yetkili) + " THEN 1  " +
                    "ELSE 0 " +
                    "END) AS OnayDurum " +
                    "FROM Tlp AS T " +
                    "INNER JOIN TeminYeri AS TM ON TM.TeminKodu=T.TeminYeri " +
                    "INNER JOIN Kullanicilar AS K ON T.Kaydeden=K.KullaniciKodu " +
                    "WHERE " + WhereSorgu +
                    "ORDER BY T.TalepID DESC";

            #endregion

        }
        else if (rdTalepler.SelectedIndex == 7)
        {
            Sayfaliyoruz = 7;
            Session["SayfaDegeri"] = "7";

            btnTalepGuncelle.Visible = true;
            btnSatinAlmayaGonder.Visible = false;
            btnDagit.Visible = false;
            btnTumunuOnayla.Visible = false;
            btnAmbaraGonder.Visible = false;
            btnSiparisKapat.Visible = false;
            OnayDurum = Session["SatinAlmaKodu"].ToString();
            string OnayDurum2 = Session["AmbarKodu"].ToString();

            if (SiparisButonu == false && SatinAlmaButonu == false)
            {
                if (DepartmanYetkilisi == Convert.ToInt32(Yetkili))
                {
                    WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnayDurumu IN(4)) " +
                    "AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  ";
                }
                else
                {
                    if (Convert.ToInt32(AltDepartmanKodu) == 0)
                    {
                        WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                                "AND K.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                                "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ") ";
                    }
                    else
                    {
                        if (Convert.ToInt32(BagliOlduguDepartman) == 0)
                        {
                            WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                            "AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                            "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  " +
                            "AND (K.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " OR K.BagliAltDepartman=" + Convert.ToInt32(AltDepartmanKodu) + ") " +
                            "AND (T.OnayDurumu IN(4)) ";
                        }
                        else
                        {
                            WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                                "AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                                "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  " +
                                "AND T.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " AND (T.OnayDurumu IN(4)) ";
                        }
                    }
                }
            }
            else if (SiparisButonu == true)
            {
                WhereSorgu = "(T.GidecekDepartman IN (" + OnayDurum.ToString() + ")) AND (T.OnayDurumu = 4)";
            }
            else if (SatinAlmaButonu == true)
            {
                WhereSorgu = "(T.GidecekDepartman IN (" + OnayDurum2.ToString() + ")) AND (T.OnayDurumu = 4)";
            }

            #region Temine Hazır İşlemler

            Datasource = "SELECT T.TalepID,T.EvrakNoTarih+T.EvrakNo AS EvrakNo, " +
                    "CONVERT(VARCHAR(10),CONVERT(DATETIME,Tarih-2),104) AS Tarih, " +
                    "T.Aciklama,T.MalKodu,T.MalAdi,CONVERT(NUMERIC(18,2),T.Miktar) AS Miktar,T.Birim, " +
                    "ISNULL((SELECT ((STK004_GirisMiktari+STK004_DevirMiktari)-STK004_CikisMiktari) AS StokMiktar " +
                    "FROM  " + LinkVeritabanı.ToString() + "." + LinkVeritabanı.ToString() + ".STK004  " +
                    "WHERE STK004_MalKodu=T.MalKodu),'0') AS StokMiktari, " +
                    "T.SipMiktar,TM.TeminAdi AS TeminYeri,T.OnayDurumu,T.AdSoyad,K.Yetki,T.MasrafMerkeziAdi AS KullanilacakYer, " +
                    "(CASE WHEN T.Kaydeden = '" + KullaniciKodumuz.ToString() + "' THEN " +
                    "((CASE T.ilkIslem WHEN 1 THEN 0 " +
                    "WHEN 0 THEN 1 END)) ELSE 0 END) AS ilkIslem, " +
                    "(CASE  " +
                    "WHEN T.OnaylayanYetki = " + Convert.ToInt32(Yetkili) + " THEN 1  " +
                    "ELSE 0 " +
                    "END) AS OnayDurum " +
                    "FROM Tlp AS T " +
                    "INNER JOIN TeminYeri AS TM ON TM.TeminKodu=T.TeminYeri " +
                    "INNER JOIN Kullanicilar AS K ON T.Kaydeden=K.KullaniciKodu " +
                    "WHERE " + WhereSorgu +
                    "ORDER BY T.TalepID DESC";

            #endregion

        }
        else if (rdTalepler.SelectedIndex == 8)
        {
            Sayfaliyoruz = 8;
            Session["SayfaDegeri"] = "8";

            grdTaleplerimiz.Columns[1].Visible = false;
            btnTalepGuncelle.Visible = true;
            btnSatinAlmayaGonder.Visible = false;
            btnTumunuOnayla.Visible = false;
            btnDagit.Visible = false;
            btnAmbaraGonder.Visible = false;
            btnSiparisKapat.Visible = false;
            OnayDurum = Session["SatinAlmaKodu"].ToString();
            string OnayDurum2 = Session["AmbarKodu"].ToString();

            if (SiparisButonu == false && SatinAlmaButonu == false)
            {
                if (DepartmanYetkilisi == Convert.ToInt32(Yetkili))
                {
                    WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnayDurumu IN(13)) " +
                    "AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  ";
                }
                else
                {
                    if (Convert.ToInt32(AltDepartmanKodu) == 0)
                    {
                        WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                                "AND K.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                                "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ") ";
                    }
                    else
                    {
                        if (Convert.ToInt32(BagliOlduguDepartman) == 0)
                        {
                            WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                            "AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                            "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  " +
                            "AND (K.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " OR K.BagliAltDepartman=" + Convert.ToInt32(AltDepartmanKodu) + ") " +
                            "AND (T.OnayDurumu IN(13)) ";
                        }
                        else
                        {
                            WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                                "AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                                "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  " +
                                "AND T.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " AND (T.OnayDurumu IN(13)) ";
                        }
                    }
                }
            }
            else if (SiparisButonu == true)
            {
                WhereSorgu = "(T.GidecekDepartman IN (" + OnayDurum.ToString() + ")) AND (T.OnayDurumu = 13)";
            }
            else if (SatinAlmaButonu == true)
            {
                WhereSorgu = "(T.GidecekDepartman IN (" + OnayDurum2.ToString() + ")) AND (T.OnayDurumu = 13)";
            }

            #region Kapalı

            Datasource = "SELECT T.TalepID,T.EvrakNoTarih+T.EvrakNo AS EvrakNo, " +
                    "CONVERT(VARCHAR(10),CONVERT(DATETIME,Tarih-2),104) AS Tarih, " +
                    "T.Aciklama,T.MalKodu,T.MalAdi,CONVERT(NUMERIC(18,2),T.Miktar) AS Miktar,T.Birim, " +
                    "ISNULL((SELECT ((STK004_GirisMiktari+STK004_DevirMiktari)-STK004_CikisMiktari) AS StokMiktar " +
                    "FROM  " + LinkVeritabanı.ToString() + "." + LinkVeritabanı.ToString() + ".STK004  " +
                    "WHERE STK004_MalKodu=T.MalKodu),'0') AS StokMiktari, " +
                    "T.SipMiktar,TM.TeminAdi AS TeminYeri,T.OnayDurumu,T.AdSoyad,K.Yetki,T.MasrafMerkeziAdi AS KullanilacakYer, " +
                    "(CASE WHEN T.Kaydeden = '" + KullaniciKodumuz.ToString() + "' THEN " +
                    "((CASE T.ilkIslem WHEN 1 THEN 0 " +
                    "WHEN 0 THEN 1 END)) ELSE 0 END) AS ilkIslem, " +
                    "(CASE  " +
                    "WHEN T.OnaylayanYetki = " + Convert.ToInt32(Yetkili) + " THEN 1  " +
                    "ELSE 0 " +
                    "END) AS OnayDurum " +
                    "FROM Tlp AS T " +
                    "INNER JOIN TeminYeri AS TM ON TM.TeminKodu=T.TeminYeri " +
                    "INNER JOIN Kullanicilar AS K ON T.Kaydeden=K.KullaniciKodu " +
                    "WHERE " + WhereSorgu +
                    "ORDER BY T.TalepID DESC";

            #endregion

        }
        else if (rdTalepler.SelectedIndex == 9)
        {
            Sayfaliyoruz = 9;
            Session["SayfaDegeri"] = "9";

            OnayDurum = Session["SatinAlmaKodu"].ToString();
            btnDagit.Visible = false;
            btnSiparisKapat.Visible = false;

            if (SiparisButonu == false)
            {
                if (DepartmanYetkilisi == Convert.ToInt32(Yetkili))
                {
                    WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnayDurumu IN(8)) " +
                    "AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  ";
                }
                else
                {
                    if (Convert.ToInt32(AltDepartmanKodu) == 0)
                    {
                        WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                                "AND K.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                                "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ") ";
                    }
                    else
                    {
                        if (Convert.ToInt32(BagliOlduguDepartman) == 0)
                        {
                            WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                            "AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                            "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  " +
                            "AND (K.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " OR K.BagliAltDepartman=" + Convert.ToInt32(AltDepartmanKodu) + ") " +
                            "AND (T.OnayDurumu IN(8)) ";
                        }
                        else
                        {
                            WhereSorgu = "T.DepartmanKodu='" + DepoKodu + "' " +
                                "AND (T.Yetki<=" + Convert.ToInt32(Yetkili) + ") AND (T.OnaylayanYetki=" + Convert.ToInt32(Yetkili) + " " +
                                "OR T.OnaylayanYetki<=" + Convert.ToInt32(Yetkili) + " OR T.OnaylayanYetki>=" + OnayYetkiDurumu + ")  " +
                                "AND T.AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " AND (T.OnayDurumu IN(8)) ";
                        }
                    }
                }
            }
            else
            {
                WhereSorgu = "(T.GidecekDepartman IN (" + OnayDurum.ToString() + ")) AND (T.OnayDurumu = 8)";
            }

            if (SiparisButonu == false)
            {
                btnBeklemeyeAl.Visible = false;
                btnTalepGuncelle.Visible = true;
                btnTumunuOnayla.Visible = false;
                btnAmbaraGonder.Visible = SiparisButonu;
            }
            else
            {
                grdTaleplerimiz.Columns[1].Visible = true;
                btnBeklemeyeAl.Visible = false;
                btnTalepGuncelle.Visible = true;
                btnTumunuOnayla.Visible = true;
                btnAmbaraGonder.Visible = true;
            }

            #region Beklemeye Alındı

            Datasource = "SELECT T.TalepID,T.EvrakNoTarih+T.EvrakNo AS EvrakNo, " +
                    "CONVERT(VARCHAR(10),CONVERT(DATETIME,Tarih-2),104) AS Tarih, " +
                    "T.Aciklama,T.MalKodu,T.MalAdi,CONVERT(NUMERIC(18,2),T.Miktar) AS Miktar,T.Birim, " +
                    "ISNULL((SELECT ((STK004_GirisMiktari+STK004_DevirMiktari)-STK004_CikisMiktari) AS StokMiktar " +
                    "FROM  " + LinkVeritabanı.ToString() + "." + LinkVeritabanı.ToString() + ".STK004  " +
                    "WHERE STK004_MalKodu=T.MalKodu),'0') AS StokMiktari, " +
                    "T.SipMiktar,TM.TeminAdi AS TeminYeri,T.OnayDurumu,T.AdSoyad,K.Yetki,T.MasrafMerkeziAdi AS KullanilacakYer, " +
                    "(CASE WHEN T.Kaydeden = '" + KullaniciKodumuz.ToString() + "' THEN " +
                    "((CASE T.ilkIslem WHEN 1 THEN 0 " +
                    "WHEN 0 THEN 1 END)) ELSE 0 END) AS ilkIslem, " +
                    "(CASE  " +
                    "WHEN T.OnaylayanYetki = " + Convert.ToInt32(Yetkili) + " THEN 1  " +
                    "ELSE 0 " +
                    "END) AS OnayDurum " +
                    "FROM Tlp AS T " +
                    "INNER JOIN TeminYeri AS TM ON TM.TeminKodu=T.TeminYeri " +
                    "INNER JOIN Kullanicilar AS K ON T.Kaydeden=K.KullaniciKodu " +
                    "WHERE " + WhereSorgu +
                    "ORDER BY T.TalepID DESC";

            #endregion
        }

        Session["TalepDurum"] = Datasource;
        dsSorgu = Datasource;
        grdTaleplerimiz.DataSource = datadoldur(dsSorgu);
        grdTaleplerimiz.DataBind();

        for (int j = 0; j < grdTaleplerimiz.Rows.Count; j++)
        {
            string ParcaliSipD = grdTaleplerimiz.Rows[j].Cells[12].Text.ToString();

            if (ParcaliSipD == "14")
            {
                grdTaleplerimiz.Rows[j].BackColor = Color.LightGreen;
            }
        }

        for (int i = 0; i < grdTaleplerimiz.Rows.Count; i++)
        {
            string deneme = OnayDurumKontrolEdiyoruz(Convert.ToInt32(Yetkili), grdTaleplerimiz.Rows[i].Cells[12].Text.ToString());
            grdTaleplerimiz.Rows[i].Cells[12].Text = deneme;
        }

        #region Gridde Miktarı Renklendiriyoruz

        for (int i = 0; i < grdTaleplerimiz.Rows.Count; i++)
        {
            string OnayDurumGrid = grdTaleplerimiz.Rows[i].Cells[12].Text.ToString();
            string Bekleme = "Bekleniyor";
            string Temin = "Temine Hazır";
            string SipA = "Sipariş";
            string Talepislemi = "Talep Beklemeye";
            string Ambar = "Gönderildi";
            string Red = "Onaylanmadı";
            string Teklif = "Teklif";

            if (OnayDurumGrid.IndexOf(Teklif) != -1)
            {
                grdTaleplerimiz.Rows[i].Cells[12].ForeColor = Color.Brown;
                grdTaleplerimiz.Rows[i].Cells[12].Font.Bold = true;
            }
            if (OnayDurumGrid.IndexOf(Red) != -1)
            {
                grdTaleplerimiz.Rows[i].Cells[12].ForeColor = Color.Red;
                grdTaleplerimiz.Rows[i].Cells[12].Font.Bold = true;
            }
            else if (OnayDurumGrid.IndexOf(Ambar) != -1)
            {
                grdTaleplerimiz.Rows[i].Cells[12].ForeColor = Color.Orange;
                grdTaleplerimiz.Rows[i].Cells[12].Font.Bold = true;
            }
            else if (OnayDurumGrid.IndexOf(Bekleme) != -1)
            {
                grdTaleplerimiz.Rows[i].Cells[12].ForeColor = Color.Orange;
                grdTaleplerimiz.Rows[i].Cells[12].Font.Bold = true;
            }
            else if (OnayDurumGrid.IndexOf(Temin) != -1)
            {
                grdTaleplerimiz.Rows[i].Cells[12].ForeColor = Color.Green;
                grdTaleplerimiz.Rows[i].Cells[12].Font.Bold = true;
            }
            else if (OnayDurumGrid.IndexOf(SipA) != -1)
            {
                grdTaleplerimiz.Rows[i].Cells[12].ForeColor = Color.Black;
                grdTaleplerimiz.Rows[i].Cells[12].Font.Bold = true;
            }
            else if (OnayDurumGrid.IndexOf(Talepislemi) != -1)
            {
                grdTaleplerimiz.Rows[i].Cells[12].ForeColor = Color.LightBlue;
                grdTaleplerimiz.Rows[i].Cells[12].Font.Bold = true;
            }

            if (SiparisButonu == true)
            {

                decimal gridMiktar = Convert.ToDecimal(grdTaleplerimiz.Rows[i].Cells[6].Text.ToString());
                decimal gridSiparisMiktar = Convert.ToDecimal(grdTaleplerimiz.Rows[i].Cells[9].Text.ToString());
                grdTaleplerimiz.Columns[8].Visible = false;

                if (gridMiktar > gridSiparisMiktar)
                {
                    grdTaleplerimiz.Rows[i].Cells[6].ForeColor = Color.Red;
                    grdTaleplerimiz.Rows[i].Cells[6].Font.Bold = true;
                    grdTaleplerimiz.Rows[i].Cells[6].ForeColor = Color.Red;
                    grdTaleplerimiz.Rows[i].Cells[6].Font.Bold = true;
                }

            }
            else
            {
                decimal gridMiktar = Convert.ToDecimal(grdTaleplerimiz.Rows[i].Cells[6].Text.ToString());
                string dddd = grdTaleplerimiz.Rows[i].Cells[8].Text.ToString();
                decimal gridStokMiktar = Convert.ToDecimal(grdTaleplerimiz.Rows[i].Cells[8].Text.ToString());
                grdTaleplerimiz.Columns[9].Visible = false;

                if (gridMiktar > gridStokMiktar)
                {
                    grdTaleplerimiz.Rows[i].Cells[6].ForeColor = Color.Red;
                    grdTaleplerimiz.Rows[i].Cells[6].Font.Bold = true;

                    grdTaleplerimiz.Rows[i].Cells[8].ForeColor = Color.Red;
                    grdTaleplerimiz.Rows[i].Cells[8].Font.Bold = true;
                }
            }
        }

        #endregion

        if (rdTalepler.SelectedIndex == 1)
        {
            for (int i = 0; i < grdTaleplerimiz.Rows.Count; i++)
            {
                GridViewRow row = grdTaleplerimiz.Rows[i];
                bool isChecked = ((HtmlInputCheckBox)row.FindControl("chk")).Checked;
                HtmlInputCheckBox chk = ((HtmlInputCheckBox)row.FindControl("chk"));

                int ParcaliTalepID = Convert.ToInt32(chk.Value.ToString());
                string EvrakNo = EvrakNoSonuc(Convert.ToInt32(chk.Value.ToString()));
                string SipMalKodu = MalKoduBul(EvrakNo);
                bool SipVisDurum = SaticiDurumKontrol(SipMalKodu);
                grdTaleplerimiz.Rows[i].Cells[14].Visible = SipVisDurum;
            }
        }
        if (rdTalepler.SelectedIndex == 4)
        {
            for (int i = 0; i < grdTaleplerimiz.Rows.Count; i++)
            {
                GridViewRow row = grdTaleplerimiz.Rows[i];
                bool isChecked = ((HtmlInputCheckBox)row.FindControl("chkSiparis")).Checked;
                HtmlInputCheckBox chk = ((HtmlInputCheckBox)row.FindControl("chkSiparis"));

                int ParcaliTalepID = Convert.ToInt32(chk.Value.ToString());
                string EvrakNo = EvrakNoSonuc(Convert.ToInt32(chk.Value.ToString()));
                string SipMalKodu = MalKoduBul(EvrakNo);
                bool SipVisDurum = SaticiDurumKontrol(SipMalKodu);
                bool ParcaliSipDurum = SiparisOnayDurumBul(ParcaliTalepID);

                if (SiparisButonu == true)
                {
                    grdTaleplerimiz.Columns[14].Visible = true;
                    //grdTaleplerimiz.Columns[13].Visible = ParcaliSipDurum;
                    if (ParcaliSipDurum == true)
                    {
                        //grdTaleplerimiz.Columns[13].Visible = ParcaliSipDurum;
                        grdTaleplerimiz.Rows[i].Cells[14].Visible = ParcaliSipDurum;
                    }
                    else if (ParcaliSipDurum == false)
                    {
                        //grdTaleplerimiz.Columns[13].Visible = false;
                        grdTaleplerimiz.Rows[i].Cells[14].Visible = false;
                    }
                }
                else
                {
                    grdTaleplerimiz.Columns[14].Visible = false;
                }
            }
        }

        ConnectionKapat();

    }

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

    private string OnayDurumKontrol(int YetkiKodu)
    {
        if (Kullanicilar.State == ConnectionState.Closed)
            Kullanicilar.Open();

        string OnaySorgu = "SELECT Yetki_Unvani+' Onayı Bekliyor' AS OnayDurum FROM Yetki_Unvanlari " +
                        "WHERE YetkiKodu=" + YetkiKodu + "";

        cmd = new SqlCommand(OnaySorgu, Kullanicilar);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();
    }

    private void YetkiKoduBul(string DepartmanKodu, string KullaniciAdi, string YetkiKodumuz, string AltDepartmanKodu)
    {
        if (DbConnYetkiKoduBul.State == ConnectionState.Closed)
            DbConnYetkiKoduBul.Open();

        string sorgu = "SELECT ISNULL(MIN(Yetki),0) AS Yetki FROM Kullanicilar AS K " +
                        "INNER JOIN Departmanlar AS D ON K.DepartmanID=D.DepartmanID " +
                        "WHERE D.DepartmanKodu='" + DepartmanKodu.ToString() + "'  AND AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " " +
                        "AND KullaniciAdi <> '" + KullaniciAdi.ToString() + "' " +
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

    private void YetkiKoduBul2(string DepartmanKodu, string KullaniciAdi, string YetkiKodumuz)
    {
        if (DbConnYetkiKoduBul.State == ConnectionState.Closed)
            DbConnYetkiKoduBul.Open();

        string sorgu = "SELECT ISNULL(MIN(Yetki),0) AS Yetki FROM Kullanicilar AS K " +
                        "INNER JOIN Departmanlar AS D ON K.DepartmanID=D.DepartmanID " +
                        "WHERE D.DepartmanKodu='" + DepartmanKodu.ToString() + "' " +
                        "AND KullaniciAdi <> '" + KullaniciAdi.ToString() + "' " +
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

    private string DepartmanOnayKontrol(string DepartmanKodu)
    {
        if (Kullanicilar.State == ConnectionState.Closed)
            Kullanicilar.Open();

        string DepartmanSorgu = "SELECT DepartmanAdi+' Onayı Bekliyor' AS OnayDurum FROM Departmanlar " +
                            "WHERE DepartmanKodu='" + DepartmanKodu.ToString() + "'";
        cmd = new SqlCommand(DepartmanSorgu, Kullanicilar);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();
    }

    private void Siparisler()
    {
        for (int i = 0; i < grdTaleplerimiz.Rows.Count; i++)
        {
            GridViewRow row = grdTaleplerimiz.Rows[i];
            bool isChecked = ((HtmlInputCheckBox)row.FindControl("chkSiparis")).Checked;
            HtmlInputCheckBox chk = ((HtmlInputCheckBox)row.FindControl("chkSiparis"));

            if (isChecked)
            {

                string TalepID = chk.Value.ToString() + ",";
                string EvrakNo = EvrakNoSonuc(Convert.ToInt32(chk.Value.ToString()));
                string MalAdi = MalAdiSonuc(Convert.ToInt32(chk.Value.ToString())) + "}";
                string AciklamaSatirimiz = AciklamaBuluyoruz(Convert.ToInt32(chk.Value.ToString())) + "}";
                string KontrolMalAdi = MalAdiSonuc(Convert.ToInt32(chk.Value.ToString()));
                string Miktar = grdTaleplerimiz.Rows[i].Cells[6].Text.ToString() + "/";
                string MalKodumuz = MalKoduBul(EvrakNo, KontrolMalAdi.Replace("'", "''")) + ",";
                string KdvMalKodu = MalKoduBul(EvrakNo, KontrolMalAdi.Replace("'", "''"));
                string Kdv = KdvBul(KdvMalKodu) + "/";

                string ihtiyacNo = EvrakNoSonuc(Convert.ToInt32(chk.Value.ToString())) + ",";
                Miktar2 += Miktar;
                ihtiyacNo2 += ihtiyacNo;
                MalKodumuz2 += MalKodumuz;
                TalepID2 += TalepID;
                Kdv2 += Kdv;
                MalAdlari2 += MalAdi;
                AciklamaSatirimiz2 += AciklamaSatirimiz;
            }
        }

        if (ihtiyacNo2.Length != 0)
        {
            int num = ihtiyacNo2.Length;
            int num2 = MalKodumuz2.Length;
            int num3 = Miktar2.Length;
            int num4 = Kdv2.Length;
            int num5 = TalepID2.Length;
            int num6 = MalAdlari2.Length;
            int num7 = AciklamaSatirimiz2.Length;
            ihtiyacNo2 = ihtiyacNo2.ToString().Substring(0, num - 1);
            MalKodumuz2 = MalKodumuz2.ToString().Substring(0, num2 - 1);
            Miktar2 = Miktar2.ToString().Substring(0, num3 - 1);
            Kdv2 = Kdv2.ToString().Substring(0, num4 - 1);
            TalepID2 = TalepID2.ToString().Substring(0, num5 - 1);
            MalAdlari2 = MalAdlari2.ToString().Substring(0, num6 - 1);
            AciklamaSatirimiz2 = AciklamaSatirimiz2.ToString().Substring(0, num7 - 1);

            Session["MalAdlari"] = MalAdlari2;
            Session["TalepEvrakNumaralari"] = ihtiyacNo2;
            Session["MalKodlari"] = MalKodumuz2;
            Session["Miktarlar"] = Miktar2;
            Session["Kdvmiz"] = Kdv2;
            Session["TalepIDler"] = TalepID2;
            Session["AciklamaSatirlarimiz"] = AciklamaSatirimiz2;
        }

        grdTaleplerimiz.Columns[14].Visible = SiparisButonu;
        imgParcaliSiparisGonder.Visible = SiparisButonu;
        imgTopluSiparis.Visible = SiparisButonu;
        btnAmbaraGonder.Visible = false;
        btnBeklemeyeAl.Visible = false;
    }

    private string MalKoduBul(string EvrakNo, string MalAdi)
    {
        if (Kullanicilar.State == ConnectionState.Closed)
            Kullanicilar.Open();

        string sorgu = "SELECT MalKodu FROM Tlp " +
                       "WHERE EvrakNoTarih+EvrakNo='" + EvrakNo + "' AND MalAdi='" + MalAdi.ToString() + "'";
        cmd = new SqlCommand(sorgu, Kullanicilar);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();
    }

    private string KdvBul(string MalKodu)
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        string sorgu = "SELECT (CASE STK004_KDV " +
                    "WHEN 1 THEN '0.00' " +
                    "WHEN 2 THEN '1.00' " +
                    "WHEN 3 THEN '8.00' " +
                    "WHEN 4 THEN '18.00' END) AS Kdv FROM STK004 " +
                    "WHERE STK004_MalKodu='" + MalKodu.ToString() + "'";
        cmd = new SqlCommand(sorgu, DbConnLink);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();
    }

    protected void imgParcaliSiparisGonder_Click(object sender, ImageClickEventArgs e)
    {
        Session["MalAdlari"] = null;
        Session["TalepEvrakNumaralari"] = null;
        Session["MalKodlari"] = null;
        Session["Miktarlar"] = null;
        Session["Kdvmiz"] = null;
        Session["TalepIDler"] = null;
        Session["AciklamaSatirlarimiz"] = null;
        Siparisler();
    }

    private bool SiparisButonDurumu(string KullaniciKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT Siparis FROM Kullanicilar " +
                    "WHERE KullaniciKodu='" + KullaniciKodu.ToString() + "'";
        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (bool)cmd.ExecuteScalar();
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

    protected void imgTopluSiparis_Click(object sender, ImageClickEventArgs e)
    {
        Session["MalAdlari"] = null;
        Session["TalepEvrakNumaralari"] = null;
        Session["MalKodlari"] = null;
        Session["Miktarlar"] = null;
        Session["Kdvmiz"] = null;
        Session["TalepIDler"] = null;
        Session["AciklamaSatirlarimiz"] = null;
        Siparisler();
    }

    private string SiparisNumarasiBul(string EvrakNo)
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        string sorgu = "SELECT TOP(1) STK002_EvrakSeriNo AS SiparisNo FROM STK002 " +
                        "WHERE STK002_MalSeriNo='" + EvrakNo + "'";

        cmdSiparisBul = new SqlCommand(sorgu, DbConnLink);
        cmdSiparisBul.CommandTimeout = 120;
        return (string)cmdSiparisBul.ExecuteScalar();
    }

    private int SiparisDurum(string SiparisNo)
    {
        if (DbConnIrsaliyeDurumu.State == ConnectionState.Closed)
            DbConnIrsaliyeDurumu.Open();

        string sorgu = "SELECT COUNT(STK005_EvrakSeriNo) AS Durum FROM STK005 " +
                    "WHERE STK005_MalSeriNo='" + SiparisNo.ToString() + "'";

        cmdIrsaliyeDurumu = new SqlCommand(sorgu, DbConnIrsaliyeDurumu);
        cmdIrsaliyeDurumu.CommandTimeout = 120;
        return (int)cmdIrsaliyeDurumu.ExecuteScalar();
    }

    private void TalepIrsaliyeDurumGuncelle(string Onaylayan, string EvrakNo)
    {
        if (DbConnTalepIrsaliyeGuncelle.State == ConnectionState.Closed)
            DbConnTalepIrsaliyeGuncelle.Open();

        string GidecekDeptKod = AnaDepartmanKodu();

        string sorgu = "UPDATE Tlp SET " +
                    "GidecekDepartman='" + GidecekDeptKod + "', " +
                    "IrsaliyeDurum=1, " +
                    "SiparisDurum=2, " +
                    "OnayDurumu='4', " +
                    "Onaylayan='" + Onaylayan.ToString() + "', " +
                    "OnaylayanYetki=" + Convert.ToInt32(Session["Yetki"].ToString()) + " , " +
                    "ilkIslem=1 " +
                    "WHERE EvrakNoTarih+EvrakNo='" + EvrakNo.ToString() + "'";

        cmdTalepIrsaliyeGuncelle = new SqlCommand(sorgu, DbConnTalepIrsaliyeGuncelle);
        cmdTalepIrsaliyeGuncelle.CommandTimeout = 120;
        cmdTalepIrsaliyeGuncelle.ExecuteNonQuery();
    }

    private void TalepAkisiIrsaliyeDurumGuncelle(int TalepID)
    {
        if (DbConnTalepAkisiIrsaliyeGuncelle.State == ConnectionState.Closed)
            DbConnTalepAkisiIrsaliyeGuncelle.Open();

        string TalepAkisDuzenle = "UPDATE TalepAkisi SET Onay='True' WHERE TalepID=" + TalepID + "";
        cmdTalepAkisiIrsaliyeGuncelle = new SqlCommand(TalepAkisDuzenle, DbConnTalepAkisiIrsaliyeGuncelle);
        cmdTalepAkisiIrsaliyeGuncelle.CommandTimeout = 120;
        cmdTalepAkisiIrsaliyeGuncelle.ExecuteNonQuery();
    }

    private void ConnectionKapat()
    {
        //DbConnUser.Dispose();
        DbConnUser.Close();

        //Kullanicilar.Dispose();
        Kullanicilar.Close();

        //DbConMaxYetki.Dispose();
        DbConMaxYetki.Close();

        //DbConnLink.Dispose();
        DbConnLink.Close();

        //DbConnDepartmanKodu.Dispose();
        DbConnDepartmanKodu.Close();

        //DbConnYetkiKoduBul.Dispose();
        DbConnYetkiKoduBul.Close();

        //DbConnYetkiTamamla.Dispose();
        DbConnYetkiTamamla.Close();

        //DbConnYetkilendir.Dispose();
        DbConnYetkilendir.Close();

        //DbConnTalepIrsaliyeGuncelle.Dispose();
        DbConnTalepIrsaliyeGuncelle.Close();

        //DbConnTalepAkisiIrsaliyeGuncelle.Dispose();
        DbConnTalepAkisiIrsaliyeGuncelle.Close();

        //DbConnSiparisBul.Dispose();
        DbConnSiparisBul.Close();

        //DbConnIrsaliyeDurumu.Dispose();
        DbConnIrsaliyeDurumu.Close();

        //cmd.Dispose();
        //cmdYetki.Dispose();
        //cmdTalepAkisi.Dispose();
        //cmdTalepAkisDuzenle.Dispose();
        //MaxYetkiBul.Dispose();
        //cmdSiparisBul.Dispose();
        //cmdIrsaliyeDurumu.Dispose();
        //cmdTalepIrsaliyeGuncelle.Dispose();
        //cmdTalepAkisiIrsaliyeGuncelle.Dispose();

        //dr.Dispose();
        //dr.Close();

        //drYetki.Dispose();
        //drYetki.Close();
    }

    protected void btnTalepGuncelle_Click(object sender, EventArgs e)
    {
        dsSorgu = Session["TalepDurum"].ToString();
        grdTaleplerimiz.DataSource = datadoldur(dsSorgu);
        grdTaleplerimiz.DataBind();
        string Yetkili = Session["Yetki"].ToString();

        if (rdTalepler.SelectedIndex == 0)
        {
            btnBeklemeyeAl.Visible = false;
            btnSatinAlmayaGonder.Visible = false;
            btnTumunuOnayla.Visible = false;
            grdTaleplerimiz.Columns[1].Visible = false;
            btnDagit.Visible = false;
            btnAmbaraGonder.Visible = false;
            btnSiparisKapat.Visible = false;
        }
        else if (rdTalepler.SelectedIndex == 1)
        {
            btnDagit.Visible = false;
            btnSiparisKapat.Visible = false;

            if (SiparisButonu == true)
            {
                btnBeklemeyeAl.Visible = true;
                btnTumunuOnayla.Visible = true;
                btnAmbaraGonder.Visible = true;
                grdTaleplerimiz.Columns[1].Visible = true;
                imgParcaliSiparisGonder.Visible = true;
                imgTopluSiparis.Visible = true;
            }
            else
            {
                btnAmbaraGonder.Visible = false;
                btnBeklemeyeAl.Visible = false;
                grdTaleplerimiz.Columns[1].Visible = false;
                btnTumunuOnayla.Visible = false;
                imgParcaliSiparisGonder.Visible = false;
                imgTopluSiparis.Visible = false;
            }
            grdTaleplerimiz.Columns[14].Visible = SiparisButonu;
        }
        else if (rdTalepler.SelectedIndex == 2)
        {
            btnAmbaraGonder.Visible = SiparisButonu;
            btnSiparisKapat.Visible = false;
            btnDagit.Visible = AnaDepartmanimiz;

            if (SatinAlmaButonu == true)
            {
                btnBeklemeyeAl.Visible = false;
                btnTumunuOnayla.Visible = true;
                btnSatinAlmayaGonder.Visible = true;

                grdTaleplerimiz.Columns[1].Visible = true;
            }
            else if (SiparisButonu == true)
            {
                btnBeklemeyeAl.Visible = true;
                btnTumunuOnayla.Visible = true;
                btnAmbaraGonder.Visible = false;
                grdTaleplerimiz.Columns[1].Visible = true;
            }
            else if (SiparisButonu == false && SatinAlmaButonu == false)
            {
                btnAmbaraGonder.Visible = false;
                btnBeklemeyeAl.Visible = false;
                grdTaleplerimiz.Columns[1].Visible = false;
                btnTumunuOnayla.Visible = false;
            }
            grdTaleplerimiz.Columns[14].Visible = SiparisButonu;
            imgParcaliSiparisGonder.Visible = SiparisButonu;
            imgTopluSiparis.Visible = SiparisButonu;
            btnBeklemeyeAl.Visible = false;
            //btnTumunuOnayla.Visible = false;
            //btnDagit.Visible = false;
            //btnAmbaraGonder.Visible = SiparisButonu;
            btnSiparisKapat.Visible = false;
        }
        else if (rdTalepler.SelectedIndex == 3)
        {
            if (SiparisButonu == true)
            {
                btnBeklemeyeAl.Visible = true;
                btnTumunuOnayla.Visible = true;
                btnAmbaraGonder.Visible = false;
                grdTaleplerimiz.Columns[1].Visible = true;
            }
            else
            {
                btnAmbaraGonder.Visible = false;
                btnBeklemeyeAl.Visible = false;
                grdTaleplerimiz.Columns[1].Visible = false;
                btnTumunuOnayla.Visible = false;
            }

            grdTaleplerimiz.Columns[14].Visible = SiparisButonu;
            imgParcaliSiparisGonder.Visible = SiparisButonu;
            imgTopluSiparis.Visible = SiparisButonu;
            //btnBeklemeyeAl.Visible = true;
            btnTumunuOnayla.Visible = false;
            btnDagit.Visible = false;
            //btnAmbaraGonder.Visible = SiparisButonu;
            btnSiparisKapat.Visible = false;
        }
        else if (rdTalepler.SelectedIndex == 4)
        {
            btnDagit.Visible = false;
            btnAmbaraGonder.Visible = false;
            btnSiparisKapat.Visible = false;

            if (SiparisButonu == true)
            {
                btnBeklemeyeAl.Visible = false;
                imgParcaliSiparisGonder.Visible = true;
                imgTopluSiparis.Visible = true;
                grdTaleplerimiz.Columns[1].Visible = false;
                btnTumunuOnayla.Visible = false;
                btnSiparisKapat.Visible = true;
            }
            else
            {
                imgParcaliSiparisGonder.Visible = false;
                btnSiparisKapat.Visible = false;
                imgTopluSiparis.Visible = false;
                btnBeklemeyeAl.Visible = false;
                btnTalepGuncelle.Visible = true;
                btnTumunuOnayla.Visible = false;
            }
        }
        else if (rdTalepler.SelectedIndex == 5)
        {
            btnDagit.Visible = AnaDepartmanimiz;
            btnAmbaraGonder.Visible = SiparisButonu;
            btnSiparisKapat.Visible = false;

            if (SiparisButonu == false)
            {
                btnTalepGuncelle.Visible = true;
                btnTumunuOnayla.Visible = true;
                grdTaleplerimiz.Columns[1].Visible = true;
            }
            else
            {
                btnTalepGuncelle.Visible = true;
                btnTumunuOnayla.Visible = true;
                grdTaleplerimiz.Columns[1].Visible = true;
            }
        }
        else if (rdTalepler.SelectedIndex == 6)
        {
            btnDagit.Visible = false;
            btnSatinAlmayaGonder.Visible = false;
            btnTumunuOnayla.Visible = false;
            btnBeklemeyeAl.Visible = false;
            btnTalepGuncelle.Visible = true;
            grdTaleplerimiz.Columns[1].Visible = false;
            btnAmbaraGonder.Visible = false;
            btnSiparisKapat.Visible = false;
        }
        else if (rdTalepler.SelectedIndex == 7)
        {
            btnTalepGuncelle.Visible = true;
            btnSatinAlmayaGonder.Visible = false;
            btnDagit.Visible = false;
            btnTumunuOnayla.Visible = false;
            btnAmbaraGonder.Visible = false;
            btnSiparisKapat.Visible = false;
        }
        else if (rdTalepler.SelectedIndex == 8)
        {
            grdTaleplerimiz.Columns[1].Visible = false;
            btnTalepGuncelle.Visible = true;
            btnSatinAlmayaGonder.Visible = false;
            btnTumunuOnayla.Visible = false;
            btnDagit.Visible = false;
            btnAmbaraGonder.Visible = false;
            btnSiparisKapat.Visible = false;
        }
        else if (rdTalepler.SelectedIndex == 9)
        {
            btnDagit.Visible = false;
            btnSiparisKapat.Visible = false;

            if (SiparisButonu == false)
            {
                btnBeklemeyeAl.Visible = false;
                btnTalepGuncelle.Visible = true;
                btnTumunuOnayla.Visible = false;
                btnAmbaraGonder.Visible = SiparisButonu;
            }
            else
            {
                grdTaleplerimiz.Columns[1].Visible = true;
                btnBeklemeyeAl.Visible = false;
                btnTalepGuncelle.Visible = true;
                btnTumunuOnayla.Visible = true;
                btnAmbaraGonder.Visible = true;
            }
        }

        for (int j = 0; j < grdTaleplerimiz.Rows.Count; j++)
        {
            string ParcaliSipD = grdTaleplerimiz.Rows[j].Cells[12].Text.ToString();

            if (ParcaliSipD == "14")
            {
                grdTaleplerimiz.Rows[j].BackColor = Color.LightGreen;
            }
        }

        for (int i = 0; i < grdTaleplerimiz.Rows.Count; i++)
        {
            string deneme = OnayDurumKontrolEdiyoruz(Convert.ToInt32(Yetkili), grdTaleplerimiz.Rows[i].Cells[12].Text.ToString());
            grdTaleplerimiz.Rows[i].Cells[12].Text = deneme;
        }

        #region Gridde Miktarı Renklendiriyoruz

        for (int i = 0; i < grdTaleplerimiz.Rows.Count; i++)
        {
            string OnayDurumGrid = grdTaleplerimiz.Rows[i].Cells[12].Text.ToString();
            string Bekleme = "Bekleniyor";
            string Temin = "Temine Hazır";
            string SipA = "Sipariş";
            string Talepislemi = "Talep Beklemeye";
            string Ambar = "Gönderildi";
            string Red = "Onaylanmadı";
            string Teklif = "Teklif";

            if (OnayDurumGrid.IndexOf(Teklif) != -1)
            {
                grdTaleplerimiz.Rows[i].Cells[12].ForeColor = Color.Brown;
                grdTaleplerimiz.Rows[i].Cells[12].Font.Bold = true;
            }
            if (OnayDurumGrid.IndexOf(Red) != -1)
            {
                grdTaleplerimiz.Rows[i].Cells[12].ForeColor = Color.Red;
                grdTaleplerimiz.Rows[i].Cells[12].Font.Bold = true;
            }
            else if (OnayDurumGrid.IndexOf(Ambar) != -1)
            {
                grdTaleplerimiz.Rows[i].Cells[12].ForeColor = Color.Orange;
                grdTaleplerimiz.Rows[i].Cells[12].Font.Bold = true;
            }
            else if (OnayDurumGrid.IndexOf(Bekleme) != -1)
            {
                grdTaleplerimiz.Rows[i].Cells[12].ForeColor = Color.Orange;
                grdTaleplerimiz.Rows[i].Cells[12].Font.Bold = true;
            }
            else if (OnayDurumGrid.IndexOf(Temin) != -1)
            {
                grdTaleplerimiz.Rows[i].Cells[12].ForeColor = Color.Green;
                grdTaleplerimiz.Rows[i].Cells[12].Font.Bold = true;
            }
            else if (OnayDurumGrid.IndexOf(SipA) != -1)
            {
                grdTaleplerimiz.Rows[i].Cells[12].ForeColor = Color.Black;
                grdTaleplerimiz.Rows[i].Cells[12].Font.Bold = true;
            }
            else if (OnayDurumGrid.IndexOf(Talepislemi) != -1)
            {
                grdTaleplerimiz.Rows[i].Cells[12].ForeColor = Color.LightBlue;
                grdTaleplerimiz.Rows[i].Cells[12].Font.Bold = true;
            }

            if (SiparisButonu == true)
            {
                decimal gridMiktar = Convert.ToDecimal(grdTaleplerimiz.Rows[i].Cells[6].Text.ToString());
                decimal gridSiparisMiktar = Convert.ToDecimal(grdTaleplerimiz.Rows[i].Cells[9].Text.ToString());
                grdTaleplerimiz.Columns[8].Visible = false;

                if (gridMiktar > gridSiparisMiktar)
                {
                    grdTaleplerimiz.Rows[i].Cells[6].ForeColor = Color.Red;
                    grdTaleplerimiz.Rows[i].Cells[6].Font.Bold = true;
                    grdTaleplerimiz.Rows[i].Cells[6].ForeColor = Color.Red;
                    grdTaleplerimiz.Rows[i].Cells[6].Font.Bold = true;
                }

            }
            else
            {
                decimal gridMiktar = Convert.ToDecimal(grdTaleplerimiz.Rows[i].Cells[6].Text.ToString());
                string dddd = grdTaleplerimiz.Rows[i].Cells[7].Text.ToString();
                decimal gridStokMiktar = Convert.ToDecimal(grdTaleplerimiz.Rows[i].Cells[8].Text.ToString());
                grdTaleplerimiz.Columns[9].Visible = false;

                if (gridMiktar > gridStokMiktar)
                {
                    grdTaleplerimiz.Rows[i].Cells[6].ForeColor = Color.Red;
                    grdTaleplerimiz.Rows[i].Cells[6].Font.Bold = true;

                    grdTaleplerimiz.Rows[i].Cells[8].ForeColor = Color.Red;
                    grdTaleplerimiz.Rows[i].Cells[8].Font.Bold = true;
                }
            }
        }

        #endregion

        if (rdTalepler.SelectedIndex == 1)
        {
            for (int i = 0; i < grdTaleplerimiz.Rows.Count; i++)
            {
                GridViewRow row = grdTaleplerimiz.Rows[i];
                bool isChecked = ((HtmlInputCheckBox)row.FindControl("chk")).Checked;
                HtmlInputCheckBox chk = ((HtmlInputCheckBox)row.FindControl("chk"));

                int ParcaliTalepID = Convert.ToInt32(chk.Value.ToString());
                string EvrakNo = EvrakNoSonuc(Convert.ToInt32(chk.Value.ToString()));
                string SipMalKodu = MalKoduBul(EvrakNo);
                bool SipVisDurum = SaticiDurumKontrol(SipMalKodu);
                grdTaleplerimiz.Rows[i].Cells[14].Visible = SipVisDurum;
            }
        }
        if (rdTalepler.SelectedIndex == 4)
        {
            for (int i = 0; i < grdTaleplerimiz.Rows.Count; i++)
            {
                GridViewRow row = grdTaleplerimiz.Rows[i];
                bool isChecked = ((HtmlInputCheckBox)row.FindControl("chkSiparis")).Checked;
                HtmlInputCheckBox chk = ((HtmlInputCheckBox)row.FindControl("chkSiparis"));

                int ParcaliTalepID = Convert.ToInt32(chk.Value.ToString());
                string EvrakNo = EvrakNoSonuc(Convert.ToInt32(chk.Value.ToString()));
                string SipMalKodu = MalKoduBul(EvrakNo);
                bool SipVisDurum = SaticiDurumKontrol(SipMalKodu);
                bool ParcaliSipDurum = SiparisOnayDurumBul(ParcaliTalepID);

                if (SiparisButonu == true)
                {
                    grdTaleplerimiz.Columns[14].Visible = true;
                    //grdTaleplerimiz.Columns[13].Visible = ParcaliSipDurum;
                    if (ParcaliSipDurum == true)
                    {
                        //grdTaleplerimiz.Columns[13].Visible = ParcaliSipDurum;
                        grdTaleplerimiz.Rows[i].Cells[14].Visible = ParcaliSipDurum;
                    }
                    else if (ParcaliSipDurum == false)
                    {
                        //S.Visible = false;
                        grdTaleplerimiz.Rows[i].Cells[14].Visible = false;
                    }
                }
                else
                {
                    grdTaleplerimiz.Columns[14].Visible = false;
                }
            }
        }

    }

    protected void btnTumunuOnayla_Click(object sender, EventArgs e)
    {
        SecimIslemleri();

        dsSorgu = Session["TalepDurum"].ToString();
        grdTaleplerimiz.DataSource = datadoldur(dsSorgu);
        grdTaleplerimiz.DataBind();

        if (SiparisButonu == true)
        {
            btnBeklemeyeAl.Visible = true;
            btnTumunuOnayla.Visible = true;
            btnAmbaraGonder.Visible = true;
            grdTaleplerimiz.Columns[1].Visible = true;
        }
        else
        {
            btnAmbaraGonder.Visible = false;
            btnBeklemeyeAl.Visible = false;
            grdTaleplerimiz.Columns[1].Visible = true;
            btnTumunuOnayla.Visible = true;
        }

        grdTaleplerimiz.Columns[14].Visible = SiparisButonu;
        imgParcaliSiparisGonder.Visible = SiparisButonu;
        imgTopluSiparis.Visible = SiparisButonu;
        btnSiparisKapat.Visible = false;

        string Yetkili = Session["Yetki"].ToString();

        for (int i = 0; i < grdTaleplerimiz.Rows.Count; i++)
        {
            string deneme = OnayDurumKontrolEdiyoruz(Convert.ToInt32(Yetkili), grdTaleplerimiz.Rows[i].Cells[11].Text.ToString());
            grdTaleplerimiz.Rows[i].Cells[11].Text = deneme;
        }

        #region Gridde Miktarı Renklendiriyoruz

        for (int i = 0; i < grdTaleplerimiz.Rows.Count; i++)
        {
            string OnayDurumGrid = grdTaleplerimiz.Rows[i].Cells[11].Text.ToString();
            string Bekleme = "Bekleniyor";
            string Temin = "Temine Hazır";
            string SipA = "Sipariş";
            string Talepislemi = "Talep Beklemeye";
            string Ambar = "Gönderildi";
            string Red = "Onaylanmadı";
            string Teklif = "Teklif";

            if (OnayDurumGrid.IndexOf(Teklif) != -1)
            {
                grdTaleplerimiz.Rows[i].Cells[11].ForeColor = Color.Brown;
                grdTaleplerimiz.Rows[i].Cells[11].Font.Bold = true;
            }
            if (OnayDurumGrid.IndexOf(Red) != -1)
            {
                grdTaleplerimiz.Rows[i].Cells[11].ForeColor = Color.Red;
                grdTaleplerimiz.Rows[i].Cells[11].Font.Bold = true;
            }
            else if (OnayDurumGrid.IndexOf(Ambar) != -1)
            {
                grdTaleplerimiz.Rows[i].Cells[11].ForeColor = Color.Orange;
                grdTaleplerimiz.Rows[i].Cells[11].Font.Bold = true;
            }
            else if (OnayDurumGrid.IndexOf(Bekleme) != -1)
            {
                grdTaleplerimiz.Rows[i].Cells[11].ForeColor = Color.Orange;
                grdTaleplerimiz.Rows[i].Cells[11].Font.Bold = true;
            }
            else if (OnayDurumGrid.IndexOf(Temin) != -1)
            {
                grdTaleplerimiz.Rows[i].Cells[11].ForeColor = Color.Green;
                grdTaleplerimiz.Rows[i].Cells[11].Font.Bold = true;
            }
            else if (OnayDurumGrid.IndexOf(SipA) != -1)
            {
                grdTaleplerimiz.Rows[i].Cells[11].ForeColor = Color.Black;
                grdTaleplerimiz.Rows[i].Cells[11].Font.Bold = true;
            }
            else if (OnayDurumGrid.IndexOf(Talepislemi) != -1)
            {
                grdTaleplerimiz.Rows[i].Cells[11].ForeColor = Color.LightBlue;
                grdTaleplerimiz.Rows[i].Cells[11].Font.Bold = true;
            }

            if (SiparisButonu == true)
            {
                decimal gridMiktar = Convert.ToDecimal(grdTaleplerimiz.Rows[i].Cells[6].Text.ToString());
                decimal gridSiparisMiktar = Convert.ToDecimal(grdTaleplerimiz.Rows[i].Cells[8].Text.ToString());
                grdTaleplerimiz.Columns[9].Visible = false;

                if (gridMiktar > gridSiparisMiktar)
                {
                    grdTaleplerimiz.Rows[i].Cells[6].ForeColor = Color.Red;
                    grdTaleplerimiz.Rows[i].Cells[6].Font.Bold = true;
                    grdTaleplerimiz.Rows[i].Cells[6].ForeColor = Color.Red;
                    grdTaleplerimiz.Rows[i].Cells[6].Font.Bold = true;
                }

            }
            else
            {
                decimal gridMiktar = Convert.ToDecimal(grdTaleplerimiz.Rows[i].Cells[6].Text.ToString());
                string dddd = grdTaleplerimiz.Rows[i].Cells[7].Text.ToString();
                decimal gridStokMiktar = Convert.ToDecimal(grdTaleplerimiz.Rows[i].Cells[7].Text.ToString());
                grdTaleplerimiz.Columns[8].Visible = false;

                if (gridMiktar > gridStokMiktar)
                {
                    grdTaleplerimiz.Rows[i].Cells[6].ForeColor = Color.Red;
                    grdTaleplerimiz.Rows[i].Cells[6].Font.Bold = true;

                    grdTaleplerimiz.Rows[i].Cells[7].ForeColor = Color.Red;
                    grdTaleplerimiz.Rows[i].Cells[7].Font.Bold = true;
                }
            }
        }

        #endregion

        if (rdTalepler.SelectedIndex == 1)
        {
            for (int i = 0; i < grdTaleplerimiz.Rows.Count; i++)
            {
                GridViewRow row = grdTaleplerimiz.Rows[i];
                bool isChecked = ((HtmlInputCheckBox)row.FindControl("chk")).Checked;
                HtmlInputCheckBox chk = ((HtmlInputCheckBox)row.FindControl("chk"));

                int ParcaliTalepID = Convert.ToInt32(chk.Value.ToString());
                string EvrakNo = EvrakNoSonuc(Convert.ToInt32(chk.Value.ToString()));
                string SipMalKodu = MalKoduBul(EvrakNo);
                bool SipVisDurum = SaticiDurumKontrol(SipMalKodu);
                grdTaleplerimiz.Rows[i].Cells[13].Visible = SipVisDurum;
            }
        }

        if (DurumKontrolu == true)
        {
            Alert.Show("Seçtiğiniz İhtiyaçlar Başarılı Bir Şekilde Onaylanmıştır Gönderilmiştir.");
        }
    }

    protected void grdTaleplerimiz_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdTaleplerimiz.PageIndex = e.NewPageIndex;
        Sayfaliyoruz = Convert.ToInt32(Session["SayfaDegeri"].ToString());
        rdTalepler.SelectedIndex = Sayfaliyoruz;
        rdTalepler_SelectedIndexChanged(sender, e);
    }

    private void SecimIslemleri()
    {
        DurumKontrolu = false;
        string GitDepBul = "";
        string Yetkili = Session["Yetki"].ToString();

        for (int i = 0; i < grdTaleplerimiz.Rows.Count; i++)
        {
            GridViewRow row = grdTaleplerimiz.Rows[i];
            bool isChecked = ((HtmlInputCheckBox)row.FindControl("chk")).Checked;
            HtmlInputCheckBox chk = ((HtmlInputCheckBox)row.FindControl("chk"));

            if (isChecked)
            {

                string TalepID = chk.Value.ToString() + ",";
                string EvrakNo = EvrakNoSonuc(Convert.ToInt32(chk.Value.ToString()));
                string MalAdi = MalAdiSonuc(Convert.ToInt32(chk.Value.ToString())) + ",";
                string KontrolMalAdi = MalAdiSonuc(Convert.ToInt32(chk.Value.ToString()));
                string Miktar = grdTaleplerimiz.Rows[i].Cells[6].Text.ToString() + "/";
                string MalKodumuz = MalKoduBul(EvrakNo, KontrolMalAdi.ToString().Replace("'", "''")) + ",";
                string KdvMalKodu = MalKoduBul(EvrakNo, KontrolMalAdi.ToString().Replace("'", "''"));
                string Kdv = KdvBul(KdvMalKodu) + "/";

                string ihtiyacNo = EvrakNoSonuc(Convert.ToInt32(chk.Value.ToString())) + ",";
                Miktar2 += Miktar;
                ihtiyacNo2 += ihtiyacNo;
                MalKodumuz2 += MalKodumuz;
                TalepID2 += TalepID;
                Kdv2 += Kdv;
                MalAdlari2 += MalAdi;
            }
        }

        evrakNumaramiz = ihtiyacNo2.Split(',');
        MalKodlarimiz = MalKodumuz2.Split(',');
        MalAdlarimiz = MalAdlari2.Split(',');
        Miktarlarimiz = Miktar2.Split('/');
        TalepIDlerimiz = TalepID2.Split(',');

        for (int i = 0; i < evrakNumaramiz.Length - 1; i++)
        {
            string EvrakNo = evrakNumaramiz[i].ToString();
            string MalKodu = MalKodlarimiz[i].ToString();
            string DepartmanKodu = Session["DepartmanKodu"].ToString();
            string KullaniciKodu = Session["KullaniciKodu"].ToString();

            GitDepBul = GidecekDepartmanBul(MalKodu);

            string StokluDepartmanKodu = DepartmanKodu;
            string StokluDepartmanKodu2 = DepartmanKodu;
            string TalepAkisDepartmanKodu = DepartmanKodu;

            //StokluDepartmanKodu = GidecekDepartmanKodu3(GidecekDepartmanBul(MalKodlarimiz[i].ToString()));
            //StokluDepartmanKodu2 = StokluDepartmanKodu;

            if (GitDepBul == "HAMMADDE" || GitDepBul == "YARDMADDE")
            {
                StokluDepartmanKodu = HammaddeDepartmanKodu(GitDepBul);
            }
            else
            {
                StokluDepartmanKodu = AnaDepartmanKodu();
            }

            YetkiKodu = Convert.ToInt32(Session["Yetki"].ToString());

            YetkiKoduBul2(DepartmanKodu, KullaniciKodu.ToString(), Convert.ToString(YetkiKodu));

            if (Convert.ToInt32(KayitYetki) == 0)
            {
                OnayDurumu = OnayDurumBelirliyoruz(3);
                //StokluDepartmanKodu = GidecekDepartmanBul(MalKodu);
                KayitYetki = "4";
                //DepartmanCountDurum = GidecekDeptDurum(GidecekDepartmanBul(MalKodu));

                //if (DepartmanCountDurum > 1)
                //{
                //    StokluDepartmanKodu = GidecekDepartmanKodu3(GidecekDepartmanBul(MalKodu));
                //}
                //else
                //{
                //    StokluDepartmanKodu = GidecekDepartmanKodu3(GidecekDepartmanBul(MalKodu));
                //}
                //YetkiTamamla(StokluDepartmanKodu);

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
                OnayDurumu = OnayDurumBelirliyoruz(Convert.ToInt32(YetkiKodu));
            }

            if (SiparisButonu == true)
            {
                string OnayliSorgu = "UPDATE Tlp SET " +
                                     "OnayDurumu='6', " +
                                     "Onaylayan='" + KullaniciKodu.ToString() + "', " +
                                     "ilkIslem=1 " +
                                     "WHERE EvrakNoTarih+EvrakNo='" + EvrakNo.ToString() + "'";

                cmd = new SqlCommand(OnayliSorgu, DbConnUser);
                cmd.CommandTimeout = 120;
                cmd.ExecuteNonQuery();
                DurumKontrolu = true;
            }

            if (SatinAlmaButonu == true)
            {
                string MiktarimizOnay = Convert.ToString(Miktar);
                MiktarimizOnay = MiktarimizOnay.ToString().Replace(',', '.');

                string OnayliSorgu = "UPDATE Tlp SET " +
                     "OnayDurumu='4', " +
                     "Onaylayan='" + KullaniciKodu.ToString() + "', " +
                     "OnayMiktar='" + Convert.ToDecimal(MiktarimizOnay.ToString()) + "', " +
                     "ilkIslem=1 " +
                     "WHERE EvrakNoTarih+EvrakNo='" + EvrakNo.ToString() + "'";

                cmd = new SqlCommand(OnayliSorgu, DbConnUser);
                cmd.CommandTimeout = 120;
                cmd.ExecuteNonQuery();

                //TalepID = Convert.ToInt32(TalepIDlerimiz[i].ToString());

                //string TalepAkisDuzenle = "UPDATE TalepAkisi SET Onay='True' WHERE TalepID=" + TalepID + "";
                //cmd = new SqlCommand(TalepAkisDuzenle, DbConnUser);
                //cmd.CommandTimeout = 120;
                //cmd.ExecuteNonQuery();
                DurumKontrolu = true;
            }
            if (SatinAlmaButonu == false && SiparisButonu == false)
            {
                string MiktarimizOnay = Convert.ToString(Miktar);
                MiktarimizOnay = MiktarimizOnay.ToString().Replace(',', '.');

                #region İzin Durum Kontrolü

                string izinayari = Session["izinAyari"].ToString();
                string BagliOlduguDepartman = Session["BagliOlduguBirim"].ToString();
                string AltDepartmanID = Session["AltDepartmanID"].ToString();

                if (izinayari == "İzne Çıkmış")
                {
                    if (BagliOlduguDepartman == "0")
                    {
                        YetkiKoduBul(DepartmanKodu, KullaniciKodu.ToString(), Session["izinYetki"].ToString(), AltDepartmanID);
                    }
                    else
                    {
                        YetkiKoduBul(DepartmanKodu, KullaniciKodu.ToString(), Session["izinYetki"].ToString(), BagliOlduguDepartman);
                    }

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
                            UnvanimiziAliyoruz = YetkiUnvaniBul(Convert.ToInt32(Yetkimiz));

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

                #endregion

                string OnayliSorgu = "UPDATE Tlp SET " +
                         "OnayDurumu='" + OnayDurumu.ToString() + "', " +
                         "GidecekDepartman='" + StokluDepartmanKodu + "'," +
                         "Onaylayan='" + KullaniciKodu.ToString() + "', " +
                         "OnayMiktar='" + Convert.ToDecimal(MiktarimizOnay.ToString()) + "', " +
                         "OnaylayanYetki=" + Convert.ToInt32(KayitYetki) + ", " +
                         "ilkIslem=1 " +
                         "WHERE EvrakNoTarih+EvrakNo='" + EvrakNo.ToString() + "'";

                cmd = new SqlCommand(OnayliSorgu, DbConnUser);
                cmd.CommandTimeout = 120;
                cmd.ExecuteNonQuery();

                DurumKontrolu = true;
            }
        }
    }

    private void BolumlereDagit()
    {
        DurumKontrolu = false;
        string Yetkili = Session["Yetki"].ToString();

        for (int i = 0; i < grdTaleplerimiz.Rows.Count; i++)
        {
            GridViewRow row = grdTaleplerimiz.Rows[i];
            bool isChecked = ((HtmlInputCheckBox)row.FindControl("chk")).Checked;
            HtmlInputCheckBox chk = ((HtmlInputCheckBox)row.FindControl("chk"));

            if (isChecked)
            {

                string TalepID = chk.Value.ToString() + ",";
                string EvrakNo = EvrakNoSonuc(Convert.ToInt32(chk.Value.ToString()));
                string MalAdi = MalAdiSonuc(Convert.ToInt32(chk.Value.ToString())) + ",";
                string KontrolMalAdi = MalAdiSonuc(Convert.ToInt32(chk.Value.ToString()));
                string Miktar = grdTaleplerimiz.Rows[i].Cells[6].Text.ToString() + "/";
                string MalKodumuz = MalKoduBul(EvrakNo, KontrolMalAdi.Replace("'", "''")) + ",";
                string KdvMalKodu = MalKoduBul(EvrakNo, KontrolMalAdi.Replace("'", "''"));
                string Kdv = KdvBul(KdvMalKodu) + "/";

                string ihtiyacNo = EvrakNoSonuc(Convert.ToInt32(chk.Value.ToString())) + ",";
                Miktar2 += Miktar;
                ihtiyacNo2 += ihtiyacNo;
                MalKodumuz2 += MalKodumuz;
                TalepID2 += TalepID;
                Kdv2 += Kdv;
                MalAdlari2 += MalAdi;
            }
        }

        evrakNumaramiz = ihtiyacNo2.Split(',');
        MalKodlarimiz = MalKodumuz2.Split(',');
        MalAdlarimiz = MalAdlari2.Split(',');
        Miktarlarimiz = Miktar2.Split('/');
        TalepIDlerimiz = TalepID2.Split(',');

        for (int i = 0; i < evrakNumaramiz.Length - 1; i++)
        {
            string EvrakNo = evrakNumaramiz[i].ToString();
            int TaID = Convert.ToInt32(TalepIDlerimiz[i].ToString());
            string MalKodu = MalKodlarimiz[i].ToString();
            string DepartmanKodu = Session["DepartmanKodu"].ToString();
            string KullaniciKodu = Session["KullaniciKodu"].ToString();
            string StokluDepartmanKodu = DepartmanKodu;
            string StokluDepartmanKodu2 = DepartmanKodu;
            string TalepAkisDepartmanKodu = DepartmanKodu;
            StokluDepartmanKodu = GidecekDepartmanKodu2(GidecekDepartmanBul(MalKodlarimiz[i].ToString()));
            StokluDepartmanKodu2 = StokluDepartmanKodu;

            YetkiKodu = Convert.ToInt32(Session["Yetki"].ToString());

            int OnayYetkisi = DagitilanYetkili(StokluDepartmanKodu);

            //if (Convert.ToInt32(KayitYetki) == 0)
            //{
            //    OnayDurumu = OnayDurumBelirliyoruz(Convert.ToInt32(YetkiKodu));
            //    //StokluDepartmanKodu = GidecekDepartmanBul(MalKodu);
            //    KayitYetki = "4";
            //    DepartmanCountDurum = GidecekDeptDurum(GidecekDepartmanBul(MalKodu));

            //    if (DepartmanCountDurum > 1)
            //    {
            //        StokluDepartmanKodu = GidecekDepartmanKodu(GidecekDepartmanBul(MalKodu));
            //    }
            //    else
            //    {
            //        StokluDepartmanKodu = GidecekDepartmanKodu2(GidecekDepartmanBul(MalKodu));
            //    }
            //    //YetkiTamamla(StokluDepartmanKodu);
            //}
            //else
            //{
            //    OnayDurumu = OnayDurumBelirliyoruz(Convert.ToInt32(YetkiKodu));
            //}

            string OnayliSorgu = "UPDATE Tlp SET " +
                     "OnayDurumu='3', " +
                     "GidecekDepartman='" + StokluDepartmanKodu + "' ," +
                     "Onaylayan='" + KullaniciKodu.ToString() + "', " +
                     "OnaylayanYetki=" + OnayYetkisi + ", " +
                     "ilkIslem=1 " +
                     "WHERE TalepID=" + TaID + "";

            cmd = new SqlCommand(OnayliSorgu, DbConnUser);
            cmd.CommandTimeout = 120;
            cmd.ExecuteNonQuery();
            DurumKontrolu = true;
        }
    }

    private int DagitilanYetkili(string DepartmanKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT ISNULL(MIN(Yetki),0) AS Yetki FROM Kullanicilar AS K " +
                    "INNER JOIN Departmanlar AS D ON K.DepartmanID=D.DepartmanID " +
                    "WHERE D.DepartmanKodu='" + DepartmanKodu + "' ";

        cmd = new SqlCommand(sorgu, DbConnUser);

        return (int)Convert.ToInt32(cmd.ExecuteScalar());
    }

    private void SatinAlmayaGonderiyoruz()
    {
        DurumKontrolu = false;

        for (int i = 0; i < grdTaleplerimiz.Rows.Count; i++)
        {
            GridViewRow row = grdTaleplerimiz.Rows[i];
            bool isChecked = ((HtmlInputCheckBox)row.FindControl("chk")).Checked;
            HtmlInputCheckBox chk = ((HtmlInputCheckBox)row.FindControl("chk"));

            if (isChecked)
            {

                string TalepID = chk.Value.ToString() + ",";
                string EvrakNo = EvrakNoSonuc(Convert.ToInt32(chk.Value.ToString()));
                string MalAdi = MalAdiSonuc(Convert.ToInt32(chk.Value.ToString())) + "}";
                string KontrolMalAdi = MalAdiSonuc(Convert.ToInt32(chk.Value.ToString()));
                string Miktar = grdTaleplerimiz.Rows[i].Cells[6].Text.ToString() + "/";
                string MalKodumuz = MalKoduBul(EvrakNo, KontrolMalAdi.Replace("'", "''")) + ",";
                string KdvMalKodu = MalKoduBul(EvrakNo, KontrolMalAdi.Replace("'", "''"));
                string Kdv = KdvBul(KdvMalKodu) + "/";

                string ihtiyacNo = EvrakNoSonuc(Convert.ToInt32(chk.Value.ToString())) + ",";
                Miktar2 += Miktar;
                ihtiyacNo2 += ihtiyacNo;
                MalKodumuz2 += MalKodumuz;
                TalepID2 += TalepID;
                Kdv2 += Kdv;
                MalAdlari2 += MalAdi;
            }
        }

        evrakNumaramiz = ihtiyacNo2.Split(',');
        MalKodlarimiz = MalKodumuz2.Split(',');
        MalAdlarimiz = MalAdlari2.Split('}');
        Miktarlarimiz = Miktar2.Split('/');
        TalepIDlerimiz = TalepID2.Split(',');

        for (int i = 0; i < evrakNumaramiz.Length - 1; i++)
        {

            string EvrakNo = evrakNumaramiz[i].ToString();
            string DepartmanKodu = Session["DepartmanKodu"].ToString();
            string KullaniciKodu = Session["KullaniciKodu"].ToString();
            string StokluDepartmanKodu = DepartmanKodu;
            Miktar = Convert.ToDecimal(Miktarlarimiz[i].ToString());
            YetkiKodu = Convert.ToInt32(Session["Yetki"].ToString());

            StokluDepartmanKodu = SatinalmayaGonderiyoruzTeminYeriBul(EvrakNo);

            string MiktarimizOnay = Convert.ToString(Miktar);
            MiktarimizOnay = MiktarimizOnay.ToString().Replace(',', '.');

            string sorgu = "UPDATE Tlp SET " +
                           "GidecekDepartman='" + StokluDepartmanKodu + "'," +
                           "OnayDurumu='5', " +
                           "Onaylayan='" + KullaniciKodu.ToString() + "', " +
                           "OnayMiktar='" + Convert.ToDecimal(MiktarimizOnay.ToString()) + "', " +
                           "OnaylayanYetki=5, " +
                           "ilkIslem=1 " +
                           "WHERE EvrakNoTarih+EvrakNo='" + EvrakNo.ToString() + "'";

            cmd = new SqlCommand(sorgu, DbConnUser);
            cmd.CommandTimeout = 120;
            cmd.ExecuteNonQuery();

            DurumKontrolu = true;
        }
    }

    private void AmbaraGonderiyoruz()
    {
        DurumKontrolu = false;

        for (int i = 0; i < grdTaleplerimiz.Rows.Count; i++)
        {
            GridViewRow row = grdTaleplerimiz.Rows[i];
            bool isChecked = ((HtmlInputCheckBox)row.FindControl("chk")).Checked;
            HtmlInputCheckBox chk = ((HtmlInputCheckBox)row.FindControl("chk"));

            if (isChecked)
            {

                string TalepID = chk.Value.ToString() + ",";
                string EvrakNo = EvrakNoSonuc(Convert.ToInt32(chk.Value.ToString()));
                string MalAdi = MalAdiSonuc(Convert.ToInt32(chk.Value.ToString())) + ",";
                string KontrolMalAdi = MalAdiSonuc(Convert.ToInt32(chk.Value.ToString()));
                string Miktar = grdTaleplerimiz.Rows[i].Cells[6].Text.ToString() + "/";
                string MalKodumuz = MalKoduBul(EvrakNo, KontrolMalAdi.Replace("'", "''")) + ",";
                string KdvMalKodu = MalKoduBul(EvrakNo, KontrolMalAdi.Replace("'", "''"));
                string Kdv = KdvBul(KdvMalKodu) + "/";

                string ihtiyacNo = EvrakNoSonuc(Convert.ToInt32(chk.Value.ToString())) + ",";
                Miktar2 += Miktar;
                ihtiyacNo2 += ihtiyacNo;
                MalKodumuz2 += MalKodumuz;
                TalepID2 += TalepID;
                Kdv2 += Kdv;
                MalAdlari2 += MalAdi;
            }
        }

        evrakNumaramiz = ihtiyacNo2.Split(',');
        MalKodlarimiz = MalKodumuz2.Split(',');
        MalAdlarimiz = MalAdlari2.Split(',');
        Miktarlarimiz = Miktar2.Split('/');
        TalepIDlerimiz = TalepID2.Split(',');

        for (int i = 0; i < evrakNumaramiz.Length - 1; i++)
        {
            int TaID = Convert.ToInt32(TalepIDlerimiz[i].ToString());
            string EvrakNo = evrakNumaramiz[i].ToString();
            string DepartmanKodu = Session["DepartmanKodu"].ToString();
            string KullaniciKodu = Session["KullaniciKodu"].ToString();
            string StokluDepartmanKodu = DepartmanKodu;
            Miktar = Convert.ToDecimal(Miktarlarimiz[i].ToString());
            YetkiKodu = Convert.ToInt32(Session["Yetki"].ToString());

            StokluDepartmanKodu = AnaDepartmanKodu();
            int OnayYetkisi = DagitilanYetkili(StokluDepartmanKodu);

            string sorgu = "UPDATE Tlp SET " +
                           "GidecekDepartman='" + StokluDepartmanKodu + "'," +
                           "OnayDurumu='3', " +
                           "Onaylayan='" + KullaniciKodu.ToString() + "', " +
                           "OnaylayanYetki=" + OnayYetkisi + " " +
                           "WHERE TalepID=" + TaID + "";

            cmd = new SqlCommand(sorgu, DbConnUser);
            cmd.CommandTimeout = 120;
            cmd.ExecuteNonQuery();

            DurumKontrolu = true;
        }
    }

    private string EvrakNoSonuc(int TalepID)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT dbo.fn_EvrakNoBul(" + TalepID + ") AS EvrakNo";
        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();
    }

    private string MalAdiSonuc(int TalepID)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT dbo.fn_MalAdiBul(" + TalepID + ") AS MalAdi";
        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();
    }

    protected void btnSatinAlmayaGonder_Click(object sender, EventArgs e)
    {
        SatinAlmayaGonderiyoruz();

        if (DurumKontrolu == true)
        {
            Alert.Show("Seçtiğiniz İhtiyaçlar Başarılı Bir Şekilde Satın Almaya Gönderilmiştir.");
        }
    }

    private string AramaDurumDepartmani(string DepartmanKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT DepartmanAdi FROM Departmanlar " +
                       "WHERE DepartmanKodu='" + DepartmanKodu + "'";

        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();
    }

    //private int SiparisDurumKontrol(string SiparisNumarasi)
    //{
    //    if (DbConnLink.State == ConnectionState.Closed)
    //        DbConnLink.Open();

    //    string sorgu = "SELECT COUNT(*) AS Kontrol FROM STK005 " +
    //                    "WHERE STK005_SiparisNo='" + SiparisNumarasi + "'";

    //    SqlCommand sipkontrolcmd = new SqlCommand(sorgu, DbConnLink);
    //    sipkontrolcmd.CommandTimeout = 120;
    //    return (int)Convert.ToInt32(sipkontrolcmd.ExecuteScalar());
    //}

    //private string SiparisIrsaliyeNoBul(int TalepID)
    //{
    //    if (DbConnUser.State == ConnectionState.Closed)
    //        DbConnUser.Open();

    //    string sorgu = "SELECT SiparisNo FROM Tlp WHERE TalepID=2";

    //    SqlCommand sipNoBulCmd = new SqlCommand(sorgu, DbConnUser);
    //    sipNoBulCmd.CommandTimeout = 120;
    //    return (string)sipNoBulCmd.ExecuteScalar();
    //}

    private void BeklemeyeAl()
    {
        DurumKontrolu = false;

        for (int i = 0; i < grdTaleplerimiz.Rows.Count; i++)
        {
            GridViewRow row = grdTaleplerimiz.Rows[i];
            bool isChecked = ((HtmlInputCheckBox)row.FindControl("chk")).Checked;
            HtmlInputCheckBox chk = ((HtmlInputCheckBox)row.FindControl("chk"));

            if (isChecked)
            {

                string TalepID = chk.Value.ToString() + ",";
                string EvrakNo = EvrakNoSonuc(Convert.ToInt32(chk.Value.ToString()));
                string MalAdi = MalAdiSonuc(Convert.ToInt32(chk.Value.ToString())) + ",";
                string KontrolMalAdi = MalAdiSonuc(Convert.ToInt32(chk.Value.ToString()));
                string Miktar = grdTaleplerimiz.Rows[i].Cells[6].Text.ToString() + "/";
                string MalKodumuz = MalKoduBul(EvrakNo, KontrolMalAdi.Replace("'", "''")) + ",";
                string KdvMalKodu = MalKoduBul(EvrakNo, KontrolMalAdi.Replace("'", "''"));
                string Kdv = KdvBul(KdvMalKodu) + "/";

                string ihtiyacNo = EvrakNoSonuc(Convert.ToInt32(chk.Value.ToString())) + ",";
                Miktar2 += Miktar;
                ihtiyacNo2 += ihtiyacNo;
                MalKodumuz2 += MalKodumuz;
                TalepID2 += TalepID;
                Kdv2 += Kdv;
                MalAdlari2 += MalAdi;
            }
        }

        evrakNumaramiz = ihtiyacNo2.Split(',');
        MalKodlarimiz = MalKodumuz2.Split(',');
        MalAdlarimiz = MalAdlari2.Split(',');
        Miktarlarimiz = Miktar2.Split('/');
        TalepIDlerimiz = TalepID2.Split(',');

        for (int i = 0; i < evrakNumaramiz.Length - 1; i++)
        {
            string EvrakNo = evrakNumaramiz[i].ToString();
            string KullaniciKodu = Session["KullaniciKodu"].ToString();

            string OnayliSorgu = "UPDATE Tlp SET " +
                                 "OnayDurumu='8', " +
                                 "Onaylayan='" + KullaniciKodu.ToString() + "', " +
                                 "ilkIslem=1 " +
                                 "WHERE EvrakNoTarih+EvrakNo='" + EvrakNo.ToString() + "'";

            cmd = new SqlCommand(OnayliSorgu, DbConnUser);
            cmd.CommandTimeout = 120;
            cmd.ExecuteNonQuery();
            DurumKontrolu = true;
        }
    }

    protected void btnBeklemeyeAl_Click(object sender, EventArgs e)
    {
        BeklemeyeAl();
    }

    private string GidecekDepartmanBul(string MalKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT dbo.fn_GidecekDepartmanBul('" + MalKodu + "') AS GidecekDepartman";

        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();
    }

    private string SatinAlmaGidecekDepartmanBul(string EvrakNo)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT GidecekKod FROM GidecekDepartman " +
                       "WHERE GidecekDepartman=(SELECT DepartmanDurumu FROM StokKodlari " +
                       "WHERE StokKodu=(SELECT dbo.fn_MalKoduBul('" + EvrakNo + "'))) AND ID=2";

        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
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
                case "14": sonuc = "Sipariş Açıldı"; break;
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
                case "14": sonuc = "Sipariş Açıldı"; break;
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
                case "14": sonuc = "Sipariş Açıldı"; break;
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
                case "14": sonuc = "Sipariş Açıldı"; break;
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
                case "14": sonuc = "Sipariş Açıldı"; break;
            }
        }
        string Yeniislem = sonuc;
        return Yeniislem;
    }

    private void AramaDepartmanlari(string DepartmanKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string Sorgu = "SELECT GidecekDepartman FROM GidecekDepartman " +
                       "WHERE GidecekKod='" + DepartmanKodu + "'";

        cmd = new SqlCommand(Sorgu, DbConnUser);
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        if (dr.Read())
        {
            GitDeptKod2 = dr["GidecekDepartman"].ToString();
        }

        dr.Dispose();
        dr.Close();
    }

    private void AramaDepartmanlari2(string DepartmanKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string Sorgu = "SELECT DepartmanKodu FROM Departmanlar AS D " +
                       "INNER JOIN GidecekDepartman AS G ON D.DepartmanKodu=G.GidecekKod " +
                       "WHERE GidecekDepartman='" + DepartmanKodu + "' AND ID=1";

        cmd = new SqlCommand(Sorgu, DbConnUser);
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dr.Read())
        {
            GitDeptKod3 += dr["DepartmanKodu"].ToString() + ",";
        }

        dr.Dispose();
        dr.Close();
    }

    private void AramaDepartmanlari3(string DepartmanKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string Sorgu = "SELECT DepartmanKodu FROM Departmanlar AS D " +
                       "INNER JOIN GidecekDepartman AS G ON D.DepartmanKodu=G.GidecekKod " +
                       "WHERE GidecekDepartman='" + DepartmanKodu + "' AND ID=2";

        cmd = new SqlCommand(Sorgu, DbConnUser);
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dr.Read())
        {
            GitDeptKod3 += dr["DepartmanKodu"].ToString() + ",";
        }

        dr.Dispose();
        dr.Close();
    }

    //Normal Kullanıcı Tarafı

    private void NormalDepartmanSatinAlma()
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string Sorgu = "SELECT GidecekDepartman FROM GidecekDepartman WHERE ID=2 " +
                       "GROUP BY GidecekDepartman";

        cmd = new SqlCommand(Sorgu, DbConnUser);
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dr.Read())
        {
            SatinAlmaKodu += dr["GidecekDepartman"].ToString() + ",";
        }

        dr.Dispose();
        dr.Close();
    }

    private void NormalDepartmanAmbar()
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string Sorgu = "SELECT GidecekDepartman FROM GidecekDepartman WHERE ID=1 " +
                       "GROUP BY GidecekDepartman";

        cmd = new SqlCommand(Sorgu, DbConnUser);
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dr.Read())
        {
            AmbarKodu += dr["GidecekDepartman"].ToString() + ",";
        }

        dr.Dispose();
        dr.Close();
    }

    private void NormalDepartmanSatinAlma2(string DepartmanKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string Sorgu = "SELECT DepartmanKodu FROM Departmanlar AS D " +
                        "INNER JOIN GidecekDepartman AS G ON D.DepartmanKodu=G.GidecekKod " +
                        "WHERE StokDepartman IN(" + DepartmanKodu + ") AND ID=2";

        cmd = new SqlCommand(Sorgu, DbConnUser);
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        SatinAlmaKodu = null;

        while (dr.Read())
        {
            SatinAlmaKodu += dr["DepartmanKodu"].ToString() + ",";
        }
        dr.Dispose();
        dr.Close();
    }

    private void NormalDepartmanAmbar2(string DepartmanKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string Sorgu = "SELECT DepartmanKodu FROM Departmanlar AS D " +
                        "INNER JOIN GidecekDepartman AS G ON D.DepartmanKodu=G.GidecekKod " +
                        "WHERE StokDepartman IN(" + DepartmanKodu + ") AND ID=1";

        cmd = new SqlCommand(Sorgu, DbConnUser);
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        AmbarKodu = null;

        while (dr.Read())
        {
            AmbarKodu += dr["DepartmanKodu"].ToString() + ",";
        }
        dr.Dispose();
        dr.Close();
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

    private string GidecekDepartmanKodu3(string MalKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT TOP(1) GidecekKod FROM GidecekDepartman " +
                    "WHERE GidecekDepartman='" + MalKodu + "' AND ID=1";

        cmd = new SqlCommand(sorgu, DbConnUser);
        return (string)cmd.ExecuteScalar();
    }

    private string GidecekDepartmanKodu2(string MalKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT GidecekKod FROM GidecekDepartman " +
                    "WHERE GidecekDepartman='" + MalKodu + "' AND ID=1";

        cmd = new SqlCommand(sorgu, DbConnUser);
        return (string)cmd.ExecuteScalar();
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

    private bool SaticiDurumKontrol(string MalKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string Sorgu = "SELECT dbo.fn_SaticiDurumuKontrol('" + MalKodu + "') AS SaticiDurumu";

        cmd = new SqlCommand(Sorgu, DbConnUser);
        return (bool)Convert.ToBoolean(cmd.ExecuteScalar());
    }

    private string MalKoduBul(string EvrakNo)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string Sorgu = "SELECT dbo.fn_MalKoduBul('" + EvrakNo + "') AS MalKodu";
        cmd = new SqlCommand(Sorgu, DbConnUser);
        return (string)cmd.ExecuteScalar();
    }

    private string AciklamaBuluyoruz(int TalepID)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT Aciklama FROM Tlp WHERE TalepID=" + TalepID + "";
        cmd = new SqlCommand(sorgu, DbConnUser);

        return (string)cmd.ExecuteScalar();
    }

    private string AnaDepartmanKodu()
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT AnaDepartmanKodu AS GidecekDepartman FROM AnaDepartman";
        cmd = new SqlCommand(sorgu, DbConnUser);

        return (string)cmd.ExecuteScalar();
    }

    protected void btnDagit_Click(object sender, EventArgs e)
    {
        BolumlereDagit();

        if (DurumKontrolu == true)
        {
            Alert.Show("Seçtiğiniz İhtiyaçlar Başarılı Bir Şekilde Bölümlere Gönderilmiştir.");
        }
    }

    protected void btnAmbaraGonder_Click(object sender, EventArgs e)
    {
        AmbaraGonderiyoruz();

        if (DurumKontrolu == true)
        {
            Alert.Show("Seçtiğiniz İhtiyaçlar Başarılı Bir Şekilde Ambara Gönderilmiştir.");
        }
    }

    private bool SiparisOnayDurumBul(int TalepID)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT " +
                     "(CASE WHEN OnayDurumu=14 THEN 1 " +
                     "ELSE 0 END) AS OnayDurum " +
                     "FROM Tlp " +
                     "WHERE TalepID=" + TalepID + "";

        cmd = new SqlCommand(sorgu, DbConnUser);

        return (bool)Convert.ToBoolean(cmd.ExecuteScalar());
    }

    private int DepartmanYetkisiBul(string DepartmanKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT ISNULL(MAX(Yetki),0) AS Yetki FROM Kullanicilar AS K " +
                     "INNER JOIN Departmanlar AS D ON K.DepartmanID=D.DepartmanID " +
                     "WHERE D.DepartmanKodu='" + DepartmanKodu + "' AND K.Durum=1 AND K.KullaniciAdi<>'Admin'";

        cmd = new SqlCommand(sorgu, DbConnUser);

        return (int)Convert.ToInt32(cmd.ExecuteScalar());
    }

    protected void btnSiparisKapat_Click(object sender, EventArgs e)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        for (int i = 0; i < grdTaleplerimiz.Rows.Count; i++)
        {
            GridViewRow row = grdTaleplerimiz.Rows[i];
            bool isChecked = ((HtmlInputCheckBox)row.FindControl("chkSiparis")).Checked;
            HtmlInputCheckBox chk = ((HtmlInputCheckBox)row.FindControl("chkSiparis"));

            if (isChecked)
            {

                string TalepID = chk.Value.ToString() + ",";
                TalepID2 += TalepID;
            }
        }

        string[] SipTalepIDlerimiz = TalepID2.Split(',');

        if (SipTalepIDlerimiz.Length != 0)
        {
            int num5 = TalepID2.Length;
            TalepID2 = TalepID2.ToString().Substring(0, num5 - 1);
            Session["TalepIDler"] = TalepID2;
        }

        SipTalepIDlerimiz = null;

        SipTalepIDlerimiz = Session["TalepIDler"].ToString().Split(',');

        for (int i = 0; i <= SipTalepIDlerimiz.Length; i++)
        {
            string sorgu = "UPDATE Tlp SET OnayDurumu=7 WHERE TalepID=" + SipTalepIDlerimiz[i].ToString() + "";

            cmd = new SqlCommand(sorgu, DbConnUser);
            cmd.ExecuteNonQuery();
        }

        cmd.Dispose();

    }

    private string AnaDepartmanKodunuBuluyoruz()
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT AnaDepartmanKodu FROM AnaDepartman";

        cmd = new SqlCommand(sorgu, DbConnUser);

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

    private string SatinalmayaGonderiyoruzTeminYeriBul(string EvrakNo)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT TeminYeri FROM Tlp " +
                    "WHERE EvrakNoTarih+EvrakNo='" + EvrakNo + "' ";

        cmd = new SqlCommand(sorgu, DbConnUser);

        return (string)cmd.ExecuteScalar();
    }

    private string YetkiUnvaniBul(int YetkiKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT Yetki_Unvani FROM Yetki_Unvanlari WHERE YetkiKodu=" + YetkiKodu + "";

        cmd = new SqlCommand(sorgu, DbConnUser);

        return (string)cmd.ExecuteScalar();
    }

    private void EvrakNumaralariniSifirla(int Yil)
    {
        if (DbConnEvrakTarihUps.State == ConnectionState.Closed)
            DbConnEvrakTarihUps.Open();

        string sorgu = "UPDATE EvrakNumaralari SET CikisHareketNo=1,SiparisNo=1,Yil=" + Yil;
        cmdEvrakTarihUps = new SqlCommand(sorgu, DbConnEvrakTarihUps);
        cmdEvrakTarihUps.ExecuteNonQuery();
    }
}
