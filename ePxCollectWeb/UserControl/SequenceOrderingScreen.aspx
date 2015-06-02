<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="SequenceOrderingScreen.aspx.cs"
    Inherits="ePxCollectWeb.UserControl.SequenceOrderingScreen" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <base target="_self" />
    <!--Style Sheets-->
    <link rel="stylesheet" type="text/css" href="~/Style/style.css" />
    <link rel="stylesheet" type="text/css" href="~/Style/menu.css" />
    <script type="text/javascript">
        function CloseDialog() {

            //            //alert("Closing");
            var strVal = "";

            //var dpt = document.getElementById("lblValuePicked");
            ////alert(dpt.value);
            //if (dpt != null) {
            //    dpt = "";
            //}

            //if (dpt != null) {
            //    strVal = dpt.value;
            //}
            //else
            {
                var dpt = document.getElementById("lstAssigned");

                for (var i = 0; i <= dpt.options.length - 1; i++) {
                    if (strVal == '')
                        strVal += dpt.options[i].value;
                    else
                        strVal += "-->" + dpt.options[i].value; //dpt.options[dpt.selectedIndex].value;

                }

            }

            return strVal;
            //window.returnValue = strVal;
            //window.close()


        }
        function displaySelect() {

            var txtDoc = document.getElementById("lblValuePicked");
            var dpt = document.getElementById("lstAvailable");
            //alert(dpt.options[dpt.selectedIndex].value);            
            var strVal = txtDoc.value;
            for (var i = 0; i <= dpt.options.length - 1; i++) {
                if (dpt.options[i].selected == true) {
                    strVal = strVal + "-->" + dpt.options[i].value + ","; //dpt.options[dpt.selectedIndex].value;
                }
            }

            if (txtDoc != null) {
                txtDoc.value = strVal.substring(0, strVal.length - 1);
            }
        }

        function CloseDialogOnCloseButton() {
            window.close()
        }

    </script>
</head>
<body bgcolor="#FFFFFF">
    <form id="form1" runat="server">
        <div class="entryOuterDiv">
            <div class="entryArea">

                <table align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center" class="labelRA" style="width: 272px">
                            <asp:ListBox ID="lstAvailable" runat="server" Height="120px" Width="250px"
                                CssClass="dllCss" SelectionMode="Multiple"></asp:ListBox>
                        </td>
                        <td align="center">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnSelect" runat="server" Text=" > " Width="50px" OnClick="btnSelect_Click" CssClass="button" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnUnSelect" runat="server" Text=" < " Width="50px" OnClick="btnUnSelect_Click" CssClass="button"
                                            SkinID="0" />
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td>
                                        <asp:Button ID="btnSelectAll" runat="server" Text=" >> " Width="50px" OnClick="btnSelectAll_Click" CssClass="button" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnUnSelectAll" runat="server" Text=" << " Width="50px" OnClick="btnUnSelectAll_Click" CssClass="button" />
                                    </td>
                                </tr>--%>
                            </table>
                        </td>
                        <td align="center" class="labelRA" style="height: 42px; width: 289px;">
                            <asp:ListBox ID="lstAssigned" runat="server" Height="120px" Width="250px" SelectionMode="Multiple" CssClass="dllCss"></asp:ListBox>
                        </td>

                    </tr>
                </table>
            </div>
            <br />
            <div>
                <asp:Label ID="Label4" runat="server" CssClass="LabelRight">Value Picked Order : </asp:Label><br />
                <asp:TextBox ID="lblValuePicked" runat="server" TextMode="MultiLine" Height="50px" ReadOnly="true" Width="500px" Font-Bold="True" CssClass="dynamictext" align="left"></asp:TextBox>

            </div>
            <br />
            <div align="center">
                <asp:HiddenField runat="server" ID="hdnSelectedValue" />
                <input id="btnClose" type="button" value="OK" style="width: 100px; display: none;" onclick="CloseDialog();" class="button" />
                <input id="btnClosePopup" type="button" value="Close" style="width: 100px; display: none;" onclick="CloseDialogOnCloseButton();" class="button" />
            </div>
        </div>

    </form>
</body>
</html>
