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
using System.IO;
using DevExpress.Web;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;

public partial class Formlar_ihtiyacPusulasi : System.Web.UI.Page
{

    #region Bağlantı Ayarları

    SqlConnection DbConnUser = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnLink = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnLink"].ToString());
    SqlCommand cmd;
    SqlDataReader dr;
    int TalepID;
    string SiparisNumarasi, ihtiyacNumarasi, AmbarDepartman, TeminYerDurumu, SipihtNo;

    string TalepID1, KullanilacakYer, ihtiyacNo, SiparisNo, StokKodu, StokAdi, EkleyenKisi, Tarih, Miktar, Birim, GelecekTarih,
           HizmetTanimi, Aciklama, SiparisCarisi, AmbarStokDurum, AmbarMevcut, KisimNo;

    string[] TalepID2, KullanilacakYer2, ihtiyacNo2, SiparisNo2, StokKodu2, StokAdi2, EkleyenKisi2, Tarih2, Miktar2, Birim2,
             GelecekTarih2, HizmetTanimi2, Aciklama2, SiparisCarisi2, AmbarStokDurum2, AmbarMevcut2, KisimNo2;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Giris"] != "Evet")
        {
            Response.Redirect("/Login.Aspx");
        }
        else
        {
            ihtiyacGetir();
        }
    }

    private void ihtiyacGetir()
    {

        TalepID1 = Session["ihtiyacTalepID"].ToString();
        ihtiyacNo = Session["ihtiyacEvrakNo"].ToString();
        SiparisNo = Session["ihtiyacSiparisNo"].ToString();
        StokKodu = Session["ihtiyacMalKodu"].ToString();
        StokAdi = Session["ihtiyacMalAdi"].ToString();
        EkleyenKisi = Session["ihtiyacDepartmanAdi"].ToString();
        Tarih = Session["ihtiyacTarih"].ToString();
        GelecekTarih = Session["ihtiyacTarih"].ToString();
        Miktar = Session["ihtiyacMiktar"].ToString();
        Birim = Session["ihtiyacBirim"].ToString();
        Aciklama = Session["ihtiyacAciklama"].ToString();
        KullanilacakYer = Session["ihtiyacKullanilacakYer"].ToString();
        SiparisCarisi = Session["ihtiyacCarisi"].ToString();

        TalepID2 = TalepID1.Split(',');
        ihtiyacNo2 = ihtiyacNo.Split(',');
        SiparisNo2 = SiparisNo.Split(',');
        StokKodu2 = StokKodu.Split(',');
        StokAdi2 = StokAdi.Split('}');
        EkleyenKisi2 = EkleyenKisi.Split(',');
        Tarih2 = Tarih.Split(',');
        GelecekTarih2 = GelecekTarih.Split(',');
        Miktar2 = Miktar.Split('/');
        Birim2 = Birim.Split(',');
        Aciklama2 = Aciklama.Split('}');
        KullanilacakYer2 = KullanilacakYer.Split(',');
        SiparisCarisi2 = SiparisCarisi.Split('/');

        for (int i = 0; i < ihtiyacNo2.Length - 1; i++)
        {
            AmbarDepartman = AmbarDepartmanBul(StokKodu2[i].ToString());
            string fffff = SiparisCarisi2[i].ToString();
            decimal AmbarStokDurum = AmbarStok(AmbarDepartman, StokKodu2[i].ToString());
            AmbarMevcut = Convert.ToString(AmbarStokDurum);
            TalepID = Convert.ToInt32(TalepID2[i].ToString());
            KisimNo = TalepAcanDepartmani(TalepID);
            TeminYerDurumu = TeminYeriBul(TalepID);

            SipihtNo = SiparisNo2[i].ToString();

            if (string.IsNullOrEmpty(SipihtNo))
            {
                SipihtNo = "&nbsp;";
            }

            Literal Lt = new Literal();
            Lt.Text = "<table width=\"100%\" border=\"0\" cellpadding=\"0\" style=\"font-family:Calibri; font-size:13px;\"> " +
    "<tr> " +
        "<td height=\"50\" style=\"border:1px solid Black;\" align=\"left\" class=\"YaziStili\"> " +
        "<img src=\"../images/sifaslogo.jpg\" width=\"100\" height=\"65\" /><br />SENTETİK İPLİK FABRİKALARI A.Ş " +
        "</td> " +
        "<td height=\"50\" colspan=\"3\" align=\"center\" style=\"border:1px solid Black; font-family:Calibri; font-size:18px; font-weight:bold;\"> " +
     "<center>İHTİYAÇ PUSULASI</center></td> " +
        "<td colspan=\"2\" style=\"border:1px solid Black;\" class=\"YaziStili\"><b>No :</b> " + ihtiyacNo2[i].ToString() + "<br /><b>Tarih :</b>" + Tarih2[i].ToString() + "</td> " +
     "</tr> " +
     "<tr> " +
        "<td width=\"120\" height=\"50\" style=\"border:1px solid Black;\" align=\"left\" class=\"YaziStili\">Sipariş No</td> " +
        "<td width=\"120\" style=\"border:1px solid Black;\">" + SipihtNo.ToString() + " </td> " +
        "<td width=\"120\" style=\"border:1px solid Black;\" align=\"left\" class=\"YaziStili\">Siparişi Veren Kısım</td> " +
        "<td width=\"400\" style=\"border:1px solid Black;\" align=\"left\">" + EkleyenKisi2[i].ToString() + "</td> " +
        "<td colspan=\"2\" align=\"center\" style=\"border:1px solid Black;\" class=\"YaziStili\">Gelmesi İstenen Tarih</td> " +
    "</tr> " +
    "<tr> " +
        "<td style=\"border:1px solid Black;\" align=\"left\" class=\"YaziStili\">Kısım No</td> " +
        "<td style=\"border:1px solid Black;\">&nbsp;</td> " +
        "<td style=\"border:1px solid Black;\" align=\"left\" class=\"YaziStili\">Kullanılacak Yer</td> " +
        "<td style=\"border:1px solid Black;\" align=\"left\">" + KullanilacakYer2[i].ToString() + "</td> " +
        "<td colspan=\"2\" align=\"center\" style=\"border:1px solid Black;\">" + GelecekTarih2[i].ToString() + "</td> " +
    "</tr> " +
    "<tr> " +
        "<td colspan=\"2\" style=\"border:1px solid Black;\" align=\"left\" class=\"YaziStili\">Stok Kodu</td> " +
        "<td colspan=\"2\" style=\"border:1px solid Black;\" align=\"left\">" + StokKodu2[i].ToString() + "</td> " +
        "<td width=\"113\" rowspan=\"2\" align=\"center\" class=\"YaziStili\" style=\"border:1px solid Black;\">Miktar</td> " +
        "<td width=\"100\" rowspan=\"2\" align=\"center\" class=\"YaziStili\" style=\"border:1px solid Black;\">Birim</td> " +
     "</tr> " +
     "<tr> " +
       "<td height=\"75\" colspan=\"4\" rowspan=\"3\" align=\"left\" valign=\"top\" class=\"YaziStili\"  " +
            "style=\"border:1px solid Black;\">Malın Hizmetin Tanımı : " + StokAdi2[i].ToString() + "<br />Açıklama : " + Aciklama2[i].ToString() + "</td> " +
     "</tr> " +
     "<tr> " +
        "<td height=\"68\" align=\"center\" class=\"YaziStili\" style=\"border:1px solid Black;\">" + Miktar2[i].ToString().Replace('.', ',') + "</td> " +
        "<td style=\"border:1px solid Black;\" align=\"center\" class=\"YaziStili\">" + Birim2[i].ToString() + "</td> " +
     "</tr> " +
     "<tr> " +
        "<td rowspan=\"2\" align=\"center\" class=\"YaziStili\" style=\"border:1px solid Black;\">Ambar Mevcudu</td> " +
        "<td rowspan=\"2\" align=\"center\" class=\"YaziStili\" style=\"border:1px solid Black;\">" + AmbarMevcut.ToString().Replace('.', ',') + "</td> " +
        "</tr><tr><td colspan=\"4\" style=\"border:1px solid Black;\" align=\"left\" class=\"YaziStili\"> " +
        "Temin Edilecek Firma : " + SiparisCarisi2[i].ToString() + "</td> " +
        "</tr> " +
        "<tr> " +
       "<td align=\"left\" class=\"YaziStili\">&nbsp;</td> " +
       "<td>&nbsp;</td> " +
       "<td align=\"left\" class=\"YaziStili\">&nbsp;</td> " +
       "<td align=\"left\">&nbsp;</td> " +
       "<td colspan=\"2\" align=\"center\">&nbsp;</td> " +
  "</tr> " +
 "</table>";

            panelihtiyacListesi.Controls.Add(Lt);
        }

        //TalepID = Convert.ToInt32(Session["ihtiyacTalepID"].ToString());

        //lblKullanilacakYer.Text = Session["ihtiyacKullanilacakYer"].ToString();
        //lblihtiyacNo.Text = Session["ihtiyacEvrakNo"].ToString();
        //lblSiparisNo.Text = Session["ihtiyacSiparisNo"].ToString();
        //lblStokKodu.Text = Session["ihtiyacMalKodu"].ToString();
        ////lblSaticiDurumu.Text = SaticiDurumu(lblStokKodu.Text.ToString());
        ////lblStokAdi.Text = Session["ihtiyacMalAdi"].ToString();
        //lblEkleyenKisi.Text = Session["ihtiyacDepartmanAdi"].ToString();
        //lblTarih.Text = Session["ihtiyacTarih"].ToString();
        //lblMiktar.Text = Session["ihtiyacMiktar"].ToString();
        //lblBirim.Text = Session["ihtiyacBirim"].ToString();
        //lblGelmesiTarih.Text = Session["ihtiyacTarih"].ToString();
        //lblHizmetTanimi.Text = Session["ihtiyacMalAdi"].ToString();
        //lblAciklama.Text = Session["ihtiyacAciklama"].ToString();
        //lblSiparisCarisi.Text = Session["ihtiyacCarisi"].ToString();
        //AmbarDepartman = AmbarDepartmanBul(lblStokKodu.Text);
        //decimal AmbarStokDurum = AmbarStok(AmbarDepartman, lblStokKodu.Text);
        //AmbarMevcut.InnerText = Convert.ToString(AmbarStokDurum);
        //kisimNo.InnerText = TalepAcanDepartmani(TalepID);
    }

    protected void imgYazdir_Click(object sender, ImageClickEventArgs e)
    {
        string TalepEvrakNoKontrol = Session["ihtiyacEvrakNo"] as string;

        if (string.IsNullOrEmpty(TalepEvrakNoKontrol))
        {
            Alert.Show("Yazdırılacak Veri Bulunamadı.");
            ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
        }
        else
        {
            ihtiyacNo = Session["ihtiyacEvrakNo"].ToString();
            ihtiyacNo2 = ihtiyacNo.Split(',');

            TalepID1 = Session["ihtiyacTalepID"].ToString();
            TalepID2 = TalepID1.Split(',');

            if (DbConnUser.State == ConnectionState.Closed)
                DbConnUser.Open();

            for (int i = 0; i < ihtiyacNo2.Length - 1; i++)
            {
                string Sorgu = "UPDATE Tlp SET " +
                               "ihtiyacPusulasi=1 " +
                               "WHERE TalepID=" + TalepID2[i].ToString() + "";
                cmd = new SqlCommand(Sorgu, DbConnUser);
                cmd.CommandTimeout = 120;
                cmd.ExecuteNonQuery();
            }

            BaglantilariKapat();
        }
    }

    private string SaticiDurumu(string MalKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT dbo.fn_SaticiDurumu('" + MalKodu.ToString() + "') AS SaticiDurumu";

        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();
    }

    private void BaglantilariKapat()
    {
        DbConnUser.Close();
        DbConnLink.Close();
    }

    private decimal AmbarStok(string DepoKodu, string MalKodu)
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        string sorgu = "SELECT ISNULL((SUM( " +
                    "CASE WHEN STK1.STK005_GC = 0 AND STK1.STK005_Depo='" + DepoKodu + "' THEN STK1.STK005_Miktari ELSE 0 END) -  SUM( " +
                    "CASE WHEN STK1.STK005_GC = 1 AND STK1.STK005_Depo='" + DepoKodu + "' THEN STK1.STK005_Miktari ELSE 0 END)),0) AS AmbarStok " +
                    "FROM STK004 AS STK " +
                    "LEFT JOIN STK005 AS STK1 ON STK.STK004_MalKodu=STK1.STK005_MalKodu " +
                    "WHERE STK.STK004_MalKodu='" + MalKodu + "'";

        cmd = new SqlCommand(sorgu, DbConnLink);

        return (decimal)Convert.ToDecimal(cmd.ExecuteScalar());
    }

    private string AmbarDepartmanBul(string MalKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT TOP(1) GidecekKod FROM GidecekDepartman " +
                    "WHERE GidecekDepartman=(SELECT DepartmanDurumu FROM StokKodlari " +
                    "WHERE StokKodu='" + MalKodu + "') AND ID=1";
        cmd = new SqlCommand(sorgu, DbConnUser);
        return (string)cmd.ExecuteScalar();
    }

    private string TalepAcanDepartmani(int TalepID)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT DepartmanAdi AS KisimNo FROM Tlp WHERE TalepID=" + TalepID + "";

        cmd = new SqlCommand(sorgu, DbConnUser);

        return (string)cmd.ExecuteScalar();
    }

    private string TeminYeriBul(int TalepID)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT DepartmanAdi FROM Departmanlar " +
                    "WHERE DepartmanKodu=(SELECT TeminYeri FROM Tlp WHERE TalepID=" + TalepID + ")";

        cmd = new SqlCommand(sorgu, DbConnUser);

        return (string)cmd.ExecuteScalar();
    }
}