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
using System.Web.UI.HtmlControls;

public partial class KullaniciDuzenle : System.Web.UI.Page
{

    #region Bağlantı Ayarları

    SqlConnection DbConnUser = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnLink = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnLink"].ToString());
    SqlCommand cmd;
    SqlDataReader dr;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["Giris"] != "Evet")
        //{
        //    Alert.Show("Lütfen Giriş Yaptıktan Sonra Tekrar Deneyin.");
        //    Response.Redirect("Login.Aspx");
        //}
        if (!Page.IsPostBack)
        {
            Kullanicilar();
        }
    }

    private void Kullanicilar()
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string UserID = Request.QueryString["ID"];

        if (!string.IsNullOrEmpty(UserID))
        {
            cmd = new SqlCommand("SELECT USERID,KullaniciKodu,AdSoyad, KullaniciAdi,  " +
                "CONVERT(varchar,DECRYPTBYPASSPHRASE ('Editor',Sifre)) AS Sifre,Yetki, " +
                "Birim,MenuKodu, " +
                "D.DepartmanAdi,D.DepartmanKodu,AD.AltDepartmanAdi,AD.AltDepartmanKodu, " +
                "Siparis,SatinAlma,MasrafMerkezi,MasrafMerkeziKodu,D1.DepartmanAdi AS StokluDepartmanAdi, " +
                "D1.DepartmanKodu AS StokluDepartmanKodu,D2.DepartmanAdi AS StoksuzDepartmanAdi, " +
                "D2.DepartmanKodu AS StoksuzDepartmanKodu " +
                "FROM Kullanicilar AS K " +
                "INNER JOIN Departmanlar AS D ON D.DepartmanID=K.DepartmanID " +
                "LEFT JOIN AltDepartman AS AD ON K.AltDepartmanID=AD.AltDepartmanID " +
                "LEFT JOIN Departmanlar AS D1 ON K.StokluDepartman=D1.DepartmanID " +
                "LEFT JOIN Departmanlar AS D2 ON K.StoksuzDepartman=D2.DepartmanID " +
                "INNER JOIN Yetki_Unvanlari AS Y ON K.Yetki=Y.YetkiKodu " +
                        "WHERE K.USERID= " + Convert.ToInt32(UserID) + " ", DbConnUser);

            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                txtKullaniciKodu.Text = dr["KullaniciKodu"].ToString();
                txtAdSoyad.Text = dr["AdSoyad"].ToString();
                txtKullaniciAdi.Text = dr["KullaniciAdi"].ToString();
                txtSifre.Text = dr["Sifre"].ToString();
                drpDepartmanlar.SelectedValue = dr["DepartmanKodu"].ToString();
                drpYetkiler.SelectedValue = dr["Yetki"].ToString();
            }

            cmd.Dispose();
            dr.Dispose();
            dr.Close();
        }
        else
        {
            Alert.Show("Kullanıcı Seçmediniz.Lütfen Tekrar Deneyin.");
        }

    }

    protected void btnKullaniciEdit_Click(object sender, EventArgs e)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string ID = Request.QueryString["ID"].ToString();
        string Sifre = HackEngelle(txtSifre.Text);
        string Sorgu =
        "UPDATE Kullanicilar SET KullaniciKodu= '" + txtKullaniciKodu.Text + "', AdSoyad = '" + txtAdSoyad.Text + "', " +
        "Sifre=EncryptByPassPhrase('Editor','" + Sifre.ToString() + "')," +
        "Departman = '" + drpDepartmanlar.SelectedValue.ToString() + "', " +
        "Yetki = '" + drpYetkiler.SelectedValue.ToString() + "' WHERE (USERID = " + Convert.ToInt32(ID) + ")";


        cmd = new SqlCommand(Sorgu, DbConnUser);

        cmd.ExecuteNonQuery();

        cmd.Dispose();
        DbConnUser.Dispose();
        DbConnUser.Close();
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

    #endregion
}