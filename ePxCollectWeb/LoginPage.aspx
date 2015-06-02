<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="ePxCollectWeb.LoginPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div style="width:700px;">
<div style="float: left;">
<asp:Image ID="Image1" runat="server" ImageUrl="images/Login.jpg" />
</div>
    
<div style="border: thin inset #0000FF; float: right; width:300px;">
<div style=" margin-left:20px">
<asp:Panel runat="server" ID="Panel1" DefaultButton="btnLogin">
<asp:Label ID="Label1" CssClass="loginDisplayLeft" runat="server" Text="User Name :" 
        Font-Bold="True" ForeColor="#CE1126"></asp:Label>
        <br />      
        <asp:DropDownList ID="cboUserName" runat="server">
        </asp:DropDownList>
        <br />
          <br />
<asp:Label ID="Label4" CssClass="loginDisplayLeft"  runat="server" Text="Password :" 
        Font-Bold="True" ForeColor="#CE1126"></asp:Label>
            <br />
    <asp:TextBox ID="txtPassword" TextMode="Password"  runat="server" AccessKey="*"></asp:TextBox>
    <br />
    <br />
    <asp:Button ID="btnLogin" runat="server" Text="Log me in" 
        onclick="btnLogin_Click" TabIndex="-1" />
    &nbsp;&nbsp;
    <asp:Button ID="btnCancell" runat="server" Text="Cancel" />
    <br />
    <asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="Red" 
        Visible="False"></asp:Label>
        </asp:Panel>
        </div>
    </div>
    </div>
</asp:Content>
