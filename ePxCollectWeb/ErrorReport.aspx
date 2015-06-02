<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="True" CodeBehind="ErrorReport.aspx.cs" Inherits="ePxCollectWeb.ErrorReport" %>

<%@ Register Assembly="SlimeeLibrary" Namespace="SlimeeLibrary" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function ismaxlength(objTxtCtrl, nLength) {
            if (objTxtCtrl.getAttribute && objTxtCtrl.value.length > nLength)
                objTxtCtrl.value = objTxtCtrl.value.substring(0, nLength)

        }
        function ClearFileUploadControl() {
            var uplctrl = document.getElementById("<%=flImage.ClientID%>");
            if (uplctrl != null) {
                uplctrl.select();
                clrctrl = uplctrl.createTextRange();
                clrctrl.execCommand('delete');
                uplctrl.focus();
            }
        }
        function checkErrors() {
            var obj = document.getElementById("<%=flImage.ClientID%>");
            if (obj != null) {
                if (!checkEmpty("<%=flImage.ClientID%>", 'Fields marked with asterisk (*) are required.')) {
                    document.getElementById("<%=flImage.ClientID%>").value = '';
                    return false;
                }
            }
            if (!checkEmpty("<%=txtURL.ClientID%>", 'Fields marked with asterisk (*) are required.')) {
                document.getElementById("<%=txtURL.ClientID%>").value = '';
                return false;
            }

            if (!ValidateURL("<%=txtURL.ClientID%>")) {  // check to make sure all URL characters are valid
                document.getElementById("<%=lblError.ClientID%>").style.color = 'red';
                document.getElementById("<%=lblError.ClientID%>").innerHTML = 'Enter valid URL.';
                document.getElementById("<%=txtURL.ClientID%>").value = '';
                document.getElementById("<%=txtURL.ClientID%>").focus();
                return false;
            }

            if (!checkEmpty("<%=txtDescription.ClientID%>", 'Fields marked with asterisk (*) are required.')) {
                document.getElementById("<%=txtDescription.ClientID%>").value = '';
                return false;
            }
            if (!checkEmpty("<%=txtReportDate.ClientID%>", 'Fields marked with asterisk (*) are required.')) {
                document.getElementById("<%=txtReportDate.ClientID%>").value = '';
                return false;
            }
           <%-- if (document.getElementById("<%=btnSave.ClientID%>").value == 'Save') {--%>
            var pickedDate = document.getElementById("<%=txtReportDate.ClientID%>").value;
            var months = new Array(12);
            months[0] = "January";
            months[1] = "February";
            months[2] = "March";
            months[3] = "April";
            months[4] = "May";
            months[5] = "June";
            months[6] = "July";
            months[7] = "August";
            months[8] = "September";
            months[9] = "October";
            months[10] = "November";
            months[11] = "December";


            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth(); //January is 0!
            var yyyy = today.getFullYear();
            today = months[mm] + ' ' + dd + ', ' + yyyy;
            if (pickedDate != today) {
                document.getElementById("<%=lblError.ClientID%>").innerHTML = 'Report date should be the current date.';
                return false;
            }
            else {
                return true;
            }
        }

        //}
        function checkEmpty(id, msg) {

            var obj = document.getElementById(id);

            if (obj.value == null || obj.value == '' || obj.value.trim().length == 0) {
                document.getElementById("<%=lblError.ClientID%>").style.color = 'red';//Code modified on March 13-2015,Subhashini
                    document.getElementById("<%=lblError.ClientID%>").innerHTML = msg;
                    obj.focus();
                    return false;
                }
                else
                    return true;
            }
            function ValidURL(id) {
                var parsed = true;
                var message;
                var myRegExp = /^(?:(?:https?|ftp):\/\/)(?:\S+(?::\S*)?@)?(?:(?!10(?:\.\d{1,3}){3})(?!127(?:\.\d{1,3}){3})(?!169\.254(?:\.\d{1,3}){2})(?!192\.168(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z\u00a1-\uffff0-9]+-?)*[a-z\u00a1-\uffff0-9]+)(?:\.(?:[a-z\u00a1-\uffff0-9]+-?)*[a-z\u00a1-\uffff0-9]+)*(?:\.(?:[a-z\u00a1-\uffff]{2,})))(?::\d{2,5})?(?:\/[^\s]*)?$/i;
                var urlToValidate = document.getElementById(id).value;;
                if (!myRegExp.test(urlToValidate)) {
                    parsed = false;
                }
            }
            function ValidateURL(id) {

                var urlToValidate = document.getElementById(id).value;
                if (urlToValidate.startsWith('http://') || urlToValidate.startsWith('https://') || urlToValidate.startsWith('fttp://')) {
                    return true;
                }
                else {
                    return false;
                }
            }
    </script>
    <div align="center" style="width: 800px; height: 473px; overflow-x: hidden; overflow-y: auto">
        <asp:UpdatePanel ID="updatepanel1" runat="server" align="center">
            <ContentTemplate>

                <asp:Panel runat="server" ID="Panel3">
                    <div id="Div1" runat="server" class="entryArea">
                        <center>
                <asp:Label ID="lblTitle" runat="server" Font-Size="Small" Text="Error Report" Font-Bold="true" ForeColor="Navy"></asp:Label></center>
                    </div>
                </asp:Panel>

                <asp:Panel runat="server" ID="pnlMain" HorizontalAlign="Center">
                    <div style="left: 20%; width: 50%; text-align: center; position: relative" align="center">
                        <%--Code remodified on March 10-2015,Subhashini--%>
                        <table style="border: none; border-collapse: collapse; text-align: center; margin-top: 10px" align="center" width="95%">
                            <tr style="height: 20px; margin-top: 5px; margin: 0px; padding: 4px;">
                                <%--Code remodified on March 09-2015,Subhashini--%>
                                <td align="right" style="margin: 0px; padding: 0px;">
                                    <asp:Label ID="Label1" runat="server" Text="Error Attachment&nbsp;&nbsp;" CssClass="LabelRight" Width="120"></asp:Label></td>
                                <%--Code remodified on March 09-2015,Subhashini--%>
                                <td style="margin: 0px; padding: 0px;" align="left">
                                    <asp:FileUpload ID="flImage" runat="server" Width="329px" BackColor="White" /><asp:Button ID="btnDownload" runat="server" Text="Download" OnClick="btnDownload_Click" Visible="false" CssClass="button" /><asp:Label ID="lblred" runat="server" Text="*" Style="color: red; font-family: Verdana;"></asp:Label>
                                    <asp:LinkButton ID="lbtnClear" runat="server" Visible="false" OnClick="lbtnClear_Click">Clear</asp:LinkButton>
                                    <%--<asp:Label ID="lblImageName" runat="server" ForeColor="#FF3300"></asp:Label>--%>
                                </td>

                            </tr>

                            <tr style="height: 20px; margin-top: 5px; margin: 0px; padding: 4px;">
                                <td align="right" style="margin: 0px; padding: 0px;">
                                    <asp:Label ID="Label2" runat="server" Text="URL&nbsp;&nbsp;" CssClass="LabelRight" Width="120"></asp:Label></td>
                                <td style="margin: 0px; padding: 0px;" align="left">
                                    <asp:TextBox ID="txtURL" runat="server" Width="250px" Height="20px" autocomplete="off" CssClass="dynamictext" MaxLength="260">
                                    </asp:TextBox>
                                    <asp:Label ID="Label5" runat="server" Text="*" Style="color: red; font-family: Verdana;"></asp:Label>
                                </td>

                            </tr>
                            <tr style="height: 20px; margin-top: 5px; margin: 0px; padding: 4px;">
                                <td align="right" style="margin: 0px; padding: 0px;">
                                    <asp:Label ID="Label4" runat="server" Text="User Name&nbsp;&nbsp;" CssClass="LabelRight" Width="120"></asp:Label></td>
                                <td style="margin: 0px; padding: 0px;" align="left">
                                    <asp:TextBox ID="txtUserName" runat="server" Width="250px" Height="20px" autocomplete="off" CssClass="dynamictext" MaxLength="50" ReadOnly="true">
                                    </asp:TextBox>
                                    <asp:Label ID="Label9" runat="server" Text="*" Style="color: red; font-family: Verdana;"></asp:Label>
                                </td>

                            </tr>
                            <tr style="height: 20px; margin-top: 5px; margin: 0px; padding: 4px;">
                                <td align="right" style="margin: 0px; padding: 0px;">
                                    <asp:Label ID="Label3" runat="server" Text="Description&nbsp;&nbsp;" CssClass="LabelRight" Width="120"></asp:Label></td>
                                <td style="margin: 0px; padding: 0px;" align="left">
                                    <asp:TextBox ID="txtDescription" runat="server" Width="250px" TextMode="MultiLine" autocomplete="off" CssClass="dynamictext" MaxLength="25"
                                        Height="50px" onkeyup="return ismaxlength(this,250)"></asp:TextBox>
                                    <asp:Label ID="Label8" runat="server" Text="*" Style="color: red; font-family: Verdana;"></asp:Label>
                                </td>
                            </tr>

                            <tr style="height: 20px; margin-top: 5px; margin: 0px; padding: 4px;">
                                <td align="right" style="margin: 0px; padding: 0px;">
                                    <asp:Label ID="Label11" runat="server" CssClass="LabelRight" Width="120" Text="Report Date&nbsp;&nbsp;"></asp:Label></td>
                                <td style="margin: 0px; padding: 0px;" align="left">
                                    <asp:TextBox ID="txtReportDate" runat="server" autocomplete="off" CssClass="dynamictext" ReadOnly="false" Width="250" onkeypress="return false;" Height="22px"></asp:TextBox>
                                    <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtReportDate" PopupPosition="Right" Format="MMMM d, yyyy"></ajax:CalendarExtender>

                                    <asp:Label ID="Label12" runat="server" Text="*" Style="color: red; font-family: Verdana;"></asp:Label>
                                </td>

                            </tr>

                            <tr style="height: 20px; margin-top: 5px; margin: 0px; padding: 4px;">
                                <td align="right" style="margin: 0px; padding: 0px;">
                                    <asp:Label ID="Label7" runat="server" CssClass="LabelRight" Width="120" Text="Status&nbsp;&nbsp;"></asp:Label></td>
                                <td style="margin: 0px; padding: 0px;" align="left">
                                    <asp:TextBox ID="txtStatus" runat="server" autocomplete="off" Width="250px" Text="Open" CssClass="dynamictext" Height="20px" MaxLength="15" ReadOnly="true"></asp:TextBox>

                                    <asp:LinkButton ID="lnkReopen" runat="server" OnClick="lnkReopen_Click" Font-Underline="false" Visible="false">Reopen</asp:LinkButton>

                                </td>

                            </tr>

                            <tr style="height: 20px; margin-top: 5px; margin: 0px; padding: 4px;">
                                <td align="right" style="margin: 0px; padding: 0px;">
                                    <asp:Label ID="Label6" runat="server" Text="Fixed Description&nbsp;" CssClass="LabelRight" Width="120px" Height="20px"></asp:Label></td>
                                <td style="margin: 0px; padding: 0px;" align="left">
                                    <asp:TextBox ID="txtFixedDescription" runat="server" Width="250px" TextMode="MultiLine" autocomplete="off" CssClass="dynamictext" MaxLength="25"
                                        Height="50px" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="height: 20px; margin-top: 5px; margin: 0px; padding: 4px; text-align: center" align="center">

                                <td style="margin: 0px; padding: 0px; text-align: center" colspan="2" align="center">
                                    <br />
                                    <asp:Label ID="lblError" runat="server" Text="" Width="250px" ForeColor="Red"></asp:Label>
                                    <br />
                                </td>
                            </tr>
                            <tr style="height: 20px; margin-top: 5px; margin: 0px; padding: 4px" align="center">

                                <td style="margin: 0px; padding: 0px; text-align: center" colspan="2" align="center">
                                    <br />
                                    <center>
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Height="25px" Width="70px"
                                OnClientClick="return checkErrors();" CssClass="button" />

                            <asp:Button ID="btnReset" runat="server" Text="Reset" Height="25px" Width="70px"
                                CssClass="button" OnClientClick="ClearFileUploadControl()" OnClick="btnReset_Click" />
                            <asp:Button ID="btnClose" runat="server" Text="Close" Height="25px" Width="70px"
                                CssClass="button" OnClick="btnClose_Click" /><br />
                        </center>
                                </td>
                            </tr>
                            <%--<tr>
                    <td colspan="2" align="center"></td>
                </tr>--%>
                        </table>
                    </div>

                </asp:Panel>
                <table cellspacing="2" cellpadding="2" style="z-index: 15; margin-left: 0px; margin-right: 0px; margin-top: 0px;"
                    width="100%" align="center">
                    <tr valign="top">
                        <td align="center">
                            <div id="div2" runat="server" align="center" style="width: 750px; height: 200px; align: center; overflow-x: auto; overflow-y: auto">
                                <asp:GridView ID="grdErrorReport" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    BorderColor="#E7E7FF" BorderStyle="Solid" BorderWidth="1px" EnableModelValidation="True" Width="870px"
                                    Font-Size="12px" GridLines="None" OnPageIndexChanging="grdErrorReport_PageIndexChanging"
                                    Style="margin-left: 0px" PageSize="4">
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Report Id" ItemStyle-Width="100px" Visible="false">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblReportId" runat="server" Text="Report Id"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="LabelReportId" runat="server" Text='<%# Eval("ReportId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="URL" ItemStyle-Width="200px">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblURL" runat="server" Text="URL"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="LabelURL" runat="server" Text='<%# Eval("URL") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Description" ItemStyle-Width="200px">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="LabelDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Report Date" ItemStyle-Width="200px">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblReportDate" runat="server" Text="Report Date"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="LabelReportDate" runat="server" Text='<%# Eval("ReportedDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="120px">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="LabelStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fixed Description" ItemStyle-Width="150px">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblFixedDescription" runat="server" Text="Fixed Description"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="LabelFixedDescription" runat="server" Text='<%# Eval("FixedDescription") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Created Date" Visible="false">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblCreatedDate" runat="server" Text="Created Date"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="LabelCreatedDate" runat="server" Text='<%# Eval("CreatedDate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Created By" Visible="false">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblCreatedBy" runat="server" Text="Created By"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="LabelCreatedBy" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Modified By" Visible="false">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblModifiedBy" runat="server" Text="Modified By"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="LabelModifiedBy" runat="server" Text='<%# Eval("LastModifiedBy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Modified Date" Visible="false">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblModifiedDate" runat="server" Text="Modified Date"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="LabelModifiedDate" runat="server" Text='<%# Eval("ModifiedDate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="25px">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblDel" runat="server" Text="Attachment"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgDownload" runat="server" CommandName="Download"
                                                    CausesValidation="False" CommandArgument='<%# Eval("ReportId") %>'
                                                    ImageUrl="~/Images/Attachment.png" OnClick="imgDownload_Click" Style="transform: rotate(45deg)" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="25px">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblView" runat="server" Text="View"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%-- <asp:LinkButton ID="lnkView" runat="server" CommandName="ViewItem" Text="View"
                                        CommandArgument='<%# Eval("ReportId") %>' OnClick="lnkView_Click"></asp:LinkButton>--%>
                                                <asp:ImageButton ID="imgView" runat="server" CausesValidation="false" CommandName="Select" ToolTip="View"
                                                    CommandArgument='<%# Eval("ReportId") %>' OnClick="imgView_Click" ImageUrl="~/Images/view.png" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="25px">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblView" runat="server" Text="Edit"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgEdit" runat="server" CausesValidation="false" CommandName="Select"
                                                    CommandArgument='<%# Eval("ReportId") %>' OnClick="imgEdit_Click" ToolTip="Edit" ImageUrl="~/Images/modify.png" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
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
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                <br />


            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSave" />
            </Triggers>
        </asp:UpdatePanel>
</asp:Content>
