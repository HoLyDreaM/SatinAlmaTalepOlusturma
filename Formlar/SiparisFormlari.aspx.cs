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

public partial class Formlar_SiparisFormlari : System.Web.UI.Page
{

    #region Bağlantı Ayarları

    SqlConnection DbConnUser = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnLink = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnLinkSorgu"].ToString());
    SqlCommand cmd, cmdCariHesap;
    SqlDataReader dr;
    DataSet ds;
    SqlDataAdapter da;
    string DepartmanKodu, ToplamID;
    int TabloSayisi;
    public HtmlInputCheckBox[] printer;
    string Linkdb;
    string SifDb;

    #endregion

    string[] SipParcala;

    protected void Page_Load(object sender, EventArgs e)
    {
        string LinkVeritabanı = ConfigurationManager.ConnectionStrings["DbConnLinkSorgu"].ToString();
        string SifVeritabani = ConfigurationManager.ConnectionStrings["DbConnUser"].ToString();

        string[] SifDizi = new string[6];
        string[] SifDizi2 = new string[3];
        string[] LinkDizi = new string[6];
        string[] LinkDizi2 = new string[3];
        LinkDizi = LinkVeritabanı.Split('=');
        LinkVeritabanı = LinkDizi[2].ToString();
        LinkDizi2 = LinkVeritabanı.Split(';');
        Linkdb = LinkDizi2[0].ToString();

        SifDizi = SifVeritabani.Split('=');
        SifVeritabani = SifDizi[2].ToString();
        SifDizi2 = SifVeritabani.Split(';');
        SifDb = SifDizi2[0].ToString();

        if (Session["Giris"] != "Evet")
        {
            Response.Redirect("../Login.Aspx");
        }
        else
        {
            SiparisFormuAl.OnClientClick = "MM_openBrWindow('SiparisFormu.Aspx','','toolbar=yes,location=yes,status=yes,resizable=yes,scrollbars=yes,width=900,height=550')";
            SipFormu();
        }
    }

    private void SipFormu()
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        ds = new DataSet();

        DepartmanKodu = Session["DepartmanKodu"].ToString();
        string Yetkili = Session["Yetki"].ToString();

        string sorgu = "CREATE TABLE #SiparisFormu( " +
                       "TalepID INT, " +
                       "EvrakNo NVARCHAR(25), " +
                       "SiparisNo NVARCHAR(25), " +
                       "Tarih NVARCHAR(25), " +
                       "Saat NVARCHAR(20), " +
                       "StokKodu NVARCHAR(150), " +
                       "StokAdi NVARCHAR(150),  " +
                       "Miktar NUMERIC(26,2), " +
                       "Birim NVARCHAR(50), " +
                       "ihtiyacYeri NVARCHAR(150), " +
                       "KullaniciIsmi NVARCHAR(150), " +
                       "KDV NUMERIC(26,2), " +
                       "BirimFiyat NUMERIC(26,6),  " +
                       "Firma NVARCHAR(150) " +
                       ")  " +
                       "INSERT INTO #SiparisFormu  " +
                       "SELECT T.TalepID,T.EvrakNoTarih+T.EvrakNo AS EvrakNo,S.STK002_EvrakSeriNo,CONVERT(VARCHAR(10),(CONVERT(DATETIME,Tarih-2)),104) AS Tarih,T.Saat, " +
                       "T.MalKodu,T.MalAdi AS 'Malzeme Adı',S.STK002_Miktari,T.Birim,i.IhtiyacAdi AS IhtiyacYeri,T.AdSoyad AS 'Kullanıcı İsmi', " +
                       "S.STK002_KDVTutari AS Kdv,S.STK002_BirimFiyati AS Fiyat,CR.CAR002_Unvan1 AS Firma " +
                       "FROM " + SifDb + ".dbo.Tlp AS T " +
                       "LEFT JOIN " + SifDb + ".dbo.Ihtiyaclar AS i on T.ihtiyacNedeni=i.IhtiyacKodu " +
                       "LEFT JOIN " + Linkdb + ".STK002 AS S ON T.EvrakNoTarih+T.EvrakNo=S.STK002_MalSeriNo " +
                       "LEFT JOIN " + Linkdb + ".CAR002 AS CR ON S.STK002_CariHesapKodu=CR.CAR002_HesapKodu " +
                       "WHERE (T.OnayDurumu IN(7,14)) AND (T.GidecekDepartman='" + DepartmanKodu + "') AND (T.SiparisFormu=0) " +
                       "GROUP BY T.TalepID,T.EvrakNoTarih+T.EvrakNo,S.STK002_EvrakSeriNo,Tarih,T.Saat,T.MalAdi,S.STK002_Miktari,T.MalKodu,CR.CAR002_Unvan1, " +
                       "i.IhtiyacAdi,T.OnayDurumu,T.AdSoyad,T.SipMiktar,S.STK002_KDVTutari,S.STK002_BirimFiyati,T.Birim " +
                       "ORDER BY T.TalepID DESC " +
                       "SELECT * FROM #SiparisFormu " +
                       "DROP TABLE #SiparisFormu";

        da = new SqlDataAdapter(sorgu, DbConnLink);
        da.Fill(ds, "SiparisFormu");

        TabloSayisi = ds.Tables["SiparisFormu"].Rows.Count;
        printer = new HtmlInputCheckBox[TabloSayisi];

        Literal Lt = new Literal();
        Lt.Text = "<table width=\"1300\" border=\"0\"><tr>" +
            "<td width=\"50\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Formu Al</td> " +
            "<td width=\"100\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">İhtiyaç No</td> " +
            "<td width=\"100\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Sipariş No</td> " +
            "<td width=\"100\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Firma Adı</td> " +
            "<td width=\"100\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Stok Kodu</td> " +
            "<td width=\"250\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Stok Adı</td> " +
            "<td width=\"150\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Tarih</td> " +
            "<td width=\"100\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Miktar</td> " +
            "<td width=\"100\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Birim</td> " +
            "<td width=\"100\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">İhtiyaç Yeri</td> " +
            "<td width=\"100\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Kullanıcı Adı</td> " +
            "<td width=\"100\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Kdv</td> " +
            "<td width=\"100\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Birim Fiyatı</td> " +
            "</<tr>";
        PanelSiparisFormlari.Controls.Add(Lt);

        for (int i = 0; i < ds.Tables["SiparisFormu"].Rows.Count; i++)
        {
            string firma = ds.Tables["SiparisFormu"].Rows[i]["Firma"].ToString();

            if (firma.ToString().Length > 15)
            {
                firma = firma.ToString().Substring(0, 15);
            }

            Literal Lt4 = new Literal();
            Lt4.Text = "<tr><td align=\"center\" style=\"border:1px solid Black;\">";
            PanelSiparisFormlari.Controls.Add(Lt4);

            string IDveSipNo = ds.Tables["SiparisFormu"].Rows[i]["TalepID"].ToString() + "," + ds.Tables["SiparisFormu"].Rows[i]["SiparisNo"].ToString();

            printer[i] = new HtmlInputCheckBox();
            printer[i].ID = "printer" + i.ToString();
            printer[i].Value = IDveSipNo;
            printer[i].Name = "printer" + i.ToString();
            PanelSiparisFormlari.Controls.Add(printer[i]);

            Literal Lt2 = new Literal();
            Lt2.Text = "</td>";

            PanelSiparisFormlari.Controls.Add(Lt2);

            Literal Lt1 = new Literal();
            Lt1.Text = "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["SiparisFormu"].Rows[i]["EvrakNo"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["SiparisFormu"].Rows[i]["SiparisNo"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + firma + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["SiparisFormu"].Rows[i]["StokKodu"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["SiparisFormu"].Rows[i]["StokAdi"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["SiparisFormu"].Rows[i]["Tarih"].ToString().Substring(0, 10) + " " + ds.Tables["SiparisFormu"].Rows[i]["Saat"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["SiparisFormu"].Rows[i]["Miktar"].ToString().Replace('.', ',') + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["SiparisFormu"].Rows[i]["Birim"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["SiparisFormu"].Rows[i]["ihtiyacYeri"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["SiparisFormu"].Rows[i]["KullaniciIsmi"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["SiparisFormu"].Rows[i]["KDV"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["SiparisFormu"].Rows[i]["BirimFiyat"].ToString() + "</td> ";
            PanelSiparisFormlari.Controls.Add(Lt1);
        }

        Literal Lt3 = new Literal();
        Lt3.Text = "</tr></table>";
        PanelSiparisFormlari.Controls.Add(Lt3);

        if (TabloSayisi <= 0)
        {
            SiparisFormuAl.Visible = false;
        }
        else
        {
            SiparisFormuAl.Visible = true;
        }

        BaglantilariKapat();
    }

    protected void btnGuncelle_Click(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SipFormu();
        }
    }

    private void BaglantilariKapat()
    {
        DbConnUser.Close();
        DbConnLink.Close();
    }

    protected void SiparisFormuAl_Click(object sender, ImageClickEventArgs e)
    {
        Session["SiparisTalepID"] = null;
        Session["SiparisEvrakNo"] = null;
        Session["LinkSiparisNo"] = null;
        Session["SiparisTalepSiparisNo"] = null;
        Session["SiparisMalKodu"] = null;
        Session["SiparisMalAdi"] = null;
        Session["SiparisBirim"] = null;
        Session["SiparisTarih"] = null;
        Session["SiparisMiktar"] = null;
        Session["SiparisBirimFiyat"] = null;
        Session["SiparisTutar"] = null;
        Session["SiparisTalepEden"] = null;
        Session["SiparisKdv"] = null;

        for (int i = 0; i < TabloSayisi; i++)
        {
            if (printer[i].Checked == true)
            {
                string BulSip = printer[i].Value.ToString();
                SipParcala = BulSip.Split(',');
                TalepBul(Convert.ToInt32(SipParcala[0].ToString()), SipParcala[1].ToString());
            }
        }
    }

    private void TalepBul(int TalepID,string SiparisNo)
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        string Sorgu = "SELECT T.TalepID,T.EvrakNoTarih+T.EvrakNo AS EvrakNo,CONVERT(VARCHAR(10),(CONVERT(DATETIME,Tarih-2)),104) AS Tarih,T.Saat, " +
                       "S.STK002_EvrakSeriNo AS SiparisNo,T.MalKodu,T.MalAdi AS MalzemeAdi,CONVERT(NUMERIC(18,2),S.STK002_Miktari) AS Miktar,T.Birim, " +
                       "i.IhtiyacAdi AS IhtiyacYeri,T.AdSoyad AS AdSoyad, " +
                       "S.STK002_KDVTutari AS Kdv,(CONVERT(NUMERIC(18,6),S.STK002_BirimFiyati)) AS Fiyat, " +
                       "CONVERT(NUMERIC(18,2),(S.STK002_Tutari)) AS Tutar " +
                       "FROM  " + SifDb + ".dbo.Tlp AS T " +
                       "INNER JOIN  " + SifDb + ".dbo.Ihtiyaclar AS i on T.ihtiyacNedeni=i.IhtiyacKodu " +
                       "INNER JOIN  " + Linkdb + ".STK002 AS S ON S.STK002_EvrakSeriNo='"+SiparisNo+"'  AND T.EvrakNoTarih+T.EvrakNo=S.STK002_MalSeriNo " +
                       "WHERE T.TalepID=" + TalepID + " " +
                       "GROUP BY T.TalepID,T.EvrakNoTarih+T.EvrakNo,Tarih,T.Saat,T.MalAdi,S.STK002_Miktari,T.MalKodu,S.STK002_Tutari, " +
                       "i.IhtiyacAdi,T.OnayDurumu,T.AdSoyad,S.STK002_KDVTutari,S.STK002_BirimFiyati,T.Birim,S.STK002_EvrakSeriNo " +
                       "ORDER BY T.TalepID DESC ";

        cmd = new SqlCommand(Sorgu, DbConnLink);
        cmd.CommandTimeout = 120;
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dr.Read())
        {
            Session["SiparisTalepID"] += dr["TalepID"].ToString() + ",";
            Session["SiparisEvrakNo"] += dr["EvrakNo"].ToString() + ",";
            Session["SiparisTalepSiparisNo"] += dr["SiparisNo"].ToString() + ",";
            Session["SiparisMalKodu"] += dr["MalKodu"].ToString() + ",";
            Session["SiparisMalAdi"] += dr["MalzemeAdi"].ToString() + "}";
            Session["SiparisBirim"] += dr["Birim"].ToString() + ",";
            Session["SiparisTarih"] += dr["Tarih"].ToString().Substring(0, 10) + " " + dr["Saat"].ToString() + ",";
            Session["SiparisMiktar"] += dr["Miktar"].ToString() + "/";
            Session["SiparisBirimFiyat"] += dr["Fiyat"].ToString() + "/";
            Session["SiparisKdv"] += dr["KDV"].ToString() + "/";
            Session["SiparisTutar"] += dr["Tutar"].ToString() + "/";
            Session["SiparisTalepEden"] += dr["AdSoyad"].ToString() + "}";
        }

        dr.Dispose();
        dr.Close();
    }

}