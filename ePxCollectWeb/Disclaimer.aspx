<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Disclaimer.aspx.cs" Inherits="ePxCollectWeb.Disclaimer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        function CloseDialog() {
            window.close();
        }


        function CloseDialogWithValue(val) {
            var SelVals = val;
            window.returnValue = SelVals;
            window.close();
        }
    </script>

    <link rel="stylesheet" href="Style/style.css" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <center>

            <div style="background-color: #92A117">
                <asp:Label ID="lbl" runat="server" Text="Disclaimer" Font-Size="Larger" Font-Names="verdana"></asp:Label><br />
                <asp:TextBox ID="TextBox1" runat="server" Height="396px" Width="400px"
                    ReadOnly="true" TextMode="MultiLine" CssClass="dynamictext" Wrap="true"></asp:TextBox>
                <br />
                <asp:Panel ID="Panel2" runat="server" Width="90%" Height="30px" BorderStyle="Solid"
                    BorderWidth="1px" BackColor="LightBlue" HorizontalAlign="Center">
                    <center>
                        <div class="btn" style="margin-top: 5px; margin-right: 5px; float: right;">
                            <input id="btnSave" type="button" value="  I Agree  " class="button" onclick="CloseDialogWithValue('IAgree');" />
                            &nbsp; &nbsp;
                <input id="btnClose" type="button" class="button" value="  I Disagree  " onclick="CloseDialogWithValue('');" />
                        </div>
                    </center>
                </asp:Panel>
                <div style="float: right; display: none">
                    <asp:Button ID="Agree" runat="server" Text="I Agree" OnClick="Agree_Click" CssClass="button" />
                    <asp:Button ID="DisAgree" runat="server" Text="I Disagree" CssClass="button"
                        OnClick="DisAgree_Click" />
                </div>

            </div>
        </center>
    </form>
</body>
</html>
