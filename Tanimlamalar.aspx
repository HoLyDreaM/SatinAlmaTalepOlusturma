<%@ Page Title="" Language="C#" MasterPageFile="~/Anasayfa.master" AutoEventWireup="true" CodeFile="Tanimlamalar.aspx.cs" Inherits="Tanimlamalar" %>

<%@ Register Assembly="DevExpress.Web.v21.2, Version=21.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 325px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br /><br /><center><fieldset style="width:50%;">
    <center>
        <table class="auto-style1" style="float:left;">
        <tr>
<td style="width:150px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Kayıtlı Departmanlar</td>
        </tr>
            <tr>
             <td><dx:ASPxGridView ID="grdDepartmanlar" runat="server" AutoGenerateColumns="False" 
                        DataSourceID="sqlDepartmanlar" Width="425px" ClientInstanceName="grdDepartmanlar" 
                 KeyFieldName="DepartmanID">
                        <Columns>
                            <dx:GridViewCommandColumn ButtonType="Image" Caption="İşlemler" VisibleIndex="3">
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
                            <dx:GridViewDataTextColumn Caption="Departman Adı" FieldName="DepartmanAdi" VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Departman Kodu" FieldName="DepartmanKodu" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Departman Bölüm" FieldName="StokDepartman" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="DepartmanID" FieldName="DepartmanID" Visible="False" VisibleIndex="3">
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
            <Summary Text="Sayfa {0} ile {1} Arası ({2} Departmanlar)" />
            <PageSizeItemSettings Caption="Sayfa:">
            </PageSizeItemSettings>
        </SettingsPager>
            <SettingsText EmptyDataRow="Kayıt Bulunamamıştır." />
                    </dx:ASPxGridView>
    <asp:SqlDataSource ID="sqlDepartmanlar" runat="server" ConnectionString="<%$ ConnectionStrings:DbConnUser %>" 
    SelectCommand="SELECT DepartmanID, DepartmanAdi, DepartmanKodu, StokDepartman FROM  Departmanlar" 
    DeleteCommand="DELETE FROM Departmanlar WHERE DepartmanID=@DepartmanID" 
    InsertCommand="INSERT INTO Departmanlar(DepartmanAdi,DepartmanKodu,StokDepartman) VALUES (@DepartmanAdi,@DepartmanKodu,@StokDepartman)" 
    UpdateCommand="UPDATE Departmanlar SET DepartmanAdi=@DepartmanAdi, DepartmanKodu=@DepartmanKodu, StokDepartman=@StokDepartman WHERE DepartmanID=@DepartmanID">
    <DeleteParameters>
        <asp:Parameter Name="DepartmanID" Type="Int32" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="DepartmanAdi" Type="String"/>
        <asp:Parameter Name="DepartmanKodu" Type="String"/>
        <asp:Parameter Name="StokDepartman" Type="String" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="DepartmanAdi" Type="String" />
        <asp:Parameter Name="DepartmanKodu" Type="String"/>
        <asp:Parameter Name="StokDepartman" Type="String" />
        <asp:Parameter Name="DepartmanID" Type="Int32"  />
    </UpdateParameters>
                 </asp:SqlDataSource>
             </td>
            </tr>
        </table>
        <table class="auto-style1" style="float:left;">
        <tr>
<td style="width:150px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">Kayıtlı Alt Departmanlar</td>
        </tr>
            <tr>
             <td><dx:ASPxGridView ID="grdAltDepartmanlar" runat="server" AutoGenerateColumns="False" 
                        DataSourceID="sqlAltDepartmanlar" Width="425px" ClientInstanceName="grdAltDepartmanlar" 
                 KeyFieldName="AltDepartmanID">
                        <Columns>
                            <dx:GridViewCommandColumn ButtonType="Image" Caption="İşlemler" VisibleIndex="3">
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
                            <dx:GridViewDataComboBoxColumn FieldName="DepartmanID" Caption="Departman Adı" VisibleIndex="2">
                            <PropertiesComboBox ValueType="System.String" DataSourceID="sqlDepartmanlar"
                            Width="100px" Height="25px" TextField="DepartmanAdi" ValueField="DepartmanID"
                            IncrementalFilteringMode="StartsWith">
                            </PropertiesComboBox>
                            </dx:GridViewDataComboBoxColumn>
                            <dx:GridViewDataTextColumn Caption="Alt Departman Adı" FieldName="AltDepartmanAdi" VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Alt Departman Kodu" FieldName="AltDepartmanKodu" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="AltDepartmanID" FieldName="AltDepartmanID" Visible="False" VisibleIndex="2">
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
            <Summary Text="Sayfa {0} ile {1} Arası ({2} Alt Departmanlar)" />
            <PageSizeItemSettings Caption="Sayfa:">
            </PageSizeItemSettings>
        </SettingsPager>
            <SettingsText EmptyDataRow="Kayıt Bulunamamıştır." />
                    </dx:ASPxGridView>
    <asp:SqlDataSource ID="sqlAltDepartmanlar" runat="server" ConnectionString="<%$ ConnectionStrings:DbConnUser %>" 
    SelectCommand="SELECT AltDepartmanID, DepartmanID, AltDepartmanAdi, AltDepartmanKodu
FROM         AltDepartman" 
    DeleteCommand="DELETE FROM AltDepartmanlar WHERE AltDepartmanID = @AltDepartmanID" 
    InsertCommand="INSERT  INTO AltDepartman(DepartmanID, AltDepartmanAdi, AltDepartmanKodu)
VALUES     (@DepartmanID,@AltDepartmanAdi,@AltDepartmanKodu)" 
    UpdateCommand="UPDATE  AltDepartman SET DepartmanID = @DepartmanID, AltDepartmanAdi = @AltDepartmanAdi, AltDepartmanKodu = @AltDepartmanKodu
WHERE AltDepartmanID= @AltDepartmanID">
    <DeleteParameters>
        <asp:Parameter Name="AltDepartmanID" Type="Int32" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="DepartmanID" Type="Int32"/>
        <asp:Parameter Name="AltDepartmanAdi" Type="String"/>
        <asp:Parameter Name="AltDepartmanKodu" Type="String" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="AltDepartmanID" Type="Int32"  />
        <asp:Parameter Name="AltDepartmanAdi" Type="String" />
        <asp:Parameter Name="AltDepartmanKodu" Type="String"/>
        <asp:Parameter Name="DepartmanID" Type="Int32" />
    </UpdateParameters>
                 </asp:SqlDataSource>
             </td>
            </tr>
        </table></center></fieldset><fieldset style="width:50%;"><center>
        <table class="auto-style1" style="float:left;">
        <tr>
        <td style="width:150px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">
                İhtiyaç Tanımları</td>
        </tr>
            <tr>
             <td><dx:ASPxGridView ID="grdIhtiyaclar" runat="server" AutoGenerateColumns="False" 
                        DataSourceID="sqlihtiyaclar" Width="325px" ClientInstanceName="sqlihtiyaclar" 
                        KeyFieldName="IhtiyacID">
                        <Columns>
                            <dx:GridViewCommandColumn ButtonType="Image" Caption="İşlemler" VisibleIndex="3">
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
                            <dx:GridViewDataTextColumn FieldName="IhtiyacAdi" VisibleIndex="0" Caption="Ihtiyaç Adı">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="IhtiyacID" FieldName="IhtiyacID" Visible="False" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Ihtiyaç Kodu" FieldName="IhtiyacKodu" VisibleIndex="2">
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
            <Summary Text="Sayfa {0} ile {1} Arası ({2} İhtiyaçlar)" />
            <PageSizeItemSettings Caption="Sayfa:">
            </PageSizeItemSettings>
        </SettingsPager>
            <SettingsText EmptyDataRow="Kayıt Bulunamamıştır." />
                    </dx:ASPxGridView>
                 <asp:SqlDataSource ID="sqlihtiyaclar" runat="server" ConnectionString="<%$ ConnectionStrings:DbConnUser %>" 
                     DeleteCommand="DELETE FROM Ihtiyaclar WHERE IhtiyacID=@IhtiyacID" 
                     InsertCommand="INSERT INTO Ihtiyaclar(IhtiyacAdi,IhtiyacKodu) VALUES (@IhtiyacAdi,@IhtiyacKodu)" 
                     SelectCommand="SELECT IhtiyacID, IhtiyacAdi, IhtiyacKodu FROM Ihtiyaclar ORDER BY IhtiyacKodu ASC" 
                     UpdateCommand="UPDATE  Ihtiyaclar SET IhtiyacAdi = @IhtiyacAdi, IhtiyacKodu= @IhtiyacKodu WHERE (IhtiyacID = @IhtiyacID)">
                     <DeleteParameters>
                         <asp:Parameter Name="IhtiyacID" Type="Int32" />
                     </DeleteParameters>
                     <InsertParameters>
                         <asp:Parameter Name="IhtiyacAdi" Type="String" />
                         <asp:Parameter Name="IhtiyacKodu" Type="String" />
                     </InsertParameters>
                     <UpdateParameters>
                         <asp:Parameter Name="IhtiyacAdi" Type="String" />
                         <asp:Parameter Name="IhtiyacKodu" Type="String" />
                         <asp:Parameter Name="IhtiyacID" Type="Int32" />
                     </UpdateParameters>
                 </asp:SqlDataSource>
             </td>
            </tr>
        </table>
        <table class="auto-style1" style="float:left;">
        <tr>
        <td style="width:150px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">
                Ana Departman</td>
        </tr>
            <tr>
             <td><dx:ASPxGridView ID="grdAnaDepartmanlar" runat="server" AutoGenerateColumns="False" 
                        DataSourceID="sqlAnaDepartmanlar" Width="325px" ClientInstanceName="sqlAnaDepartmanlar" 
                        KeyFieldName="AnaDeptID">
                        <Columns>
                            <dx:GridViewCommandColumn ButtonType="Image" Caption="İşlemler" VisibleIndex="3">
                                <EditButton Text="Düzenle" Visible="True">
                                    <Image Url="~/images/edit.png">
                                    </Image>
                                </EditButton>
                                <NewButton Text="Ekle">
                                    <Image Url="~/images/insert.png">
                                    </Image>
                                </NewButton>
                                <DeleteButton Text="Sil">
                                    <Image Url="~/images/delete.png">
                                    </Image>
                                </DeleteButton>
                                <CancelButton Text="Vazgeç">
                                </CancelButton>
                                <UpdateButton Text="Düzenle  |  ">
                                </UpdateButton>
                                <HeaderStyle HorizontalAlign="Center" />
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn FieldName="DepartmanAdi" VisibleIndex="0" Caption="Departman Adı">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="StoksuzID" FieldName="StoksuzID" Visible="False" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Departman Kodu" FieldName="DepartmanKodu" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                        </Columns>
        <SettingsPager PageSize="20">
            <FirstPageButton Text="İlk Sayfa">
            </FirstPageButton>
            <LastPageButton Text="Son Sayfa">
            </LastPageButton>
            <NextPageButton Text="İleri">
            </NextPageButton>
            <PrevPageButton Text="Geri">
            </PrevPageButton>
            <Summary Text="Sayfa {0} ile {1} Arası ({2} Ana Departmanlar)" />
            <PageSizeItemSettings Caption="Sayfa:">
            </PageSizeItemSettings>
        </SettingsPager>
            <SettingsText EmptyDataRow="Kayıt Bulunamamıştır." />
                    </dx:ASPxGridView>
                 <asp:SqlDataSource ID="sqlAnaDepartmanlar" runat="server" ConnectionString="<%$ ConnectionStrings:DbConnUser %>" 
                     SelectCommand="SELECT D.DepartmanAdi,D.DepartmanKodu,AD.AnaDeptID FROM Departmanlar AS D
                                    INNER JOIN AnaDepartman AS AD ON D.DepartmanKodu=AD.AnaDepartmanKodu" 
                     UpdateCommand="UPDATE  AnaDepartman SET AnaDepartmanKodu = @AnaDepartmanKodu WHERE (AnaDeptID = @AnaDeptID)">
                     <InsertParameters>
                         <asp:Parameter Name="AnaDepartmanKodu" Type="String" />
                     </InsertParameters>
                     <UpdateParameters>
                         <asp:Parameter Name="AnaDepartmanKodu" Type="String" />
                         <asp:Parameter Name="AnaDeptID" Type="Int32" />
                     </UpdateParameters>
                 </asp:SqlDataSource>
             </td>
            </tr>
        </table>
            <table class="auto-style1" style="float:left;">
        <tr>
        <td style="width:150px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">
                Link Döviz Referans Türü</td>
        </tr>
            <tr>
             <td><dx:ASPxGridView ID="grdDovizTurleri" runat="server" AutoGenerateColumns="False" 
                        DataSourceID="sqlDovizTurleri" Width="325px" ClientInstanceName="sqlDovizTurleri" 
                        KeyFieldName="DovizID">
                        <Columns>
                            <dx:GridViewCommandColumn ButtonType="Image" Caption="İşlemler" VisibleIndex="3">
                                <EditButton Text="Düzenle" Visible="True">
                                    <Image Url="~/images/edit.png">
                                    </Image>
                                </EditButton>
                                <CancelButton Text="Vazgeç">
                                </CancelButton>
                                <UpdateButton Text="Düzenle  |  ">
                                </UpdateButton>
                                <HeaderStyle HorizontalAlign="Center" />
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn FieldName="DovizReferansTuru" VisibleIndex="0" Caption="Döviz Referans Türü">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="DovizID" FieldName="DovizID" Visible="False" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                        </Columns>
            <SettingsText EmptyDataRow="Kayıt Bulunamamıştır." />
                    </dx:ASPxGridView>
                 <asp:SqlDataSource ID="sqlDovizTurleri" runat="server" ConnectionString="<%$ ConnectionStrings:DbConnUser %>" 
                     SelectCommand="SELECT DovizID, DovizReferansTuru FROM DovizTurleri" 
                     UpdateCommand="UPDATE DovizTurleri SET DovizReferansTuru=@DovizReferansTuru WHERE DovizID=@DovizID">
                     <UpdateParameters>
                         <asp:Parameter Name="DovizReferansTuru" Type="String" />
                         <asp:Parameter Name="DovizID" Type="Int32" />
                     </UpdateParameters>
                 </asp:SqlDataSource>
             </td>
            </tr>
        </table>
        <table class="auto-style1" style="float:left;">
        <tr>
        <td style="width:150px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">
                Temin Yerleri</td>
        </tr>
            <tr>
             <td><dx:ASPxGridView ID="grdTeminYeri" runat="server" AutoGenerateColumns="False" 
                        DataSourceID="sqlTeminYeri" Width="325px" ClientInstanceName="sqlTeminYeri" 
                        KeyFieldName="TeminID">
                        <Columns>
                            <dx:GridViewCommandColumn ButtonType="Image" Caption="İşlemler" VisibleIndex="3">
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
                            <dx:GridViewDataTextColumn FieldName="TeminAdi" VisibleIndex="0" Caption="Temin Yeri Adi">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="TeminID" FieldName="TeminID" Visible="False" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Temin Yeri Kodu" FieldName="TeminKodu" VisibleIndex="2">
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
            <Summary Text="Sayfa {0} ile {1} Arası ({2} Temin Yerleri)" />
            <PageSizeItemSettings Caption="Sayfa:">
            </PageSizeItemSettings>
        </SettingsPager>
            <SettingsText EmptyDataRow="Kayıt Bulunamamıştır." />
                    </dx:ASPxGridView>
                 <asp:SqlDataSource ID="sqlTeminYeri" runat="server" ConnectionString="<%$ ConnectionStrings:DbConnUser %>" 
                     DeleteCommand="DELETE FROM TeminYeri WHERE TeminID=@TeminID" 
                     InsertCommand="INSERT INTO TeminYeri(TeminAdi,TeminKodu) VALUES (@TeminAdi,@TeminKodu)" 
                     SelectCommand="SELECT TeminID,TeminAdi,TeminKodu FROM TeminYeri ORDER BY  TeminID DESC" 
                     UpdateCommand="UPDATE  TeminYeri SET TeminAdi = @TeminAdi, TeminKodu = @TeminKodu WHERE (TeminID = @TeminID)">
                     <DeleteParameters>
                         <asp:Parameter Name="TeminID" Type="Int32" />
                     </DeleteParameters>
                     <InsertParameters>
                         <asp:Parameter Name="TeminAdi" Type="String" />
                         <asp:Parameter Name="TeminKodu" Type="String" />
                     </InsertParameters>
                     <UpdateParameters>
                         <asp:Parameter Name="TeminAdi" Type="String" />
                         <asp:Parameter Name="TeminKodu" Type="String" />
                         <asp:Parameter Name="TeminID" Type="Int32" />
                     </UpdateParameters>
                 </asp:SqlDataSource>
             </td>
            </tr>
        </table>
        <table class="auto-style1" style="float:left;">
        <tr>
        <td style="width:150px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">
                Red Sebepleri</td>
        </tr>
            <tr>
             <td><dx:ASPxGridView ID="grdRedSebepleri" runat="server" AutoGenerateColumns="False" 
                        DataSourceID="sqlRedNedeni" Width="325px" ClientInstanceName="sqlRedNedeni" 
                        KeyFieldName="RedID">
                        <Columns>
                            <dx:GridViewCommandColumn ButtonType="Image" Caption="İşlemler" VisibleIndex="3">
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
                            <dx:GridViewDataTextColumn FieldName="RedSebebi" VisibleIndex="0" Caption="Red Sebebi">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="RedID" FieldName="RedID" Visible="False" VisibleIndex="1">
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
            <Summary Text="Sayfa {0} ile {1} Arası ({2} Red Sebepleri)" />
            <PageSizeItemSettings Caption="Sayfa:">
            </PageSizeItemSettings>
        </SettingsPager>
            <SettingsText EmptyDataRow="Kayıt Bulunamamıştır." />
                    </dx:ASPxGridView>
                 <asp:SqlDataSource ID="sqlRedNedeni" runat="server" ConnectionString="<%$ ConnectionStrings:DbConnUser %>" 
                     DeleteCommand="DELETE FROM RedNedenleri WHERE  RedID=@RedID" 
                     InsertCommand="INSERT INTO RedNedenleri(RedSebebi) VALUES (@RedSebebi)" 
                     SelectCommand="SELECT RedID, RedSebebi FROM RedNedenleri ORDER BY  RedID DESC" 
                     UpdateCommand="UPDATE   RedNedenleri SET RedSebebi = @RedSebebi WHERE  (RedID= @RedID)">
                     <DeleteParameters>
                         <asp:Parameter Name="RedID" Type="Int32" />
                     </DeleteParameters>
                     <InsertParameters>
                         <asp:Parameter Name="RedSebebi" Type="String" />
                     </InsertParameters>
                     <UpdateParameters>
                         <asp:Parameter Name="RedSebebi" Type="String" />
                         <asp:Parameter Name="RedID" Type="Int32" />
                     </UpdateParameters>
                 </asp:SqlDataSource>
             </td>
            </tr>
        </table><table class="auto-style1" style="float:left;">
        <tr>
        <td style="width:130px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">
               Yetki Ünvanları</td>
        </tr>
            <tr>
             <td><dx:ASPxGridView ID="grdYetkiler" runat="server" AutoGenerateColumns="False" 
                        DataSourceID="sqlYetkiler" Width="325px" ClientInstanceName="grdYetkiler" 
                 KeyFieldName="YetkiID">
                        <Columns>
                            <dx:GridViewCommandColumn ButtonType="Image" Caption="İşlemler" VisibleIndex="18">
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
                            <dx:GridViewDataTextColumn FieldName="Yetki_Unvani" VisibleIndex="0" Caption="Yetki Ünvanı" Width="50px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="YetkiID" FieldName="YetkiID" Visible="False" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Yetki Kodu" FieldName="YetkiKodu" VisibleIndex="2" Width="50px">
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
            <Summary Text="Sayfa {0} ile {1} Arası ({2} Yetkiler)" />
            <PageSizeItemSettings Caption="Sayfa:">
            </PageSizeItemSettings>
        </SettingsPager>
            <SettingsText EmptyDataRow="Kayıt Bulunamamıştır." />
                    </dx:ASPxGridView>
                 <asp:SqlDataSource ID="sqlYetkiler" runat="server" ConnectionString="<%$ ConnectionStrings:DbConnUser %>" 
                     DeleteCommand="DELETE FROM Yetki_Unvanlari WHERE YetkiID=@YetkiID" 
                     InsertCommand="INSERT INTO Yetki_Unvanlari(Yetki_Unvani,YetkiKodu) 
                     VALUES (@Yetki_Unvani,@YetkiKodu)" 
                     SelectCommand="SELECT YetkiID, Yetki_Unvani,YetkiKodu
                     FROM Yetki_Unvanlari" 
                     UpdateCommand="UPDATE  Yetki_Unvanlari SET 
                                    Yetki_Unvani = @Yetki_Unvani, 
                                    YetkiKodu= @YetkiKodu
                                    WHERE (YetkiID = @YetkiID)">
                     <DeleteParameters>
                         <asp:Parameter Name="YetkiID" Type="Int32" />
                     </DeleteParameters>
                     <InsertParameters>
                         <asp:Parameter Name="Yetki_Unvani" Type="String" />
                         <asp:Parameter Name="YetkiKodu" Type="String" />
                     </InsertParameters>
                     <UpdateParameters>
                         <asp:Parameter Name="Yetki_Unvani" Type="String" />
                         <asp:Parameter Name="YetkiKodu" Type="String" />
                         <asp:Parameter Name="YetkiID" Type="Int32" />
                     </UpdateParameters>
                 </asp:SqlDataSource>
        </td></tr></table>
            <table class="auto-style1" style="float:left;">
        <tr>
        <td style="width:150px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">
                Evrak Numaraları</td>
        </tr>
            <tr>
             <td><dx:ASPxGridView ID="grdEvrakNumaralari" runat="server" AutoGenerateColumns="False" 
                        DataSourceID="sqlEvrakNo" Width="550px" ClientInstanceName="sqlEvrakNo" 
                        KeyFieldName="EvrakID">
                        <Columns>
                            <dx:GridViewCommandColumn ButtonType="Image" Caption="İşlemler" VisibleIndex="5">
                                <EditButton Text="Düzenle" Visible="True">
                                    <Image Url="~/images/edit.png">
                                    </Image>
                                </EditButton>
                                <CancelButton Text="Vazgeç">
                                </CancelButton>
                                <UpdateButton Text="Düzenle  |  ">
                                </UpdateButton>
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn FieldName="EvrakID" VisibleIndex="0" ReadOnly="True">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="CikisHareketiSeri" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="CikisHareketNo" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="SiparisNoSeri" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="SiparisNo" VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                        </Columns>
            <SettingsText EmptyDataRow="Kayıt Bulunamamıştır." />
                    </dx:ASPxGridView>
                 <asp:SqlDataSource ID="sqlEvrakNo" runat="server" ConnectionString="<%$ ConnectionStrings:DbConnUser %>" 
                     SelectCommand="SELECT EvrakID, CikisHareketiSeri,CikisHareketNo, SiparisNoSeri, SiparisNo
                                    FROM EvrakNumaralari
                                    ORDER BY  EvrakID DESC" 
                     UpdateCommand="UPDATE   EvrakNumaralari SET 
                                    CikisHareketiSeri = @CikisHareketiSeri,
                                    CikisHareketNo = @CikisHareketNo,
                                    SiparisNoSeri = @SiparisNoSeri,
                                    SiparisNo = @SiparisNo
                                    WHERE  (EvrakID= @EvrakID)">
                     <UpdateParameters>
                         <asp:Parameter Name="CikisHareketiSeri" Type="String" />
                         <asp:Parameter Name="CikisHareketNo" Type="Int32" />
                         <asp:Parameter Name="SiparisNoSeri" Type="String" />
                         <asp:Parameter Name="SiparisNo" Type="Int32" />
                         <asp:Parameter Name="EvrakID" Type="Int32" />
                     </UpdateParameters>
                 </asp:SqlDataSource>
             </td>
            </tr>
        </table>
            <br< />
            <table class="auto-style1" style="float:left;">
        <tr>
        <td style="width:150px; text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">
                Gidecek Departmanlar</td>
        </tr>
            <tr>
             <td><dx:ASPxGridView ID="grdGidecekDepartmanlar" runat="server" AutoGenerateColumns="False" 
                        DataSourceID="SqlGidecekDepartmanlar" Width="550px" ClientInstanceName="SqlGidecekDepartmanlar" 
                        KeyFieldName="DeptID">
                        <Columns>
                            <dx:GridViewCommandColumn ButtonType="Image" Caption="İşlemler" VisibleIndex="4">
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
                                <UpdateButton Text="Düzenle  | ">
                                </UpdateButton>
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn FieldName="DeptID" VisibleIndex="0" Visible="false">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="GidecekDepartman" Caption="Gidecek Departman Bölümü" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="GidecekKod" Caption="Gidecek Departman Kodu" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="ID" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                        </Columns>
            <SettingsText EmptyDataRow="Kayıt Bulunamamıştır." />
                    </dx:ASPxGridView>
                 <asp:SqlDataSource ID="SqlGidecekDepartmanlar" runat="server" ConnectionString="<%$ ConnectionStrings:DbConnUser %>" 
                     SelectCommand="SELECT DeptID,GidecekDepartman,GidecekKod,ID FROM GidecekDepartman
ORDER BY ID DESC" 
                     UpdateCommand="UPDATE    GidecekDepartman SET              
GidecekDepartman = @GidecekDepartman,
GidecekKod = @GidecekKod,
ID = @ID
WHERE     (DeptID = @DeptID)" DeleteCommand="DELETE FROM GidecekDepartman
WHERE     (DeptID = @DeptID)" InsertCommand="INSERT     INTO            GidecekDepartman(GidecekDepartman, GidecekKod, ID)
VALUES     (@GidecekDepartman,@GidecekKod,@ID)">
                     <DeleteParameters>
                         <asp:Parameter Name="DeptID" />
                     </DeleteParameters>
                     <InsertParameters>
                         <asp:Parameter Name="GidecekDepartman" />
                         <asp:Parameter Name="GidecekKod" />
                         <asp:Parameter Name="ID" />
                     </InsertParameters>
                     <UpdateParameters>
                         <asp:Parameter Name="GidecekDepartman" />
                         <asp:Parameter Name="GidecekKod" />
                         <asp:Parameter Name="ID" />
                         <asp:Parameter Name="DeptID" />
                     </UpdateParameters>
                 </asp:SqlDataSource>
             </td>
            </tr>
        </table>
    <div style="float:left; width:500px;">
        <p style="text-align:center; width:525px; font-size:14px; font-family:Calibri; background-color:gray; color:white;">
                Masraf Merkezi</p><div style="padding-top:-30px;">
        <dx:ASPxGridView ID="grdMasrafMerkezi" runat="server" AutoGenerateColumns="False" 
                        DataSourceID="sqlMasrafMerkezi" Width="320px" ClientInstanceName="sqlMasrafMerkezi" 
                        KeyFieldName="MasrafMerkeziID">
                        <Columns>
                            <dx:GridViewCommandColumn ButtonType="Image" Caption="İşlemler" VisibleIndex="5" Width="150px">
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
                            <dx:GridViewDataTextColumn FieldName="MasrafMerkeziID" VisibleIndex="0" ReadOnly="True" Visible="False">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="MasrafMerkeziKodu" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataComboBoxColumn FieldName="LinkMasrafKodu" Caption="Link Masraf Kodu" VisibleIndex="2">
                            <PropertiesComboBox ValueType="System.String" DataSourceID="sqlLinkMasrafKodu"
                             Height="25px" TextField="LinkMasrafKodu" ValueField="LinkMasrafKodu"
                            IncrementalFilteringMode="StartsWith" TextFormatString="{0}">
                            </PropertiesComboBox>
                            </dx:GridViewDataComboBoxColumn>
                            <dx:GridViewDataTextColumn FieldName="MasrafMerkeziAdi" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataComboBoxColumn FieldName="LinkMasrafAdi" Caption="Link Masraf Adı" VisibleIndex="4">
                            <PropertiesComboBox ValueType="System.String" DataSourceID="sqlLinkMasrafKodu"
                            Height="25px" TextField="LinkMasrafAdi" ValueField="LinkMasrafAdi"
                            IncrementalFilteringMode="StartsWith" TextFormatString="{1}">
                            </PropertiesComboBox>
                            </dx:GridViewDataComboBoxColumn>
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
            <Summary Text="Sayfa {0} ile {1} Arası ({2} Masraf Merkezi)" />
            <PageSizeItemSettings Caption="Sayfa:">
            </PageSizeItemSettings>
        </SettingsPager>
            <SettingsText EmptyDataRow="Kayıt Bulunamamıştır." />
                    </dx:ASPxGridView>
                 <asp:SqlDataSource ID="sqlMasrafMerkezi" runat="server" ConnectionString="<%$ ConnectionStrings:DbConnUser %>" 
                     SelectCommand="SELECT MasrafMerkeziID,MasrafMerkeziKodu,LinkMasrafMerkeziKodu AS LinkMasrafKodu,
                        MasrafMerkeziAdi,LinkMasrafMerkeziAdi AS LinkMasrafAdi
                        FROM MasrafMerkezi
                        ORDER BY MasrafMerkeziID DESC" 
                     DeleteCommand="DELETE FROM MasrafMerkezi WHERE MasrafMerkeziID=@MasrafMerkeziID" 
                     InsertCommand="INSERT INTO MasrafMerkezi(MasrafMerkeziKodu, MasrafMerkeziAdi, LinkMasrafMerkeziKodu, 
                     LinkMasrafMerkeziAdi)
                        VALUES     (@MasrafMerkeziKodu,@MasrafMerkeziAdi,@LinkMasrafKodu,@LinkMasrafAdi)" 
                     UpdateCommand="UPDATE  MasrafMerkezi
                        SET 
                        MasrafMerkeziKodu = @MasrafMerkeziKodu, 
                        MasrafMerkeziAdi = @MasrafMerkeziAdi, 
                        LinkMasrafMerkeziKodu = @LinkMasrafKodu, 
                        LinkMasrafMerkeziAdi = @LinkMasrafAdi
                        WHERE MasrafMerkeziID=@MasrafMerkeziID">
                     <DeleteParameters>
                         <asp:Parameter Name="MasrafMerkeziID" Type="Int32" />
                     </DeleteParameters>
                     <InsertParameters>
                         <asp:Parameter Name="MasrafMerkeziKodu" Type="String" />
                         <asp:Parameter Name="MasrafMerkeziAdi" Type="String" />
                         <asp:Parameter Name="LinkMasrafKodu" Type="String" />
                         <asp:Parameter Name="LinkMasrafAdi" Type="String" />
                     </InsertParameters>
                     <UpdateParameters>
                         <asp:Parameter Name="MasrafMerkeziKodu" Type="String" />
                         <asp:Parameter Name="MasrafMerkeziAdi" Type="String" />
                         <asp:Parameter Name="LinkMasrafKodu" Type="String" />
                         <asp:Parameter Name="LinkMasrafAdi" Type="String" />
                         <asp:Parameter Name="MasrafMerkeziID" Type="Int32"  />
                     </UpdateParameters>
                 </asp:SqlDataSource>
                 <asp:SqlDataSource ID="sqlLinkMasrafKodu" runat="server" ConnectionString="<%$ ConnectionStrings:DbConnLink %>" 
                     SelectCommand="SELECT MHS007_MasrafMerkeziKodu AS LinkMasrafKodu,MHS007_MMAdi AS LinkMasrafAdi
FROM MHS007 WITH(NOLOCK) 
ORDER BY MHS007_MasrafMerkeziKodu Desc">
                 </asp:SqlDataSource></div></div> 
        </fieldset></center><center>
            <fieldset style="width:95%;">
                <table width="100%" border="0">
  <tr>
        <td style="text-align:center; font-size:14px; font-family:Calibri; background-color:gray; color:white;">
                Menü Tanımları</td>
       </tr>
</table><dx:ASPxGridView ID="grdMenuKontrolu" runat="server" AutoGenerateColumns="False" 
                        DataSourceID="sqlMenuKontrolu" ClientInstanceName="sqlMenuKontrolu" 
                        KeyFieldName="MenuID" Width="100%">
                        <Columns>
                            <dx:GridViewCommandColumn Caption="İşlemler" VisibleIndex="19" ButtonType="Image">
                                <EditButton Text="Düzenle | " Visible="True">
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
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn FieldName="MenuID" ReadOnly="True" VisibleIndex="0" Visible="false">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="MenuKodu" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataCheckColumn FieldName="Anasayfa" VisibleIndex="2">
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewDataCheckColumn FieldName="StokKartiArama" VisibleIndex="3">
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewDataCheckColumn FieldName="ihtiyacArama" VisibleIndex="4">
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewDataCheckColumn FieldName="TalepAcma" VisibleIndex="5">
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewDataCheckColumn FieldName="Formlar" VisibleIndex="6">
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewDataCheckColumn FieldName="idariKisim" VisibleIndex="7">
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewDataCheckColumn FieldName="TeknikKisim" VisibleIndex="8">
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewDataCheckColumn FieldName="ihtiyacPusulasi" VisibleIndex="9">
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewDataCheckColumn FieldName="StokCikisFormu" VisibleIndex="10">
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewDataCheckColumn FieldName="StokCikisHareketi" VisibleIndex="11">
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewDataCheckColumn FieldName="SiparisFormu" VisibleIndex="12">
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewDataCheckColumn FieldName="Fax" VisibleIndex="13">
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewDataCheckColumn FieldName="Kullanicilar" VisibleIndex="14">
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewDataCheckColumn FieldName="Tanimlamalar" VisibleIndex="15">
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewDataCheckColumn FieldName="StokTanimlama" VisibleIndex="16">
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewDataCheckColumn FieldName="Arama" VisibleIndex="17">
                            </dx:GridViewDataCheckColumn>
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
            <Summary Text="Sayfa {0} ile {1} Arası ({2} Menü Kontrolü)" />
            <PageSizeItemSettings Caption="Sayfa:">
            </PageSizeItemSettings>
        </SettingsPager>
            <SettingsText EmptyDataRow="Kayıt Bulunamamıştır." />
                    </dx:ASPxGridView>
                 <asp:SqlDataSource ID="sqlMenuKontrolu" runat="server" ConnectionString="<%$ ConnectionStrings:DbConnUser %>" 
                     SelectCommand="SELECT MenuID,MenuKodu,Anasayfa,StokKartiArama,ihtiyacArama,TalepAcma,Formlar,
idariKisim,TeknikKisim,ihtiyacPusulasi,StokCikisFormu,StokCikisHareketi,SiparisFormu,
Fax,Kullanicilar,Tanimlamalar,StokTanimlama,Arama 
FROM MenuKontrolu
ORDER BY MenuID ASC" DeleteCommand="DELETE FROM MenuKontrolu WHERE MenuID=@MenuID" 
                     InsertCommand="INSERT  INTO MenuKontrolu(MenuKodu, Anasayfa, StokKartiArama, ihtiyacArama, TalepAcma, Formlar, idariKisim, TeknikKisim, ihtiyacPusulasi, StokCikisFormu, StokCikisHareketi,SiparisFormu, 
                      Fax,Kullanicilar, Tanimlamalar,StokTanimlama,Arama)
VALUES     (@MenuKodu,@Anasayfa,@StokKartiArama,@ihtiyacArama,@TalepAcma,@Formlar,@idariKisim,@TeknikKisim,@ihtiyacPusulasi,@StokCikisFormu,@StokCikisHareketi,@SiparisFormu,@Fax,@Kullanicilar,@Tanimlamalar,@StokTanimlama,@Arama)" 
                     UpdateCommand="UPDATE    MenuKontrolu  SET             
 MenuKodu = @MenuKodu, 
Anasayfa = @Anasayfa, 
StokKartiArama = @StokKartiArama, 
ihtiyacArama = @ihtiyacArama, 
TalepAcma = @TalepAcma, 
Formlar = @Formlar, 
idariKisim = @idariKisim, 
TeknikKisim = @TeknikKisim, 
ihtiyacPusulasi = @ihtiyacPusulasi, 
StokCikisFormu = @StokCikisFormu, 
StokCikisHareketi = @StokCikisHareketi,
SiparisFormu = @SiparisFormu,
Fax =@Fax,
Kullanicilar = @Kullanicilar, 
Tanimlamalar = @Tanimlamalar,
StokTanimlama = @StokTanimlama,
Arama = @Arama
WHERE MenuID=@MenuID">
                     <DeleteParameters>
                         <asp:Parameter Name="MenuID" Type="Int32" />
                     </DeleteParameters>
                     <InsertParameters>
                         <asp:Parameter Name="MenuKodu" Type="String" />
                         <asp:Parameter Name="Anasayfa" Type="Boolean"/>
                         <asp:Parameter Name="StokKartiArama" Type="Boolean" />
                         <asp:Parameter Name="ihtiyacArama" Type="Boolean" />
                         <asp:Parameter Name="TalepAcma" Type="Boolean" />
                         <asp:Parameter Name="Formlar" Type="Boolean" />
                         <asp:Parameter Name="idariKisim" Type="Boolean" />
                         <asp:Parameter Name="TeknikKisim" Type="Boolean" />
                         <asp:Parameter Name="ihtiyacPusulasi" Type="Boolean" />
                         <asp:Parameter Name="StokCikisFormu" Type="Boolean" />
                         <asp:Parameter Name="StokCikisHareketi" Type="Boolean" />
                         <asp:Parameter Name="SiparisFormu" />
                         <asp:Parameter Name="Fax" Type="Boolean" />
                         <asp:Parameter Name="Kullanicilar" Type="Boolean" />
                         <asp:Parameter Name="Tanimlamalar" Type="Boolean" />
                         <asp:Parameter Name="StokTanimlama" />
                         <asp:Parameter Name="Arama" Type="Boolean" />
                     </InsertParameters>
                     <UpdateParameters>
                         <asp:Parameter Name="MenuKodu" Type="String" />
                         <asp:Parameter Name="Anasayfa" Type="Boolean" />
                         <asp:Parameter Name="StokKartiArama" Type="Boolean" />
                         <asp:Parameter Name="ihtiyacArama" Type="Boolean" />
                         <asp:Parameter Name="TalepAcma" Type="Boolean" />
                         <asp:Parameter Name="Formlar" Type="Boolean" />
                         <asp:Parameter Name="idariKisim" Type="Boolean" />
                         <asp:Parameter Name="TeknikKisim" Type="Boolean" />
                         <asp:Parameter Name="ihtiyacPusulasi" Type="Boolean" />
                         <asp:Parameter Name="StokCikisFormu" Type="Boolean" />
                         <asp:Parameter Name="StokCikisHareketi" Type="Boolean" />
                         <asp:Parameter Name="SiparisFormu" />
                         <asp:Parameter Name="Fax" Type="Boolean" />
                         <asp:Parameter Name="Kullanicilar" Type="Boolean" />
                         <asp:Parameter Name="Tanimlamalar" Type="Boolean" />
                         <asp:Parameter Name="StokTanimlama" />
                         <asp:Parameter Name="Arama" Type="Boolean" />
                         <asp:Parameter Name="MenuID" Type="Int32"  />
                     </UpdateParameters>
                 </asp:SqlDataSource>
             
            </fieldset></center>

</asp:Content>



