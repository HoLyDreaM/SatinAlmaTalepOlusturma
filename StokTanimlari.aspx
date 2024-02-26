﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Anasayfa.master" AutoEventWireup="true" CodeFile="StokTanimlari.aspx.cs" Inherits="StokTanimlari" %>

<%@ Register Assembly="DevExpress.Web.v21.2, Version=21.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br /><center><fieldset style="width:70%;">
        <center></center><br />
        <table width="525">
        <tr>
<td style="width:150px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Stok Kodları</td>
        </tr>
            <tr>
             <td><dx:ASPxGridView ID="grdStokKodlari" runat="server" AutoGenerateColumns="False" 
                        DataSourceID="sqlStokKodlari" Width="625px" ClientInstanceName="grdStokKodlari" 
                 KeyFieldName="StokID">
                        <Columns>
                            <dx:GridViewCommandColumn ButtonType="Image" Caption="İşlemler" VisibleIndex="5">
                                <EditButton Text="Düzenle" Visible="True">
                                    <Image Url="~/images/edit.png">
                                    </Image>
                                </EditButton>
                                <NewButton Text="Ekle" Visible="True">
                                    <Image Url="~/images/insert.png">
                                    </Image>
                                </NewButton>
                                <DeleteButton Text="Sil" Visible="True">
                                    <Image Url="~/images/delete.png">
                                    </Image>
                                </DeleteButton>
                                <CancelButton Text="Vazgeç">
                                </CancelButton>
                                <UpdateButton Text="Düzenle  |  ">
                                </UpdateButton>
                                <HeaderStyle HorizontalAlign="Center" />
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn Caption="Stok Kodu" FieldName="StokKodu" VisibleIndex="0">
                <Settings AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Stok Adı" FieldName="StokAciklama" VisibleIndex="1">
                <Settings AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Satıcı Durumu" FieldName="SaticiDurumu" VisibleIndex="2">
                <Settings AllowAutoFilter="False" />
            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Departman Durumu" FieldName="DepartmanDurumu" VisibleIndex="3">
                <Settings AllowAutoFilter="False" />
            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="StokID" FieldName="StokID" Visible="False" VisibleIndex="4">
                <Settings AllowAutoFilter="False" />
            </dx:GridViewDataTextColumn>
                        </Columns>
        <SettingsPager SEOFriendly="Enabled" PageSize="20">
            <FirstPageButton Text="İlk Sayfa">
            </FirstPageButton>
            <LastPageButton Text="Son Sayfa">
            </LastPageButton>
            <NextPageButton Text="İleri">
            </NextPageButton>
            <PrevPageButton Text="Geri">
            </PrevPageButton>
            <Summary Text="Sayfa {0} ile {1} Arası ({2} Stok Kodları)" />
            <PageSizeItemSettings Caption="Sayfa:">
            </PageSizeItemSettings>
        </SettingsPager>
            <Settings ShowFilterRow="True" />
            <SettingsText EmptyDataRow="Kayıt Bulunamamıştır." />
                    </dx:ASPxGridView>
    <asp:SqlDataSource ID="sqlStokKodlari" runat="server" ConnectionString="<%$ ConnectionStrings:DbConnUser %>" 
    SelectCommand="SELECT  StokID, StokKodu, StokAciklama, SaticiDurumu, DepartmanDurumu FROM  StokKodlari ORDER BY StokKodu ASC" 
    DeleteCommand="DELETE FROM StokKodlari WHERE StokID=@StokID" 
    InsertCommand="INSERT INTO StokKodlari(StokKodu, StokAciklama, SaticiDurumu, DepartmanDurumu) 
        VALUES (@StokKodu, @StokAciklama, @SaticiDurumu, @DepartmanDurumu)" 
    UpdateCommand="UPDATE StokKodlari SET StokKodu=@StokKodu, StokAciklama=@StokAciklama, 
        SaticiDurumu=@SaticiDurumu, DepartmanDurumu=@DepartmanDurumu WHERE StokID=@StokID">
    <DeleteParameters>
        <asp:Parameter Name="StokID" Type="Int32" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="StokKodu" Type="String"/>
        <asp:Parameter Name="StokAciklama" Type="String"/>
        <asp:Parameter Name="SaticiDurumu" Type="String"/>
        <asp:Parameter Name="DepartmanDurumu" Type="String" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="StokKodu" Type="String"/>
        <asp:Parameter Name="StokAciklama" Type="String"/>
        <asp:Parameter Name="SaticiDurumu" Type="String"/>
        <asp:Parameter Name="DepartmanDurumu" Type="String" />
        <asp:Parameter Name="StokID" Type="Int32"  />
    </UpdateParameters>
                 </asp:SqlDataSource>
             </td>
            </tr>
        </table></fieldset></center>
</asp:Content>

