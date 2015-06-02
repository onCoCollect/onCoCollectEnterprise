<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="True" CodeBehind="Regimen.aspx.cs" Inherits="ePxCollectWeb.Regimen" %>

<%--<%@ Register assembly="DatePickerControl" namespace="DatePickerControl" tagprefix="cc1" %>--%>
<%@ Register Assembly="SlimeeLibrary" Namespace="SlimeeLibrary" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>


<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function ValidateRegimen(lstsiteofprimary, chkTest, lblerror) {
            var message = "";
            var boolvallst = false;
            var SOP = "";
            var options = document.getElementById(lstsiteofprimary).options;
            for (var i = 0; i < options.length; i++) {
                if (options[i].selected == true) {
                    SOP = options[i].value;
                    boolvallst = true;

                }
            }

            if (boolvallst == false) {
                message = message + "\n" + "Please select Site Of Primary.";
            }
            if (SOP == " -") {
                message = message + "\n" + "Please select valid data from Site Of Primary.";
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
                message = message + "\n" + "Please select atleast one item from List";

            }
            if (message == "") {
                document.getElementById(lblerror).innerText = "";
                if (confirm("Do you wish to save the Drug Group Diagnosis?"))
                    return true;
                else
                    return false;
            }
            else {
                document.getElementById(lblerror).innerText = message;
                return false;
            }

        }

    </script>
    <asp:Panel runat="server" ID="pnlMain">

        <table style="border: none;" width="99%" cellpadding="2" cellspacing="2">
            <tr style="margin: 0px; padding: 0px;">
                <td align="left" width="50%">
                    <asp:ListBox ID="lstsiteofPrimary"  runat="server" CssClass="lst" Width="80%" DataTextField="TestGroup" DataValueField="TestGroup" Height="350px"></asp:ListBox>
                </td>
                <td align="left" valign="top" width="50%">

                    <asp:Panel ID="Panel3" runat="server" BorderStyle="Solid" BorderWidth="1px" Height="250px" Style="overflow: scroll;" Width="80%">
                        <asp:CheckBoxList ID="lstGroups" runat="server" RepeatColumns="1" CssClass="checkcss" Width="80%" DataTextField="TestGroup" DataValueField="TestGroup"  >
                        </asp:CheckBoxList>
                    </asp:Panel>

                </td>

            </tr>

            <tr>

                <td style="margin: 0px; padding: 0px;"></td>
                <td style="margin: 0px; padding: 5px; margin-left: 206px; margin-top: 5px;">
                    <asp:Label ID="lblError" runat="server" Text="" Width="400px" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr style="margin: 0px; padding: 0px;">
                <td style="margin: 0px; padding: 0px;"></td>
                <td style="margin: 0px; padding: 0px; margin-left: 206px; margin-top: 5px;">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Height="25px" Width="150px"
                        CssClass="button" />

                    <asp:Button ID="btnReset" runat="server" Text="Reset" Height="25px" Width="70px"
                        CssClass="button" OnClick="btnReset_Click" />
                    <asp:Button ID="btnClose" runat="server" Text="Close" Height="25px" Width="70px"
                        CssClass="button" OnClick="btnClose_Click1" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:GridView ID="grdList" runat="server" AllowPaging="false" EmptyDataText="No Records Found." AutoGenerateColumns="false"
                        OnPageIndexChanging="grdTestGroup_PageIndexChanging" DataKeyNames="DiagnosisName" HorizontalAlign="Center" 
                        CellPadding="15" ForeColor="#333333" GridLines="None" Width="90%" OnRowCommand="grdList_RowCommand">
                        <AlternatingRowStyle BackColor="White" CssClass="even" />
                        <Columns>

                            <asp:TemplateField HeaderText="Diagnosis Name" ItemStyle-Width="30%">
                                <HeaderTemplate>
                                    <asp:Label ID="lblDiagnosis" runat="server" Text="Test Group Name"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDiagnosisName" runat="server" Text='<%# Eval("DiagnosisName") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TestListCSV" ItemStyle-Width="50%">
                                <HeaderTemplate>
                                    <asp:Label ID="lblTestNasme" runat="server" Text="Drug Group CSV"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTestListCSV" runat="server" Text='<%# Eval("DrugGroups") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <HeaderTemplate>
                                    <asp:Label ID="lblEdit" runat="server" Text="Edit"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditItem" Text="Edit"></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <HeaderTemplate>
                                    <asp:Label ID="lblDelete" runat="server" Text="Delete"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteItem" Text="Delete" OnClientClick="return confirm('Do you wish to remove all the Drugs Groups for the selected Diagnosis?');"></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                        </Columns>


                        <EmptyDataRowStyle Font-Bold="true" ForeColor="Red" HorizontalAlign="Center" />
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False"
                            ForeColor="White" BackColor="#2461BF" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </asp:Panel>

</asp:Content>
