<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="PopupMultiSelect.aspx.cs"
    Inherits="ePxCollectWeb.UserControl.PopupMultiSelect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <!--Style Sheets-->
    <base target="_self" />
    <link rel="stylesheet" type="text/css" href="~/Style/style.css" />
    <link rel="stylesheet" type="text/css" href="~/Style/menu.css" />
    <script src="../Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>

    <script type="text/javascript">


        function getNewFormType() {
            $("#entryDiv").css({ "visibility": "hidden" });
            var widths = $(window).width();
            var heights = $(window).height();
            if (eval(heights) > 600 & eval(widths) > 1200) {
                window.location("../Login.aspx");
                $('#<%=PopUpClicks.ClientID%>').val("True");
                return;
            }
            $("#entryDiv").css({ "visibility": "visible" });
        }


        function CloseDialog() {
            //            //alert("Closing");
            var strVal = "";

            var dpt = document.getElementById("txtValue");
            //            //alert(dpt);
            //            if (dpt !=null) {
            //                dpt = "";
            //            }

            if (dpt != null) {
                strVal = dpt.value;
            }
            else {
                var dpt = document.getElementById("lstValues");

                for (var i = 0; i <= dpt.options.length - 1; i++) {
                    if (dpt.options[i].selected == true) {
                        strVal += dpt.options[i].value + ","; //dpt.options[dpt.selectedIndex].value;
                    }
                }
                strVal = strVal.substring(0, strVal.length - 1);
            }

            return strVal;
            //alert(strVal);
            //window.returnValue = strVal;
            //window.close()


        }
        function display() {
            var dpt = document.getElementById("lstValues");
            //alert(dpt.options[dpt.selectedIndex].value);            
            var strVal = "";
            for (var i = 0; i <= dpt.options.length - 1; i++) {
                if (dpt.options[i].selected == true) {
                    strVal += dpt.options[i].value + ","; //dpt.options[dpt.selectedIndex].value;
                }
            }
            var txtDoc = document.getElementById("txtValue");
            if (txtDoc != null) {
                txtDoc.value = strVal.substring(0, strVal.length - 1);
            }
        }

        function CloseDialogOnCloseButton() {
            window.close()
        }
        function validate() {

            var field = document.getElementById("<%=txtValue.ClientID%>");
            var val = field.value.replace(/^[\s!@#$%^&*(){}|+_)~`<>,.]+|\s+$/, '');

            if (val.length == 0) {
                document.getElementById("<%=txtValue.ClientID%>").value = "";
                // alert('Please Enter valid text');
            }

        }

        function ValidateTextBoxForDataTypeTextAlphaNumericCommaSpaceHypen(evt, value, input, lbltext) {
            //CommaSpaceHypen  {}-@,.~!%$()*/
            var charCode;
            if (evt.keyCode) //For IE
                charCode = evt.keyCode;
            else if (evt.Which)
                charCode = evt.Which; // For FireFox
            else
                charCode = evt.charCode; // Other Browser
            if ((charCode >= 48 && charCode <= 57) || (charCode >= 64 && charCode <= 90) || (charCode >= 97 && charCode <= 122) || (charCode == 08 || charCode == 32 || (charCode >= 44 && charCode <= 47) || charCode == 123 || charCode == 125 || charCode == 126 || (charCode >= 40 && charCode <= 42) || charCode == 33 || charCode == 36 || charCode == 37)) {
                return true;
            }

            else
                return false;

        }
    </script>
</head>
<body bgcolor="#FFFFFF">
    <form id="form1" runat="server">

        <asp:HiddenField runat="server" ID="PopUpClicks" Value="False" />
        <div class="entryOuterDiv" id="entryDiv" runat="server" style="visibility: hidden">
            <asp:ScriptManager runat="server" ID="ScriptManager" />
            <asp:Panel ID="pnl" runat="server" DefaultButton="btnClose">
                <div class="entryArea">
                    <div class="entryOuterDiv" style="width: 390px; height: 309px;">
                        <%--Code modified on April 15,2015-Subhashini--%>
                        <asp:ListBox ID="lstValues" runat="server" Height="250px" onchange="display();" Width="390px" CssClass="dllCss"
                            ViewStateMode="Enabled" OnSelectedIndexChanged="lstValues_SelectedIndexChanged"></asp:ListBox>
                        <br />
                        <br />
                        <asp:Label ID="Label1text" runat="server" Text="Alternatively enter a Text here:" CssClass="LabelRight"></asp:Label>
                        <br />
                        <asp:TextBox ID="txtValue" runat="server" Width="386px" MaxLength="100" onkeyup="return validate()" onkeypress="return ValidateTextBoxForDataTypeTextAlphaNumericCommaSpaceHypen(event,'','','')" CssClass="dynamictext"></asp:TextBox>
                        <br />
                    </div>
                    <br />
                    <div align="center" style="width: 390px;">
                        <asp:HiddenField runat="server" ID="hdnSelectedValue" />
                        <asp:Button runat="server" ID="btnClose" Text="OK" Style="width: 100px; display: none;" OnClientClick="CloseDialog();" CssClass="button" />
                        <input id="btnClosePopup" type="button" value="Close" style="width: 100px; display: none;" onclick="CloseDialogOnCloseButton();" class="button" />
                        <br />
                    </div>
                </div>
            </asp:Panel>
        </div>

    </form>
</body>
</html>
