<%@ Page Title="" Language="C#" MasterPageFile="~/Anasayfa.master" AutoEventWireup="true" CodeFile="ProfilEdit.aspx.cs" Inherits="ProfilEdit" %>

<%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 750px;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br /><br />
    <center><fieldset style="width: 78%;">
                   <table class="auto-style1">
        <tr>
            <td style="width:180px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Kullanıcı Adı</td>
            <td style="width:180px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Kullanıcı Kodu</td>
            <td style="width:180px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Şifre</td>
            <td style="width:180px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Ad Soyad</td>
            <td style="width:180px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">İzin Baş. Tarihi</td>
            <td style="width:180px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">İzin Bit. Tarihi</td>
        </tr>
        <tr>
            <td><asp:TextBox ID="txtKullaniciAdi" runat="server" Font-Names="Calibri" Font-Size="12px" TabIndex="1" Enabled="False"></asp:TextBox></td>
            <td><asp:TextBox ID="txtKullaniciKodu" runat="server" Font-Names="Calibri" Font-Size="12px" TabIndex="2" Enabled="False"></asp:TextBox></td>
            <td><asp:TextBox ID="txtSifre" runat="server" Font-Names="Calibri" Font-Size="12px" TabIndex="3"></asp:TextBox></td>
            <td><asp:TextBox ID="txtAdSoyad" runat="server" Font-Names="Calibri" Font-Size="12px" TabIndex="4"></asp:TextBox></td>
            <td><dx:ASPxDateEdit ID="dtizinBas" runat="server" TabIndex="5" DisplayFormatString="dd-MM-yyyy"></dx:ASPxDateEdit></td>
            <td><dx:ASPxDateEdit ID="dtizinBit" runat="server" TabIndex="6" DisplayFormatString="dd-MM-yyyy"></dx:ASPxDateEdit></td>
        </tr>
        <tr>
            <td colspan="7"><center><dx:ASPxButton ID="btnKullaniciEdit" runat="server" Text="Kullanıcı Düzenle" Width="150px" OnClick="btnKullaniciEdit_Click"></dx:ASPxButton></center>
            </td>
        </tr>
    </table></fieldset></center>
</asp:Content>

