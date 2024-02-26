<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ihtiyacPusulasi.aspx.cs" Inherits="Formlar_ihtiyacPusulasi" %>

<%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>İhtiyaç Pusulası</title>
    <link href="<%= Page.ResolveUrl("~")%>Css/StyleSheet.css" type="text/css" rel="stylesheet" />
    <link href="<%= Page.ResolveUrl("~")%>Css/jquery-ui.css" rel="stylesheet" type="text/css" media="all" /> 
    <link href="<%= Page.ResolveUrl("~")%>Css/Popup.css" rel="stylesheet" type="text/css" media="all" />
    <link rel="shortcut icon" href="<%= Page.ResolveUrl("~")%>images/sifaslogo.jpg" />
    <style type="text/css">
        .FormTasarim {
            font-family:Calibri;
            font-size:18px;
            font-weight:bold;
        }
        .YaziStili
        {
            font-family:Calibri;
            font-size:14px;
        }
    </style>
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
<body topmargin="0">
    <form id="form1" runat="server">
    <div>
    <table width="60" border="0">
  <tr>
    <td scope="col"><asp:ImageButton ID="imgYazdir" runat="server" Height="35px" ImageUrl='../images/yazdir.png'  OnClientClick="PrintEtBakalim('Tablo')"  Width="60px" OnClick="imgYazdir_Click" />
      </td>
  </tr>
</table><div id="Tablo">
     <center>
         <asp:Panel ID="panelihtiyacListesi" runat="server"></asp:Panel>
    </center></div>
    </div>
    </form>
</body>
</html>
