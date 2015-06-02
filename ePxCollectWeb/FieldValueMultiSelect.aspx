﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FieldValueMultiSelect.aspx.cs" Inherits="ePxCollectWeb.FieldValueMultiSelect" EnableSessionState="True" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Page-Exit" content="Alpha(opacity=100)" />
    <link rel="stylesheet" type="text/css" href="~/Style/style.css" />
    <title></title>

    <base target="_self" />

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

        .button {
            -moz-box-shadow: inset 0px 1px 0px 0px #ffffff;
            -webkit-box-shadow: inset 0px 1px 0px 0px #ffffff;
            box-shadow: inset 0px 1px 0px 0px #ffffff;
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffffff', endColorstr='#f6f6f6',GradientType=0);
            background-color: #ffffff;
            -moz-border-radius: 6px;
            -webkit-border-radius: 6px;
            border-radius: 3px;
            border: 1px solid #dcdcdc;
            display: inline-block;
            cursor: pointer;
            color: #666666;
            font-family: Verdana;
            font-size: 11px;
            font-weight: bold;
            padding: 5px;
            text-decoration: none;
            text-shadow: 0px 1px 0px #ffffff;
            width: 100px;
        }

        .dynamictext {
            /*height:20px;
    width:197px;
    font-family:Verdana;
    font-size:11px;
   margin-top:3px;
   background-color:white;
   position:relative;
   margin-left:2px;
   background-color:#FAFAFA;*/
            width: 200px;
            height: 18px;
            border-radius: 0px;
            border: 1px solid #CCC;
            padding: 1px;
            margin-top: 4px;
            color: black;
            font-size: 11px;
            font-family: Verdana;
            box-shadow: 1px 1px 3px #CCC;
        }
    </style>

    <script type="text/javascript">
        function CloseDialog() {
            window.close();
        }
        function CloseDialogWithValue() {
            //            var dpt = document.getElementById("lstValues");
            //            var SelVals = "";
            //            //alert(dpt.options[dpt.selectedIndex].value);
            //            SelVals = "";
            //            //alert(dpt.rows.length);
            //            for (var i = 0; i <= dpt.rows.length - 1; i++) {

            //                if (dpt.rows[i].innerHTML.indexOf("CHECKED") != -1) {
            //                    SelVals += dpt.rows[i].innerText + ","; //dpt.options[dpt.selectedIndex].value;
            //                    //alert(SelVals);
            //                }
            //            }
            //            SelVals = SelVals.substring(0, SelVals.length - 1)
            var SelVals = document.getElementById("lblValuePicked").innerText.toString();

            if (SelVals == '') {
                alert('Please pick a value.');
            }
            else {
                if (window.opener) {
                    window.opener.returnValue = SelVals;
                }

                return SelVals;
                //alert(SelVals);
                //window.returnValue = SelVals;
                //window.close();
            }


        }
    </script>



</head>
<body style="height: 456px; width: 508px">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptManager" runat="server" />

        <script type="text/javascript">
            var xPos, yPos;
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            function BeginRequestHandler(sender, args) {
                if ($get('<%=divCheckBox.ClientID%>') != null) {
                    // Get X and Y positions of scrollbar before the partial postback
                    xPos = $get('<%=divCheckBox.ClientID%>').scrollLeft;
                    yPos = $get('<%=divCheckBox.ClientID%>').scrollTop;
                }
            }

            function EndRequestHandler(sender, args) {
                if ($get('<%=divCheckBox.ClientID%>') != null) {
                    // Set X and Y positions back to the scrollbar
                    // after partial postback
                    $get('<%=divCheckBox.ClientID%>').scrollLeft = xPos;
                    $get('<%=divCheckBox.ClientID%>').scrollTop = yPos;
                }
            }

            prm.add_beginRequest(BeginRequestHandler);
            prm.add_endRequest(EndRequestHandler);
        </script>

        <script type="text/javascript">
            function CheckBoxListSelect() {

                var CHK = document.getElementById(<%= lstValues.ClientID%>);
                var checkbox = CHK.getElementsByTagName("input");
                var label = CHK.getElementsByTagName("label");
                var counter = 0;
                for (var i = 0; i < checkbox.length; i++) {
                    if (checkbox[i].checked) {
                        var PickedValue = document.getElementById(<%= lstValues.ClientID%>);;
                        document.getElementById(<%= lstValues.ClientID%>).value = PickedValue + label[i].innerHTML;
                    }
                }
            }


        </script>

        <div>

            <center>
                <asp:Panel ID="Panel1" runat="server" Width="100%" Height="50px" BorderStyle="Solid" BorderWidth="1px" BackColor="LightBlue"
                    HorizontalAlign="Center">
                    <asp:Label ID="Label1" runat="server" Text="All Values" CssClass="LabelRight"></asp:Label><br />
                    <asp:DropDownList ID="dpAllValues" runat="server" Width="350px"
                        OnSelectedIndexChanged="dpAllValues_SelectedIndexChanged" AutoPostBack="True" CssClass="dllCss">
                    </asp:DropDownList>
                </asp:Panel>

                <asp:Panel ID="Panel3" runat="server" Width="100%" Height="50px" BorderStyle="Solid" BorderWidth="1px" BackColor="LightBlue"
                    HorizontalAlign="Center">
                    <asp:Label ID="Label2" runat="server" Text="Favorite List - By Site of Primary" CssClass="LabelRight"></asp:Label>
                    <br />
                    <asp:DropDownList ID="dpByDiag" runat="server" Width="350px" CssClass="dllCss"
                        OnSelectedIndexChanged="dpByDiag_SelectedIndexChanged" AutoPostBack="True">
                    </asp:DropDownList>
                </asp:Panel>
                <asp:Panel ID="Panel4" runat="server" Width="100%" Height="50px" BorderStyle="Solid" BorderWidth="1px" BackColor="LightBlue"
                    HorizontalAlign="Center">
                    <asp:Label ID="Label3" runat="server" Text="Favorite List - By Study" CssClass="LabelRight"></asp:Label>
                    <br />
                    <asp:DropDownList ID="dpByStudy" runat="server" Width="350px" CssClass="dllCss"
                        OnSelectedIndexChanged="dpByStudy_SelectedIndexChanged" AutoPostBack="True">
                    </asp:DropDownList>
                </asp:Panel>
            </center>
            <br />
            <asp:Label ID="Label5" runat="server" CssClass="LabelRight">Value Picked Earlier:</asp:Label><br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <asp:TextBox ID="lblOrigVal" runat="server" TextMode="MultiLine" Height="40px" ReadOnly="true" Width="500px" Font-Bold="True" CssClass="dynamictext" align="left"></asp:TextBox>
                    <%--                    <asp:Label ID="lblOrigVal" runat="server" Width="500px" Font-Bold="True" CssClass="lbltext" align="left"></asp:Label>--%>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div style="width: 410px;">

                <br />
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="divCheckBox" runat="server" style="border-style: solid; border-color: inherit; border-width: thin; overflow: auto; width: 480px; height: 250px">
                            <asp:CheckBoxList ID="lstValues" runat="server" CssClass="chkBoxList" Font-Names="Verdana" Font-Size="11px"
                                AutoPostBack="true" OnSelectedIndexChanged="lstValues_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lstValues" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>



            </div>

            <asp:Label ID="Label4" runat="server" CssClass="LabelRight">Value Picked Now:</asp:Label><br />
            <asp:UpdatePanel ID="updValues" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:TextBox ID="lblValuePicked" runat="server" TextMode="MultiLine" Height="40px" ReadOnly="true" Width="500px" Font-Bold="True" CssClass="dynamictext" align="left"></asp:TextBox>
                    <%--<asp:Label ID="lblValuePicked" runat="server" Width="500px" Font-Bold="True" CssClass="lbltext" align="left"></asp:Label>--%>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
        <br />
        <asp:Panel ID="Panel2" runat="server" Width="100%" Height="35px"
            HorizontalAlign="Center">
            <center>
                <div>
                    <asp:Button ID="btnOk" Text="  Ok " runat="server" CssClass="button" Width="100px" Style="display: none;"
                        OnClientClick="CloseDialogWithValue();" />
                    <asp:Button ID="btnClose" Text="  Close " runat="server" CssClass="button" Width="100px" Style="display: none;"
                        OnClientClick="CloseDialog();" />
                    &nbsp; &nbsp;
                </div>
            </center>
        </asp:Panel>
    </form>
</body>
</html>
