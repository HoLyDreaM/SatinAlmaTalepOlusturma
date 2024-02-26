<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sifaş Web Satın Alma Modülü</title>
    <link rel="shortcut icon" href="~/images/sifaslogo.jpg" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width="30%" border="0" align="center" cellpadding="1" cellspacing="1" style="border: 1px solid #333;">
  <tr>
    <td align="center"><img src="images/sifaslogo.jpg" width="196" height="128" alt="" /></td>
  </tr>
  <tr>
    <td align="center" style="font-family:Calibri; font-size:14px;border-top: 1px solid #333;">Kullanıcı Kodu</td>
  </tr>
  <tr>
    <td align="center">
        <asp:TextBox ID="txtKullaniciAdi" runat="server" Width="150px" 
            Font-Names="Calibri" Font-Size="12px"></asp:TextBox>
      </td>
  </tr>
  <tr>
    <td align="center">
        <asp:Label ID="lblError" runat="server" Font-Names="Calibri" Font-Size="12px" 
            ForeColor="#333333"></asp:Label>
      </td>
  </tr>
  <tr>
    <td align="center" style="font-family:Calibri; font-size:14px;" style="border-top: 1px solid #333;">Şifre</td>
  </tr>
  <tr>
    <td align="center">
        <asp:TextBox ID="txtPass" runat="server" Width="150px" Font-Names="Calibri" 
            Font-Size="12px" TextMode="Password"></asp:TextBox>
      </td>
  </tr>
  <tr>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td align="center" style="border-top: 1px solid #333;">
        <asp:Button ID="btnGiris" runat="server" Font-Names="Calibri" Font-Size="14px" 
            onclick="btnGiris_Click" Text="Giriş" Width="100px" />
      </td>
  </tr>
  <tr>
    <td>&nbsp;</td>
  </tr>
</table>
    </div>
    </form>
</body>
</html>
