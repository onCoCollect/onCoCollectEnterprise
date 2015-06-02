<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="True" CodeBehind="SitesofMet.aspx.cs" Inherits="ePxCollectWeb.SitesofMet" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="updMet">
        <ContentTemplate>
            <table>
                <tr>
                    <td colspan="2">
                        <div id="Div1" runat="server" class="entryArea" >                           
                           <center> <asp:Label ID="lblTitle" runat="server" Font-Size="Medium" Text="Sites of Metastasis" ></asp:Label></center><br />
                        </div>                   
                    </td>

                </tr>
                <tr>
                    <td style="vertical-align: top;">

                        <div style="float: left">
                            <%--   <asp:Label ID="sas" runat="server" Text="Lines of Metastasis :" CssClass="lbltext"></asp:Label>--%>
                            <asp:ListBox ID="LstLines" runat="server" Width="200px" Height="90px"
                                OnSelectedIndexChanged="LstLines_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                        </div>
                    </td>
                    <td style="vertical-align: top;">
                        <div style="overflow: auto; width: 500px; height: 260px;  border:solid; border-width:1px;" >
                            <%--Code remodified on March 10-2015,Subhashini--%>
                            <asp:CheckBoxList ID="LstSitesofMet" runat="server" Enabled="False" CssClass="checkcss" RepeatColumns="2" RepeatDirection="Horizontal" Width="450px">
                            </asp:CheckBoxList>

                        </div>

                    </td>
                </tr>

            </table>
            <div style="text-align: center">
                <%--Code modified on March 06-2015,Subhashini--%>
                <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                <br />
            </div>
            <br />
            <center>
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="70px"
                        CssClass="button"
                onclick="btnSave_Click" />
                <asp:Button ID="btnReset" runat="server" Text="Reset" Width="70px"
                        CssClass="button" OnClick="btnReset_Click" />
                <asp:Button ID="btnClose" runat="server" Text="Close" Width="70px"
                        CssClass="button" onclick="btnClose_Click" 
                 />
                </center>
            <br />

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
