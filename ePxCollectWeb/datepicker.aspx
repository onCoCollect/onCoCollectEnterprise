<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="datepicker.aspx.cs" Inherits="OncoCollectEnterprise.datepicker" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-1.8.2.min.js"></script>
    <link href="Scripts/jquery-ui-1.11.2.custom/jquery-ui.min.css" rel="stylesheet" />
    <script src="Scripts/jquery-ui-1.11.2.custom/jquery-ui.min.js"></script>

    <script type="text/javascript">
        $(function () {
            $(".mydate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: "MM d, yy",
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="TextBox1" CssClass="mydate" runat="server" ReadOnly="true"></asp:TextBox>
        <asp:TextBox ID="TextBox2" CssClass="mydate" runat="server" ReadOnly="false"></asp:TextBox>
        <asp:TextBox ID="TextBox3" CssClass="mydate" runat="server" ReadOnly="false"></asp:TextBox>
        <asp:TextBox ID="TextBox4" CssClass="mydate" runat="server" ReadOnly="false"></asp:TextBox>
        <asp:TextBox ID="TextBox5" CssClass="mydate" runat="server" ReadOnly="false"></asp:TextBox>

        <asp:Button ID="Button1" runat="server" Text="Button" />
    </div>
    </form>
</body>
</html>
