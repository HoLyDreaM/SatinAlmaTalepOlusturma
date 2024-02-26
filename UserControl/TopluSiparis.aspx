<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TopluSiparis.aspx.cs" Inherits="UserControl_TopluSiparis" %>
<%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>




<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sifaş Web Satın Alma Modülü</title>
    <link href="../Css/StyleSheet.css" type="text/css" rel="stylesheet" />
    <link href="../Css/jquery-ui.css" rel="stylesheet" type="text/css" media="all" /> 
    <link href="../Css/Popup.css" rel="stylesheet" type="text/css" media="all" />
    

    <script type="text/javascript" src="../Scripts/script.js"></script>
    <link rel="shortcut icon" href="../images/sifaslogo.jpg" />
    <style type="text/css">
        .auto-style1 {
            width: 250px;
        }
        #dis_bolme {
            width: 900px;
        }
        .ic_bolme {
            float: left;
            width: 150px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
<table border="0" style="width: 400px">
  <tr>
    <td align="center" scope="row" class="auto-style1">Cari Hesap Adı</td>
    <td align="center" scope="row" class="auto-style1"><dx:ASPxDateEdit ID="dtSiparisTarihi" runat="server" Date="09/06/2013 11:45:21" EditFormat="Custom" Font-Names="Calibri" Font-Size="12px" TabIndex="6" EditFormatString="dd-MM-yyyy" Enabled="False"></dx:ASPxDateEdit></td>
  </tr>
  <tr>
    <td scope="row" class="auto-style1">
        <dx:ASPxTextBox ID="txtCariHesapAdi" runat="server" Width="170px">
        </dx:ASPxTextBox>
      </td>
    <td scope="row" class="auto-style1">
        <dx:ASPxButton ID="btnCariKodAra" runat="server" Text="Cari Hesap Kodu Ara" Width="200px" OnClick="btnCariKodAra_Click">
        </dx:ASPxButton>
      </td>
  </tr></table>

<table border="0" style="width:950px">
  <tr>
    <td align="center" scope="row" class="auto-style1">Cari Hesap Ünvanı</td>
    <td align="center" scope="row" class="auto-style1">Cari Hesap Kodu</td>
    <td align="center" scope="row" class="auto-style1">&nbsp;</td>
    <td align="center" scope="row" class="auto-style1">Para Birimi</td>
    <td align="center" scope="row" class="auto-style1">Döviz Cinsi</td>
    <td align="center" scope="row" class="auto-style1">Döviz Kuru</td>
    <td align="center" scope="row" class="auto-style1">Döviz Hesapla</td>
  </tr>
  <tr>
    <td scope="row" class="auto-style1"><asp:DropDownList ID="drpCariAdi" runat="server" AutoPostBack="True" DataTextField="Unvan" DataValueField="HesapKodu" Font-Names="Calibri" Font-Size="12px" Width="200px" OnSelectedIndexChanged="drpCariAdi_SelectedIndexChanged"></asp:DropDownList></td>
    <td scope="row" class="auto-style1"><asp:TextBox ID="txtHesapKodu" runat="server" Width="120px" Font-Names="Calibri" Font-Size="12px"></asp:TextBox></td>
    <td scope="row" class="auto-style1"><dx:ASPxButton ID="btnTopluSiparis" runat="server" Text="Toplu Sipariş Aç" Width="150px" OnClick="btnTopluSiparis_Click"></dx:ASPxButton></td>
    <td align="center"><asp:DropDownList ID="drpParaBirimi" runat="server" Font-Names="Calibri" Font-Size="12px" Width="80px" AutoPostBack="True"></asp:DropDownList></td>
    <td align="center"><asp:DropDownList ID="drpDovizCinsi" runat="server" Font-Names="Calibri" Font-Size="12px" Width="80px" AutoPostBack="True"></asp:DropDownList></td>
    <td align="center"><asp:TextBox ID="txtDovizKuru" runat="server" Width="65px" Font-Names="Calibri" Font-Size="12px"></asp:TextBox></td>
    <td align="center"><dx:ASPxButton ID="btnDovizHesapla" runat="server" OnClick="btnDovizHesapla_Click" Text="Döviz Hesapla" Width="125px"></dx:ASPxButton></td>
  </tr></table>

    <asp:Table ID="tabloSiparis" runat="server" Width="76%">
      </asp:Table>
                <asp:SqlDataSource ID="sqlCariHesaplar" runat="server" 
            ConnectionString="<%$ ConnectionStrings:DbConnLink %>" 
            SelectCommand="SELECT [CAR002_HesapKodu], [CAR002_Unvan1] FROM [CAR002]">
        </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlDepoKodlari" runat="server" 
            ConnectionString="<%$ ConnectionStrings:DbConnLink %>" 
            SelectCommand="SELECT TOP(1) 'Seçiniz' AS DepoKodu,'Seçiniz' AS DepoAdi
UNION ALL
SELECT STK006_ReferansKodu AS DepoKodu,STK006_ReferansAciklamasi AS DepoAdi FROM STK006
WHERE STK006_ReferansTuru=96">
        </asp:SqlDataSource>

    </div>
        <asp:Panel ID="Panel1" runat="server">
        </asp:Panel>
    </form>
</body>
</html>
