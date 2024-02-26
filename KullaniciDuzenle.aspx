<%@ Page Language="C#" AutoEventWireup="true" CodeFile="KullaniciDuzenle.aspx.cs" Inherits="KullaniciDuzenle" %>

<%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
           width: 900px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
<center><fieldset style="width:78%;"><table class="auto-style1">
        <tr>
            <td style="width:180px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Kullanıcı Kodu</td>
            <td style="width:180px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Kullanıcı Adı</td>
            <td style="width:180px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Şifre</td>
            <td style="width:180px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Ad Soyad</td>
            <td style="width:180px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Departman</td>
            <td style="width:180px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Yetki</td>
        </tr>
        <tr>
            <td><dx:ASPxTextBox ID="txtKullaniciKodu" runat="server" Width="180px"></dx:ASPxTextBox></td>
            <td><dx:ASPxTextBox ID="txtKullaniciAdi" runat="server" Width="180px" Enabled="False"></dx:ASPxTextBox></td>
            <td><dx:ASPxTextBox ID="txtSifre" runat="server" Width="180px"></dx:ASPxTextBox></td>
            <td><dx:ASPxTextBox ID="txtAdSoyad" runat="server" Width="180px"></dx:ASPxTextBox></td>
            <td>
                <asp:DropDownList ID="drpDepartmanlar" runat="server" 
                    DataSourceID="sqlDepartmanlar" DataTextField="STK006_ReferansAciklamasi" 
                    DataValueField="STK006_ReferansKodu" Font-Names="Calibri" Width="150px">
                </asp:DropDownList>
                <asp:SqlDataSource ID="sqlDepartmanlar" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:DbConnLink %>" 
                    SelectCommand="SELECT STK006_Row_ID,STK006_ReferansTuru, STK006_ReferansKodu, STK006_ReferansAciklamasi 
                                  FROM  STK006 WHERE STK006_ReferansTuru=96"></asp:SqlDataSource>
            </td>
            <td>
                <asp:DropDownList ID="drpYetkiler" runat="server" DataSourceID="sqlYetkiler" 
                    DataTextField="Ünvan" DataValueField="YetkiKodu" Font-Names="Calibri" Width="150px">
                </asp:DropDownList>
                <asp:SqlDataSource ID="sqlYetkiler" runat="server" ConnectionString="<%$ ConnectionStrings:DbConnUser %>" 
                    SelectCommand="SELECT YetkiID AS YetkiID,Yetki_Unvani AS 'Ünvan',YetkiKodu  FROM Yetki_Unvanlari"></asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td><center><dx:ASPxButton ID="btnKullaniciEdit" runat="server" Text="Kullanıcı Düzenle" Width="180px" OnClick="btnKullaniciEdit_Click"></dx:ASPxButton></center></td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table></fieldset></center>
    </div>
    </form>
</body>
</html>
