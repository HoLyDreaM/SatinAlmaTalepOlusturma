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

public partial class Kullanicilar : System.Web.UI.Page
{

    #region Bağlantı Ayarları

    SqlConnection DbConnUser = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnLink = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnLink"].ToString());
    SqlCommand cmd;
    SqlDataReader dr;
    bool DepartmanDurum;
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
            else
            {
                Session["Sorgu"] = "";
                KullaniciDoldur();
            }
        }
    }

    public void KullaniciDoldur()
    {
        string sorgu = "SELECT USERID, KullaniciKodu, KullaniciAdi, " +
          "CONVERT(varchar,DECRYPTBYPASSPHRASE ('Editor',Sifre)) AS Sifre, " +
          "(CASE Durum " +
          "WHEN 1 THEN 'Aktif' " +
          "WHEN 0 THEN 'Pasif' END) AS Durum, " +
          "AdSoyad, D.DepartmanAdi,D.DepartmanKodu,D.DepartmanID,AD.AltDepartmanAdi,AD.AltDepartmanKodu,AD.AltDepartmanID, " +
          "Y.Yetki_Unvani, LastLogin AS Tarih " +
          "FROM Kullanicilar AS K " +
          "INNER JOIN Departmanlar AS D ON D.DepartmanID=K.DepartmanID " +
          "LEFT JOIN AltDepartman AS AD ON AD.AltDepartmanID=K.AltDepartmanID " +
          "INNER JOIN Yetki_Unvanlari AS Y ON K.Yetki=Y.YetkiKodu";

        Session["KullaniciDoldur"] = sorgu;
        lstKullanicilar.DataSource = datadoldur(sorgu);
        lstKullanicilar.DataBind();
    }

    //protected void btnKullaniciEkle_Click(object sender, EventArgs e)
    //{
    //    if (DbConnUser.State == ConnectionState.Closed)
    //        DbConnUser.Open();

    //    //string ip = Request.ServerVariables["REMOTE_ADDR"].ToString();
    //    string DepartmanKodu = drpDepartmanlar.SelectedValue.ToString();
    //    string YetkiKodu = drpYetkiler.SelectedValue.ToString();

    //    string InsertSorgusu = "INSERT INTO  " +
    //        "Kullanicilar(KullaniciKodu,AdSoyad, KullaniciAdi, Sifre, Departman,Yetki,LastLogin,izin_BasTarih,izin_BitTarih) " +
    //        "VALUES ('" + txtKullaniciKodu.Text + "','" + txtAdSoyad.Text + "','" + txtKullaniciAdi.Text + "', " +
    //        "EncryptByPassPhrase('Editor','" + txtSifre.Text + "'), " +
    //        "'" + DepartmanKodu + "','" + YetkiKodu + "',GETDATE(),'1900-01-01 00:00:00.000','1900-01-01 00:00:00.000')";

    //    cmd = new SqlCommand(InsertSorgusu, DbConnUser);
    //    cmd.ExecuteNonQuery();

    //    sqlKullanicilar.SelectCommand = "SELECT USERID,KullaniciKodu,AdSoyad, KullaniciAdi, " +
    //                   "CONVERT(varchar,DECRYPTBYPASSPHRASE ('Editor',Sifre)) AS Sifre, " +
    //                   "D.STK006_ReferansAciklamasi AS 'Departmani', " +
    //                   "Y.YetkiKodu AS 'Yetkisi',LastLogin AS 'Tarih'   " +
    //                   "FROM SatinAlma.EDSatinAlma.dbo.Kullanicilar AS K  " +
    //                   "INNER JOIN  STK006 AS D ON D.STK006_ReferansKodu=K.Departman  " +
    //                   "INNER JOIN  SatinAlma.EDSatinAlma.dbo.Yetki_Unvanlari AS Y ON Y.YetkiID=K.Yetki  " +
    //                   "Order by USERID ASC";

    //    txtAdSoyad.Text = null;
    //    txtKullaniciAdi.Text = null;
    //    txtKullaniciKodu.Text = null;
    //    txtSifre.Text = null;
    //}

    //protected void drpDepartmanlar1_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    string DepID = drpDepartmanlar1.SelectedValue.ToString();
    //    string sorgu = "SELECT AD.AltDepartmanID,AD.AltDepartmanAdi,AD.AltDepartmanKodu FROM Departmanlar AS D " +
    //                            "INNER JOIN AltDepartman AS AD ON AD.DepartmanID=" + Convert.ToInt32(DepID) + " " +
    //                            "GROUP BY AD.AltDepartmanAdi,AD.AltDepartmanKodu,AD.AltDepartmanID";

    //    drpAltDepartmanlar.DataTextField = "AltDepartmanAdi";
    //    drpAltDepartmanlar.DataValueField = "AltDepartmanID";
    //    drpAltDepartmanlar.DataSource = datadoldur(sorgu);
    //    drpAltDepartmanlar.DataBind();
    //}

    protected void dpSayfalama_PreRender(object sender, EventArgs e)
    {

        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        lstKullanicilar.DataSource = datadoldur(Session["KullaniciDoldur"].ToString());
        lstKullanicilar.DataBind();
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

    protected void lstKullanicilar_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        string itemID = "";
        if (e.CommandName == "Duzenle")
        {
            itemID = e.CommandArgument.ToString();

            if (DbConnUser.State == ConnectionState.Closed)
                DbConnUser.Open();

            int YetkiKodu = 0;

            string Sorgumuz = "SELECT USERID, KullaniciKodu, KullaniciAdi, " +
                            "CONVERT(varchar,DECRYPTBYPASSPHRASE ('Editor',Sifre)) AS Sifre, " +
                            "AdSoyad, D.DepartmanAdi,D.DepartmanKodu,D.DepartmanID,AD.AltDepartmanAdi, " +
                            "AD.AltDepartmanKodu,AD.AltDepartmanID,D1.DepartmanAdi AS StokluDepartman,D2.DepartmanAdi AS StoksuzDepartman, " +
                            "Y.Yetki_Unvani,Y.YetkiKodu, LastLogin AS Tarih   " +
                            "FROM Kullanicilar AS K  " +
                            "INNER JOIN Departmanlar AS D ON D.DepartmanID=K.DepartmanID " +
                            "INNER JOIN Departmanlar AS D1 ON D1.DepartmanID=K.StokluDepartman " +
                            "INNER JOIN Departmanlar AS D2 ON D2.DepartmanID=K.StoksuzDepartman  " +
                            "INNER JOIN AltDepartman AS AD ON AD.AltDepartmanID=K.AltDepartmanID  " +
                            "INNER JOIN Yetki_Unvanlari AS Y ON K.Yetki=Y.YetkiKodu " +
                            "WHERE K.USERID=" + Convert.ToInt32(itemID) + "";

            cmd = new SqlCommand(Sorgumuz, DbConnUser);
            cmd.CommandTimeout = 120;
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            //if (dr.Read())
            //{
            //    txtUSERID.Text = dr["USERID"].ToString();
            //    Alert.Show(txtUSERID.Text);
            //    txtKullanıcıKodu.Text = dr["KullaniciKodu"].ToString();
            //    txtKullaniciAdi.Text = dr["KullaniciAdi"].ToString();
            //    txtSifre.Text = dr["Sifre"].ToString();
            //    txtAdSoyad.Text = dr["AdSoyad"].ToString();                
            //    cmbDepartmanAdi.Text = dr["DepartmanAdi"].ToString();
            //    cmbDepartmanKodu.Text = dr["DepartmanKodu"].ToString();
            //    cmbAltDepartmanAdi.Text = dr["AltDepartmanAdi"].ToString();
            //    cmbAltDepartmanKodu.Text = dr["AltDepartmanKodu"].ToString();

            //    string ID = cmbDepartmanAdi.Value.ToString();
            //    string sorgu = "SELECT  DepartmanID, DepartmanAdi, DepartmanKodu FROM Departmanlar WHERE DepartmanID=" + Convert.ToInt32(ID) + "";
            //    cmbDepartmanKodu.TextField = "DepartmanKodu";
            //    cmbDepartmanKodu.ValueField = "DepartmanKodu";
            //    cmbDepartmanKodu.DataSource = datadoldur(sorgu);
            //    cmbDepartmanKodu.DataBind();

            //    YetkiKodu = Convert.ToInt32(dr["YetkiKodu"].ToString());
            //    cmbStokluDepartman.Text = dr["StokluDepartman"].ToString();
            //    cmbStoksuzDepartman.Text = dr["StoksuzDepartman"].ToString();

            //    string AltDepartmanSorgu = "SELECT AD.AltDepartmanID,AD.AltDepartmanAdi,AD.AltDepartmanKodu FROM Departmanlar AS D " +
            //            "INNER JOIN AltDepartman AS AD ON  AD.DepartmanID=" + ID + " " +
            //            "GROUP BY AD.AltDepartmanAdi,AD.AltDepartmanKodu,AD.AltDepartmanID";

            //    cmbAltDepartmanAdi.TextField = "AltDepartmanAdi";
            //    cmbAltDepartmanAdi.ValueField = "AltDepartmanID";
            //    cmbAltDepartmanKodu.ValueField = "AltDepartmanKodu";
            //    cmbAltDepartmanKodu.ValueField = "AltDepartmanKodu";
            //    cmbAltDepartmanAdi.DataSource = datadoldur(AltDepartmanSorgu);
            //    cmbAltDepartmanAdi.DataBind();
            //    cmbAltDepartmanKodu.DataSource = datadoldur(AltDepartmanSorgu);
            //    cmbAltDepartmanKodu.DataBind();

            //    //cmbAltDepartmanKodu.SelectedIndex = 0;
            //    //cmbAltDepartmanAdi.SelectedIndex = 0;
            //}
            //dr.Dispose();
            //dr.Close();

            //cmbYetkiler.Text = yetkiUnvaniBul(YetkiKodu);
        }
        if (e.CommandName == "Sil")
        {
            itemID = e.CommandArgument.ToString();

            if (DbConnUser.State == ConnectionState.Closed)
                DbConnUser.Open();

            string Sorgu = "DELETE FROM Kullanicilar WHERE USERID=" + Convert.ToInt32(itemID) + "";

            cmd = new SqlCommand(Sorgu, DbConnUser);
            cmd.CommandTimeout = 120;
            cmd.ExecuteNonQuery();

            Alert.Show("Kullanıcı Başarılı Bir Şekilde Silinmiştir");
        }
    }

    private string yetkiUnvaniBul(int YetkiKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT     Yetki_Unvani " +
                        "FROM         Yetki_Unvanlari " +
                        "WHERE     (YetkiKodu = " + YetkiKodu.ToString() + ")";
        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();
    }

    //protected void cmbDepartmanAdi_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    string ID = cmbDepartmanAdi.Value.ToString();
    //    int sID = cmbDepartmanAdi.SelectedIndex;

    //    string sorgu = "SELECT  DepartmanID, DepartmanAdi, DepartmanKodu FROM Departmanlar WHERE DepartmanID=" + Convert.ToInt32(ID) + "";
    //    cmbDepartmanKodu.TextField = "DepartmanKodu";
    //    cmbDepartmanKodu.ValueField = "DepartmanKodu";
    //    cmbDepartmanKodu.DataSource = datadoldur(sorgu);
    //    cmbDepartmanKodu.DataBind();
    //    cmbDepartmanKodu.SelectedIndex = sID;
    //    //cmbAltDepartmanKodu.SelectedIndex = sID;
    //}

    //protected void cmbAltDepartmanAdi_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    string ID = cmbDepartmanAdi.Value.ToString();
    //    int sID = cmbAltDepartmanAdi.SelectedIndex;

    //    if (sID < 0)
    //    {
    //        sID = 0;
    //    }

    //    string AltDepartmanSorgu = "SELECT AD.AltDepartmanID,AD.AltDepartmanAdi,AD.AltDepartmanKodu FROM Departmanlar AS D " +
    //    "INNER JOIN AltDepartman AS AD ON AD.DepartmanID=" + Convert.ToInt32(ID) + " " +
    //    "GROUP BY AD.AltDepartmanAdi,AD.AltDepartmanKodu,AD.AltDepartmanID";

    //    cmbAltDepartmanAdi.TextField = "AltDepartmanAdi";
    //    cmbAltDepartmanAdi.ValueField = "AltDepartmanID";
    //    cmbAltDepartmanKodu.ValueField = "AltDepartmanKodu";
    //    cmbAltDepartmanKodu.ValueField = "AltDepartmanKodu";
    //    cmbAltDepartmanAdi.DataSource = datadoldur(AltDepartmanSorgu);
    //    cmbAltDepartmanAdi.DataBind();
    //    cmbAltDepartmanKodu.DataSource = datadoldur(AltDepartmanSorgu);
    //    cmbAltDepartmanKodu.DataBind();
    //    cmbAltDepartmanKodu.SelectedIndex = sID;
    //    //cmbAltDepartmanAdi.SelectedIndex = 0;
    //}

    //protected void btnDuzenle_Click(object sender, EventArgs e)
    //{
    //    if (DbConnUser.State == ConnectionState.Closed)
    //        DbConnUser.Open();

    //    string AltDepartman = cmbAltDepartmanAdi.Value.ToString();
    //    string Departman = cmbDepartmanAdi.Value.ToString();

    //    string sorgu = "";

    //    int i;
    //    if (int.TryParse(AltDepartman, out i))
    //    {
    //        DepartmanDurum = true;
    //        AltDepartmanID = Convert.ToInt32(AltDepartman);
    //    }
    //    else
    //    {
    //        DepartmanDurum = false;
    //        AltDepartmanID = AltDepartmanIDBul(AltDepartman, Convert.ToInt32(Departman));
    //    }


    //    sorgu = "UPDATE Kullanicilar SET " +
    //                   "Sifre = EncryptByPassPhrase('Editor','" + txtSifre.Text + "'),  " +
    //                   "AdSoyad = '" + txtAdSoyad.Text + "',  " +
    //                   "DepartmanID = " + Convert.ToInt32(cmbDepartmanAdi.Value.ToString()) + ",  " +
    //                   "AltDepartmanID = " + AltDepartmanID + ",  " +
    //                   "StokluDepartman = " + Convert.ToInt32(cmbStokluDepartman.Value.ToString()) + ",  " +
    //                   "StoksuzDepartman = " + Convert.ToInt32(cmbStoksuzDepartman.Value.ToString()) + ",  " +
    //                   "Yetki = '" + cmbYetkiler.Value.ToString() + "' " +
    //                   "WHERE USERID=" + Convert.ToInt32(txtUSERID.Text) + "";


    //    cmd = new SqlCommand(sorgu, DbConnUser);
    //    cmd.CommandTimeout = 120;
    //    cmd.ExecuteNonQuery();

    //    //txtAdSoyad.Text = null;
    //    //txtKullaniciAdi.Text = null;
    //    //txtKullanıcıKodu.Text = null;
    //    //txtSifre.Text = null;
    //    //txtUSERID.Text = null;
    //    //cmbYetkiler.Text = null;

    //    Alert.Show("Kullanıcı Kaydı Başarılı Bir Şekilde Güncellenmiştir.");
    //    popupKullanicilar.ShowOnPageLoad = false;
    //}

    private int AltDepartmanIDBul(string AltDepartmanAdi, int DepartmanID)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT AD.AltDepartmanID FROM AltDepartman AS AD " +
                    "INNER JOIN Departmanlar AS D ON AD.AltDepartmanAdi='" + AltDepartmanAdi.ToString() + "' AND D.DepartmanID=" + DepartmanID + "";
        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (int)cmd.ExecuteScalar();
    }

    protected void btnGuncelle_Click(object sender, EventArgs e)
    {
        KullaniciDoldur();
    }
}