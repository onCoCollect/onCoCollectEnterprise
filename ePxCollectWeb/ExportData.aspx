<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportData.aspx.cs" Inherits="ePxCollectWeb.ExportData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Export to Excel</title>
    <link href="Style/styleNew.css" rel="stylesheet" />
    <link href="Style/StyleSheet.css" rel="stylesheet" />
    <link href="Style/style.css" rel="stylesheet" />
   <base target='_self' />
</head>
<body>
    <form id="form1" runat="server">
    <div>
         <script type="text/javascript">
             window.onunload = OnClose;
       function CloseDialog() {
           window.close();
       }
       window.onload = function () {
           if (window.opener != null && !window.opener.closed) {
               window.opener.LoadModalDiv();
           }
       };
       function OnClose() {
           if (window.opener != null && !window.opener.closed) {
               window.opener.HideModalDiv();
           }
       }

    </script>
                <asp:Panel ID="pnlConfirm" runat="server" Height="300px" Width="380px" BackColor="white" 
                BorderColor="Black" Style="border: 1px solid;">
                <div style="width: 100%; background: #006699; height: 25px; color: White">
                    <h4 style="font-family: Verdana; font-size: 11px; margin-left: 10px;">Export to Excel</h4>
                </div>
               
                <%--  <table align="center" style="margin-top: 20px; color: Black;" width="450px">
                    <tr>
                    </tr>
                    <tr>
                   
                    <tr>
                        <td colspan="2" style="display: inline-flex; font-size: 11px; font-family: Verdana; ">--%>
                <div style="margin-left: 20px; margin-top: 10px; font-family: Verdana; font-size: 11px; width: 350px;">
                    <fieldset>
                        <legend style="font-style: italic; font-family: Verdana; margin-left: 30px;">Export Options</legend>
                        <asp:RadioButtonList ID="rdExportType" runat="server" AutoPostBack="true"   OnSelectedIndexChanged="rdExportType_SelectedIndexChanged" OnClientClick="JavaScript: return false;"  >
                            <asp:ListItem Value="export" Selected="True">Export Data</asp:ListItem>
                            <asp:ListItem Value="WithGrouping">Export with Grouping</asp:ListItem>
                            <asp:ListItem Value="WithCoding" Enabled="false">Export with Grouping and Coding for Multivariate Analysis</asp:ListItem>
                            
                        </asp:RadioButtonList>

                    </fieldset>

                </div>
                 <div style="margin-left: 20px; margin-top: 10px; width: 300px;">
                    <b style="font-family:Verdana;font-size:11px;">Select Template :</b>
                        
                            <asp:DropDownList ID="ddlTemplates" runat="server" CssClass="dllCss" OnSelectedIndexChanged="ddlTemplates_SelectedIndexChanged" ></asp:DropDownList>
                </div>
                <br />
                <div style="margin-left: 20px; font-family: Verdana; font-size: 11px; color: red;">

                    <asp:Label ID="lblExportMessage" runat="server" Text=""></asp:Label>
                </div>
                <%--  </td>
                    </tr--%>
                <%--  <tr>
                        <td> &nbsp;
                        </td>
                   <%-- </tr>OnClientClick="return ValidateDropDown();return false;"--%>
                <%-- <tr>
                        <td colspan="2" align="center">
                --%>

                <div style="margin-left: 20px; margin-top: 10px;">
                    <asp:Button ID="btnYes" runat="server" Text="OK" Width="50px" OnClick="btnYes_Click" CssClass="button" />
                    &nbsp;
                            <asp:Button ID="btnNo" runat="server" Text="Close" Width="50px" CssClass="button"  OnClick="btnNo_Click" OnClientClick="CloseDialog();return false;"   />

                </div>
            </asp:Panel>
    </div>
    </form>
</body>
</html>
