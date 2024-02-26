using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class KullaniciSil : System.Web.UI.Page
{

    #region Bağlantı Ayarları

    SqlConnection DbConnUser = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlCommand cmd;
    SqlDataReader dr;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["Giris"] != "Evet")
        //{
        //    Alert.Show("Lütfen Giriş Yaptıktan Sonra Tekrar Deneyin.");
        //    Response.Redirect("Login.Aspx");
        //}

        if (DbConnUser.State == ConnectionState.Closed)
            DbConnUser.Open();

        string ID = Request.QueryString["ID"].ToString();
        string MyReferrer = Request.UrlReferrer.ToString();

        if (!string.IsNullOrEmpty(ID))
        {
            cmd = new SqlCommand("DELETE FROM Kullanicilar Where USERID=@USERID", DbConnUser);
            cmd.Parameters.Add("@USERID", SqlDbType.Int).Value = ID;

            cmd.ExecuteNonQuery();

            cmd.Dispose();
            DbConnUser.Dispose();
            DbConnUser.Close();

            Response.Redirect(MyReferrer);
        }
        else
        {
            Alert.Show("Seçim Yapmadınız.Lütfen Tekrar Deneyin");
        }
    }
}