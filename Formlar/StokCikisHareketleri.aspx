<%@ Page Title="" Language="C#" MasterPageFile="~/Anasayfa.master" AutoEventWireup="true" CodeFile="StokCikisHareketleri.aspx.cs" Inherits="Formlar_StokCikisHareketleri" %>
<%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>




<%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <style type="text/css">
        .auto-style1 {
            width: 250px;
        }
        #dis_bolme {
            width: 900px;
        }
        .ic_bolme {
            float: left;
            width: 150px;
        }
    </style>
        <script language="javascript" type="text/javascript">
            function radioButtonKontrol() {
            }
    </script>
            <script type="text/javascript">
                function checkAll(bx) {
                    var cbs = document.getElementsByTagName('input');
                    for (var i = 0; i < cbs.length; i++) {
                        if (cbs[i].type == 'checkbox') {
                            cbs[i].checked = bx.checked;
                        }
                    }
                }
</script>
    <br /><center>
<table border="0" style="width:520px" runat="server" id="CikisTablomuz">
  <tr>
    <td align="center" scope="row" class="auto-style1">&nbsp;</td>
    <td align="center" scope="row" class="auto-style1">Çıkış Yapılacak Depo</td>
    <td align="center" scope="row" class="auto-style1">&nbsp;</td>
    <td align="center" scope="row" class="auto-style1">Tümünü Seç</td>
    <td align="center" scope="row" class="auto-style1"></td>
  </tr>
  <tr>
    <td scope="row" class="auto-style1">
        <dx:ASPxButton ID="btnStokCikisHareketiOlustur" runat="server" Text="Stok Çıkış Hareketi Oluştur" Width="200px" OnClick="btnStokCikisHareketiOlustur_Click">
        </dx:ASPxButton>
      </td>
    <td scope="row" class="auto-style1">
        <asp:DropDownList ID="drpDepoKodu" runat="server" AutoPostBack="True" DataSourceID="sqlDepoKodlari" DataTextField="DepoAdi" DataValueField="DepoKodu" Font-Names="Calibri" Font-Size="12px" Width="170px">
        </asp:DropDownList>
      </td>
    <td scope="row" class="auto-style1">
        <dx:ASPxButton ID="btnGuncelle" runat="server" Text="Güncelle" Width="110px" OnClick="btnGuncelle_Click">
        </dx:ASPxButton>
      </td>
       <td align="center">
                <input type="checkbox" onclick="checkAll(this)" title="Tümünü Seç" />
                </td>
      <td><dx:ASPxDateEdit ID="stkCikisTarih" runat="server" Date="09/06/2013 11:45:21" EditFormat="Custom" Font-Names="Calibri" Font-Size="12px" TabIndex="6" EditFormatString="dd-MM-yyyy" Enabled="False"></dx:ASPxDateEdit></td>
  </tr></table></center>
         <br /><center><fieldset style="width:80%">
    <asp:Panel ID="panelStokCikisHareketi" runat="server"></asp:Panel>
    </fieldset></center>
    <asp:SqlDataSource ID="sqlDepoKodlari" runat="server" 
            ConnectionString="<%$ ConnectionStrings:DbConnLink %>" 
            SelectCommand="SELECT TOP(1) 'Seçiniz' AS DepoKodu,'Seçiniz' AS DepoAdi
UNION ALL
SELECT STK006_ReferansKodu AS DepoKodu,STK006_ReferansAciklamasi AS DepoAdi FROM STK006
WHERE STK006_ReferansTuru=96">
        </asp:SqlDataSource>

                </asp:Content>

