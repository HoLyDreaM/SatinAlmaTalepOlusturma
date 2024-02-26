﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="KullaniciDuzenle.aspx.cs" Inherits="UserControl_KullaniciDuzenle" %>
<%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>





<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sifaş Web Satın Alma Modülü</title>
    <link href="../Css/StyleSheet.css" type="text/css" rel="stylesheet" />
    <link href="../Css/jquery-ui.css" rel="stylesheet" type="text/css" media="all" /> 
    <link href="../Css/Popup.css" rel="stylesheet" type="text/css" media="all" />
    <script type="text/javascript" src='http://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js'></script>

    <script type="text/javascript" src="../Scripts/script.js"></script>
    <link rel="shortcut icon" href="~/images/sifaslogo.jpg" />
</head>
<body>
    <form id="form1" runat="server">
    <div><asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager><fieldset><center>
    <table width="404" border="0" style="border:1px solid;" cellpadding="0" cellspacing="0">
  <tr>
    <td width="150" scope="row" class="auto-style1" height="25">USER ID</td>
    <td width="10" class="auto-style2">:</td>
    <td width="240"><dx:ASPxTextBox ID="txtUSERID" runat="server" Width="222px" ReadOnly="True"></dx:ASPxTextBox></td>
  </tr>
  <tr>
    <td scope="row" class="auto-style1" height="25">Kullanıcı Kodu&nbsp;</td>
    <td class="auto-style2">:</td>
    <td><asp:TextBox ID="txtKullanıcıKodu" runat="server" Font-Names="Calibri" Font-Size="12px" Width="219px" ReadOnly="True" Enabled="False"></asp:TextBox></td>
  </tr>
  <tr>
    <td scope="row" class="auto-style1" height="25">Kullanıcı Adı</td>
    <td class="auto-style2">:</td>
    <td><asp:TextBox ID="txtKullaniciAdi" runat="server" Font-Names="Calibri" Font-Size="12px" Width="219px" ReadOnly="True" Enabled="False"></asp:TextBox></td>
  </tr>
  <tr>
    <td scope="row" class="auto-style1" height="25">Şifre</td>
    <td class="auto-style2">:</td>
    <td><asp:TextBox ID="txtSifre" runat="server" Font-Names="Calibri" Font-Size="12px" Width="219px"></asp:TextBox></td>
  </tr>
  <tr>
    <td scope="row" class="auto-style1" height="25">Ad Soyad</td>
    <td class="auto-style2">:</td>
    <td><asp:TextBox ID="txtAdSoyad" runat="server" Font-Names="Calibri" Font-Size="12px" Width="219px"></asp:TextBox></td>
  </tr>
  <tr>
    <td scope="row" class="auto-style1" height="25">Departman Adı</td>
    <td class="auto-style2">:</td>
    <td><asp:UpdatePanel ID="updateDepartmanAdi1" runat="server">
        <ContentTemplate>
            <dx:ASPxComboBox ID="cmbDepartmanAdi" runat="server" AutoPostBack="True" Width="219px" TextField="DepartmanAdi" ValueField="DepartmanID" OnSelectedIndexChanged="cmbDepartmanAdi_SelectedIndexChanged" DataSourceID="sqlDepartmanlar">
        </dx:ASPxComboBox>
        </ContentTemplate>
</asp:UpdatePanel>
      </td>
  </tr>
  <tr>
    <td scope="row" class="auto-style1" height="25">Departman Kodu</td>
    <td class="auto-style2">:</td>
    <td><asp:UpdatePanel ID="updateDepartmanKodu1" runat="server">
        <ContentTemplate>
            <dx:ASPxComboBox ID="cmbDepartmanKodu" runat="server" AutoPostBack="True" Width="219px" TextField="DepartmanKodu" ValueField="DepartmanKodu" OnSelectedIndexChanged="cmbAltDepartmanAdi_SelectedIndexChanged">
        </dx:ASPxComboBox>
        </ContentTemplate>
</asp:UpdatePanel>
      </td>
  </tr>
  <tr>
    <td scope="row" class="auto-style1" height="25">Alt Departman Adı</td>
    <td class="auto-style2">:</td>
    <td><asp:UpdatePanel ID="updateAltDepartmanAdi1" runat="server">
    <ContentTemplate>
        <dx:ASPxComboBox ID="cmbAltDepartmanAdi" runat="server" AutoPostBack="True" Width="219px" TextField="AltDepartmanAdi" ValueField="AltDepartmanID" OnSelectedIndexChanged="cmbAltDepartmanAdi_SelectedIndexChanged">
        </dx:ASPxComboBox></ContentTemplate>
</asp:UpdatePanel>
      </td>
  </tr>
        <tr>
            <td class="auto-style1" scope="row" height="25">Alt Departman Kodu</td>
            <td class="auto-style2">:</td>
            <td><asp:UpdatePanel ID="updateAltDepartmanKodu1" runat="server">
    <ContentTemplate>
                <dx:ASPxComboBox ID="cmbAltDepartmanKodu" runat="server" AutoPostBack="True" Width="219px" TextField="AltDepartmanKodu" ValueField="AltDepartmanKodu">
                </dx:ASPxComboBox></ContentTemplate>
</asp:UpdatePanel>
            </td>
        </tr>
  <tr>
    <td scope="row" class="auto-style1" height="25">Baglı Olduğu Şeflik</td>
    <td class="auto-style2">:</td>
    <td>
                <dx:ASPxComboBox ID="cmbBagliAltDept" runat="server" AutoPostBack="True" Width="219px" TextField="AltDepartmanAdi" ValueField="AltDepartmanID">
                </dx:ASPxComboBox>
      </td>
  </tr>
  <tr>
    <td scope="row" class="auto-style1" height="25">Kops Durum Raporu</td>
    <td class="auto-style2">:</td>
    <td>
        <dx:ASPxComboBox ID="cmbKopsDurumRaporu" runat="server" Width="219px" SelectedIndex="1">
            <Items>
                <dx:ListEditItem Text="Evet" Value="true" />
                <dx:ListEditItem Selected="True" Text="Hayır" Value="false" />
            </Items>
        </dx:ASPxComboBox>
      </td>
  </tr>
  <tr>
    <td scope="row" class="auto-style1" height="25">Yetki</td>
    <td class="auto-style2">:</td>
    <td>
        <dx:ASPxComboBox ID="cmbYetkiler" runat="server" Width="219px" DataSourceID="sqlYetkiler" TextField="Yetki_Unvani" ValueField="YetkiKodu">
        </dx:ASPxComboBox>
      </td>
  </tr>
<tr>
    <td scope="row" class="auto-style1" height="25">Birim</td>
    <td class="auto-style2">:</td>
    <td>
        <dx:ASPxComboBox ID="cmbBirimler" runat="server" Width="219px" SelectedIndex="0">
            <Items>
                <dx:ListEditItem Selected="True" Text="İdari Birim" Value="idari Birim" />
                <dx:ListEditItem Text="Teknik Birim" Value="Teknik Birim" />
            </Items>
        </dx:ASPxComboBox>
      </td>
  </tr>
<tr>
    <td scope="row" class="auto-style1" height="25">Menü Ayarı</td>
    <td class="auto-style2">:</td>
    <td>
        <dx:ASPxComboBox ID="cmbMenuKodlari" runat="server" Width="219px" SelectedIndex="0" DataSourceID="sqlMenuKodlari" TextField="MenuKodu" ValueField="MenuKodu">
        </dx:ASPxComboBox>
      </td>
  </tr>
<tr>
    <td scope="row" class="auto-style1" height="25" colspan="3"><center><asp:CheckBox ID="chkSiparis" runat="server" Text="Sipariş" AutoPostBack="True" /> | <asp:CheckBox ID="chkSatinAlma" runat="server" Text="Satın Alma" AutoPostBack="True" />&nbsp;| <asp:CheckBox ID="chkAnaDepartman" runat="server" Text="Ana Departman" />&nbsp;<asp:CheckBox ID="chkKullaniciDurum" runat="server" Text="Aktif - Pasif" /></center></td>
  </tr>
  <tr>
    <td scope="row" class="auto-style1" height="25">Masraf Merkezi</td>
    <td class="auto-style2">:</td>
    <td align="left"><asp:CheckBox ID="chkMasrafMerkezi" runat="server" Text="Masraf Merkezi" /></td>
  </tr>
  <tr>
    <td scope="row" class="auto-style1" height="25">Masraf Merkezi Kodu</td>
    <td class="auto-style2">:</td>
    <td align="left">
        <dx:ASPxComboBox ID="cmbMasrafMerkeziKodu" runat="server" Width="219px" DataSourceID="sqlMasrafMerkezleri" TextField="LinkMasrafAdi" ValueField="LinkMasrafKodu" TextFormatString="{1}">
        </dx:ASPxComboBox>
      </td>
  </tr>
        <tr>
    <td colspan="3" align="center" scope="row">&nbsp;</td></tr>
</table><br /><table>
    <tr>
        <td><dx:ASPxButton ID="btnDuzenle" runat="server" Text="Kullanıcı Düzenle" OnClick="btnDuzenle_Click"></dx:ASPxButton>
        </td>
    </tr></table>
                      <asp:SqlDataSource ID="sqlDepartmanlar" runat="server" 
                          ConnectionString="<%$ ConnectionStrings:DbConnUser %>" 
                          SelectCommand="SELECT  DepartmanID, DepartmanAdi, DepartmanKodu FROM Departmanlar">
                      </asp:SqlDataSource>
                      <asp:SqlDataSource ID="sqlYetkiler" runat="server" 
                          ConnectionString="<%$ ConnectionStrings:DbConnUser %>" 
                          SelectCommand="SELECT   Yetki_Unvani, YetkiKodu  FROM Yetki_Unvanlari">
                      </asp:SqlDataSource>
                      <asp:SqlDataSource ID="sqlMasrafMerkezleri" runat="server" ConnectionString="<%$ ConnectionStrings:DbConnUser %>" SelectCommand="SELECT LinkMasrafMerkeziKodu AS LinkMasrafKodu,
MasrafMerkeziAdi AS LinkMasrafAdi
FROM MasrafMerkezi
ORDER BY MasrafMerkeziID DESC"></asp:SqlDataSource>
                          <asp:SqlDataSource ID="sqlMenuKodlari" runat="server" ConnectionString="<%$ ConnectionStrings:DbConnUser %>" SelectCommand="SELECT [MenuKodu] FROM [MenuKontrolu]"></asp:SqlDataSource>
                      </center></fieldset>
    </div>
    </form>
</body>
</html>
