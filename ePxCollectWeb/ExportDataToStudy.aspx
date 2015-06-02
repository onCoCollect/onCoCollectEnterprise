﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="True" CodeBehind="ExportDataToStudy.aspx.cs" Inherits="ePxCollectWeb.ExportDataToStudy" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        tr th {
            background: none repeat scroll 0 0 #c3ebf8 !important;
            padding: 5px;
            font-weight: normal;
            color: #003660 !important;
        }
    </style>
    <script type="text/javascript">
        function showProgress() {
            $('#dvListLoading').show();
        }

        function HideProgress() {
            $('#dvListLoading').fadeOut(1000);
        }

    </script>
    <style type="text/css">
        #dvListLoading {
            background: url(../images/fancybox_loading.gif) no-repeat center center;
            position: fixed;
            z-index: 1000;
            top: 0%;
            left: 0%;
            margin: 0;
            height: 100%;
            width: 100%;
            background-color: gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }
    </style>
    <script type="text/javascript">

        function Clear() {
            document.getElementById("<%=lblMessage.ClientID%>").innerHTML = "";


        }
    </script>
    <div id="dvListLoading" style="display: none;"></div>

    <div style="float: left; position: relative; width: 100%; overflow-x: hidden; overflow-y: auto">
        <%--Code remodified on April 6-2015,Subhashini--%>
        <div class="div-column-center" align="left" style="float: left; width: 80%; position: relative;">

            <table cellpadding="10" cellspacing="10" width="90%" align="center">
                <tr>
                    <td colspan="2" style="height: 35px; font-weight: bold; font-size: 10pt; font-family: Verdana; color: #2A75A9"
                        align="center">Export Data to Study Pool
                    </td>
                </tr>

                <tr style="height: 20px; margin-top: 5px; margin: 0px; padding: 4px;">
                    <td align="right" style="margin: 0px; padding: 0px;">
                        <asp:Label ID="Label2" runat="server" Text="Study Name&nbsp;&nbsp;" CssClass="LabelRight" Width="120"></asp:Label></td>
                    <td style="margin: 0px; padding: 0px;" align="left">
                        <asp:DropDownList ID="ddlStutyList" runat="server" Width="201px" Height="20px" autocomplete="off" CssClass="dllCss" onchange="Clear();">
                        </asp:DropDownList>
                        <asp:Label ID="Label5" runat="server" Text="*" Style="color: red; font-family: Verdana;"></asp:Label>
                    </td>

                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Label ID="lblMessage" runat="server" Text="" CssClass="lbltext"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="btnExportDatatoCloud" UseSubmitBehavior="true" Text="Export" CssClass="button"
                            OnClientClick="showProgress();"
                            runat="server" OnClick="btnExportDatatoCloud_Click" />

                        <%-- <asp:Button ID="btnExportDatatoCloud" runat="server" CssClass="button" Text="Export"  usesubmitbehavior="true" OnClientClick="showProgress();"
                            OnClick="btnExportDatatoCloud_Click"/>--%>
                        &nbsp;
                     <asp:Button ID="btnCancel" runat="server" CssClass="button" Text="Close" OnClick="btnCancel_Click" />
                        <%-- &nbsp;
                     <asp:Button ID="btnReset" runat="server" CssClass="button" Text="Reset" OnClick="btnReset_Click" />--%>
                    </td>
                </tr>

            </table>

        </div>


        <div style="width: 18%; position: relative; text-align: center; vertical-align: top">
            <br />

            <asp:Button ID="btnAuditTrail" runat="server" Text="Audit Trail" 
                TabIndex="11" OnClick="btnAuditTrail_Click" CausesValidation="false" CssClass="button" Style="position: absolute;" />


        </div>
    </div>
    <%--Code modified on March 09-2015,Subhashini--%>
    <%--    <asp:Panel ID="pnlSelection" runat="server" BackColor="white" Height="250px" Width="300px"
        BorderColor="Black" Style="border: 1px solid; display: none; vertical-align: middle; text-align: center" HorizontalAlign="Center">
            <div style="width: 100%; background: #2A75A9; height: 35px; vertical-align: middle; color: White; text-align: center; padding-top: 5px;">
       <%--  <div style="width: 100%; height: 350px; vertical-align: middle; color: White; text-align: center; padding-top: 5px; overflow: auto;">

          <%--  <div style="width: 100%; background: #0099C8; height: 35px; color: White; vertical-align: middle">
                <b>Audit Trail</b>
            </div>

            <table align="center" style="padding: 5px;width:300px;height:300px;overflow:auto; color: Black; vertical-align: middle">
                                
                <tr>
                    <td colspan="2" align="center">--%>
    <asp:Panel ID="pnlSelect" runat="server" Height="610px" Width="1200px" BackColor="white"
        BorderColor="#465c71" Style="border: 3px solid; display: none; vertical-align: middle">
        <div style="background-color: #eef3fa">
            <div style="width: 100%; background: #0099c8; height: 25px; color: White; font-size: 12px!important; box-sizing: border-box!important; line-height: 0.6cm; vertical-align: middle; text-align: center">
                <center>
                                <b>Export Data to Study Pool</b></center>
            </div>
            <table align="center" style="color: Black; background-color: #eef3fa; vertical-align: middle; padding: 5px">
                <tr>
                    <td colspan="2">
                        <div id="div1" style="width: 1150px; height: 520px; align: center; overflow-x: auto; overflow-y: auto">


                            <asp:GridView ID="GridExportAudit" runat="server" AutoGenerateColumns="False" Style="overflow: auto; position: relative"
                                BorderColor="#E7E7FF" BorderStyle="Solid" BorderWidth="1px" EnableModelValidation="True" AllowPaging="true"
                                OnPageIndexChanging="GridExportAudit_PageIndexChanging" Width="1100" PageSize="4" Height="500"
                                Font-Size="12px" GridLines="None">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:TemplateField HeaderText="LogTime">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblLogTime" runat="server" Text="User ID"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="LabelLogTime" runat="server" Text='<%# Eval("UserID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblAction" runat="server" Text="Created Date"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="LabelAction" runat="server" Text='<%# Eval("CreatedDate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comments" ItemStyle-Width="150px">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblComments" runat="server" Text="Study Code"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="LabelComments" runat="server" Text='<%# Eval("StudyCode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comments">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblComments" runat="server" Text="Study Patients List"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="LabelComments" runat="server" Text='<%# Eval("StudyPatientsList") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comments">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblComments" runat="server" Text="Study Fields List CSV"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="LabelComments" runat="server" Text='<%# Eval("StudyFieldsListCSV") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comments">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblComments" runat="server" Text="Study Criteria"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="LabelComments" runat="server" Text='<%# Eval("StudyCriteria") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comments"  Visible="false">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblComments" runat="server" Text="Cumpulsory Fields Criteria"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="LabelComments" runat="server" Text='<%# Eval("CumpulsoryFieldsCriteria") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comments">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblComments" runat="server" Text="Cumpulsory Fields"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="LabelComments" runat="server" Text='<%# Eval("CumpulsoryFields") %>'></asp:Label>
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
                                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />

                                <SelectedRowStyle BackColor="#D1DDF1" CssClass="gridSelected" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center" style="vertical-align: middle;">

                        <asp:Button ID="btnClick" runat="server" Text="Close" Width="100px" CssClass="button" />
                        <br />


                    </td>
                </tr>
            </table>
        </div>
        <%--</div>--%>
        <%--   <div style="position: relative;">
            <center>
       
                </center>
        </div>--%>
    </asp:Panel>

    <ajax:ModalPopupExtender ID="ModalPopupExtender2" TargetControlID="HiddenField1" PopupControlID="pnlSelect"
        BackgroundCssClass="overlay_back" DropShadow="false" CancelControlID="btnClick"
        runat="server">
    </ajax:ModalPopupExtender>
    <asp:HiddenField ID="HiddenField1" runat="server" />

</asp:Content>
