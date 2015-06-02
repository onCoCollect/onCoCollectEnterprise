<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnalysisResultFilter.aspx.cs" Inherits="ePxCollectWeb.AnalysisResultFilter" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
                 </asp:ScriptManager>
    <div>
    <asp:UpdatePanel runat="server" ID="updConfirm" UpdateMode="Conditional">
        <ContentTemplate>
         
                <asp:Panel ID="pnlFilter" runat="server" Height="250px" Width="500px" BackColor="white"
                    BorderColor="Black" Style="border: 1px solid;">
                    <div style="width: 100%; background: #fbbb23; height: 25px; color: White;">
                        <b>Filter</b></div>
                    <asp:DropDownList ID="dpColumns" runat="server" Width="191px" Height="18px" OnSelectedIndexChanged="dpColumns_SelectedIndexChanged" AutoPostBack="true" >
                    </asp:DropDownList>
                    <asp:DropDownList ID="dpOperator" runat="server">
                        <asp:ListItem>=</asp:ListItem>
                        <asp:ListItem>&lt;&gt;</asp:ListItem>
                        <asp:ListItem>Contains</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="txtValue" runat="server" Width="170px"></asp:TextBox>
                    <asp:Button ID="btnApply" runat="server" Text="Filter" 
                        onclick="btnApply_Click" />
                    <asp:Panel ID="Panel1" Height="400px" runat="server" Style="overflow: scroll;" Wrap="False">
                     <asp:ListBox ID="grdCols" runat="server" AutoPostBack="True" 
                            onselectedindexchanged="grdCols_SelectedIndexChanged2"></asp:ListBox> <br />
                        <asp:Button ID="btnNo" runat="server" Text="Cancel" Width="100px" CssClass="button" OnClientClick="window.close();" />
                       
                    </asp:Panel>
                </asp:Panel>
            
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
