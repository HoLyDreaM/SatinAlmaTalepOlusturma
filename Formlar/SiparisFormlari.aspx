﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Anasayfa.master" AutoEventWireup="true" CodeFile="SiparisFormlari.aspx.cs" Inherits="Formlar_SiparisFormlari" %>
<%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
                    
                    <asp:ImageButton ID="SiparisFormuAl" runat="server" ImageUrl="~/images/siparisformual.png" 
                        OnClick="SiparisFormuAl_Click" />
                </td>
                <td>
                    <dx:ASPxButton ID="btnGuncelle" runat="server" OnClick="btnGuncelle_Click" Text="Güncelle">
                    </dx:ASPxButton>
                </td>
                <td>Tümünü Seç
                <input type="checkbox" onclick="checkAll(this)" title="Tümünü Seç" />
                </td>
            </tr>
        </table> <br /><center><fieldset style="width:70%">
    <asp:Panel ID="PanelSiparisFormlari" runat="server"></asp:Panel>
    </fieldset></center>
</asp:Content>