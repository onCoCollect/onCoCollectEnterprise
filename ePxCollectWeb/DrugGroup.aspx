<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="True" CodeBehind="DrugGroup.aspx.cs" Inherits="ePxCollectWeb.DrugGroup" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>


<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        tr th {
            background: none repeat scroll 0 0 #c3ebf8;
            padding: 5px;
            font-weight: normal;
            color: #003660;
            text-align: center;
        }
    </style>
    <script type="text/javascript">
        var xPos, yPos;
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        function BeginRequestHandler(sender, args) {
            if ($get('<%=Panel3.ClientID%>') != null) {
                // Get X and Y positions of scrollbar before the partial postback
                xPos = $get('<%=Panel3.ClientID%>').scrollLeft;
                yPos = $get('<%=Panel3.ClientID%>').scrollTop;
            }
        }

        function EndRequestHandler(sender, args) {
            if ($get('<%=Panel3.ClientID%>') != null) {
                // Set X and Y positions back to the scrollbar
                // after partial postback
                $get('<%=Panel3.ClientID%>').scrollLeft = xPos;
                $get('<%=Panel3.ClientID%>').scrollTop = yPos;
            }
        }

        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);

        function ValidateDrugGroup(chkTest, lblerror) {
            var message = "";

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
                message = "Fields marked with asterisk (*) are required.";//Code modified on March 31,2015
                // message = message + "" + "Please select atleast one item from Test";//Code modified on March 06-2015,Subhashini

            }
            if (message == "") {
                document.getElementById(lblerror).innerText = "";//Code modified on March 31 2015,Subhashini
                //if (confirm("Do you wish to save the Drug Group?"))
                //    return true;
                //else
                //    return false;
            }
            else {
                document.getElementById(lblerror).style.color = "Red";//Code modified on March 31 2015,Subhashini
                document.getElementById(lblerror).innerText = message;
                return false;
            }

        }
    </script>

    <asp:Panel runat="server" ID="pnlMain">

        <table style="border: none;" width="98%" cellpadding="2" cellspacing="2">
            <tr>
                <td align="right" style="margin: 0px; padding: 5px;" valign="top"><%--Code modified on March 10-2015,Subhashini--%>
                    <span style="color: red">*</span>Drug Name&nbsp;
                </td>
                <td style="margin: 0px; padding: 0px;" valign="top">
                    <asp:Panel ID="Panel3" runat="server" BorderStyle="Solid" BorderWidth="1px" Height="250px" Style="overflow: scroll;" Width="657px">
                        <asp:CheckBoxList ID="lstTests" runat="server" RepeatColumns="2" CssClass="checkcss" Width="650px" OnSelectedIndexChanged="lstTests_SelectedIndexChanged" AutoPostBack="true">
                        </asp:CheckBoxList>
                    </asp:Panel>
                </td>

            </tr>

            <tr>
                <td align="right" style="margin: 0px; padding: 5px;" valign="top">Drug List&nbsp; <%--Code modified on March 10-2015,Subhashini--%>
                </td>
                <td style="margin: 0px; padding: 0px;" valign="top" align="left">
                    <asp:TextBox ID="txtDrugList" runat="server" TextMode="MultiLine" Height="40px" ReadOnly="true"
                        Width="657px" Font-Bold="True" CssClass="dynamictext" align="left"></asp:TextBox>

                </td>

            </tr>

            <tr>
                <%--Code modified on March 06-2015,Subhashini--%>
                <%-- <td style="margin: 0px; padding: 0px;"></td>--%>

                <td style="margin: 0px; padding: 5px; margin-top: 5px;" colspan="2" align="center">
                    <br />
                    <asp:Label ID="lblError" runat="server" Text="" Width="400px" ForeColor="Red"></asp:Label><%--Code modified on March 06-2015,Subhashini--%>

                    <br />
                </td>

            </tr>
            <tr style="margin: 0px; padding: 0px;">

                <td style="margin: 0px; padding: 0px;" colspan="2" align="center">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Width="70px"
                        CssClass="button" />

                    <asp:Button ID="btnReset" runat="server" Text="Reset" Width="70px"
                        CssClass="button" OnClick="btnReset_Click" />
                    <asp:Button ID="btnClose" runat="server" Text="Close" Width="70px"
                        CssClass="button" OnClick="btnClose_Click1" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">



                    <asp:GridView ID="grdDrugGroup" runat="server" AllowPaging="True" AutoGenerateColumns="False" 
                        BorderColor="#E7E7FF" BorderStyle="Solid" BorderWidth="1px" EnableModelValidation="True" Width="98%"
                        Font-Size="12px" GridLines="None" OnPageIndexChanging="grdDrugGroup_PageIndexChanging"
                        Style="margin-left: 0px" EnablePersistedSelection="false" 
                        PageSize="5" OnRowCommand="grdDrugGroup_RowCommand" DataKeyNames="GroupName"><%--Code modified on May 8,2015-Subhashini--%>
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="Drug Name" ItemStyle-Width="80%">
                                <HeaderTemplate>
                                    <asp:Label ID="lblHDrugList" runat="server" Text="Drug Groups"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDrugList" runat="server" Text='<%# Eval("GroupName") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False" ItemStyle-Width="10%">
                                <HeaderTemplate>
                                    <asp:Label ID="lblEdit" runat="server" Text="View"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="ViewItem" Text="View"></asp:LinkButton>
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
