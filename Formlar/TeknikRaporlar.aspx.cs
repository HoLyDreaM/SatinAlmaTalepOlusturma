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

public partial class Formlar_TeknikRaporlar : System.Web.UI.Page
{

    #region Bağlantı Ayarları

    SqlConnection DbConnUser = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnLink = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnLink"].ToString());
    SqlConnection DbConnLinkSorgu = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnLinkSorgu"].ToString());
    SqlCommand cmd, cmds;
    SqlDataReader dr;
    DataSet ds;
    SqlDataAdapter da;
    string DepartmanKodu;
    int TabloSayisi;
    public HtmlInputCheckBox[] printer;
    string Linkdb;
    string SifDb;
    string Dizimiz;
    string[] Dizimiz2;
    decimal Miktarimiz, Tutarimiz;
    string MalKodumuzNedir, AciklamaSatirimiz;

    #endregion

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
            string TalepIDKontrol = Session["TeknikTalepID"] as string;

            if (!string.IsNullOrEmpty(TalepIDKontrol))
            {
                imgYaziciyaGonder.OnClientClick = "MM_openBrWindow('TeknikRapor.Aspx','','toolbar=yes,location=yes,status=yes,resizable=yes,scrollbars=yes,width=900,height=550')";
            }
            else
            {
                Session["TeknikTalepID"] = "1";
            }
            TeknikKisimTablomuz();
        }
    }

    private void TeknikKisimTablomuz()
    {
        if (DbConnLinkSorgu.State == ConnectionState.Closed)
            DbConnLinkSorgu.Open();

        ds = new DataSet();

        DepartmanKodu = Session["DepartmanKodu"].ToString();
        string Yetkili = Session["Yetki"].ToString();

        string sorgu = "CREATE TABLE #TeknikKisimTablo( " +
                       "TalepID INT, " +
                       "EvrakNo NVARCHAR(50), " +
                       "StokAdi NVARCHAR(250), " +
                       "Aciklama NVARCHAR(250), " +
                       "Miktar NUMERIC(25,2), " +
                       "Birim NVARCHAR(50), " +
                       "BirimFiyat NUMERIC(25,2), " +
                       "Tutar NUMERIC(25,2), " +
                       "TalepEden NVARCHAR(150), " +
                       "KullanilacakDepartman NVARCHAR(250), " +
                       "Firma NVARCHAR(250) " +
                       ") " +
                       "INSERT INTO #TeknikKisimTablo " +
                        "SELECT T.TalepID,T.EvrakNoTarih+T.EvrakNo AS EvrakNo,T.MalAdi,T.Aciklama+T.Aciklama2+T.Aciklama3, " +
                        "(CASE " +
                        "WHEN S2.STK002_Miktari > 0 THEN STK002_Miktari " +
                        "ELSE T.Miktar END) AS Miktar,T.Birim,  " +
                        "(CASE " +
                        "WHEN S2.STK002_BirimFiyati > 0 THEN S2.STK002_BirimFiyati " +
                        "ELSE S4.STK004_SonAlimFaturasiBirimFiyati END) AS BirimFiyati, " +
                        "(CASE " +
                        "WHEN S2.STK002_Tutari > 0 THEN S2.STK002_Tutari " +
                        "ELSE T.Miktar*S4.STK004_SonAlimFaturasiBirimFiyati END) AS Tutar, " +
                        "T.DepartmanAdi AS TalepEden,T.MasrafMerkeziAdi AS KullanilacakDepartman, " +
                        "ISNULL(CR.CAR002_Unvan1+CAR002_Unvan2,'  ') AS Firma " +
                        "FROM " + SifDb + ".dbo.Tlp AS T " +
                        "LEFT JOIN " + Linkdb.ToString() + ".STK002 AS S2 ON T.EvrakNoTarih+T.EvrakNo=S2.STK002_EvrakSeriNo2 " +
                        "LEFT JOIN " + Linkdb.ToString() + ".CAR002 AS CR ON S2.STK002_CariHesapKodu=CR.CAR002_HesapKodu " +
                        "LEFT JOIN " + Linkdb.ToString() + ".STK004 AS S4 ON S4.STK004_MalKodu=T.MalKodu " +
                        "INNER JOIN " + SifDb + ".dbo.Kullanicilar AS K ON T.Kaydeden=K.KullaniciKodu " +
                        "WHERE (T.TeknikKisim = 0) AND (CASE WHEN T.TalepBirim = '' THEN K.Birim " +
                        "ELSE T.TalepBirim END) = 'Teknik Birim' AND (T.GidecekDepartman='" + DepartmanKodu + "') AND (T.OnayDurumu NOT IN(9,10,11,12)) AND " +
                        "(CASE WHEN T.OnaylayanYetki = " + Convert.ToInt32(Yetkili) + "  THEN 1 ELSE 0 END)=1 " +
                        "GROUP BY T.TalepID,T.EvrakNoTarih+T.EvrakNo,T.MalAdi,S2.STK002_BirimFiyati,T.Miktar,STK004_SonAlimFaturasiBirimFiyati,  " +
                        "S2.STK002_Miktari,T.DepartmanAdi,T.MasrafMerkeziAdi,S2.STK002_Tutari,CR.CAR002_Unvan1+CAR002_Unvan2, " +
                        "T.Aciklama+T.Aciklama2+T.Aciklama3,T.Birim  " +
                        "ORDER BY T.EvrakNoTarih+T.EvrakNo DESC " +
                       "SELECT * FROM #TeknikKisimTablo " +
                       "DROP TABLE #TeknikKisimTablo";

        da = new SqlDataAdapter(sorgu, DbConnLinkSorgu);
        da.Fill(ds, "TeknikKisimTablo");

        TabloSayisi = ds.Tables["TeknikKisimTablo"].Rows.Count;
        printer = new HtmlInputCheckBox[TabloSayisi];

        Literal Lt = new Literal();
        Lt.Text = "<table width=\"1100\" border=\"0\"><tr>" +
            "<td width=\"50\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Çıktı Al</td> " +
            "<td width=\"100\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">İhtiyaç No</td> " +
            "<td width=\"200\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Malzeme Adı</td> " +
            "<td width=\"100\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Miktar</td> " +
            "<td width=\"100\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Birim Fiyatı</td> " +
            "<td width=\"100\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Toplam Tutar</td> " +
            "<td width=\"200\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Talep Eden</td> " +
            "<td width=\"150\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Kullanılacak Yer</td> " +
            "<td width=\"200\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Firma</td> " +
            "</<tr>";
        panelidariKisimlar.Controls.Add(Lt);

        for (int i = 0; i < ds.Tables["TeknikKisimTablo"].Rows.Count; i++)
        {
            MalKodumuzNedir = MalKodumuz(ds.Tables["TeknikKisimTablo"].Rows[i]["EvrakNo"].ToString());

            if (MalKodumuzNedir.ToString().Substring(0, 1) == "M")
            {
                AciklamaSatirimiz = ds.Tables["TeknikKisimTablo"].Rows[i]["Aciklama"].ToString();
            }
            else
            {
                AciklamaSatirimiz = ds.Tables["TeknikKisimTablo"].Rows[i]["StokAdi"].ToString();
            }

            Literal Lt2 = new Literal();
            Lt2.Text = "<tr><td align=\"center\" style=\"border:1px solid Black;\">";
            panelidariKisimlar.Controls.Add(Lt2);

            printer[i] = new HtmlInputCheckBox();
            printer[i].ID = "printer" + i.ToString();
            printer[i].Value = ds.Tables["TeknikKisimTablo"].Rows[i]["TalepID"].ToString() + "/" + ds.Tables["TeknikKisimTablo"].Rows[i]["BirimFiyat"].ToString();
            printer[i].Name = "printer" + i.ToString();
            panelidariKisimlar.Controls.Add(printer[i]);

            Literal Lt4 = new Literal();
            Lt4.Text = "</td>";
            panelidariKisimlar.Controls.Add(Lt4);

            Literal Lt1 = new Literal();
            Lt1.Text = "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["TeknikKisimTablo"].Rows[i]["EvrakNo"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + AciklamaSatirimiz + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["TeknikKisimTablo"].Rows[i]["Miktar"].ToString().Replace('.', ',') + " " + ds.Tables["TeknikKisimTablo"].Rows[i]["Birim"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["TeknikKisimTablo"].Rows[i]["BirimFiyat"].ToString().Replace('.', ',') + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["TeknikKisimTablo"].Rows[i]["Tutar"].ToString().Replace('.', ',') + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["TeknikKisimTablo"].Rows[i]["TalepEden"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["TeknikKisimTablo"].Rows[i]["KullanilacakDepartman"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["TeknikKisimTablo"].Rows[i]["Firma"].ToString() + "</td>";

            panelidariKisimlar.Controls.Add(Lt1);
        }

        Literal Lt3 = new Literal();
        Lt3.Text = "</tr></table>";
        panelidariKisimlar.Controls.Add(Lt3);

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

    protected void imgYaziciyaGonder_Click(object sender, ImageClickEventArgs e)
    {
        Session["TeknikTalepID"] = null;
        Session["TeknikEvrakNo"] = null;
        Session["TeknikMalAdi"] = null;
        Session["TeknikTarih"] = null;
        Session["TeknikMalAciklama"] = null;
        Session["TeknikMiktar"] = null;
        Session["TeknikBirimFiyat"] = null;
        Session["TeknikTutar"] = null;
        Session["TeknikTalepEden"] = null;
        Session["TeknikKullanilacakYer"] = null;
        Session["TeknikFirma"] = null;

        for (int i = 0; i < TabloSayisi; i++)
        {
            if (printer[i].Checked == true)
            {
                Dizimiz = printer[i].Value.ToString();
                Dizimiz2 = Dizimiz.Split('/');
                int a = 0;

                int TDID = Convert.ToInt32(Dizimiz2[a].ToString());
                string Btbirimfiyat = Dizimiz2[a + 1].ToString();
                if (Convert.ToDecimal(Btbirimfiyat) > 0)
                {
                    decimal TDBirimFİyat = Convert.ToDecimal(Dizimiz2[a + 1].ToString());
                    //TalepBul(TDID, TDBirimFİyat);
                    TalepBul2(TDID);
                }
                else
                {
                    TalepBul2(TDID);
                }
            }
        }

        BaglantilariKapat();
    }

    protected void btnGuncelle_Click(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TeknikKisimTablomuz();
        }
    }

    private void TalepBul(int TalepID, decimal BirimFiyati)
    {
        if (DbConnLinkSorgu.State == ConnectionState.Closed)
            DbConnLinkSorgu.Open();

        string Sorgu = "SELECT T.TalepID,T.EvrakNoTarih+T.EvrakNo AS EvrakNo,T.MalAdi, " +
                  "CONVERT(VARCHAR(10),CONVERT(DATETIME,Tarih-2),120) AS Tarih,S2.STK002_Miktari AS Miktar, " +
                  "S2.STK002_BirimFiyati AS BirimFiyat,S2.STK002_Tutari AS Tutar,T.DepartmanAdi AS TalepEden, " +
                  "M.MasrafMerkeziAdi AS KullanilacakDepartman, " +
                  "CR.CAR002_Unvan1+CAR002_Unvan2 AS Firma " +
                  "FROM " + SifDb + ".dbo.Tlp AS T " +
                  "INNER JOIN " + Linkdb.ToString() + ".STK002 AS S2 ON T.EvrakNoTarih+T.EvrakNo=S2.STK002_EvrakSeriNo2 " +
                  "INNER JOIN " + Linkdb.ToString() + ".CAR002 AS CR ON S2.STK002_CariHesapKodu=CR.CAR002_HesapKodu " +
                  "INNER JOIN " + SifDb + ".dbo.Kullanicilar AS K ON T.Kaydeden=K.KullaniciKodu " +
                  "INNER JOIN " + SifDb + ".dbo.MasrafMerkezi AS M ON T.KullanilacakDepartman=M.MasrafMerkeziKodu " +
                  "WHERE T.TalepID=" + TalepID + "  AND S2.STK002_BirimFiyati='" + BirimFiyati.ToString().Replace(',', '.') + "' " +
                  "GROUP BY T.TalepID,T.EvrakNoTarih+T.EvrakNo,T.MalAdi,S2.STK002_BirimFiyati, " +
                  "S2.STK002_Miktari,T.DepartmanAdi,M.MasrafMerkeziAdi,S2.STK002_Tutari,CR.CAR002_Unvan1+CAR002_Unvan2, " +
                  "CONVERT(VARCHAR(10),CONVERT(DATETIME,Tarih-2),120)";

        cmd = new SqlCommand(Sorgu, DbConnLinkSorgu);
        cmd.CommandTimeout = 120;
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        if (dr.Read())
        {
            Session["TeknikTalepID"] += dr["TalepID"].ToString() + ",";
            Session["TeknikEvrakNo"] += dr["EvrakNo"].ToString() + ",";
            Session["TeknikMalAdi"] += dr["MalAdi"].ToString() + "}";
            Session["TeknikTarih"] += dr["Tarih"].ToString() + ",";
            Session["TeknikMiktar"] += dr["Miktar"].ToString() + "/";
            Session["TeknikBirimFiyat"] += dr["BirimFiyat"].ToString() + "/";
            Session["TeknikTutar"] += dr["Tutar"].ToString() + "/";
            Session["TeknikTalepEden"] += dr["TalepEden"].ToString() + ",";
            Session["TeknikKullanilacakYer"] += dr["KullanilacakDepartman"].ToString() + ",";
            Session["TeknikFirma"] += dr["Firma"].ToString() + ",";
        }

        dr.Dispose();
        dr.Close();
    }

    private void TalepBul2(int TalepID)
    {
        if (DbConnLinkSorgu.State == ConnectionState.Closed)
            DbConnLinkSorgu.Open();

        string Sorgu = "SELECT T.TalepID,T.EvrakNoTarih+T.EvrakNo AS EvrakNo,T.MalAdi,T.Aciklama+'' + T.Aciklama2+'' + T.Aciklama3 AS Aciklama, " +
                  "CONVERT(VARCHAR(10),CONVERT(DATETIME,Tarih-2),120) AS Tarih, " +
                  "CONVERT(NUMERIC(18,2),(CASE " +
                  "WHEN S2.STK002_Miktari>0 THEN S2.STK002_Miktari " +
                  "ELSE T.Miktar END)) AS Miktar ,T.Birim, " +
                  "(CASE " +
                  "WHEN S2.STK002_BirimFiyati > 0 THEN S2.STK002_BirimFiyati " +
                  "ELSE S4.STK004_SonAlimFaturasiBirimFiyati END) AS BirimFiyat, " +
                  "CONVERT(NUMERIC(18,2),(CASE " +
                  "WHEN S2.STK002_Tutari > 0 THEN S2.STK002_Tutari " +
                  "ELSE T.Miktar*S4.STK004_SonAlimFaturasiBirimFiyati END)) AS Tutar " +
                  ",T.DepartmanAdi AS TalepEden, " +
                  "T.MasrafMerkeziAdi AS KullanilacakDepartman, " +
                  "CR.CAR002_Unvan1+CAR002_Unvan2 AS Firma " +
                  "FROM " + SifDb + ".dbo.Tlp AS T " +
                  "LEFT JOIN " + Linkdb.ToString() + ".STK002 AS S2 ON T.EvrakNoTarih+T.EvrakNo=S2.STK002_EvrakSeriNo2 " +
                  "LEFT JOIN " + Linkdb.ToString() + ".CAR002 AS CR ON S2.STK002_CariHesapKodu=CR.CAR002_HesapKodu " +
                  "LEFT JOIN " + Linkdb.ToString() + ".STK004 AS S4 ON S4.STK004_MalKodu=T.MalKodu " +
                  "INNER JOIN " + SifDb + ".dbo.Kullanicilar AS K ON T.Kaydeden=K.KullaniciKodu " +
                  "WHERE T.TalepID=" + TalepID + " " +
                  "GROUP BY T.TalepID,T.EvrakNoTarih+T.EvrakNo,T.MalAdi,S2.STK002_BirimFiyati,STK004_SonAlimFaturasiBirimFiyati, " +
                  "T.Miktar,STK002_Miktari,T.DepartmanAdi,T.MasrafMerkeziAdi,S2.STK002_Tutari,CR.CAR002_Unvan1+CAR002_Unvan2, " +
                  "CONVERT(VARCHAR(10),CONVERT(DATETIME,Tarih-2),120),T.Aciklama+'' + T.Aciklama2+'' + T.Aciklama3,T.Birim";

        cmd = new SqlCommand(Sorgu, DbConnLinkSorgu);
        cmd.CommandTimeout = 120;
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        if (dr.Read())
        {
            Session["TeknikTalepID"] += dr["TalepID"].ToString() + ",";
            Session["TeknikEvrakNo"] += dr["EvrakNo"].ToString() + ",";
            Session["TeknikMalAdi"] += dr["MalAdi"].ToString() + "}";
            Session["TeknikMalAciklama"] += dr["Aciklama"].ToString() + "}";
            Session["TeknikTarih"] += dr["Tarih"].ToString() + ",";
            Session["TeknikMiktar"] += dr["Miktar"].ToString() + "/";
            Session["TeknikBirim"] += dr["Birim"].ToString() + "/";
            Session["TeknikBirimFiyat"] += dr["BirimFiyat"].ToString() + "/";
            Session["TeknikTutar"] += dr["Tutar"].ToString() + "/";
            Session["TeknikTalepEden"] += dr["TalepEden"].ToString() + ",";
            Session["TeknikKullanilacakYer"] += dr["KullanilacakDepartman"].ToString() + ",";
            Session["TeknikFirma"] += dr["Firma"].ToString() + ",";
        }

        dr.Dispose();
        dr.Close();
    }

    private void BaglantilariKapat()
    {
        DbConnUser.Close();
        DbConnLink.Close();
        DbConnLinkSorgu.Close();
    }

    private decimal BirimFiyatimiz(string MalKodu)
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        string Sorgu = "SELECT ISNULL(STK004_SonAlimFaturasiBirimFiyati,0) AS BirimFiyat " +
                     "FROM STK004 " +
                     "WHERE STK004_MalKodu='" + MalKodu + "' " +
                     "ORDER BY STK004_Row_ID DESC";

        cmd = new SqlCommand(Sorgu, DbConnLink);

        return (decimal)Convert.ToDecimal(cmd.ExecuteScalar());
    }

    private string MalKodumuz(string EvrakNo)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT MalKodu FROM Tlp " +
                      "WHERE EvrakNoTarih+EvrakNo='" + EvrakNo + "' ";

        cmd = new SqlCommand(sorgu, DbConnUser);

        return (string)cmd.ExecuteScalar();
    }
}