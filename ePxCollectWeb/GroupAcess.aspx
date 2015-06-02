<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupAcess.aspx.cs" Inherits="ePxCollectWeb.GroupAcess" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Style/style.css" rel="stylesheet" />

    <title>Analysis Access Group Details</title>
 

</head>
<body>
    <form id="form1" runat="server">

        <div style="width: 1200px; background: #006699; height: 25px; color: White; vertical-align: top;">
            <h4 style="font-family: Verdana; font-size: 11px; margin-left: 10px;">Analysis Access Details</h4>
        </div>
        <div style="width: 100px;">
            <asp:GridView ID="grdAccess" runat="server" BackColor="White" BorderColor="#CCCCCC" ShowFooter="true" Width="1200px" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" AllowPaging="true" PageSize="10" HeaderStyle-Width="200px" OnPageIndexChanging="grdAccess_PageIndexChanging">
                <FooterStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />

                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" Height="30px" />
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                <RowStyle ForeColor="#000066" />
                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#00547E" />
                <EmptyDataTemplate>
                    No Data Found.
                </EmptyDataTemplate>
            </asp:GridView>


        </div>
        <br />
<%--        <div style="margin-left: 500px;">
            <asp:Button ID="btnClose" runat="server" OnClick="btnClose_Click" Style="height: 26px" Text="Close" CssClass="button" OnClientClick="CloseDialog();return false;" />
        </div>--%>
    </form>
   
</body>
</html>
