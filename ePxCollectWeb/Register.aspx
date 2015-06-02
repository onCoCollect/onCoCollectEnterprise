<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="ePxCollectWeb.Register" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Panel runat="server" ID="Panel3">
        <div id="Div1" runat="server" class="entryArea">
            <br />
            <center> <asp:Label ID="lblTitle" runat="server" Font-Size="Medium" Text="Registration" ></asp:Label></center>
            <br />
        </div>
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlMain" HorizontalAlign="Center">
        <center>
        <div>
            <%--Code remodified on March 10-2015,Subhashini--%>
            <table style="border: none; border-collapse: collapse; text-align: center; margin-top: 10px">
                  <tr style="height: 30px; margin-top: 0px; margin: 0px; padding: 2px;text-align:center ">
                     <td colspan="2" >
                 
            <div style="text-align:center!important;padding:20px">
           <asp:RadioButton  runat="server" Id="rdoCreate" Text="&nbsp;Create" GroupName="Action" Checked="True" AutoPostBack="True" Font-Names="Verdana" Font-Size="12px" Font-Bold="false" CssClass="LabelRight" OnCheckedChanged="rdoCreate_OnCheckedChanged"/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:RadioButton runat="server" Id="rdoEdit" Text="&nbsp;Edit" AutoPostBack="True" GroupName="Action" Font-Names="Verdana" Font-Size="Small" Font-Bold="False" CssClass="LabelRight" OnCheckedChanged="rdoCreate_OnCheckedChanged"/>
                </div>
                      
                    </td>
                      </tr>
                <tr style="height: 20px; margin-top: 5px; margin: 0px; padding: 4px;">
                    <%--Code remodified on March 09-2015,Subhashini--%>
                    <td align="right" style="margin: 0px; padding: 0px;">
                        <asp:Label ID="Label1" runat="server" Text="Title&nbsp;" CssClass="LabelRight" Width="120"></asp:Label></td>
                    <%--Code remodified on March 09-2015,Subhashini--%>
                    <td style="margin: 0px; padding: 0px;" align="left">
                        <asp:DropDownList ID="cboTitle" runat="server" Font-Names="Verdana" Font-Size="11px" CssClass="dllCss" Height="20px">
                            <asp:ListItem Text="" Value="0"> </asp:ListItem>
                            <asp:ListItem Text="Mr" Value="Mr"></asp:ListItem>
                            <asp:ListItem Value="Mrs" Text="Mrs"></asp:ListItem>
                            <asp:ListItem Text="Ms" Value="Ms"></asp:ListItem>
                            <asp:ListItem Text="Dr" Value="Dr"></asp:ListItem>
                            <asp:ListItem Text="Baby" Value="Baby"></asp:ListItem>
                            <asp:ListItem Text="Master" Value="Master"></asp:ListItem>
                            <asp:ListItem Text="Miss" Value="Miss"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lblred" runat="server" Text="*" Style="color: red; font-family: Verdana;"></asp:Label>
                    </td>

                </tr>

                <tr style="height: 20px; margin-top: 5px; margin: 0px; padding: 4px;">
                    <td align="right" style="margin: 0px; padding: 0px;">
                        <asp:Label ID="Label2" runat="server" Text="First Name&nbsp;" CssClass="LabelRight" Width="120"></asp:Label></td>
                    <td style="margin: 0px; padding: 0px;" align="left">
                        <asp:TextBox ID="TxtFirstName" runat="server" Width="201px" Height="20px" autocomplete="off" CssClass="dynamictext" onkeypress="return ValidateTextBoxForDataTypeTextAlphaNumericCommaSpaceHypen(event,'','','');" onkeyup="return validateKeyup(this)" MaxLength="25">
                        </asp:TextBox>
                        <asp:Label ID="Label5" runat="server" Text="*" Style="color: red; font-family: Verdana;"></asp:Label>
                    </td>

                </tr>

                <tr style="height: 20px; margin-top: 5px; margin: 0px; padding: 4px;">
                    <td class="auto-style1" align="right" style="margin: 0px; padding: 0px;">
                        <asp:Label ID="Label4" runat="server" Text="Middle Name&nbsp;" CssClass="LabelRight" Width="120"></asp:Label></td>
                    <td class="auto-style1" style="margin: 0px; padding: 0px;" align="left">
                        <asp:TextBox ID="txtMidName" runat="server" Width="200px" autocomplete="off" CssClass="dynamictext" onkeypress="return ValidateTextBoxForDataTypeTextAlphaNumericCommaSpaceHypen(event,'','','');" onkeyup="return validateKeyup(this)" MaxLength="25"
                            Height="20px"></asp:TextBox></td>
                </tr>

                <tr style="height: 20px; margin-top: 5px; margin: 0px; padding: 4px;">
                    <td align="right" style="margin: 0px; padding: 0px;">
                        <asp:Label ID="Label3" runat="server" Text="Last Name&nbsp;" CssClass="LabelRight" Width="120"></asp:Label></td>
                    <td style="margin: 0px; padding: 0px;" align="left">
                        <asp:TextBox ID="txtLastName" runat="server" Width="201px" autocomplete="off" CssClass="dynamictext" onkeypress="return ValidateTextBoxForDataTypeTextAlphaNumericCommaSpaceHypen(event,'','','');" onkeyup="return validateKeyup(this)" MaxLength="25"
                            Height="20px"></asp:TextBox>
                        <asp:Label ID="Label8" runat="server" Text="*" Style="color: red; font-family: Verdana;"></asp:Label>
                    </td>
                </tr>

                <tr style="height: 20px; margin-top: 5px; margin: 0px; padding: 4px;">
                    <td align="right" style="margin: 0px; padding: 0px;">
                        <asp:Label ID="Label11" runat="server" CssClass="LabelRight" Width="120" Text="Hospital Code&nbsp;"></asp:Label></td>
                    <td style="margin: 0px; padding: 0px;" align="left">
                        <asp:DropDownList ID="ddlHospitalCode" runat="server" Font-Names="Verdana" Font-Size="11px" CssClass="dllCss" Height="20px">
                        </asp:DropDownList>
                        <asp:Label ID="Label12" runat="server" Text="*" Style="color: red; font-family: Verdana;"></asp:Label>
                    </td>

                </tr>

                <tr style="height: 20px; margin-top: 5px; margin: 0px; padding: 4px;">
                    <td align="right" style="margin: 0px; padding: 0px;">
                        <asp:Label ID="Label7" runat="server" CssClass="LabelRight" Width="120" Text="File Number&nbsp;"></asp:Label></td>
                    <td style="margin: 0px; padding: 0px;" align="left">
                        <asp:TextBox ID="txtFileNo" runat="server" autocomplete="off" Width="200px" CssClass="dynamictext" Height="20px" onkeypress="return ValidateTextBoxForDataTypeTextAlphaNumericFileNumber(event,'','','');" onkeyup="return validateKeyupFileNumber(this)" MaxLength="15"></asp:TextBox>
                        <asp:Label ID="Label9" runat="server" Text="*" Style="color: red; font-family: Verdana;"></asp:Label>
                    </td>

                </tr>

                <tr style="height: 20px; margin-top: 5px; margin: 0px; padding: 4px;">
                    <td align="right" style="margin: 0px; padding: 0px;">
                        <asp:Label ID="Label6" runat="server" Text="Registration Date&nbsp;" CssClass="LabelRight" Width="120px" Height="20px"></asp:Label></td>
                    <td style="margin: 0px; padding: 0px;" align="left">
                        <asp:TextBox ID="txtRegDate" runat="server" autocomplete="off" CssClass="dynamictext onCoDatePik_uptotoday" ReadOnly="false" Width="200" onkeypress="return false;"></asp:TextBox>
                        <asp:Label ID="Label10" runat="server" Text="*" Style="color: red; font-family: Verdana;"></asp:Label>
                    </td>
                </tr>
                <tr style="height: 20px; margin-top: 5px; margin: 0px; padding: 4px; text-align: center" align="center">
                    <td style="margin: 0px; padding: 0px; text-align: center" colspan="2" align="center">
                        <br />
                        <asp:Label ID="lblError" runat="server" Text="" Width="250px" ForeColor="Red"></asp:Label>
                        <br />
                    </td>
                </tr>
                <tr style="height: 20px; margin-top: 5px; margin: 0px; padding: 4px" align="center">
                    <td style="margin: 0px; padding: 0px; text-align: center" colspan="2" align="center">
                        <br />
                        <center>
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Width="70px" 
                        CssClass="button" />

                    <asp:Button ID="btnReset" runat="server" Text="Reset" Width="70px"
                        CssClass="button" OnClick="btnReset_Click" />
                    <asp:Button ID="btnClose" runat="server" Text="Close" Width="70px"
                        CssClass="button" OnClick="btnClose_Click1" />
                        </center>
                    </td>
                    <caption>
                        <br />
                    </caption>
                </tr>
            </table>
        </div>
            </center>
    </asp:Panel>
</asp:Content>
