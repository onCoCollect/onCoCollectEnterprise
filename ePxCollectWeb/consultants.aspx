﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="consultants.aspx.cs" Inherits="ePxCollectWeb.consultants" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div align="center" class="MainDiv">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" align="center">
            <ContentTemplate>
                <div class="column-center" align="center">
                    <table cellspacing="2" cellpadding="2" style="z-index: 15; margin-left: 0px; margin-right: 0px"
                        align="center">
                        <tr valign="top">
                            <td align="center">
                                <table cellspacing="2" cellpadding="2" style="z-index: 15; margin-left: 0px; margin-right: 0px; margin-top: 0px;"
                                    align="center">
                                    <%--   <tr>
                                        <td width="70%" align="right">
                                            User ID :
                                        </td>
                                        <td width="30%">
                                            <asp:TextBox ID="txtUserID" runat="server" TabIndex="3" MaxLength="10" Width="200px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="70%" align="right">
                                            Patient ID :
                                        </td>
                                        <td width="30%">
                                            <asp:TextBox ID="txtPatientID" runat="server" TabIndex="2" MaxLength="20" Width="200px"></asp:TextBox>
                                            <br />
                                            <asp:Button ID="btnGet" runat="server" Text="Get Consultants" OnClick="btnGet_Click" />
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="btnAddMe" runat="server" TabIndex="5" Text="Add"
                                                CssClass="button" OnClick="btnAddMe_Click" />&nbsp;</td>
                                        <td align="left">
                                            <asp:Button ID="btnRemoveMe" runat="server" CausesValidation="false"
                                                Text="Remove" CssClass="button" OnClick="btnRemoveMe_Click" />
                                        </td>

                                        <td align="left">
                                            <asp:Button ID="btnclose" runat="server" CausesValidation="false"
                                                Text="Close" CssClass="button" OnClick="btnclose_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="70%" align="center" valign="top" colspan="3">
                                            <asp:GridView ID="GridConsultants" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                                                BorderColor="#E7E7FF" BorderStyle="Solid" BorderWidth="1px" EnableModelValidation="True"
                                                Font-Size="12px" GridLines="None" Style="margin-left: 0px" Width="250px">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Consultants" ItemStyle-Width="200px">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblHospitalName" runat="server" Text="Consultants"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="LabelHospitalName" runat="server" Text='<%# Eval("Consultants") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Consultant Name" ItemStyle-Width="200px">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblHospitalName" runat="server" Text="Consultant Name"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="LabelHospitalName" runat="server" Text='<%# Eval("ConsultantName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EditRowStyle BackColor="#2461BF" />
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#CCCCCC" Font-Bold="true" Font-Underline="true" ForeColor="White"
                                                    HorizontalAlign="Left" />
                                                <RowStyle BackColor="#EEEEEE" />
                                                <SelectedRowStyle BackColor="#D1DDF1" CssClass="gridSelected" />
                                            </asp:GridView>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td align="center" colspan="3">
                                            <h3>
                                                <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Bold="false" CssClass="lbltext"></asp:Label></h3>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="column-right">
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>