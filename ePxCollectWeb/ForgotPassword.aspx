﻿<%@ Page Title="" Language="C#" AutoEventWireup="True" CodeBehind="ForgotPassword.aspx.cs" Inherits="ePxCollectWeb.ForgotPassword" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="~/Style/style.css" />
    <script language="javascript" type="text/javascript" src="../Scripts/backfix.min.js"> </script>
    <script type="text/javascript">

        function preventBack() {
            window.history.forward();
        }
        setTimeout("preventBack()", 0);


        window.onunload = function () {
            null
        };


        var ClickBkButton = false;

        function noBack() {
            if (document.referrer == null || document.referrer == "") {
                document.location.href = "login.aspx";
            } window.history.forward(); setTimeout("noBack()", 500);
        }
        noBack();
        window.onload = noBack;
        window.onpageshow = function (evt) { if (evt.persisted) noBack() }

        window.onunload = function () {
            void (0)
        }



        bajb_backdetect.OnBack = function () {
            ClickBkButton = true;
            if (navigator.appName.indexOf("Microsoft") != -1)
                window.history.forward(1);
            else
                window.history.forward(-1);
        }

    </script>
    <script type="text/javascript">

        function checkChangePassword() {

            if (!checkEmpty('txtNewPassword', 'Fields marked with asterisk (*) are required.')) {

                document.getElementById('txtNewPassword').value = '';

                return false;

            }

            if (!CheckPassword('txtNewPassword')) { // check to make sure all characters are valid

                document.getElementById('lblmsg').innerHTML = "New password should have at least one numeric digit and a special character (Password Length:8-15).";

                document.getElementById('txtNewPassword').value = '';

                document.getElementById('txtNewPassword').focus();

                return false;

            }


            if (!checkEmpty('txtConfirmPassword', 'Fields marked with asterisk (*) are required.')) {

                document.getElementById('txtConfirmPassword').value = '';

                return false;

            }

            if (!CheckPassword('txtConfirmPassword')) { // check to make sure all characters are valid

                document.getElementById('lblmsg').innerHTML = "Confirm password should have at least one numeric digit and a special character (Password Length:8-15).";

                document.getElementById('txtConfirmPassword').value = '';

                document.getElementById('txtConfirmPassword').focus();

                return false;

            }

            if (document.getElementById('txtNewPassword').value != document.getElementById('txtConfirmPassword').value) {

                document.getElementById('lblmsg').innerHTML = "New password and Confirm password should match.";/* Code modified on March 17,2015*/

                document.getElementById('txtNewPassword').value = '';/* Code added on March 17,2015*/

                document.getElementById('txtConfirmPassword').value = '';

                document.getElementById('txtNewPassword').focus();/* Code modified on March 17,2015*/


                return false;

            }


            return true;
        }


        function checkEmpty(id, msg) {

            var obj = document.getElementById(id);

            if (obj.value == null || obj.value == '' || obj.value.trim().length == 0) {

                document.getElementById('lblmsg').innerHTML = msg;

                obj.focus();

                return false;

            }

            else

                return true;

        }

        function allValidPasswordChars(id) {

            var obj = document.getElementById(id).value;

            var parsed = true;

            var validchars = "abcdefghijklmnopqrstuvwxyz0123456789.-_!@#$%^&*() ";

            for (var i = 0; i < obj.length; i++) {

                var letter = obj.charAt(i).toLowerCase();

                if (validchars.indexOf(letter) != -1)

                    continue;

                parsed = false;

                break;



            }


            return parsed;



        }

        function CheckPassword(id) {

            var obj = document.getElementById(id).value;

            var parsed = true;

            var paswd = /^(?=.*[0-9])(?=.*[!@#$%^&*()<>-_])[a-zA-Z0-9!@#$%^&*()<>-_]{8,15}$/;

            if (obj.match(paswd)) {


                parsed = true;



            }


            else {


                parsed = false;



            }


            return parsed;


        }
        function checkRedirectPage() {
            alert('Password changed successfully.Redirecting to Login Page.');
            window.location("Login.aspx");
        }
    </script>
</head>

<body>
    <!--style="height: 456px; width: 501px"> -->
    <form id="frmLogin" runat="server">
        <ajax:ToolkitScriptManager ID="scriptManager" runat="server" AsyncPostBackTimeout="3600" CombineScripts="False"></ajax:ToolkitScriptManager>
        <div class="contain">
            <div class="loginheader">
                <img src="images/logo.png" alt="logo" onmousedown="return false;" />
                <div id='menu'>
                </div>
                <div id="userDetail">
                </div>
            </div>
            <div class="content">
                <div class="data">
                    <div id="loginForm" style="margin-top:20px">

                        <table align="center" width="50%">
                            <tr>
                                <td align="right" class="NewLabelRight">UserID&nbsp;
                                </td>
                                <td align="left">

                                    <asp:TextBox ID="txtUserId" runat="server" CssClass="dynamictext" MaxLength="10"></asp:TextBox>
                                    <span style="color: red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="NewLabelRight">Security Question&nbsp;
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlPasswordHintQuestion" runat="server" TabIndex="4" Width="200px" CssClass="dllCss"
                                        AutoPostBack="True">
                                    </asp:DropDownList><span style="color: red">*</span>

                                </td>
                            </tr>                            
                            <tr>                                
                                <td align="right" class="NewLabelRight">Answer&nbsp;
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtAnswer" runat="server" TabIndex="5" MaxLength="20" Width="200px" CssClass="dynamictext"></asp:TextBox><span style="color: red">*</span>
                                    <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtAnswer"
                                        FilterType="numbers,lowercaseLetters,UppercaseLetters,Custom" ValidChars=".-_!@#$%^&*()<> " />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                     <div id="divChangePassword" runat="server" visible="false" >
                            <table align="center" width="100%">
                                <tr>
                                    <td align="right" width="32.5%" class="NewLabelRight">New Password&nbsp;
                                    </td>
                                    <td align="left">
                                       
                                        <asp:TextBox ID="txtNewPassword" runat="server" TabIndex="2" MaxLength="15" CssClass="dynamictextpwd" TextMode="Password"
                                            Width="200px"></asp:TextBox>
                                        <span style="color: red">*</span>
                                        <ajax:FilteredTextBoxExtender ID="fteNewPassword" runat="server" TargetControlID="txtNewPassword"
                                            FilterType="lowercaseLetters,UppercaseLetters,numbers,Custom" ValidChars=".-_!@#$%^&*()<>" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="NewLabelRight">Confirm Password&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtConfirmPassword" runat="server" Width="200px" CssClass="dynamictextpwd" TextMode="Password"
                                            TabIndex="3" MaxLength="15"></asp:TextBox>
                                        <span style="color: red">*</span>
                                        <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtConfirmPassword"
                                            FilterType="lowercaseLetters,UppercaseLetters,numbers,Custom" ValidChars=".-_!@#$%^&*()<>" />
                                    </td>
                                </tr>                               
                            </table>
                                </div>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="false" ForeColor="Red" CssClass="lbltext"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Button ID="btnNewPassword" runat="server" Text="Submit" Width="100px" OnClick="btnNewPassword_Click" CssClass="button" />
                                    &nbsp;
                                   <asp:Button ID="btncancel" runat="server" Text="Cancel" Width="100px" OnClick="btncancel_Click" CssClass="button" />
                               <asp:HiddenField ID="hdnUserId" runat="server" />
                                     </td>
                            </tr>
                            <%-- <tr>
                                    <td colspan="2" align="center">
                                        <asp:Label ID="lblmsg" runat="server" ForeColor="Red" CssClass="lbltext"></asp:Label>


                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Button ID="btnSave" runat="server" Text="Submit" Width="100px" CssClass="button" OnClick="btnSave_Click" OnClientClick="return checkChangePassword();"></asp:Button>
                                        <asp:Button ID="btnNo" runat="server" Text="Cancel" Width="100px" CssClass="button" OnClick="btnNo_Click"></asp:Button>
                                         
                                    </td>
                                </tr>--%>
                          
                        </table>
                        <br />
                        <%--Code modified on March 17-2015,Subhashini--%>
                    </div>
                </div>
            </div>
            <div style="height: 320px;"></div>
            <div class="footer noborder">
                Best viewed with Internet Explorer <span>All rights Reserved OncoCollect Enterprise<sup>TM</sup></span><%--Code modified on March 17-2015,Subhashini--%>
            </div>
    </form>
</body>
</html>
