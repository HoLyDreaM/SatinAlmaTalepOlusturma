using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Kontrol : System.Web.UI.Page
{
    SqlConnection DbConnUser = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnUser2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection Guncelle = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlConnection DbConnLink = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnLink"].ToString());
    SqlCommand cmd, cmd2, cmdguncelle;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (DbConnUser.State == ConnectionState.Closed)
                DbConnUser.Open();

            if (DbConnUser2.State == ConnectionState.Closed)
                DbConnUser2.Open();

            if (Guncelle.State == ConnectionState.Closed)
                Guncelle.Open();

            string sorgu = @"CREATE TABLE #TblSiparisler(
                         SiparisNo NVARCHAR(50),
                         Miktar	NUMERIC(18,3),
                         Unvan NVARCHAR(150),
                         BirimFiyat NUMERIC(18,3),
                         MalKodu NVARCHAR(50)
                         )

                         CREATE TABLE #LnkSiparisler(
                         SiparisNo NVARCHAR(50),
                         Miktar	NUMERIC(18,3),
                         Unvan NVARCHAR(150),
                         BirimFiyat NUMERIC(18,3),
                         MalKodu NVARCHAR(50)
                         )

                         INSERT INTO #TblSiparisler
                         SELECT EvrakNoTarih+EvrakNo AS SiparisNo,SipMiktar AS SiparisMiktar,CariUnvan AS Unvan,
                         BirimFiyati AS BirimFiyat,MalKodu FROM Tlp
                         WHERE SiparisNo<>''
                         GROUP BY EvrakNoTarih+EvrakNo,SipMiktar,CariUnvan,BirimFiyati,MalKodu

                         INSERT INTO #LnkSiparisler
                         SELECT s.STK002_EvrakSeriNo2  AS SiparisNo,s.STK002_Miktari  AS SiparisMiktar,
                         c.CAR002_Unvan1 +' '+ c.CAR002_Unvan2  AS Unvan,s.STK002_BirimFiyati AS BirimFiyat,STK002_MalKodu AS MalKodu
                         FROM YNSSFS14.YNSSFS14.STK002 AS s 
                         INNER JOIN YNSSFS14.YNSSFS14.CAR002 c ON s.STK002_CariHesapKodu = c.CAR002_HesapKodu
                         WHERE STK002_EvrakSeriNo2<>''
                         GROUP BY s.STK002_EvrakSeriNo2,s.STK002_Miktari,
                         c.CAR002_Unvan1 +' '+ c.CAR002_Unvan2,s.STK002_BirimFiyati,STK002_MalKodu

                         SELECT L.SiparisNo,L.Miktar,L.Unvan,L.BirimFiyat,L.MalKodu FROM #LnkSiparisler AS L
                         INNER JOIN #TblSiparisler AS T ON T.SiparisNo=L.SiparisNo
                         GROUP BY L.SiparisNo,L.Miktar,L.Unvan,L.BirimFiyat,L.MalKodu

                         DROP TABLE #LnkSiparisler
                         DROP TABLE #TblSiparisler";

            cmd = new SqlCommand(sorgu, DbConnUser);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            string sorgu2 = @"SELECT EvrakNoTarih+EvrakNo AS SiparisNo,SipMiktar AS SiparisMiktar,CariUnvan AS Unvan,
                         BirimFiyati AS BirimFiyat,MalKodu FROM Tlp
                         WHERE SiparisNo<>''
                         GROUP BY EvrakNoTarih+EvrakNo,SipMiktar,CariUnvan,BirimFiyati,MalKodu";

            cmd2 = new SqlCommand(sorgu2, DbConnUser2);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);

            foreach (DataRow dr in dt.Rows)
            {
                string EvrakNo = dr["SiparisNo"].ToString();
                string MalKodu = dr["MalKodu"].ToString();

                foreach (DataRow dr2 in dt2.Rows)
                {
                    string EvrakNo2 = dr2["SiparisNo"].ToString();
                    string MalKodu2 = dr2["MalKodu"].ToString();

                    if (EvrakNo == EvrakNo2 && MalKodu == MalKodu2)
                    {
                        string Miktarimiz = dr["Miktar"].ToString().Replace(',', '.');
                        string MalKodumuz = dr["MalKodu"].ToString();
                        string CariUnvanimiz = dr["Unvan"].ToString();
                        string BirimFiyati = dr["BirimFiyat"].ToString().Replace(',', '.');

                        string gnc = "UPDATE Tlp SET " +
                                     "CariUnvan='" + CariUnvanimiz + "', " +
                                     "BirimFiyati='" + BirimFiyati + "', " +
                                     "SipMiktar='" + Miktarimiz + "' " +
                                     "WHERE EvrakNoTarih+EvrakNo='" + EvrakNo2 + "' AND MalKodu='" + MalKodumuz + "'";

                        cmdguncelle = new SqlCommand(gnc, Guncelle);
                        cmdguncelle.ExecuteNonQuery();
                    }
                }
            }

            Alert.Show("Güncelleme İşlemi Başarılı");
        }
        catch (Exception ex)
        {

            Alert.Show("Bilinmeyen Bir Hata Oluştu. " + ex.Message);
        }
    }
}