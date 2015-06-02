﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/epxWebEmpty.Master" AutoEventWireup="true" CodeBehind="PickPatient.aspx.cs" Inherits="ePxCollectWeb.PickPatient" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>


<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

      

    <div >
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
          <ajax:ModalPopupExtender ID="ModalPopupExtender1"
         TargetControlID="Hidden1"
         PopupControlID="pnlConfirm"
         CancelControlID="btnNo"
         BackgroundCssClass="overlay_back"
         DropShadow="false"
         runat="server">
        </ajax:ModalPopupExtender>
        <div style="padding: 10px 5px 10px 10px; background-color: #eee; margin-bottom: 20px;">
        <br />
        <div >
          <asp:HiddenField ID="Hidden1" runat="server"  />
            <asp:Label ID="Label1" runat="server" Text="Search for:"></asp:Label>   
            <asp:DropDownList ID="cboSearchFor" runat="server" 
                onselectedindexchanged="cboSearchFor_SelectedIndexChanged" 
                AutoPostBack="True">
                <asp:ListItem>Name</asp:ListItem>
                <asp:ListItem>File Number</asp:ListItem>
                <asp:ListItem>Patient ID</asp:ListItem>
                <asp:ListItem>All Patients </asp:ListItem>
            </asp:DropDownList>
      
            &nbsp;&nbsp;
            <asp:Label ID="lblFirstName" runat="server" Text="First Name"></asp:Label>
            <asp:TextBox ID="txtFname" runat="server"></asp:TextBox>
            <asp:Label ID="lblLastName" runat="server" Text="Last Name"></asp:Label>
            <asp:TextBox ID="txtLName" runat="server"></asp:TextBox>
            &nbsp;
            <asp:Label ID="lblOthers" runat="server" Visible="False"></asp:Label>
            <asp:TextBox ID="txtOther" runat="server" Visible="False"></asp:TextBox>
      
         </div>
    
      </div>
        </ContentTemplate>
        </asp:UpdatePanel>
   <div  style="padding: 10px 5px 10px 10px; background-color: #eee; margin-bottom: 20px;"  >
        
        <asp:Panel ID="pnlOptions" runat="server" Visible="False">
            <asp:RadioButtonList ID="OptSearch" runat="server" RepeatColumns="2" 
                RepeatLayout="Flow" Width="489px">
                <asp:ListItem Value="1">Find in current Search Results</asp:ListItem>
                <asp:ListItem Selected="True" Value="2">Fresh Search</asp:ListItem>
            </asp:RadioButtonList>
        </asp:Panel><div style="float:right"> <asp:Button ID="btnPickPatient" runat="server" CssClass="button" 
             Text="Register" onclick="btnPickPatient_Click" /></div>  
        
        <asp:Button ID="btnSearch" runat="server" Text="Search" 
            onclick="btnSearch_Click" CssClass="button" />
        &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" 
            onclick="btnCancel_Click" CssClass="button" />
            
       
    <div >
     </div>
     
     
        <asp:GridView ID="grdResult" runat="server" AllowPaging="True" 
            AutoGenerateSelectButton="True" 
            onselectedindexchanged="grdResult_SelectedIndexChanged" PageSize="20" 
            onpageindexchanging="grdResult_PageIndexChanging" CellPadding="4" 
            BackColor="White" BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" Font-Names="verdana" Font-Size="11px">
            <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
            <PagerStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" 
                BackColor="#FFFFCC" ForeColor="#330099" />
            <RowStyle BackColor="White" ForeColor="#330099" />
            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
            <SortedAscendingCellStyle BackColor="#FEFCEB" />
            <SortedAscendingHeaderStyle BackColor="#AF0101" />
            <SortedDescendingCellStyle BackColor="#F6F0C0" />
            <SortedDescendingHeaderStyle BackColor="#7E0000" />
        </asp:GridView>
    </div>  
     </div>
    <asp:Panel ID="pnlConfirm" runat="server" Height="180px" Width="300px" BackColor="white" BorderColor="Black" style="border: 1px solid;">
        <div style="width:100%; background:#fbbb23; height:25px; color:White"><b>Confirm Selection</b></div>
           
                <table align="center"  style="margin-top:20px; color:Black;" >
                <tr><td colspan="2">
                <asp:Label ID="lblConfirmText" runat="server" Text="You have selected :"></asp:Label>
                </td></tr>
                <tr><td colspan="2">&nbsp; <asp:Label ID="lbl1" runat="server" Text="PatientID :"></asp:Label>
                <asp:Label ID="lblPatientID" runat="server" Text=""></asp:Label>
                </td></tr>
                <tr><td colspan="2">&nbsp; <asp:Label ID="Label2" runat="server" Text="PatientName :"></asp:Label>
                <asp:Label ID="lblPatientName" runat="server" Text=""></asp:Label>
                </td></tr>
                <tr><td colspan="2">&nbsp; <asp:Label ID="Label3" runat="server" Text="File No :"></asp:Label>
                <asp:Label ID="lblFileNo" runat="server" Text=""></asp:Label>                   
                </td></tr>
                <tr><td colspan="2">&nbsp; <asp:Label ID="Label4" runat="server" Text="City :"></asp:Label>
                <asp:Label ID="lblCity" runat="server" Text=""></asp:Label>
                    
                </td></tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                <td><asp:Button ID="btnYes" runat="server" Text="OK"  Width="100px" 
                        OnClick="btnOk_Click" CssClass="button"/></td>
                <td>&nbsp;&nbsp;&nbsp; <asp:Button ID="btnNo" runat="server" Text="Cancel" Width="100px" 
                        CssClass="button" /></td>
                </tr>
               </table>
        
        </asp:Panel>
  
</asp:Content>

