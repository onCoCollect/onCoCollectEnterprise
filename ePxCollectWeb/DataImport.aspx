<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="True" CodeBehind="DataImport.aspx.cs" Inherits="ePxCollectWeb.DataImport" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div>
        <table cellpadding="10" cellspacing="10" width="90%" align="center">
            <tr>
                <td style="height: 35px; font-weight: bold; font-size: 16pt; font-family: Verdana; color: #2A75A9" colspan="2"
                    align="center">Import Data 
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label5" runat="server" Text="Connection String :" Font-Bold="True"></asp:Label></td>
                <td>
                    <asp:TextBox ID="txtConnString" runat="server" CssClass="dynamictext"></asp:TextBox>
                    <asp:Button ID="btnGetConnection" runat="server" CssClass="button" Text="Test Connection" OnClick="btnGetConnection_Click" />
                </td>

            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label1" runat="server" Text="Import Query :" Font-Bold="True"></asp:Label></td>
                <td>
                    <asp:TextBox ID="txtQuery" runat="server" CssClass="dynamictext"></asp:TextBox>

                </td>

            </tr>
            <tr>
                <td align="right" colspan="2" align="center">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Height="25px" Width="70px"
                        CssClass="button" />


                    <asp:Button ID="btnClose" runat="server" Text="Close" Height="25px" Width="70px"
                        CssClass="button" OnClick="btnClose_Click" />

                </td>
            </tr>
        </table>
    </div>
</asp:Content>
