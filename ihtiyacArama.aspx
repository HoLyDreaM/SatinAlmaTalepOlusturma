<%@ Page Title="" Language="C#" MasterPageFile="~/Anasayfa.master" AutoEventWireup="true" CodeFile="ihtiyacArama.aspx.cs" Inherits="ihtiyacArama" %>


<%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 950px;
        }
        .HucreAyarlari {
            font-family:Calibri;
            font-size:14px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
    <center><fieldset style="width:78%;">
        <table align="center" cellpadding="0" cellspacing="0" width="600">
            <tr>
                <td align="center" style="width:200px; height:25px; font-family:Calibri; font-size:14px; font-weight:bold;">Stok Kodu</td>
                <td align="center" style="width:200px; height:25px; font-family:Calibri; font-size:14px; font-weight:bold;">Stok Adı</td>
                <td align="center" style="width:200px; height:25px; font-family:Calibri; font-size:14px; font-weight:bold;">Ara</td>
            </tr>
            <tr>
                <td align="center">
                    <dx:ASPxTextBox ID="txtStokKodu" runat="server" Width="170px">
                    </dx:ASPxTextBox>
                </td>
                <td>
                    <dx:ASPxTextBox ID="txtStokAdi" runat="server" Width="170px">
                    </dx:ASPxTextBox>
                </td>
                <td>
                    <dx:ASPxButton ID="btnStokArama" runat="server" Text="Stok Kartı Arama" Width="150px" OnClick="btnStokArama_Click">
                    </dx:ASPxButton>
                </td>
            </tr>
        </table></fieldset></center><br />
        <center><fieldset style="width:78%;">
       <table align="center" cellpadding="0" cellspacing="0" width="400">
            <tr>
                <td align="center" style="height:25px; font-family:Calibri; font-size:14px; font-weight:bold;">İhtiyaç No</td>
                <td align="center" style="width:200px; height:25px; font-family:Calibri; font-size:14px; font-weight:bold;">İhtiyaç No ya Göre Ara</td>
            </tr>
            <tr>
                <td>
                    <dx:ASPxTextBox ID="txtihtiyacNo" runat="server" Width="170px">
                    </dx:ASPxTextBox>
                </td>
                <td>
                    <dx:ASPxButton ID="btnihtiyacNoAra" runat="server" Text="İhtiyaç No ya Göre Ara" Width="200px" OnClick="btnihtiyacNoAra_Click">
                    </dx:ASPxButton>
                </td>
            </tr>
        </table></fieldset></center>
    <br /><center><fieldset style="width:78%;">
        <table align="center" cellpadding="0" cellspacing="0" class="auto-style1">
            <tr>
                <td align="center" style="width:150px; font-family:Calibri; font-size:12px; font-weight:bold;">Stok Kodu</td>
                <td align="center" style="width:150px; font-family:Calibri; font-size:12px; font-weight:bold;">Stok Adı</td>
                <td align="center" style="width:150px; font-family:Calibri; font-size:12px; font-weight:bold;">Başlangıç Tarihi</td>
                <td align="center" style="width:150px; font-family:Calibri; font-size:12px; font-weight:bold;">Bitiş Tarihi</td>
                <td align="center" style="width:150px; font-family:Calibri; font-size:12px; font-weight:bold;">Departman Kodu</td>
            </tr>
            <tr><td align="center" style="font-family:Calibri;font-size:12px;width:150px;">
                <asp:DropDownList ID="drpStokKodlari" runat="server" Font-Names="Calibri" Font-Size="12px" Width="150px" DataTextField="StokKodu" DataValueField="StokKodu" OnSelectedIndexChanged="drpStokKodlari_SelectedIndexChanged" AutoPostBack="True">
                </asp:DropDownList>
                </td>
                <td align="center" style="font-family:Calibri;font-size:12px;width:250px;">
                    <asp:DropDownList ID="drpStokAdi" runat="server" Font-Names="Calibri" Font-Size="12px" Width="250px" DataTextField="StokAciklama" DataValueField="StokAciklama" OnSelectedIndexChanged="drpStokAdi_SelectedIndexChanged" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                <td align="center">
                    <dx:ASPxDateEdit ID="minTarih" runat="server" DisplayFormatString="dd-MM-yyyy">
                    </dx:ASPxDateEdit>
                </td>
                <td>
                    <dx:ASPxDateEdit ID="maxTarih" runat="server" DisplayFormatString="dd-MM-yyyy">
                    </dx:ASPxDateEdit>
                </td>
                <td>
                    <asp:DropDownList ID="drpDepartmanKodu" runat="server" Width="150px" Font-Names="Calibri" Font-Size="12px">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table class="auto-style1">
            <tr>
                <td align="center">
                    <dx:ASPxButton ID="btnihtiyacAra" runat="server" Text="İhtiyaç Ara" OnClick="btnihtiyacAra_Click">
                    </dx:ASPxButton>
                </td>
            </tr>
        </table></fieldset>
        <br /><fieldset style="width:78%;">
                        <asp:GridView ID="grdTalepler" runat="server" AutoGenerateColumns="False" CellPadding="4" 
                    DataKeyNames="TalepID" ForeColor="#333333" GridLines="None" 
                    OnPageIndexChanging="grdTalepler_PageIndexChanging" PageSize="20" AllowPaging="True" ShowHeaderWhenEmpty="True" >
                    <AlternatingRowStyle CssClass="gridViewAlternatigRow" BackColor="White" 
                            ForeColor="#284775" />
                        <EditRowStyle CssClass="gridViewEditRow" BackColor="#999999" />
                        <EmptyDataRowStyle CssClass="gridViewEmptyDataRow" />
                        <FooterStyle CssClass="gridViewFooter" BackColor="#5D7B9D" Font-Bold="True" 
                            ForeColor="White" />
                        <HeaderStyle CssClass="gridViewHeader" BackColor="#5D7B9D" Font-Bold="True" 
                            ForeColor="White" />
                        <PagerStyle CssClass="gridViewPager" HorizontalAlign="Center" 
                            BackColor="#284775" ForeColor="White" />
                        <PagerSettings FirstPageText="İlk Sayfa" LastPageText="Son Sayfa" />
                        <RowStyle CssClass="gridViewRow" BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle CssClass="gridViewSelectedRow" BackColor="#E2DED6" 
                            Font-Bold="True" ForeColor="#333333" />
                    <Columns>
                        <asp:BoundField DataField="TalepID" HeaderText="TalepID" InsertVisible="False" ReadOnly="True" SortExpression="TalepID" Visible="False" />
                        <asp:TemplateField>
                            <HeaderTemplate>
                                İhtiyaç No
                            </HeaderTemplate>
                          <ItemTemplate>
                       <a href="#" target="_self" onClick=MM_openBrWindow('UserControl/TalepKontrol.aspx?TalepID=<%# Eval("TalepID") %>','','toolbar=no,location=yes,status=yes,resizable=no,scrollbars=yes,width=650,height=650')><%# Eval("EvrakNo") %></a>
                          </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Tarih" HeaderText="Tarih" ReadOnly="True" SortExpression="Tarih" >
                        <HeaderStyle HorizontalAlign="Center" Width="120px" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MalAdi" HeaderText="Malzeme Adı" SortExpression="Malzeme Adı" >
                        <HeaderStyle HorizontalAlign="Center" Width="150px" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Miktar" HeaderText="Miktar" SortExpression="Miktar" DataFormatString="{0:N2}">
                        <HeaderStyle HorizontalAlign="Center" Width="100px" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SipMiktar" HeaderText="Sipariş Miktarı" SortExpression="SipMiktar"  DataFormatString="{0:N2}">
                        <HeaderStyle HorizontalAlign="Center" Width="100px" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TeminYeri" HeaderText="Temin Yeri" SortExpression="TeminYeri" >
                        <HeaderStyle HorizontalAlign="Center" Width="150px" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="OnayDurumu" HeaderText="Durum" SortExpression="Durum" >
                        <HeaderStyle HorizontalAlign="Center" Width="200px" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AdSoyad" HeaderText="Kullanıcı İsmi" SortExpression="Kullanıcı İsmi" >
                        <HeaderStyle HorizontalAlign="Center" Width="130px" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                    </Columns>
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EFF3FB" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                </asp:GridView></fieldset></center>
    <br />
</asp:Content>

