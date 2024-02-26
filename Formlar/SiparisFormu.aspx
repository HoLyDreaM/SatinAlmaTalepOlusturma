<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SiparisFormu.aspx.cs" Inherits="Formlar_SiparisFormu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sipariş Formu</title>
    <link href="../Css/StyleSheet.css" type="text/css" rel="stylesheet" />
    <link href="../Css/jquery-ui.css" rel="stylesheet" type="text/css" media="all" /> 
    <link href="../Css/Popup.css" rel="stylesheet" type="text/css" media="all" />
    <link rel="shortcut icon" href="~/images/sifaslogo.jpg" />
    <meta http-equiv="content-type" content="application/xhtml+xml; charset=UTF-8" />
</head>
<body>
    <form id="form1" runat="server">
    <div><table width="60" border="0">
  <tr>
<td align="left" scope="col" style="font-family:Calibri; font-size:14px; font-weight:bold;"><asp:ImageButton ID="imgKaydet" runat="server" Height="35px" ImageUrl="~/images/kaydet.png" Width="60px" OnClick="imgKaydet_Click" /></td>
  </tr>
</table><div id="Tablo" runat="server" style="font-family:Tahoma;font-size:10px;">
    <style type="text/css">
        .FormTasarim {
            font-family:Calibri;
            font-size:9px;
        }
        .auto-style2
        {
            height: 17px;
        }
        </style>
<table width="900" border="0" class="FormTasarim">
  <tr>
    <td width="300">
        <asp:Label ID="lblUnvan" runat="server" Font-Names="Tahoma" Font-Size="10px"></asp:Label>
      </td>
    <td colspan="4" rowspan="4" align="center" style="font-weight:bold;font-family:Tahoma; font-size:13px;">SİPARİŞ FORMU</td>
    <td width="300" align="right" style="font-family:Tahoma;font-size:10px;">SİFAŞ Sentetik İplik Fabrikaları A.Ş</td>
  </tr>
  <tr>
    <td><asp:Label ID="lblAdres1" runat="server" Font-Names="Tahoma" Font-Size="10px"></asp:Label></td>
    <td align="right" style="font-family:Tahoma;font-size:10px;">Organize Sanayi Bölgesi</td>
  </tr>
  <tr>
    <td><asp:Label ID="lblAdres2" runat="server" Font-Names="Tahoma" Font-Size="10px"></asp:Label></td>
    <td align="right" style="font-family:Tahoma;font-size:10px;">Mavi Cad. No :17</td>
  </tr>
  <tr>
    <td>
        <asp:Label ID="lblAdres3" runat="server" Font-Names="Tahoma" Font-Size="10px"></asp:Label>
      </td>
    <td align="right" style="font-family:Tahoma;font-size:10px;">Bursa</td>
  </tr>
  <tr>
    <td width="200">
        <asp:Label ID="lblVergiDairesi" runat="server" Font-Names="Tahoma" Font-Size="10px"></asp:Label>
      </td>
    <td width="200">&nbsp;</td>
    <td width="200">&nbsp;</td>
    <td width="200">&nbsp;</td>
    <td width="200">&nbsp;</td>
    <td width="200" style="font-weight:bold;">&nbsp;</td>
  </tr>
  <tr>
    <td width="200">
        <asp:Label ID="lblVergiNo" runat="server" Font-Names="Tahoma" Font-Size="10px"></asp:Label>
      </td>
    <td width="200">&nbsp;</td>
    <td width="200">&nbsp;</td>
    <td width="200">&nbsp;</td>
    <td width="200">&nbsp;</td>
    <td width="200" style="font-weight:bold;">&nbsp;</td>
  </tr>
  <tr>
    <td width="200">&nbsp;</td>
    <td width="200">&nbsp;</td>
    <td width="200">&nbsp;</td>
    <td width="200">&nbsp;</td>
    <td width="200">&nbsp;</td>
    <td width="200"style="font-weight:bold;">
        <asp:Label ID="lblTarih" runat="server" Font-Names="Tahoma" Font-Size="10px"></asp:Label>
      </td>
  </tr>
  <tr>
    <td colspan="6"><p style="font-family:Tahoma;font-size:10px;">Firmamıza bildirmiş olduğunuz teklif tarafımızca değerlendirilerek fabrikamızın ihtiyacı olan malzemelerin firmanızdan alınması uygun görülmüştür.Aşağıda dökümü yapılan 
    siparişlerin teklifimizdeki şartlar dahilinde teyidini rica ederiz.</p></td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td align="center" style="font-family:Tahoma;font-size:10px;">Saygılarımızla</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td align="center">&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  </table>
<table width="900" border="0" class="FormTasarim">
  <tr>
    <td width="100" align="center" style="font-weight:bold;font-family:Tahoma; font-size:10px;">&nbsp;Sipariş No :</td>
    <td width="400" align="left"><asp:Label ID="lblSiparisNo" runat="server" Font-Names="Tahoma" Font-Size="10px"></asp:Label></td>
    <td width="100" align="center" style="font-weight:bold;">&nbsp;</td>
    <td width="100" align="center" style="font-weight:bold;">&nbsp;</td>
    <td width="100" align="center" style="font-weight:bold;">&nbsp;</td>
    <td width="100" align="center" style="font-weight:bold;">&nbsp;</td>
  </tr>
  <tr>
    <td width="100" align="center" style="font-weight:bold;font-family:Tahoma;font-size:10px;">Stok Kodu</td>
    <td width="400" align="left" style="font-weight:bold;font-family:Tahoma;font-size:10px;">Stok Adı</td>
    <td width="100" align="center" style="font-weight:bold;font-family:Tahoma;font-size:10px;">Miktar</td>
    <td width="100" align="center" style="font-weight:bold;font-family:Tahoma;font-size:10px;">Birim</td>
    <td width="100" align="center" style="font-weight:bold;font-family:Tahoma;font-size:10px;">Fiyat</td>
    <td width="100" align="center" style="font-weight:bold;font-family:Tahoma;font-size:10px;">Toplam Fiyat</td>
  </tr>
  <tr>
    <td height="1" colspan="6"><hr /></td>
  </tr>
    <asp:Panel ID="panelSiparisFormu" runat="server"></asp:Panel>
</table>
<table width="900" border="0" class="FormTasarim">
  <tr>
    <td width="100" align="center"></td>
    <td width="400" align="left"></td>
    <td width="100" align="center"></td>
    <td width="100" align="center"></td>
    <td width="100" align="center"></td>
    <td width="100" align="right" style="font-family:Tahoma;font-size:10px;">--------------------</td>
  </tr>
  <tr>
    <td width="100" align="center"></td>
    <td width="400" align="left" style="font-weight:bold;font-family:Tahoma;font-size:10px;">MAL BEDELİ</td>
    <td width="100" align="center"></td>
    <td width="100" align="center"></td>
    <td width="100" align="center"></td>
    <td runat="server" id="araToplam" width="100" align="center" style="font-weight:bold;font-family:Tahoma;font-size:10px;"></td>
  </tr>
  <tr>
    <td align="center"></td>
    <td align="left" style="font-weight:bold;font-family:Tahoma;font-size:10px;">KATMA DEĞER VERGİSİ</td>
    <td align="center"></td>
    <td align="center"></td>
    <td align="center"></td>
    <td width="100" align="center" style="font-weight:bold;font-family:Tahoma;font-size:10px;" runat="server" id="Kdv"></td>
  </tr>
  <tr>
    <td align="center">&nbsp;</td>
    <td align="center">&nbsp;</td>
    <td align="center">&nbsp;</td>
    <td align="center">&nbsp;</td>
    <td align="center">&nbsp;</td>
    <td width="100" align="right" style="font-family:Tahoma;font-size:10px;">--------------------</td>
  </tr>
  <tr>
    <td align="center">&nbsp;</td>
    <td align="left" style="font-weight:bold;font-family:Tahoma;font-size:10px;">GENEL TOPLAM</td>
    <td align="center">&nbsp;</td>
    <td align="center">&nbsp;</td>
    <td align="center">&nbsp;</td>
    <td width="100" align="center" style="font-weight:bold;font-family:Tahoma;font-size:10px;" runat="server" id="GenelTotal"></td>
  </tr>
</table>
<table width="900" border="0" class="FormTasarim">
  <tr>
    <td colspan="6" class="auto-style2" style="font-family:Tahoma;font-size:10px;">&nbsp;</td>
  </tr>
  <tr>
    <td colspan="6" class="auto-style2" style="font-family:Tahoma;font-size:10px;">&nbsp;</td>
  </tr>
  <tr>
    <td colspan="6" class="auto-style2" style="font-family:Tahoma;font-size:10px;">Yukarıda yazılı siparişlerinizi belirtilen şartlarda ve belirtilen süre içinde teslim edeceğimizi kabul ve teyid ederiz.</td>
  </tr>
  <tr>
    <td width="150"></td>
    <td width="150"></td>
    <td width="150">&nbsp;</td>
    <td width="150" style="font-family:Tahoma;font-size:10px;">İmza</td>
    <td align="center"  width="150"></td>
    <td width="150">&nbsp;</td>
  </tr>
  <tr>
    <td colspan="6" style="font-family:Tahoma;font-size:10px;">Sipariş teyidi için bu kısmı imzalayarak (224)-243 11 91 numaraya fakslayınız.</td>
  </tr>
</table></div>
    </div>
    </form>
</body>
</html>
