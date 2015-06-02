<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeseaseScreen.aspx.cs"
    Inherits="ePxCollectWeb.DeseaseScreen" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="~/Style/style.css" />
    <script type="text/javascript" src="<%=Page.ResolveUrl("~/Scripts/jquery-1.8.2.min.js")%>"></script>
    <script type="text/javascript" src="<%=Page.ResolveUrl("~/Scripts/jquery-ui-1.11.2.custom/jquery-ui.min.js")%>"></script>
    <link href="~/Scripts/jquery-ui-1.11.2.custom/jquery-ui.min.css" rel="stylesheet" />
    <base target="_self" />
    <script type="text/javascript">
        function CloseDialog() {
            var dpt = document.getElementById("txtValue");
            if (dpt == "undefined") {
                dpt = "";
            }
            window.close();
        }
        function display() {
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

            var lbl = document.getElementById("lblValue");
            var SelVals = "";
            SelVals = lbl.innerText.toString();
            //alert(SelVals);
            window.returnValue = SelVals;
            window.close();
        }

        function ValidateTextBoxForDataTypeSingleIsNumericWithDot1(evt, value, inpu, lbltextt) {
            var charCode;
            if (evt.keyCode) //For IE
                charCode = evt.keyCode;
            else if (evt.Which)
                charCode = evt.Which; // For FireFox
            else
                charCode = evt.charCode; // Other Browser
            if ((charCode >= 48 && charCode <= 57) || (charCode == 46))
                return true;
            else
                return false;

            var numericvalue = value;
            var valarr = numericvalue.split(".").length;
            if (valarr > 1 && String.fromCharCode(charCode) == ".") {
                if (typeof input.selectionStart == "number") {
                    return input.selectionStart == 0 && input.selectionEnd == input.value.length;
                }
                else if (typeof document.selection != "undefined") {
                    input.focus();
                    return document.selection.createRange().text == input.value;
                }
            }
            else {

                return true;
            }
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
<body>
    <!--style="height: 456px; width: 501px"> -->
    <form id="form1" runat="server">
        <ajax:ToolkitScriptManager ID="scriptManager" runat="server" AsyncPostBackTimeout="3600" CombineScripts="False"></ajax:ToolkitScriptManager>
        <div id="divMain" runat="server" style="width: 100%; height: 99%;">
            <center>
                <div>
                    <%--<asp:UpdatePanel ID="updTop" runat="server" UpdateMode="Conditional" ><ContentTemplate> --%>
                    <asp:Panel ID="pnlTop" runat="server" Width="98%" Height="50px" BorderStyle="Solid"
                        BorderWidth="1px" BackColor="LightBlue">
                        <table width="100%">
                            <tr>
                                <td style="width: 30%">
                                    <asp:Label ID="Label1" runat="server" Text="All Regimen" class="lbltext"></asp:Label>
                                    <asp:Button ID="btnAll" runat="server" Text="..." OnClick="btnAll_Click" CssClass="button"
                                        Width="39px" />
                                </td>
                                <td style="width: 35%">
                                    <asp:Label ID="Label2" runat="server" Text="Favorite List - By Site of Primary" class="lbltext"></asp:Label>
                                    <asp:Button ID="btnDiag" runat="server" Text="..." OnClick="btnDiag_Click" CssClass="button"
                                        Width="38px" />
                                </td>
                                <td style="width: 35%">
                                    <asp:Label ID="Label3" runat="server" Text="Favorite List - By Study" class="lbltext"></asp:Label>
                                    <asp:Button ID="btnStudy" runat="server" Text="..." OnClick="btnStudy_Click" CssClass="button"
                                        Width="44px" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>


                    <asp:Panel ID="PanelMid" runat="server" Width="98%" Height="350px" BorderStyle="Solid"
                        BorderWidth="1px" BackColor="ActiveCaption" HorizontalAlign="Center"
                        ScrollBars="Horizontal">
                        <asp:UpdatePanel runat="server" ID="updGrid" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="grdValues" runat="server" AutoGenerateColumns="False"
                                    DataKeyNames="GroupName,DrugName"
                                    OnRowDataBound="grdValues_RowDataBound" OnRowDeleting="grdValues_RowDeleting">
                                    <Columns>
                                        <asp:TemplateField HeaderText="GroupName" ControlStyle-CssClass="lbltext">

                                            <ItemTemplate>
                                                <asp:Label ID="lblGroupName" runat="server" Text='<%# Eval("GroupName") %>' class="lbltext"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DrugName" ControlStyle-CssClass="lbltext">

                                            <ItemTemplate>
                                                <asp:Label ID="lblDrug" runat="server" Text='<%# Eval("DrugName") %>' class="lbltext"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Max Dose Per m2" ControlStyle-CssClass="lbltext"
                                            SortExpression="[Max Dose Per m2]">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMaxDose" runat="server" CssClass="dynamictext" MaxLength="7" onkeypress="javascript:return ValidateTextBoxForDataTypeSingleIsNumericWithDot1(event,this.value,this,'');"
                                                    Text='<%# Bind("[Max Dose Per m2]") %>' Width="75"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Min Dose Per m2" SortExpression="[Min Dose Per m2]" ControlStyle-CssClass="lbltext">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMinDose" runat="server" MaxLength="7" onkeypress="javascript:return ValidateTextBoxForDataTypeSingleIsNumericWithDot1(event,this.value,this,'');"
                                                    Text='<%# Bind("[Min Dose Per m2]") %>' Width="75" CssClass="dynamictext"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Dose from All Cycles" ControlStyle-CssClass="lbltext">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTotDose" runat="server" MaxLength="7" onkeypress="javascript:return ValidateTextBoxForDataTypeSingleIsNumericWithDot1(event,this.value,this,'');"
                                                    Text='<%# Bind("[Total Dose from All Cycles]") %>' Width="75" CssClass="dynamictext"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Number of Weeks" ControlStyle-CssClass="lbltext">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtNoWk" runat="server" MaxLength="7" onkeypress="javascript:return ValidateTextBoxForDataTypeSingleIsNumericWithDot1(event,this.value,this,'');"
                                                    Text='<%# Bind("[Number of Weeks]") %>' Width="75" CssClass="dynamictext"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="[Dose/M2/Week]" ControlStyle-CssClass="lbltext">
                                            <ItemTemplate>
                                                <asp:TextBox ID="DoseWk" runat="server" MaxLength="7" onkeypress="javascript:return ValidateTextBoxForDataTypeSingleIsNumericWithDot1(event,this.value,this,'');"
                                                    Text='<%# Bind("[Dose/M2/Week]") %>' Width="75"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Units" ControlStyle-CssClass="lbltext">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtUnits" runat="server" Text='<%# Bind("[Units]") %>' Width="50" CssClass="dynamictext" MaxLength="7" onkeypress="javascript:return ValidateTextBoxForDataTypeSingleIsNumericWithDot1(event,this.value,this,'');"></asp:TextBox>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:CommandField ShowDeleteButton="True" />
                                    </Columns>
                                    <HeaderStyle CssClass="lbltext" />
                                    <RowStyle CssClass="lbltext" />
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <%-- <asp:GridView ID="grdValues" runat="server" 
            onselectedindexchanged="grdValues_SelectedIndexChanged" PageSize="1" CellPadding="3" 
            BackColor="#DEBA84" BorderColor="#DEBA84" BorderStyle="None" 
            BorderWidth="1px" CellSpacing="2" AutoGenerateEditButton="True" 
                 onrowediting="grdValues_RowEditing1" onrowupdating="grdValues_RowUpdating" 
                 onselectedindexchanging="grdValues_SelectedIndexChanged">
            <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
            <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
            <PagerStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" 
                ForeColor="#8C4510" />
            <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
            <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#FFF1D4" />
            <SortedAscendingHeaderStyle BackColor="#B95C30" />
            <SortedDescendingCellStyle BackColor="#F1E5CE" />
            <SortedDescendingHeaderStyle BackColor="#93451F" />
        </asp:GridView>--%>
                        <asp:Label ID="lblValue" runat="server" Text="SJ" ForeColor="ActiveCaption"></asp:Label>
                    </asp:Panel>
                    <%--</ContentTemplate></asp:UpdatePanel>--%>
                </div>
                <asp:Panel ID="PanelBot" runat="server" Width="98%" BorderStyle="Solid"
                    BorderWidth="1px" HorizontalAlign="Center">
                    <center>
                        <%--//onclick="CloseDialogWithValue();"--%>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" OnClick="btnSave_Click" />

                        &nbsp; &nbsp;
                    <asp:Button ID="btnClose" runat="server" CssClass="button" Text="Close" OnClientClick="CloseDialog();" Style="display: none;" />

                    </center>
                </asp:Panel>
            </center>
        </div>
        <div id="divPopup" runat="server">
            <%--Code modified on April 2,2015-Subhashini--%>
            <%--  <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ><ContentTemplate> --%>
            <asp:Panel ID="pnlConfirm" runat="server" Height="320px" Width="100%" BackColor="AliceBlue"
                BorderColor="Black" Style="border: 1px solid">
                <div style="width: 100%; background: #0085C3; height: 30px; padding: 5px; color: White; text-align: center; vertical-align: middle" class="lbltext">
                    <b>Pick from the List</b>
                </div>
                <table align="center" style="margin-top: 2px; color: Black;" width="100%">
                    <tr align="center">
                        <td colspan="2" style="width: 100%" align="center">
                            <center>
                                <asp:DropDownList ID="dpAllValues" runat="server" Width="780px" OnSelectedIndexChanged="dpAllValues_SelectedIndexChanged"
                                    AutoPostBack="True" ViewStateMode="Enabled">
                                </asp:DropDownList>
                            </center>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="width: 100%" align="center">
                            <center>
                                <asp:ListBox ID="lstValues" runat="server" Height="170px" Width="780px"
                                    ViewStateMode="Enabled"></asp:ListBox>
                            </center>
                        </td>
                    </tr>
                    <tr>

                        <td colspan="2" style="width: 100%; position: relative; text-align: center"><%--Code modified on April 2,2015-Subhashini--%>
                            <center>
                                <asp:Button ID="btnOk" runat="server" Text="OK" Width="100px" class="button" OnClick="btnOk_Click" />

                                &nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="button"
                                Width="100px" OnClick="btnCancel_Click" />
                            </center>
                        </td>

                    </tr>
                </table>
                <%-- <asp:UpdatePanel ID="updatePanelBtn" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
            </asp:Panel>
            <asp:Panel ID="pnlYESNO" runat="server" Height="250px" Width="475px" BackColor="white" BorderColor="Black" Style=" display:none; border: 1px solid; text-align: center; vertical-align: top">
                <div style="width: 100%; background: #0085C3; height: 25px; color: White; text-align: center" class="lbltext"><b>Update Drug Group</b></div>

                <table align="center" style="margin-top: 10px; color: Black;">
                    <tr>
                        <td colspan="2">
                            <asp:TextBox ID="lblConfirmText" runat="server" CssClass="dynamictext" Text="Would you like to Delete the current Drug Group?" Width="370px" ReadOnly="true" Height="110px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <%-- <tr>
                        <td colspan="2">&nbsp;
                        </td>
                    </tr>--%>
                    <tr>
                        <td colspan="2" align="center">
                            <center>
                                <asp:Button ID="btnYes" runat="server" Text="OK" Width="100px" CssClass="button"
                                    OnClick="btnYes_Click" />
                                &nbsp;&nbsp;
                            <asp:Button ID="btnNo" runat="server" Text="Cancel" Width="100px" CssClass="button"
                                OnClick="btnNo_Click" />
                            </center>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <ajax:ModalPopupExtender ID="ModalPopupExtender1" TargetControlID="Hidden1" PopupControlID="pnlYESNO"
                CancelControlID="btnNo" BackgroundCssClass="overlay_back" DropShadow="false"
                runat="server">
            </ajax:ModalPopupExtender>
            <asp:HiddenField ID="Hidden1" runat="server" />
            <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>

            <%-- </ContentTemplate> </asp:UpdatePanel>--%>
        </div>
        <input type="button" value="click me" onclick="sendValuetoParent_dese('wqewq');" />
        <input type="hidden" runat="server" id="ctrlName" />
    </form>

    <script>
        var cur_ctrlName1 = "";
        function getValFromDialog(ctlname) {
            //cur_ctrlName1 = ctlname;
            //document.getElementById("ctrlName").value = ctlname
            document.getElementById('<%= btnSave.ClientID %>').click();
        }

        function sendValuetoParent_dese(retVal) {
            //alert(document.getElementById("ctrlName").value);
            //window.parent.document.getElementById(cur_ctrlName).value = retVal;
            //alert(window.parent.document.getElementById('ctl00_MainContent_1st_ - Line_ - Drug_ - Group_RTX_Y').value);
            window.parent.$("#onCoRightPickDiag").dialog('close');
        }
    </script>
</body>
</html>
