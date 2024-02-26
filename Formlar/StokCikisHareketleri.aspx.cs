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

public partial class Formlar_StokCikisHareketleri : System.Web.UI.Page
{

    #region Bağlantı Ayarları

    SqlConnection DbConnUser = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnLink = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnLink"].ToString());
    SqlConnection DbConnLinkSorgu = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnLinkSorgu"].ToString());
    SqlCommand cmd, cmds;
    SqlDataReader dr;
    DataSet ds;
    SqlDataAdapter da;
    string DepartmanKodu, TalepDepoAdi;
    int TabloSayisi, TalepID, seqNumarasi, TarihRakam;
    public HtmlInputCheckBox[] printer;
    public HtmlInputText[] txtMiktar;
    string Linkdb, DepoKodu, DepoAdimiz, DepoKoduEvrakNo, MalKodu, MasrafMerkezi, PcAdi, LinkMasKod, LinkBorcluHesap, STK005KOD1;
    double Tarih;
    decimal StokMiktari;
    bool StokCikisYeriBul;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Giris"] != "Evet")
        {
            Response.Redirect("../Login.Aspx");
        }
        else
        {
            StokCikis();

            if (!IsPostBack)
            {
                stkCikisTarih.Date = DateTime.Now;
            }
        }
    }

    private void StokCikis()
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        ds = new DataSet();

        DepartmanKodu = Session["DepartmanKodu"].ToString();

        string sorgu = "CREATE TABLE #StokCikisHrk( " +
                       "TalepID INT, " +
                       "ihtiyacNo NVARCHAR(25), " +
                       "Tarih NVARCHAR(25), " +
                       "DepartmanAdi NVARCHAR(150), " +
                       "AdSoyad NVARCHAR(250), " +
                       "MalKodu NVARCHAR(150), " +
                       "MalAdi NVARCHAR(250), " +
                       "Miktar NUMERIC(25,6), " +
                       "MasrafMerkeziAdi NVARCHAR(150) " +
                       ") " +
                       "INSERT INTO #StokCikisHrk " +
                       "SELECT T.TalepID,T.EvrakNoTarih+T.EvrakNo AS ihtiyacNo,CONVERT(VARCHAR(10),(CONVERT(DATETIME,Tarih-2)),104) AS Tarih, " +
                       "T.DepartmanAdi,T.AdSoyad,T.MalKodu,T.MalAdi,T.Miktar, " +
                       "T.MasrafMerkeziAdi " +
                       "FROM Tlp AS T " +
                       "WHERE T.OnayDurumu=4 AND StokCikisHareketi=0 AND (T.GidecekDepartman='" + DepartmanKodu + "') " +
                       "SELECT * FROM #StokCikisHrk " +
                       "DROP TABLE #StokCikisHrk";

        da = new SqlDataAdapter(sorgu, DbConnUser);
        da.Fill(ds, "StokCikisHrk");

        TabloSayisi = ds.Tables["StokCikisHrk"].Rows.Count;
        printer = new HtmlInputCheckBox[TabloSayisi];
        txtMiktar = new HtmlInputText[TabloSayisi];

        Literal Lt = new Literal();
        Lt.Text = "<table width=\"1250\" border=\"0\"><tr> " +
            "<td width=\"75\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Çıkış Oluştur</td> " +
            "<td width=\"75\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">İhtiyaç No</td> " +
            "<td width=\"120\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Kullanıcı Kodu</td> " +
            "<td width=\"250\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Stok Kodu</td> " +
            "<td width=\"250\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Stok Adı</td> " +
            "<td width=\"110\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Departman Adı</td> " +
            "<td width=\"150\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Tarih</td> " +
            "<td width=\"75\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Miktar</td> " +
            "<td width=\"75\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Stok Miktarı</td> " +
            "<td width=\"100\" align=\"center\" scope=\"col\" style=\"font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;\">Masraf Yeri</td></<tr>";
        panelStokCikisHareketi.Controls.Add(Lt);

        for (int i = 0; i < ds.Tables["StokCikisHrk"].Rows.Count; i++)
        {
            StokMiktari = StokMiktariBul(ds.Tables["StokCikisHrk"].Rows[i]["MalKodu"].ToString());

            Literal Lt5 = new Literal();
            Lt5.Text = "<tr><td align=\"center\" style=\"border:1px solid Black;\">";
            panelStokCikisHareketi.Controls.Add(Lt5);

            printer[i] = new HtmlInputCheckBox();
            printer[i].ID = "printer" + i.ToString();
            printer[i].Value = ds.Tables["StokCikisHrk"].Rows[i]["TalepID"].ToString();
            printer[i].Name = "printer" + i.ToString();
            panelStokCikisHareketi.Controls.Add(printer[i]);

            Literal Lt2 = new Literal();
            Lt2.Text = "</td>";
            panelStokCikisHareketi.Controls.Add(Lt2);

            //Literal Lt2 = new Literal();
            //Lt2.Text = "</td>";

            //panelihtiyaclarimiz.Controls.Add(Lt2);

            Literal Lt1 = new Literal();
            Lt1.Text = "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["StokCikisHrk"].Rows[i]["ihtiyacNo"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["StokCikisHrk"].Rows[i]["AdSoyad"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["StokCikisHrk"].Rows[i]["MalKodu"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["StokCikisHrk"].Rows[i]["MalAdi"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["StokCikisHrk"].Rows[i]["DepartmanAdi"].ToString() + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["StokCikisHrk"].Rows[i]["Tarih"].ToString().Substring(0, 10) + "</td> " +
                     "<td align=\"center\" style=\"border:1px solid Black;\">";
            panelStokCikisHareketi.Controls.Add(Lt1);

            txtMiktar[i] = new HtmlInputText();
            txtMiktar[i].ID = "txtMiktar" + i.ToString();
            txtMiktar[i].Name = "txtMiktar" + i.ToString();
            txtMiktar[i].Value = ds.Tables["StokCikisHrk"].Rows[i]["Miktar"].ToString().Replace('.', ',');
            panelStokCikisHareketi.Controls.Add(txtMiktar[i]);

            Literal Lt4 = new Literal();
            Lt4.Text = "<td align=\"center\" style=\"border:1px solid Black;\">" + StokMiktari + "</td> " +
            "<td align=\"center\" style=\"border:1px solid Black;\">" + ds.Tables["StokCikisHrk"].Rows[i]["MasrafMerkeziAdi"].ToString() + "</td> ";
            panelStokCikisHareketi.Controls.Add(Lt4);

        }

        Literal Lt3 = new Literal();
        Lt3.Text = "</tr></table>";
        panelStokCikisHareketi.Controls.Add(Lt3);

        if (TabloSayisi <= 0)
        {
            CikisTablomuz.Visible = false;
        }
        else
        {
            CikisTablomuz.Visible = true;
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

    protected void btnStokCikisHareketiOlustur_Click(object sender, EventArgs e)
    {
        string DepoKoduKontrolu = drpDepoKodu.SelectedItem.Text.ToString();

        if (DepoKoduKontrolu == "Seçiniz")
        {
            Alert.Show("Lütfen Çıkış Yapılacak Depoyu Seçiniz");
            return;
        }
        else
        {
            for (int i = 0; i < TabloSayisi; i++)
            {
                if (printer[i].Checked == true)
                {
                    TalepID = Convert.ToInt32(printer[i].Value.ToString());
                    //DepoAdimiz = drpDepoKodlari.SelectedItem.Text.ToString();
                    DepoKodu = CikilacakDepoBul(TalepID);
                    MalKodu = MalKoduBul(TalepID);

                    TalepDepoAdi = GidecekDepoKodumuzuBuluyoruz(TalepID);
                    string DepoKod = drpDepoKodu.SelectedItem.Value.ToString();

                    MasrafMerkezi = MasrafMerkeziKoduSearch(TalepID);
                    LinkMasKod = LinkMasrafKodu(MasrafMerkezi);
                    LinkBorcluHesap = BorcluHesapBul(MalKodu);

                    if (!string.IsNullOrEmpty(LinkMasKod))
                    {
                        if (LinkBorcluHesap.ToString().Substring(3, 3) == "001")
                        {
                            STK005KOD1 = "710" + LinkMasKod;
                        }
                        else if (LinkBorcluHesap.ToString().Substring(3, 3) != "001")
                        {
                            if (LinkMasKod == "642")
                            {
                                STK005KOD1 = "760" + LinkMasKod;
                            }
                            else if (LinkMasKod == "743")
                            {
                                STK005KOD1 = "770" + LinkMasKod;
                            }
                            else if (LinkMasKod == "666")
                            {
                                STK005KOD1 = "258" + LinkMasKod;
                            }
                            else if (LinkMasKod == "777")
                            {
                                STK005KOD1 = "689" + LinkMasKod;
                            }
                            else if (LinkMasKod == "888")
                            {
                                STK005KOD1 = "689" + LinkMasKod;
                            }
                            else
                            {
                                STK005KOD1 = "730" + LinkMasKod;
                            }
                        }
                    }
                    else if (string.IsNullOrEmpty(LinkMasKod))
                    {
                        if (LinkBorcluHesap.ToString().Substring(3, 3) == "001")
                        {
                            STK005KOD1 = "710999";
                        }
                        else if (LinkBorcluHesap.ToString().Substring(3, 3) != "001")
                        {
                            if (LinkMasKod == "642")
                            {
                                STK005KOD1 = "760999";
                            }
                            else if (LinkMasKod == "743")
                            {
                                STK005KOD1 = "770999";
                            }
                            else if (LinkMasKod == "666")
                            {
                                STK005KOD1 = "258999";
                            }
                            else if (LinkMasKod == "777")
                            {
                                STK005KOD1 = "689999";
                            }
                            else if (LinkMasKod == "888")
                            {
                                STK005KOD1 = "689999";
                            }
                            else
                            {
                                STK005KOD1 = "730999";
                            }
                        }

                        MasrafMerkezi = "999";
                    }


                    string Miktarimiz = txtMiktar[i].Value.Replace(',', '.');
                    decimal Miktar = Convert.ToDecimal(Miktarimiz);

                    string CariHareketSeriNo = CikisHareketSeri();
                    int CariHareketNo = CikisHareketNo();
                    string CariHareketNums = Convert.ToString(CariHareketNo);
                    CariHareketNums = CariHareketNums.PadLeft(6, '0');
                    DepoKoduEvrakNo = CariHareketSeriNo + CariHareketNums;

                    string GirenKodu = Session["KullAdi"].ToString();

                    if (GirenKodu.ToString().Length > 10)
                    {
                        GirenKodu = GirenKodu.ToString().Substring(0, 10);
                    }

                    PcAdi = System.Net.Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]).HostName.ToString();

                    if (PcAdi.ToString().Length > 10)
                    {
                        PcAdi = PcAdi.ToString().Substring(0, 10);
                    }

                    seqNumarasi = SeqNo();

                    DateTime obj = new DateTime();
                    obj = Convert.ToDateTime(stkCikisTarih.Date);
                    string str;
                    Tarih = 0;
                    Tarih = obj.ToOADate();
                    str = Tarih.ToString().Substring(0, 5);
                    TarihRakam = Convert.ToInt32(str);
                    string LinkDepoCikisYeri = "";

                    DateTime DtSaat = DateTime.Now;
                    string Saatimiz = Convert.ToString(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));

                    if (Saatimiz.ToString().Length == 18)
                    {
                        Saatimiz = Saatimiz.ToString().Substring(10, 8);
                    }
                    else if (Saatimiz.ToString().Length == 19)
                    {
                        Saatimiz = Saatimiz.ToString().Substring(11, 8);
                    }
                    Saatimiz = Saatimiz.ToString().Replace(":", "");

                    if (Saatimiz.ToString().Length > 8)
                    {
                        Saatimiz = Saatimiz.ToString().Substring(0, 8);
                    }

                    #region Stok Kartı Sütün Bul

                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter();
                    int a = 0;

                    if (DbConnLink.State == ConnectionState.Closed)
                        DbConnLink.Open();

                    string sorgu = "SELECT STK004_DepoCikisMiktari1,STK004_DepoCikisMiktari2,STK004_DepoCikisMiktari3, " +
                                "STK004_DepoCikisMiktari4,STK004_DepoCikisMiktari5,STK004_DepoCikisMiktari6, " +
                                "STK004_DepoCikisMiktari7,STK004_DepoCikisMiktari8,STK004_DepoCikisMiktari9, " +
                                "STK004_DepoCikisMiktari10, " +
                                "STK004_DepoKodu1,STK004_DepoKodu2,STK004_DepoKodu3, " +
                                "STK004_DepoKodu4,STK004_DepoKodu5,STK004_DepoKodu6, " +
                                "STK004_DepoKodu7,STK004_DepoKodu8,STK004_DepoKodu9,STK004_DepoKodu10, " +
                                "STK004_SonCikisTarihi " +
                                "FROM STK004  " +
                                "WHERE STK004_MalKodu='" + MalKodu.ToString() + "'";

                    da = new SqlDataAdapter(sorgu, DbConnLink);
                    da.Fill(ds, "STK004");

                    for (int c = 10; c < ds.Tables["STK004"].Columns.Count - 1; c++)
                    {
                        if (StokCikisYeriBul == false)
                        {
                            string dep = drpDepoKodu.SelectedItem.Value.ToString();
                            string deplink = ds.Tables["STK004"].Rows[0][c].ToString();
                            string LinkDepoSutunu = ds.Tables["STK004"].Columns[c].ToString();

                            if (dep == deplink)
                            {
                                LinkDepoCikisYeri = ds.Tables["STK004"].Columns[a].ToString();

                                StokCikisYeriBul = true;
                            }
                            a++;
                        }
                    }

                    StokCikisYeriBul = false;
                    #endregion

                    if (!string.IsNullOrEmpty(LinkDepoCikisYeri))
                    {
                        StokKartiGuncelle(Saatimiz, PcAdi, TarihRakam, MalKodu, Miktar, LinkDepoCikisYeri);

                        StokHareketiCikisYap(MalKodu, TarihRakam, DepoKoduEvrakNo, Saatimiz, seqNumarasi,
                        PcAdi, DepoKod, Miktar, MasrafMerkezi, STK005KOD1);

                        TalepGuncelle(TalepID, TalepDepoAdi.ToString(), DepoKoduEvrakNo, GirenKodu.ToString());

                        TalepCikisMiktarGuncelle(TalepID, Miktar);

                        DepoSeriNoGuncelle();
                        BaglantilariKapat();
                    }
                }
            }
            if (!IsPostBack)
            {
                StokCikis();
            }
        }
    }

    private string DepoKoduBul(string DepoAdi)
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        string Sorgu = "SELECT STK006_ReferansKodu FROM STK006 " +
                    "WHERE STK006_ReferansAciklamasi='" + DepoAdi.ToString() + "'";

        cmds = new SqlCommand(Sorgu, DbConnLink);
        cmds.CommandTimeout = 120;
        return (string)cmds.ExecuteScalar();
    }

    private int SeqNo()
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        string sorgu = "SELECT " +
                    "ISNULL((CASE " +
                    "WHEN MAX(STK005_SEQNo) < 2000000 THEN 2000000 ELSE MAX(STK005_SEQNo)+1 " +
                    "END),2000000+1) AS SEQ  " +
                    "FROM STK005 " +
                    "WHERE (STK005_SEQNo > 2000000 AND STK005_SEQNo < 5000000)";

        cmd = new SqlCommand(sorgu, DbConnLink);
        cmd.CommandTimeout = 120;
        return (int)cmd.ExecuteScalar();
    }

    private string CikisHareketSeri()
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT CikisHareketiSeri FROM EvrakNumaralari ";

        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();
    }

    private int CikisHareketNo()
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT MAX(CikisHareketNo)+1 AS FisNo FROM EvrakNumaralari  ";

        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (int)cmd.ExecuteScalar();
    }

    private void DepoSeriNoGuncelle()
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "UPDATE EvrakNumaralari SET " +
                     "CikisHareketNo=CikisHareketNo+1";
        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        cmd.ExecuteNonQuery();
    }

    private decimal MiktarBul(int TalepID)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string Sorgu = "SELECT ISNULL(Miktar,0) AS Miktar FROM Tlp " +
                        "WHERE TalepID=" + TalepID + "";

        cmd = new SqlCommand(Sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (decimal)Convert.ToDecimal(cmd.ExecuteScalar());
    }

    private string MalKoduBul(int TalepID)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT MalKodu FROM Tlp " +
                        "WHERE TalepID=" + TalepID + "";

        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();
    }

    private void TalepGuncelle(int TalepID, string DepoAdi, string CikisHareketNo, string KullaniciKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "UPDATE Tlp SET " +
                    "CikisHareketNo='" + CikisHareketNo.ToString() + "', " +
                    "CikisHareketDepo='" + DepoAdi.ToString() + "' ," +
                    "StokCikisHareketi=1, " +
                    "OnayDurumu='13', " +
                    "Onaylayan='" + KullaniciKodu.ToString() + "' " +
                    "WHERE TalepID=" + TalepID + "";

        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        cmd.ExecuteNonQuery();
    }

    private void TalepAkisiGuncelle(int TalepID)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string TalepAkisDuzenle = "UPDATE TalepAkisi SET Onay='True' WHERE TalepID=" + TalepID + "";
        cmd = new SqlCommand(TalepAkisDuzenle, DbConnUser);
        cmd.CommandTimeout = 120;
        cmd.ExecuteNonQuery();
    }

    private void TalepCikisMiktarGuncelle(int TalepID, decimal Miktar)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "UPDATE Tlp SET " +
                    "CikisMiktar='" + Miktar.ToString().Replace(',', '.') + "' " +
                    "WHERE TalepID=" + TalepID + "";

        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.ExecuteNonQuery();
    }

    private void StokHareketiCikisYap(string MalKodu, int Tarih, string EvrakNo, string Saat, int SeqNo, string GirenKodu, string DepoKodu, decimal Miktar, string MasrafMerkezi, string Kod1)
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        string Sorgu = "INSERT INTO STK005(STK005_MalKodu, STK005_IslemTarihi, STK005_GC, STK005_CariHesapKodu, STK005_EvrakSeriNo, STK005_Miktari, STK005_BirimFiyati,  " +
                      "STK005_Tutari, STK005_Iskonto, STK005_KDVTutari, STK005_IslemTipi, STK005_Kod1, STK005_Kod2, STK005_IrsaliyeNo, STK005_FaturaDurumu,   " +
                      "STK005_MuhasebelesmeDurumu, STK005_KDVDurumu, STK005_ParaBirimi, STK005_SEQNo, STK005_GirenKaynak, STK005_GirenTarih, STK005_GirenSaat,   " +
                      "STK005_GirenKodu, STK005_GirenSurum, STK005_DegistirenKaynak, STK005_DegistirenTarih, STK005_DegistirenSaat, STK005_DegistirenKodu,   " +
                      "STK005_DegistirenSurum, STK005_IptalDurumu, STK005_AsilEvrakTarihi, STK005_OTVDahilHaric, STK005_OTV, STK005_Miktar2, STK005_Tutar2,   " +
                      "STK005_KalemIskontoOrani1, STK005_KalemIskontoOrani2, STK005_KalemIskontoOrani3, STK005_KalemIskontoOrani4, STK005_KalemIskontoOrani5,   " +
                      "STK005_KalemIskontoTutari1, STK005_KalemIskontoTutari2, STK005_KalemIskontoTutari3, STK005_KalemIskontoTutari4, STK005_KalemIskontoTutari5,   " +
                      "STK005_DovizCinsi, STK005_DovizTutari, STK005_DovizKuru, STK005_Aciklama1, STK005_Aciklama2, STK005_Depo, STK005_Kod3, STK005_Kod4, STK005_Kod5,   " +
                      "STK005_Kod6, STK005_Kod7, STK005_Kod8, STK005_Kod9, STK005_Kod10, STK005_Kod11, STK005_Kod12, STK005_Vasita, STK005_MalSeriNo,   " +
                      "STK005_EvrakSeriNo2, STK005_SiparisNo, STK005_IrsaliyeFaturaTarihi, STK005_Masraf, STK005_MaliyetTutari, STK005_MaliyetMuhasebelesmeSekli,   " +
                      "STK005_MaliyetMuhasebelesmeDurumu, STK005_MasrafMerkezi, STK005_MuhasebeFisNo, STK005_MuhasebeFisKodu, STK005_MuhasebeHesapNo,   " +
                      "STK005_MuhasebeKarsiHesapNo, STK005_Kod9Flag, STK005_Kod10Flag, STK005_StokTrFinansmanGider, STK005_StokTrVadeFarki, STK005_StokTrSozFaizOrani,   " +
                      "STK005_StokTrStokDuzHesapKodu, STK005_StokTrSmmDuzHesapKodu, STK005_StokTrNonReelFinansGidSpk, STK005_StokTrNonReelFinansGidMly,   " +
                      "STK005_StokTrDuzKatsayiSpk, STK005_StokTrDuzKatsayiMly, STK005_StokTrDuzTutarSpk, STK005_StokTrDuzTutarMly, STK005_StokTrDuzSatSpk,   " +
                      "STK005_StokTrDuzSatMly, STK005_StokTrSatMaliyeti, STK005_StokTrKrediTutari, STK005_StokTrIlgiliEvrak, STK005_KDVOranFlag, STK005_EvrakTipi,   " +
                      "STK005_TeslimCHKodu, STK005_KarsiMuhasebeKodu, STK005_ExtFldTutar1, STK005_FaturalasanMiktar, STK005_KDVTevkOrani, STK005_KDVTevkTutar,   " +
                      "STK005_IthalatNo, STK005_BarkodNo, STK005_PartiNo, STK005_EFaturaOTVListeNo, STK005_ToplamKalemIskontosu)  " +
                      "VALUES ('" + MalKodu.ToString() + "','" + Tarih + "', 1, N'','" + EvrakNo.ToString() + "','" + Miktar.ToString().Replace(',', '.') + "', '0.000000', '0.00', '0.00', '0.00', 12, '" + Kod1.ToString() + "', N'', N'', 1, 0, 0,   " +
                      "1," + SeqNo + ",N'Y4005'," + Tarih + ",'" + Saat.ToString() + "','" + GirenKodu.ToString() + "', N'6.1.10',N'Y4005'," + Tarih + ",'" + Saat.ToString() + "','" + GirenKodu.ToString() + "', N'6.1.10', 1," + Tarih + ", 0, '0.00',   " +
                      "'0.000', '0.000', '0.00', '0.00', '0.00', '0.00', '0.00', '0.00', '0.00', '0.00', '0.00', '0.00', N'', '0.00', '0.000000', N'', N'','" + DepoKodu.ToString() + "', N'', N'', N'', N'', N'', N'', N'', N'', '0.00',   " +
                      "'0.00', N'', N'', N'', N''," + Tarih + ", '0.00', '0.00', 1, 0, '" + MasrafMerkezi + "', 0, N'', N'', N'', 0, 0, '0.00', '0.00', '0.00', N'', N'', '0.00', '0.00', '0.00000', '0.00000', '0.00', '0.00', '0.00', '0.00',   " +
                      "'0.00', '0.00', N'', 4, 53, N'', N'', '0.00', '0.0000', N'', '0.00', N'', N'', N'', N'', '0.00')";

        cmd = new SqlCommand(Sorgu, DbConnLink);
        cmd.CommandTimeout = 120;
        cmd.ExecuteNonQuery();
    }

    private void StokKartiGuncelle(string Saat, string GirenKodu, int Tarih, string MalKodu, decimal Miktar, string DepoCikisMiktarYeri)
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        string sorgu = "UPDATE STK004 SET " +
                       "STK004_DegistirenSaat='" + Saat + "', " +
                       "STK004_DegistirenKodu='" + GirenKodu + "', " +
                       "STK004_DegistirenTarih=" + Tarih + ", " +
                       "STK004_SonCikisTarihi=" + Tarih + ", " +
                       "STK004_CikisMiktari=STK004_CikisMiktari" + "+'" + Miktar + "'," +
                       DepoCikisMiktarYeri + "=" + DepoCikisMiktarYeri + "+'" + Miktar + "' " +
                       "WHERE STK004_MalKodu='" + MalKodu + "'";

        cmd = new SqlCommand(sorgu, DbConnLink);
        cmd.CommandTimeout = 120;
        cmd.ExecuteNonQuery();
    }

    private string MasrafMerkeziBul(int TalepID)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT DepartmanDurumu FROM StokKodlari " +
                       "WHERE StokKodu=(Select MalKodu FROM Tlp WHERE TalepID=" + TalepID + ")";

        cmd = new SqlCommand(sorgu, DbConnUser);

        return (string)cmd.ExecuteScalar();
    }

    private decimal StokMiktariBul(string MalKodu)
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        string sorgu = "SELECT CONVERT(NUMERIC(18,3),((STK004_GirisMiktari+STK004_DevirMiktari)-STK004_CikisMiktari)) AS StokMiktari FROM STK004 " +
                    "WHERE STK004_MalKodu='" + MalKodu + "'";

        cmd = new SqlCommand(sorgu, DbConnLink);

        return (decimal)Convert.ToDecimal(cmd.ExecuteScalar());
    }

    private string CikilacakDepoBul(int TalepID)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT GidecekDepartman FROM Tlp WHERE TalepID=" + TalepID + "";

        cmd = new SqlCommand(sorgu, DbConnUser);

        return (string)cmd.ExecuteScalar();

    }

    private string LinkDepoKoduBuluyoruz(string StokDepoYeri)
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        string sorgu = "SELECT STK006_ReferansKodu " +
                     "FROM STK006 WITH(NOLOCK)  " +
                     "WHERE (STK006_ReferansTuru = 096) AND (STK006_ReferansAciklamasi='" + StokDepoYeri + "')";

        cmd = new SqlCommand(sorgu, DbConnLink);

        return (string)cmd.ExecuteScalar();
    }

    private string TalepLinkDepoBagliyoruz(int TalepID)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT StokDepartman FROM Departmanlar WHERE " +
                       "DepartmanKodu=(SELECT GidecekDepartman FROM Tlp WHERE TalepID=" + TalepID + ")";

        cmd = new SqlCommand(sorgu, DbConnUser);

        return (string)cmd.ExecuteScalar();
    }

    private string GidecekDepoKodumuzuBuluyoruz(int TalepID)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT DepartmanKodu FROM Departmanlar " +
                        "WHERE DepartmanID=(SELECT DepartmanID FROM Kullanicilar " +
                        "WHERE KullaniciKodu=(SELECT Kaydeden FROM Tlp " +
                        "WHERE TalepID=" + TalepID + "))";

        cmd = new SqlCommand(sorgu, DbConnUser);

        return (string)cmd.ExecuteScalar();
    }

    private string MasrafMerkeziKoduSearch(int TalepID)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT MasrafMerkeziKodu FROM MasrafMerkezi " +
                     "WHERE MasrafMerkeziAdi=(Select MasrafMerkeziAdi FROM Tlp WHERE TalepID=" + TalepID + ")";

        cmd = new SqlCommand(sorgu, DbConnUser);

        return (string)cmd.ExecuteScalar();
    }

    private string LinkMasrafKodu(string MasrafMerkeziKodu)
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        string sorgu = "SELECT MHS007_MasrafMerkeziKodu AS LinkMasrafKodu,MHS007_MMAdi AS LinkMasrafAdi " +
                     "FROM MHS007 WITH(NOLOCK)  " +
                     "WHERE MHS007_MasrafMerkeziKodu='" + MasrafMerkeziKodu + "'";

        cmd = new SqlCommand(sorgu, DbConnLink);

        return (string)cmd.ExecuteScalar();
    }

    private string BorcluHesapBul(string MalKodu)
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        string sorgu = "SELECT STK004_BorcluHesap FROM STK004 " +
                     "WHERE STK004_MalKodu='" + MalKodu + "'";

        cmd = new SqlCommand(sorgu, DbConnLink);

        return (string)cmd.ExecuteScalar();
    }

    private void BaglantilariKapat()
    {
        DbConnUser.Close();
        DbConnLink.Close();
        DbConnLinkSorgu.Close();
    }
}