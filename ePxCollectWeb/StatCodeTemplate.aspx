<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/MasterPage/Site.Master" CodeBehind="StatCodeTemplate.aspx.cs" Inherits="ePxCollectWeb.StatCodeTemplate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        .itemwrap {
            word-break: break-all;
            font-family: Verdana;
            font-size: 11px;
            text-align:justify;
        }

        .hideGridColumn {
            display: none;
        }

        .radioButtonList td {
            vertical-align: top;
            text-indent: 3px;
            display: -ms-inline-flexbox;
            text-orientation: sideways-right;
        }

        .radioButtonList input[type="radio"] {
            float: left;
        }

        .radioButtonList label {
            width: 50px;
            display: block;
        }
       
        .btnAlert
        {
            display:none;
        }
    
    </style>

    <script type="text/javascript">
        function ShowConfirmation() {
            
            if (confirm("Some Group Names are created for this Template. Are you sure want to delete?") == true) {
                //Calling the server side code after confirmation from the user
                document.getElementById("<%=btnAlelrt.ClientID%>").click();
                return true;
            }
            //if (confirm('Are you sure that you want to delete the selected record?')) {
            //    return true;
            //}

            //else {

            //    return false;

            //};

        }
        function blockSpecialChar(event) {
            var field = document.getElementById("<%=txtTemplateName.ClientID%>");
            var val = field.value.replace(/^[\s!@#$%^'&*()0-9]+|\s+$/, '');

            if (val.length == 0) {
                document.getElementById("<%=txtTemplateName.ClientID%>").value = "";
            }
            else
            {
                var k;
                document.all ? k = e.keyCode : k = event.which;

                var charCode = (event.which) ? event.which : event.keyCode;
                if (k == 39)
                    return false;
            }

            }
            function isNumberKey(evt) {

                var k;
                document.all ? k = e.keyCode : k = event.which;

                var charCode = (evt.which) ? evt.which : evt.keyCode;
                if (k == 39)
                    return false;
                else
                    return true;
                //if ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57) || k > 31 && (k < 48 || k > 57))
                //    return false;

               // return true;
            }
    </script>


    <asp:UpdatePanel runat="server">

        <ContentTemplate>

            <asp:Panel runat="server" ID="pnlMain">

                <table width="600px" style="margin-left: 50px;">

                    <tr>
                        <td>
                            <fieldset>
                                <legend>
                                    <h3 style="color: #006699;">Manage Template</h3>
                                </legend>
                                <table width="600px">

                                    <tr style="margin: 0px; padding: 0px;">
                                        <td align="right" style="margin: 0px; padding: 0px;"></td>
                                        <td style="display: inline; margin: 0px; padding: 0px;">
                                            <asp:RadioButtonList ID="rbList" CssClass="radioButtonList" RepeatColumns="2" RepeatLayout="Table" TextAlign="Right" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbList_SelectedIndexChanged" Height="16px" Width="300px">
                                                <asp:ListItem Text="Create" Value="1" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Edit" Value="2"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>

                                    </tr>
                                    <tr style="margin: 0px; padding: 0px;">
                                        <td style="width: 150px;" align="right"><span style="color: red; text-align: right;"></span></td>
                                        <td style="margin: 0px; padding: 0px;">

                                            <asp:DropDownList ID="ddlTemplateNames" runat="server" AutoPostBack="True" CssClass="dllCss" OnSelectedIndexChanged="ddlTemplateNames_SelectedIndexChanged">
                                                <asp:ListItem Text="" Value="0">

                                                </asp:ListItem>
                                            </asp:DropDownList>&nbsp<asp:Button ID="btnDelete" runat="server" Height="25px" Text="Delete" OnClick="btnDelete_Click" CssClass="button" />
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr style="margin: 0px; padding: 0px;">
                                        <td style="width: 150px; text-align: right; margin-right: 0px; padding-right: 0px;">
                                            <span style="color: red; text-align: right;">*</span>
                                            <asp:Label ID="lblTemplateName" runat="server" Text="Template Name :" Font-Names="Verdana" Font-Size="8pt"></asp:Label></td>
                                        <td align="left" style="margin: 0px; padding: 0px;">
                                            <asp:TextBox ID="txtTemplateName" AutoPostBack="true" OnTextChanged="txtTemplateName_TextChanged" runat="server"  autocomplete="off" CssClass="dynamictext" onkeypress="return blockSpecialChar(event)" Font-Size="8pt"></asp:TextBox></td>
                                    </tr>
                                    <tr style="margin: 0px; padding: 0px;">
                                        <td style="width: 150px; text-align: right; margin-right: 0px; padding-right: 0px;">
                                            <span style="color: red; text-align: right;">*</span>
                                            <asp:Label ID="lblDesc" runat="server" Text="Description :" Font-Names="Verdana" Font-Size="8pt"></asp:Label></td>
                                        <td style="margin: 0px; padding: 0px;">
                                            <asp:TextBox ID="txtDesc" runat="server" autocomplete="off" TextMode="MultiLine" Height="77px" Width="200px" CssClass="dynamictext" Font-Size="8pt"></asp:TextBox></td>
                                    </tr>
                                    <tr style="margin: 0px; padding: 0px;">
                                        <td align="right" style="width: 150px; margin: 0px; padding: 0px;"></td>
                                        <td style="margin: 0px; padding: 0px;">
                                            <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" CssClass="button" />
                                            <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" CssClass="button" /></td>
                                    </tr>
                                </table>
                            </fieldset>


                        </td>
                        <td valign="top">
                            <div style="margin-top: 10px;">
                               <%-- <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="button" OnClick="btnClose_Click" />--%>
                            </div>
                        </td>
                    </tr>


                    <tr>
                        <td>
                            <div style="width:700px;">
                            <fieldset>
                                <legend>
                                    <h3 style="color: #006699;">Create Stat Code </h3>
                                </legend>
                                <table width="600px" style="margin-left: 10px;">
                                    <tr>
                                        <td align="right" style="width: 150px; margin: 0px; padding: 0px;">
                                            <span style="color: red; text-align: right;">*</span>
                                            <asp:Label ID="lblTemplate" runat="server" Text="Select Template :" Font-Names="Verdana" Font-Size="8pt"></asp:Label>
                                        </td>
                                        <td style="margin: 0px; padding: 0px;">
                                            <asp:DropDownList ID="ddlSelectTemplate" runat="server" AutoPostBack="true" CssClass="dllCss" Font-Size="8pt" OnSelectedIndexChanged="ddlSelectTemplate_SelectedIndexChanged"></asp:DropDownList>
                                        </td>

                                    </tr>
                                    <tr style="margin: 0px; padding: 0px;">
                                        <td align="right" style="width: 150px; margin: 0px; padding: 0px;">
                                            <span style="color: red; text-align: right;">*</span>
                                            <asp:Label ID="lblField" runat="server" Text="Select Field :" Font-Names="Verdana" Font-Size="8pt"></asp:Label></td>
                                        <td style="margin: 0px; padding: 0px;">
                                            <asp:DropDownList ID="ddlField" runat="server" CssClass="dllCss" OnSelectedIndexChanged="ddlField_SelectedIndexChanged" AutoPostBack="True" Font-Size="8pt"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="margin: 0px; padding: 0px;">
                                        <td align="right" style="width: 150px; margin: 0px; padding: 0px;">
                                            <span style="color: red; text-align: right;">*</span>
                                            <asp:Label ID="lblValue" runat="server" Text="Value :" Font-Names="Verdana" Font-Size="8pt"></asp:Label></td>
                                        <td style="margin: 0px; padding: 0px;">

                                            <asp:DropDownCheckBoxes ID="dpValues" runat="server" AutoPostBack="true" UseSelectAllNode="true" CssClass="dllCss" Height="20px"
                                                OnSelectedIndexChanged="dpValues_SelectedIndexChanged"
                                                UseButtons="True">
                                                <Style SelectBoxWidth="200px" DropDownBoxBoxWidth="200px" DropDownBoxBoxHeight="70px" />

                                            </asp:DropDownCheckBoxes>

                                        </td>
                                    </tr>
                                    <tr style="margin: 0px; padding: 0px;">
                                        <td align="right" style="width: 150px; margin: 0px; padding: 0px;">
                                            <span style="color: red; text-align: right;">*</span>
                                            <asp:Label ID="lblGroupName" runat="server" Text="Group Name :" Font-Names="Verdana" Font-Size="8pt"></asp:Label></td>
                                        <td style="margin: 0px; padding: 0px;">
                                            <asp:TextBox ID="txtGroupName" autocomplete="off" runat="server" CssClass="dynamictext" Font-Size="8pt" onkeypress="return isNumberKey(event)" ></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px; margin: 0px; padding: 0px;"></td>
                                        <td style="margin: 0px; padding: 0px;">
                                            <asp:Button ID="btnstatSave" runat="server" Text="Save" OnClick="btnstatSave_Click" CssClass="button" />
                                            <asp:Button ID="btnstatReset" runat="server" Text="Reset" OnClick="btnstatReset_Click" CssClass="button" />&nbsp;</td>
                                    </tr>

                                </table>
                            </fieldset>
                            </div>


                        </td>
                    </tr>







                </table>

                <table width="800px;">
                    <tr style="margin: 0px; padding: 0px;text-align:center">
                        <td align="left" style="width: 600px; margin-left: 62px; margin: 0px; padding: 0px;">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<asp:Label ID="Label1" runat="server" Text="Select Template :" Font-Names="Verdana" Font-Size="8pt"></asp:Label><%--Code altered on Feb-25-2015,Subhashini--%>
                            &nbsp  
                            <asp:DropDownList ID="ddlStatGroupName" runat="server" AutoPostBack="true" CssClass="dllCss" OnSelectedIndexChanged="ddlStatGroupName_SelectedIndexChanged" Font-Size="8pt">
                            </asp:DropDownList>
                         <%--   <asp:Button ID="btnHide" runat="server" Text="Close" CssClass="button" OnClick="btnHide_Click" />--%>
                             <asp:Button ID="Button1" runat="server" Text="Close" CssClass="button" OnClick="btnClose_Click" />
                        </td>
                        <td></td>
                    </tr>
                </table>

                <div style="margin-left: 10px;Width:790px;">
                 <asp:GridView ID="grdStatDetails" runat="server" BorderStyle="Solid"  BorderColor="#006699" BorderWidth="1px" ShowFooter="false" Visible="true" CellPadding="4" AllowPaging="true" ForeColor="#333333" GridLines="None" Width="790px" AutoGenerateColumns="False" PageSize="3" OnRowDeleting="grdStatDetails_RowDeleting" OnSelectedIndexChanging="grdStatDetails_SelectedIndexChanging" OnPageIndexChanging="grdStatDetails_PageIndexChanging">
                     <AlternatingRowStyle BackColor="White" />

                     <EditRowStyle BackColor="#2461BF" />
                     <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"  />
                     <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" Height="30px" Width="100px" />
                     <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                     <RowStyle BackColor="#EFF3FB" />
                     <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="false" ForeColor="#333333" />
                     <SortedAscendingCellStyle BackColor="#F5F7FB" />
                     <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                     <SortedDescendingCellStyle BackColor="#E9EBEF" />
                     <SortedDescendingHeaderStyle BackColor="#4870BE" />
                     <Columns>

                         <asp:BoundField HeaderText="STAT-ID" DataField="stat_ID" ItemStyle-CssClass="hideGridColumn" HeaderStyle-CssClass="hideGridColumn" HeaderStyle-HorizontalAlign="Left" />
                       <%--  <asp:BoundField HeaderText="STAT-Template Name" DataField="stat_TemplateName" ItemStyle-CssClass="hideGridColumn" HeaderStyle-CssClass="hideGridColumn" HeaderStyle-HorizontalAlign="Left" />--%>
                         <asp:BoundField HeaderText="STAT-Template Name" DataField="stat_TemplateName" Visible="true" ItemStyle-CssClass="hideGridColumn" HeaderStyle-CssClass="hideGridColumn" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                         <asp:BoundField HeaderText="Field Name" DataField="stat_FieldName" HeaderStyle-Width="100px" Visible="true" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                             <HeaderStyle HorizontalAlign="Left" />
                             <ItemStyle HorizontalAlign="Left" />
                         </asp:BoundField>
                         <asp:BoundField HeaderText="Field Value" DataField="stat_Value" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="500px" ItemStyle-CssClass="itemwrap">
                             <HeaderStyle HorizontalAlign="Left" />
                             <ItemStyle HorizontalAlign="Left" />
                         </asp:BoundField>
                         <asp:BoundField HeaderText="Group Name" DataField="stat_GroupName" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                             <HeaderStyle HorizontalAlign="Left" Wrap="true" Width="100px" />
                             <ItemStyle HorizontalAlign="Left" Wrap="true" Width="100px" />
                         </asp:BoundField>
                         <asp:BoundField HeaderText="MANG-TemplateID" DataField="mang_TemplateID" Visible="false" />

                         <asp:CommandField ShowSelectButton="True" SelectImageUrl="~/images/modify.png" ButtonType="Image" HeaderText="Edit" HeaderStyle-HorizontalAlign="Left" />
                         <asp:CommandField ShowDeleteButton="True" DeleteImageUrl="~/images/del.png" ButtonType="Image" HeaderText="Delete" HeaderStyle-HorizontalAlign="Left" />
                     </Columns>
                 </asp:GridView>
             </div>

            </asp:Panel>


            <asp:HiddenField ID="Hidden1" runat="server" />
            <asp:Button ID="btnAlelrt" runat="server" Text="GetDetails" OnClick="btnAlelrt_Click" CssClass="btnAlert" />
        </ContentTemplate>

    </asp:UpdatePanel>


</asp:Content>



