<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TeklifFormu.aspx.cs" Inherits="Formlar_TeklifFormu" %>
<%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Teklif Formu</title>
    <link href="../Css/StyleSheet.css" type="text/css" rel="stylesheet" />
    <link href="../Css/jquery-ui.css" rel="stylesheet" type="text/css" media="all" /> 
    <link href="../Css/Popup.css" rel="stylesheet" type="text/css" media="all" />
    <link rel="shortcut icon" href="~/images/sifaslogo.jpg" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
<table width="60" border="0">
  <tr>
<td align="left" scope="col" style="font-family:Tahoma; font-size:10px; font-weight:bold;"><asp:ImageButton ID="imgKaydet" runat="server" Height="35px" ImageUrl="~/images/kaydet.png" Width="60px" OnClick="imgKaydet_Click" /></td>
  </tr>
</table><div id="Tablo" runat="server">
        <style type="text/css">
        .FormTasarim {
            font-family:Tahoma;
            font-size:10px;
        }
       .FormTasarim2 {
            font-family:Tahoma;
            font-size:10px;
            float:right;
        }
       .auto-style1 {
                width: 196px;
                height: 128px;
            }
    </style>
<table width="600" border="0">
  <tr>
  <td align="left" scope="col">
      <img class="auto-style1" src="http://www.editorgroup.net/Programlar/Sifas/sifaslogo.jpg" alt="" /></td>
  </tr>
</table>
<table width="600" border="0" >
  <tr>
  <td align="right" scope="col" width="200">&nbsp;</td>
  <td align="right" scope="col" width="200">&nbsp;</td>
  <td align="right" scope="col" width="200">&nbsp;</td>
  <td align="right" scope="col" width="200">&nbsp;</td>
  <td align="right" scope="col" width="200"><b style="font-family:Tahoma; font-size:10px;">Tarih : </b> <asp:Label ID="lblTarih" runat="server" CssClass="FormTasarim" Font-Names="Tahoma" Font-Size="10px"></asp:Label></td>
  </tr>
</table>
<table width="600" border="0">
  <tr>
    <td colspan="6" align="center" scope="col" style="font-family:Tahoma; font-size:10px; font-weight:bold;"><p align="center"><strong>T E K L İ F&nbsp;&nbsp; F O R M U</strong><br />
      ----------------------------------<br />
      <strong>A C İ L</strong></p></td>
  </tr>
  <tr>
    <td width="300"><b style="font-family:Tahoma; font-size:10px;">Firma Adı :</b> </td>
    <td colspan="5">&nbsp;</td>
  </tr>
  <tr>
    <td colspan="6">&nbsp;</td>
  </tr>
  <tr>
    <td colspan="6" style="font-family:Tahoma; font-size:10px;">Fabrikamızın İhtiyacı olan  aşağıda dökümünü yapmış olduğumuz malzemeler ile ilgili teklifinizi 
    tarafımıza  fakslamanızı rica ederiz. </td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td width="100">&nbsp;</td>
    <td width="250">&nbsp;</td>
    <td width="100">&nbsp;</td>
    <td width="100">&nbsp;</td>
    <td width="150">&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td colspan="2" align="center" style="font-family:Tahoma; font-size:10px;">Saygılarımızla,</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td colspan="2" align="center" style="font-family:Tahoma; font-size:10px;">SİFAŞ<br />
    SENTETİK İPLİK FABRİKALARI<br />
    ANONİM ŞİRKETİ</td>
  </tr>
   <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td align="center"><b style="font-family:Tahoma; font-size:10px;">MALIN CİNSİ</b></td>
    <td align="center"><b style="font-family:Tahoma; font-size:10px;">MİKTARI</b></td>
    <td colspan="3" align="center"><b style="font-family:Tahoma; font-size:10px;">TEKLİF ETTİĞİNİZ FİYAT</b></td>
    <td align="center"><b style="font-family:Tahoma; font-size:10px;">TESLİM SÜRESİ</b></td>
  </tr>
  <tr>
<td colspan="6">---------------------------------------------------------------------------------------------------------------</td>
  </tr>
    <asp:Panel ID="panelTeklifFormu" runat="server"></asp:Panel>
  <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td colspan="6" style="font-family:Tahoma; font-size:10px;">NOT: ÖDEME <asp:Label ID="lblOdemePlani" runat="server" CssClass="FormTasarim" Font-Names="Tahoma" Font-Size="10px"></asp:Label> AYLIK OLACAKTIR.<br />
      <br />
LÜTFEN 224-243 11 91 NUMARALI  FAKSIMIZA TEYİD FAKSINIZI GÖNDERİNİZ.<br />
TEKLİFLERİNİZİ AYNI GÜN İÇİNDE  TARAFIMIZA BİLDİRMENİZİ RİCA EDERİZ!<br />
<br />
Yukarıda yazılı  siparişlerinizi belirtilen şartlarda ve belirtilen süre içinde teslim  edeceğimizi kabul ve teyit ediyoruz.<br /></td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td height="65" colspan="2" align="center" style="font-family:Tahoma; font-size:10px;">                                                                                                                                            Saygılarımızla 
                                                                                                                                            <br />
      <br />
    (Kaşe ve İmza) </td>
  </tr>
  <tr>
    <td colspan="6">---------------------------------------------------------------------------------------------------------------</td>
  </tr>
  <tr>
    <td colspan="6" style="font-family:Tahoma; font-size:10px;"><b>Fabrika : </b>Organize Sanayi  Bölgesi Mavi Cd.No: 17 BURSA <b>Vergi Dairesi</b> : Ertuğrulgazi <b>No:</b> 7700012472<br />
<b>Tel.:</b> 224-243 14 00 (5 hat) <b>Fax:</b> 224- 243 11 91 <b>E-mail :</b> satinalma@sifas.com.tr </td>
  </tr>
</table>
    </div>
    </div>
    </form>
</body>
</html>
