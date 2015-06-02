<%@ Page Title="About Us" Language="C#" MasterPageFile="~/MasterPAge/epxWebEmpty.Master" AutoEventWireup="true"
    CodeBehind="About.aspx.cs" Inherits="ePxCollectWeb.About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>

     
          <asp:TextBox ID="TextBox1" runat="server" Width="699px"></asp:TextBox> <br />
          &nbsp;
          <asp:Button ID="Button1" runat="server" Text="Encrypt" 
              onclick="Button1_Click" />  <asp:Label ID="Label1" runat="server" Text="Decrypted Value"></asp:Label>
          <br />
          <asp:TextBox ID="TextBox2" runat="server" TextMode="MultiLine" Width="909px"></asp:TextBox>
        <asp:Button ID="Button2" runat="server" Text="Decrypt" onclick="Button2_Click" />
        <input type="button" onclick="clickMe();" />

    </h2>
    <h2>
        &nbsp;</h2>
    <h2>
        &nbsp;</h2>
    <h2>
       
        
        About
    </h2>
    <p>
        Put content here.
    </p>
</asp:Content>
