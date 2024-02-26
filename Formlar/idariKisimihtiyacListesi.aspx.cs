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
using System.IO;
using DevExpress.Web;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;

public partial class Formlar_idariKisimihtiyacListesi : System.Web.UI.Page
{

    #region Bağlantı Ayarları

    SqlConnection DbConnUser = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnLink = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnLink"].ToString());
    SqlCommand cmd;
    SqlDataReader dr;
    int TalepID;
    string SaticiDurum, ssKontrol, EvrakNo2, BirimFiyat2, KullanilacakYer2, Miktar2, StokAdi2, Birim2, Aciklama2, TalepEden2, Tutar2, idariTalepID2, Firma2;
    string[] EvrakNo, BirimFiyat, KullanilacakYer, Miktar, StokAdi, TalepEden, Tutar, idariTalepID, Firma, Aciklama, Birim;

    #endregion

    string YeniMalKodumuz, Aciklamamiz;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Giris"] != "Evet")
        {
            Response.Redirect("/Login.Aspx");
        }
        else
        {
            idariKisimListesi();
        }
    }

    private void idariKisimListesi()
    {
        string TalepEvrakNoKontrol = Session["idariEvrakNo"] as string;

        if (string.IsNullOrEmpty(TalepEvrakNoKontrol))
        {
            Alert.Show("Yazdırılacak Veri Bulunamadı.");
            ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
        }
        else
        {
            EvrakNo2 = Session["idariEvrakNo"].ToString();
            BirimFiyat2 = Session["idariBirimFiyat"].ToString();
            KullanilacakYer2 = Session["idariKullanilacakYer"].ToString();
            Miktar2 = Session["idariMiktar"].ToString();

            Birim2 = Session["idariBirim"].ToString();
            Aciklama2 = Session["idariMalAciklama"].ToString();

            StokAdi2 = Session["idariMalAdi"].ToString();
            TalepEden2 = Session["idariTalepEden"].ToString();
            Tutar2 = Session["idariTutar"].ToString();
            idariTalepID2 = Session["idariTalepID"].ToString();
            Firma2 = Session["idariFirma"].ToString();
            lblTarih.Text = Convert.ToString(DateTime.Now.ToString("dd-MM-yyyy"));

            EvrakNo = EvrakNo2.Split(',');
            BirimFiyat = BirimFiyat2.Split('/');
            KullanilacakYer = KullanilacakYer2.Split(',');

            Birim = Birim2.Split('/');
            Aciklama = Aciklama2.Split('}');

            Miktar = Miktar2.Split('/');
            StokAdi = StokAdi2.Split('}');
            TalepEden = TalepEden2.Split(',');
            Tutar = Tutar2.Split('/');
            idariTalepID = idariTalepID2.Split(',');
            Firma = Firma2.Split(',');

            for (int i = 0; i < EvrakNo.Length - 1; i++)
            {
                ssKontrol = SaticiDurumu(EvrakNo[i].ToString());
                ssKontrol = SaticiDurumKontrol(ssKontrol.ToString());

                YeniMalKodumuz = MalKodumuz(EvrakNo[i].ToString());

                if (YeniMalKodumuz.ToString().Substring(0, 1) == "M")
                {
                    Aciklamamiz = Aciklama[i].ToString();
                }
                else
                {
                    Aciklamamiz = StokAdi[i].ToString();
                }

                if (ssKontrol == "Onaylı Satıcı")
                {
                    SaticiDurum = "Onaylı Satıcı";
                }
                else
                {
                    SaticiDurum = Firma[i].ToString();
                }

                if (string.IsNullOrEmpty(SaticiDurum))
                {
                    SaticiDurum = "&nbsp;";
                }

                Literal Lt = new Literal();
                Lt.Text = "<tr> " +
                         "<td align=\"center\" style=\"border:1px solid Black; height:30px;\">" + EvrakNo[i].ToString() + "</td> " +
                         "<td align=\"center\" style=\"border:1px solid Black;\">" + Aciklamamiz.ToString() + "</td> " +
                         "<td align=\"center\" style=\"border:1px solid Black;\">" + Miktar[i].ToString().Replace('.', ',') + " " + Birim[i].ToString() + "</td> " +
                         "<td align=\"center\" style=\"border:1px solid Black;\">" + BirimFiyat[i].ToString().Replace('.', ',') + "</td> " +
                         "<td align=\"center\" style=\"border:1px solid Black;\">" + Tutar[i].ToString().Replace('.', ',') + "</td> " +
                         "<td align=\"center\" style=\"border:1px solid Black;\">" + TalepEden[i].ToString() + "</td> " +
                         "<td align=\"center\" style=\"border:1px solid Black;\">" + KullanilacakYer[i].ToString() + "</td> " +
                         "<td align=\"center\" style=\"border:1px solid Black;\">" + SaticiDurum.ToString() + "</td> " +
                         "</tr> ";
                panelidariListe.Controls.Add(Lt);

            }
        }
    }

    protected void imgYazdir_Click(object sender, ImageClickEventArgs e)
    {
        string TalepEvrakNoKontrol = Session["idariEvrakNo"] as string;

        if (string.IsNullOrEmpty(TalepEvrakNoKontrol))
        {
            Alert.Show("Yazdırılacak Veri Bulunamadı.");
            ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
        }
        else
        {
            EvrakNo2 = Session["idariEvrakNo"].ToString();
            EvrakNo = EvrakNo2.Split(',');

            if (DbConnUser.State == ConnectionState.Closed)
                DbConnUser.Open();

            for (int i = 0; i < EvrakNo.Length - 1; i++)
            {
                string Sorgu = "UPDATE Tlp SET " +
                               "idariKisim=1 " +
                               "WHERE EvrakNoTarih+EvrakNo='" + EvrakNo[i].ToString() + "'";
                cmd = new SqlCommand(Sorgu, DbConnUser);
                cmd.CommandTimeout = 120;
                cmd.ExecuteNonQuery();
            }
            BaglantilariKapat();
        }
    }

    private void BaglantilariKapat()
    {
        DbConnUser.Close();
        DbConnLink.Close();
    }

    private string SaticiDurumKontrol(string MalKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string Sorgu = "SELECT dbo.fn_SaticiDurumu('" + MalKodu + "') AS SaticiDurumu";

        cmd = new SqlCommand(Sorgu, DbConnUser);
        return (string)cmd.ExecuteScalar();
    }

    private string SaticiDurumu(string EvrakNo)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT MalKodu FROM Tlp WHERE EvrakNoTarih+EvrakNo='" + EvrakNo + "'";

        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();
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