<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompositeForm.aspx.cs" Inherits="ePxCollectWeb.CompositeForm" EnableSessionState="True" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <!--Style Sheets-->
    <link rel="stylesheet" type="text/css" href="~/Style/style.css" />
    <link rel="stylesheet" type="text/css" href="~/Style/menu.css" />
    <script type="text/javascript" src="<%=Page.ResolveUrl("~/Scripts/jquery-1.8.2.min.js")%>"></script>
    <script type="text/javascript" src="<%=Page.ResolveUrl("~/Scripts/jquery-ui-1.11.2.custom/jquery-ui.min.js")%>"></script>
    <link href="~/Scripts/jquery-ui-1.11.2.custom/jquery-ui.min.css" rel="stylesheet" />

    <script type="text/javascript">
        function handleKey() {
            //alert('hello');
            return false;
        }
        function CloseDialog() {
            window.close()
        }
        $(document).ready(function () {
            if ($('#<%=HdnClicks.ClientID%>').val() == "False") {
                $("#entryDiv").css({ "visibility": "visible" });                
            }
        });

        function getFormType() {
            $("#entryDiv").css({ "visibility": "hidden" });
            var widths = $(window).width();
            var heights = $(window).height();
            if (eval(heights) > 600 & eval(widths) > 1200) {
                window.location("Login.aspx");
                $('#<%=HdnClicks.ClientID%>').val("True");
            }

        }
        function fnPopWindowC(fName, ColName) {
            if (window.showModalDialog) {
                var opt = 'dialogWidth:650px; dialogHeight:400px; center:yes; scroll:no; status:no';
                window.showModalDialog(fName, ColName, opt);
                return false;
            } else {
                var opt = 'dialogWidth:650px; dialogHeight:400px; center:yes; scroll:no; status:no';
                window.showModalDialog(fName, ColName, opt);
                return false;
            }
        }

            //Opening Composite Form  in Jquery Dialog
            function fnPopWindowC(fName, ColName) {

                var $iframe = $('#onCoRightPickDiag_composite_iframe');
                var url = fName;
                if ($iframe.length) {
                    $iframe.attr('src', url);
                    $("#onCoRightPickDiag_composite").dialog(
                        {
                            model: true,
                            width: '60%',
                            minHeight: '400',
                            resizable: true,
                            stack: true,
                            title: 'Select',
                            buttons: {
                                "OK": function () {
                                    var retVal = $("#onCoRightPickDiag_composite_iframe")[0].contentWindow.CloseDialog();
                                    $(this).dialog('close');
                                },
                                "Close": function () {
                                    $(this).dialog('close');
                                }
                            }
                        }
                        );
                    return false;
                }
                return true;

            }

            //Replace &+ PopupMultiSelect using JQuery Dialog
            function fnPopupC(tName, ctlName) {

                var ctl = document.getElementById(ctlName);

                if (ctl != null) {
                    strVal = document.getElementById(ctlName).value.toString();
                }

                strVal = strVal.replace(/&/gi, '*ampersand*')
                strVal = strVal.replace(/\+/gi, '*plus*')

                var $iframe = $('#onCoRightPickDiag_composite_iframe');
                var url = "UserControl/PopupMultiSelect.aspx?tName=" + tName;
                if ($iframe.length) {
                    $iframe.attr('src', url);
                    $("#onCoRightPickDiag_composite").dialog(
                        {
                            model: true,
                            width: '60%',
                            minHeight: '400',
                            resizable: true,
                            stack: true,
                            title: 'Select',
                            buttons: {
                                "OK": function () {
                                    var retVal = $("#onCoRightPickDiag_composite_iframe")[0].contentWindow.CloseDialog();
                                    try {
                                        if (retVal == null) {
                                            document.getElementById(ctlName).value = document.getElementById(ctlName).value;
                                        } else
                                            if (retVal == undefined) {
                                                document.getElementById(ctlName).value = document.getElementById(ctlName).value;
                                            } else
                                                if (retVal != "") {
                                                    document.getElementById(ctlName).value = retVal.toString();
                                                }
                                                else
                                                    if (retVal == "") {
                                                        document.getElementById(ctlName).value = "";;
                                                    }
                                    }
                                    catch (e) {
                                    }

                                    $(this).dialog('close');
                                },
                                "Close": function () {
                                    $(this).dialog('close');
                                }
                            }
                        }
                        );
                    return false;
                }
                return true;



            }

            //Replace &+ FieldValueSelect  using JQuery Dialog
            function fnSingleSelectC(tName, ctlName, strVal) {
                var ctl = document.getElementById(ctlName);
                if (ctl != null) {
                    strVal = document.getElementById(ctlName).value.toString();
                }

                strVal = strVal.replace(/&/gi, '*ampersand*')
                strVal = strVal.replace(/\+/gi, '*plus*')

                var $iframe = $('#onCoRightPickDiag_composite_iframe');
                var url = "FieldValueSelect.aspx?FN=" + tName + "&Val=" + strVal;
                if ($iframe.length) {
                    $iframe.attr('src', url);
                    $("#onCoRightPickDiag_composite").dialog(
                        {
                            model: true,
                            width: '60%',
                            minHeight: '400',
                            resizable: true,
                            stack: true,
                            title: 'Select',
                            buttons: {
                                "OK": function () {
                                    var retVal = $("#onCoRightPickDiag_composite_iframe")[0].contentWindow.CloseDialogWithValue();
                                    try {
                                        if (retVal == null) {
                                            document.getElementById(ctlName).value = document.getElementById(ctlName).value;
                                        } else
                                            if (retVal == undefined) {
                                                document.getElementById(ctlName).value = document.getElementById(ctlName).value;
                                            } else
                                                if (retVal != "") {
                                                    document.getElementById(ctlName).value = retVal.toString();
                                                }
                                    }
                                    catch (e) {
                                    }
                                    $(this).dialog('close');
                                },
                                "Close": function () {
                                    $(this).dialog('close');
                                }
                            }
                        }
                        );
                    return false;
                }
                return true;


            }

            //Replace &+ RightMultiPick  using JQuery Dialog
            function fnPopupRightMultiPickC(tName, ctlName) {

                var ctl = document.getElementById(ctlName);
                if (ctl != null) {
                    strVal = document.getElementById(ctlName).value.toString();
                }

                strVal = strVal.replace(/&/gi, '*ampersand*')
                strVal = strVal.replace(/\+/gi, '*plus*')

                var $iframe = $('#onCoRightPickDiag_composite_iframe');
                var url = "UserControl/RightMultiPick.aspx?tName=" + tName + "&Val=" + strVal;
                if ($iframe.length) {
                    $iframe.attr('src', url);
                    $("#onCoRightPickDiag_composite").dialog(
                        {

                            model: true,
                            width: '60%',
                            minHeight: '400',
                            resizable: true,
                            stack: true,
                            title: 'Select',
                            buttons: {
                                "OK": function () {
                                    var retVal = $("#onCoRightPickDiag_composite_iframe")[0].contentWindow.GetSelectedValue();
                                    try {

                                        if (retVal == null) {
                                            document.getElementById(ctlName).value = document.getElementById(ctlName).value;
                                        } else
                                            if (retVal == undefined) {
                                                document.getElementById(ctlName).value = document.getElementById(ctlName).value;
                                            } else
                                                if (retVal != "") {
                                                    document.getElementById(ctlName).value = retVal.toString().replace("undefined", "");
                                                }
                                                else
                                                    if (retVal == "") {
                                                        document.getElementById(ctlName).value = "";;
                                                    }

                                    }
                                    catch (e) {
                                    }
                                    $(this).dialog('close');
                                },
                                "Close": function () {
                                    $(this).dialog('close');
                                }
                            }
                        }
                        );
                    return false;
                }
                return true;

            }

            function fnPopupRightMultiPickOthers(tName, ctlName) {
                var val = document.getElementById(ctlName).value;

                val = val.replace(/&/gi, '*ampersand*');
                val = val.replace(/\+/gi, '*plus*');
                var $iframe = $('#onCoRightPickDiag_composite_iframe');
                //var url = "UserControl/RightMultiPick.aspx?tName=" + tName + "&Val=" + val;
                var url = "UserControl/PopupMultiSelect.aspx";
                if ($iframe.length) {
                    $iframe.attr('src', url);
                    $("#onCoRightPickDiag_composite").dialog(
                        {
                            model: false,
                            width: '60%',
                            minHeight: '400',
                            resizable: true,
                            stack: true,
                            title: 'Select',
                            buttons: {
                                "OK": function () {
                                    var retVal = $("#onCoRightPickDiag_composite_iframe")[0].contentWindow.GetSelectedValue();
                                    try {

                                        if (retVal == null) {
                                            document.getElementById(ctlName).value = document.getElementById(ctlName).value;
                                        } else
                                            if (retVal == undefined) {
                                                document.getElementById(ctlName).value = document.getElementById(ctlName).value;
                                            } else
                                                if (retVal != "") {
                                                    document.getElementById(ctlName).value = retVal.toString().replace("undefined", "");
                                                }
                                                else
                                                    if (retVal == "") {
                                                        document.getElementById(ctlName).value = "";;
                                                    }

                                    }
                                    catch (e) {
                                    }
                                    $(this).dialog('close');
                                },
                                "Close": function () {
                                    $(this).dialog('close');
                                }
                            }
                        }
                        );
                    return false;
                }
                return true;
            }

            //Replace &+ FieldValueMultiSelect  using JQuery Dialog
            function fnMultiSelectC(tName, ctlName, strVal) {
                var ctl = document.getElementById(ctlName);
                if (ctl != null) {
                    strVal = document.getElementById(ctlName).value.toString();
                }
                strVal = strVal.replace(/&/gi, '*ampersand*')
                strVal = strVal.replace(/\+/gi, '*plus*')


                var $iframe = $('#onCoRightPickDiag_composite_iframe');
                var url = "FieldValueMultiSelect.aspx?FN=" + tName + "&Val=" + strVal;
                if ($iframe.length) {
                    $iframe.attr('src', url);
                    $("#onCoRightPickDiag_composite").dialog(
                        {
                            model: true,
                            width: '60%',
                            minHeight: '400',
                            resizable: true,
                            stack: true,
                            title: 'Select',
                            buttons: {
                                "OK": function () {
                                    var retVal = $("#onCoRightPickDiag_composite_iframe")[0].contentWindow.CloseDialogWithValue();
                                    try {

                                        if (retVal == null) {
                                            document.getElementById(ctlName).value = document.getElementById(ctlName).value;
                                        } else
                                            if (retVal == undefined) {
                                                document.getElementById(ctlName).value = document.getElementById(ctlName).value;
                                            } else
                                                if (retVal != "") {
                                                    document.getElementById(ctlName).value = retVal.toString().replace("undefined", "");
                                                }
                                    }
                                    catch (e) {

                                    }
                                    $(this).dialog('close');
                                },
                                "Close": function () {
                                    $(this).dialog('close');
                                }
                            }
                        }
                        );
                    return false;
                }
                return true;


            }


            function SaveCompositeForm() {
                var ReturnValue = document.getElementById("hdnReturnValue").value;
                alert(ReturnValue);
                return ReturnValue;
            }

            function ValidateTextBoxForDataTypeLongINT(evt, value, input, lbltext) {
                var charCode;
                if (evt.keyCode) //For IE
                    charCode = evt.keyCode;
                else if (evt.Which)
                    charCode = evt.Which; // For FireFox
                else
                    charCode = evt.charCode; // Other Browser
                if (charCode > 31 && (charCode < 48 || charCode > 57))
                    return false;
                else
                    return true;
            }
            function ValidateTextBoxForDataTypeTextAlphaNumericCommaSpaceHypen(evt, value, input, lbltext) {

                var charCode;
                if (evt.keyCode) //For IE
                    charCode = evt.keyCode;
                else if (evt.Which)
                    charCode = evt.Which; // For FireFox
                else
                    charCode = evt.charCode; // Other Browser
                if ((charCode >= 48 && charCode <= 57) || (charCode >= 64 && charCode <= 90) || (charCode >= 97 && charCode <= 122) || (charCode == 08 || charCode == 32 || (charCode >= 44 && charCode <= 47) || charCode == 123 || charCode == 125 || charCode == 126 || (charCode >= 40 && charCode <= 42) || charCode == 33 || charCode == 36 || charCode == 37))
                    return true;
                else
                    return false;

            }

            function ValidateTextBoxForDataTypeTextPostalCode(evt, value, input, lbltext) {

                var charCode;
                if (evt.keyCode) //For IE
                    charCode = evt.keyCode;
                else if (evt.Which)
                    charCode = evt.Which; // For FireFox
                else
                    charCode = evt.charCode; // Other Browser
                if ((charCode >= 48 && charCode <= 57) || (charCode >= 64 && charCode <= 90) || (charCode >= 97 && charCode <= 122) || (charCode == 45 || charCode == 40 || charCode == 41))
                    return true;
                else
                    return false;

            }

            function validateKeyup(input) {
                var val = input.value.replace(/^[\s!@#$%^&*(){}|+_)~`<>,.]+|\s+$/, '');

                if (val.length == 0) {
                    input.value = "";
                    // alert('Please Enter valid text');
                }

            }

            function validateKeyupforRTBoostDose(input) {
                var val = input.value.replace(/^[\s!@#$%^&*(){}|+_)~`<>,.]+|\s+$/, '');

                if (val.length == 0) {
                    input.value = "";
                    // alert('Please Enter valid text');
                }

            }
            function ValidateTextBoxForDataTypeSingleIsNumericWithDot1(evt, value, inpu, lbltextt) {
                var charCode;
                if (evt.keyCode) //For IE
                    charCode = evt.keyCode;
                else if (evt.Which)
                    charCode = evt.Which; // For FireFox
                else
                    charCode = evt.charCode; // Other Browser
                if ((charCode >= 48 && charCode <= 57) || (charCode == 46))
                    return true;
                else
                    return false;

                var numericvalue = value;
                var valarr = numericvalue.split(".").length;
                if (valarr > 1 && String.fromCharCode(charCode) == ".") {
                    if (typeof input.selectionStart == "number") {
                        return input.selectionStart == 0 && input.selectionEnd == input.value.length;
                    }
                    else if (typeof document.selection != "undefined") {
                        input.focus();
                        return document.selection.createRange().text == input.value;
                    }
                }
                else {

                    return true;
                }
            }

            function ValidateTextBoxForDataTypeEmail(evt, value, inpu, lbltextt) {
                var charCode;
                var charCode;
                if (evt.keyCode) //For IE
                    charCode = evt.keyCode;
                else if (evt.Which)
                    charCode = evt.Which; // For FireFox
                else
                    charCode = evt.charCode; // Other Browser
                // var e = event || evt;
                // var charCode = e.which || e.keyCode || e.charcode;

                if (charCode == 38 || charCode == 39 || charCode == 42 || charCode == 43 || charCode == 8 || (charCode >= 45 && charCode <= 57) || (charCode == 32) || charCode == 61 || (charCode >= 63 && charCode <= 90) || (charCode == 95) || (charCode >= 97 && charCode <= 122) || charCode == 123 || charCode == 125 || charCode == 126 || (charCode == 9)) {
                    return true;
                }
                else {
                    return false;
                }
            }
            function ValidateTextBoxForDataTypeEmailFormat(evt, value, inpu, lbltextt) {
                var emailtext = inpu.value;
                var reg = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/
                if (reg.test(emailtext)) {
                    return true;
                }
                else {
                    return false;
                }


            }


    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="HdnClicks" runat="server" value="False" />
        <div class="entryOuterDiv" style="width: 100%;">
            <div class="entryArea" id="entryDiv" runat="server"  style="width: 100%">
                <asp:ScriptManager ID="scriptManager" runat="server" />
                <asp:UpdatePanel ID="updPHControls" runat="server" UpdateMode="Conditional" >
                    <ContentTemplate>

                        <asp:Panel runat="server" ID="pnlControls" ScrollBars="Auto" Width="100%">
                            <div id="DivPH" runat="server" class="entryArea" style="width: 100%;">
                                <br />
                                <asp:PlaceHolder ID="phProject" runat="server" ViewStateMode="Disabled"></asp:PlaceHolder>
                            </div>
                        </asp:Panel>

                        <asp:Panel ID="Panel2" runat="server" Width="90%" Height="50px" HorizontalAlign="Center">

                            <table align="center" width="100%" style="display:none">
                                <tr align="center" width="100%">
                                    <td>
                                        <asp:Button ID="btnSave" Text="  Save " runat="server" CssClass="button"
                                            OnClick="btnSave_Click" />
                                        &nbsp; &nbsp;
                                       <asp:Button ID="btnClose" runat="server" CssClass="button" Text="Close" OnClientClick="CloseDialog(); " Style="display: none;" /></td>

                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
    </form>
    <script>
        var cur_ctrlName = "";
        function getValFromDialog(ctlname) {
            document.getElementById('<%= btnSave.ClientID %>').click();
            cur_ctrlName = ctlname;
        }

        function sendValuetoParent(retVal) {

            if (retVal == undefined)
                retVal = "";
            if (retVal == "123123123")
                retVal = "";

            window.parent.document.getElementById(cur_ctrlName).value = retVal;
            window.parent.$("#onCoRightPickDiag").dialog('close');

        }
    </script>

    <div id="onCoRightPickDiag_composite" title="Select" style="display:none;">
        <iframe id="onCoRightPickDiag_composite_iframe" src="UserControl/PopupMultiSelect.aspx" height="400px" width="100%"></iframe>
    </div>
</body>

</html>
