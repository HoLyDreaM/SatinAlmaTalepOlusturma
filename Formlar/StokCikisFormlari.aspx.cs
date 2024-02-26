﻿using System;
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

public partial class Formlar_StokCikisFormlari : System.Web.UI.Page
{

    #region Bağlantı Ayarları

    SqlConnection DbConnUser = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnLink = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnLink"].ToString());
    SqlConnection DbConnLinkSorgu = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnLinkSorgu"].ToString());
    SqlCommand cmd, cmds;
    SqlDataReader dr;
    DataSet ds;
    SqlDataAdapter da;
    string DepartmanKodu, ToplamID;
    int TabloSayisi;
    public HtmlInputCheckBox[] printer;
    string Linkdb;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Giris"] != "Evet")
        {
            Response.Redirect("../Login.Aspx");
        }
        else
        {
            string TalepIDKontrol = Session["ihtiyacTalepID"] as string;

            if (!string.IsNullOrEmpty(TalepIDKontrol))
            {
                imgYaziciyaGonder.OnClientClick = "MM_openBrWindow('StokCikisFormu.Aspx','','toolbar=yes,location=yes,status=yes,resizable=yes,scrollbars=yes,width=900,height=550')";
            }
            else
            {
                Session["ihtiyacTalepID"] = "1";
            }
            StokCikis();
        }
    }

    private void StokCikis()
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        ds = new DataSet();

        DepartmanKodu = Session["DepartmanKodu"].ToString();
        string Yetkili = Session["Yetki"].ToString();

        string sorgu = "CREATE TABLE #StokCksFormu( " +
                       "TalepID INT,   " +
                       "EvrakNo NVARCHAR(50),   " +
                       "MalKodu NVARCHAR(150),  " +
                       "MalAdi NVARCHAR(150),   " +
                       "DepartmanAdi NVARCHAR(150), " +
                       "AdSoyad NVARCHAR(250),   " +
                       "Tarih NVARCHAR(150),  " +
                       "Saat NVARCHAR(50), " +
                       "Miktar NUMERIC(25,2),   " +
                       "KullanilacakYer NVARCHAR(150)   " +
                       ")   " +
                       "INSERT INTO #StokCksFormu   " +
                       "SELECT T.TalepID,T.EvrakNoTarih+T.EvrakNo AS ihtiyacNo,T.MalKodu,T.MalAdi,  " +
                       "T.DepartmanAdi,T.AdSoyad,CONVERT(VARCHAR(10),(CONVERT(DATETIME,Tarih-2)),104) AS Tarih,T.Saat,   " +
                       "T.Miktar,T.MasrafMerkeziAdi AS KullanilacakYer  " +
                       "FROM Tlp AS T   " +
            //"INNER JOIN TalepAkisi AS TA ON T.TalepID = TA.TalepID   " +
                       "WHERE (T.OnayDurumu IN(4,13)) AND (T.GidecekDepartman='" + DepartmanKodu + "') AND (CASE " +
                       "WHEN T.OnaylayanYetki = " + Convert.ToInt32(Yetkili) + "  THEN 1 " +
                       "ELSE 0 END)=1 " +
                       "AND (T.StokCikisFormu = 0) AND (T.StokCikisHareketi = 1) " +
                       "GROUP BY T.TalepID,T.EvrakNoTarih+T.EvrakNo,T.MalKodu,T.MalAdi,   " +
                       "T.DepartmanAdi,T.AdSoyad,Tarih,T.Saat,   " +
                       "T.Miktar,T.MasrafMerkeziAdi   " +
                       "ORDER BY CONVERT(DATETIME,T.Tarih-2) DESC   " +
                       "SELECT * FROM #StokCksFormu   " +
                       "DROP TABLE #StokCksFormu";

        da = new SqlDataAdapter(sorgu, DbConnUser);
        da.Fill(ds, "StokCksFormu");

        TabloSayisi = ds.Tables["StokCksFormu"].Rows.Count;
        printer = new HtmlInputCheckBox[TabloSayisi];

        Literal Lt = new Literal();
        Lt.Text = "<table width=\"1200\" border=\"0\"><tr> " +
            "<td width=\"50\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Çıktı Al</td></<tr> " +
                        "<td width=\"75\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">İhtiyaç No</td> " +
            "<td width=\"120\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Stok Kodu</td> " +
            "<td width=\"250\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Stok Adı</td> " +
            "<td width=\"110\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Departman Adı</td> " +
            "<td width=\"110\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Ad Soyad</td> " +
            "<td width=\"150\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Tarih</td> " +
            "<td width=\"75\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Miktar</td> " +
            "<td width=\"100\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Masraf Yeri</td> ";
        panelStokCikisFormlari.Controls.Add(Lt);

        for (int i = 0; i < ds.Tables["StokCksFormu"].Rows.Count; i++)
        {

            Literal Lt4 = new Literal();
            Lt4.Text = "<tr><td align=\"center\" style=\"border:1px solid Black;\">";
            panelStokCikisFormlari.Controls.Add(Lt4);

            printer[i] = new HtmlInputCheckBox();
            printer[i].ID = "printer" + i.ToString();
            printer[i].Value = ds.Tables["StokCksFormu"].Rows[i]["TalepID"].ToString();
            printer[i].Name = "printer" + i.ToString();
            panelStokCikisFormlari.Controls.Add(printer[i]);

            Literal Lt2 = new Literal();
            Lt2.Text = "</td>";

            panelStokCikisFormlari.Controls.Add(Lt2);

            Literal Lt1 = new Literal();
            Lt1.Text = "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["StokCksFormu"].Rows[i]["EvrakNo"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["StokCksFormu"].Rows[i]["MalKodu"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["StokCksFormu"].Rows[i]["MalAdi"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["StokCksFormu"].Rows[i]["DepartmanAdi"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["StokCksFormu"].Rows[i]["AdSoyad"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["StokCksFormu"].Rows[i]["Tarih"].ToString().Substring(0, 10) + " " + ds.Tables["StokCksFormu"].Rows[i]["Saat"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["StokCksFormu"].Rows[i]["Miktar"].ToString().Replace('.', ',') + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["StokCksFormu"].Rows[i]["KullanilacakYer"].ToString() + "</td> ";
            panelStokCikisFormlari.Controls.Add(Lt1);

            //printer[i] = new HtmlInputCheckBox();
            //printer[i].ID = "printer" + i.ToString();
            //printer[i].Value = ds.Tables["StokCksFormu"].Rows[i]["TalepID"].ToString();
            //printer[i].Name = "printer" + i.ToString();
            //panelStokCikisFormlari.Controls.Add(printer[i]);

            //Literal Lt2 = new Literal();
            //Lt2.Text = "</td>";
            //panelStokCikisFormlari.Controls.Add(Lt2);
        }

        Literal Lt3 = new Literal();
        Lt3.Text = "</tr></table>";
        panelStokCikisFormlari.Controls.Add(Lt3);

        if (TabloSayisi <= 0)
        {
            imgYaziciyaGonder.Visible = false;
        }
        else
        {
            imgYaziciyaGonder.Visible = true;
        }

        BaglantilariKapat();
    }

    protected void btnGuncelle_Click(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            StokCikis();
        }
    }

    protected void imgYaziciyaGonder_Click(object sender, ImageClickEventArgs e)
    {
        Session["StokCikisTalepIDleri"] = null;

        for (int i = 0; i < TabloSayisi; i++)
        {
            if (printer[i].Checked == true)
            {
                ToplamID += printer[i].Value.ToString() + ",";
            }
        }

        Session["StokCikisTalepIDleri"] = ToplamID.ToString();
        BaglantilariKapat();
    }

    private void BaglantilariKapat()
    {
        DbConnUser.Close();
        DbConnLink.Close();
        DbConnLinkSorgu.Close();
    }

}