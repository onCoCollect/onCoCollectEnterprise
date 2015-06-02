﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SaveMyQuery.aspx.cs" Inherits="ePxCollectWeb.SaveMyQuery" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Save your Query</title>
    <link rel="stylesheet" type="text/css" href="~/Style/style.css" />
    <link rel="stylesheet" type="text/css" href="~/Style/menu.css" />
    <script type="text/javascript" language="javascript">
        function blockSpecialChar(event) {
            var k;
            document.all ? k = e.keyCode : k = event.which;
            if (k == 32) {
                return false;
            }
            return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57));
        }

        function validate() {
            //var val = document.getElementById("<%=txtQueryName.ClientID %>").value
        var re = new Regex("^[a-zA-Z0-9 ]+$");
        if (re.test(document.getElementById("<%=txtQueryName.ClientID %>").value)) {
        return true;
        alert('Exact Match');
    }
    else {
        alert('Exact Not Match');
        return false;
    }
}
function validateInput() {

    var obj = document.getElementById('ctl00_ControlPlaceHolder1_txtQueryName.ClientID').value;

    var parsed = true;

    var paswd = /^(?=.*[0-9])(?=.*[!@#$%^&*()<>])[a-zA-Z0-9!@#$%^&*()<>]{8,15}$/;

    if (obj.exec(paswd)) {


        //    alert('Input is Valid');

    }

    else {


        //  alert('Input is Invalid');
    }


    function validateSpecials(event) {
        var k;
        var count = 0;
        if ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57)) {
            count = count + 1;
            if (count == 2) {
                alert('Should not allow continuous special characters');
            }
        }
    }
}

function validate() {

    var field = document.getElementById("<%=txtQueryName.ClientID%>");
            var val = field.value.replace(/^[\s'!@#$%^&*()0-9]+|\s+$/, '');

            if (val.length == 0) {
                document.getElementById("<%=txtQueryName.ClientID%>").value = "";
                // alert('Please Enter valid text');
            }

        }

        function CloseDialog() {
           
            document.getElementById('<%= btnYes.ClientID %>').click();
           
        }
    </script>




</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="scriptManager" runat="server" />
            <asp:UpdatePanel runat="server" ID="updConfirm" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlConfirm" runat="server" Height="215px" Width="800" BackColor="white">
                        <div style="width: 100%; background: #006699; height: 25px; margin-left: 0px; color: White"><b>Save your Query</b></div>

                        <table>
                            <tr>
                                <td align="right" style="font-family: Verdana; font-size: 11px;"><span style="color: red;">*</span>Query Name :
                     <%--<asp:Label ID="lbl1" runat="server"
                         Text="Query Name :" CssClass="LabelRight"></asp:Label>onkeypress="return validateSpecials()" --%></td>
                                <td style="margin-left: 22px;">
                                    <asp:TextBox ID="txtQueryName" runat="server" Width="250px" CssClass="dynamictext" MaxLength="50" onkeyup="return validate()"></asp:TextBox></td>
                                <td style="vertical-align: top;">
                                    <%-- <asp:RegularExpressionValidator ID="RegExp1" runat="server" Display="Dynamic" ForeColor="Red" Font-Size="11" Font-Names="Verdhana" ErrorMessage="Please enter a valid Query Name." ControlToValidate="txtQueryName" ValidationExpression="^[ A-Za-z0-9_@./#&+-]*$" />--%>

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ForeColor="Red" Font-Size="11" Font-Names="Verdhana" ControlToValidate="txtQueryName" runat="server" ErrorMessage="Please enter a Query Name."></asp:RequiredFieldValidator></td>

                                </td>
                

                            </tr>
                            <tr>
                                <td align="right" style="font-family: Verdana; font-size: 11px;">
                                    <span style="color: red; text-align: right;">*</span>Description :<%--<asp:Label ID="Label2" runat="server" Text="Description :"onkeypress="return validateSpecials()" OnClientClick="return validate()"
                         CssClass="LabelRight" Width="100px"></asp:Label>--%></td>
                                <td>
                                    <asp:TextBox ID="txtFilterText" runat="server" Width="250px" Height="84px" CssClass="dynamictext" MaxLength="255"
                                        TextMode="MultiLine" ViewStateMode="Enabled"></asp:TextBox></td>
                                <td style="vertical-align: top;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Font-Size="11" Font-Names="Verdhana" ControlToValidate="txtFilterText" runat="server" ErrorMessage="Please enter a Description."></asp:RequiredFieldValidator>
                                </td>
                                <td>
                            </tr>
                            <tr>
                                <td style="margin-left: 110px;">
                                    <center>
                                </td>
                                <div class="divider" />
                                <td>
                                    <asp:Button ID="btnYes" runat="server" Text="OK" Width="100px" Height="25px"
                                        OnClick="btnOk_Click" CssClass="button" style="display:none;" />&nbsp;
                 <%--    <asp:Button D="btnNo" runat="server" Text="Close" Width="100px" OnClientClick="CloseDialog();" Height="25px" 
                        CssClass="button" />--%>
                    </center></td>
                            </tr>
                            <%-- <tr>
                 <td></td>
                 <td style="margin-left: 100px;">
                     <asp:Button  ID="btnYes" runat="server" Text="OK" Width="100px" Height="25px"
                         OnClick="btnOk_Click" CssClass="button" /> 
                        <div class="divider"/>
                     <asp:Button ID="btnNo" runat="server" Text="Cancel" Width="100px" Height="25px"  OnClientClick="CloseDialog();"
                         CssClass="button" />
                 </td>
             </tr>--%>
                        </table>



                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>