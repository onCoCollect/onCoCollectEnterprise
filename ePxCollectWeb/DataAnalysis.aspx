<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="True" CodeBehind="DataAnalysis.aspx.cs" Inherits="ePxCollectWeb.DataAnalysis" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            function showProgress() {
                $('#dvListLoading').show();
            }

            function HideProgress() {
                $('#dvListLoading').fadeOut(1000);
            }
        });

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
    <style type="text/css">
        .auto-style4 {
            width: 83px;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <ajax:ModalPopupExtender ID="ModalPopupExtender1" TargetControlID="Hidden1" PopupControlID="pnlConfirm"
                CancelControlID="btnNo" BackgroundCssClass="overlay_back" DropShadow="false"
                runat="server">
            </ajax:ModalPopupExtender>
            <div>

                <div style="float: left; width: 50%; height: 325px;">
                    <asp:HiddenField ID="Hidden1" runat="server" />
                    <asp:Panel ID="Panel1" runat="server" Height="75px" BorderStyle="Outset" BorderWidth="1px" BackColor="#006699">
                        <asp:DropDownList ID="cboStudyName" runat="server" Width="250px" AutoPostBack="True"
                            OnSelectedIndexChanged="cboStudyName_SelectedIndexChanged">
                        </asp:DropDownList>
                        <table style="width:100%;">
                            <tr>
                                <td style="display: -ms-inline-flexbox; margin-left: 12px; text-indent: 10px; vertical-align: bottom;">
                                    <asp:CheckBox ID="chkStudyAll" Text="Select All" runat="server" AutoPostBack="True" CssClass="chkAlign" Style="display: -ms-inline-flexbox; margin-left: -5px; text-indent: 3px;"
                                        OnCheckedChanged="chkStudyAll_CheckedChanged" ForeColor="White" />
                                </td>
                                <td align="right" style="width: 60%; vertical-align: central;">
                                    <asp:Button ID="btnOther" runat="server" CssClass="button" Text="Include Other Fields" Width="143px"
                                        OnClick="btnOther_Click" />
                                    &nbsp;<br /> </td>
                            </tr>
                        </table>

                    </asp:Panel>
                    <asp:Panel ID="Panel3" runat="server" BorderStyle="Solid" BorderColor="LightGray" BorderWidth="1px" Height="242px" CssClass="ListBoxCssClass"
                        Style="overflow: scroll;" BackColor="White">
                        <asp:CheckBoxList ID="lstStudy" runat="server" CellSpacing="3" CssClass="chkBoxList">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <br />
                    <table id ="tblDiag" runat="server" visible="false">
                        <tr>
                            <td><span style="color: red; text-align: right;">*</span><asp:Label ID="lblReg" runat="server" Text="Drug Groups&nbsp;" Visible="false"></asp:Label>
                                <asp:DropDownList ID="ddlRegNames" runat="server" CssClass="dllCss" Visible="false" AutoPostBack="True" OnSelectedIndexChanged="ddlRegNames_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td><span style="color: red; text-align: right;">*</span><asp:Label ID="lblDrug" runat="server" Text="Drug Name&nbsp;" Visible="false"></asp:Label>
                                <asp:DropDownList ID="ddlDrug" runat="server" CssClass="dllCss" Visible="false">
                                </asp:DropDownList>
                            </td>
                        </tr>

                    </table>

                    <div id="Button">

                        <asp:Button ID="btnEnter" runat="server" Text="View the Results" OnClick="btnEnter_Click" CssClass="button" Width="130px"
                            OnClientClick="showProgress();" />
                        &nbsp;
          
                        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="button" OnClick="btnClose_Click" />
                        <br />
                        <br />
                        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                    <div id="dvListLoading" style="display: none;">
                    </div>
                </div>
                <div style="float: right; width: 50%">
                    <table style="border: 1px solid black; height: 75px; background-color: #006699; width: 100%;">
                        <tr style="width: 100%">
                            <td class="auto-style4">
                                <asp:CheckBox ID="chkLinesAll" Text="Select All" runat="server" AutoPostBack="True" CssClass="chkAlign" Style="display: -ms-inline-flexbox; margin-left: -5px; text-indent: 3px; margin-left: 6px; white-space:nowrap;"
                                    OnCheckedChanged="chkLinesAll_CheckedChanged" ForeColor="White" /></td>
                            <td align="right" style="vertical-align: central;">

                                <asp:DropDownList ID="CboLines" runat="server" AutoPostBack="True" CssClass="dllCss" Height="25px" Width="100%"
                                    OnSelectedIndexChanged="CboLines_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="Panel4" runat="server" BorderStyle="Solid" BorderColor="LightGray" BorderWidth="1px" Height="244px"
                        Style="overflow: scroll;">
                        <asp:CheckBoxList ID="lstLines" runat="server" CellSpacing="3" RepeatColumns="1" RepeatDirection="Horizontal"
                            CssClass="chkBoxList">
                        </asp:CheckBoxList>
                    </asp:Panel>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel runat="server" ID="updConfirm" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlConfirm" runat="server" Width="300px" BackColor="white"
                BorderColor="gray" BorderStyle="Solid" BorderWidth="1">
                <div style="width: 100%; background: #E5E5E5; height: 30px; color: white; font-family: Verdana; background-color: #006699;">

                    <span style="margin-left: 10px; margin-top: 20px;">Select Other Fields</span>
                </div>
                <asp:Panel ID="Panel5" runat="server" Height="250px" Width="300px" BackColor="white" ScrollBars="Auto"
                    BorderColor="Black" >
                    <asp:CheckBoxList ID="chkOtherFields" runat="server" CssClass="chkBoxList">
                    </asp:CheckBoxList>
                </asp:Panel>
                <div style="margin-left: 12px; margin-bottom: 3px; margin-top: 3px;text-align:center">
                    <center>
                        <asp:Label ID="lblMessage" runat="server" Text="Label" Visible="False"  Font-Names="Verdana" ForeColor="#FF3300" ></asp:Label>
                        <br/>
                        <asp:Button ID="btnYes" runat="server" Text="OK" Width="65px" OnClick="btnOk_Click"  OnClientClick="return checkSelection()"
                                CssClass="button" />
                        <asp:Button ID="btnNo" runat="server" Text="Close" Width="65px" CssClass="button" />
                    </center>
                    <br />
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
