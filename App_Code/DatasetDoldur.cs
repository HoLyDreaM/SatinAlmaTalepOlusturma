using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for DatasetDoldur
/// </summary>
public class DatasetDoldur
{
    SqlConnection DbConnUser = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnUser"].ToString());

    public DatasetDoldur()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public DataSet StokKartlarimiz(string AramaSorgu)
    {
        string sorgu = AramaSorgu.ToString();
        SqlDataAdapter da = new SqlDataAdapter(sorgu, DbConnUser);
        DataSet ds = new DataSet();
        da.Fill(ds, "StokKodlari");
        return ds;
    }
}