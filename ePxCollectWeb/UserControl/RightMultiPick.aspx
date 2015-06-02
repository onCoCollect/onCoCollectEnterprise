﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RightMultiPick.aspx.cs" Inherits="ePxCollectWeb.UserControl.RightMultiPick" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <!--Style Sheets-->
    <link rel="stylesheet" type="text/css" href="~/Style/style.css" />
    <link rel="stylesheet" type="text/css" href="~/Style/menu.css" />
    <script type="text/javascript">



        function GetSelectedValue() {

          
            var chkBox = document.getElementById("<%=lstValues.ClientID%>");

            var checkbox = chkBox.getElementsByTagName("input");

            var objTextBox = document.getElementById("<%=txtChkValue.ClientID%>");
            var SelVals = "";
            var counter = 0;

            objTextBox.value = "";
            var bval = true;
            for (var i = 0; i < checkbox.length; i++) {

                if (checkbox[i].checked) {
                    bval = false;

                    var chkBoxText = checkbox[i].parentNode.getElementsByTagName('label');

                    if (objTextBox.value == "") {

                        objTextBox.value = chkBoxText[0].innerHTML;



                    }

                    else {

                        objTextBox.value = objTextBox.value + "," + chkBoxText[0].innerHTML;

                    }

                }


            }
            if (bval == true)
                objTextBox.value = '';

            SelVals = objTextBox.value;
           
            return SelVals;

            //window.returnValue = objTextBox.value;
            ////alert(bval);
            ////alert(objTextBox.value);
            //window.close();

        }

        function CloseDialog() {
            window.close();
        }
        function CloseDialogWithValue() {
            var dpt = document.getElementById("<%=lstValues.ClientID%>");
            var SelVals = "";
            //alert("hi");
            //alert(dpt.options[dpt.selectedIndex].value);
            SelVals = "";
            //alert(dpt.rows.length);
            for (var i = 0; i <= dpt.rows.length - 1; i++) {
                if (dpt.rows[i].innerHTML.indexOf("CHECKED") != -1) {
                    SelVals += dpt.rows[i].innerText + ","; //dpt.options[dpt.selectedIndex].value;
                    //alert(SelVals);
                }
            }

            SelVals = SelVals.substring(0, SelVals.length - 1)
            ////alert(SelVals);
            //window.returnValue = SelVals;

            //window.close();

            return SelVals;
           

        }
    </script>
    <style type="text/css">
        #btnOk {
            width: 85px;
        }
    </style>
</head>
<body bgcolor="#FFFFFF">
    <form id="form1" runat="server">
        <div class="entryOuterDiv">
            <div class="entryArea">
                <div>
                    <div style="left: 25%; width: 60%; position: relative">
                        <%--Code modified on April 7,2015-Subhashini--%>

                        <%--<asp:CheckBoxList ID="CheckBoxList1" runat="server">

<asp:ListItem Value="1">Item1</asp:ListItem>

<asp:ListItem Value="2">Item2</asp:ListItem>

<asp:ListItem Value="3">Item3</asp:ListItem>

<asp:ListItem Value="4">Item4</asp:ListItem>

</asp:CheckBoxList>--%>

                        <%--<asp:Button ID="Button1" runat="server" Text="Button" OnClientClick="return GetSelectedValue();" />--%>
                        <asp:HiddenField ID="txtChkValue" runat="server"></asp:HiddenField>
                        <%--<asp:TextBox ID="txtChkValue" runat="server" CssClass="dynamictext" ></asp:TextBox>--%>
                        <br />
                        <%--Code modified on April 7,2015-Subhashini--%>
                        <div style="border-style: solid; border-color: inherit; border-width: thin; overflow: auto; width: 100%; position: relative; height: 275px">
                            <asp:CheckBoxList ID="lstValues" runat="server" CssClass="chkBoxList" Font-Names="Verdana" Font-Size="11px">
                            </asp:CheckBoxList>
                        </div>
                        <br />

                    </div>
                    <%--Code modified on April 7,2015-Subhashini--%>
                    <div style="left: 25%; width: 60%; position: relative; text-align: center">
                        <asp:Button ID="btnOk" runat="server" Text="Ok" OnClientClick="GetSelectedValue();" Style="display: none;" CssClass="button" />
                       <%-- <input id="btnOk" type="button" value="Ok" onclick="return GetSelectedValue();" class="button" />--%>
                        <input id="btnClose" type="button" value="Close" onclick="CloseDialog();" class="button" style="display: none;" />
                    </div>
                </div>
            </div>
    </form>
</body>
</html>
