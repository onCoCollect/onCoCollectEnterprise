<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="OthersPage.aspx.cs" Inherits="ePxCollectWeb.OthersPage" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%-- <div id="Div1" runat="server" class="entryArea" >                           
                           <center> <asp:Label ID="lblTitle" runat="server" Font-Size="Medium" Text="Other" ></asp:Label></center><br />
                        </div> --%>
    <asp:Panel ID="Panel3" runat="server" BorderStyle="Solid" BorderWidth="1px"
        Height="400px" Style="overflow: auto;">
        <%--Code modified on April 24,2015-Subhashini--%>

        <asp:CheckBoxList ID="lstStudy" runat="server" RepeatColumns="2" TextAlign="right" Width="70%" CssClass="checkcss" Font-Names="verdana" Font-Size="11px"
            RepeatDirection="Vertical" ViewStateMode="Enabled">
        </asp:CheckBoxList>
    </asp:Panel>
    <br />
    <div align="center">
        <asp:Button ID="btnEnter" runat="server" Text="OK" Width="70px"
            CssClass="button"
            OnClick="btnEnter_Click" />
        <asp:Button ID="btnReset" runat="server" Text="Reset" Width="70px"
            CssClass="button" OnClick="btnReset_Click" />
        <asp:Button ID="btnClose" runat="server" Text="Close" Width="70px"
            CssClass="button"
            OnClick="btnClose_Click" />
    </div>
    <div align="center">
        <br />
        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>

    </div>

    <br />
</asp:Content>
