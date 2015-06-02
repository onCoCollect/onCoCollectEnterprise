<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ePxCollectWeb.Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head>
    <title>OncoCollectEnterprise: Login</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>

    <script src="Scripts/jquery-1.8.2.min.js"></script>
    <script src="Scripts/modernizr.min.js"></script>
    <script src="Scripts/respond.min.js"></script>
    <script src="Scripts/jquery-ui-1.11.2.custom/jquery-ui.js"></script>
    <script src="Scripts/prefixfree.min.js"></script>
    <script src="Scripts/jquery.flip.js"></script>
    <script src="Scripts/login.js"></script>
    <script src="Scripts/WaterMark.min.js"></script>
    <script src="Scripts/backfix.min.js"> </script>

<!--[if (gte IE 9)|!(IE)]><--> 
    <script src="onCoAlert/sweetalert.min.js"></script>
<!--<![endif]-->

    <link href="Style/reset.css" rel="stylesheet" />
    <link href="Style/loginform.css" rel="stylesheet" />
    <link href="Scripts/jquery-ui-1.11.2.custom/jquery-ui.css" rel="stylesheet" />
    <link href="Style/styleNew.css" rel="stylesheet" type="text/css" />
    <link href="onCoAlert/sweetalert.css" rel="stylesheet" />

    <style type="text/css" media="screen">   
        body {
            opacity:0;
        }
	</style>
    
    <script type="text/javascript">
        function myScript() {
            $get("TextBox1").focus();
        }

        function onKeyPressIn() {
            if (window.event.keyCode == 13) {
                var ele = document.getElementById('<%=btnYes.ClientID%>');
                if (ele.style.visibility == "visible") {
                    __doPostBack('<%=this.ClientID%>', 'OnLoad');
                }
            }
        }
    </script>

    <script type="text/javascript">

        function preventBack() {
            window.history.forward();
        }
        setTimeout("preventBack()", 0);
        window.onunload = function () {
            null
        };


        var ClickBkButton = false;

        function noBack() { window.history.forward(); setTimeout("noBack()", 500); }
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


    <script language="javascript">

        function cancelBack() {
            if ((event.keyCode == 8 ||
               (event.keyCode == 37 && event.altKey) ||
               (event.keyCode == 39 && event.altKey))
                &&
               (event.srcElement.form == null || event.srcElement.isTextEdit == false)
              ) {
                event.cancelBubble = true;
                event.returnValue = false;
            }
        }

    </script>

    <script type="text/javascript">
        function showDisclaimer() {
            $("#disclaimerDiag").dialog({
                title: "Disclaimer",
                modal: true,
                //hide: { effect: "slide", duration: 300 },
                //show: { effect: "slide", duration: 300 },
                width: 'auto',
                maxWidth: 600,
                height: 'auto',
                fluid: true,
                position: {
                    my: 'center',
                    at: 'center',
                    of: window
                }
            });
        }

        // on window resize run function
        $(window).resize(function () {
            fluidDialog();
        });

        // catch dialog if opened within a viewport smaller than the dialog width
        $(document).on("dialogopen", ".ui-dialog", function (event, ui) {
            fluidDialog();
        });

        function fluidDialog() {
            var $visible = $(".ui-dialog:visible");
            // each open dialog
            $visible.each(function () {
                var $this = $(this);
                var dialog = $this.find(".ui-dialog-content").data("ui-dialog");
                // if fluid option == true
                if (dialog.options.fluid) {
                    var wWidth = $(window).width();
                    // check window width against dialog width
                    if (wWidth < (parseInt(dialog.options.maxWidth) + 50)) {
                        // keep dialog from filling entire screen
                        $this.css("max-width", "90%");
                    } else {
                        // fix maxWidth bug
                        $this.css("max-width", dialog.options.maxWidth + "px");
                    }
                }
                //reposition dialog
                dialog.option("position", dialog.options.position);
            });

        }
    </script>

<!--[if lt IE 9]>
    <script>
        function showErrMsg(errMsg) {
            alert(errMsg);
        }
    </script>
<![endif]-->
<!--[if (gte IE 9)|!(IE)]><--> 
    <script>
        function showErrMsg(errMsg) {
            onco({
                title: "Sorry!",
                text: "<span style=\"color:#CC0000\">" + errMsg + "<span>",
                html: true,
                //timer: 2000,
                showConfirmButton: true,
            });
        }
    </script>
<!--<![endif]-->
</head>

<!--[if lt IE 9]>
    <body style="background-color:#095DB7">
<![endif]-->
<!--[if (gte IE 9)|!(IE)]><--> 
    <body id="coColBg">
<!--<![endif]-->

    <form id="frmLogin" runat="server" onkeydown="onKeyPressIn();">
        <section>
            <div>
                <img id="coColTitleImg" style="max-width: 457px; min-width: 250px; display: block; width: 100%;" src="images/title.png" alt="" />
            </div>
        </section>
        <section>
            <div class="form-signin" id="card">
                <div class="front">
                    <div class="account-wall">
                        <p>
                            <input id="txtUsrName" class="txtBx" placeholder="User ID" runat="server" maxlength="10" type="text" required autofocus /><span style="color: red">*</span>
                            <input id="txtPasword" class="txtBx" placeholder="Password" runat="server" type="password" accesskey="*" required /><span style="color: red">*</span>
                        </p>
                        <p>
                            <asp:Button ID="submitLogin" CssClass="button" runat="server" Text="Sign in" OnClick="btnLogin_Click" />
                        </p>
                        <p>
                            <asp:Label ID="Label3" runat="server" Visible="False"></asp:Label>
                        </p>
                        <p>
                            <asp:LinkButton ID="lbtnForgotPassword" class="pull-right need-help flip-on" runat="server" Text="Forgot Password? " OnClick="lbtnForgotPassword_Click"></asp:LinkButton>
                        </p>
                        <div id="logImage">
                        </div>
                    </div>
                </div>
<%--                <div class="back">
                    <div class="account-wall">
                        <p>
                            <input type="text" placeholder="User ID" />
                            <input type="text" placeholder="Security question" />
                            <input type="text" placeholder="Answer" />
                        </p>
                        <p>
                            <button class="button" style="width: 150px;" type="button" onclick="doAlertJob()">Submit</button>
                        </p>
                        <p>
                            <a href="#" class="pull-right need-help flip-off">Back to login </a><span class="clearfix"></span>
                        </p>
                    </div>
                </div>--%>
            </div>
        </section>
        <section>
            <div style="margin: 380px auto; float: right">
                <img id="coColRameshImg" style="max-width: 562px; min-width: 250px; display: block; width: 100%;" src="images/ramesh.png" alt="" />
            </div>
        </section>

            <asp:Panel ID="pnlYESNO" runat="server" Height="557px" Width="540px" BackColor="white" HorizontalAlign="Center" BorderWidth="2"
                DefaultButton="btnYes" BorderColor="#2a75a9" Style="display: none;text-align:center;vertical-align:top;position:relative ">
                <div style="background-color: #0099C8; height: 100%;vertical-align:top;border:2px solid #2a75a9" onkeydown="onKeyPressIn();">
                     <div style="width: 100%; background: #0099c8; height: 25px; color: White; font-size: large;line-height: 1.0cm;">
                                    <center>
                                        <b>Disclaimer</b></center>
                                </div>                   
                    <table align="center" style="color: Black; background-color: #0099C8;vertical-align:top;padding:0px;width:100%;position:relative">
                        <tr>
                            <td align="center" style="vertical-align:top">
                                <asp:TextBox ID="TextBox1" runat="server" Height="475px" Width="515px" TextMode="MultiLine"
                                    ReadOnly="True" CssClass="dynamictext" Font-Names="verdana" Font-Size="11px" ></asp:TextBox>                                
                            </td>
                        </tr>
                        </table>
                      <%--  <tr>
                            <td>&nbsp;
                            </td>
                       </tr>--%>
                       <%-- <tr style="width: 100%; background: #0099C8; height: 20px;vertical-align:top">
                            <td align="center" style="vertical-align:top">
                                
                                    <asp:Button ID="btnYes" runat="server" Text="I Agree" Width="100px" OnClick="btnYes_Click" CssClass="button" />
</td><td>&nbsp;&nbsp;
                            <asp:Button ID="btnNo" runat="server" Text="I Disagree" Width="100px" OnClick="btnNo_Click" CssClass="button" />
                                </center>
                            </td>
                        </tr>--%>
                        <%--<tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>--%>
                    
                     <div style="width: 100%;padding:0px; background: #0099C8;border:none; height: 20px; color: White; vertical-align:top;text-align:center;position:relative ">
                         <center>
                          <asp:Button ID="btnYes" runat="server" Text="I Agree" Width="100px" OnClick="btnYes_Click" CssClass="button" />
                                    <%--</td><td>--%>&nbsp;
                            <asp:Button ID="btnNo" runat="server" Text="I Disagree" Width="100px" OnClick="btnNo_Click" CssClass="button" />
                             </center> 
                </div>
                    </div>
            </asp:Panel>
            <ajax:ModalPopupExtender ID="ModalPopupExtender1" TargetControlID="Hidden1" PopupControlID="pnlYESNO"
                BehaviorID="ModalBehaviour" CancelControlID="btnNo" BackgroundCssClass="overlay_back"
                DropShadow="false" runat="server">
            </ajax:ModalPopupExtender>
            <asp:HiddenField ID="Hidden1" runat="server" />



            <asp:Panel ID="pnlMutipleUser" runat="server" Height="150px" Width="400px" BackColor="white"
                DefaultButton="btnYes" BorderColor="#efefed" Style="display: none">
                <div style="background-color: #0099C8; height: 100%">
                    <div style="width: 100%; background: #0099C8; height: 25px; color: White; font-size: x-large;" class="dynamictext">
                        <center>
                            <b>Confirm Mutiple User</b></center>
                    </div>
                    <table align="center" style="color: Black;">
                        <tr>
                            <td>
                                <asp:Label ID="lblMultipleUser" runat="server" Font-Bold="true" Text="You were already logged in from IP : . Do you Still prefer to Force Login now?  "></asp:Label>
                                <br />
                                <asp:Label ID="Label5" runat="server" Font-Bold="true" Text="Note: You will be logged out from the Other System."></asp:Label>

                            </td>
                        </tr>

                        <tr>
                            <td>
                                <center>
                                    <asp:Button ID="btnMutipleUser" runat="server" Text="I Agree" Width="100px" OnClick="btnMutipleUser_Click" CssClass="button" />
                                    <%--</td><td>--%>&nbsp;
                            <asp:Button ID="btnMutipleUserCancel" runat="server" Text="I Disagree" Width="100px" CssClass="button" />
                                </center>
                            </td>
                        </tr>
                        <%--<tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>--%>
                    </table>
                </div>
            </asp:Panel>
            <ajax:ModalPopupExtender ID="ModalPopupExtender2" TargetControlID="HiddenField1" PopupControlID="pnlMutipleUser"
                CancelControlID="btnMutipleUserCancel" BackgroundCssClass="overlay_back"
                DropShadow="false" runat="server">
            </ajax:ModalPopupExtender>
            <asp:HiddenField ID="HiddenField1" runat="server" />


<%--        <div class="footer">
            Best viewed with Internet Explorer <span>All rights Reserved OncoCollect Enterprise<sup>TM</sup></span>
        </div>--%>
        <asp:ScriptManager ID="scriptManager" runat="server" />
        <div id="disclaimerDiag" runat="server" style="display:none; overflow:auto; line-height:10px; font-size:small; font-family:Arial;">
            
        </div>
    </form>
    <script>
        $(document).ready(function () {
            $('body').animate({ opacity: 1 }, 2000);

            $(document).on("click", "#discOK", function () {
                $.ajax(
                 {
                     type: "POST",
                     url: "login.aspx/doRedirect",
                     data: "{}",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     async: true,
                     cache: false,
                     success: function (msg) {
                         window.location = msg.d;
                     },
                     error: function (x, e) {
                         //alert("The call to the server side failed. doCheckLoggedOut. " + x.responseText);
                     }
                 });
            });

            $(document).on("click", "#discNO", function () {
                $("#disclaimerDiag").dialog('close');
            });

            $('#txtPassword').bind('copy paste cut', function (e) {
                e.preventDefault(); //disable cut,copy,paste
            });

            setTimeout("myScript()", 1000);
        });
    </script>
</body>
</html>
