﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="True" CodeBehind="DataGroup.aspx.cs" Inherits="ePxCollectWeb.DataGroup"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
        function ShowConfirmation() {

            if (confirm("Some Groups already exist for this Template. Are you sure want to delete this Template and Groups?") == true) {
                //Calling the server side code after confirmation from the user
                document.getElementById("<%=btnAlelrt.ClientID%>").click();
                return true;
            }
        }
        function checkDelete(id) {
            var obj = document.getElementById(id);
            if (!checkEmptyDataGroup('ctl00_MainContent_ddlTemplateNames', 'Please select a Template Name.')) {
                return false;
            }

            return true;
        }

        function checkTemplate(id) {
            var obj = document.getElementById(id);
            if (!checkEmptyDataGroup('ctl00_MainContent_txtTemplateName', 'Please enter a Template name.')) {
                return false;
            }
            if (!checkEmptyDataGroup('ctl00_MainContent_txtDesc', 'Please enter a Description.')) {
                return false;
            }

            return true;
        }
        function checkEmptyDataGroup(id, msg) {
            var obj = document.getElementById(id);
            if (obj.value == null || obj.value == '' || obj.value.trim().length == 0) {
                document.getElementById("<%=lblmsg.ClientID%>").style.color = 'red';
                document.getElementById("<%=lblmsg.ClientID%>").innerText = msg;
                obj.focus();
                return false;
            }
            else
                return true;
        }
    </script>
    <style type="text/css">
        .itemwrap {
            word-break: break-all;
            font-family: Verdana;
            font-size: 11px;
            text-align: justify;
        }

        .hideGridColumn {
            display: none;
        }

        .radioButtonList td {
            vertical-align: top;
            text-indent: 3px;
            display: -ms-inline-flexbox;
            text-orientation: sideways-right;
        }

        .radioButtonList input[type="radio"] {
            float: left;
        }

        .radioButtonList label {
            width: 50px;
            display: block;
        }

        .btnAlert {
            display: none;
        }
    </style>
    <asp:UpdatePanel runat="server">

        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlMain">
                <table width="600px" style="margin-left: 50px;">
                    <tr>
                        <td>
                            <fieldset>
                                <legend>
                                    <h3 style="color: #006699;">Manage Template</h3>
                                </legend>
                                <table width="600px" style="margin-left: 7px;">

                                    <tr style="margin: 0px; padding: 0px;">
                                        <td align="right" style="margin: 0px; padding: 0px;"></td>
                                        <td style="display: inline; margin: 0px; padding: 0px;">
                                            <asp:RadioButtonList ID="rbList" CssClass="radioButtonList" RepeatColumns="2" RepeatLayout="Table" TextAlign="Right" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbList_SelectedIndexChanged" Height="16px" Width="300px">
                                                <asp:ListItem Text="Create" Value="1" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Edit" Value="2"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>

                                    </tr>
                                    <tr style="margin: 0px; padding: 0px;">
                                        <td style="width: 150px; text-align: right; margin-right: 0px; padding-right: 0px;">
                                            <asp:Label ID="Label2" runat="server" Text="Select Template&nbsp;" Font-Names="Verdana" Font-Size="8pt"></asp:Label></td>
                                        <td style="margin: 0px; padding: 0px;">

                                            <asp:DropDownList ID="ddlTemplateNames" runat="server" AutoPostBack="True" CssClass="dllCssDataGroup" OnSelectedIndexChanged="ddlTemplateNames_SelectedIndexChanged">
                                                <asp:ListItem Text="" Value="0">

                                                </asp:ListItem>
                                            </asp:DropDownList>&nbsp<asp:Button ID="btnDelete" runat="server" Height="25px" Text="Delete" OnClick="btnDelete_Click" CssClass="button"
                                                OnClientClick="return checkDelete('ctl00_MainContent_hiddenValue');" />
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr style="margin: 0px; padding: 0px;">
                                        <td style="width: 150px; text-align: right; margin-right: 0px; padding-right: 0px;">
                                            <span style="color: red; text-align: right;">*</span>
                                            <asp:Label ID="lblTemplateName" runat="server" Text="Template Name&nbsp;" Font-Names="Verdana" Font-Size="8pt"></asp:Label></td>
                                        <td align="left" style="margin: 0px; padding: 0px;">
                                            <asp:TextBox ID="txtTemplateName" runat="server" autocomplete="off" CssClass="dynamictextDataGroup" Font-Size="8pt" MaxLength="50"></asp:TextBox></td>
                                        <ajax:FilteredTextBoxExtender ID="fteCurrentPassword" runat="server" TargetControlID="txtTemplateName"
                                            FilterType="lowercaseLetters,UppercaseLetters,numbers,Custom" ValidChars="-_ " />
                                    </tr>
                                    <tr style="margin: 0px; padding: 0px;">
                                        <td style="width: 150px; text-align: right; margin-right: 0px; padding-right: 0px;">
                                            <span style="color: red; text-align: right;">*</span>
                                            <asp:Label ID="lblDesc" runat="server" Text="Description&nbsp;" Font-Names="Verdana" Font-Size="8pt"></asp:Label></td>
                                        <td style="margin: 0px; padding: 0px;">
                                            <asp:TextBox ID="txtDesc" runat="server" autocomplete="off" TextMode="MultiLine" Height="77px" CssClass="dynamictextDataGroup" Font-Size="8pt"></asp:TextBox>
                                            <asp:HiddenField ID="hiddenValue" runat="server" />
                                        </td>
                                    </tr>
                                    <tr style="margin: 0px; padding: 0px;">
                                        <td align="right" style="width: 150px; margin: 0px; padding: 0px;"></td>
                                        <td style="margin: 0px; padding: 0px;">
                                            <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" CssClass="button"
                                                OnClientClick="return checkTemplate('ctl00_MainContent_hiddenValue');" />
                                            <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" CssClass="button" />&nbsp;<asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red"></asp:Label></td>
                                    </tr>
                                </table>
                            </fieldset>


                        </td>
                        <td valign="top">
                            <div style="margin-top: 10px;">
                                <%-- <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="button" OnClick="btnClose_Click" />--%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="width: 700px;">
                                <fieldset>
                                    <legend>
                                        <h3 style="color: #006699;">Create Stat Code </h3>
                                    </legend>
                                    <table width="600px" style="margin-left: 10px;">
                                        <tr>
                                            <td align="right" style="width: 150px; margin: 0px; padding: 0px;">
                                                <span style="color: red; text-align: right;">*</span>
                                                <asp:Label ID="lblTemplate" runat="server" Text="Select Template&nbsp;" Font-Names="Verdana" Font-Size="8pt"></asp:Label>
                                            </td>
                                            <td style="margin: 0px; padding: 0px;">
                                                <asp:DropDownList ID="ddlSelectTemplate" runat="server" CssClass="dllCssDataGroup" Font-Size="8pt"
                                                    EnableViewState="true" OnSelectedIndexChanged="ddlSelectTemplate_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                                <asp:Button ID="btnOKTemplate" runat="server" Text="OK" Height="25px" Width="30px" CssClass="button"
                                                    OnClientClick="showProgress();" OnClick="btnOKTemplate_Click" />
                                            </td>

                                        </tr>
                                        <tr style="margin: 0px; padding: 0px;">
                                            <td align="right" style="width: 150px; margin: 0px; padding: 0px;">
                                                <span style="color: red; text-align: right;">*</span>
                                                <asp:Label ID="lblField" runat="server" Text="Select Field&nbsp;" Font-Names="Verdana" Font-Size="8pt"></asp:Label></td>
                                            <td style="margin: 0px; padding: 0px;">
                                                <asp:DropDownList ID="ddlField" runat="server" CssClass="dllCssDataGroup" AutoPostBack="true"
                                                    Font-Size="8pt" EnableViewState="true" OnSelectedIndexChanged="ddlField_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:Button ID="btnOKField" runat="server" Text="OK" Height="25px" Width="30px" CssClass="button"
                                                    OnClientClick="showProgress();" OnClick="btnOKField_Click" />
                                            </td>
                                        </tr>
                                        <tr style="margin: 0px; padding: 0px;">
                                            <td align="right" style="width: 150px; margin: 0px; padding: 0px;">
                                                <span style="color: red; text-align: right;">*</span>
                                                <asp:Label ID="lblValue" runat="server" Text="Value&nbsp;" Font-Names="Verdana" Font-Size="8pt"></asp:Label></td>
                                            <td style="margin: 0px; padding: 0px;">

                                                <asp:DropDownCheckBoxes ID="dpValues" runat="server" AutoPostBack="true" UseSelectAllNode="true" CssClass="dllCssDataGroup" Height="20px"
                                                    OnSelectedIndexChanged="dpValues_SelectedIndexChanged"
                                                    UseButtons="True">
                                                </asp:DropDownCheckBoxes>

                                            </td>
                                        </tr>
                                        <tr style="margin: 0px; padding: 0px;">
                                            <td align="right" style="width: 150px; margin: 0px; padding: 0px;">
                                                <span style="color: red; text-align: right;">*</span>
                                                <asp:Label ID="lblGroupName" runat="server" Text="Group Name&nbsp;" Font-Names="Verdana" Font-Size="8pt"></asp:Label></td>
                                            <td style="margin: 0px; padding: 0px;">
                                                <asp:TextBox ID="txtGroupName" autocomplete="off" runat="server" CssClass="dynamictextDataGroup" Font-Size="8pt" MaxLength="50"></asp:TextBox>
                                                <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtGroupName"
                                                    FilterType="lowercaseLetters,UppercaseLetters,numbers,Custom" ValidChars="-_ " />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="width: 150px; margin: 0px; padding: 0px;"></td>
                                            <td style="margin: 0px; padding: 0px;">
                                                <asp:Button ID="btnstatSave" runat="server" Text="Save" OnClick="btnstatSave_Click" CssClass="button" />
                                                <asp:Button ID="btnstatReset" runat="server" Text="Reset" OnClick="btnstatReset_Click" CssClass="button" />&nbsp;<asp:Label ID="lblMsgGroup" runat="server" Text="" ForeColor="Red"></asp:Label></td>
                                        </tr>

                                    </table>
                                </fieldset>
                            </div>
                            <div id="dvListLoading" style="display: none;"></div>

                        </td>
                        <%--Code modified on April 13,2015-Subhashini--%>
                    </tr>
                    <tr>
                        <td>
                            <div style="width: 700px;">
                                <table width="600px" style="margin-left: 10px;">
                                    <tr style="margin: 0px; padding: 0px;">
                                        <td align="right" style="width: 150px; margin: 0px; padding: 0px;">
                                            <asp:Label ID="Label1" runat="server" Text="Select Template&nbsp;" Font-Names="Verdana" Font-Size="8pt"></asp:Label><%--Code modified on April 13,2015-Subhashini--%>
                                        </td>
                                        <td style="margin: 0px; padding: 0px;">
                                            <asp:DropDownList ID="ddlStatGroupName" runat="server" AutoPostBack="true" CssClass="dllCssDataGroup" OnSelectedIndexChanged="ddlStatGroupName_SelectedIndexChanged" Font-Size="8pt">
                                            </asp:DropDownList>
                                            <%--   <asp:Button ID="btnHide" runat="server" Text="Close" CssClass="button" OnClick="btnHide_Click" />--%>
                                            <asp:Button ID="Button1" runat="server" Text="Close" CssClass="button" OnClick="btnClose_Click" />
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </div>

                        </td>
                    </tr>
                </table>
                <%--Code modified on April 13,2015-Subhashini--%>
                <div id="div2" runat="server" align="center" style="width: auto; margin-left: 0px; align: center; overflow-x: auto; overflow-y: auto">
                    <asp:GridView ID="GridDataGroup" runat="server" AllowPaging="true" ForeColor="#333333" AutoGenerateColumns="False"
                        BorderColor="#E7E7FF" BorderStyle="Solid" BorderWidth="1px" EnableModelValidation="True"
                        Font-Size="12px" GridLines="None" OnPageIndexChanging="GridDataGroup_PageIndexChanging" EmptyDataText="No Records Found."
                        Style="margin-left: 0px" Width="770px" PageSize="5" OnSelectedIndexChanged="GridDataGroup_SelectedIndexChanged">

                        <Columns>
                            <asp:TemplateField HeaderText="STAT ID" ItemStyle-Width="200px" Visible="false">
                                <HeaderTemplate>
                                    <asp:Label ID="lblSTATID" runat="server" Text="STAT ID"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LabelSTATID" runat="server" Text='<%# Eval("stat_ID") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Template Name" ItemStyle-Width="200px">
                                <HeaderTemplate>
                                    <asp:Label ID="lblTemplateName" runat="server" Text="Template Name"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LabelTemplateName" runat="server" Text='<%# Eval("stat_TemplateName") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Field Name" ItemStyle-Width="200px">
                                <HeaderTemplate>
                                    <asp:Label ID="lblFieldName" runat="server" Text="Field Name"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LabelFieldName" runat="server" Text='<%# Eval("stat_FieldName") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Table Name" ItemStyle-Width="200px" Visible="false">
                                <HeaderTemplate>
                                    <asp:Label ID="lblTableName" runat="server" Text="Table Name"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LabelTableName" runat="server" Text='<%# Eval("Table Name") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Field Value" ItemStyle-Width="200px">
                                <HeaderTemplate>
                                    <asp:Label ID="lblFieldValue" runat="server" Text="Field Value"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LabelFieldValue" runat="server" Text='<%# Eval("stat_Value") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Group Name" ItemStyle-Width="200px">
                                <HeaderTemplate>
                                    <asp:Label ID="lblGroupName" runat="server" Text="Group Name"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LabelGroupName" runat="server" Text='<%# Eval("stat_GroupName") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="MANG-TemplateID" ItemStyle-Width="200px" Visible="false">
                                <HeaderTemplate>
                                    <asp:Label ID="lblMANGTemplateID" runat="server" Text="MANG-TemplateID"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LabelMANGTemplateID" runat="server" Text='<%# Eval("mang_TemplateID") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>

                            <asp:TemplateField ShowHeader="True" ItemStyle-Width="75px">
                                <HeaderTemplate>
                                    <asp:Label ID="lblEdi" runat="server" Text="Edit"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgEdit" runat="server" CausesValidation="false" CommandName="Select"
                                        ImageUrl="~/Images/modify.png" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <HeaderTemplate>
                                    <asp:Label ID="lblDel" runat="server" Text="Delete"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgDelete" runat="server" AlternateText='<%# Eval("stat_ID")  %>'
                                        CausesValidation="False" CommandArgument='<%# Eval("stat_ID") %>' CommandName="DeleteRow"
                                        ImageUrl="~/Images/del.png" OnClick="imgDelete_Click" OnClientClick="javascript:if (confirm('Are you sure that you want to delete the selected record?')){}else{return false;};" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle BackColor="White" />

                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" Height="30px" Width="100px" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="false" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                    </asp:GridView>
                </div>

            </asp:Panel>
            <asp:HiddenField ID="Hidden1" runat="server" />
            <asp:Button ID="btnAlelrt" runat="server" Text="GetDetails" OnClick="btnAlelrt_Click" CssClass="btnAlert" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript" src="<%=Page.ResolveUrl("~/Scripts/jquery.smartmenus.bootstrap.js")%>"></script>
</asp:Content>