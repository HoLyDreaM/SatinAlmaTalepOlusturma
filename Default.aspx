<%@ Page Title="" Language="C#" MasterPageFile="~/Anasayfa.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <script type="text/javascript">
         function SelectAllCheckboxes1(chk) {
             $('#<%=grdTaleplerimiz.ClientID%>').find("input:checkbox").each(function () {
                 if (this != chk) { this.checked = chk.checked; }
             });
         }
          </script>
    <br /><center><fieldset style="width:90%;">
    <table class="auto-style1">
        <tr>
            <td><center>
                <asp:RadioButtonList ID="rdTalepler" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rdTalepler_SelectedIndexChanged" RepeatDirection="Horizontal">
                    <asp:ListItem>Tümü</asp:ListItem>
                    <asp:ListItem Value="Satın Alma Onayı Bekliyor">Satın Alma Onayı Bekleyenler</asp:ListItem>
                    <asp:ListItem Value="Ambar Onayı">Ambar Onayı Bekleyenler</asp:ListItem>
                    <asp:ListItem Value="Teklif Aşamasında">Teklif Bekleyenler</asp:ListItem>
                    <asp:ListItem>Sipariş Açılmış</asp:ListItem>
                    <asp:ListItem>Onay Bekleyenler</asp:ListItem>
                    <asp:ListItem>Rededilen Talepler</asp:ListItem>
                    <asp:ListItem>Temine Hazır</asp:ListItem>
                    <asp:ListItem>Kapalı İhtiyaçlar</asp:ListItem>
                    <asp:ListItem>Beklete Alınanlar</asp:ListItem>
                </asp:RadioButtonList>
                </center></td>
        </tr>
    </table></fieldset></center>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
        <asp:UpdatePanel ID="updatePanelGirisSorgusu" runat="server">
        <ContentTemplate>
            <br /><center><table width="30" border="0" align="center">
  <tr>
    <td scope="row">
      <asp:ImageButton ID="imgParcaliSiparisGonder" runat="server" Height="35px" ImageUrl="~/images/parcalisiparis.png" OnClick="imgParcaliSiparisGonder_Click" OnClientClick="MM_openBrWindow('UserControl/ParcaliSiparis.Aspx','','toolbar=no,location=yes,status=yes,resizable=no,scrollbars=yes,width=900,height=550')" Width="128px" /></td>
      <td><asp:ImageButton ID="imgTopluSiparis" runat="server" Height="35px" ImageUrl="~/images/toplusiparis.png" OnClick="imgTopluSiparis_Click" OnClientClick="MM_openBrWindow('UserControl/TopluSiparis.Aspx','','toolbar=no,location=yes,status=yes,resizable=no,scrollbars=yes,width=900,height=550')" Width="128px" /></td>
      <td><dx:ASPxButton ID="btnSiparisKapat" runat="server" Text="Sipariş Kapat" Width="150px" OnClick="btnSiparisKapat_Click"></dx:ASPxButton></td>
      <td><dx:ASPxButton ID="btnTalepGuncelle" runat="server" Text="Talepleri Güncelle" Width="150px" OnClick="btnTalepGuncelle_Click"></dx:ASPxButton></td>
      <td><dx:ASPxButton ID="btnTumunuOnayla" runat="server" OnClick="btnTumunuOnayla_Click" Text="Onayla" Width="130px"></dx:ASPxButton></td>
      <td><dx:ASPxButton ID="btnDagit" runat="server" OnClick="btnDagit_Click" Text="Bölümlere Dağıt" Width="130px"></dx:ASPxButton></td>
      <td><dx:ASPxButton ID="btnSatinAlmayaGonder" runat="server" OnClick="btnSatinAlmayaGonder_Click" Text="Satın Almaya Gönder" Width="170px"></dx:ASPxButton></td>
      <td><dx:ASPxButton ID="btnAmbaraGonder" runat="server" OnClick="btnAmbaraGonder_Click" Text="Ambara Gönder" Width="130px"></dx:ASPxButton></td>
      <td><dx:ASPxButton ID="btnBeklemeyeAl" runat="server" Text="Beklemeye Al" Width="170px" OnClick="btnBeklemeyeAl_Click"></dx:ASPxButton></td>
  </tr>
</table></center>
            <center><div class="taleptablosu"><br /><fieldset style="width: 90%;">
                <asp:GridView ID="grdTaleplerimiz" runat="server" AutoGenerateColumns="False" CellPadding="4" 
                    DataKeyNames="TalepID" ForeColor="#333333" GridLines="None" 
                    OnPageIndexChanging="grdTaleplerimiz_PageIndexChanging" PageSize="20" AllowPaging="True" ShowHeaderWhenEmpty="True" >
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
                                <input id="chkAll" onclick="javascript: SelectAllCheckboxes1(this);" runat="server" type="checkbox" />  
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="chk" runat="server" type="checkbox" value='<%# Eval("TalepID") %>' visible='<%# Convert.ToBoolean(Eval("OnayDurum")) %>' />  
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
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
                        <asp:BoundField DataField="Aciklama" HeaderText="Açıklama" SortExpression="Açıklama" >
                        <HeaderStyle HorizontalAlign="Center" Width="150px" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Miktar" HeaderText="Miktar" SortExpression="Miktar" >
                        <HeaderStyle HorizontalAlign="Center" Width="100px" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Birim" HeaderText="Birim" SortExpression="Birim" >
                        <HeaderStyle HorizontalAlign="Center" Width="100px" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="StokMiktari" HeaderText="Stok Miktarı" SortExpression="Stok Miktarı" DataFormatString="{0:N2}">
                        <HeaderStyle HorizontalAlign="Center" Width="100px" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SipMiktar" HeaderText="Sipariş Miktarı" SortExpression="SipMiktar" DataFormatString="{0:N2}">
                        <HeaderStyle HorizontalAlign="Center" Width="100px" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TeminYeri" HeaderText="Temin Yeri" SortExpression="TeminYeri" >
                        <HeaderStyle HorizontalAlign="Center" Width="150px" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="KullanilacakYer" HeaderText="Kullanılacak Yer" SortExpression="KullanilacakYer" >
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
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Sipariş İşlemi
                            </HeaderTemplate>
                        <ItemTemplate>
                                <input id="chkSiparis" runat="server" type="checkbox" value='<%# Eval("TalepID") %>' />  
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="100px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
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
                </asp:GridView>

                <br /></center>  </fieldset></div></center></ContentTemplate>
</asp:UpdatePanel>

</asp:Content>

