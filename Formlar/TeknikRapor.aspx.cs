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

public partial class Formlar_TeknikRapor : System.Web.UI.Page
{

    #region Bağlantı Ayarları

    SqlConnection DbConnUser = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnLink = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnLink"].ToString());
    SqlCommand cmd;
    SqlDataReader dr;
    int TalepID;
    string SaticiDurum, ssKontrol, EvrakNo2, Aciklama2, BirimFiyat2, KullanilacakYer2, Miktar2, Birim2, StokAdi2, TalepEden2, Tutar2, idariTalepID2, Firma2;
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
            TeknikKisimListesi();
        }
    }

    private void TeknikKisimListesi()
    {
        string TalepEvrakNoKontrol = Session["TeknikEvrakNo"] as string;

        if (string.IsNullOrEmpty(TalepEvrakNoKontrol))
        {
            Alert.Show("Yazdırılacak Veri Bulunamadı.");
            ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
        }
        else
        {
            EvrakNo2 = Session["TeknikEvrakNo"].ToString();
            BirimFiyat2 = Session["TeknikBirimFiyat"].ToString();
            KullanilacakYer2 = Session["TeknikKullanilacakYer"].ToString();
            Miktar2 = Session["TeknikMiktar"].ToString();
            Birim2 = Session["TeknikBirim"].ToString();
            StokAdi2 = Session["TeknikMalAdi"].ToString();
            TalepEden2 = Session["TeknikTalepEden"].ToString();
            Tutar2 = Session["TeknikTutar"].ToString();
            idariTalepID2 = Session["TeknikTalepID"].ToString();
            Firma2 = Session["TeknikFirma"].ToString();
            Aciklama2 = Session["TeknikMalAciklama"].ToString();
            lblTarih.Text = Convert.ToString(DateTime.Now.ToString("dd-MM-yyyy"));

            EvrakNo = EvrakNo2.Split(',');
            Birim = Birim2.Split('/');
            Aciklama = Aciklama2.Split('}');
            BirimFiyat = BirimFiyat2.Split('/');
            KullanilacakYer = KullanilacakYer2.Split(',');
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
                panelTeknikListe.Controls.Add(Lt);

            }
        }
    }

    protected void imgYazdir_Click(object sender, ImageClickEventArgs e)
    {
        string TalepEvrakNoKontrol = Session["TeknikEvrakNo"] as string;

        if (string.IsNullOrEmpty(TalepEvrakNoKontrol))
        {
            Alert.Show("Yazdırılacak Veri Bulunamadı.");
            ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
        }
        else
        {
            EvrakNo2 = Session["TeknikEvrakNo"].ToString();
            EvrakNo = EvrakNo2.Split(',');

            if (DbConnUser.State == ConnectionState.Closed)
                DbConnUser.Open();

            for (int i = 0; i < EvrakNo.Length; i++)
            {
                if (DbConnUser.State == ConnectionState.Closed)
                    DbConnUser.Open();

                string Sorgu = "UPDATE Tlp SET " +
                               "TeknikKisim=1 " +
                                "WHERE EvrakNoTarih+EvrakNo='" + EvrakNo[i].ToString() + "'";
                cmd = new SqlCommand(Sorgu, DbConnUser);
                cmd.CommandTimeout = 120;
                cmd.ExecuteNonQuery();

            }
            BaglantilariKapat();
        }
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

    private void BaglantilariKapat()
    {
        DbConnUser.Close();
        DbConnLink.Close();
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