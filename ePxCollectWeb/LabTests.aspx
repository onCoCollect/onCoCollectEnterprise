<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="LabTests.aspx.cs" Inherits="ePxCollectWeb.LabTests" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="updLab" >
        <ContentTemplate>
            <table width="90%">
                <tr>
                    <td colspan="3">
                        <div id="Div1" runat="server" class="entryArea" >                           
                           <center> <asp:Label ID="lblTitle" runat="server" Font-Size="Medium" Text="LAB Tests" ></asp:Label></center><br />
                        </div>                   
                    </td>

                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblRec1" runat="server" CssClass="LabelRight" Text="Pick a New Date&nbsp;"></asp:Label><br />
                        <asp:TextBox ID="txtLabDate" runat="server" CssClass="dynamictext onCoDatePik" onkeypress="return false;" AutoPostBack="true" Width="200px" OnTextChanged="txtLabDate_TextChanged" ></asp:TextBox>


                    </td>
                    <td>
                        <asp:Label ID="Label1" runat="server" CssClass="LabelRight" Text="Pick existing&nbsp;"></asp:Label><br />
                        <asp:DropDownList ID="cboDates" runat="server" Width="200px" CssClass="dllCss"
                            AutoPostBack="True"
                            OnSelectedIndexChanged="cboDates_SelectedIndexChanged1">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label2" runat="server" CssClass="LabelRight" Text="Test Group&nbsp;"></asp:Label><br />
                        <asp:DropDownList ID="cboTestGroup" runat="server" Width="200px" CssClass="dllCss"
                            AutoPostBack="True"
                            OnSelectedIndexChanged="cboTestGroup_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>

                <tr>
                    <td colspan="3" align="center">
                        <asp:Panel ID="Panel3" runat="server" BorderStyle="Solid" BorderWidth="1px" Height="140px" Style="overflow: scroll;" Width="97%">
                            <asp:CheckBoxList ID="lstTests" runat="server" RepeatColumns="3" CssClass="checkcss" Width="97%"
                                OnSelectedIndexChanged="lstTests_SelectedIndexChanged" AutoPostBack="true">
                            </asp:CheckBoxList>
                        </asp:Panel>

                    </td>
                </tr>
                <tr>

                    <td colspan="3" align="center">
                        <asp:Label ID="lblMsg" Text="" runat="server" ForeColor="Red" Font-Size="12px"></asp:Label>
                    </td>
                    <br />
                </tr>
                <tr style="width: 100%">
                    <td align="center" style="width: 100%" colspan="3">
                        <asp:Button ID="btnAdd" runat="server" Text="     Add Test    " OnClick="btnAdd_Click" CssClass="button" />

                    </td>
                </tr>

                <tr>
                    <td colspan="3">
                        <%-- <asp:GridView ID="grdTests" runat="server">
</asp:GridView>--%>
                        <asp:UpdatePanel runat="server" ID="updGrid" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div style="height: 100px; overflow-y: auto;">
                                    <asp:GridView ID="grdTests" runat="server" AutoGenerateColumns="False" OnRowDataBound="grdTests_RowDataBound"
                                        CellPadding="4" ForeColor="#333333" GridLines="None" Width="97%" Style="text-align: left"
                                        OnRowDeleting="grdTests_RowDeleting" Font-Names="verdana" Font-Size="11px">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Date of Investigation">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDOI" runat="server" Width="100"></asp:Label>
                                                    <%-- <asp:TextBox ID="lblDOI" runat="server" Width="85" MaxLength="10" ReadOnly="true"></asp:TextBox>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Investigation">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInvestigation" runat="server" Width="100"></asp:Label>
                                                    <%--<asp:TextBox ID="lblInvestigation" runat="server" Width="85" MaxLength="10" ReadOnly="true"></asp:TextBox>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Observed Value"
                                                SortExpression="[Observed Value]">
                                                <ItemTemplate>
                                                    <%--<asp:Label ID="lblObsValue" runat="server" Width="100" ></asp:Label>--%>
                                                    <asp:TextBox ID="lblObsValue" runat="server" Width="85" MaxLength="10" TabIndex="1" onkeypress="javascript:return ValidateTextBoxForDataTypeLongINT(event,this.value,this,'');"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Measure" SortExpression="Measure">

                                                <ItemTemplate>
                                                    <%--<asp:TextBox ID="txtMeasure" runat="server" Width="85" MaxLength="10" AutoPostBack="true" TabIndex="1"></asp:TextBox>--%>
                                                    <asp:Label ID="lblMeasure" runat="server" Text='<%# Eval("Measure")%>' Visible="false"></asp:Label>
                                                    <asp:DropDownList ID="ddlMeasure" runat="server" Font-Names="verdana" Font-Size="11px" AutoPostBack="false" DataValueField="id" DataTextField="name">
                                                        <%-- <asp:ListItem Text="Mg/M2" Value="1"></asp:ListItem>
                            <asp:ListItem Text="MMOIS" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Mcg" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Mg/M2" Value="1"></asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:CommandField ShowDeleteButton="True" />
                                        </Columns>
                                        <RowStyle BackColor="#EFF3FB" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <AlternatingRowStyle BackColor="White" />
                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <center>
<br />
             
            </center>
                    </td>
                </tr>

                <tr>

                    <td colspan="3" align="center">
                        <br />
                        <asp:Button ID="btnSave" runat="server" Text="Save Data" OnClick="btnSave_Click" CssClass="button" />

                        <asp:Button ID="btnCancel" runat="server" Text=" Close " CssClass="button"
                            OnClick="btnCancel_Click" />
                    </td>
                </tr>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>


