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

public partial class StokSorgulama : System.Web.UI.Page
{

    #region Bağlantı Ayarları

    SqlConnection DbConnLink = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnLink"].ToString());
    SqlCommand cmd;
    SqlDataReader dr;
    string Sorgu, dsSorgu;
    bool GridDurum;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        //dsSorgu = Convert.ToString(Session["Sorgu"]);

        if (string.IsNullOrEmpty(Convert.ToString(Session["Sorgu"])))
        {
            Session["Sorgu"] = "SELECT STK004_MalKodu AS 'Mal Kodu',STK004_Aciklama AS 'Açıklama', " +
                               "STK004_Birim1 AS Birim,STK004_TipKodu AS 'Tip Kodu',STK004_OzelKodu AS 'Özel Kodu', " +
                               "STK004_GrupKodu AS 'Grup Kodu', " +
                               "((STK004_GirisMiktari+STK004_DevirMiktari)-STK004_CikisMiktari) AS 'Stok Miktarı' " +
                               "FROM STK004 WITH(NOLOCK)  " +
                               "WHERE STK004_MalKodu ='1'" +
                               "ORDER BY STK004_MalKodu";

            dsSorgu = Session["Sorgu"].ToString();
        }
        else
        {
            dsSorgu = Session["Sorgu"].ToString();
        }

        SqlSorgulama.SelectCommand = dsSorgu.ToString();
    }

    protected void btnStokArama_Click(object sender, EventArgs e)
    {

        SqlSorgulama.SelectCommand = "";
        string Where = "";

        if (string.IsNullOrEmpty(txtStokKodu.Text))
        {
            Where = "WHERE STK004_Aciklama LIKE '%" + txtStokAdi.Text + "%'  ";
        }
        if (string.IsNullOrEmpty(txtStokAdi.Text))
        {
            Where = "WHERE STK004_MalKodu LIKE '%" + txtStokKodu.Text + "%'  ";
        }
        if (string.IsNullOrEmpty(txtStokKodu.Text) && string.IsNullOrEmpty(txtStokAdi.Text))
        {
            Alert.Show("Stok Kodu Ve Stok Adı Boş Geçilemez.Lütfen Kontrol Edip Tekrar Deneyin.");
            return;
        }
        else
        {
            Sorgu = "SELECT STK004_MalKodu AS 'Mal Kodu',STK004_Aciklama AS 'Açıklama', " +
                           "STK004_Birim1 AS Birim,STK004_TipKodu AS 'Tip Kodu',STK004_OzelKodu AS 'Özel Kodu', " +
                           "STK004_GrupKodu AS 'Grup Kodu',CONVERT(NUMERIC(18,3),((STK004_GirisMiktari+STK004_DevirMiktari)-STK004_CikisMiktari)) AS 'Stok Miktarı' " +
                           "FROM STK004 WITH(NOLOCK)  " +
                            Where +
                           "ORDER BY STK004_MalKodu";

            SqlSorgulama.SelectCommand = Sorgu;
            Session["Sorgu"] = Sorgu;
            dsSorgu = Sorgu;
        }
    }

}