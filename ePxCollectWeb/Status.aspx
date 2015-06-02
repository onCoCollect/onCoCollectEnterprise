﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="True"
    CodeBehind="Status.aspx.cs" Inherits="ePxCollectWeb.Status" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript"><%--Code modified on April 15,2015-Subhashini--%>
        function UpdateValid() {
            var ele = document.getElementById('ctl00_MainContent_lblErrorMsg');
            ele.innerHTML = "";
        }
    </script>
    <asp:UpdatePanel runat="server" ID="updPanel">
        <ContentTemplate>
            <asp:Panel runat="server" ID="Panel3">
                        <div id="Div1" runat="server" class="entryArea" >                           
                          <br /> <center> <asp:Label ID="lblTitle" runat="server" Font-Size="Medium" Text="Status" ></asp:Label></center><br />
                        </div>
                    </asp:Panel>
            <asp:Panel ID="Panel1" runat="server" Width="100%" Height="250px" HorizontalAlign="Center" >
                <br />
                <br />
                <table align="center" style="width:40%;"><%--Code remodified on March 09-2015,Subhashini--%>
                    <tr>
                        <td align="right" style="margin: 0px; padding: 0px">
                            <asp:Label ID="lblRec1" runat="server" CssClass="LabelRight" Text="Status&nbsp;" ></asp:Label></td>
                        <td align="left" style="margin: 0px; padding: 0px">
                            <asp:DropDownList ID="cboStatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboStatus_SelectedIndexChanged" CssClass="dllCss" Width="160px">
                            </asp:DropDownList>
                            <asp:Label runat="server" ForeColor="Red" ID="w" Text="*"></asp:Label>
                        </td>

                    </tr>
                    <tr>
                        <td  align="right"  style="margin: 0px; padding: 0px">
                            <asp:Label ID="Label5" runat="server" CssClass="LabelRight" Text="Status Date&nbsp;"
                                ></asp:Label></td>
                        <td  align="left" style="margin: 0px; padding: 0px">
                            <asp:TextBox runat="server" ID="txtStatusDate"  ViewStateMode="Enabled" CssClass="dynamictext onCoDatePik_uptotoday" onkeypress="return false;" onclick="UpdateValid();" Width="155px" />
                        </td>

                    </tr>
                    <tr>
                        <td align="right"  style="margin: 0px; padding: 0px">
                            <asp:Label ID="Label6" runat="server" Text="Cause of Death&nbsp;" CssClass="LabelRight"
                               ></asp:Label></td>
                        <td  align="left" style="margin: 0px; padding: 0px">
                            <asp:DropDownList ID="cboCauseofDeath" runat="server" CssClass="dllCss" Width="160px">
                            </asp:DropDownList></td>

                    </tr>
                    <tr>
                        <td   align="right"  style="margin: 0px; padding: 0px">
                            <asp:Label ID="Label4" runat="server" Text="Date of Death&nbsp;" CssClass="LabelRight"
                               ></asp:Label></td>
                        <td align="left" style="margin: 0px; padding: 0px">
                            <asp:TextBox runat="server" ID="txtDOD" onkeypress="return false;" Width="155px" CssClass="dynamictext onCoDatePik_uptotoday" />
                        </td>

                    </tr>

                    <tr>
                        <%--Code modified on March 06-2015,Subhashini--%>
                        <td colspan="2" align="center">
                       
                          <br />
                           <asp:Label ID="lblErrorMsg"  runat="server" ForeColor="Red" Visible="True" Text=""></asp:Label> 
                              <br />
                       
                            </td> 
                    </tr>
                    <tr>
                        <%--Code modified on March 06-2015,Subhashini--%>
                        <td style="margin: 0px; padding: 0px" align="center" colspan="2">
                           
                            

                            <asp:Button ID="btnSave" runat="server" CssClass="button" Text="Save" Width="70px"
                                OnClick="btnSave_Click" />
                            &nbsp;
                            <asp:Button ID="btnReset" runat="server" CssClass="button" Text="Reset" Width="70px"
                                OnClick="btnReset_Click" />&nbsp;

                    <asp:Button ID="btnExit" runat="server" CssClass="button" Text="Close" Width="70px"
                        OnClick="btnExit_Click" />&nbsp;
                            
                    <asp:Button ID="btnRegister" runat="server" CssClass="button" OnClick="btnRegister_Click" Width="70px" Height="25px"
                        Text="Pick Another Patient" Visible="False" /></td>

                    </tr>
                </table>

            </asp:Panel>
            <%--<asp:Panel ID="Panel2" runat="server" Width="100%" Height="50px" BorderStyle="Solid"
                BorderWidth="1px" BackColor="LightBlue" HorizontalAlign="Center">
               
            </asp:Panel>--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
