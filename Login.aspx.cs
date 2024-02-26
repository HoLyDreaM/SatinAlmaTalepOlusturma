using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Configuration;
using System.Web.Security;

public partial class Login : System.Web.UI.Page
{

    #region Bağlantı Ayarları

    SqlConnection DbUser = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnMenu = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnizinAyari = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnYetkiKoduBul = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnYetkiTamamla = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());

    SqlCommand cmd, cmdYetki, cmdYetkiTamamla, cmdMenu, cmdizinDurum;
    SqlDataReader dr, rd, dryetki, dryetkitamamla;
    bool izinDurum, KullaniciDurumKontrol;
    string KayitYetki;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Giris"] == "Evet")
        {
            Response.Redirect("Default.Aspx");
        }
    }

    protected void btnGiris_Click(object sender, EventArgs e)
    {
        if (HackEngelle(txtKullaniciAdi.Text) == "" || HackEngelle(txtPass.Text) == "")
        {
            lblError.Text = "Kullanıcı Adı Veya Şifreyi Boş Bıraktınız.Lütfen Kontrol Edip Tekrar Deneyin.";
            return;
        }

        try
        {
            if (DbUser.State == ConnectionState.Closed)
                DbUser.Open();

            string KullaniciAdi = txtKullaniciAdi.Text;
            string Sifre = txtPass.Text;

            KullaniciDurumKontrol = KullaniciDurumu(KullaniciAdi);

            if (KullaniciDurumKontrol == true)
            {
                string LoginSorgu = "SELECT USERID, KullaniciKodu, KullaniciAdi,AnaDepartman, " +
                                  "CONVERT(varchar,DECRYPTBYPASSPHRASE ('Editor',Sifre)) AS Sifre,DurumRaporu, " +
                                  "AdSoyad, D.DepartmanAdi,D.DepartmanKodu,D.DepartmanID,ISNULL(AD.AltDepartmanAdi,'') AS AltDepartmanAdi, " +
                                  "ISNULL(AD.AltDepartmanKodu,'') AS AltDepartmanKodu,ISNULL(AD.AltDepartmanID,0) AS AltDepartmanID,  " +
                                  "ISNULL(BD.AltDepartmanID,0) AS BagliOlduguSeflik, Yetki, LastLogin AS Tarih,MasrafMerkezi,MasrafMerkeziKodu,   " +
                                  "Birim,MenuKodu  " +
                                  "FROM Kullanicilar AS K " +
                                  "INNER JOIN Departmanlar AS D ON D.DepartmanID=K.DepartmanID " +
                                  "LEFT JOIN AltDepartman AS AD ON AD.AltDepartmanID=K.AltDepartmanID " +
                                  "LEFT JOIN AltDepartman AS BD ON BD.AltDepartmanID=K.BagliAltDepartman " +
                    //"LEFT JOIN Departmanlar AS D1 ON D1.DepartmanID=K.StokluDepartman " +
                    //"LEFT JOIN Departmanlar AS D2 ON D2.DepartmanID=K.StoksuzDepartman " +
                                  "WHERE KullaniciAdi='" + HackEngelle(txtKullaniciAdi.Text) + "' AND CONVERT(varchar,DECRYPTBYPASSPHRASE ('Editor',Sifre))='" + HackEngelle(txtPass.Text) + "'";

                cmd = new SqlCommand(LoginSorgu, DbUser);
                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    Session["Giris"] = "Evet";
                    Session["ID"] = dr["USERID"].ToString();
                    Session["KullAdi"] = dr["KullaniciAdi"].ToString();
                    Session["KullaniciKodu"] = dr["KullaniciKodu"].ToString();
                    Session["Name"] = dr["AdSoyad"].ToString();
                    Session["DepartmanAdi"] = dr["DepartmanAdi"].ToString();
                    Session["DepartmanKodu"] = dr["DepartmanKodu"].ToString();
                    Session["DepartmanID"] = dr["DepartmanID"].ToString();
                    Session["AltDepartmanAdi"] = dr["AltDepartmanAdi"].ToString();
                    Session["DurumRaporu"] = dr["DurumRaporu"].ToString();
                    string dsada = dr["AltDepartmanKodu"].ToString();
                    Session["AltDepartmanKodu"] = dr["AltDepartmanKodu"].ToString();
                    Session["AltDepartmanID"] = dr["AltDepartmanID"].ToString();
                    Session["AnaDepartman"] = dr["AnaDepartman"].ToString();
                    Session["BagliOlduguBirim"] = dr["BagliOlduguSeflik"].ToString();

                    //Session["StokluDepartmanAdi"] = dr["StokluDepartmanAdi"].ToString();
                    //Session["StokluDepartmanKodu"] = dr["StokluDepartmanKodu"].ToString();
                    //Session["StokluDepartmanID"] = dr["StokluDepartmanID"].ToString();

                    //Session["StoksuzDepartmanAdi"] = dr["StoksuzDepartmanAdi"].ToString();
                    //Session["StoksuzDepartmanKodu"] = dr["StoksuzDepartmanKodu"].ToString();
                    //Session["StoksuzDepartmanID"] = dr["StoksuzDepartmanID"].ToString();

                    string MasrafMerkezi = dr["MasrafMerkezi"].ToString();
                    bool MasrafMerkeziBool = Convert.ToBoolean(MasrafMerkezi);
                    Session["MasrafMerkeziDurumu"] = Convert.ToString(MasrafMerkeziBool);
                    Session["MasrafMerkeziKodu"] = dr["MasrafMerkeziKodu"].ToString();

                    Session["Yetki"] = dr["Yetki"].ToString();
                    Session["GirisTarihi"] = dr["Tarih"].ToString();
                    Session["MenuKodu"] = dr["MenuKodu"].ToString();
                    //cmd.Dispose();

                    Menuler(Session["MenuKodu"].ToString());

                    //YetkiKoduBul(Session["DepartmanKodu"].ToString(), Session["DepartmanKodu"].ToString(), Session["Yetki"].ToString());

                    string BagliOlduguDepartman = Session["BagliOlduguBirim"].ToString();

                    if (BagliOlduguDepartman == "0")
                    {
                        YetkiKoduBul(Session["DepartmanKodu"].ToString(), Session["KullaniciKodu"].ToString(), Session["Yetki"].ToString(), Session["AltDepartmanID"].ToString());
                    }
                    else
                    {
                        YetkiKoduBul(Session["DepartmanKodu"].ToString(), Session["KullaniciKodu"].ToString(), Session["Yetki"].ToString(), BagliOlduguDepartman);

                    }



                    dr.Dispose();
                    dr.Close();

                    string izinayari = "";

                    if (Convert.ToInt32(KayitYetki) == 1 || Convert.ToInt32(KayitYetki) == 2 || Convert.ToInt32(KayitYetki) == 3)
                    {
                        if (BagliOlduguDepartman == "0")
                        {
                            izinayari = izinDurumu(Session["DepartmanKodu"].ToString(), Session["KullaniciKodu"].ToString(), Session["Yetki"].ToString(), Session["AltDepartmanID"].ToString());
                        }
                        else
                        {
                            izinayari = izinDurumu(Session["DepartmanKodu"].ToString(), Session["KullaniciKodu"].ToString(), Session["Yetki"].ToString(), BagliOlduguDepartman);

                        }
                    }
                    else
                    {
                        izinayari = "İzinde Değil";
                    }

                    if (Convert.ToInt32(KayitYetki) == 0)
                    {
                        KayitYetki = "4";
                    }

                    Session["izinAyari"] = izinayari.ToString();

                    if (izinayari == "İzne Çıkmış")
                    {
                        Session["izinYetki"] = KayitYetki.ToString();
                        //Session["Yetki"] = KayitYetki.ToString();
                    }
                    else if (izinayari == "İzinde Değil")
                    {
                        Session["izinYetki"] = Session["Yetki"].ToString();
                    }

                    //dr.Dispose();
                    //dr.Close();
                    Response.Redirect("Default.Aspx");
                }
                else
                {
                    lblError.Text = "Kullanıcı Adı Veya Şifre Yanlış";

                }
            }
            else
            {
                lblError.Text = "Bu Kullanıcı Pasif Durumdadır";
            }

        }
        catch (Exception ex)
        {

            lblError.Text = "Bir Hata Oluştu.Hata Kodu " + ex;
        }
    }

    private void Menuler(string MenuKodu)
    {
        if (DbConnMenu.State == ConnectionState.Closed)
            DbConnMenu.Open();

        string Sorgu = "SELECT Anasayfa, StokKartiArama, ihtiyacArama, TalepAcma, Formlar, idariKisim, TeknikKisim, " +
                      "ihtiyacPusulasi, StokCikisFormu, StokCikisHareketi, SiparisFormu, Fax, Tanimlar, StokTanimlama, " +
                      "Kullanicilar, Tanimlamalar, Arama " +
                        "FROM MenuKontrolu " +
                        "WHERE MenuKodu='" + MenuKodu.ToString() + "'";
        cmdMenu = new SqlCommand(Sorgu, DbConnMenu);
        cmdMenu.CommandTimeout = 120;
        rd = cmdMenu.ExecuteReader();

        if (rd.Read())
        {
            Session["HomePage"] = rd["Anasayfa"].ToString();
            Session["StokKartiAra"] = rd["StokKartiArama"].ToString();
            Session["IhtiyacArama"] = rd["ihtiyacArama"].ToString();
            Session["Talepler"] = rd["TalepAcma"].ToString();
            Session["Formlar"] = rd["Formlar"].ToString();
            Session["idariKisim"] = rd["idariKisim"].ToString();
            Session["TeknikKisim"] = rd["TeknikKisim"].ToString();
            Session["ihtiyacPusulasi"] = rd["ihtiyacPusulasi"].ToString();
            Session["StokCikisFormu"] = rd["StokCikisFormu"].ToString();
            Session["StokCikisHareketi"] = rd["StokCikisHareketi"].ToString();
            Session["SiparisFormu"] = rd["SiparisFormu"].ToString();
            Session["FaxCekimi"] = rd["Fax"].ToString();
            Session["Tanimlar"] = rd["Tanimlar"].ToString();
            Session["Tanimlamalar"] = rd["Tanimlamalar"].ToString();
            Session["StokTanimlama"] = rd["StokTanimlama"].ToString();
            Session["KullaniciTanimi"] = rd["Kullanicilar"].ToString();
            Session["Aramalar"] = rd["Arama"].ToString();
        }

        rd.Dispose();
        rd.Close();

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

        cmdYetki = new SqlCommand(sorgu, DbConnYetkiKoduBul);
        cmdYetki.CommandTimeout = 120;
        dryetki = cmdYetki.ExecuteReader(CommandBehavior.CloseConnection);

        if (dryetki.Read())
        {
            KayitYetki = dryetki["Yetki"].ToString();
        }

        dryetki.Dispose();
        dryetki.Close();
    }

    private void YetkiTamamla(string Departman)
    {
        if (DbConnYetkiTamamla.State == ConnectionState.Closed)
            DbConnYetkiTamamla.Open();

        string sorgu = "SELECT ISNULL(MIN(Yetki),0) AS Yetki FROM Kullanicilar AS K " +
                    "INNER JOIN Departmanlar AS D ON K.DepartmanID=D.DepartmanID " +
                    "WHERE D.DepartmanKodu ='" + Departman.ToString() + "'";

        cmdYetkiTamamla = new SqlCommand(sorgu, DbConnYetkiTamamla);
        cmdYetkiTamamla.CommandTimeout = 120;
        dryetkitamamla = cmdYetkiTamamla.ExecuteReader(CommandBehavior.CloseConnection);

        if (dryetkitamamla.Read())
        {
            KayitYetki = dryetkitamamla["Yetki"].ToString();
        }

        dryetkitamamla.Dispose();
        dryetkitamamla.Close();
    }

    private string izinDurumu(string DepartmanKodu, string KullaniciKodu, string YetkiKodumuz, string AltDepartmanKodu)
    {
        if (DbConnizinAyari.State == ConnectionState.Closed)
            DbConnizinAyari.Open();

        string sorgu = "SELECT (CASE " +
                    "WHEN GETDATE() >= izin_Bastarih AND izin_BitTarih >= GETDATE() THEN 'İzne Çıkmış'" +
                    " ELSE 'İzinde Değil' " +
                    "END) AS izinDurumu FROM Kullanicilar AS K " +
                    "INNER JOIN Departmanlar AS D ON K.DepartmanID=D.DepartmanID " +
                    "WHERE D.DepartmanKodu='" + DepartmanKodu.ToString() + "' AND (AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " OR AltDepartmanID = 0) AND Yetki =(SELECT ISNULL(MIN(Yetki),0) AS Yetki FROM Kullanicilar AS K " +
                    "INNER JOIN Departmanlar AS D ON K.DepartmanID=D.DepartmanID " +
                    "WHERE D.DepartmanKodu='" + DepartmanKodu.ToString() + "'  AND (AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " OR AltDepartmanID = 0) " +
                    "AND KullaniciAdi <> '" + KullaniciKodu.ToString() + "' " +
                    "AND Yetki > " + Convert.ToInt32(YetkiKodumuz) + "  AND K.Durum=1 " +
                    "AND (MenuKodu<>'Satınalma' AND MenuKodu<>'Ambar') AND K.KullaniciAdi<>'Admin' )";

        cmdizinDurum = new SqlCommand(sorgu, DbConnizinAyari);
        cmdizinDurum.CommandTimeout = 120;
        return (string)cmdizinDurum.ExecuteScalar();
    }

    private string izinDurumuUst(string DepartmanKodu, string KullaniciKodu, string YetkiKodumuz, string AltDepartmanKodu)
    {
        if (DbConnizinAyari.State == ConnectionState.Closed)
            DbConnizinAyari.Open();

        string sorgu = "SELECT (CASE " +
                    "WHEN izin_BitTarih > GETDATE() THEN 'İzne Çıkmış'" +
                    "ELSE 'İzinde Değil' " +
                    "END) AS izinDurumu FROM Kullanicilar AS K " +
                    "INNER JOIN Departmanlar AS D ON K.DepartmanID=D.DepartmanID " +
                    "WHERE D.DepartmanKodu='" + DepartmanKodu.ToString() + "' AND Yetki =(SELECT ISNULL(MIN(Yetki),0) AS Yetki  " +
                    "FROM Kullanicilar AS K  " +
                    "INNER JOIN Departmanlar AS D ON K.DepartmanID=D.DepartmanID  " +
                    "WHERE D.DepartmanKodu='" + DepartmanKodu + "'  AND (AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " OR AltDepartmanID = 0)  " +
                    "AND KullaniciAdi = '" + KullaniciKodu + "' AND Yetki = " + YetkiKodumuz + "  AND K.Durum=1 AND (MenuKodu<>'Satınalma' AND MenuKodu<>'Ambar')  " +
                    "AND K.KullaniciAdi<>'Admin')";

        cmdizinDurum = new SqlCommand(sorgu, DbConnizinAyari);
        cmdizinDurum.CommandTimeout = 120;
        return (string)cmdizinDurum.ExecuteScalar();
    }

    private bool KullaniciDurumu(string KullaniciAdi)
    {
        if (DbUser.State == ConnectionState.Closed)
            DbUser.Open();

        string sorgu = "SELECT Durum FROM Kullanicilar " +
                     "WHERE KullaniciAdi='" + HackEngelle(KullaniciAdi) + "'";

        cmd = new SqlCommand(sorgu, DbUser);

        return (bool)Convert.ToBoolean(cmd.ExecuteScalar());
    }

    #region Sql Saldırılarını Engelliyoruz

    public static string HackEngelle(string Deger)
    {
        if (Deger != null)
        {
            Deger = Deger.Replace("'", "");
            Deger = Deger.Replace("++", "");
            Deger = Deger.Replace("--", "");
            Deger = Deger.Replace("=", "");
            Deger = Deger.Replace(";--", "");
            Deger = Deger.Replace(";", "");
            Deger = Deger.Replace("/*", "");
            Deger = Deger.Replace("*/", "");
            Deger = Deger.Replace("@@", "");
            Deger = Deger.Replace("@", "");
        }
        return Deger;
    }

    public static string HarfKontrolu(string Deger)
    {
        if (Deger != null)
        {
            Deger = Deger.Replace("İ", "I");
            Deger = Deger.Replace("Ş", "S");
            Deger = Deger.Replace("Ö", "O");
            Deger = Deger.Replace("Ü", "U");
            Deger = Deger.Replace("Ç", "C");
            Deger = Deger.Replace("Ğ", "G");
        }
        return Deger;
    }

    #endregion
}