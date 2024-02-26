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

public partial class ProfilEdit : System.Web.UI.Page
{

    #region Bağlantı Ayarları

    SqlConnection DbConnUser = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnLink = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnLink"].ToString());
    SqlCommand cmd;
    SqlDataReader dr;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Giris"] != "Evet")
        {
            Response.Redirect("Login.Aspx");
        }
        else
        {
            if (!Page.IsPostBack)
            {
                Session["Sorgu"] = "";

                //dtizinBas.Date = DateTime.Now;
                //dtizinBit.Date = DateTime.Now;
                Kullanicilar();
            }
        }
    }

    private void Kullanicilar()
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string UserID = Request.QueryString["ID"];

        if (!string.IsNullOrEmpty(UserID))
        {
            cmd = new SqlCommand("SELECT USERID,KullaniciKodu,AdSoyad, KullaniciAdi, (CASE WHEN izin_BitTarih > GEtDATE() THEN 'True' " +
                                "ELSE 'False' END) AS izinDurum, izin_BasTarih, izin_BitTarih, " +
                            "CONVERT(varchar,DECRYPTBYPASSPHRASE ('Editor',Sifre)) AS Sifre,LastLogin AS 'Tarih' " +
                            "FROM Kullanicilar WHERE USERID='" + UserID + "'", DbConnUser);

            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                txtKullaniciKodu.Text = dr["KullaniciKodu"].ToString();
                txtAdSoyad.Text = dr["AdSoyad"].ToString();
                txtKullaniciAdi.Text = dr["KullaniciAdi"].ToString();
                txtSifre.Text = dr["Sifre"].ToString();
                dtizinBas.Date = Convert.ToDateTime(dr["izin_BasTarih"].ToString());
                dtizinBit.Date = Convert.ToDateTime(dr["izin_BitTarih"].ToString());

            }

            //dtizinBas.Date = DateTime.Now;
            //dtizinBit.Date = DateTime.Now;

            cmd.Dispose();
            dr.Dispose();
            dr.Close();
        }
        else
        {
            Alert.Show("Aradığınız Kullanıcı Bulunamadı.");
        }

    }

    protected void btnKullaniciEdit_Click(object sender, EventArgs e)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string UserID = Request.QueryString["ID"];
        string baslangic = dtizinBas.Date.ToString("yyyy-MM-dd 00:00:00");
        string bitis = dtizinBit.Date.ToString("yyyy-MM-dd 00:00:00");

        if (!string.IsNullOrEmpty(UserID))
        {

            string Sorgu = "UPDATE Kullanicilar SET AdSoyad='" + txtAdSoyad.Text + "',Sifre=EncryptByPassPhrase('Editor','" + txtSifre.Text + "'), " +
                           "izin_BasTarih='" + baslangic + "',izin_BitTarih='" + bitis + "'" +
                           "WHERE USERID='" + UserID + "'";

            cmd = new SqlCommand(Sorgu, DbConnUser);
            cmd.ExecuteNonQuery();

            DbConnUser.Close();

            Alert.Show("Bilgileriniz Başarılı Bir Şekilde Güncellenmiştir.");
        }
    }
}