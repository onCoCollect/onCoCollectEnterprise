<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="ViewAuditLog.aspx.cs" Inherits="ePxCollectWeb.ViewAuditLog" %>
<%@ Register Assembly="SlimeeLibrary" Namespace="SlimeeLibrary" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <style type="text/css">
        DIV.dd_chk_select
        {
            width: 250px;
            top: 0px;
            left: 0px;
        }
        DIV.dd_chk_select DIV#caption
        {
            display:inline-block;
            position:relative;
            width:170px;
            width:235px;
            overflow:hidden;   
             
        }
    
        .style1
        {
            width: 245px;
        }
        .style2
        {
            width: 250px;
        }
        .style3
        {
            width: 250px;
        }
    
        .style4
        {
            width: 57px;
        }
    
    </style>
Select columns to show :
 <asp:DropDownCheckBoxes ID="ddlColumns" runat="server" AutoPostBack="true" UseSelectAllNode="true" class="inputText"  onselectedindexchanged="ddlColumns_SelectedIndexChanged" Width="250px" UseButtons="True"> 
</asp:DropDownCheckBoxes>
<asp:Label ID="lblPID" runat="server"> </asp:Label>
                                                                    
    <asp:UpdatePanel runat="server" ID="updREsult" UpdateMode="Conditional">
    <ContentTemplate>
            
            &nbsp;<asp:Panel ID="GridViewPanel" runat="server" Style="overflow:scroll;" Wrap="False">
            <%-- <asp:GridView ID="grdAnalysisRes" runat="server" AllowPaging="True" BackColor="White"
                    BorderColor="#CC9966" BorderStyle="None" PagerSettings-Position="TopAndBottom"  RowStyle-Wrap="false" OnPageIndexChanging="grdAnalysisRes_PageIndexChanging">
                     <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" 
                        Wrap="False" /> <PagerStyle CssClass="pagination" />
                    </asp:GridView>--%>
            <center>
                <asp:GridView  ID="grdAuditRes" runat="server" AllowPaging="True" BackColor="White"
                    BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" 
                    OnPageIndexChanging="grdAuditRes_PageIndexChanging" 
                    PagerSettings-Position="TopAndBottom" Font-Size="10pt" AllowSorting="True" 
                    PageSize="15"  >
                    <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" 
                        Wrap="False" Font-Size="8pt" />
                    <PagerSettings Position="TopAndBottom" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Left" CssClass="pagination" />
                    <RowStyle BackColor="White" ForeColor="#330099" />
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                    <SortedAscendingCellStyle BackColor="#FEFCEB" />
                    <SortedAscendingHeaderStyle BackColor="#AF0101" />
                    <SortedDescendingCellStyle BackColor="#F6F0C0" />
                    <SortedDescendingHeaderStyle BackColor="#7E0000" />
                </asp:GridView>
            </center>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
