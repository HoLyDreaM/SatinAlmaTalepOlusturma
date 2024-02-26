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

public partial class UserControl_KullaniciDuzenle : System.Web.UI.Page
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
        if (Session["Giris"] != "Evet")
        {
            Alert.Show("Lütfen Sisteme Tekrardan Giriş Yapınız.");
            ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
        }
        else
        {
            if (!IsPostBack)
            {
                KullaniciEdit();
            }
        }
    }

    protected void cmbDepartmanAdi_SelectedIndexChanged(object sender, EventArgs e)
    {
        string ID = cmbDepartmanAdi.Value.ToString();
        int sID = cmbDepartmanAdi.SelectedIndex;

        string sorgu = "SELECT  DepartmanID, DepartmanAdi, DepartmanKodu FROM Departmanlar WHERE DepartmanID=" + Convert.ToInt32(ID) + "";
        cmbDepartmanKodu.TextField = "DepartmanKodu";
        cmbDepartmanKodu.ValueField = "DepartmanKodu";
        cmbDepartmanKodu.DataSource = datadoldur(sorgu);
        cmbDepartmanKodu.DataBind();
        cmbDepartmanKodu.SelectedIndex = sID;
        //cmbAltDepartmanKodu.SelectedIndex = sID;

        if (sID < 0)
        {
            sID = 0;
        }

        string AltDepartmanSorgu = "SELECT AD.AltDepartmanID,AD.AltDepartmanAdi,AD.AltDepartmanKodu FROM Departmanlar AS D " +
        "INNER JOIN AltDepartman AS AD ON AD.DepartmanID=" + Convert.ToInt32(ID) + " " +
        "GROUP BY AD.AltDepartmanAdi,AD.AltDepartmanKodu,AD.AltDepartmanID";

        cmbAltDepartmanAdi.TextField = "AltDepartmanAdi";
        cmbAltDepartmanAdi.ValueField = "AltDepartmanID";

        cmbAltDepartmanKodu.TextField = "AltDepartmanKodu";
        cmbAltDepartmanKodu.ValueField = "AltDepartmanKodu";

        cmbBagliAltDept.TextField = "AltDepartmanAdi";
        cmbBagliAltDept.ValueField = "AltDepartmanID";

        cmbAltDepartmanAdi.DataSource = datadoldur(AltDepartmanSorgu);
        cmbAltDepartmanAdi.DataBind();

        cmbAltDepartmanKodu.DataSource = datadoldur(AltDepartmanSorgu);
        cmbAltDepartmanKodu.DataBind();

        cmbBagliAltDept.DataSource = datadoldur(AltDepartmanSorgu);
        cmbBagliAltDept.DataBind();

        cmbAltDepartmanKodu.SelectedIndex = sID;

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

        cmbAltDepartmanAdi.TextField = "AltDepartmanAdi";
        cmbAltDepartmanAdi.ValueField = "AltDepartmanID";

        cmbAltDepartmanKodu.TextField = "AltDepartmanKodu";
        cmbAltDepartmanKodu.ValueField = "AltDepartmanKodu";

        cmbAltDepartmanAdi.DataSource = datadoldur(AltDepartmanSorgu);
        cmbAltDepartmanAdi.DataBind();

        cmbAltDepartmanKodu.DataSource = datadoldur(AltDepartmanSorgu);
        cmbAltDepartmanKodu.DataBind();

        cmbAltDepartmanKodu.SelectedIndex = sID;
        //cmbAltDepartmanAdi.SelectedIndex = 0;
    }

    protected void btnDuzenle_Click(object sender, EventArgs e)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string AltDepartman = "";
        string BagliAltDepartman = "";

        string Departman = cmbDepartmanAdi.Value.ToString();
        string Siparis = chkSiparis.Checked.ToString();
        string SatinAlma = chkSatinAlma.Checked.ToString();
        string AnaDept = chkAnaDepartman.Checked.ToString();
        string KullDurum = chkKullaniciDurum.Checked.ToString();
        string KopsRaporuAl = cmbKopsDurumRaporu.SelectedItem.Value.ToString();
        bool KopsDurumRaporu = Convert.ToBoolean(KopsRaporuAl);

        if (string.IsNullOrEmpty(cmbAltDepartmanAdi.Text.ToString()))
        {
            AltDepartman = "0";
        }
        else
        {
            AltDepartman = cmbAltDepartmanAdi.Value.ToString();
        }

        if (string.IsNullOrEmpty(cmbBagliAltDept.Text.ToString()))
        {
            BagliAltDepartman = "0";
        }
        else
        {
            BagliAltDepartman = cmbBagliAltDept.Value.ToString();
        }

        string MasrafKodu = "0";
        bool MasrafKoduCheck = chkMasrafMerkezi.Checked;

        if (MasrafKoduCheck == false)
        {
            MasrafKodu = cmbMasrafMerkeziKodu.Value.ToString();
        }

        string Birim = "";
        string MenuKodlari = "";

        if (string.IsNullOrEmpty(cmbBirimler.Text.ToString()))
        {
            Birim = "idari Birim";
        }
        else
        {
            Birim = cmbBirimler.Value.ToString();
        }

        if (string.IsNullOrEmpty(cmbMenuKodlari.Value.ToString()))
        {
            MenuKodlari = "";
        }
        else
        {
            MenuKodlari = cmbMenuKodlari.Value.ToString();
        }

        string sorgu = "";

        int i;
        if (int.TryParse(AltDepartman, out i))
        {
            DepartmanDurum = true;
            AltDepartmanID = Convert.ToInt32(AltDepartman);
        }
        else
        {
            DepartmanDurum = false;
            AltDepartmanID = AltDepartmanIDBul(AltDepartman, Convert.ToInt32(Departman));
        }


        sorgu = "UPDATE Kullanicilar SET " +
                       "Sifre = EncryptByPassPhrase('Editor','" + txtSifre.Text + "'),  " +
                       "AdSoyad = '" + txtAdSoyad.Text + "',  " +
                       "Durum = '" + KullDurum.ToString() + "',  " +
                       "DepartmanID = " + Convert.ToInt32(cmbDepartmanAdi.Value.ToString()) + ",  " +
                       "AltDepartmanID = " + AltDepartmanID + ",  " +
                       "BagliAltDepartman = " + BagliAltDepartman + ",  " +
                       "DurumRaporu = '" + Convert.ToString(KopsDurumRaporu) + "', " +
            //"StokluDepartman = " + Convert.ToInt32(cmbStokluDepartman.Value.ToString()) + ",  " +
            //"StoksuzDepartman = " + Convert.ToInt32(cmbStoksuzDepartman.Value.ToString()) + ",  " +
                       "Siparis ='" + Siparis.ToString() + "'," +
                       "SatinAlma ='" + SatinAlma.ToString() + "', " +
                       "AnaDepartman = '" + AnaDept + "'," +
                       "MasrafMerkezi ='" + MasrafKoduCheck.ToString() + "', " +
                       "MasrafMerkeziKodu ='" + MasrafKodu.ToString() + "', " +
                       "Birim = '" + Birim.ToString() + "', " +
                       "MenuKodu = '" + MenuKodlari.ToString() + "', " +
                       "Yetki = '" + cmbYetkiler.Value.ToString() + "' " +
                       "WHERE USERID=" + Convert.ToInt32(txtUSERID.Text) + "";


        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        cmd.ExecuteNonQuery();

        BaglantilariKapat();

        Alert.Show("Kullanıcı Kaydı Başarılı Bir Şekilde Güncellenmiştir.");
        ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
    }

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

    private void KullaniciEdit()
    {
        string KullaniciID = Request.QueryString["USERID"].ToString();

        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        int YetkiKodu = 0;

        string Sorgumuz = "SELECT USERID, KullaniciKodu, KullaniciAdi, " +
                        "CONVERT(varchar,DECRYPTBYPASSPHRASE ('Editor',Sifre)) AS Sifre, (CASE DurumRaporu WHEN 'True' THEN 'Evet' ELSE 'Hayır' END) AS DurumRaporu, " +
                        "AdSoyad, D.DepartmanAdi,D.DepartmanKodu,D.DepartmanID,AD.AltDepartmanAdi, " +
                        "AD.AltDepartmanKodu,AD.AltDepartmanID,AD1.AltDepartmanID AS BagliAltDepartmanID, " +
                        "AD1.AltDepartmanAdi AS BagliAltDepartmanAdi,AnaDepartman,Durum, " +
            //"SD.DepartmanAdi AS StokluDepartman,SD.DepartmanID AS StokluDepartmanID,  " +
            //"DS.DepartmanAdi AS StoksuzDepartman,DS.DepartmanID AS StoksuzDepartmanID,  " +
                        "Siparis,SatinAlma,MasrafMerkezi,M.MasrafMerkeziAdi,Y.Yetki_Unvani,Y.YetkiKodu, " +
                        "LastLogin AS Tarih,Birim,MenuKodu   " +
                        "FROM Kullanicilar AS K  " +
                        "INNER JOIN Departmanlar AS D ON D.DepartmanID=K.DepartmanID  " +
                        "LEFT JOIN AltDepartman AS AD ON AD.AltDepartmanID=K.AltDepartmanID  " +
                        "LEFT JOIN AltDepartman AS AD1 ON AD1.AltDepartmanID=K.BagliAltDepartman " +
                        "INNER JOIN Yetki_Unvanlari AS Y ON K.Yetki=Y.YetkiKodu " +
            //"INNER JOIN Departmanlar AS SD ON SD.DepartmanID=K.StokluDepartman " +
            //"INNER JOIN Departmanlar AS DS ON DS.DepartmanID=K.StoksuzDepartman " +
                        "LEFT JOIN MasrafMerkezi AS M ON K.MasrafMerkeziKodu=M.MasrafMerkeziKodu " +
                        "WHERE K.USERID=" + Convert.ToInt32(KullaniciID) + "";

        cmd = new SqlCommand(Sorgumuz, DbConnUser);
        cmd.CommandTimeout = 120;
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        if (dr.Read())
        {
            txtUSERID.Text = dr["USERID"].ToString();
            txtKullanıcıKodu.Text = dr["KullaniciKodu"].ToString();
            txtKullaniciAdi.Text = dr["KullaniciAdi"].ToString();
            txtSifre.Text = dr["Sifre"].ToString();
            txtAdSoyad.Text = dr["AdSoyad"].ToString();
            cmbDepartmanAdi.Text = dr["DepartmanAdi"].ToString();
            cmbDepartmanAdi.Value = dr["DepartmanID"].ToString();
            cmbDepartmanKodu.Text = dr["DepartmanKodu"].ToString();
            cmbAltDepartmanAdi.Text = dr["AltDepartmanAdi"].ToString();
            cmbAltDepartmanKodu.Text = dr["AltDepartmanKodu"].ToString();
            cmbAltDepartmanAdi.Value = dr["AltDepartmanID"].ToString();

            cmbBagliAltDept.Text = dr["BagliAltDepartmanAdi"].ToString();
            cmbBagliAltDept.Value = dr["BagliAltDepartmanID"].ToString();

            chkSatinAlma.Checked = Convert.ToBoolean(dr["SatinAlma"].ToString());
            chkKullaniciDurum.Checked = Convert.ToBoolean(dr["Durum"].ToString());
            chkAnaDepartman.Checked = Convert.ToBoolean(dr["AnaDepartman"].ToString());
            chkSiparis.Checked = Convert.ToBoolean(dr["Siparis"].ToString());
            chkMasrafMerkezi.Checked = Convert.ToBoolean(dr["MasrafMerkezi"].ToString());
            cmbMasrafMerkeziKodu.Text = dr["MasrafMerkeziAdi"].ToString();
            cmbBirimler.Text = dr["Birim"].ToString();
            cmbMenuKodlari.Text = dr["MenuKodu"].ToString();
            cmbKopsDurumRaporu.SelectedItem.Text = dr["DurumRaporu"].ToString();

            string ID = cmbDepartmanAdi.Value.ToString();
            string sorgu = "SELECT  DepartmanID, DepartmanAdi, DepartmanKodu FROM Departmanlar WHERE DepartmanID=" + Convert.ToInt32(ID) + "";
            cmbDepartmanKodu.TextField = "DepartmanKodu";
            cmbDepartmanKodu.ValueField = "DepartmanKodu";
            cmbDepartmanKodu.DataSource = datadoldur(sorgu);
            cmbDepartmanKodu.DataBind();

            YetkiKodu = Convert.ToInt32(dr["YetkiKodu"].ToString());
            cmbYetkiler.Text = dr["Yetki_Unvani"].ToString();
            cmbYetkiler.Value = dr["YetkiKodu"].ToString();
            //cmbStokluDepartman.Text = dr["StokluDepartman"].ToString();
            //cmbStokluDepartman.Value = dr["StokluDepartmanID"].ToString();
            //cmbStoksuzDepartman.Text = dr["StoksuzDepartman"].ToString();
            //cmbStoksuzDepartman.Value = dr["StoksuzDepartmanID"].ToString();

            string AltDepartmanSorgu = "SELECT AD.AltDepartmanID,AD.AltDepartmanAdi,AD.AltDepartmanKodu FROM Departmanlar AS D " +
                    "INNER JOIN AltDepartman AS AD ON  AD.DepartmanID=" + ID + " " +
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

            //cmbAltDepartmanKodu.SelectedIndex = 0;
            //cmbAltDepartmanAdi.SelectedIndex = 0;
        }
        dr.Dispose();
        dr.Close();
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

    private void BaglantilariKapat()
    {
        //DbConnUser.Dispose();
        DbConnUser.Close();

        //DbConnLink.Dispose();
        DbConnLink.Close();

        //cmd.Dispose();

        //dr.Dispose();
        //dr.Close();
    }
}