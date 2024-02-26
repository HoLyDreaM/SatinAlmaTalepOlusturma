<%@ Page Title="" Language="C#" MasterPageFile="~/Anasayfa.master" AutoEventWireup="true" CodeFile="TeklifFormlari.aspx.cs" Inherits="Formlar_TeklifFormlari" %>
<%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 400px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
    <br />
    <table align="center">
            <tr>
                <td>
                    
                    <asp:ImageButton ID="imgTeklifFormuAl" runat="server" ImageUrl="~/images/teklifformual.png" 
                        OnClick="imgTeklifFormuAl_Click" />
                </td>
                <td>
                    <dx:ASPxButton ID="btnGuncelle" runat="server" OnClick="btnGuncelle_Click" Text="Güncelle">
                    </dx:ASPxButton>
                </td>
                <td>Tümünü Seç
                <input type="checkbox" onclick="checkAll(this)" title="Tümünü Seç" />
                </td>
            </tr>
        </table><br />
            <table width="150" border="0" style="margin-left:180px;" runat="server" id="GenelTablomuz">
                <tr>
                    <td width="10" style="width: 210px" align="center">Ödeme Kaç Aylık Olacak</td>
                </tr>
                <tr>
                    <td align="center">
<dx:ASPxTextBox ID="txtOdemePlani" runat="server" Width="150px" TabIndex="2">
                              <ClientSideEvents KeyPress="function(s,e) {ProcessKeyPress(s, e);}" /> 
                               </dx:ASPxTextBox>
                    </td>
                </tr>
            </table>
    <br /><center><fieldset style="width:70%">
    <asp:Panel ID="panelidariKisimlar" runat="server"></asp:Panel>
    </fieldset></center><asp:SqlDataSource ID="sqlCariHesaplar" runat="server" 
            ConnectionString="<%$ ConnectionStrings:DbConnLink %>" 
            SelectCommand="SELECT [CAR002_HesapKodu], [CAR002_Unvan1] FROM [CAR002]">
        </asp:SqlDataSource>
</asp:Content>

