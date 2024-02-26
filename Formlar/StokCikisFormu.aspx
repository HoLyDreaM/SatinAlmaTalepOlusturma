<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StokCikisFormu.aspx.cs" Inherits="Formlar_StokCikisFormu" %>
<%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Stok Çıkış Formu</title>
    <link href="../Css/StyleSheet.css" type="text/css" rel="stylesheet" />
    <link href="../Css/jquery-ui.css" rel="stylesheet" type="text/css" media="all" /> 
    <link href="../Css/Popup.css" rel="stylesheet" type="text/css" media="all" />
    <link rel="shortcut icon" href="<%= Page.ResolveUrl("~")%>images/sifaslogo.jpg" />
</head>
<script language="javascript" type="text/javascript">
    function PrintEtBakalim(strid) {
        //var prtContent = document.getElementById(strid);
        var printContent = document.getElementById(strid);
        var windowUrl = 'about:blank';
        var uniqueName = new Date();
        var windowName = 'Print' + uniqueName.getTime();
        var printWindow = window.open(windowUrl, windowName,
                                         'width=%100,height=auto');
        printWindow.document.write(printContent.innerHTML);
        printWindow.document.close();
        printWindow.focus();
        printWindow.print();
        printWindow.close();
    }
    </script>
<body>
    <form id="form1" runat="server">
    <div>
<table width="60" border="0">
  <tr>
<td align="left" scope="col" style="font-family:Calibri; font-size:14px; font-weight:bold;"><asp:ImageButton ID="imgYazdir" runat="server" Height="35px" ImageUrl="~/images/yazdir.png"  OnClientClick="PrintEtBakalim('Tablo')"  Width="60px" OnClick="imgYazdir_Click" /></td>
  </tr>
</table><div id="Tablo">
    <style type="text/css">
        .FormTasarim {
            font-family:Calibri;
            font-size:12px;
        }
        .auto-style1
        {
            height: 25px;
        }
        </style>
     <center>
<table width="650" border="0" align="center">
  <tr>
    <td height="40" colspan="5" align="center" scope="col" style="font-family:Calibri; font-size:18px; font-weight:bold;border:1px solid Black; height:30px;">STOK ÇIKIŞ HAREKETİ</td>
  </tr>
  <tr>
    <td  align="left" style="font-family:Calibri; font-size:12px; font-weight:bold;border:1px solid Black; width:100px;">Hareket No :</td>
    <td  align="left" colspan="3" style="font-family:Calibri; font-size:12px; border:1px solid Black;">
        <asp:Label ID="lblHareketNo" runat="server"></asp:Label>
      </td>
    <td width="150" align="left" style="font-family:Calibri; font-size:12px; border:1px solid Black; height:30px;">Tarih : 
        <asp:Label ID="lblTarih" runat="server" Font-Names="Calibri" Font-Size="12px"></asp:Label>
      </td>
  </tr>
  <tr>
    <td  align="left" style="font-family:Calibri; font-size:12px; font-weight:bold;border:1px solid Black; width:100px;">Depo :</td>
    <td  align="left" style="font-family:Calibri; font-size:12px; border:1px solid Black; width:150px;">
        <asp:Label ID="lblDepo" runat="server" Font-Names="calibri" Font-Size="12px"></asp:Label>
      </td>
    <td width="140" style="font-family:Calibri; font-size:12px; font-weight:bold;border:1px solid Black; height:30px;">İhtiyacı Açan Kullanıcı:</td>
    <td colspan="2" align="left" style="font-family:Calibri; font-size:12px; border:1px solid Black; height:30px;">
        <asp:Label ID="lblDepartman" runat="server" Font-Names="Calibri" Font-Size="12px"></asp:Label>
      </td>
  </tr>
</table>
<table border="0" align="center" style="width: 650px">
  <tr>
    <td width="110" scope="col" style="font-family:Calibri; font-size:12px; font-weight:bold;border:1px solid Black;" class="auto-style1">STOK KODU</td>
    <td scope="col" style="font-family:Calibri; font-size:14px; font-weight:bold;border:1px solid Black;" class="auto-style1">STOK ADI</td>
    <td width="65" scope="col" style="font-family:Calibri; font-size:12px; font-weight:bold;border:1px solid Black;" class="auto-style1">MİKTAR</td>
    <td width="75" scope="col" style="font-family:Calibri; font-size:12px; font-weight:bold;border:1px solid Black;" class="auto-style1">İHT. NO</td>
    <td width="110" scope="col" style="font-family:Calibri; font-size:12px; font-weight:bold;border:1px solid Black;" class="auto-style1">MASRAF YERİ</td>
  </tr>
<asp:Panel ID="panelCikisFormu" runat="server"></asp:Panel>
  <tr>
    <td style="font-family:Calibri; font-size:16px;border:1px solid Black;">&nbsp;</td>
    <td style="font-family:Calibri; font-size:16px;border:1px solid Black;">&nbsp;</td>
    <td style="font-family:Calibri; font-size:16px;border:1px solid Black;">&nbsp;</td>
    <td style="font-family:Calibri; font-size:16px;border:1px solid Black;">&nbsp;</td>
    <td style="font-family:Calibri; font-size:16px;border:1px solid Black;">&nbsp;</td>
  </tr>
</table>
<table width="650" border="0" align="center">
  <tr>
    <td scope="col" style="font-family:Calibri; font-size:16px; font-weight:bold;border:1px solid Black;" align="center" valign="top">Teslim Eden</td>
    <td height="90" scope="col" style="font-family:Calibri; font-size:16px; font-weight:bold;border:1px solid Black;" align="center" valign="top">Teslim Alan</td>
  </tr>
</table>
         </center>
    </div>
    </form>
</body>
</html>
