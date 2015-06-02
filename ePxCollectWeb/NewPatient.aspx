<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewPatient.aspx.cs" Inherits="ePxCollectWeb.NewPatient" %>

<%@ Register Assembly="DatePickerControl" Namespace="DatePickerControl" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link rel="stylesheet" type="text/css" href="~/Style/style.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="width: 750px">
                <tr>
                    <td style="width: 10px">
                        <asp:Label ID="Label1" runat="server" Text="Title" CssClass="labelText"></asp:Label>
                        <asp:DropDownList ID="cboTitle" runat="server" Height="24px">
                            <asp:ListItem>Mr</asp:ListItem>
                            <asp:ListItem Value="Mrs">Mrs</asp:ListItem>
                            <asp:ListItem>Ms</asp:ListItem>
                            <asp:ListItem>Dr</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="First Name" CssClass="labelText"></asp:Label>
                        <asp:TextBox ID="TxtFirstName" runat="server" Width="237px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="Last Name" CssClass="labelText"></asp:Label>
                        <asp:TextBox ID="txtLastName" runat="server" Width="270px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="Middle Name" CssClass="labelText"></asp:Label>
                        <asp:TextBox ID="txtMidName" runat="server" Width="57px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="4" style="width: 700px;">
                        <asp:Label ID="Label5" runat="server" Text="Site of Primary" CssClass="labelText"></asp:Label>
                        <asp:DropDownList ID="cboDiagnosis" runat="server" Width="700px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="Label7" runat="server" CssClass="labelText" Text="File Number"></asp:Label>
                        <br />
                        <asp:TextBox ID="txtFileNo" runat="server"></asp:TextBox></td>
                    <td width="150px">
                        <asp:Label ID="Label6" runat="server" Text="Registration Date" CssClass="labelText"></asp:Label>
                        <cc1:DatePicker ID="dtRegDate" runat="server" Width="100px" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                        <cc1:DatePicker ID="DatePicker1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Label ID="lblError" runat="server" Text=" "></asp:Label>
                    </td>
                </tr>

            </table>
        </div>
    </form>
</body>
</html>
