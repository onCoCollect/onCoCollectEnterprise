<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrequencyPopup.aspx.cs" Inherits="ePxCollectWeb.FrequencyPopup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Frequency</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
        <asp:GridView ID="freView" runat="server" CellPadding="4" BorderColor="#CCCCCC"  PageSize="21" BorderStyle="Solid" BorderWidth="1px"  Width="590px" AllowPaging="True" AutoGenerateColumns="False" OnPageIndexChanging="IndexChanging" BackColor="White"  Font-Names="Verdana" Font-Size="11px"  >
            <FooterStyle BackColor="#006699" ForeColor="#003399" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" Width="25%" Font-Names="Verdana" />
            <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
            <RowStyle ForeColor="#000066" />
            <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
            <SortedAscendingCellStyle BackColor="#EDF6F6" />
            <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
            <SortedDescendingCellStyle BackColor="#D6DFDF" />
            <SortedDescendingHeaderStyle BackColor="#002876" />
           
        </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
