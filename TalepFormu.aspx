<%@ Page Title="" ValidateRequest="false" EnableEventValidation="false" EnableViewStateMac="false" ViewStateEncryptionMode="Never" Language="C#" MasterPageFile="~/Anasayfa.master" AutoEventWireup="true" CodeFile="TalepFormu.aspx.cs" Inherits="TalepFormu" %>

<%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 900px;
        }
        </style>
<script type="text/javascript">
    //-------------------------------------------
    // Function to only allow numeric data entry
    //-------------------------------------------
    function jsNumbers(e) {
        var evt = (e) ? e : window.event;
        var key = (evt.keyCode) ? evt.keyCode : evt.which;
        if (key != null) {
            key = parseInt(key, 10);
            if ((key < 48 || key > 57) && (key < 96 || key > 105)) {
                if (!jsIsUserFriendlyChar(key, "Numbers")) {
                    return false;
                }
            }
            else {
                if (evt.shiftKey) {
                    return false;
                }
            }
        }
        return true;
    }

    //-------------------------------------------
    // Function to only allow decimal data entry
    //-------------------------------------------
    function jsDecimals(e) {
        var evt = (e) ? e : window.event;
        var key = (evt.keyCode) ? evt.keyCode : evt.which;
        if (key != null) {
            key = parseInt(key, 10);
            if ((key < 48 || key > 57) && (key < 96 || key > 188)) {
                if (!jsIsUserFriendlyChar(key, "Decimals")) {
                    return false;
                }
            }
            else {
                if (evt.shiftKey) {
                    return false;
                }
            }
        }
        return true;
    }
    //------------------------------------------
    // Function to check for user friendly keys
    //------------------------------------------
    function jsIsUserFriendlyChar(val, step) {
        // Backspace, Tab, Enter, Insert, and Delete
        if (val == 8 || val == 9 || val == 13 || val == 45 || val == 46 || val == 188) {
            return true;
        }
        // Ctrl, Alt, CapsLock, Home, End, and Arrows
        if ((val > 16 && val < 21)) {
            return true;
        }
        if (step == "Decimals") {
            if (val == 190 || val == 110) {
                return true;
            }
        }
        // The rest
        return false;
    }
</script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
    <asp:MultiView ID="AnaEkranPenceresi" runat="server">
        <asp:View ID="AnaSorgulama" runat="server"><center>
             <table class="auto-style1">
            <tr>
           <td align="center"><asp:RadioButtonList ID="rdTalepDurumu" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rdTalepDurumu_SelectedIndexChanged">
            <asp:ListItem Value="1">İhtiyaç Oluşturma</asp:ListItem>
            <asp:ListItem Value="2">Masraf - Demirbaş -Sabit Kıymet İhtaç Açma</asp:ListItem>
        </asp:RadioButtonList></td>
            </tr>
        </table></center>
        </asp:View>
    <asp:View ID="Stoksuz" runat="server">
        <center><table class="auto-style1">
        <tr>
            <td style="width:100px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Departman Adı</td>
            <td style="width:100px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Departman Kodu</td>
            <td style="width:100px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Talep Eden Adı</td>
            <td style="width:100px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Talep Eden Kodu</td>
            <td style="width:100px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">İhtiyac Nedeni</td>
            <td style="width:170px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Kullanılacak Departman</td>
            <td style="width:100px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Temin Yeri</td>
            <td style="width:100px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Talep Tarihi</td>
            <td style="width:100px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Talep Seri No</td>
        </tr>
        <tr>
            <td><asp:TextBox ID="txtDepAdi" runat="server" Font-Names="Calibri" Font-Size="12px" TabIndex="1" Enabled="False" Width="100"></asp:TextBox></td>
            <td><asp:TextBox ID="txtDepKodu" runat="server" Font-Names="Calibri" Font-Size="12px" TabIndex="2" Enabled="False"></asp:TextBox></td>
            <td><asp:TextBox ID="txtTalepAdi" runat="server" Font-Names="Calibri" Font-Size="12px" TabIndex="3" Enabled="False" Width="100"></asp:TextBox></td>
            <td><asp:TextBox ID="txtTalepKodu" runat="server" Font-Names="Calibri" Font-Size="12px" TabIndex="4" Enabled="False" Width="100"></asp:TextBox></td>
            <td><asp:DropDownList ID="drpIhtNedeni" runat="server" Font-Names="Calibri" Font-Size="12px" Width="170px" TabIndex="5"></asp:DropDownList></td>
            <td><asp:DropDownList ID="drpKullDepartman" runat="server" Font-Names="Calibri" Font-Size="12px" Width="100px" TabIndex="5" DataTextField="MasrafMerkeziAdi" DataValueField="MasrafMerkeziKodu"></asp:DropDownList></td>
            <td><asp:DropDownList ID="drpTeminYeri" runat="server" Font-Names="Calibri" Font-Size="12px" Width="100px" TabIndex="5"></asp:DropDownList></td>
            <td><dx:ASPxDateEdit ID="dtIhtTarih" runat="server" Date="09/06/2013 11:45:21" EditFormat="Custom" Font-Names="Calibri" Font-Size="12px" TabIndex="6" DisplayFormatString="dd-MM-yyyy"></dx:ASPxDateEdit></td>
            <td><asp:TextBox ID="txtIhtTalepSeriNo" runat="server" Font-Names="Calibri" Font-Size="12px" TabIndex="7" Enabled="False"></asp:TextBox></td>
        </tr>
    </table></center><br /><center><fieldset style="width: 87%;">
        <table align="center" cellpadding="0" cellspacing="0" width="600">
            <tr>
                <td align="center" style="width:200px; height:25px; font-family:Calibri; font-size:14px; font-weight:bold;">Stok Kodu</td>
                <td align="center" style="width:200px; height:25px; font-family:Calibri; font-size:14px; font-weight:bold;">Stok Adı</td>
                <td align="center" style="width:200px; height:25px; font-family:Calibri; font-size:14px; font-weight:bold;">Ara</td>
            </tr>
            <tr>
                <td>
                    <dx:ASPxTextBox ID="txtStokKodu1" runat="server" Width="170px">
                    </dx:ASPxTextBox>
                </td>
                <td>
                    <dx:ASPxTextBox ID="txtStokAdi1" runat="server" TabIndex="1" Width="170px">
                    </dx:ASPxTextBox>
                </td>
                <td>
                    <dx:ASPxButton ID="btnStoksuzAra" runat="server" OnClick="btnStoksuzAra_Click" TabIndex="2" Text="Stok Kartı Arama" Width="150px">
                    </dx:ASPxButton>
                </td>
            </tr>
        </table><br />
        <table width="600" border="0" align="center">
  <tr>
    <td width="200" align="left" scope="col" style="font-family:Calibri;font-size:14px;background-color:gray; color:white;">Mal Kodu</td>
    <td width="10" align="center" scope="col" style="font-family:Calibri;font-size:14px; background-color:gray; color:white;">:</td>
    <td align="left" scope="col">
        <asp:DropDownList ID="drpMasrafKodlari" runat="server" Font-Names="Calibri" Font-Size="12px" Width="250px" DataTextField="StokKodu" DataValueField="StokKodu" OnSelectedIndexChanged="drpMasrafKodlari_SelectedIndexChanged" TabIndex="3" AutoPostBack="True">
        </asp:DropDownList>
      </td>
  </tr>
  <tr>
    <td align="left" style="font-family:Calibri;font-size:14px;background-color:gray; color:white;">Mal Adı</td>
    <td align="center" style="font-family:Calibri;font-size:14px; background-color:gray; color:white;">:</td>
    <td align="left">
        <asp:DropDownList ID="drpMasrafisimleri" runat="server" Font-Names="Calibri" Font-Size="12px" TabIndex="4" Width="350px" DataTextField="StokAciklama" DataValueField="StokAciklama" OnSelectedIndexChanged="drpMasrafisimleri_SelectedIndexChanged" AutoPostBack="True">
        </asp:DropDownList>
      </td>
  </tr>
  <tr>
    <td align="left" style="font-family:Calibri;font-size:14px;background-color:gray; color:white;">Birim</td>
    <td align="center" style="font-family:Calibri;font-size:14px; background-color:gray; color:white;">:</td>
    <td align="left"><dx:ASPxTextBox ID="txtStoksuzBirim" runat="server" Width="250px" TabIndex="5" Enabled="False"></dx:ASPxTextBox></td>
  </tr>
  <tr>
    <td align="left" style="font-family:Calibri;font-size:14px;background-color:gray; color:white;">Stok Miktarı</td>
    <td align="center" style="font-family:Calibri;font-size:14px; background-color:gray; color:white;">:</td>
    <td align="left"><dx:ASPxTextBox ID="txtStoksuzMiktar" runat="server" Enabled="False" Width="250px" TabIndex="6"></dx:ASPxTextBox></td>
  </tr>
            <tr>
                <td align="left" style="font-family:Calibri;font-size:14px;background-color:gray; color:white;">Talep Edilen Miktar</td>
                <td align="center" style="font-family:Calibri;font-size:14px; background-color:gray; color:white;">:</td>
                <td align="left"><table cellpadding="0" cellspacing="0" >
                    <tr>
                        <td><asp:TextBox ID="txtStoksuzTalepMiktar" runat="server" TabIndex="8" onkeydown="return jsDecimals(event);" Width="125px" DisplayFormatString="{0:n3}" Font-Names="Calibri" Font-Size="12px"></asp:TextBox></td>
                        <td><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtStoksuzTalepMiktar" Display="Dynamic" ErrorMessage="Lütfen sadece Rakam Giriniz.Ayraçı Virgül Olarak Kullanınız." ValidationExpression="[0-9]*\,?[0-9]*"></asp:RegularExpressionValidator></td>
                    </tr></table>
                </td>
            </tr>
              <tr id="StoksuzBirimDurumu" runat="server">
    <td align="left" style="font-family:Calibri;font-size:14px;background-color:gray; color:white;">Birim Durumu</td>
    <td align="center" style="font-family:Calibri;font-size:14px; background-color:gray; color:white;">:</td>
    <td align="left">
        <asp:DropDownList ID="drpStoksuzBirim" runat="server" Font-Names="Calibri" Font-Size="12px" TabIndex="9" Width="100px" AutoPostBack="True">
            <asp:ListItem>Seçiniz</asp:ListItem>
            <asp:ListItem>idari Birim</asp:ListItem>
            <asp:ListItem>Teknik Birim</asp:ListItem>
        </asp:DropDownList>
      </td>
  </tr>
            <tr>
                <td align="left" style="font-family:Calibri;font-size:14px;background-color:gray; color:white;">Açıklama 1</td>
                <td align="center" style="font-family:Calibri;font-size:14px; background-color:gray; color:white;">:</td>
                <td align="left">
                    <dx:ASPxTextBox ID="txtStoksuzAciklama" runat="server" MaxLength="40" TabIndex="10" Width="250px">
                    </dx:ASPxTextBox>
                </td>
            </tr>
  <tr>
    <td align="left" style="font-family:Calibri;font-size:14px;background-color:gray; color:white;">Açıklama 2</td>
    <td align="center" style="font-family:Calibri;font-size:14px; background-color:gray; color:white;">:</td>
    <td align="left"><dx:ASPxTextBox ID="txtStoksuzAciklama2" runat="server" Width="250px" TabIndex="11" MaxLength="40"></dx:ASPxTextBox></td>
  </tr>
  <tr>
    <td align="left" style="font-family:Calibri;font-size:14px;background-color:gray; color:white;">Açıklama 3</td>
    <td align="center" style="font-family:Calibri;font-size:14px; background-color:gray; color:white;">:</td>
    <td align="left"><dx:ASPxTextBox ID="txtStoksuzAciklama3" runat="server" Width="250px" TabIndex="12" MaxLength="40"></dx:ASPxTextBox></td>
  </tr>
  <tr>
    <td colspan="3" align="center"><dx:ASPxButton ID="btnStoksuzTalepGonder" runat="server" Text="Talep Gönder" Width="170px" TabIndex="13" OnClick="btnStoksuzTalepGonder_Click"></dx:ASPxButton></td>
  </tr>
</table></fieldset></center>
    </asp:View>
        <asp:View ID="Stoklu" runat="server">
             <center><table class="auto-style1">
        <tr>
            <td style="width:100px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Departman Adı</td>
            <td style="width:100px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Departman Kodu</td>
            <td style="width:100px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Talep Eden Adı</td>
            <td style="width:100px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Talep Eden Kodu</td>
            <td style="width:100px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">İhtiyac Nedeni</td>
            <td style="width:170px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Kullanılacak Departman</td>
            <td style="width:100px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Temin Yeri</td>
            <td style="width:100px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Talep Tarihi</td>
            <td style="width:100px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Talep Seri No</td>
        </tr>
        <tr>
            <td><asp:TextBox ID="txtDepartmanAdi" runat="server" Font-Names="Calibri" Font-Size="12px" TabIndex="1" Enabled="False" Width="100px"></asp:TextBox></td>
            <td><asp:TextBox ID="txtDepartmanKodu" runat="server" Font-Names="Calibri" Font-Size="12px" TabIndex="2" Enabled="False" Width="100px"></asp:TextBox></td>
            <td><asp:TextBox ID="txtTalepEdenAdi" runat="server" Font-Names="Calibri" Font-Size="12px" TabIndex="3" Enabled="False" Width="100px"></asp:TextBox></td>
            <td><asp:TextBox ID="txtTalepEdenKodu" runat="server" Font-Names="Calibri" Font-Size="12px" TabIndex="4" Enabled="False" Width="100px"></asp:TextBox></td>
            <td><asp:DropDownList ID="drpihtiyacNedeni" runat="server" Font-Names="Calibri" Font-Size="12px" Width="170px" TabIndex="5"></asp:DropDownList></td>
            <td><asp:DropDownList ID="drpKullDepartman1" runat="server" Font-Names="Calibri" Font-Size="12px" Width="100px" TabIndex="5" DataTextField="MasrafMerkeziAdi" DataValueField="MasrafMerkeziKodu"></asp:DropDownList></td>
            <td><asp:DropDownList ID="drpTeminYeri1" runat="server" Font-Names="Calibri" Font-Size="12px" Width="100px" TabIndex="5"></asp:DropDownList></td>
            <td><dx:ASPxDateEdit ID="dtTalepTarihi" runat="server" Date="09/06/2013 11:45:21" EditFormat="Custom" Font-Names="Calibri" Font-Size="12px" TabIndex="6" DisplayFormatString="dd-MM-yyyy"></dx:ASPxDateEdit></td>
            <td><asp:TextBox ID="txtTalepSeriNo" runat="server" Font-Names="Calibri" Font-Size="12px" TabIndex="7" Enabled="False"></asp:TextBox></td>
        </tr>
    </table></center><br /><center><fieldset style="width: 79%;">
            <table align="center" cellpadding="0" cellspacing="0" width="600">
            <tr>
                <td align="center" style="width:200px; height:25px; font-family:Calibri; font-size:14px; font-weight:bold;">Stok Kodu</td>
                <td align="center" style="width:200px; height:25px; font-family:Calibri; font-size:14px; font-weight:bold;">Stok Adı</td>
                <td align="center" style="width:200px; height:25px; font-family:Calibri; font-size:14px; font-weight:bold;">Ara</td>
            </tr>
            <tr>
                <td>
                    <dx:ASPxTextBox ID="txtStokKodu" runat="server" Width="170px">
                    </dx:ASPxTextBox>
                </td>
                <td>
                    <dx:ASPxTextBox ID="txtStokAdi" runat="server" TabIndex="1" Width="170px">
                    </dx:ASPxTextBox>
                </td>
                <td>
                    <dx:ASPxButton ID="btnStokArama" runat="server" OnClick="btnStokArama_Click" TabIndex="2" Text="Stok Kartı Arama" Width="150px">
                    </dx:ASPxButton>
                </td>
            </tr>
        </table>
        <br />
        <table width="600" border="0" align="center">
  <tr>
    <td width="200" align="left" scope="col" style="font-family:Calibri;font-size:14px;background-color:gray; color:white;">Mal Kodu</td>
    <td width="10" align="center" scope="col" style="font-family:Calibri;font-size:14px; background-color:gray; color:white;">:</td>
    <td align="left" scope="col">
        <asp:DropDownList ID="drpStokKodlari" runat="server" Font-Names="Calibri" Font-Size="12px" Width="250px" DataTextField="StokKodu" DataValueField="StokKodu" OnSelectedIndexChanged="drpStokKodlari_SelectedIndexChanged" TabIndex="3" AutoPostBack="True">
        </asp:DropDownList>
      </td>
  </tr>
  <tr>
    <td align="left" style="font-family:Calibri;font-size:14px;background-color:gray; color:white;">Mal Adı</td>
    <td align="center" style="font-family:Calibri;font-size:14px; background-color:gray; color:white;">:</td>
    <td align="left">
        <asp:DropDownList ID="drpStokAdi" runat="server" Font-Names="Calibri" Font-Size="12px" TabIndex="4" Width="350px" DataTextField="StokAciklama" DataValueField="StokAciklama" OnSelectedIndexChanged="drpStokAdi_SelectedIndexChanged" AutoPostBack="True">
        </asp:DropDownList>
      </td>
  </tr>
  <tr>
    <td align="left" style="font-family:Calibri;font-size:14px;background-color:gray; color:white;">Birim</td>
    <td align="center" style="font-family:Calibri;font-size:14px; background-color:gray; color:white;">:</td>
    <td align="left"><dx:ASPxTextBox ID="txtBirim" runat="server" Width="250px" TabIndex="5" Enabled="False"></dx:ASPxTextBox></td>
  </tr>
  <tr>
    <td align="left" style="font-family:Calibri;font-size:14px;background-color:gray; color:white;">Stok Miktarı</td>
    <td align="center" style="font-family:Calibri;font-size:14px; background-color:gray; color:white;">:</td>
    <td align="left"><dx:ASPxTextBox ID="txtStokMiktari" runat="server" Enabled="False" Width="250px" TabIndex="6"></dx:ASPxTextBox></td>
  </tr>
  <tr>
    <td align="left" style="font-family:Calibri;font-size:14px;background-color:gray; color:white;">Rezerv Miktarı</td>
    <td align="center" style="font-family:Calibri;font-size:14px; background-color:gray; color:white;">:</td>
    <td align="left"><div style="float:left;"><table cellpadding="0" cellspacing="0" style="width:150px;border-collapse:collapse;"><tr><td><dx:ASPxTextBox ID="txtRezervMiktari" runat="server" Enabled="False" TabIndex="7" Width="150px"></dx:ASPxTextBox></td><td><asp:Literal ID="ltdLink" runat="server"></asp:Literal></td></tr></table></div></td>
  </tr>
            <tr>
                <td align="left" style="font-family:Calibri;font-size:14px;background-color:gray; color:white;">Talep Edilen Miktar</td>
                <td align="center" style="font-family:Calibri;font-size:14px; background-color:gray; color:white;">:</td>
                <td align="left"><table cellpadding="0" cellspacing="0" >
                    <tr>
                        <td><asp:TextBox ID="txtMiktar" runat="server" TabIndex="8" onkeydown="return jsDecimals(event);" Width="125px" DisplayFormatString="{0:n3}" Font-Names="Calibri" Font-Size="12px"></asp:TextBox></td>
                        <td><asp:RegularExpressionValidator ID="rgx" runat="server" ControlToValidate="txtMiktar" Display="Dynamic" ErrorMessage="Lütfen sadece Rakam Giriniz.Ayraçı Virgül Olarak Kullanınız." ValidationExpression="[0-9]*\,?[0-9]*"></asp:RegularExpressionValidator></td>
                    </tr></table>
                </td>
            </tr>
  <tr id="StokluBirimDurumu" runat="server">
    <td align="left" style="font-family:Calibri;font-size:14px;background-color:gray; color:white;">Birim Durumu</td>
    <td align="center" style="font-family:Calibri;font-size:14px; background-color:gray; color:white;">:</td>
    <td align="left">
        <asp:DropDownList ID="drpStokluBirim" runat="server" Font-Names="Calibri" Font-Size="12px" TabIndex="9" Width="100px" AutoPostBack="True">
            <asp:ListItem>Seçiniz</asp:ListItem>
            <asp:ListItem>idari Birim</asp:ListItem>
            <asp:ListItem>Teknik Birim</asp:ListItem>
        </asp:DropDownList>
      </td>
  </tr>
            <tr>
                <td align="left" style="font-family:Calibri;font-size:14px;background-color:gray; color:white;">Açıklama 1</td>
                <td align="center" style="font-family:Calibri;font-size:14px; background-color:gray; color:white;">:</td>
                <td align="left">
                    <dx:ASPxTextBox ID="txtAciklama" runat="server" MaxLength="40" TabIndex="10" Width="250px">
                    </dx:ASPxTextBox>
                </td>
            </tr>
  <tr>
    <td align="left" style="font-family:Calibri;font-size:14px;background-color:gray; color:white;">Açıklama 2</td>
    <td align="center" style="font-family:Calibri;font-size:14px; background-color:gray; color:white;">:</td>
    <td align="left"><dx:ASPxTextBox ID="txtAciklama2" runat="server" Width="250px" TabIndex="11" MaxLength="40"></dx:ASPxTextBox></td>
  </tr>
  <tr>
    <td align="left" style="font-family:Calibri;font-size:14px;background-color:gray; color:white;">Açıklama 3</td>
    <td align="center" style="font-family:Calibri;font-size:14px; background-color:gray; color:white;">:</td>
    <td align="left"><dx:ASPxTextBox ID="txtAciklama3" runat="server" Width="250px" TabIndex="12" MaxLength="40"></dx:ASPxTextBox>
      </td>
  </tr>
  <tr>
    <td colspan="3" align="center"><dx:ASPxButton ID="btnTalepGonder" runat="server" Text="Talep Gönder" Width="170px" TabIndex="13" OnClick="btnTalepGonder_Click"></dx:ASPxButton></td>
  </tr>
</table>

                                   </fieldset></center>
    <asp:SqlDataSource ID="sqlMalKodlari" runat="server" ConnectionString="<%$ ConnectionStrings:DbConnLink %>" 
        SelectCommand="SELECT [STK004_MalKodu], [STK004_Aciklama] FROM [STK004]"></asp:SqlDataSource>
             <asp:SqlDataSource ID="sqlMasrafMerkezi" runat="server" 
                 ConnectionString="<%$ ConnectionStrings:DbConnUser %>" 
                 SelectCommand="SELECT [MasrafMerkeziKodu], [MasrafMerkeziAdi] FROM [MasrafMerkezi]"></asp:SqlDataSource>
             <asp:SqlDataSource ID="sqlStoksuzMalKodlari" runat="server" ConnectionString="<%$ ConnectionStrings:DbConnUser %>" SelectCommand="SELECT [StoksuzMalKodu], [StoksuzMalAdi] FROM [Stoksuz]"></asp:SqlDataSource>
        </asp:View>
    </asp:MultiView>
</asp:Content>

