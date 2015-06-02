<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="True" CodeBehind="TestGroup.aspx.cs" Inherits="ePxCollectWeb.TestGroup" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>


<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function ValidateTestGroup(txtgroupName, chkTest, lblerror) {
            var message = "";
            if (document.getElementById(txtgroupName).value.trim() == "") {
                //message = "Please enter Test Group Name.";
                document.getElementById(lblerror).style.color = "Red";
                message = "Fields marked with asterisk (*) are required.";//Code modified on March 31,2015


            }
            var boolval = false;
            var CHK = document.getElementById(chkTest);
            var checkbox = CHK.getElementsByTagName("input");
            var counter = 0;
            for (var i = 0; i < checkbox.length; i++) {
                if (checkbox[i].checked) {
                    boolval = true;
                }

            }

            if (boolval == false) {
                // message = message + "\n" + "Please select atleast one item from Test";
                document.getElementById(lblerror).style.color = "Red";
                message = "Fields marked with asterisk (*) are required.";//Code modified on March 31,2015
            }
            if (message == "") {
                document.getElementById(lblerror).innerText = "";
                //if (confirm("Do you wish to save the Test Group?"))//Code modified on March 31 2015,Subhashini
                //    return true;
                //else
                //    return false;
            }
            else {
                document.getElementById(lblerror).style.color = "Red";
                document.getElementById(lblerror).innerText = message;
                return false;
            }

        }
    </script>
    <asp:Panel runat="server" ID="pnlMain">

        <table style="border: none;" width="99%" cellpadding="2" cellspacing="2">
            <tr style="margin: 0px; padding: 0px;">
                <td align="right" class="lbltext">
                    <span style="color: red">*</span>Test Group Name&nbsp;
                </td>

                <td style="margin: 0px; padding: 0px;">
                    <asp:TextBox ID="txtGroupName" runat="server" Width="201px" Height="20px" CssClass="dynamictext" onkeypress="return ValidateTextBoxForDataTypeTextAlphaNumericCommaSpaceHypen(event,'','','');" onkeyup="return validateKeyup(this)" MaxLength="25">
                    </asp:TextBox>
                </td>

            </tr>
            <tr>
                <td colspan="2"></td>
            </tr>

            <tr>
                <td align="right" style="margin: 0px; padding: 0px;" valign="top" class="lbltext">
                    <span style="color: red">*</span>Test List&nbsp;
                </td>
                <td style="margin: 0px; padding: 0px;" valign="top">
                    <asp:Panel ID="Panel3" runat="server" BorderStyle="Solid" BorderWidth="1px" Height="250px" Style="overflow: scroll;" Width="93%">
                        <asp:CheckBoxList ID="lstTests" runat="server" RepeatColumns="2" CssClass="checkcss" Width="95%" DataTextField="TestGroup" DataValueField="TestGroup">
                        </asp:CheckBoxList>
                    </asp:Panel>

                </td>

            </tr>

            <tr>

                <%-- <td style="margin: 0px; padding: 0px;"></td>--%>
                <td style="margin: 0px; padding: 5px; margin-left: 206px; margin-top: 5px;" align="center" colspan="2">
                    <asp:Label ID="lblError" runat="server" Text="" Width="400px" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr style="margin: 0px; padding: 0px;" align="center">
                <%--<td style="margin: 0px; padding: 0px;"></td>--%><%--Code modified on March 31-2015,Subhashini--%>
                <td style="margin: 0px; padding: 0px; margin-left: 206px; margin-top: 5px;" colspan="2" align="center">
                    <center>
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Width="70px"
                        CssClass="button" />

                    <asp:Button ID="btnReset" runat="server" Text="Reset" Width="70px"
                        CssClass="button" OnClick="btnReset_Click" />
                    <asp:Button ID="btnClose" runat="server" Text="Close" Width="70px"
                        CssClass="button" OnClick="btnClose_Click1" />
                        </center>
                </td>
            </tr>
            <tr>
                <%--Code modified on April 18,2015-Subhashini--%>
                <td colspan="2" align="center">
                    <asp:GridView ID="grdTestGroup" runat="server" AllowPaging="true" AutoGenerateColumns="false"
                        OnPageIndexChanging="grdTestGroup_PageIndexChanging" PageSize="5"
                        CellPadding="4" ForeColor="#333333" GridLines="None" Width="98%" OnRowCommand="grdTestGroup_RowCommand" DataKeyNames="TestGroup">
                        <AlternatingRowStyle BackColor="White" CssClass="even" />
                        <Columns>

                            <asp:TemplateField HeaderText="Group Name" ItemStyle-Width="200px">
                                <HeaderTemplate>
                                    <asp:Label ID="lblTestName" runat="server" Text="Test Group Name" class="lbltext"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTestGroupName" runat="server" Text='<%# Eval("TestGroup") %>' class="lbltext"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TestListCSV" ItemStyle-Width="300px">
                                <HeaderTemplate>
                                    <asp:Label ID="lblTestNasme" runat="server" Text="Test  List" class="lbltext"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTestListCSV" runat="server" Text='<%# Eval("TestListCSV") %>' class="lbltext"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False" ItemStyle-Width="50px">
                                <HeaderTemplate>
                                    <asp:Label ID="lblEdit" runat="server" Text="Edit" class="lbltext"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="lnkEdit" runat="server" CommandName="EditItem" ImageUrl="~/images/modify.png"></asp:ImageButton>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False" ItemStyle-Width="50px">
                                <HeaderTemplate>
                                    <asp:Label ID="lblDelete" runat="server" Text="Delete" class="lbltext"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="lnkDelete" runat="server" class="lbltext" CommandName="DeleteItem" ImageUrl="~/images/del.png" OnClientClick="return confirm('Do You wish to delete the Test Group?');"></asp:ImageButton>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>


                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#CCCCCC" Font-Bold="true" Font-Underline="true" ForeColor="White"
                            HorizontalAlign="Left" />
                        <RowStyle BackColor="#EEEEEE" />
                        <SelectedRowStyle BackColor="#D1DDF1" CssClass="gridSelected" />
                    </asp:GridView>

                </td>
            </tr>
        </table>
    </asp:Panel>

</asp:Content>
