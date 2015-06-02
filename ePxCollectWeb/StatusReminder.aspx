<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="StatusReminder.aspx.cs" Inherits="ePxCollectWeb.StatusReminder" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function showProgress() {
            $('#dvListLoading').show();
        }

        function HideProgress() {
            $('#dvListLoading').fadeOut(1000);
        }

    </script>
    <style type="text/css">
        #dvListLoading {
            background: url(../images/fancybox_loading.gif) no-repeat center center;
            position: fixed;
            z-index: 1000;
            top: 0%;
            left: 0%;
            margin: 0;
            height: 100%;
            width: 100%;
            background-color: gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <div>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="No of Days" CssClass="lbltext"></asp:Label>
                            <asp:TextBox ID="txtNoOfDays" runat="server" Width="35px" CssClass="dynamictext" MaxLength="5" onkeypress="javascript:return  ValidateTextBoxForDataTypeLongINT(event,'','','');">
                            </asp:TextBox>
                            <asp:Button ID="BtnOK" runat="server" Text="Ok" OnClick="BtnOK_Click" OnClientClick="showProgress();"
                                CssClass="button" Width="70px" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" OnClick="btnclose_Click"
                                CssClass="button" Width="70px" />

                        </td>
                        <td>
                            <asp:LinkButton ID="btnPatientStatus" runat="server" OnClientClick="showProgress();" OnClick="btnPatientStatus_Click" CssClass="lbltext">Show only Patient with no status Dates</asp:LinkButton>
                            &nbsp; &nbsp;
       <asp:LinkButton ID="btnSendToGrid" runat="server" Visible="false" OnClick="btnSendToGrid_Click" CssClass="lbltext">Export to Excel</asp:LinkButton>



                        </td>

                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Label ID="lblError" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>



            </div>
            <div id="dvListLoading" style="display: none; overflow: auto; width: 780px;"></div>


            <div id="div1" style="width: 780px; height: 400px; align: center; overflow-x: auto; overflow-y: auto">

                <asp:GridView ID="grdREsults" OnPageIndexChanging="grdREsults_PageIndexChanging"
                    PageSize="7" runat="server" AllowPaging="True"
                    EnableViewState="false" AutoGenerateColumns="true" BorderStyle="Solid" BorderWidth="1px"
                    CellPadding="4" GridLines="Both" Width="96%" BorderColor="#E7E7FF">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                    </Columns>
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="False" ForeColor="White" />
                    <HeaderStyle BackColor="#c3ebf8" ForeColor="Black" />
                    <PagerStyle BackColor="#c3ebf8" Font-Bold="False" Font-Underline="true" ForeColor="White"
                        HorizontalAlign="Left" />
                    <RowStyle BackColor="#EEEEEE" />
                    <SelectedRowStyle BackColor="#D1DDF1" CssClass="gridSelected" />
                </asp:GridView>

            </div>
            <div style="float: left">
                <asp:Label ID="lblCount" runat="server" Font-Bold="True" ForeColor="#0080FF"></asp:Label>
            </div>
            <div style="float: right">
                <asp:Label ID="lblNote" runat="server" Visible="False"
                    Text="* Pls Note that this Output also includes patients with NO status Date"
                    ForeColor="#0080FF"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
