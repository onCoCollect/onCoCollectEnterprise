<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="RightPickwithGrouping.aspx.cs"
    Inherits="ePxCollectWeb.UserControl.RightPickwithGrouping" %>

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

            //var dpt = document.getElementById("txtValue");
            //            //alert(dpt);
            //            if (dpt !=null) {
            //                dpt = "";
            //            }

            if (dpt != null) {
                strVal = dpt.value;
            }
            else {
                var dpt = document.getElementById("ddlSiteOfPrimary");

                for (var i = 0; i <= dpt.options.length - 1; i++) {
                    if (dpt.options[i].selected == true) {
                        strVal += dpt.options[i].value + ","; //dpt.options[dpt.selectedIndex].value;
                    }
                }
                strVal = strVal.substring(0, strVal.length - 1);
            }
            return strVal;

            //window.returnValue = strVal;
            //window.close()


        }
        function display() {
            var dpt = document.getElementById("ddlSiteOfPrimary");
            //alert(dpt.options[dpt.selectedIndex].value);            
            var strVal = "";
            for (var i = 0; i <= dpt.options.length - 1; i++) {
                if (dpt.options[i].selected == true) {
                    strVal += dpt.options[i].value + ","; //dpt.options[dpt.selectedIndex].value;
                }
            }
            var txtDoc = document.getElementById("lblcurrentlypicked");
            if (txtDoc != null) {
                txtDoc.innerText = strVal.substring(0, strVal.length - 1);
            }
        }

        function CloseDialogOnCloseButton() {
            window.close()
        }
        <%--function validate() {

            var field = document.getElementById("<%=txtValue.ClientID%>");
            var val = field.value.replace(/^[\s!@#$%^&*()0-9{}|+_)~`<>,.]+|\s+$/, '');

            if (val.length == 0) {
                document.getElementById("<%=txtValue.ClientID%>").value = "";
                // alert('Please Enter valid text');
            }

        }--%>
    </script>
</head>
<body bgcolor="#FFFFFF">
    <form id="form1" runat="server">
        <div class="entryOuterDiv">
            <div class="entryArea" style="width: 280px">
                <div>
                    <asp:Label ID="Label1" runat="server" Text="Previously picked:" CssClass="lbltext"></asp:Label>
                    <asp:Label ID="lblPreviouslypicked" runat="server" Text="" CssClass="lbltext" Font-Bold="true"></asp:Label>
                </div>
                <br />
                <div>
                    <asp:Label ID="sdsd" runat="server" Text="Site of Primary : By Groups" CssClass="lbltext"></asp:Label>
                    <asp:DropDownList ID="ddlGroup" runat="server" Width="250px" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged" CssClass="dllCss"
                        AutoPostBack="True">
                    </asp:DropDownList>
                </div>
                <div class="entryOuterDiv" style="width: 280px;">
                    <asp:Label ID="Label3" runat="server" Text="Site of Primary" CssClass="lbltext"></asp:Label>
                    <asp:DropDownList ID="ddlSiteOfPrimary" runat="server" Width="250px" onchange="display();" CssClass="dllCss">
                    </asp:DropDownList>
                </div>
                <br />
                <div>
                    <asp:Label ID="Label2" runat="server" Text="Currently picked:" CssClass="lbltext"></asp:Label>
                    <asp:Label ID="lblcurrentlypicked" runat="server" Text="" CssClass="lbltext" Font-Bold="true"></asp:Label>
                </div>

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
