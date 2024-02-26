<%@ Page Title="" Language="C#" MasterPageFile="~/Anasayfa.master" AutoEventWireup="true" CodeFile="Kullanicilar.aspx.cs" Inherits="Kullanicilar" %>

<%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
        <style type="text/css">
        .taleptablosu {
            overflow-y: scroll;
            width: 80%;
            height: auto;
            border:1px solid;
        }
    </style>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br /><br /><asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager><center>
    <table>
        <tr>
            <td><a href="#" target="_self" onClick=MM_openBrWindow('KullaniciEkle.aspx','','toolbar=no,location=yes,status=yes,resizable=yes,scrollbars=yes,width=650,height=525')><img src="images/kullaniciekle.jpg" width="133" height="50" alt="" border="0" /></a></td>
            <td><dx:ASPxButton ID="btnGuncelle" runat="server" OnClick="btnGuncelle_Click" Text="Güncelle"></dx:ASPxButton></td>
        </tr>
    </table>
        <br />
    </center>
    <center><div class="taleptablosu"><fieldset><asp:UpdatePanel ID="updatePanelGirisSorgusu" runat="server">
        <ContentTemplate>
        <asp:ListView ID="lstKullanicilar" runat="server" GroupItemCount="1" OnItemCommand="lstKullanicilar_ItemCommand" >
       <LayoutTemplate>
       <table border="0" cellpadding="5" width="1000px">
        <tr>
    <td width="100" style="text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;" scope="row">USERID</td>
    <td width="100" style="text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Kullanıcı Kodu</td>
    <td width="100"style="text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Kullanıcı Adı</td>
    <td width="100"style="text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Şifre</td>
    <td width="100" style="text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Ad Soyad</td>
    <td width="100" style="text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Departman Adı</td>
    <td width="120"style="text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Alt Departman Adı</td>
    <td width="100"style="text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Yetki</td>
    <td width="100"style="text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Durum</td>
    <td width="100" style="text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">İşlem</td>
  </tr>
           <table border="0" cellpadding="5" width="1000px">
       <asp:PlaceHolder ID="groupPlaceHolder" runat="server"></asp:PlaceHolder>
           </table>
       </td>
       </tr>
       </table>
       
       </LayoutTemplate>
       <GroupTemplate>
       <tr>
       <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
       </tr>
       </GroupTemplate>
       <ItemTemplate>
                <td width="100" style="text-align:center; font-family:Calibri; background-color:#c2e9bf; color:black;"><%# Eval("USERID")%></td>
                <td width="100" style="text-align:center; font-family:Calibri; background-color:#c2e9bf; color:black;"><%# Eval("KullaniciKodu")%></td>
                <td width="100"style="text-align:center; font-family:Calibri; background-color:#c2e9bf; color:black;"><%# Eval("KullaniciAdi")%></td>
                <td width="100"style="text-align:center; font-family:Calibri; background-color:#c2e9bf; color:black;"><%# Eval("Sifre")%></td>
                <td width="100" style="text-align:center; font-family:Calibri; background-color:#c2e9bf; color:black;"><%# Eval("AdSoyad")%></td>
                <td width="100" style="text-align:center; font-family:Calibri; background-color:#c2e9bf; color:black;"><%# Eval("DepartmanAdi")%></td>
                <td width="120"style="text-align:center; font-family:Calibri; background-color:#c2e9bf; color:black;"><%# Eval("AltDepartmanAdi")%></td>
                <td width="100"style="text-align:center; font-family:Calibri; background-color:#c2e9bf; color:black;"><%# Eval("Yetki_Unvani")%></td>
                <td width="100"style="text-align:center; font-family:Calibri; background-color:#c2e9bf; color:black;"><%# Eval("Durum")%></td>
                <td width="100" style="text-align:center; text-align:center; font-family:Calibri; background-color:#c2e9bf; color:black;">
                <a href="#" target="_self" onClick=MM_openBrWindow('UserControl/KullaniciDuzenle.aspx?USERID=<%# Eval("USERID") %>','','toolbar=no,location=yes,status=yes,resizable=yes,scrollbars=yes,width=650,height=425')><img src="images/edit.png" width="15" height="15" alt="" border="0" /></a>
                <asp:LinkButton ID="lnkSil" runat="server" CommandArgument='<%# Eval("USERID") %>' CommandName="Sil"><img src="images/delete.png" width="15" height="15" alt="" border="0" /></asp:LinkButton></td>
       </ItemTemplate>
       </asp:ListView><br /><center>
           <asp:DataPager ID="dpSayfalama" runat="server" PagedControlID="lstKullanicilar"
        PageSize="20" QueryStringField="Sayfa" onprerender="dpSayfalama_PreRender">
        <Fields>
            <asp:NumericPagerField />
            <asp:NextPreviousPagerField FirstPageText="İlk" LastPageText="Son" 
                NextPageText="İleri" PreviousPageText="Geri"/>
        </Fields>
    </asp:DataPager>
            </center></ContentTemplate>
</asp:UpdatePanel>
    </fieldset></div></center> 
        
</asp:Content>

