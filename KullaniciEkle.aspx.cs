﻿using System;
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
using DevExpress.Web;

public partial class KullaniciEkle : System.Web.UI.Page
{

    #region Bağlantı Ayarları

    SqlConnection DbConnUser = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnLink = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnLink"].ToString());
    SqlCommand cmd;
    SqlDataReader dr;
    bool DepartmanDurum, KullanicininDurumu;
    int AltDepartmanID;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["Giris"] != "Evet")
            {
                Response.Redirect("Login.Aspx");
            }
        }
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

    protected void cmbDepartmanAdi_SelectedIndexChanged(object sender, EventArgs e)
    {
        string ID = cmbDepartmanAdi.Value.ToString();

        string sorgu = "SELECT  DepartmanID, DepartmanAdi, DepartmanKodu FROM Departmanlar WHERE DepartmanID=" + Convert.ToInt32(ID) + "";
        cmbDepartmanKodu.TextField = "DepartmanKodu";
        cmbDepartmanKodu.ValueField = "DepartmanKodu";
        cmbDepartmanKodu.DataSource = datadoldur(sorgu);
        cmbDepartmanKodu.DataBind();

        string AltDepartmanSorgu = "SELECT AD.AltDepartmanID,AD.AltDepartmanAdi,AD.AltDepartmanKodu FROM Departmanlar AS D " +
        "INNER JOIN AltDepartman AS AD ON AD.DepartmanID=" + Convert.ToInt32(ID) + " " +
        "GROUP BY AD.AltDepartmanAdi,AD.AltDepartmanKodu,AD.AltDepartmanID";

        cmbAltDepartmanAdi.TextField = "AltDepartmanAdi";
        cmbAltDepartmanAdi.ValueField = "AltDepartmanID";

        cmbAltDepartmanKodu.ValueField = "AltDepartmanKodu";
        cmbAltDepartmanKodu.ValueField = "AltDepartmanKodu";

        cmbBagliAltDept.TextField = "AltDepartmanAdi";
        cmbBagliAltDept.ValueField = "AltDepartmanID";

        cmbAltDepartmanAdi.DataSource = datadoldur(AltDepartmanSorgu);
        cmbAltDepartmanAdi.DataBind();

        cmbAltDepartmanKodu.DataSource = datadoldur(AltDepartmanSorgu);
        cmbAltDepartmanKodu.DataBind();

        cmbBagliAltDept.DataSource = datadoldur(AltDepartmanSorgu);
        cmbBagliAltDept.DataBind();



        //cmbAltDepartmanAdi1.SelectedIndex = 0;
        //cmbAltDepartmanKodu1.SelectedIndex = 0;
        cmbDepartmanKodu.SelectedIndex = 0;
    }

    protected void btnKullaniciEkle_Click(object sender, EventArgs e)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string KullaniciKodu = txtKullaniciKodu.Text;
        string KullaniciAdi = txtKullaniciAdi.Text;
        KullanicininDurumu = KullaniciAdiBul(KullaniciAdi);
        if (KullanicininDurumu)
        {
            Alert.Show("Bu Kullanıcı Sistemde Kayıtlı Lütfen Tekrar Deneyin.");
            return;
        }
        else
        {
            string AltDepartmanID = "";
            string BagliAltDepartman = "";
            //string StokluDepartman = "";
            //string StoksuzDepartman = "";
            string Sifre = txtSifre.Text;
            string AdSoyad = txtAdSoyad.Text;
            string DepartmanID = cmbDepartmanAdi.SelectedItem.Value.ToString();

            if (string.IsNullOrEmpty(cmbAltDepartmanAdi.Text.ToString()))
            {
                AltDepartmanID = "0";
            }
            else
            {
                AltDepartmanID = cmbAltDepartmanAdi.SelectedItem.Value.ToString();
            }

            if (string.IsNullOrEmpty(cmbBagliAltDept.Text.ToString()))
            {
                BagliAltDepartman = "0";
            }
            else
            {
                BagliAltDepartman = cmbBagliAltDept.SelectedItem.Value.ToString();
            }
            //if (string.IsNullOrEmpty(cmbStokluDepartman.Text.ToString()))
            //{
            //    StokluDepartman = "0";
            //}
            //else
            //{
            //    StokluDepartman = cmbStokluDepartman.SelectedItem.Value.ToString();
            //}
            //if (string.IsNullOrEmpty(cmbStoksuzDepartman.Text.ToString()))
            //{
            //    StoksuzDepartman = "0";
            //}
            //else
            //{
            //    StoksuzDepartman = cmbStoksuzDepartman.SelectedItem.Value.ToString();
            //}

            string Yetki = cmbYetki.SelectedItem.Value.ToString();
            string MasrafKodu = "0";
            bool MasrafKoduCheck = chkMasrafMerkezi.Checked;

            if (MasrafKoduCheck == false)
            {
                MasrafKodu = cmbMasrafMerkeziKodu.Value.ToString();
            }

            bool Siparis = chkSiparis.Checked;
            bool SatinAlma = chkSatinAlma.Checked;
            bool AnaDept = chkAnaDepartman.Checked;
            bool KullDurum = chkKullaniciDurumu.Checked;
            string KopsRaporuAl = cmbKopsDurumRaporu.SelectedItem.Value.ToString();
            bool KopsDurumRaporu = Convert.ToBoolean(KopsRaporuAl);
            string Birim = cmbBirimler.Value.ToString();
            string MenuKodlari = "";

            if (string.IsNullOrEmpty(cmbMenuKodlari.Value.ToString()))
            {
                MenuKodlari = "";
            }
            else
            {
                MenuKodlari = cmbMenuKodlari.Value.ToString();
            }

            string KullaniciEkle = "INSERT INTO Kullanicilar(KullaniciKodu, KullaniciAdi, " +
                                   "Sifre,DurumRaporu, AdSoyad, DepartmanID, AltDepartmanID,BagliAltDepartman, " +
                                   "Yetki, Birim, MenuKodu,Durum, " +
                                   "Siparis,SatinAlma,AnaDepartman,MasrafMerkezi,MasrafMerkeziKodu, izin_BasTarih, izin_BitTarih,LastLogin) " +
                                   "VALUES    ('" + KullaniciKodu.ToString() + "','" + KullaniciAdi.ToString() + "'" +
                                   ",EncryptByPassPhrase('Editor','" + Sifre.ToString() + "'),'" + Convert.ToString(KopsDurumRaporu) + "'," +
                                   "'" + AdSoyad.ToString() + "'," + Convert.ToInt32(DepartmanID) + " " +
                                   "," + Convert.ToInt32(AltDepartmanID) + "," + Convert.ToInt32(BagliAltDepartman) + "," + Convert.ToInt32(Yetki) + ", " +
                                   "'" + Birim.ToString() + "','" + MenuKodlari.ToString() + "', '" + KullDurum + "', " +
                                   "'" + Siparis + "','" + SatinAlma + "','" + AnaDept + "','" + MasrafKoduCheck + "','" + MasrafKodu + "'," +
                                   "'1900 - 01 - 01 00:00:00.000','1900 - 01 - 01 00:00:00.000',GETDate())";

            cmd = new SqlCommand(KullaniciEkle, DbConnUser);
            cmd.CommandTimeout = 120;
            cmd.ExecuteNonQuery();

            ConnectionKapat();

            Alert.Show("Kullanıcı Başarılı Bir Şekilde Eklenmiştir.");
        }
    }

    protected void cmbAltDepartmanAdi_SelectedIndexChanged(object sender, EventArgs e)
    {
        string ID = cmbDepartmanAdi.Value.ToString();
        int sID = cmbAltDepartmanAdi.SelectedIndex;

        if (sID < 0)
        {
            sID = 0;
        }

        string AltDepartmanSorgu = "SELECT AD.AltDepartmanID,AD.AltDepartmanAdi,AD.AltDepartmanKodu FROM Departmanlar AS D " +
        "INNER JOIN AltDepartman AS AD ON AD.DepartmanID=" + Convert.ToInt32(ID) + " " +
        "GROUP BY AD.AltDepartmanAdi,AD.AltDepartmanKodu,AD.AltDepartmanID";

        cmbAltDepartmanKodu.TextField = "AltDepartmanKodu";
        cmbAltDepartmanKodu.ValueField = "AltDepartmanKodu";

        cmbAltDepartmanKodu.DataSource = datadoldur(AltDepartmanSorgu);
        cmbAltDepartmanKodu.DataBind();
        cmbAltDepartmanKodu.SelectedIndex = sID;
    }

    private bool KullaniciAdiBul(string KullaniciAdi)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT COUNT(KullaniciAdi) AS Durum FROM Kullanicilar " +
                        "WHERE KullaniciAdi='" + KullaniciAdi.ToString() + "'";
        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return Convert.ToBoolean(cmd.ExecuteScalar());
    }

    private void ConnectionKapat()
    {
        DbConnUser.Close();
        //Kullanicilar.Close();
        //DbConMaxYetki.Close();
        DbConnLink.Close();
        //DbConnDepartmanKodu.Close();
        //DbConnYetkiKoduBul.Close();
        //DbConnYetkiTamamla.Close();
        //DbConnYetkilendir.Close();
        //DbConnTalepIrsaliyeGuncelle.Close();
        //DbConnTalepAkisiIrsaliyeGuncelle.Close();
        //DbConnSiparisBul.Close();
        //DbConnIrsaliyeDurumu.Close();
    }

}