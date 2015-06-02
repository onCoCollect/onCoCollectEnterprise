<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/epxWebEmpty.Master"
    AutoEventWireup="True" CodeBehind="SearchPatient.aspx.cs" Inherits="ePxCollectWeb.SearchPatient" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        function validate() {

            var field = document.getElementById("<%=txtOther.ClientID%>");
            var val = field.value.replace(/^[\s!@#$%^&*(){}|+_)~`<>,.-]+|\s+$/, '');

            if (val.length == 0) {
                document.getElementById("<%=txtOther.ClientID%>").value = "";
                // alert('Please Enter valid text');
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <ajax:ModalPopupExtender ID="ModalPopupExtender1" TargetControlID="Hidden1" PopupControlID="pnlConfirm"
                    CancelControlID="btnNo" BackgroundCssClass="overlay_back" DropShadow="false"
                    runat="server">
                </ajax:ModalPopupExtender>

                <ajax:ModalPopupExtender ID="ModalPopupExtender2" TargetControlID="HiddenField1" PopupControlID="pnlSelection"
                    BackgroundCssClass="overlay_back" DropShadow="false" CancelControlID="btnCancel"
                    runat="server">
                </ajax:ModalPopupExtender>
                <div style="padding: 10px 5px 10px 5px; background-color: #EEEEEE; vertical-align: middle">
                    <asp:Panel runat="server" ID="pnl" DefaultButton="btnPickPatient">
                        <%--Code modified on May 8,2015-Subhashini --%>
                        <div>
                            <asp:HiddenField ID="Hidden1" runat="server" />
                            <asp:HiddenField ID="HiddenField1" runat="server" />

                            <asp:Label ID="Label1" runat="server" Text="Search for:" CssClass="lbltext"></asp:Label>
                            <asp:DropDownList ID="cboSearchFor" runat="server" OnSelectedIndexChanged="cboSearchFor_SelectedIndexChanged" CssClass="dynamictext" Height="20px"
                                AutoPostBack="True">

                                <asp:ListItem>File Number</asp:ListItem>
                                <asp:ListItem>Patient ID</asp:ListItem>

                                <%--<asp:ListItem>All Patients</asp:ListItem>
                                 <asp:ListItem>Name</asp:ListItem>
                                <asp:ListItem>City_Town</asp:ListItem>
                            <asp:ListItem>State</asp:ListItem>
                            <asp:ListItem>SiteOfPrimary</asp:ListItem>
                                <asp:ListItem>City_Town</asp:ListItem>
                            <asp:ListItem>State</asp:ListItem>
                            <asp:ListItem>SiteOfPrimary</asp:ListItem>
                                --%>
                            </asp:DropDownList>
                            &nbsp;&nbsp;
                   <asp:Label ID="lblFirstName" runat="server" Text="First Name"></asp:Label>
                            <asp:TextBox ID="txtFname" runat="server"></asp:TextBox>
                            <asp:Label ID="lblLastName" runat="server" Text="Last Name"></asp:Label>
                            <asp:TextBox ID="txtLName" runat="server"></asp:TextBox>
                            &nbsp;
                        <asp:Label ID="lblOthers" runat="server" Visible="False"></asp:Label>
                            <asp:TextBox ID="txtOther" runat="server" CssClass="dynamictext" Height="20px" Visible="False" onkeypress="return ValidateTextBoxForDataTypeTextAlphaNumericFileNumber(event,'',this,'')" onkeyup="return validateKeyup(this);"></asp:TextBox>
                            <asp:Button ID="btnPickPatient" runat="server" Text="Search" OnClick="btnSearch_Click" UseSubmitBehavior="false"
                                CssClass="button" />
                            &nbsp;&nbsp;&nbsp;&nbsp; 
                            
                            <%-- <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                            CssClass="button" />--%>
                            <asp:Button ID="btn_newpatient" runat="server" PostBackUrl="~/Register.aspx" CssClass="button" Text="New Patient" />&nbsp;
                            <asp:Label ID="lblErrorMsg" runat="server" Text="" Visible="false" ForeColor="Red"></asp:Label>
                        </div>
                    </asp:Panel>
                    <%-- <div style="float:right"> 
                        <asp:HyperLink ID="HyperLink1" runat="server"  NavigateUrl="Register.aspx">Register New Patient</asp:HyperLink>
                    </div>--%>
                    <div id="logImage" runat="server">
                        <br />
                        <%--     <img alt="" width="99%" height="400px" src="images/doctors.png" onmousedown="return false;">--%>
                    </div>
                    <%--</div>--%>
                    <%-- <br />--%>
                    <div id="divpager" align="right" runat="server">

                        <asp:Label ID="Label5" runat="server" Text="Page Size :"
                            Font-Bold="True"></asp:Label>
                        <asp:DropDownList ID="pager" runat="server" OnSelectedIndexChanged="pager_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>


                    </div>
                </div>
                <asp:GridView ID="grdResult" runat="server" AllowPaging="True" AutoGenerateSelectButton="True"
                    OnSelectedIndexChanged="grdResult_SelectedIndexChanged" OnPageIndexChanging="grdResult_PageIndexChanging"
                    CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%" OnRowCommand="grdResult_RowCommand">
                    <%--Code modified on March 05-2015,Subhashini--%>
                    <AlternatingRowStyle BackColor="White" CssClass="even" />
                    <Columns>
                    </Columns>


                    <%--<EmptyDataRowStyle Font-Bold="false" BackColor="#eeeeee" ForeColor="Red" HorizontalAlign="Center" VerticalAlign="Bottom" />--%>
                    <%--Code modified on March 05-2015,Subhashini--%>
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

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>


    <asp:UpdatePanel runat="server" ID="updConfirm" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlConfirm" runat="server" Height="400px" Width="300px" BackColor="white"
                BorderColor="Black" Style="border: 1px solid; display: none;" DefaultButton="btnYes">
                <%--Code modified on May 8,2015-Subhashini --%>
                <div style="width: 100%; text-align: center; background: #0099C8; height: 35px; color: White; vertical-align: middle; text-align: center">
                    <%--Code modified on March 05-2015,Subhashini--%>
                    <br />
                    <b>Confirm Selection</b>
                </div>
                <table align="center" style="margin-top: 20px; color: Black;">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblConfirmText" runat="server" Text="You have selected :"
                                Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                            <asp:Label ID="lbl1" runat="server" Text="PatientID     :"></asp:Label>
                            <asp:Label ID="lblPatientID" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                            <asp:Label ID="Label2" runat="server" Text="PatientName   :"></asp:Label>
                            <asp:Label ID="lblPatientName" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                            <asp:Label ID="Label3" runat="server" Text="File No       :"></asp:Label>
                            <asp:Label ID="lblFileNo" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                            <asp:Label ID="Label4" runat="server" Text="City          :"></asp:Label>
                            <asp:Label ID="lblCity" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                            <asp:Label ID="Label6" runat="server" Text="Address          :"></asp:Label>
                            <asp:Label ID="lblAddress" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                            <asp:Label ID="Label8" runat="server" Text="Phone Number          :"></asp:Label>
                            <asp:Label ID="lblPhoneNumber" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                            <asp:Label ID="Label10" runat="server" Text="Email ID          :"></asp:Label>
                            <%--Code remodified on March 10-2015,Subhashini--%>
                            <asp:Label ID="lblmailid" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <%--  <tr>
                        <td colspan="2">&nbsp;
                            <asp:Label ID="Label12" runat="server" Text="Consultant          :"></asp:Label>
                            <asp:Label ID="lblConsultants" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>--%>
                    <tr>
                        <td colspan="2">&nbsp;
                            <asp:Label ID="Label14" runat="server" Text="Mobile Number          :"></asp:Label>
                            <asp:Label ID="lblmob" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <%--    <tr>
                        <td colspan="2">&nbsp;
                            <asp:Label ID="Label16" runat="server" Text="Relative Phone Number          :"></asp:Label>
                            <asp:Label ID="lblRPNum" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>--%>
                    <%--<tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>--%>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button ID="btnYes" runat="server" Text="OK" Width="100px" OnClick="btnOk_Click"
                                CssClass="button" />

                            <asp:Button ID="btnNo" runat="server" Text="Cancel" Width="100px" CssClass="button" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Panel ID="pnlSelection" runat="server" Height="150px" Width="350px" BackColor="white"
                BorderColor="Black" Style="border: 1px solid; display: none;">
                <div style="width: 100%; text-align: center; background: #0099C8; height: 35px; color: White; vertical-align: middle">
                    <br />
                    <b>Confirm Selection</b>
                </div>
                <table style="margin-top: 20px; text-align:center; color: Black;">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblMessgae" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button ID="btnOkP" runat="server" Text="OK" Width="100px" OnClick="btnOkP_Click"
                                CssClass="button" />
                            &nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="100px" CssClass="button" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
