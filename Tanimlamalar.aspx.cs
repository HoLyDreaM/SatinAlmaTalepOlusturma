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

public partial class Tanimlamalar : System.Web.UI.Page
{

    #region Bağlantı Ayarları

    SqlConnection DbConnUser = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());
    SqlCommand cmd;
    SqlDataReader dr;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Giris"] != "Evet")
        {
            Alert.Show("Lütfen Giriş Yaptıktan Sonra Tekrar Deneyin.");
            Session["Sorgu"] = "";
            Response.Redirect("Login.Aspx");
        }
    }

}