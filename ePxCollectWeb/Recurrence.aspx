<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" CodeBehind="Recurrence.aspx.cs"
    Inherits="ePxCollectWeb.Recurrence" EnableViewState="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="updMain" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="100%" Height="250px"
                HorizontalAlign="Center">
                <br />

                <asp:UpdatePanel ID="updPHControls" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="Panel2">
                        <div id="Div1" runat="server" class="entryArea" >                           
                           <center> <asp:Label ID="lblTitle" runat="server" Font-Size="Medium" Text="Recurrence" ></asp:Label></center><br />
                        </div>
                    </asp:Panel>
                        <asp:Panel ID="pnl" runat="server">
                            <table style="width:40%;position:relative;left:27%" > <%-- Code remodified on March 09-2015,Subhashini--%>
                                <tr style="margin: 0px; padding: 3px;text-align:center">
                                    <td align="right" style="margin: 0px; padding: 3px;">
                                        <asp:Label ID="Label2" Width="150" runat="server" CssClass="lbltext" Text="Patient ID&nbsp;"></asp:Label></td>
                                    <td align="left" style="margin: 0px; padding: 3px;">
                                        <asp:Label ID="txtPatient" runat="server" CssClass="lbltext"></asp:Label>
                                    </td>
                                    <%--  <asp:TextBox ID="txtPatient" runat="server" ReadOnly="True"  CssClass="dynamictext" Enabled="false" ForeColor="Gray"></asp:TextBox></td>--%>
                                </tr>
                                <tr style="margin: 0px; padding: 5px;">
                                    <td align="right" style="margin: 0px; padding: 3px;">
                                        <asp:Label ID="lpbPatient0" runat="server" Text="Date of Diagnosis&nbsp;" CssClass="lbltext"
                                            Width="150px"></asp:Label></td>

                                    <td align="left" style="margin: 0px; padding: 3px;">
                                        <asp:Label ID="txtDiagnosisDate" runat="server" CssClass="lbltext"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="margin: 0px; padding: 3px;">
                                    <td align="right" style="margin: 0px; padding: 3px;">
                                        <asp:Label ID="Label1" runat="server" Text="Date of Death&nbsp;" CssClass="lbltext"
                                            Width="150px"></asp:Label></td>
                                    </td>
                            <td align="left" style="margin: 0px; padding: 3px;">
                                <asp:Label ID="txtDateofDeath" runat="server" CssClass="lbltext"></asp:Label>
                            </td>
                                </tr>
                                <tr style="margin: 0px; padding: 0px;">
                                    <td align="right" style="margin: 0px; padding: 0px;">
                                        <asp:Label ID="lblRec1" runat="server" Text="1st RecurrenceDate&nbsp;" CssClass="NewLabelRight"></asp:Label>
                                    </td>
                                    <td align="left" style="margin: 0px; padding: 0px;">
                                        <asp:TextBox ID="dtRec1" runat="server" ReadOnly="false" onkeypress="return false;" CssClass="dynamictext onCoDatePik"></asp:TextBox>

                                    </td>

                                </tr>
                                <tr style="margin: 0px; padding: 0px;">
                                    <td align="right" style="margin: 0px; padding: 0px;">
                                        <asp:Label ID="lblRec2" runat="server" Text="2nd RecurrenceDate&nbsp;" CssClass="NewLabelRight"
                                            Width="150"></asp:Label></td>
                                    <td align="left" style="margin: 0px; padding: 0px;">

                                        <asp:TextBox ID="dtRec2" runat="server" ReadOnly="false" onkeypress="return false;" CssClass="dynamictext onCoDatePik"></asp:TextBox>


                                    </td>

                                </tr>

                                <tr style="margin: 0px; padding: 0px;">
                                    <td align="right" style="margin: 0px; padding: 0px;">
                                        <asp:Label ID="lblrec3" runat="server" Text="3rd RecurrenceDate&nbsp;" CssClass="NewLabelRight"
                                            Width="150"></asp:Label></td>
                                    <td align="left" style="margin: 0px; padding: 0px;">

                                        <asp:TextBox ID="dtRec3" runat="server" ReadOnly="false" onkeypress="return false;" CssClass="dynamictext onCoDatePik"></asp:TextBox>
                                    </td>

                                </tr>

                                <tr style="margin: 0px; padding: 0px;">
                                    <td align="right" style="margin: 0px; padding: 0px;">
                                        <asp:Label ID="lblrec4" runat="server" Text="4th RecurrenceDate&nbsp;" CssClass="NewLabelRight"
                                            Width="150"></asp:Label></td>
                                    <td align="left" style="margin: 0px; padding: 0px;">
                                        <asp:TextBox ID="dtRec4" runat="server" ReadOnly="false" Width="200" onkeypress="return false;" CssClass="dynamictext onCoDatePik"></asp:TextBox>

                                    </td>

                                </tr>
                              <%--  <tr>
                                    <td colspan="2" align="center">
                                        <br />
                                     
                                    </td>
                                </tr>--%>

                            </table>
                            <br />

                            <div style="text-align:center">
                               
                              <asp:Label ID="lblError" runat="server" ForeColor="Red" Text=""></asp:Label>
                                <br />
                            </div>

                            <br />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Panel ID="Panel3" runat="server" Width="100%" Height="35px"
                    HorizontalAlign="Center">
                    <center>
                       
                        <div  style="margin-top: 5px;width:100%;position:relative">
                             <table >
                                 <tr align="center"><%--Code remodified on March 09-2015,Subhashini--%>
                                     <td><asp:Button ID="btnSave" runat="server" CssClass="button" OnClick="btnSave_Click" Width="70px"
                                Text="Save" />

                                     </td>
                                     <td >
                                         <asp:Button ID="btnReset" runat="server" CssClass="button" OnClick="btnReset_Click" Width="70px"
                                Text="Reset" />

                                     </td>
                                 <td > <asp:Button ID="btnClose" runat="server" CssClass="button" OnClick="btnClose_Click" Width="70px"
                                Text="Close" />

                                 </td>
                                     
                                    </tr>

                             </table>
                          
                            &nbsp;
                           
                        </div>
                    </center>
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
