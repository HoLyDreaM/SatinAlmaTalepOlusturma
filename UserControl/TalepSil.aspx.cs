using System;
using System.Collections.Generic;
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

public partial class UserControl_TalepSil : System.Web.UI.Page
{

    #region Bağlantı Ayarları

    SqlConnection DbConnUser = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnKaydeden = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnilkislem = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlCommand cmd, cmdKaydeden, cmdilkislem;
    SqlDataReader dr;
    int TalepID;
    string Kaydeden, KullaniciKodu;
    bool ilkIslem;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Giris"] != "Evet")
        {
            Alert.Show("Lütfen Sisteme Tekrardan Giriş Yapınız.");
            ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
        }
        else
        {
            if (!Page.IsPostBack)
            {
                string KontrolTalepID = Request.QueryString["TalepID"].ToString();
                TalepID = Convert.ToInt32(KontrolTalepID);
                string EvrakNumaramiz = EvrakNumarasiBul(TalepID);

                if (MessageBox.Show(EvrakNumaramiz + " No lu İhtiyacı Silmek İstediğinize Emin Misiniz?", "İhtiyaç Sil", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    KullaniciKodu = Session["KullaniciKodu"].ToString();
                    Kaydeden = KaydedenBul(TalepID);
                    ilkIslem = ilkislemBul(TalepID);

                    if (KullaniciKodu == Kaydeden && ilkIslem == false)
                    {
                        TalepSil(TalepID);
                        //TalepAkisSil(TalepID);

                        BaglantilariKapat();

                        Alert.Show("İhtiyaç Başarılı Bir Şekilde Silinmiştir.");
                        ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
                    }
                    else
                    {
                        Alert.Show("Bu İhtiyaç İşlem Gördüğü İçin Silme İşlemi Yapılamaz.");
                        ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
                }
            }
        }
    }

    private string KaydedenBul(int TalepID)
    {
        if (DbConnKaydeden.State == ConnectionState.Closed)
            DbConnKaydeden.Open();

        string sorgu = "SELECT Kaydeden FROM Tlp " +
                    "WHERE TalepID=" + TalepID + "";

        cmdKaydeden = new SqlCommand(sorgu, DbConnKaydeden);
        cmdKaydeden.CommandTimeout = 120;
        return (string)cmdKaydeden.ExecuteScalar();
    }

    private bool ilkislemBul(int TalepID)
    {
        if (DbConnilkislem.State == ConnectionState.Closed)
            DbConnilkislem.Open();

        string sorgu = "SELECT ilkIslem FROM Tlp " +
                        "WHERE TalepID=" + TalepID + "";
        cmdilkislem = new SqlCommand(sorgu, DbConnilkislem);
        cmdilkislem.CommandTimeout = 120;

        return (bool)cmdilkislem.ExecuteScalar();
    }

    private void TalepSil(int TalepID)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "DELETE FROM Tlp WHERE TalepID=" + TalepID + "";

        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        cmd.ExecuteNonQuery();
    }

    private void TalepAkisSil(int TalepID)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "DELETE FROM TalepAkisi WHERE TalepID=" + TalepID + "";

        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        cmd.ExecuteNonQuery();
    }

    private string EvrakNumarasiBul(int TalepID)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT EvrakNoTarih+EvrakNo AS EvrakNo FROM Tlp " +
                        "WHERE TalepID=" + TalepID + "";

        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();
    }

    private void BaglantilariKapat()
    {
        //DbConnUser.Dispose();
        DbConnUser.Close();
        //DbConnKaydeden.Dispose();
        DbConnKaydeden.Close();
        //DbConnilkislem.Dispose();
        DbConnilkislem.Close();

        //cmd.Dispose();
        //cmdKaydeden.Dispose();
        //cmdilkislem.Dispose();

        //dr.Dispose();
        //dr.Close();
    }
}