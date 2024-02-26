﻿using System;
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

public partial class TalepDuzenle : System.Web.UI.Page
{

    #region Bağlantı Ayarları

    SqlConnection DbConnUser = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnLink = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnLink"].ToString());
    SqlConnection DbConnTalepAkisi = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());

    SqlCommand cmd, cmdTalepAkisi, cmdMalAdi, cmdMalKodu, cmdBirimveStokKodu;
    SqlDataReader dr;
    int Durum, EvrakNo, TalepID, YetkiKodu, TalepCount, TalepOnayDurum, DepartmanCountDurum;
    string KullaniciKodu, DepartmanKodu, AltDepartmanKodu, ilkislem, GelenKullanici, KayitYetki, OnayDurumu, sonuc2, AltDepartmanID;
    bool SiparisButonu, SatinAlmaButonu;

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
                TalepEdit();

                string sorgu = "SELECT IhtiyacID, IhtiyacAdi, IhtiyacKodu FROM Ihtiyaclar Order BY IhtiyacKodu ASC";
                drpihtiyacNedeni.DataTextField = "IhtiyacAdi";
                drpihtiyacNedeni.DataValueField = "IhtiyacKodu";
                drpihtiyacNedeni.DataSource = datadoldur(sorgu);
                drpihtiyacNedeni.DataBind();

                string sorgu2 = "SELECT TeminID,TeminAdi,TeminKodu FROM TeminYeri ORDER BY TeminID ASC";
                drpTeminYeri.DataTextField = "TeminAdi";
                drpTeminYeri.DataValueField = "TeminKodu";
                drpTeminYeri.DataSource = datadoldur(sorgu2);
                drpTeminYeri.DataBind();
            }
        }
    }

    public void TalepEdit()
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        int say = 0;
        TalepID = Convert.ToInt32(Request.QueryString["TalepID"].ToString());
        GelenKullanici = Session["KullaniciKodu"].ToString();
        SiparisButonu = SiparisButonDurumu(GelenKullanici);
        SatinAlmaButonu = SatinAlmaButonDurumu(GelenKullanici);
        DepartmanKodu = Session["DepartmanKodu"].ToString();
        AltDepartmanKodu = Session["AltDepartmanKodu"].ToString();
        AltDepartmanID = Session["AltDepartmanID"].ToString();
        YetkiKodu = Convert.ToInt32(Session["Yetki"].ToString());
        string KullaniciAdi = Session["KullAdi"].ToString();
        string StokluDepartmanKodu = DepartmanKodu;

        TalepOnayDurum = TalepOnayKontrolDurumu(TalepID, YetkiKodu, DepartmanKodu);

        if (TalepOnayDurum > 0)
        {
            btnTalepOnayla.Visible = true;
            btnRedOnayla.Visible = true;
        }
        else
        {
            //btnDuzenle.Visible = false;
            btnTalepOnayla.Visible = false;
            btnRedOnayla.Visible = false;
        }

        YetkiKoduBul(DepartmanKodu, KullaniciAdi.ToString(), Convert.ToString(YetkiKodu), AltDepartmanID);

        //if (Convert.ToInt32(KayitYetki) == 0)
        //{
        StokluDepartmanKodu = StokluDepartmanBul(TalepID);

        TalepCount = TalepCountSearch(KullaniciAdi, YetkiKodu, DepartmanKodu, TalepID);

        string YetkiUnvanlari = YetkiUnvanimiz(Convert.ToInt32(YetkiKodu));
        string OnayDurumNotIn = "";

        if (TalepCount != 0)
        {
            if (YetkiUnvanlari == "Müdür")
            {
                OnayDurumNotIn = "6,7,8,10,13";
            }
            else if (YetkiUnvanlari == "Şef")
            {
                OnayDurumNotIn = "6,7,8,9,13";
            }
            else if (YetkiUnvanlari == "Memur")
            {
                OnayDurumNotIn = "6,7,8,13";
            }
            else
            {
                OnayDurumNotIn = "6,7,8,11,12,13";
            }


            string TalepSorgumuz = "SELECT T.TalepID,EvrakNoTarih+EvrakNo AS EvrakNo,Kaydeden,CONVERT(VARCHAR(10),(CONVERT(DATETIME,Tarih-2)),104) AS Tarih, " +
                "MalKodu,MalAdi,Birim,Miktar,Aciklama,Aciklama2,Aciklama3,TeminYeri,ihtiyacNedeni,(CASE " +
                "WHEN T.OnaylayanYetki = " + YetkiKodu + " THEN 1 " +
                "ELSE 0 END) AS Onay ,T.OnaylayanYetki, " +
                "(CASE T.ilkIslem " +
                "WHEN 0 THEN 'Evet' " +
                "WHEN 1 THEN 'Hayır' END) AS IlkIslem " +
                "FROM Tlp AS T " +
                "WHERE T.TalepID= " + TalepID + " AND (T.OnayDurumu NOT IN(" + OnayDurumNotIn + "))";

            cmd = new SqlCommand(TalepSorgumuz, DbConnUser);
            cmd.CommandTimeout = 120;
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            if (dr.Read())
            {
                say++;
                ilkislem = dr["IlkIslem"].ToString();
                KullaniciKodu = dr["Kaydeden"].ToString();
                int onaylayanYetki = Convert.ToInt32(dr["OnaylayanYetki"].ToString());

                if (ilkislem == "Evet" && KullaniciKodu == GelenKullanici)
                {
                    Session["ilkislem"] = "Evet";
                    Session["KullaniciDurum"] = "ilk";

                    txtTalepNo.Text = dr["TalepID"].ToString();
                    txtEvrakNo.Text = dr["EvrakNo"].ToString();
                    txtTarih.Text = dr["Tarih"].ToString();
                    //grdLookUpMalAdlari.Value
                    txtBirim.Text = dr["Birim"].ToString();
                    txtMiktar.Text = dr["Miktar"].ToString();
                    txtAciklama.Text = dr["Aciklama"].ToString();
                    txtAciklama2.Text = dr["Aciklama2"].ToString();
                    txtAciklama3.Text = dr["Aciklama3"].ToString();
                    grdLookUpMalAdlari.Value = dr["MalAdi"].ToString();
                    Session["MalAdi"] = dr["MalAdi"].ToString();
                    grdLookUpMalKodu.Value = dr["MalKodu"].ToString();
                    Session["MalKodu"] = dr["MalKodu"].ToString();
                    drpTeminYeri.SelectedValue = dr["TeminYeri"].ToString();
                    drpihtiyacNedeni.SelectedValue = dr["ihtiyacNedeni"].ToString();
                }
                else if (ilkislem != "Evet" && KullaniciKodu == GelenKullanici)
                {
                    Session["ilkislem"] = "Hayır";
                    Session["KullaniciDurum"] = "ilk";

                    txtTalepNo.Text = dr["TalepID"].ToString();
                    txtEvrakNo.Text = dr["EvrakNo"].ToString();
                    txtTarih.Text = dr["Tarih"].ToString();
                    //grdLookUpMalAdlari.Value
                    txtBirim.Text = dr["Birim"].ToString();
                    txtMiktar.Text = dr["Miktar"].ToString();
                    txtAciklama.Text = dr["Aciklama"].ToString();
                    txtAciklama2.Text = dr["Aciklama2"].ToString();
                    txtAciklama3.Text = dr["Aciklama3"].ToString();
                    grdLookUpMalAdlari.Value = dr["MalAdi"].ToString();
                    Session["MalAdi"] = dr["MalAdi"].ToString();
                    grdLookUpMalKodu.Value = dr["MalKodu"].ToString();
                    Session["MalKodu"] = dr["MalKodu"].ToString();
                    drpTeminYeri.SelectedValue = dr["TeminYeri"].ToString();
                    drpihtiyacNedeni.SelectedValue = dr["ihtiyacNedeni"].ToString();
                    grdLookUpMalKodu.Enabled = false;
                    grdLookUpMalAdlari.Enabled = false;
                    drpihtiyacNedeni.Enabled = false;
                    drpTeminYeri.Enabled = false;
                    txtMiktar.Enabled = false;
                    txtAciklama.Enabled = false;
                    txtAciklama2.Enabled = false;
                    txtAciklama3.Enabled = false;
                    btnDuzenle.Enabled = false;
                    btnDuzenle.Text = "Talepde İşlem Yapıldığı için Düzenleme Yapılamaz";
                    //Alert.Show("Bu Talep İşlem Gördüğü İçin İşlem Yapılamaz.");
                    //ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
                }
                else if (ilkislem == "Evet" && KullaniciKodu != GelenKullanici)
                {
                    Session["ilkislem"] = "Evet";
                    Session["KullaniciDurum"] = "ikinci";

                    txtTalepNo.Text = dr["TalepID"].ToString();
                    txtEvrakNo.Text = dr["EvrakNo"].ToString();
                    txtTarih.Text = dr["Tarih"].ToString();
                    //grdLookUpMalAdlari.Value
                    txtBirim.Text = dr["Birim"].ToString();
                    txtMiktar.Text = dr["Miktar"].ToString();
                    txtAciklama.Text = dr["Aciklama"].ToString();
                    txtAciklama2.Text = dr["Aciklama2"].ToString();
                    txtAciklama3.Text = dr["Aciklama3"].ToString();
                    grdLookUpMalAdlari.Value = dr["MalAdi"].ToString();
                    Session["MalAdi"] = dr["MalAdi"].ToString();
                    grdLookUpMalKodu.Value = dr["MalKodu"].ToString();
                    Session["MalKodu"] = dr["MalKodu"].ToString();
                    drpTeminYeri.SelectedValue = dr["TeminYeri"].ToString();
                    drpihtiyacNedeni.SelectedValue = dr["ihtiyacNedeni"].ToString();
                    grdLookUpMalKodu.Enabled = false;
                    grdLookUpMalAdlari.Enabled = false;
                    drpTeminYeri.Enabled = false;
                }
                else if (ilkislem != "Evet" && KullaniciKodu != GelenKullanici)
                {
                    Session["ilkislem"] = "Hayır";
                    Session["KullaniciDurum"] = "ikinci";

                    txtTalepNo.Text = dr["TalepID"].ToString();
                    txtEvrakNo.Text = dr["EvrakNo"].ToString();
                    txtTarih.Text = dr["Tarih"].ToString();
                    //grdLookUpMalAdlari.Value
                    txtBirim.Text = dr["Birim"].ToString();
                    txtMiktar.Text = dr["Miktar"].ToString();
                    txtAciklama.Text = dr["Aciklama"].ToString();
                    txtAciklama2.Text = dr["Aciklama2"].ToString();
                    txtAciklama3.Text = dr["Aciklama3"].ToString();
                    grdLookUpMalAdlari.Value = dr["MalAdi"].ToString();
                    Session["MalAdi"] = dr["MalAdi"].ToString();
                    grdLookUpMalKodu.Value = dr["MalKodu"].ToString();
                    Session["MalKodu"] = dr["MalKodu"].ToString();
                    drpTeminYeri.SelectedValue = dr["TeminYeri"].ToString();
                    drpihtiyacNedeni.SelectedValue = dr["ihtiyacNedeni"].ToString();
                    grdLookUpMalKodu.Enabled = false;
                    grdLookUpMalAdlari.Enabled = false;
                    drpTeminYeri.Enabled = false;
                }
            }
            dr.Dispose();
            dr.Close();

            if (say <= 0)
            {
                Alert.Show("Bu Talep İşlem Gördüğü İçin Düzenleme Yapılamaz.");
                ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
            }
        }
        else
        {
            Alert.Show("Bu Talep İşlem Gördüğü İçin Düzenleme Yapılamaz.");
            ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
        }
    }

    protected void grdLookUpMalKodu_ValueChanged(object sender, EventArgs e)
    {
        string MalKodu = grdLookUpMalKodu.Value.ToString();
        Durum = 1;
        BirimveStokMiktariMalKodu(MalKodu);
        MalKodu = MalAdiBul(MalKodu);
        grdLookUpMalAdlari.Value = MalKodu;
    }

    protected void grdLookUpMalAdlari_ValueChanged(object sender, EventArgs e)
    {
        string MalAdi = grdLookUpMalAdlari.Value.ToString();
        Durum = 1;
        BirimveStokMiktariMalAdi(MalAdi);
        MalAdi = MalKoduBul(MalAdi);
        grdLookUpMalKodu.Value = MalAdi;
    }

    private void BirimveStokMiktariMalKodu(string MalKodu)
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        string sorgu = "SELECT STK004_Birim1 AS Birim,CONVERT(NUMERIC(18,3),((STK004_GirisMiktari+STK004_DevirMiktari)-STK004_CikisMiktari)) AS StokMiktari FROM STK004 " +
                     "WHERE STK004_MalKodu='" + MalKodu.ToString() + "'";
        cmdBirimveStokKodu = new SqlCommand(sorgu, DbConnLink);
        cmdBirimveStokKodu.CommandTimeout = 120;
        dr = cmdBirimveStokKodu.ExecuteReader();

        if (dr.Read())
        {
            txtBirim.Text = dr["Birim"].ToString();
        }

        dr.Dispose();
        dr.Close();
    }

    private void BirimveStokMiktariMalAdi(string MalAdi)
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        string sorgu = "SELECT STK004_Birim1 AS Birim,(STK004_GirisMiktari-STK004_CikisMiktari) AS StokMiktari FROM STK004 " +
                     "WHERE STK004_Aciklama='" + MalAdi.ToString() + "'";
        cmdBirimveStokKodu = new SqlCommand(sorgu, DbConnLink);
        cmdBirimveStokKodu.CommandTimeout = 120;
        dr = cmdBirimveStokKodu.ExecuteReader();

        if (dr.Read())
        {
            txtBirim.Text = dr["Birim"].ToString();
        }

        dr.Dispose();
        dr.Close();
    }

    private string MalAdiBul(string MalKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT StokAciklama FROM StokKodlari " +
                        "WHERE StokKodu='" + MalKodu.ToString() + "'";
        cmdMalAdi = new SqlCommand(sorgu, DbConnUser);
        cmdMalAdi.CommandTimeout = 120;
        return (string)cmdMalAdi.ExecuteScalar();     
    }

    private string MalKoduBul(string MalAdi)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT StokKodu,StokAciklama FROM StokKodlari " +
                    "WHERE StokAciklama='" + MalAdi.ToString() + "'";
        cmdMalKodu = new SqlCommand(sorgu, DbConnUser);
        cmdMalKodu.CommandTimeout = 120;
        return (string)cmdMalKodu.ExecuteScalar();
    }

    public static DataTable datadoldur(string Sql)
    {
        SqlConnection conn = new SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DbConnUser"].ToString());

        DataTable dt = new DataTable();
        try
        {
            conn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(Sql, conn);
            adapter.Fill(dt);
        }
        finally
        {
            conn.Close();
        }
        return dt;
    }

    protected void btnDuzenle_Click(object sender, EventArgs e)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string Sorgu = "";
        string ilkislem = Session["ilkislem"].ToString();
        string KullaniciDurum = Session["KullaniciDurum"].ToString();
        string MiktarimizOnay = Convert.ToString(txtMiktar.Text.ToString().Replace(',', '.'));
        decimal Miktar = Convert.ToDecimal(MiktarimizOnay);
        string Aciklama = txtAciklama.Text;
        string Aciklama2 = txtAciklama2.Text;
        string Aciklama3 = txtAciklama3.Text;
        string IhtiyacNedeni = drpihtiyacNedeni.SelectedValue.ToString();
        string TeminYeri = drpTeminYeri.SelectedValue.ToString();
        string MalKodu = grdLookUpMalKodu.Text.ToString();
        string MalAdi = grdLookUpMalAdlari.Text.ToString();
        string DegisiklikNedeni = txtDegisiklikNedeni.Text;
        TalepID = Convert.ToInt32(txtTalepNo.Text);

        MalAdi = MalAdiniAliyoruz(MalKodu);

        if (ilkislem == "Evet" && KullaniciDurum == "ilk")
        {
            Sorgu = "UPDATE Tlp SET " +
               "Aciklama='" + Aciklama.ToString().Replace("'", "''") + "', " +
               "Aciklama2='" + Aciklama2.ToString().Replace("'", "''") + "', " +
               "Aciklama3='" + Aciklama3.ToString().Replace("'", "''") + "', " +
               "DegisiklikNedeni='" + DegisiklikNedeni.ToString() + "'," +
               "Miktar='" + Miktar + "', " +
               "TeminYeri='" + TeminYeri.ToString() + "', " +
               "ihtiyacNedeni='" + IhtiyacNedeni.ToString() + "', " +
               "MalAdi='" + MalAdi.ToString().Replace("'", "''") + "', " +
               "MalKodu='" + MalKodu.ToString() + "' " +
               "WHERE TalepID=" + TalepID + " ";
        }
        else if (ilkislem == "Evet" && KullaniciDurum == "ikinci")
        {
            Sorgu = "UPDATE Tlp SET " +
                "Aciklama='" + Aciklama.ToString().Replace("'", "''") + "', " +
                "Aciklama2='" + Aciklama2.ToString().Replace("'", "''") + "', " +
                "Aciklama3='" + Aciklama3.ToString().Replace("'", "''") + "', " +
                "DegisiklikNedeni='" + DegisiklikNedeni.ToString() + "'," +
                "Miktar='" + Miktar + "', " +
                "ihtiyacNedeni='" + IhtiyacNedeni.ToString() + "' " +
                "WHERE TalepID=" + TalepID + " ";

        }
        else if (ilkislem == "Hayır" && KullaniciDurum == "ikinci")
        {
            Sorgu = "UPDATE Tlp SET " +
                    "Aciklama='" + Aciklama.ToString().Replace("'", "''") + "', " +
                    "Aciklama2='" + Aciklama2.ToString().Replace("'", "''") + "', " +
                    "Aciklama3='" + Aciklama3.ToString().Replace("'", "''") + "', " +
                    "DegisiklikNedeni='" + DegisiklikNedeni.ToString() + "', " +
                    "Miktar='" + Miktar + "', " +
                    "ihtiyacNedeni='" + IhtiyacNedeni.ToString() + "' " +
                    "WHERE TalepID=" + TalepID + " ";
        }

        cmd = new SqlCommand(Sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        cmd.ExecuteNonQuery();

        BaglantilariKapat();

        Alert.Show("Talep Başarılı Bir Şekilde Güncellenmiştir.");
        ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
    }

    private int TalepCountSearch(string KullaniciKodu, int Yetki, string DepartmanKodu, int TalepID)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string EkSorgu = "";

        if (SatinAlmaButonu == true || SiparisButonu == true)
        {
            EkSorgu = "T.OnaylayanYetki=" + Yetki + " AND T.GidecekDepartman='" + DepartmanKodu + "' AND (T.OnayDurumu NOT IN(13)) ";
        }
        else
        {
            EkSorgu = "(T.OnaylayanYetki <= " + Yetki + " OR T.Yetki = " + Yetki + ") AND T.DepartmanKodu='" + DepartmanKodu + "' AND (T.OnayDurumu NOT IN(13)) ";
        }

        string OnayCountSorgusu = "SELECT COUNT(*) AS OnayCount FROM(SELECT T.TalepID,T.EvrakNoTarih+T.EvrakNo AS EvrakNo, " +
                                "(CASE " +
                                "WHEN T.Kaydeden = '" + KullaniciKodu + "' THEN  " +
                                "((CASE T.ilkIslem " +
                                "WHEN 1 THEN 0 " +
                                "WHEN 0 THEN 1 END)) ELSE 0 END) AS ilkIslem, " +
                                "(CASE " +
                                "WHEN T.OnaylayanYetki = " + Yetki + " THEN 1  " +
                                "ELSE 0 END) AS OnayDurum  " +
                                "FROM Tlp AS T " +
                                "INNER JOIN Kullanicilar AS K ON T.Kaydeden=K.KullaniciKodu  " +
                                "WHERE T.TalepID=" + TalepID + " AND " +
                                EkSorgu +
                                ") AS TBL";

        cmd = new SqlCommand(OnayCountSorgusu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (int)cmd.ExecuteScalar();
    }

    private int TalepCountSearch2(int TalepNo, int UstYetki, int YetkiKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string TalepEditCountSorgu = "SELECT COUNT(*) AS Durum FROM(SELECT T.TalepID,EvrakNoTarih+EvrakNo AS EvrakNo,Kaydeden,CONVERT(DATETIME,Tarih-2) AS Tarih, " +
                "MalKodu,MalAdi,Birim,Miktar,Aciklama,Aciklama2,Aciklama3,TeminYeri,ihtiyacNedeni,TA.Onay, " +
                "(CASE T.ilkIslem  " +
                "WHEN 0 THEN 'Evet' " +
                "WHEN 1 THEN 'Hayır' END) AS IlkIslem " +
                "FROM Tlp AS T " +
                "INNER JOIN TalepAkisi AS TA ON TA.TalepID=T.TalepID AND TA.Departman=T.DepartmanKodu " +
                "WHERE T.TalepID = " + TalepNo + " " +
                " AND T.TalepDurum = 0  AND (TA.OnaylayanYetki= " + YetkiKodu + " OR TA.EkleyenYetki = " + YetkiKodu + ")" +
                "GROUP BY T.TalepID,EvrakNoTarih+EvrakNo,Tarih,TA.Onay,Kaydeden, " +
                "MalKodu,MalAdi,Birim,Miktar,Aciklama,Aciklama2,Aciklama3,TeminYeri,ihtiyacNedeni,T.ilkIslem) AS TBL";

        cmd = new SqlCommand(TalepEditCountSorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (int)cmd.ExecuteScalar();
    }

    private void YetkiKoduBul(string DepartmanKodu, string KullaniciAdi, string YetkiKodumuz, string AltDepartmanKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT ISNULL(MIN(Yetki),0) AS Yetki FROM Kullanicilar AS K " +
                        "INNER JOIN Departmanlar AS D ON K.DepartmanID=D.DepartmanID " +
                        "WHERE D.DepartmanKodu='" + DepartmanKodu.ToString() + "'  AND (AltDepartmanID=" + Convert.ToInt32(AltDepartmanKodu) + " OR AltDepartmanID=0) " +
                        "AND KullaniciAdi <> '" + KullaniciAdi.ToString() + "' " +
                        "AND Yetki > " + Convert.ToInt32(YetkiKodumuz) + "  AND K.Durum=1 " +
                        "AND (MenuKodu<>'Satınalma' AND MenuKodu<>'Ambar') AND K.KullaniciAdi<>'Admin' ";

        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        if (dr.Read())
        {
            KayitYetki = dr["Yetki"].ToString();
        }

        dr.Dispose();
        dr.Close();
    }

    private void YetkiTamamla(string Departman)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT ISNULL(MIN(Yetki),0) AS Yetki FROM Kullanicilar AS K " +
                    "INNER JOIN Departmanlar AS D ON K.DepartmanID=D.DepartmanID " +
                    "WHERE D.DepartmanKodu ='" + Departman.ToString() + "'";

        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        if (dr.Read())
        {
            KayitYetki = dr["Yetki"].ToString();
        }

        dr.Dispose();
        dr.Close();
    }

    protected void btnTalepOnayla_Click(object sender, EventArgs e)
    {
        if (DbConnTalepAkisi.State == ConnectionState.Closed)
            DbConnTalepAkisi.Open();

        TalepID = Convert.ToInt32(txtTalepNo.Text);
        KullaniciKodu = Session["KullaniciKodu"].ToString();
        DepartmanKodu = Session["DepartmanKodu"].ToString();
        string TalepAkisDepartmanKodu = DepartmanKodu;
        string Yetkimiz = Session["Yetki"].ToString();
        AltDepartmanKodu = Session["AltDepartmanKodu"].ToString();
        AltDepartmanID = Session["AltDepartmanID"].ToString();

        string MalKodu = grdLookUpMalKodu.Value.ToString();
        string MalAdi = grdLookUpMalAdlari.Value.ToString();
        string StokluDepartmanKodu = GidecekDepartmanBul(MalKodu);

        string izinayari = Session["izinAyari"].ToString();

        if (izinayari == "İzne Çıkmış")
        {
            YetkiKoduBul(DepartmanKodu, KullaniciKodu.ToString(), Session["izinYetki"].ToString(), AltDepartmanID);
            string DurumKullaniciAdi = KullaniciAdiBul(DepartmanKodu, Convert.ToInt32(KayitYetki));
            //izin_Durumu = izinDurum(DurumKullaniciAdi);

            if (Convert.ToInt32(KayitYetki) == 0)
            {
                TalepAkisDepartmanKodu = StokluDepartmanKodu.ToString();
                OnayDurumu = OnayDurumBelirliyoruz(Convert.ToInt32(Yetkimiz));
                //StokluDepartmanKodu = GidecekDepartmanBul(MalKodu);
                KayitYetki = "4";
                DepartmanCountDurum = GidecekDeptDurum(GidecekDepartmanBul(MalKodu));

                if (DepartmanCountDurum > 1)
                {
                    StokluDepartmanKodu = GidecekDepartmanKodu(GidecekDepartmanBul(MalKodu));
                }
                else
                {
                    StokluDepartmanKodu = GidecekDepartmanKodu2(GidecekDepartmanBul(MalKodu));
                }
                //YetkiTamamla(StokluDepartmanKodu);
            }
            else
            {
                int YeniYetki = Convert.ToInt32(KayitYetki) - Convert.ToInt32(Yetkimiz);
                string YenYetki = "";
                string UnvanimiziAliyoruz = "";

                if (YeniYetki == 2)
                {
                    YenYetki = Convert.ToString(YeniYetki);
                }
                else
                {
                    UnvanimiziAliyoruz = YetkiUnvanimiz(Convert.ToInt32(Yetkimiz));

                    if (UnvanimiziAliyoruz == "Memur")
                    {
                        YenYetki = "1";
                    }
                    else
                    {
                        YenYetki = Yetkimiz;
                    }
                }
                OnayDurumu = OnayDurumBelirliyoruz(Convert.ToInt32(YenYetki));
            }
            //if (string.IsNullOrEmpty(OnayDurumu))
            //{
            //    OnayDurumu = OnayDurumKontrol(Convert.ToInt32(KayitYetki));
            //}
        }
        else if (izinayari == "İzinde Değil")
        {
            YetkiKoduBul(DepartmanKodu, KullaniciKodu.ToString(), Yetkimiz, AltDepartmanID);
            string DurumKullaniciAdi = "";
            //izin_Durumu = izinDurum(DurumKullaniciAdi);

            if (Convert.ToInt32(KayitYetki) == 0)
            {
                OnayDurumu = OnayDurumBelirliyoruz(Convert.ToInt32(Yetkimiz));
                DepartmanCountDurum = GidecekDeptDurum(GidecekDepartmanBul(MalKodu));
                KayitYetki = "4";
                if (DepartmanCountDurum > 1)
                {
                    StokluDepartmanKodu = GidecekDepartmanKodu(GidecekDepartmanBul(MalKodu));
                }
                else
                {
                    StokluDepartmanKodu = GidecekDepartmanKodu2(GidecekDepartmanBul(MalKodu));
                }
            }
            else
            {
                int YeniYetki = Convert.ToInt32(KayitYetki) - Convert.ToInt32(Yetkimiz);
                string YenYetki = "";
                string UnvanimiziAliyoruz = "";

                if (YeniYetki == 2)
                {
                    YenYetki = Convert.ToString(YeniYetki);
                }
                else
                {
                    UnvanimiziAliyoruz = YetkiUnvanimiz(Convert.ToInt32(Yetkimiz));

                    if (UnvanimiziAliyoruz == "Memur")
                    {
                        YenYetki = "1";
                    }
                    else
                    {
                        YenYetki = Yetkimiz;
                    }
                }
                OnayDurumu = OnayDurumBelirliyoruz(Convert.ToInt32(YenYetki));
            }
        }

        string TalepGuncelleSorgu = "UPDATE Tlp SET " +
                                "MalKodu='" + MalKodu.ToString() + "', " +
                                "MalAdi='" + MalAdi.ToString() + "', " +
                                "OnayDurumu='" + OnayDurumu.ToString() + "', " +
                                "Yetki=" + Yetkimiz + ", " +
                                "OnaylayanYetki=" + Convert.ToInt32(KayitYetki) + " " +
                                "WHERE TalepID=" + TalepID + "";

        cmd = new SqlCommand(TalepGuncelleSorgu, DbConnTalepAkisi);
        cmd.CommandTimeout = 120;
        cmd.ExecuteNonQuery();


        //string talepdeletesorgu = "DELETE FROM TalepAkisi WHERE TalepID=" + TalepID + "";

        //cmd = new SqlCommand(talepdeletesorgu, DbConnUser);
        //cmd.CommandTimeout = 120;
        //cmd.ExecuteNonQuery();


        //string TalepisleyisSorgu = "INSERT INTO TalepAkisi(TalepID, OnaylayanYetki, EkleyenYetki, TalepDurumu, Departman, Onay, OnayTarih) " +
        //             "VALUES (" + TalepID + "," + Convert.ToInt32(KayitYetki) + ", " + Convert.ToInt32(Yetkimiz) + ", " +
        //             "'" + TalepAkisDepartmanKodu.ToString() + "','" + DepartmanKodu.ToString() + "', 'False', GETDATE())";
        //cmdTalepAkisi = new SqlCommand(TalepisleyisSorgu, DbConnTalepAkisi);
        //cmdTalepAkisi.ExecuteNonQuery();

        BaglantilariKapat();

        Alert.Show("Talebiniz Tekrardan İşleme Gönderilmiştir.");
        ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
    }

    private int TalepOnayKontrolDurumu(int TalepID, int Yetki, string DepartmanKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT COUNT(*) AS Durum FROM Tlp AS T " +
                        "WHERE T.ilkIslem=0 AND T.Yetki=" + Yetki + " AND T.OnayDurumu<>13 AND T.TalepID=" + TalepID + " AND " +
                        "T.DepartmanKodu='" + DepartmanKodu + "' AND (T.OnayDurumu IN(9,10,11,12)) ";

        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (int)Convert.ToInt32(cmd.ExecuteScalar());
    }

    private string StokluDepartmanBul(int TalepID)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string Sorgu = "SELECT GidecekDepartman FROM Tlp " +
                    "WHERE TalepID=" + TalepID + "";

        cmd = new SqlCommand(Sorgu, DbConnUser);
        return (string)cmd.ExecuteScalar();
    }

    private string KullaniciAdiBul(string DepartmanKodu, int Yetki)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT K.KullaniciAdi FROM Kullanicilar AS K " +
                        "INNER JOIN Departmanlar AS D ON K.DepartmanID=D.DepartmanID " +
                        "WHERE D.DepartmanKodu='" + DepartmanKodu.ToString() + "'" +
                        "AND Yetki = " + Yetki + "";

        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();

    }

    private string DepartmanOnayKontrol(string DepartmanKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string DepartmanSorgu = "SELECT DepartmanAdi+' Onayı Bekliyor' AS OnayDurum FROM Departmanlar " +
                            "WHERE DepartmanKodu='" + DepartmanKodu.ToString() + "'";
        cmd = new SqlCommand(DepartmanSorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();
    }

    private string OnayDurumKontrol(int YetkiKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string OnaySorgu = "SELECT Yetki_Unvani+' Onayı Bekliyor' AS OnayDurum FROM Yetki_Unvanlari " +
                        "WHERE YetkiKodu=" + YetkiKodu + "";

        cmd = new SqlCommand(OnaySorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();
    }

    protected void btnRedOnayla_Click(object sender, EventArgs e)
    {
        if (DbConnTalepAkisi.State == ConnectionState.Closed)
            DbConnTalepAkisi.Open();

        TalepID = Convert.ToInt32(txtTalepNo.Text);

        string AkisYapilandir = "UPDATE Tlp SET OnayDurumu='13' WHERE TalepID=" + TalepID + "";

        cmd = new SqlCommand(AkisYapilandir, DbConnTalepAkisi);
        cmd.CommandTimeout = 120;
        cmd.ExecuteNonQuery();

        BaglantilariKapat();

        Alert.Show("Talep Red Onaylanmıştır.");
        ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
    }

    private void BaglantilariKapat()
    {
        //DbConnUser.Dispose();
        DbConnUser.Close();

        //DbConnLink.Dispose();
        DbConnLink.Close();

        //DbConnTalepAkisi.Dispose();
        DbConnTalepAkisi.Close();

        //cmd.Dispose();
        //cmdBirimveStokKodu.Dispose();
        //cmdMalAdi.Dispose();
        //cmdMalKodu.Dispose();
        //cmdTalepAkisi.Dispose();

        //dr.Dispose();
        //dr.Close();
    }

    private string GidecekDepartmanBul(string MalKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT dbo.fn_GidecekDepartmanBul('" + MalKodu + "') AS GidecekDepartman";

        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();
    }

    private bool SiparisButonDurumu(string KullaniciKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT Siparis FROM Kullanicilar " +
                    "WHERE KullaniciKodu='" + KullaniciKodu.ToString() + "'";
        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (bool)cmd.ExecuteScalar();
    }

    private bool SatinAlmaButonDurumu(string KullaniciKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT SatinAlma FROM Kullanicilar " +
                       "WHERE KullaniciKodu='" + KullaniciKodu.ToString() + "'";
        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (bool)cmd.ExecuteScalar();
    }

    private string OnayDurumBelirliyoruz(int YetkiDurumu)
    {
        switch (YetkiDurumu)
        {
            case 1: sonuc2 = "1"; break;
            case 2: sonuc2 = "2"; break;
            case 3: sonuc2 = "3"; break;
            case 4: sonuc2 = "4"; break;
        }
        string Yeniislem = sonuc2;
        return Yeniislem;
    }

    private int GidecekDeptDurum(string StokDurum)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT COUNT(GidecekKod) AS Durum FROM GidecekDepartman " +
                        "WHERE GidecekDepartman='" + StokDurum + "'";

        cmd = new SqlCommand(sorgu, DbConnUser);
        return (int)Convert.ToInt32(cmd.ExecuteScalar());
    }

    private string GidecekDepartmanKodu(string MalKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT TOP(1) GidecekKod FROM GidecekDepartman " +
                    "WHERE GidecekDepartman='" + MalKodu + "' ";

        cmd = new SqlCommand(sorgu, DbConnUser);
        return (string)cmd.ExecuteScalar();
    }

    private string GidecekDepartmanKodu2(string MalKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT GidecekKod FROM GidecekDepartman " +
                    "WHERE GidecekDepartman='" + MalKodu + "' ";

        cmd = new SqlCommand(sorgu, DbConnUser);
        return (string)cmd.ExecuteScalar();
    }

    private string YetkiUnvanimiz(int YetkiKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT Yetki_Unvani FROM Yetki_Unvanlari WHERE YetkiKodu=" + YetkiKodu + "";

        cmd = new SqlCommand(sorgu, DbConnUser);

        return (string)cmd.ExecuteScalar();
    }

    private string MalAdiniAliyoruz(string MalKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT StokAciklama FROM StokKodlari " +
                    "WHERE StokKodu='" + MalKodu.ToString() + "'";

        cmd = new SqlCommand(sorgu, DbConnUser);

        return (string)cmd.ExecuteScalar();
    }
}