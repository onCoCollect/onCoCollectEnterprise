<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserMenu.ascx.cs" Inherits="ePxCollectWeb.UserControl.UserMenu" %>
<table cellpadding="0" cellspacing="0" class="menustyle" width="100%" align="left">
    <tr>
        <td style="width: 100%; height: 25px; padding-left: 0px; vertical-align: middle; padding-right: 0px"
            align="left">
            <asp:Menu ID="User_Menu" runat="server" OnMenuItemClick="AdminMenu_Onclick"
                Orientation="Horizontal" Font-Names="verdana" Font-Size="12px" class="MainMenu"
                align="left">
                <StaticMenuItemStyle HorizontalPadding="15px" VerticalPadding="2px" />
                <DynamicHoverStyle CssClass="hoverstyle" />
                <DynamicMenuStyle BackColor="#2a75a9" />
                <StaticSelectedStyle />
                <DynamicMenuItemStyle HorizontalPadding="15px" VerticalPadding="2px" />
                <StaticHoverStyle CssClass="hoverstyle" />
                <Items>
                </Items>
                <LevelMenuItemStyles>
                    <asp:MenuItemStyle Font-Bold="true" />
                </LevelMenuItemStyles>
            </asp:Menu>
        </td>
    </tr>
</table>
