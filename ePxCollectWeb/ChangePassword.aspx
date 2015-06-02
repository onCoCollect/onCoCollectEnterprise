<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="ePxCollectWeb.ChangePassword" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function checkRedirectPage(txt) {
            alert(txt);
            window.location("Login.aspx");

        }
    </script>

    <script type="text/javascript">

        function Clear() {
            document.getElementById("<%=lblmsg.ClientID%>").innerHTML = "";


        }
    </script>
    <div align="center" class="MainDivCP">
        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" align="center" UpdateMode="Conditional">
            <ContentTemplate>--%>
        <div style="float: left; position: relative; width: 100%; overflow-x: hidden; overflow-y: auto">
            <%--Code remodified on April 6-2015,Subhashini--%>
            <div class="div-column-center" align="left" style="float: left; width: 80%; position: relative;">
                <%--Code remodified on April 6-2015,Subhashini--%>
                <center>
            <table cellspacing="1" cellpadding="2" style="z-index: 0; margin-left: 0px; margin-right: 0px"
                width="80%" align="center">
                <tr>
                    <td align="center" style="width:80%;position:relative;">
                        <table cellspacing="1" cellpadding="2" style="z-index: 0" style="width:100%;position:absolute;">
                            <tr>
                                <td width="30%" align="right" class="LabelRight">Current Password :
                                </td>
                                <td width="70%">
                                    <asp:TextBox ID="txtCurrentPassword" runat="server" TabIndex="1" MaxLength="15" CssClass="dynamictextpwd"
                                        Width="200px"></asp:TextBox>
                                    <span style="color: red">*</span>
                                    <ajax:FilteredTextBoxExtender ID="fteCurrentPassword" runat="server" TargetControlID="txtCurrentPassword"
                                        FilterType="lowercaseLetters,UppercaseLetters,numbers,Custom" ValidChars=".-_!@#$%^&*()<>" />


                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="LabelRight">New Password :
                                </td>
                                <td width="22%">
                                    <asp:TextBox ID="txtNewPassword" runat="server" TabIndex="2" MaxLength="15" CssClass="dynamictextpwd"
                                        Width="200px"></asp:TextBox>
                                    <span style="color: red">*</span>
                                    <ajax:FilteredTextBoxExtender ID="fteNewPassword" runat="server" TargetControlID="txtNewPassword"
                                        FilterType="lowercaseLetters,UppercaseLetters,numbers,Custom" ValidChars=".-_!@#$%^&*()<>" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="LabelRight">Confirm Password :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConfirmPassword" runat="server" Width="200px" CssClass="dynamictextpwd"
                                        TabIndex="3" MaxLength="15"></asp:TextBox>
                                    <span style="color: red">*</span>
                                    <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtConfirmPassword"
                                        FilterType="lowercaseLetters,UppercaseLetters,numbers,Custom" ValidChars=".-_!@#$%^&*()<>" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" width="45%" class="LabelRight">Security Question :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlPasswordHintQuestion" runat="server" TabIndex="4" Width="200px" CssClass="dllCsspwd"  onchange="Clear();"
                                        >
                                    </asp:DropDownList><span style="color: red">*</span>

                                </td>
                            </tr>
                            <tr>
                                <td width="22%" align="right" class="LabelRight">Answer :
                                </td>
                                <td width="20%">
                                    <asp:TextBox ID="txtAnswer" runat="server" TabIndex="5" MaxLength="20" Width="200px" CssClass="dynamictextpwd"></asp:TextBox><span style="color: red">*</span>
                                    <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtAnswer"
                                        FilterType="numbers,lowercaseLetters,UppercaseLetters,Custom" ValidChars=".-_!@#$%^&*()<> " />
                                </td>
                            </tr>
                            <tr>
                                <td width="22%" align="right" class="LabelRight">First Name :
                                </td>
                                <td width="20%">
                                    <asp:TextBox ID="txtFirstName" runat="server" MaxLength="50" Width="200px" CssClass="dynamictextpwd"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td width="22%" align="right" class="LabelRight">Middle Name:
                                </td>
                                <td width="20%">
                                    <asp:TextBox ID="txtMiddleName" runat="server" Width="200px" CssClass="dynamictextpwd"
                                        MaxLength="15"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="LabelRight">Last Name :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLastName" runat="server" MaxLength="50" Width="200px" CssClass="dynamictextpwd"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="LabelRight">Email :
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="txtEmail" runat="server" TabIndex="6" MaxLength="50" Width="200px" CssClass="dynamictextpwd"></asp:TextBox>
                                    <span style="color: red">*</span>

                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="LabelRight">Mobile :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtExtension" MaxLength="4" runat="server" Width="30px" type="text" CssClass="dynamictextpwd"
                                        TabIndex="7" />
                                                <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtExtension"
                                                    FilterType="numbers,Custom" ValidChars="" />
                                    <asp:TextBox ID="txtPhone" MaxLength="10" runat="server" type="text" Width="163px" CssClass="dynamictextpwd"
                                        TabIndex="8" /><%--Code modified on March 05-2015,Subhashini--%>
                                    <span style="color: red">*</span>
                                    <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtPhone"
                                        FilterType="numbers,Custom" ValidChars="" />
                                </td>
                            </tr>
                          
                        </table>
                    </td>
                  
                </tr>
                 <%-- <tr>
                                <td align="center" >
                                    <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr align="center">
                                <td >
                                    
                                    
                                    <asp:Button ID="btnSubmit" runat="server" Height="25px" OnClick="btnSubmit_Click"
                                        TabIndex="9" Text="Save"
                                        ValidationGroup="validationUser"
                                        OnClientClick="return checkChangePassword('ctl00_ContentPlaceHolder1_hiddenValue');" CssClass="button" />&nbsp;
                                            <asp:Button ID="btnReset" runat="server" Height="25px" OnClick="btnReset_Click" TabIndex="10"
                                                CausesValidation="false" Text="Reset" CssClass="button" />&nbsp;
                                            <asp:Button ID="btnClose" runat="server" Text="Close" Height="25px"
                                                TabIndex="11" OnClick="btnClose_Click" CausesValidation="false" CssClass="button" />
                                    &nbsp;
                                          
                                        
                                </td>
                            </tr>--%>
            </table>
                </center>
            </div>
            <%--Code remodified on April 6-2015,Subhashini--%>
            <div style="width: 18%; position: relative; text-align: center; float: left; vertical-align: top">
                <br />

                <asp:Button ID="btnChangePasswordHistory" runat="server" Text="Audit Trail"
                    TabIndex="11" OnClick="btnChangePasswordHistory_Click" CausesValidation="false" CssClass="button" Style="position: absolute; z-index: 1200" />
                <asp:HiddenField ID="HiddenField1" runat="server" />

            </div>
        </div>
        <%--Code remodified on April 6-2015,Subhashini--%>
        <div style="text-align: center; width: 100%; vertical-align: top;">
            <table cellspacing="1" cellpadding="5" style="z-index: 0; margin-left: 0px; margin-right: 0px; text-align: center"
                width="100%" align="center">
                <tr>
                    <td>
                        <asp:Label ID="lblmsg" runat="server" Text=" " ForeColor="Red"></asp:Label>
                        <br />

                    </td>
                </tr>
                <tr>
                    <td>

                        <center>
                                    <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click"
                                        TabIndex="9" Text="Save"
                                        ValidationGroup="validationUser"
                                        OnClientClick="return checkChangePassword('ctl00_ContentPlaceHolder1_hiddenValue');" CssClass="button" />&nbsp;
                                            <asp:Button ID="btnReset" runat="server" OnClick="btnReset_Click" TabIndex="10"
                                                CausesValidation="false" Text="Reset" CssClass="button" />&nbsp;
                                            <asp:Button ID="btnClose" runat="server" Text="Close"
                                                TabIndex="11" OnClick="btnClose_Click" CausesValidation="false" CssClass="button" />
                           </center>
                    </td>
                </tr>
            </table>

        </div>
        <br />
        <div style="float: left; width: 100%; position: relative">
            <asp:Panel ID="pnlNewPassword" runat="server" Height="310px" Width="550px" BackColor="white"
                BorderColor="Black" Style="border: 1px solid; display: none;">
                <div style="width: 100%; background: #0099C8; height: 35px; color: White; vertical-align: middle">
                    <br />
                    <b class="lbltext">Audit Trail</b>
                </div>
                <br />
                <table align="center">
                    <tr>
                        <td colspan="2" align="center">
                            <div id="div1" style="width: 520px; height: 200px; align: center; overflow-x: auto; overflow-y: auto">
                                <asp:GridView ID="GridLoginAudit" runat="server" AutoGenerateColumns="False"
                                    BorderColor="#E7E7FF" BorderStyle="Solid" BorderWidth="1px" EnableModelValidation="True"
                                    Font-Size="12px" GridLines="None"
                                    Style="margin-left: 0px" Width="500px" PageSize="15">
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="LogTime" ItemStyle-Width="100px">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblLogTime" runat="server" BackColor="#CFCFCF" ForeColor="#0099C8" Text="Modified Date"></asp:Label><%--Code modified on March 05-2015,Subhashini--%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="LabelLogTime" runat="server" Text='<%# Eval("LogTime") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" ItemStyle-Width="100px">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblAction" runat="server" Text="Action" BackColor="#CFCFCF" ForeColor="#0099C8"></asp:Label><%--Code modified on March 05-2015,Subhashini--%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="LabelAction" runat="server" Text='<%# Eval("Action") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Comments" ItemStyle-Width="100px">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblComments" runat="server" Text="Comments" BackColor="#CFCFCF" ForeColor="#0099C8"></asp:Label><%--Code modified on March 05-2015,Subhashini--%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="LabelComments" runat="server" Text='<%# Eval("Comments") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>

                                    </Columns>
                                    <EditRowStyle BackColor="#2461BF" />
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#CFCFCF" Font-Bold="True" ForeColor="#0099C8" Height="28px" />
                                    <%--Code remodified on March 10-2015,Subhashini--%>
                                    <PagerStyle BackColor="#CCCCCC" Font-Bold="true" Font-Underline="true" ForeColor="White"
                                        HorizontalAlign="Left" />
                                    <RowStyle BackColor="#EEEEEE" />
                                    <SelectedRowStyle BackColor="#D1DDF1" CssClass="gridSelected" />
                                </asp:GridView>
                            </div>
                            <br />
                            <asp:Button ID="btnCancel" runat="server" Text="Close" Width="100px" CssClass="button" />
                            <br />
                            <%--Code modified on March 05-2015,Subhashini--%>
                        </td>
                    </tr>


                </table>

            </asp:Panel>
        </div>
    </div>
    <ajax:ModalPopupExtender ID="ModalPopupExtender2" TargetControlID="HiddenField2" PopupControlID="pnlNewPassword"
        CancelControlID="btnCancel" BackgroundCssClass="overlay_back"
        DropShadow="false" runat="server">
    </ajax:ModalPopupExtender>
    <asp:HiddenField ID="HiddenField2" runat="server" />

    <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
</asp:Content>
