using System;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using System.IO;
using DevExpress.Web;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;

public partial class UserControl_RezervKontrol : System.Web.UI.Page
{

    #region Bağlantı Ayarları

    SqlConnection DbConnUser = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlCommand cmd;
    SqlDataReader dr;
    DataSet ds;
    SqlDataAdapter da;
    int TabloSayisi;

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
                string MalKodu = Request.QueryString["MalKodu"].ToString(); ;
                string AltKategori = Request.QueryString["AltKategori"].ToString();
                string DepartmanKodu = Request.QueryString["DepartmanKodu"].ToString();

                RezervMiktari(MalKodu, Convert.ToInt32(AltKategori), DepartmanKodu);
            }

        }
    }

    private void RezervMiktari(string MalKodu, int AltDepartmanID, string DepartmanKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        ds = new DataSet();

        string Sorgu = "SELECT MalKodu,MalAdi,Miktar,AdSoyad,DepartmanAdi,AltDepartmanAdi FROM Tlp " +
                     "WHERE MalKodu='" + MalKodu.ToString() + "' AND AltDepartmanID=" + AltDepartmanID + " AND DepartmanKodu='" + DepartmanKodu + "' AND OnayDurumu<>13";

        da = new SqlDataAdapter(Sorgu, DbConnUser);
        da.Fill(ds, "Tlp");

        TabloSayisi = ds.Tables["Tlp"].Rows.Count;

        for (int i = 0; i < ds.Tables["Tlp"].Rows.Count; i++)
        {
            Literal Lt1 = new Literal();
            Lt1.Text = "<tr>" +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["Tlp"].Rows[i]["MalKodu"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["Tlp"].Rows[i]["MalAdi"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["Tlp"].Rows[i]["Miktar"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["Tlp"].Rows[i]["AdSoyad"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["Tlp"].Rows[i]["DepartmanAdi"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["Tlp"].Rows[i]["AltDepartmanAdi"].ToString() + "</td></tr> ";
            RezervMiktarlari.Controls.Add(Lt1);
        }

        //cmd.Dispose();
        DbConnUser.Dispose();
        DbConnUser.Close();

    }
}