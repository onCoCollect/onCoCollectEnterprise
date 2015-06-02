﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="SavedQueriesView.aspx.cs" Inherits="ePxCollectWeb.SavedQueriesView" %>

<%--<%@ Register assembly="DatePickerControl" namespace="DatePickerControl" tagprefix="cc1" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="updMain" runat="server" >
            <ContentTemplate>
    <div>
   <script type="text/javascript">

       function CloseDialog() {
           window.close();
       }
       function deleteQueryName() {
           if(document.getElementById("<%=dpQueryNames.ClientID%>").selectedIndex == 0)
          
           {
               alert('Please select a Query Name.');
               
           }
           else if(confirm('Are you sure want to delete Saved Query.'))
           {
              return true;
           }
       else 
       {
            return false;
       }           
       }  
    </script>
        
    <asp:UpdatePanel runat="server" ID="updMain1" UpdateMode="Conditional">
     <ContentTemplate> 
     <%--<ajax:ModalPopupExtender ID="ModalPopupExtender1"
         TargetControlID="btnDelete"
         PopupControlID="pnlConfirm"
         CancelControlID="btnNo"
         BackgroundCssClass="overlay_back"
         DropShadow="false"
         runat="server">
        </ajax:ModalPopupExtender>--%>
     <asp:Panel ID="pnlMain" runat="server" Height="250px" Width="500px" BackColor="white" BorderColor="Black" style="border: 1px solid;">
        <div style="width:100%; background: #006699; height:25px; color:White"><b>Choose your Query</b></div>
           
           <asp:HiddenField ID="Hidden1" runat="server"  />
                <table align="center"  style="margin-top:20px; color:Black;" >
               
                <tr><td style="font-family:Verdana;font-size:11px;"><span style="color:red;">*</span>Query Name&nbsp;<%--<asp:Label ID="lbl1" runat="server"    
                        Text="Query Name&nbsp;" CssClass="LabelRight"></asp:Label>--%></td><td align="left">
                        <asp:DropDownList ID="dpQueryNames" runat="server" Width="260px" 
                            onselectedindexchanged="dpQueryNames_SelectedIndexChanged" 
                            AutoPostBack="True">
                        </asp:DropDownList>&nbsp;
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="button" OnClientClick="return deleteQueryName()"
                            Width="92px" onclick="btnDelete_Click" />
                </td></tr>
                <tr><td align="right" style="font-family:Verdana;font-size:11px;">Description&nbsp; <%--<asp:Label ID="Label2" runat="server" Text="Description&nbsp;" 
                        CssClass="LabelRight"></asp:Label>--%>
                </td><td>
                <asp:TextBox ID="txtFilterText" runat="server" Width="260px" Height="84px" 
                            TextMode="MultiLine" ></asp:TextBox>
                </td></tr>
             
                <tr>
                    <td></td>
                <td style="margin-left:83px;" >
                    <asp:Button ID="btnOk" runat="server" Text="OK"  Width="100px" 
                        OnClick="btnOk_Click" CssClass="button"/>&nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Close" 
                        Width="100px" CssClass="button" onclick="btnNo_Click" /></td>
              
                </tr>
               </table>
        </asp:Panel>
        </ContentTemplate>
     </asp:UpdatePanel>
     <%--<asp:UpdatePanel runat="server" ID="updConfirm" UpdateMode="Conditional">
     <ContentTemplate> 
     <asp:Panel ID="pnlConfirm" runat="server" Height="120px" Width="300px" BackColor="white" BorderColor="Black" style="border: 1px solid;">
        <div style="width:100%; background:#fbbb23; height:25px; color:White"><b>Confirm Selection</b></div>
           
                <table align="center"  style="margin-top:20px; color:Black;" >
                <tr><td colspan="2">
                <asp:Label ID="lblConfirmText" runat="server" Text="Selected Query will be deleted. Are you sure?"></asp:Label>
                </td></tr>
                <tr><td></td></tr>
                <tr>
               
                <td><asp:Button ID="btnYes" runat="server" Text="OK"  Width="100px" height="25px"
                        OnClick="btnYes_Click" CssClass="button"/>&nbsp <asp:Button ID="btnNo" runat="server" Text="Cancel" Width="100px" height="25px"
                        CssClass="button" /></td>
                <td>
                    </td>
                </tr>
               </table>
        </asp:Panel>
        </ContentTemplate>
        </asp:UpdatePanel>--%>

    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>