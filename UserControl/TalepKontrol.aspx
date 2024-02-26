<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TalepKontrol.aspx.cs" Inherits="UserControl_TalepKontrol" %>

<%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Talep Kontrol</title>
    <link href="../Css/StyleSheet.css" type="text/css" rel="stylesheet" />
    <link href="../Css/jquery-ui.css" rel="stylesheet" type="text/css" media="all" /> 
    <link href="../Css/Popup.css" rel="stylesheet" type="text/css" media="all" />
    <script type="text/javascript" src='http://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js'></script>

    <script type="text/javascript" src="../Scripts/script.js"></script>
    <link rel="shortcut icon" href="~/images/sifaslogo.jpg" />
        <style type="text/css">
            .auto-style2 {
                width: 600px;
                height: 433px;
            }
            .auto-style3
            {
                width: 114px;
            }
            </style>
    <script type="text/javascript">
        function ProcessKeyPress(s, evt) {
            var charCode = (evt.htmlEvent.which) ? evt.htmlEvent.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                _aspxPreventEvent(evt.htmlEvent);

        }
</script>
    <script type="text/javascript">
<!--
    function MM_openBrWindow(theURL, winName, features) { //v2.0
        window.open(theURL, winName, features);
    }
    //-->
</script>
<script type = "text/javascript">
    function Confirm() {
        var confirm_value = document.createElement("INPUT");
        confirm_value.type = "hidden";
        confirm_value.name = "confirm_value";
        if (confirm("Bu Talebi Silmek İstediğinize Emin Misiniz?")) {
            confirm_value.value = "Evet";
        } else {
            confirm_value.value = "Hayır";
        }
        document.forms[0].appendChild(confirm_value);
    }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <center><div style="font-size:16px;font-weight:bold;">SİFAŞ İHTİYAÇ DETAY TABLOSU</div></center>
        <fieldset>
        <table class="auto-style2" align="center">
            <tr>
                <td height="25" width="200" align="center" bgcolor="#3366CC" style="color:white;">İhtiyaç No / İhtiyaç Tarihi</td>
                <td width="10" align="center" bgcolor="#3366CC" style="color:white;">:</td>
                <td align="left" bgcolor="#3366CC"><asp:Label ID="lblihtiyacNo" runat="server" ForeColor="White"></asp:Label>&nbsp;/ <asp:Label ID="lblihtiyacTarihi" runat="server" ForeColor="White"></asp:Label></td>
            </tr>
            <tr>
                <td height="25" width="200" align="center" bgcolor="#3366CC" style="color:white;">Departman / Talep Eden Kişi</td>
                <td width="10" align="center" bgcolor="#3366CC" style="color:white;">:</td>
                <td align="left" bgcolor="#3366CC"><asp:Label ID="lblDepartman" runat="server" ForeColor="White"></asp:Label>&nbsp;/ <asp:Label ID="lblTalepEdenKisi" runat="server" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td height="25" width="200" align="center" bgcolor="#3366CC" style="color:white;">Kullanılacak Departman</td>
                <td width="10" align="center" bgcolor="#3366CC" style="color:white;">:</td>
                <td align="left" bgcolor="#3366CC"><asp:Label ID="lblKullanilacakDepartman" runat="server" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td height="25" width="200" align="center" bgcolor="#3366CC" style="color:white;">İsteme Nedeni / Temin Yeri / Teslim Tarihi</td>
                <td width="10" align="center" bgcolor="#3366CC" style="color:white;">:</td>
                <td align="left" bgcolor="#3366CC"><asp:Label ID="lblistemeNedeni" runat="server" ForeColor="White"></asp:Label>&nbsp;/ <asp:Label ID="lblTeminYeri" runat="server" ForeColor="White"></asp:Label>&nbsp;/ <asp:Label ID="lblTeslimTarihi" runat="server" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td height="25" width="200" align="center" bgcolor="#3366CC" style="color:white;">Stok Kodu</td>
                <td width="10" align="center" bgcolor="#3366CC" style="color:white;">:</td>
                <td align="left" bgcolor="#3366CC"><asp:Label ID="lblStokKodu" runat="server" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td height="25" width="200" align="center" bgcolor="#3366CC" style="color:white;">Stok Adı</td>
                <td width="10" align="center" bgcolor="#3366CC" style="color:white;">:</td>
                <td align="left" bgcolor="#3366CC"><asp:Label ID="lblStokAdi" runat="server" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td height="25" width="200" align="center" bgcolor="#3366CC" style="color:white;">İhtiyaç Miktarı</td>
                <td width="10" align="center" bgcolor="#3366CC" style="color:white;">:</td>
                <td align="left" bgcolor="#3366CC"><asp:Label ID="lblihtiyacMiktari" runat="server" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td height="25" width="200" align="center" bgcolor="#3366CC" style="color:white;">Açıklama 1</td>
                <td width="10" align="center" bgcolor="#3366CC" style="color:white;">:</td>
                <td align="left" bgcolor="#3366CC"><asp:Label ID="lblAciklama1" runat="server" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td height="25" width="200" align="center" bgcolor="#3366CC" style="color:white;">Açıklama 2</td>
                <td width="10" align="center" bgcolor="#3366CC" style="color:white;">:</td>
                <td align="left" bgcolor="#3366CC"><asp:Label ID="lblAciklama2" runat="server" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td height="25" width="200" align="center" bgcolor="#3366CC" style="color:white;">Açıklama 3</td>
                <td width="10" align="center" bgcolor="#3366CC" style="color:white;">:</td>
                <td align="left" bgcolor="#3366CC"><asp:Label ID="lblAciklama3" runat="server" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td height="25" width="200" align="center" bgcolor="#3366CC" style="color:white;">Reddetme Nedeni</td>
                <td width="10" align="center" bgcolor="#3366CC" style="color:white;">:</td>
                <td align="left" bgcolor="#3366CC">&nbsp;<asp:Label ID="lblRedNedeni" runat="server" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr runat="server" id="redSebep">
                <td height="25" width="200" align="center" bgcolor="#3366CC" style="color:white;">Reddetme Nedeni</td>
                <td width="10" align="center" bgcolor="#3366CC" style="color:white;">:</td>
                <td align="left" bgcolor="#3366CC">
                    <asp:DropDownList ID="drpRedNedenleri" runat="server" Font-Names="Calibri" Font-Size="12px" Width="170px" DataSourceID="sqlRedNedenleri" DataTextField="Red Nedeni" DataValueField="Red Nedeni">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td height="25" width="200" align="center" bgcolor="#3366CC" style="color:white;">Değişiklik Nedeni</td>
                <td width="10" align="center" bgcolor="#3366CC" style="color:white;">:</td>
                <td align="left" bgcolor="#3366CC"><asp:Label ID="lblDegisiklikNedeni" runat="server" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td height="25" width="200" align="center" bgcolor="#3366CC" style="color:white;">Sipariş No</td>
                <td width="10" align="center" bgcolor="#3366CC" style="color:white;">:</td>
                <td align="left" bgcolor="#3366CC"><asp:Label ID="lblSiparisNo" runat="server" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td height="25" width="200" align="center" bgcolor="#3366CC" style="color:white;">Cari Hesap / Birim Fiyatı / Miktar</td>
                <td width="10" align="center" bgcolor="#3366CC" style="color:white;">:</td>
                <td align="left" bgcolor="#3366CC">
                    <asp:Label ID="lblCariHesap" runat="server" ForeColor="White"></asp:Label>  
                    <asp:Label ID="lblBirimFiyat" runat="server" ForeColor="White"></asp:Label> 
                    <asp:Label ID="lblMiktar" runat="server" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td height="25" width="200" align="center" bgcolor="#3366CC" style="color:white;">Gelen Miktar</td>
                <td width="10" align="center" bgcolor="#3366CC" style="color:white;">:</td>
                <td align="left" bgcolor="#3366CC"><asp:Label ID="lblGelenMiktar" runat="server" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td height="25" width="200" align="center" bgcolor="#3366CC" style="color:white;">Toplam Yapılan Çıkış Miktarı</td>
                <td width="10" align="center" bgcolor="#3366CC" style="color:white;">:</td>
                <td align="left" bgcolor="#3366CC"><asp:Label ID="lblCikisMiktar" runat="server" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td height="25" width="200" align="center" bgcolor="#3366CC" style="color:white;">Çıkış Evrak Numarası</td>
                <td width="10" align="center" bgcolor="#3366CC" style="color:white;">:</td>
                <td align="left" bgcolor="#3366CC"><asp:Label ID="lblCikisEvrakNo" runat="server" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td height="25" width="200" align="center" colspan="3">
                    <table align="center">
                <tr align="center">
                    <td align="center"><asp:Literal ID="ltdLink" runat="server"></asp:Literal></td>
                    <td align="center"><asp:ImageButton ID="imgTalepSil" runat="server" ImageUrl="~/images/delete.png" OnClick="OnConfirm" OnClientClick="Confirm()" /></td>
                    <td align="center"><dx:ASPxButton ID="btnReddet" runat="server" Text="Reddet" OnClick="btnReddet_Click"></dx:ASPxButton></td>
                    <td align="center"><dx:ASPxButton ID="btnTalepKapat" runat="server" Text="Talep Kapat" OnClick="btnTalepKapat_Click" Width="120px"></dx:ASPxButton></td>
                </tr>
            </table>
                </td>
            </tr>
        </table>
            <table align="center" width="600">
                <tr>
                    <td>
                        <dx:ASPxButton ID="btnFax" runat="server" OnClick="btnFax_Click" Text="Fax" Width="100px">
                        </dx:ASPxButton>
                    </td>
                    <td>
                        <dx:ASPxButton ID="btnidari" runat="server" OnClick="btnidari_Click" Text="İdari" Width="100px">
                        </dx:ASPxButton>
                    </td>
                    <td>
                        <dx:ASPxButton ID="btnTeknik" runat="server" OnClick="btnTeknik_Click" Text="Teknik" Width="100px">
                        </dx:ASPxButton>
                    </td>
                    <td>
                        <dx:ASPxButton ID="btnSiparis" runat="server" OnClick="btnSiparis_Click" Text="Sipariş" Width="100px">
                        </dx:ASPxButton>
                    </td>
                    <td class="auto-style3">
                        <dx:ASPxButton ID="btnihtiyac" runat="server" OnClick="btnihtiyac_Click" style="height: 23px" Text="İhtiyaç" Width="100px">
                        </dx:ASPxButton>
                    </td>
                    <td>
                        <dx:ASPxButton ID="btnStokCikis" runat="server" OnClick="btnStokCikis_Click" Text="Stok Çıkış" Width="100px">
                        </dx:ASPxButton>
                    </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
            
            <div style="font-size:16px; font-weight:bold;" runat="server" id="Kapat">
                <center>
                <asp:LinkButton ID="lnkKapat" runat="server" OnClick="lnkKapat_Click">Kapat</asp:LinkButton>
                <asp:SqlDataSource ID="sqlRedNedenleri" runat="server" ConnectionString="<%$ ConnectionStrings:DbConnUser %>" SelectCommand="SELECT RedSebebi AS 'Red Nedeni' FROM RedNedenleri
            ORDER BY RedSebebi ASC "></asp:SqlDataSource>
                </center></div>
    </fieldset>
    </div>
    </form>
</body>
</html>
