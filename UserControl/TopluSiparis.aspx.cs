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


public partial class UserControl_TopluSiparis : System.Web.UI.Page
{

    #region Bağlantı Ayarları

    SqlConnection DbConnUser = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnDovizTuru = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnSiparisKarti = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnYetkiKoduBul = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnLink = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnLink"].ToString());
    SqlConnection DbConnLinkDoviz = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnLinkDoviz"].ToString());
    SqlConnection DbConnYetkiTamamla = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbSiparisDurumu = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlCommand cmd, cmdSiparisDurumu, cmdSiparisKarti, cmdDovizTuru;
    SqlDataReader dr, drAciklama, drSiparisKarti;

    int SEQNO, TarihRakam, SiparisKontrol, KdvOranimiz;
    double Tarih;
    decimal GenelToplam, GenelKdv, MalBedeli, SiparisMiktarimiz;
    string SiparisNumarasi, KayitYetki, PcAdi, LinkMasrafKod;
    bool SiparisDurum;

    #endregion

    #region Tanımlar

    string[] EvrakNoParcala;
    string[] MalKodlariParcala;
    string[] MalAdlariParcala;
    string[] MiktarParcala;
    string[] KdvParcala;
    string[] TalepIDParcala;
    string[] AciklamaSatirParcala;
    string ihtiyacNo, MalKodu, Miktar, Kdv, TalepID, MalAdi, Aciklama1, Aciklama2, Aciklama3, AciklamaSatirimiz, DovizReferansTuru;

    public TextBox[] txtTalepID;
    public TextBox[] txtMalAdi;
    public TextBox[] txtMalKodu;
    public TextBox[] txtAciklama;
    public TextBox[] txtKdv;
    public TextBox[] txtFiyat;
    public TextBox[] txtMiktar;
    public TextBox[] txtEvrakNo;
    public DropDownList[] drpDepoKodlari;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Giris"] != "Evet")
        {
            Alert.Show("Lütfen Sisteme Tekrar Giriş Yapın.");
            ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
        }
        else
        {
            SiparisTablosuEkle();

            if (!IsPostBack)
            {
                dtSiparisTarihi.Date = DateTime.Now;

                string sorgu1 = "CREATE TABLE #DovizCinsleri( " +
                              "DovizCinsi NVARCHAR(50), " +
                              "ParaBirimi INT) " +
                              "INSERT INTO #DovizCinsleri " +
                              "SELECT 'Yerel' AS DovizCinsi,1 AS ParaBirimi " +
                              "UNION ALL " +
                              "SELECT 'Doviz' AS DovizCinsi,2 AS ParaBirimi " +
                              "SELECT * FROM #DovizCinsleri " +
                              "DROP TABLE #DovizCinsleri ";

                drpParaBirimi.DataTextField = "DovizCinsi";
                drpParaBirimi.DataValueField = "ParaBirimi";
                drpParaBirimi.DataSource = datadoldur(sorgu1);
                drpParaBirimi.DataBind();

                DovizReferansTuru = DovizTuru();

                string sorgu = "SELECT 'Seçim Yapınız' AS DovizKodu,'Seçim Yapınız' AS DovizAdi " +
                             "UNION ALL " +
                             "SELECT CAR006_ReferansKodu AS DovizKodu,CAR006_ReferansAciklamasi AS DovizAdi " +
                             "FROM CAR006 WITH(NOLOCK) " +
                             "WHERE ((CAR006_ReferansTuru = " + DovizReferansTuru + "))";

                drpDovizCinsi.DataTextField = "DovizAdi";
                drpDovizCinsi.DataValueField = "DovizKodu";
                drpDovizCinsi.DataSource = datadoldurDoviz(sorgu);
                drpDovizCinsi.DataBind();
            }
        }
    }

    protected void btnTopluSiparis_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < EvrakNoParcala.Length; i++)
        {
            string DepoKontrolEditoruz = drpDepoKodlari[i].SelectedItem.Text.ToString();

            if (DepoKontrolEditoruz == "Seçiniz")
            {
                Alert.Show("Sipariş Depolarından Seçmedikleriniz Var.Lütfen Kontrol Ediniz.");
                return;
            }
        }

        string HesapKodu = drpCariAdi.SelectedItem.Value.ToString();

        string CariAdiKontrolu = drpCariAdi.SelectedItem.Value.ToString();
        string DovizKuru = txtDovizKuru.Text;

        for (int i = 0; i < EvrakNoParcala.Length; i++)
        {
            string FiyatKontrolu = txtFiyat[i].Text;

            if (string.IsNullOrEmpty(FiyatKontrolu))
            {
                Alert.Show("Fiyatı Yazılmayan Siparişleriniz Var Lütfen Kontrol Edip Tekrar Deneyin.");
                return;
            }
            else if (string.IsNullOrEmpty(CariAdiKontrolu))
            {
                Alert.Show("Cari Hesap Seçmediniz.Lütfen Seçim Yapıp Tekrar Deneyin.");
                return;
            }
            else
            {

                string MalKodu = txtMalKodu[i].Text;
                string Aciklama = txtAciklama[i].Text;

                string DovizCinsi = drpDovizCinsi.SelectedItem.Value.ToString();
                string IslemTipi = drpParaBirimi.SelectedItem.Value.ToString();

                decimal DovizToplami = 0;

                string KKdv = txtKdv[i].Text.Replace(',', '.');
                string MMiktar = txtMiktar[i].Text.Replace(',', '.');
                string FFiyat = txtFiyat[i].Text.Replace(',', '.');

                decimal Kdv = 0;
                decimal Miktar = 0;
                decimal Fiyat = 0;
                decimal KdvTutari = 0;
                decimal Tutar = 0;

                Kdv = Convert.ToDecimal(KKdv.ToString());
                Miktar = Convert.ToDecimal(MMiktar.ToString());
                Fiyat = Convert.ToDecimal(FFiyat.ToString());
                KdvTutari = ((Math.Round(Miktar, 4) * Math.Round(Fiyat, 6)) * Kdv) / 100;
                Tutar = (Math.Round(Miktar, 4) * Math.Round(Fiyat, 6));

                if (DovizCinsi == "Seçim Yapınız")
                {
                    DovizCinsi = "";
                }

                if (!string.IsNullOrEmpty(DovizKuru))
                {
                    if (DovizKuru != "0")
                    {
                        DovizKuru = DovizKuru.ToString().Replace(',', '.');
                        DovizToplami = Tutar / Convert.ToDecimal(DovizKuru);
                        DovizToplami = Math.Round(DovizToplami, 2);
                    }
                }
                else
                {
                    DovizKuru = "0";
                }

                //if (Convert.ToInt32(IslemTipi) == 2)
                //{
                //    DovizToplami = 0;
                //}

                PcAdi = System.Net.Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]).HostName.ToString();

                if (PcAdi.ToString().Length > 10)
                {
                    PcAdi = PcAdi.ToString().Substring(0, 10);
                }

                string EvrakNo = txtEvrakNo[i].Text;
                string MasrafMerkezi = MasrafMerkeziBul(EvrakNo, MalKodu);
                LinkMasrafKod = LinkMasrafKodu(MasrafMerkezi);
                string DepoKod = drpDepoKodlari[i].SelectedItem.Value.ToString();


                if (string.IsNullOrEmpty(LinkMasrafKod))
                {
                    MasrafMerkezi = "999";
                }
                else
                {
                    MasrafMerkezi = LinkMasrafKod;
                }

                string SiparisSeriNo = SiparisNoSeri();
                int SipNo = SiparisNo();
                string SiparisNums = Convert.ToString(SipNo);
                SiparisNums = SiparisNums.PadLeft(6, '0');
                SiparisNumarasi = SiparisSeriNo + SiparisNums;

                string GirenKodu = Session["KullAdi"].ToString();

                if (GirenKodu.ToString().Length > 10)
                {
                    GirenKodu = GirenKodu.ToString().Substring(0, 10);
                }

                SEQNO = SeqNo();

                DateTime obj = new DateTime();
                obj = Convert.ToDateTime(dtSiparisTarihi.Date);
                string str;
                Tarih = 0;
                Tarih = obj.ToOADate();
                str = Tarih.ToString().Substring(0, 5);
                TarihRakam = Convert.ToInt32(str);

                //DateTime DtSaat = new DateTime();
                //DtSaat = Convert.ToDateTime(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
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

                Stk002Ekle(MalKodu, TarihRakam, HesapKodu, SiparisNumarasi, Miktar, Fiyat,
                Tutar, KdvTutari, SEQNO, Saatimiz, PcAdi, EvrakNo, MasrafMerkezi, Aciklama.ToString().Replace("'", "''"),
                Convert.ToDecimal(DovizKuru.ToString()), DovizCinsi, DovizToplami, Convert.ToInt32(IslemTipi), DepoKod, Convert.ToString(Convert.ToInt32(Kdv)));

                Stk004Guncelle(TarihRakam, Saatimiz, PcAdi, Miktar, MalKodu);

                string CariUnvan = drpCariAdi.SelectedItem.Text.ToString().Replace("'", "''");

                SiparisKartlariGuncelle(EvrakNo, SiparisNumarasi, Miktar, CariUnvan, Fiyat);

            }
        }

        CariEkle();

        for (int i = 0; i < EvrakNoParcala.Length; i++)
        {
            if (string.IsNullOrEmpty(txtFiyat[i].Text))
            {
                Alert.Show("Fiyatı Yazılmayan Siparişleriniz Var Lütfen Kontrol Edip Tekrar Deneyin.");
                return;
            }
            else if (string.IsNullOrEmpty(drpCariAdi.SelectedItem.Value.ToString()))
            {
                Alert.Show("Cari Hesap Seçmediniz.Lütfen Seçim Yapıp Tekrar Deneyin.");
                return;
            }
            else
            {
                string EvrakNo = txtEvrakNo[i].Text;
                string MalKodu = txtMalKodu[i].Text;
                string MMdiktar = txtMiktar[i].Text;
                decimal Miktar = Convert.ToDecimal(MMdiktar.ToString().Replace(',', '.'));
                SiparisMiktarimiz = SiparisMiktariBul(EvrakNo, MalKodu);

                ParcaliTalepSiparisDurumGuncelle();
            }
        }

        SiparisNoGuncelle();

        BaglantilariKapat();

        Alert.Show("Siparişiniz Başarılı Bir Şekilde Eklenmiştir.Siparis Numaranız : " + SiparisNumarasi.ToString() + "");
        ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
    }

    private void SiparisTablosuEkle()
    {
        string Kontrol = Session["TalepEvrakNumaralari"] as string;

        if (!string.IsNullOrEmpty(Kontrol))
        {
            ihtiyacNo = Session["TalepEvrakNumaralari"].ToString();
            AciklamaSatirimiz = Session["AciklamaSatirlarimiz"].ToString();
            MalKodu = Session["MalKodlari"].ToString();
            MalAdi = Session["MalAdlari"].ToString();
            Miktar = Session["Miktarlar"].ToString();
            Kdv = Session["Kdvmiz"].ToString().Replace('.', ',');
            TalepID = Session["TalepIDler"].ToString();
            EvrakNoParcala = ihtiyacNo.Split(',');
            MalKodlariParcala = MalKodu.Split(',');
            MiktarParcala = Miktar.Split('/');
            KdvParcala = Kdv.Split('/');
            TalepIDParcala = TalepID.Split(',');
            MalAdlariParcala = MalAdi.Split('}');
            AciklamaSatirParcala = AciklamaSatirimiz.Split('}');

            txtMalAdi = new TextBox[EvrakNoParcala.Length];
            txtMalKodu = new TextBox[EvrakNoParcala.Length];
            txtAciklama = new TextBox[EvrakNoParcala.Length];
            txtKdv = new TextBox[EvrakNoParcala.Length];
            txtFiyat = new TextBox[EvrakNoParcala.Length];
            txtMiktar = new TextBox[EvrakNoParcala.Length];
            txtEvrakNo = new TextBox[EvrakNoParcala.Length];
            txtTalepID = new TextBox[EvrakNoParcala.Length];
            drpDepoKodlari = new DropDownList[EvrakNoParcala.Length];

            Literal Lt = new Literal();
            Lt.Text = "</br><center><fieldset>" +
                      "<table width=\"950\" border=\"0\">" +
                      "<tr>" +
                      "<td width=\"150\" align=\"center\" scope=\"row\">TalepID</td>" +
                      "<td width=\"150\" align=\"center\">Mal Kodu</td>" +
                      "<td width=\"150\" align=\"center\">Malzeme Adı</td>" +
                      "<td width=\"150\" align=\"center\">Açıklama</td>" +
                      "<td width=\"150\" align=\"center\">Kdv</td>" +
                      "<td width=\"150\" align=\"center\">Fiyat</td>" +
                      "<td width=\"150\" align=\"center\">Miktar</td>" +
                      "<td width=\"150\" align=\"center\">İhtiyaç Numarası</td>" +
                      "<td width=\"150\" align=\"center\">Depo Seçiniz</td>" +
                      "</<tr>";
            Panel1.Controls.Add(Lt);

            for (int i = 0; i < EvrakNoParcala.Length; i++)
            {
                string SipEvrakNo = EvrakNoParcala[i].ToString();
                string SipMalKodu = MalKodlariParcala[i].ToString();

                SiparisDurum = SiparisDurumu(SipEvrakNo, SipMalKodu);

                if (SiparisDurum == false)
                {
                    SiparisKontrol++;

                    Literal Lt14 = new Literal();
                    Lt14.Text = "<tr><td scope=\"row\" align=\"center\">";
                    Panel1.Controls.Add(Lt14);
                    txtTalepID[i] = new TextBox();
                    txtTalepID[i].ID = "txtTalepID" + i.ToString();
                    txtTalepID[i].Text = TalepIDParcala[i].ToString();
                    txtTalepID[i].Font.Name = "Calibri";
                    txtTalepID[i].Font.Size = 10;
                    txtTalepID[i].Width = 50;
                    txtTalepID[i].Height = 15;
                    Panel1.Controls.Add(txtTalepID[i]);
                    Literal Lt15 = new Literal();
                    Lt15.Text = "</td>";
                    Panel1.Controls.Add(Lt15);

                    Literal Lt1 = new Literal();
                    Lt1.Text = "<td scope=\"row\" align=\"center\">";
                    Panel1.Controls.Add(Lt1);
                    txtMalKodu[i] = new TextBox();
                    txtMalKodu[i].ID = "txtMalKodu" + i.ToString();
                    txtMalKodu[i].Text = MalKodlariParcala[i].ToString();
                    txtMalKodu[i].Font.Name = "Calibri";
                    txtMalKodu[i].Font.Size = 10;
                    txtMalKodu[i].Width = 125;
                    txtMalKodu[i].Height = 15;
                    Panel1.Controls.Add(txtMalKodu[i]);
                    Literal Lt2 = new Literal();
                    Lt2.Text = "</td>";
                    Panel1.Controls.Add(Lt2);

                    Literal Lt16 = new Literal();
                    Lt16.Text = "<td scope=\"row\" align=\"center\">";
                    Panel1.Controls.Add(Lt16);
                    txtMalAdi[i] = new TextBox();
                    txtMalAdi[i].ID = "txtMalAdi" + i.ToString();
                    txtMalAdi[i].Text = MalAdlariParcala[i].ToString();
                    txtMalAdi[i].Font.Name = "Calibri";
                    txtMalAdi[i].Font.Size = 10;
                    txtMalAdi[i].Width = 125;
                    txtMalAdi[i].Height = 15;
                    Panel1.Controls.Add(txtMalAdi[i]);
                    Literal Lt17 = new Literal();
                    Lt17.Text = "</td>";
                    Panel1.Controls.Add(Lt17);

                    Literal Lt3 = new Literal();
                    Lt3.Text = "<td scope=\"row\" align=\"center\">";
                    Panel1.Controls.Add(Lt3);
                    txtAciklama[i] = new TextBox();
                    txtAciklama[i].ID = "txtAciklama" + i.ToString();
                    txtAciklama[i].Text = AciklamaSatirParcala[i].ToString();
                    txtAciklama[i].Font.Name = "Calibri";
                    txtAciklama[i].Font.Size = 10;
                    txtAciklama[i].Width = 125;
                    txtAciklama[i].Height = 15;
                    txtAciklama[i].MaxLength = 20;
                    Panel1.Controls.Add(txtAciklama[i]);
                    Literal Lt4 = new Literal();
                    Lt4.Text = "</td>";
                    Panel1.Controls.Add(Lt4);

                    Literal Lt5 = new Literal();
                    Lt5.Text = "<td scope=\"row\" align=\"center\">";
                    Panel1.Controls.Add(Lt5);
                    txtKdv[i] = new TextBox();
                    txtKdv[i].ID = "txtKdv" + i.ToString();
                    txtKdv[i].Text = KdvParcala[i].ToString();
                    txtKdv[i].Font.Name = "Calibri";
                    txtKdv[i].Font.Size = 10;
                    txtKdv[i].Width = 50;
                    txtKdv[i].Height = 15;
                    Panel1.Controls.Add(txtKdv[i]);
                    Literal Lt6 = new Literal();
                    Lt6.Text = "</td>";
                    Panel1.Controls.Add(Lt6);

                    Literal Lt7 = new Literal();
                    Lt7.Text = "<td scope=\"row\" align=\"center\">";
                    Panel1.Controls.Add(Lt7);
                    txtFiyat[i] = new TextBox();
                    txtFiyat[i].ID = "txtFiyat" + i.ToString();
                    txtFiyat[i].Font.Name = "Calibri";
                    txtFiyat[i].Font.Size = 10;
                    txtFiyat[i].Width = 70;
                    txtFiyat[i].Height = 15;
                    Panel1.Controls.Add(txtFiyat[i]);
                    Literal Lt8 = new Literal();
                    Lt8.Text = "</td>";
                    Panel1.Controls.Add(Lt8);

                    Literal Lt9 = new Literal();
                    Lt9.Text = "<td scope=\"row\" align=\"center\">";
                    Panel1.Controls.Add(Lt9);
                    txtMiktar[i] = new TextBox();
                    txtMiktar[i].ID = "txtMiktar" + i.ToString();
                    txtMiktar[i].Text = MiktarParcala[i].ToString().Replace('.', ',');
                    txtMiktar[i].Font.Name = "Calibri";
                    txtMiktar[i].Font.Size = 10;
                    txtMiktar[i].Width = 70;
                    txtMiktar[i].Height = 15;
                    Panel1.Controls.Add(txtMiktar[i]);
                    Literal Lt10 = new Literal();
                    Lt10.Text = "</td>";
                    Panel1.Controls.Add(Lt10);

                    Literal Lt11 = new Literal();
                    Lt11.Text = "<td scope=\"row\" align=\"center\">";
                    Panel1.Controls.Add(Lt11);
                    txtEvrakNo[i] = new TextBox();
                    txtEvrakNo[i].ID = "txtEvrakNo" + i.ToString();
                    txtEvrakNo[i].Text = EvrakNoParcala[i].ToString();
                    txtEvrakNo[i].Font.Name = "Calibri";
                    txtEvrakNo[i].Font.Size = 10;
                    txtEvrakNo[i].Width = 100;
                    txtEvrakNo[i].Height = 15;
                    Panel1.Controls.Add(txtEvrakNo[i]);
                    Literal Lt12 = new Literal();
                    Lt12.Text = "</td>";
                    Panel1.Controls.Add(Lt12);

                    Literal LtDepo = new Literal();
                    LtDepo.Text = "<td scope=\"row\" align=\"center\">";
                    Panel1.Controls.Add(LtDepo);
                    
                    drpDepoKodlari[i] = new DropDownList();
                    drpDepoKodlari[i].ID = "drpDepoKodlari" + i.ToString();
                    drpDepoKodlari[i].Font.Name = "Calibri";
                    drpDepoKodlari[i].Width = 170;
                    drpDepoKodlari[i].AutoPostBack = true;
                    drpDepoKodlari[i].DataSourceID = "sqlDepoKodlari";
                    drpDepoKodlari[i].DataTextField = "DepoAdi";
                    drpDepoKodlari[i].DataValueField = "DepoKodu";
                    Panel1.Controls.Add(drpDepoKodlari[i]);

                    Literal LtDepoKapat = new Literal();
                    LtDepoKapat.Text = "</td>";
                    Panel1.Controls.Add(LtDepoKapat);
                }
            }
            Literal Lt13 = new Literal();
            Lt13.Text = "</tr></table></fieldset></center>";
            Panel1.Controls.Add(Lt13);

            if (SiparisKontrol == 0)
            {
                Alert.Show("Açık Sipariş Bulunamadı.");
                ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
            }
        }
        else
        {
            Alert.Show("Sipariş Açılacak Kayıtları Seçmediniz.Lütfen Kontrol Edip Tekrar Deneyin.");
            ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
        }
    }

    private void Stk002Ekle(string MalKodu, int Tarih, string HesapKodu, string EvrakSeriNo, decimal Miktar, decimal Fiyat,
    decimal Tutar, decimal KdvTutari, int SEQNO, string Saat, string GirenKodu, string ihtiyacNumarasi, string MasrafMerkezi, string Aciklama,
    decimal DovizKuru, string DovizCinsi, decimal DovizTutari, int ParaBirimi, string DepoKodu,string KdvOran)
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        if (Aciklama.ToString().Length > 25)
        {
            Aciklama = Aciklama.ToString().Substring(0, 25);
        }

        KdvOranimiz = KdvOranFlag(KdvOran);

        string insertStk = "INSERT INTO STK002(STK002_MalKodu, STK002_IslemTarihi, STK002_GC, STK002_CariHesapKodu, STK002_EvrakSeriNo, STK002_Miktari, STK002_BirimFiyati, " +
                      "STK002_Tutari, STK002_Iskonto, STK002_KDVTutari, STK002_IslemTipi, STK002_Kod1, STK002_Kod2, STK002_IrsaliyeNo, STK002_TeslimMiktari, " +
                      "STK002_SipDurumu, STK002_Bos, STK002_KDVDurumu, STK002_TeslimTarihi, STK002_ParaBirimi, STK002_SEQNo, STK002_GirenKaynak, STK002_GirenTarih,  " +
                      "STK002_GirenSaat, STK002_GirenKodu, STK002_GirenSurum, STK002_DegistirenKaynak, STK002_DegistirenTarih, STK002_DegistirenSaat, STK002_DegistirenKodu,  " +
                      "STK002_DegistirenSurum, STK002_IptalDurumu, STK002_AsilEvrakTarihi, STK002_Miktar2, STK002_Tutar2, STK002_KalemIskontoOrani1, " +
                      "STK002_KalemIskontoOrani2, STK002_KalemIskontoOrani3, STK002_KalemIskontoOrani4, STK002_KalemIskontoOrani5, STK002_KalemIskontoTutari1,  " +
                      "STK002_KalemIskontoTutari2, STK002_KalemIskontoTutari3, STK002_KalemIskontoTutari4, STK002_KalemIskontoTutari5, STK002_DovizCinsi, STK002_DovizTutari,  " +
                      "STK002_DovizKuru, STK002_Aciklama1, STK002_Aciklama2, STK002_Depo, STK002_Kod3, STK002_Kod4, STK002_Kod5, STK002_Kod6, STK002_Kod7,  " +
                      "STK002_Kod8, STK002_Kod9, STK002_Kod10, STK002_Kod11, STK002_Kod12, STK002_Vasita, STK002_MalSeriNo, STK002_VadeTarihi, STK002_Masraf, " +
                      "STK002_EvrakSeriNo2, STK002_Kod9Flag, STK002_Kod10Flag, STK002_KDVOranFlag, STK002_TeslimCHKodu, STK002_MasrafMerkezi, " +
                      "STK002_EFaturaOTVListeNo) " +
                      "VALUES ('" + MalKodu.ToString() + "'," + Tarih + ", 0,'" + HesapKodu.ToString() + "','" + EvrakSeriNo.ToString() + "', " +
                      "'" + Miktar + "','" + Fiyat + "', " +
                      "'" + Tutar + "', '0.00','" + KdvTutari + "', 2, N'', N'', N'', '0.0000', " +
                      "0, N'', 0," + Tarih + ", " + ParaBirimi + "," + SEQNO + ", N'Y6016'," + Tarih + ",'" + Saat + "','" + GirenKodu + "', " +
                      "N'6.1.10', N'Y6016'," + Tarih + ", '" + Saat + "','" + GirenKodu + "', N'6.1.10', 1," + Tarih + ", '0.000',  " +
                      "'0.000', '0.00', '0.00', '0.00', '0.00', '0.00', '0.00', '0.00', '0.00', '0.00', '0.00', '" + DovizCinsi.ToString() + "', '" + DovizTutari + "', " +
                      "'" + DovizKuru.ToString() + "', '" + Aciklama.ToString().Replace("'", "''") + "', N'', '" + DepoKodu.ToString() + "', N'', N'', N'', N'', N'', N'', N'', N'', '0.00', '0.00', N'',  " +
                      "'" + ihtiyacNumarasi.ToString() + "'," + Tarih + ", '0.00', '" + ihtiyacNumarasi.ToString() + "', 0, 0, " + KdvOranimiz + ",'" + HesapKodu.ToString() + "','" + MasrafMerkezi.ToString() + "', N'')";

        cmd = new SqlCommand(insertStk, DbConnLink);
        cmd.CommandTimeout = 120;
        cmd.ExecuteNonQuery();
    }

    private void Stk004Guncelle(int DegistirenTarih, string DegistirenSaat, string DegistirenKodu, decimal AlisSiparisi, string MalKodu)
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        string STK004Update = "UPDATE  STK004 SET " +
                            "STK004_DegistirenTarih=" + DegistirenTarih + ", " +
                            "STK004_DegistirenSaat='" + DegistirenSaat.ToString() + "', " +
                            "STK004_DegistirenKodu = '" + DegistirenKodu.ToString() + "', " +
                            "STK004_AlisSiparisi=STK004_AlisSiparisi+'" + AlisSiparisi.ToString().Replace(',', '.') + "' " +
                            "WHERE STK004_MalKodu='" + MalKodu.ToString() + "'";

        cmd = new SqlCommand(STK004Update, DbConnLink);
        cmd.CommandTimeout = 120;
        cmd.ExecuteNonQuery();
    }

    private int KdvOranFlag(string KdvOrani)
    {
        if(DbConnLink.State==ConnectionState.Closed)
            DbConnLink.Open();

        string sorgu = "SELECT (CASE '" + KdvOrani + "' " +
                        "WHEN 0 THEN 1  " +
                        "WHEN 1 THEN 2 " +
                        "WHEN 8 THEN 3 " +
                        "WHEN 18 THEN 4 END) AS KDV";
        cmd = new SqlCommand(sorgu, DbConnLink);

        return (int)Convert.ToInt32(cmd.ExecuteScalar());
    }

    private string MasrafMerkeziBul(string EvrakNo, string MalKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT KullanilacakDepartman FROM TLP " +
                    "WHERE EvrakNoTarih+EvrakNo='" + EvrakNo.ToString() + "' AND MalKodu='" + MalKodu.ToString() + "'";

        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();
    }

    private int SeqNo()
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        string sorgu = "SELECT " +
                    "ISNULL((CASE " +
                    "WHEN MAX(STK002_SEQNo) < 2000000 THEN 2000000 ELSE MAX(STK002_SEQNo)+1 " +
                    "END),2000000) AS SEQ  " +
                    "FROM STK002 " +
                    "WHERE STK002_SEQNo<20000000";

        cmd = new SqlCommand(sorgu, DbConnLink);
        cmd.CommandTimeout = 120;
        return (int)cmd.ExecuteScalar();
    }

    private string SiparisNoSeri()
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT SiparisNoSeri FROM EvrakNumaralari ";

        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (string)cmd.ExecuteScalar();
    }

    private int SiparisNo()
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT MAX(SiparisNo)+1 AS FisNo FROM EvrakNumaralari  ";

        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (int)cmd.ExecuteScalar();
    }

    private void SiparisNoGuncelle()
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "UPDATE EvrakNumaralari SET " +
                     "SiparisNo=SiparisNo+1";
        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        cmd.ExecuteNonQuery();
    }

    private void CariEkle()
    {
        int kayitSayisi = 2;

        for (int j = 0; j < EvrakNoParcala.Length; j++)
        {
            if (!string.IsNullOrEmpty(txtAciklama[j].Text))
            {
                string SiparisSeriNo = SiparisNoSeri();
                int SipNo = SiparisNo();
                string SiparisNums = Convert.ToString(SipNo);
                SiparisNums = SiparisNums.PadLeft(6, '0');
                string SiparisNumarasi = SiparisSeriNo + SiparisNums;
                string HesapKodu = drpCariAdi.SelectedItem.Value.ToString();

                AciklamalariAliyoruz(EvrakNoParcala[j].ToString());

                for (int i = 0; i < 2; i++)
                {
                    if (i == 0)
                    {
                        if (!string.IsNullOrEmpty(Aciklama2))
                        {
                            Car005AciklamaEkle(TarihRakam, SiparisNumarasi, "A", kayitSayisi, Aciklama2.ToString().Replace("'", "''"), "", "", HesapKodu, 0, 0, 0);
                            kayitSayisi = kayitSayisi + 1;
                        }
                    }
                    else if (i == 1)
                    {
                        if (!string.IsNullOrEmpty(Aciklama3))
                        {
                            Car005AciklamaEkle(TarihRakam, SiparisNumarasi, "A", kayitSayisi, Aciklama3.ToString().Replace("'", "''"), "", "", HesapKodu, 0, 0, 0);
                            kayitSayisi = kayitSayisi + 1;
                        }
                    }
                }
            }
        }

        string DovizCinsi = drpDovizCinsi.SelectedItem.Value.ToString();
        string IslemTipi = drpParaBirimi.SelectedItem.Value.ToString();
        string DovizKuru = txtDovizKuru.Text;
        decimal DovizToplami = 0;
        decimal DovizMalBedeli = 0;
        decimal DovizKdv = 0;
        decimal KfKdv = 0;

        if (DovizCinsi == "Seçim Yapınız")
        {
            DovizCinsi = "";
        }

        kayitSayisi = kayitSayisi - 1;

        for (int c = 0; c < EvrakNoParcala.Length; c++)
        {
            string KKdv = txtKdv[c].Text.Replace(',', '.');
            string MMiktar = txtMiktar[c].Text.Replace(',', '.');
            string FFiyat = txtFiyat[c].Text.Replace(',', '.');
            KfKdv = Convert.ToDecimal(txtKdv[c].Text.Replace(',', '.'));

            decimal Kdv = Convert.ToDecimal(txtKdv[c].Text.Replace(',', '.'));
            decimal Miktar = Convert.ToDecimal(MMiktar.ToString());
            decimal Fiyat = Convert.ToDecimal(FFiyat.ToString());
            decimal KdvTutari = ((Math.Round(Miktar, 4) * Math.Round(Fiyat, 6)) * Kdv) / 100;
            decimal Tutar = (Math.Round(Miktar, 4) * Math.Round(Fiyat, 6));
            MalBedeli += Tutar;
            GenelKdv += KdvTutari;
        }

        GenelToplam = MalBedeli + GenelKdv;

        if (!string.IsNullOrEmpty(DovizKuru))
        {
            if (DovizKuru != "0")
            {
                DovizKuru = DovizKuru.ToString().Replace(',', '.');
                DovizToplami = GenelToplam / Convert.ToDecimal(DovizKuru.ToString());
                DovizToplami = Math.Round(DovizToplami, 2);

                DovizMalBedeli = MalBedeli / Convert.ToDecimal(DovizKuru.ToString());
                DovizMalBedeli = Math.Round(DovizMalBedeli, 2);

                DovizKdv = GenelKdv / Convert.ToDecimal(DovizKuru.ToString());
                DovizKdv = Math.Round(DovizKdv, 2);
            }
        }
        else
        {
            DovizKuru = "0";
        }

        for (int i = 0; i < 6; i++)
        {
            string HesapKodu = drpCariAdi.SelectedItem.Value.ToString();

            string SiparisSeriNo = SiparisNoSeri();
            int SipNo = SiparisNo();
            string SiparisNums = Convert.ToString(SipNo);
            SiparisNums = SiparisNums.PadLeft(6, '0');
            string SiparisNumarasi = SiparisSeriNo + SiparisNums;

            PcAdi = System.Net.Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]).HostName.ToString();

            if (PcAdi.ToString().Length > 10)
            {
                PcAdi = PcAdi.ToString().Substring(0, 10);
            }

            string GirenKodu = Session["KullAdi"].ToString();
            if (GirenKodu.ToString().Length > 10)
            {
                GirenKodu = GirenKodu.ToString().Substring(0, 10);
            }

            DateTime obj = new DateTime();
            obj = Convert.ToDateTime(dtSiparisTarihi.Date);
            string str;
            Tarih = 0;
            Tarih = obj.ToOADate();
            str = Tarih.ToString().Substring(0, 5);
            TarihRakam = Convert.ToInt32(str);

            //DateTime DtSaat = new DateTime();
            //DtSaat = Convert.ToDateTime(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
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

            if (i == 0)
            {

                Car005Ekle(TarihRakam, SiparisNumarasi, "Z", kayitSayisi, "", "", "", HesapKodu, 0, 0, 0, "", 0, 0);
            }
            else if (i == 1)
            {
                Car005Ekle(TarihRakam, SiparisNumarasi, "T", kayitSayisi, "", "", "MAL BEDELİ", HesapKodu, MalBedeli, 0,
                    Convert.ToInt32(IslemTipi), DovizCinsi, Convert.ToDecimal(DovizKuru), DovizMalBedeli);
            }
            else if (i == 2)
            {
                Car005Ekle(TarihRakam, SiparisNumarasi, "K", kayitSayisi, "", "1", "KATMA DEĞER VERGİSİ", HesapKodu, GenelKdv, KfKdv,
                    Convert.ToInt32(IslemTipi), DovizCinsi, Convert.ToDecimal(DovizKuru), DovizKdv);
            }
            else if (i == 3)
            {
                Car005Ekle(TarihRakam, SiparisNumarasi, "Z", kayitSayisi, "", "1", "", HesapKodu, 0, 0, 0, "", 0, 0);
            }
            else if (i == 4)
            {
                Car005Ekle(TarihRakam, SiparisNumarasi, "G", kayitSayisi, "", "1", "GENEL TOPLAM", HesapKodu, GenelToplam, 0,
                    Convert.ToInt32(IslemTipi), DovizCinsi, Convert.ToDecimal(DovizKuru.ToString()), DovizToplami);
            }
            else if (i == 5)
            {
                Car005Ekle(TarihRakam, SiparisNumarasi, "Y", kayitSayisi, "", "1", "", HesapKodu, 0, 0, 0, "", 0, 0);
            }

            kayitSayisi++;
        }
    }

    private void SiparisKartlariGuncelle(string EvrakNo,string SiparisNumarasi,decimal SiparisMiktari,string CariUnvani,decimal BirimFiyati)
    {
        if (DbConnSiparisKarti.State == ConnectionState.Closed)
            DbConnSiparisKarti.Open();

        string Sorgu = "UPDATE Tlp SET   " +
                        "SiparisNo='" + SiparisNumarasi + "', " +
                        "SipMiktar='" + SiparisMiktari.ToString().Replace(',', '.') + "', " +
                        "CariUnvan='" + CariUnvani + "', " +
                        "BirimFiyati='" + BirimFiyati.ToString().Replace(',', '.') + "' " +
                        "WHERE EvrakNoTarih+EvrakNo=" + EvrakNo;

        cmdSiparisKarti = new SqlCommand(Sorgu, DbConnSiparisKarti);
        cmdSiparisKarti.ExecuteNonQuery();
    }

    private void Car005Ekle(int Tarih, string FaturaNo, string SatirTipi, int SatirNo, string SatirKodu, string Filler,
        string Aciklama, string CariKodu, decimal Tutar, decimal Oran, int ParaBirimi, string DovizCinsi, decimal DovizKuru, decimal DovizTutari)
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        string insertCar005 = "INSERT INTO CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                      "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran, CAR005_TevkOrani, CAR005_TevkKDVOrani, " +
                      "CAR005_EkBilgi1, CAR005_EFaturaOTVListeNo, CAR005_EFaturaTipi, CAR005_EFaturaDurumu, CAR005_EFaturaDonemAciklama, CAR005_EFaturaNot, " +
                      "CAR005_EFaturaReferansNo, CAR005_ParaBirimi, CAR005_DovizCinsi, CAR005_DovizKuru, CAR005_DovizTutari, CAR005_TeslimCHKodu) " +
                      "VALUES (6," + Tarih + ",'" + FaturaNo.ToString() + "', N'A', 4,'" + SatirTipi.ToString() + "'," + SatirNo + ", " +
                      "'" + SatirKodu.ToString() + "','" + Filler.ToString() + "','" + Aciklama.ToString().Replace("'", "''") + "','" + CariKodu.ToString() + "','" + Tutar.ToString() + "', " +
                      "'" + Oran.ToString() + "', N'', N'0.00', N'', N'', 0, 0, N'', N'', N''," + ParaBirimi + ", " +
                      "'" + DovizCinsi.ToString() + "', '" + DovizKuru.ToString() + "', '" + DovizTutari.ToString() + "', " +
                      "'" + CariKodu.ToString() + "')";
        cmd = new SqlCommand(insertCar005, DbConnLink);
        cmd.CommandTimeout = 120;
        cmd.ExecuteNonQuery();
    }

    private void Car005AciklamaEkle(int Tarih, string FaturaNo, string SatirTipi, int SatirNo, string SatirKodu, string Filler,
    string Aciklama, string CariKodu, decimal Tutar, decimal Oran, int ParaBirimi)
    {
        if (DbConnLink.State == ConnectionState.Closed)
            DbConnLink.Open();

        if (SatirKodu.ToString().Length > 20)
        {
            SatirKodu = SatirKodu.ToString().Substring(0, 20);
        }

        string insertCar005 = "INSERT INTO CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                      "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran, CAR005_TevkOrani, CAR005_TevkKDVOrani, " +
                      "CAR005_EkBilgi1, CAR005_EFaturaOTVListeNo, CAR005_EFaturaTipi, CAR005_EFaturaDurumu, CAR005_EFaturaDonemAciklama, CAR005_EFaturaNot, " +
                      "CAR005_EFaturaReferansNo, CAR005_ParaBirimi, CAR005_DovizCinsi, CAR005_DovizKuru, CAR005_DovizTutari, CAR005_TeslimCHKodu) " +
                      "VALUES (6," + Tarih + ",'" + FaturaNo.ToString() + "', N'A', 4,'" + SatirTipi.ToString() + "'," + SatirNo + ", " +
                      "'" + SatirKodu.ToString() + "','" + Filler.ToString() + "','" + Aciklama.ToString().Replace("'", "''") + "','" + CariKodu.ToString() + "','" + Tutar.ToString().Replace(',', '.') + "', " +
                      "'" + Oran.ToString().Replace(',', '.') + "', N'', N'0.00', N'', N'', 0, 0, N'', N'', N''," + ParaBirimi + ", " +
                      "N'', '0.000000', '0.00',N'')";
        cmd = new SqlCommand(insertCar005, DbConnLink);
        cmd.CommandTimeout = 120;
        cmd.ExecuteNonQuery();
    }

    private void ParcaliTalepSiparisDurumGuncelle()
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        for (int i = 0; i < EvrakNoParcala.Length; i++)
        {
            TalepID = txtTalepID[i].Text;
            Miktar = txtMiktar[i].Text.Replace(',', '.');
            string StokluDepartmanKodu = "";
            string DepartmanKodu = Session["DepartmanKodu"].ToString();
            string KullaniciKodu = Session["KullaniciKodu"].ToString();
            string TalepAkisDepartmanKodu = DepartmanKodu;
            int YetkiKodu = Convert.ToInt32(Session["Yetki"].ToString());

            YetkiKoduBul(DepartmanKodu, KullaniciKodu.ToString(), Convert.ToString(YetkiKodu));

            if (Convert.ToInt32(KayitYetki) == 0)
            {
                TalepAkisDepartmanKodu = StokluDepartmanKodu.ToString();
                YetkiTamamla(DepartmanKodu);
            }


            string SiparisSeriNo = SiparisNoSeri();
            int SipNo = SiparisNo();
            string SiparisNums = Convert.ToString(SipNo);
            SiparisNums = SiparisNums.PadLeft(6, '0');
            string SiparisNumarasi = SiparisSeriNo + SiparisNums;
            string GitDepartman = "";
            GitDepartman = AnaDepartmanKodu();

            string GuncellemeSorgusu = "UPDATE Tlp SET " +
                //"GidecekDepartman='" + GitDepartman.ToString() + "', " +                         
                                         "OnayDurumu = '7', " +
                                         "Onaylayan = '" + KullaniciKodu.ToString() + "', " +
                                         "OnayMiktar = OnayMiktar+" + Convert.ToDecimal(Miktar.ToString()) + ", " +
                                         "SiparisNo = '" + SiparisNumarasi.ToString() + "', " +
                                         //"SipMiktar = " + Convert.ToDecimal(Miktar.ToString()) + ", " +
                                         "SiparisDurum = 2, " +
                                         "ilkIslem = 1 " +
                                         "WHERE TalepID = " + TalepID + "";

            cmd = new SqlCommand(GuncellemeSorgusu, DbConnUser);
            cmd.CommandTimeout = 120;
            cmd.ExecuteNonQuery();

            //string TalepAkisDuzenle = "UPDATE TalepAkisi SET Onay='True' WHERE TalepID=" + Convert.ToInt32(TalepID) + "";
            //cmd = new SqlCommand(TalepAkisDuzenle, DbConnUser);
            //cmd.CommandTimeout = 120;
            //cmd.ExecuteNonQuery();

            //string TalepEkleSorgu = "INSERT INTO TalepAkisi(TalepID, OnaylayanYetki, EkleyenYetki, TalepDurumu, Departman, Onay, OnayTarih) " +
            //              "VALUES (" + TalepID + "," + Convert.ToInt32(KayitYetki) + "," + Convert.ToInt32(YetkiKodu) + "," +
            //              "'" + TalepAkisDepartmanKodu.ToString() + "','" + DepartmanKodu.ToString() + "', 'False', GETDATE())";
            //cmd = new SqlCommand(TalepEkleSorgu, DbConnUser);
            //cmd.CommandTimeout = 120;
            //cmd.ExecuteNonQuery();
        }
    }

    private void YetkiKoduBul(string DepartmanKodu, string KullaniciAdi, string YetkiKodumuz)
    {
        if (DbConnYetkiKoduBul.State == ConnectionState.Closed)
            DbConnYetkiKoduBul.Open();

        string sorgu = "SELECT ISNULL(MIN(Yetki),0) AS Yetki FROM Kullanicilar AS K " +
                        "INNER JOIN Departmanlar AS D ON K.DepartmanID=D.DepartmanID " +
                        "WHERE D.DepartmanKodu='" + DepartmanKodu.ToString() + "' " +
                        "AND KullaniciAdi <> '" + KullaniciAdi.ToString() + "' " +
                        "AND Yetki > " + Convert.ToInt32(YetkiKodumuz) + "";

        cmd = new SqlCommand(sorgu, DbConnYetkiKoduBul);
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
        if (DbConnYetkiTamamla.State == ConnectionState.Closed)
            DbConnYetkiTamamla.Open();

        string sorgu = "SELECT ISNULL(MIN(Yetki),0) AS Yetki FROM Kullanicilar AS K " +
                    "INNER JOIN Departmanlar AS D ON K.DepartmanID=D.DepartmanID " +
                    "WHERE D.DepartmanKodu ='" + Departman.ToString() + "'";

        cmd = new SqlCommand(sorgu, DbConnYetkiTamamla);
        cmd.CommandTimeout = 120;
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        if (dr.Read())
        {
            KayitYetki = dr["Yetki"].ToString();
        }

        dr.Dispose();
        dr.Close();
    }

    private decimal SiparisMiktariBul(string EvrakNo, string MalKodu)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT Miktar FROM Tlp " +
                       "WHERE EvrakNoTarih+EvrakNo='" + EvrakNo.ToString() + "' AND MalKodu='" + MalKodu.ToString() + "'";
        cmd = new SqlCommand(sorgu, DbConnUser);
        cmd.CommandTimeout = 120;
        return (decimal)cmd.ExecuteScalar();
    }

    private void ParcaliTalepSiparisDurumilkGuncelle()
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        for (int i = 0; i < EvrakNoParcala.Length; i++)
        {
            TalepID = txtTalepID[i].Text;
            Miktar = txtMiktar[i].Text;
            string StokluDepartmanKodu = "";
            string DepartmanKodu = Session["DepartmanKodu"].ToString();
            string KullaniciKodu = Session["KullaniciKodu"].ToString();
            string TalepAkisDepartmanKodu = DepartmanKodu;
            int YetkiKodu = Convert.ToInt32(Session["Yetki"].ToString());

            YetkiKoduBul(DepartmanKodu, KullaniciKodu.ToString(), Convert.ToString(YetkiKodu));

            if (Convert.ToInt32(KayitYetki) == 0)
            {
                TalepAkisDepartmanKodu = StokluDepartmanKodu.ToString();
                YetkiTamamla(DepartmanKodu);
            }


            string SiparisSeriNo = SiparisNoSeri();
            int SipNo = SiparisNo();
            string SiparisNums = Convert.ToString(SipNo);
            SiparisNums = SiparisNums.PadLeft(6, '0');
            string SiparisNumarasi = SiparisSeriNo + SiparisNums;

            string GuncellemeSorgusu = "UPDATE Tlp SET " +
                                         "OnayDurumu='Sipariş Açılmış', " +
                                         "Onaylayan='" + KullaniciKodu.ToString() + "', " +
                                         "OnayMiktar=" + Miktar.ToString().Replace(",", ".") + ", " +
                                         "SiparisNo='" + SiparisNumarasi.ToString() + "', " +
                                         "SipMiktar=" + Miktar.ToString().Replace(",", ".") + ", " +
                                         "SiparisDurum=1 " +
                                         "WHERE TalepID=" + TalepID + "";

            cmd = new SqlCommand(GuncellemeSorgusu, DbConnUser);
            cmd.CommandTimeout = 120;
            cmd.ExecuteNonQuery();

        }
    }

    private void AciklamalariAliyoruz(string EvrakNo)
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT Aciklama,Aciklama2,Aciklama3 FROM Tlp " +
                        "WHERE EvrakNoTarih+EvrakNo='" + EvrakNo.ToString() + "'";
        cmd = new SqlCommand(sorgu, DbConnUser);
        drAciklama = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        if (drAciklama.Read())
        {
            Aciklama1 = drAciklama["Aciklama"].ToString();
            Aciklama2 = drAciklama["Aciklama2"].ToString();
            Aciklama3 = drAciklama["Aciklama3"].ToString();
        }

        drAciklama.Dispose();
        drAciklama.Close();
    }

    private void BaglantilariKapat()
    {
        //DbConnUser.Dispose();
        DbConnUser.Close();

        //DbConnYetkiKoduBul.Dispose();
        DbConnYetkiKoduBul.Close();

        //DbConnLink.Dispose();
        DbConnLink.Close();

        //DbConnYetkiTamamla.Dispose();
        DbConnYetkiTamamla.Close();

        //cmd.Dispose();

        //dr.Dispose();
        dr.Close();
    }

    public static DataTable datadoldur(string Sql)
    {
        SqlConnection conn = new SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DbConnLink"].ToString());

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

    public static DataTable datadoldurDoviz(string Sql)
    {
        SqlConnection conn = new SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DbConnLinkDoviz"].ToString());

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

    protected void btnCariKodAra_Click(object sender, EventArgs e)
    {
        string CariHesapAdi = txtCariHesapAdi.Text.ToString();

        string sorgu = "SELECT CAR002_Unvan1 + ' ' + CAR002_Unvan2 AS Unvan, " +
                        "CAR002_HesapKodu AS HesapKodu FROM CAR002 " +
                        "WHERE CAR002_Unvan1+CAR002_Unvan2 LIKE '%" + CariHesapAdi.ToString() + "%' AND LEFT(CAR002_HesapKodu,3)='320'";

        drpCariAdi.DataSource = datadoldur(sorgu);
        drpCariAdi.DataBind();

        txtHesapKodu.Text = drpCariAdi.SelectedItem.Value.ToString();
    }

    private string AnaDepartmanKodu()
    {
        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string sorgu = "SELECT AnaDepartmanKodu AS GidecekDepartman FROM AnaDepartman";
        cmd = new SqlCommand(sorgu, DbConnUser);

        return (string)cmd.ExecuteScalar();
    }

    private bool SiparisDurumu(string EvrakNo, string MalKodu)
    {
        if (DbSiparisDurumu.State == ConnectionState.Closed)
            DbSiparisDurumu.Open();

        string Sorgu = "SELECT (CASE " +
                    "WHEN SiparisDurum=1 THEN 0 " +
                    "WHEN SiparisDurum=2 THEN 1 ELSE 0 END) AS SiparisDurum FROM Tlp " +
                    "WHERE EvrakNoTarih+EvrakNo='" + EvrakNo.ToString() + "' AND MalKodu='" + MalKodu.ToString() + "'";
        cmdSiparisDurumu = new SqlCommand(Sorgu, DbSiparisDurumu);
        cmdSiparisDurumu.CommandTimeout = 120;
        return (bool)Convert.ToBoolean(cmdSiparisDurumu.ExecuteScalar());
    }

    protected void btnDovizHesapla_Click(object sender, EventArgs e)
    {
        ihtiyacNo = Session["TalepEvrakNumaralari"].ToString();
        EvrakNoParcala = ihtiyacNo.Split(',');

        string ParaBirimimiz = drpParaBirimi.SelectedItem.Text.ToString();
        decimal BirimFiyat = 0;
        decimal BirimFiyat2 = 0;
        decimal DovKur = 0;
        decimal DovKur2 = 0;
        decimal YeniBirimFiyat = 0;
        string BFiyat = "0";
        string DDovKur = "0";

        if (ParaBirimimiz == "Doviz")
        {

            for (int i = 0; i < EvrakNoParcala.Length; i++)
            {
                BFiyat = txtFiyat[i].Text.Replace(',', '.');
                DDovKur = txtDovizKuru.Text.Replace(',', '.');

                BirimFiyat = Convert.ToDecimal(BFiyat);
                BirimFiyat2 = Math.Round(BirimFiyat, 6);
                DovKur = Convert.ToDecimal(DDovKur);
                DovKur2 = Math.Round(DovKur, 6);
                YeniBirimFiyat = Math.Round(BirimFiyat2, 6) * Math.Round(DovKur2, 6);
                txtFiyat[i].Text = YeniBirimFiyat.ToString().Replace('.', ',');
            }
        }
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

    protected void drpCariAdi_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtHesapKodu.Text = drpCariAdi.SelectedItem.Value.ToString();
    }

    private string DovizTuru()
    {
        if (DbConnDovizTuru.State == ConnectionState.Closed)
            DbConnDovizTuru.Open();

        string sorgu = "SELECT DovizReferansTuru FROM DovizTurleri";
        cmdDovizTuru = new SqlCommand(sorgu, DbConnDovizTuru);
        return (string)cmdDovizTuru.ExecuteScalar();
    }
}