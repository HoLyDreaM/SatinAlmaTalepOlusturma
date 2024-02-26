<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RezervKontrol.aspx.cs" Inherits="UserControl_RezervKontrol" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rezerv Kontrol</title>
        <link href="../Css/StyleSheet.css" type="text/css" rel="stylesheet" />
    <link href="../Css/jquery-ui.css" rel="stylesheet" type="text/css" media="all" /> 
    <link href="../Css/Popup.css" rel="stylesheet" type="text/css" media="all" />

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
             .auto-style1
        {
            width: 600px;
        }
            </style>
    <script type="text/javascript">
<!--
    function MM_openBrWindow(theURL, winName, features) { //v2.0
        window.open(theURL, winName, features);
    }
    //-->
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table class="auto-style1">
            <tr>
                <td align="center" style="border:1px solid Black;">Stok Kodu</td>
                <td align="center" style="border:1px solid Black;">Stok Adı</td>
                <td align="center" style="border:1px solid Black;">Miktar</td>
                <td align="center" style="border:1px solid Black;">Talep Eden</td>
                <td align="center" style="border:1px solid Black;">Departman</td>
                <td align="center" style="border:1px solid Black;">Alt Departman</td>
            </tr>
            <asp:Panel ID="RezervMiktarlari" runat="server"></asp:Panel>
        </table>
        
    </div>
    </form>
</body>
</html>
