<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SingleSelectivePick.ascx.cs" Inherits="ePxCollectWeb.SingleSelectivePick" %>

<div>
        <asp:Label ID="Label1" runat="server" Text="All Values"></asp:Label><br />
        <asp:DropDownList ID="dpAllValues" runat="server"  Width="500px" 
            OnSelectedIndexChanged="dpAllValues_SelectedIndexChanged" AutoPostBack="True">
        </asp:DropDownList>
        <br />
        <asp:Label ID="Label2" runat="server" Text="Favorite List - By Diagnosis"></asp:Label>
        <br />
        <asp:DropDownList ID="dpByDiag" runat="server" Width="500px" 
            OnSelectedIndexChanged="dpByDiag_SelectedIndexChanged" AutoPostBack="True">
        </asp:DropDownList>
        <br />
        <asp:Label ID="Label3" runat="server" Text="Favorite List - By Study"></asp:Label>
        <br />
        <asp:DropDownList ID="dpByStudy" runat="server"  Width="500px" 
            OnSelectedIndexChanged="dpByStudy_SelectedIndexChanged" AutoPostBack="True">
        </asp:DropDownList>
        <br />
        <div style="width: 410px;">
            <br />
            <div style="border-style: solid; border-color: inherit; border-width: thin; overflow: auto;
                width: 480px; height: 250px">
                <asp:ListBox ID="lstValues" runat="server" Height="250px" Width="480px"></asp:ListBox>
            </div>
            <br />
        </div>
        <br />
        <asp:Label ID="Label4" runat="server">Value Picked Now:</asp:Label><br />
        <asp:Label ID="lblValuePicked" runat="server" Width="500px"></asp:Label>
    </div>
    <input id="btnOk" type="button" value="Ok" onclick="CloseDialogWithValue();" />
    &nbsp; &nbsp;
    <input id="btnClose" type="button" value="Close" onclick="CloseDialog();" />