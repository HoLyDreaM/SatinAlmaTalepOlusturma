using System;
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

public partial class Anasayfa : System.Web.UI.MasterPage
{

    #region Bağlantı Ayarları

    SqlConnection Kullanicilar = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlCommand cmd;
    bool HomePage, StokKartiSorgulama, IhtiyacArama, Talepler, Formlar, idariKisim, TeknikKisim,KopsDurumRaporu,
        ihtiyacPusulasi, Tanimlar, KullaniciTanimi, StokCikisFormu, StokCikisHareketi, Fax, Tanimlamalar, SiparisFormu, StokTanimlama;


    SqlConnection DbConnUser = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConMaxYetki = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnLink = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnLink"].ToString());
    SqlConnection DbConnDepartmanKodu = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnYetkiKoduBul = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnYetkiTamamla = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnYetkilendir = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());

    SqlConnection DbConnTalepIrsaliyeGuncelle = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnTalepAkisiIrsaliyeGuncelle = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());

    SqlConnection DbConnSiparisBul = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnIrsaliyeDurumu = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnLink"].ToString());

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Giris"] != "Evet")
        {
            Response.Redirect("Login.Aspx");
        }
        else
        {
            try
            {
                DateTime dtGirisLogin = new DateTime();
                DateTime dt1 = new DateTime();
                string LoginTarih = "";

                if (string.IsNullOrEmpty(Session["GirisTarihi"].ToString()))
                {
                    dtGirisLogin = Convert.ToDateTime(DateTime.Now.ToString("dd-MM-yyyy"));
                }
                else
                {
                    string LoginTime = Session["GirisTarihi"].ToString();
                    dt1 = Convert.ToDateTime(LoginTime);
                    String dt2 = dt1.ToString("dd-MM-yyyy");
                    LoginTarih = dt2;
                }

                kullaniciAdi.InnerText = Session["Name"].ToString();
                Tarih.InnerText = Convert.ToString(LoginTarih);
                Bilgiler.HRef = "ProfilEdit.aspx?ID=" + Session["ID"].ToString();

                HomePage = Convert.ToBoolean(Session["HomePage"].ToString());
                StokKartiSorgulama = Convert.ToBoolean(Session["StokKartiAra"].ToString());
                IhtiyacArama = Convert.ToBoolean(Session["IhtiyacArama"].ToString());
                Talepler = Convert.ToBoolean(Session["Talepler"].ToString());
                Formlar = Convert.ToBoolean(Session["Formlar"].ToString());
                idariKisim = Convert.ToBoolean(Session["idariKisim"].ToString());
                TeknikKisim = Convert.ToBoolean(Session["TeknikKisim"].ToString());
                ihtiyacPusulasi = Convert.ToBoolean(Session["ihtiyacPusulasi"].ToString());
                StokCikisFormu = Convert.ToBoolean(Session["StokCikisFormu"].ToString());
                StokCikisHareketi = Convert.ToBoolean(Session["StokCikisHareketi"].ToString());
                Fax = Convert.ToBoolean(Session["FaxCekimi"].ToString());
                Tanimlar = Convert.ToBoolean(Session["Tanimlar"].ToString());
                KullaniciTanimi = Convert.ToBoolean(Session["KullaniciTanimi"].ToString());
                Tanimlamalar = Convert.ToBoolean(Session["Tanimlamalar"].ToString());
                SiparisFormu = Convert.ToBoolean(Session["SiparisFormu"].ToString());
                StokTanimlama = Convert.ToBoolean(Session["StokTanimlama"].ToString());
                KopsDurumRaporu = Convert.ToBoolean(Session["DurumRaporu"].ToString());

                DevMenu.Items[0].Visible = HomePage;
                DevMenu.Items[1].Visible = StokKartiSorgulama;
                DevMenu.Items[2].Visible = IhtiyacArama;
                DevMenu.Items[3].Visible = Talepler;
                DevMenu.Items[4].Visible = Formlar;
                DevMenu.Items[4].Items[0].Visible = idariKisim;
                DevMenu.Items[4].Items[1].Visible = TeknikKisim;
                DevMenu.Items[4].Items[2].Visible = Fax;
                DevMenu.Items[4].Items[3].Visible = ihtiyacPusulasi;
                DevMenu.Items[4].Items[4].Visible = StokCikisHareketi;
                DevMenu.Items[4].Items[5].Visible = StokCikisFormu;
                DevMenu.Items[4].Items[6].Visible = SiparisFormu;
                DevMenu.Items[4].Items[7].Visible = StokTanimlama;

                if (KopsDurumRaporu == true)
                {
                    DevMenu.Items[4].Visible = true;
                    DevMenu.Items[4].Items[8].Visible = KopsDurumRaporu;
                }
                else
                {
                    DevMenu.Items[4].Items[8].Visible = KopsDurumRaporu;
                }
                DevMenu.Items[5].Visible = Tanimlamalar;
                DevMenu.Items[5].Items[0].Visible = KullaniciTanimi;
                DevMenu.Items[5].Items[1].Visible = Tanimlamalar;
            }
            catch (Exception ex)
            {

                Alert.Show(ex.ToString());
            }

            //BaglantilariKapat();
        }
    }

    private void BaglantilariKapat()
    {
        DbConnUser.Dispose();
        DbConnUser.Close();

        DbConMaxYetki.Dispose();
        DbConMaxYetki.Close();

        DbConnLink.Dispose();
        DbConnLink.Close();

        DbConnDepartmanKodu.Dispose();
        DbConnDepartmanKodu.Close();

        DbConnYetkiKoduBul.Dispose();
        DbConnYetkiKoduBul.Close();

        DbConnYetkiTamamla.Dispose();
        DbConnYetkiTamamla.Close();

        DbConnYetkilendir.Dispose();
        DbConnYetkilendir.Close();

        DbConnTalepIrsaliyeGuncelle.Dispose();
        DbConnTalepIrsaliyeGuncelle.Close();

        DbConnTalepAkisiIrsaliyeGuncelle.Dispose();
        DbConnTalepAkisiIrsaliyeGuncelle.Close();

        DbConnSiparisBul.Dispose();
        DbConnSiparisBul.Close();

        DbConnIrsaliyeDurumu.Dispose();
        DbConnIrsaliyeDurumu.Close();
    }
}
