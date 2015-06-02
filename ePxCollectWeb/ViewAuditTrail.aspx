﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="ViewAuditTrail.aspx.cs" Inherits="ePxCollectWeb.ViewAuditTrail" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div align="center" class="MainDiv2" >
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" align="center">
            <ContentTemplate>
                <div class="column-center" align="center">
                    <table cellspacing="2" cellpadding="2" style="z-index: 15; margin-left: 0px; margin-right: 0px; margin-top: 0px;" width="100%" align="center">
                        <tr valign="top">
                            <td align="left">
                                <table cellspacing="2" cellpadding="2" style="z-index: 15; margin-left: 0px; margin-right: 0px; margin-top: 0px;" align="left">
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:UpdatePanel ID="updateSearch" runat="server">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>List of Fields : 
                                                                <asp:DropDownCheckBoxes ID="ddlListOfFields" runat="server" UseSelectAllNode="true" AutoPostBack="true" 
                                                                    UseButtons="true" class="newInputText" Width="375px" TabIndex="11" OnSelectedIndexChanged="btnView_Click"  ><%--Code modified on April 24,2015-Subhashini %>
                                                                    <%--Code modified on March 09-2015,Subhashini--%>
                                                                    <Texts SelectBoxCaption="Select Fields"  />
                                                                    
                                                                </asp:DropDownCheckBoxes>
                                                                &nbsp;&nbsp;
                                                               <%-- <asp:Button ID="btnView" runat="server"  CssClass="button" Text="View" TabIndex="18" Height="25px" Width="70px" Visible="true"
                                                                    OnClick="btnView_Click" />--%>
                                                                &nbsp;&nbsp;
                                                                   <asp:Label ID="lblSearchMSG" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                                &nbsp;&nbsp;
                                                                  <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>

                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div id="div1" style="width: 750px; height: 350px; align: center; overflow-x: auto; overflow-y: auto">

                                    <asp:GridView ID="GridViewAudit" runat="server" EnableViewState="false" AutoGenerateColumns="false" EmptyDataText="No Records Found."
                                        CellPadding="4" ForeColor="#333333" GridLines="Both" Width="98%">
                                        <AlternatingRowStyle BackColor="White" CssClass="even" />

                                        <EmptyDataRowStyle Font-Bold="true" ForeColor="Red" HorizontalAlign="Center" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Width="200px" />

                                        <PagerStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False"
                                            ForeColor="White" BackColor="#2461BF" />
                                        <RowStyle BackColor="#EFF3FB" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                    </asp:GridView>

                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript" src="<%=Page.ResolveUrl("~/Scripts/bootstrap.min.js")%>"></script>
    <!-- SmartMenus jQuery Bootstrap Addon -->
    <script type="text/javascript" src="<%=Page.ResolveUrl("~/Scripts/jquery.smartmenus.bootstrap.js")%>"></script>
    <!-- SmartMenus jQuery plugin -->
    <script type="text/javascript" src="<%=Page.ResolveUrl("~/Scripts/jquery.smartmenus.js")%>"></script>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Scripts/jquery-ui-1.11.2.custom/jquery-ui.min.js")%>"></script>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/onCoAlert/alertify.min.js")%>"></script>
</asp:Content>
