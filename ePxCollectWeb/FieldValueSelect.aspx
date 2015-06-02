<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FieldValueSelect.aspx.cs" EnableEventValidation="false" EnableSessionState="True"
    Inherits="ePxCollectWeb.FieldValueSelect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <link rel="stylesheet" type="text/css" href="~/Style/style.css" />
    <title></title>
    <base target="_self" />
    <script type="text/javascript">
        function CloseDialog() {
            //alert("Closing");
            var dpt = document.getElementById("txtValue");
            //alert(dpt);
            if (dpt == "undefined") {
                dpt = "";
            }
            //window.returnValue = dpt.value;
            //window.opener.update("Test")
            window.close();
        }
        function display(input) {
            var dpt = document.getElementById("lstValues");
            //alert(dpt.options[dpt.selectedIndex].value);            
            var strVal = "";
            for (var i = 0; i <= dpt.options.length - 1; i++) {
                if (dpt.options[i].selected == true) {
                    strVal = dpt.options[i].value + ","; //dpt.options[dpt.selectedIndex].value;
                }
            }
            document.getElementById("lblValuePicked").value = strVal.substring(0, strVal.length - 1);
        }

        function CloseDialogWithValue() {
            var dpt = document.getElementById("lstValues");
            var SelVals = "";
            SelVals = dpt.options[dpt.selectedIndex].text;
            return SelVals;
        }
    </script>
    <style type="text/css">
        table {
            border-collapse: collapse;
            border-spacing: 0;
        }

        * {
            margin: 0px;
            padding: 0px;
        }

        table td {
            vertical-align: top;
        }
    </style>
</head>
<body style="height: 456px; width: 501px">
    <form id="form1" runat="server">
        <div>
            <center>
                <asp:Panel ID="Panel1" runat="server" Width="100%" Height="50px" BorderStyle="Solid"
                    BorderWidth="1px" BackColor="LightBlue" HorizontalAlign="Center">
                    <asp:Label ID="Label1" runat="server" Text="All Values" CssClass="LabelRight"></asp:Label><br />
                    <asp:DropDownList ID="dpAllValues" runat="server" Width="500px" OnSelectedIndexChanged="dpAllValues_SelectedIndexChanged" CssClass="dllCss"
                        AutoPostBack="True">
                    </asp:DropDownList>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" Width="100%" Height="50px" BorderStyle="Solid"
                    BorderWidth="1px" BackColor="LightBlue" HorizontalAlign="Center">
                    <asp:Label ID="Label2" runat="server" Text="Favorite List - By Site of Primary" CssClass="LabelRight"></asp:Label>
                    <br />
                    <asp:DropDownList ID="dpByDiag" runat="server" Width="500px" OnSelectedIndexChanged="dpByDiag_SelectedIndexChanged" CssClass="dllCss"
                        AutoPostBack="True">
                    </asp:DropDownList>
                </asp:Panel>
                <asp:Panel ID="Panel4" runat="server" Width="100%" Height="50px" BorderStyle="Solid"
                    BorderWidth="1px" BackColor="LightBlue" HorizontalAlign="Center">
                    <asp:Label ID="Label3" runat="server" Text="Favorite List - By Study" CssClass="LabelRight"></asp:Label>
                    <br />
                    <asp:DropDownList ID="dpByStudy" runat="server" Width="500px" OnSelectedIndexChanged="dpByStudy_SelectedIndexChanged" CssClass="dllCss"
                        AutoPostBack="True">
                    </asp:DropDownList>
                </asp:Panel>
            </center>
            <br />
            <div style="width: 410px;">
                <br />
                <div style="border-style: solid; border-color: inherit; border-width: thin; overflow: auto; width: 480px; height: 250px">
                    <asp:ListBox ID="lstValues" runat="server" Height="250px" Width="480px" CssClass="chkBoxList" onclick="display(this);"></asp:ListBox>
                </div>
                <br />
            </div>


            <asp:Label ID="Label5" runat="server" CssClass="LabelRight">Value Picked Now:</asp:Label><br />

            <%-- <asp:TextBox ID="lblValuePicked" runat="server" TextMode="MultiLine" Height="40px" ReadOnly="true" Width="500px" Font-Bold="True" CssClass="dynamictext" align="left"></asp:TextBox>
            --%>
            <asp:Label ID="lblValuePicked" runat="server" Width="500px" Font-Bold="True" CssClass="lbltext" align="left"></asp:Label>


        </div>
        <asp:Panel ID="Panel2" runat="server" Width="100%" Height="35px"
            HorizontalAlign="Center">
            <center>
                <asp:Button ID="btnOk" Text="  Ok " runat="server" CssClass="button" Width="100px"
                    OnClientClick="CloseDialogWithValue();" />
                <asp:Button ID="btnClose" Text="  Close " runat="server" CssClass="button" Width="100px"
                    OnClientClick="CloseDialog();" />


            </center>
        </asp:Panel>
    </form>
</body>
</html>
