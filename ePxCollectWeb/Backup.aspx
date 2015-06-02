<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Backup.aspx.cs" Inherits="ePxCollectWeb.Backup" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div>
        <table cellpadding="10" cellspacing="10" width="90%" align="center">
            <tr>
                <td style="height: 35px; font-weight: bold; font-size: 16pt; font-family: Verdana; color:#2A75A9"
                    align="center">Backup SQL Server DataBase  
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Button ID="btnBackup" runat="server" Text="Backup DataBase" OnClick="btnBackup_Click"  CssClass="button" Width="120px"/>
                </td>

            </tr>
        </table>
    </div>
</asp:Content>
