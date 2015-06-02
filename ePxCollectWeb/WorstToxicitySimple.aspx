<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="WorstToxicitySimple.aspx.cs" Inherits="ePxCollectWeb.WorstToxicitySimple" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="border: thin solid #C0C0C0; float:left; width:55%;" >
<asp:ListBox ID="lstLines1" runat="server" Width="130px" AutoPostBack="True" 
        Height="95px" onselectedindexchanged="lstLines_SelectedIndexChanged" 
        Visible="False" Font-Size="Smaller" SelectionMode="Multiple" ></asp:ListBox> 
    <asp:RadioButtonList ID="lstLines" runat="server" 
        onselectedindexchanged="lstLines_SelectedIndexChanged" AutoPostBack="True" 
        BorderColor="#CCCCCC" BorderStyle="None" Font-Size="Small" 
        RepeatLayout="Flow" RepeatDirection="Horizontal">
    </asp:RadioButtonList> </div> 
    <asp:UpdatePanel runat="server" ID="updMain"> <ContentTemplate>
<div style="width:100%">
<br />
<asp:Panel ID="pnlControls" runat="server" Height="420px" Width="100%" BackColor="AliceBlue"  
        BorderColor="Black" Style="border: 1px solid;" ScrollBars="Both">
        <table> <tr> <td style=" border-color:Blue; border:2px solid;"  align="center">
    <asp:Label ID="Label1" runat="server" Text="NEUTROPENIA"  Height="22px" Width="130px" 
                ForeColor="#CC3300"></asp:Label> <br />
    <asp:ListBox ID="ListBox1" runat="server" Width="130px" AutoPostBack="True"  
                onselectedindexchanged="ListBoxChanged" Height="110px" Font-Size="Small" 
                SelectionMode="Multiple"></asp:ListBox> 
    <asp:TextBox ID="TextBox1" runat="server" Height="40px" Width="125px" CssClass="txtBox" 
                TextMode="MultiLine"></asp:TextBox>
   </td><td style=" border-color:Blue; border:2px solid;"  align="center">
    <asp:Label ID="Label2" runat="server" Text="LEUKOCYTOPENIA"  Height="40px" Width="130px" ForeColor="#CC3300"></asp:Label><br />
    <asp:ListBox ID="ListBox2" runat="server" Width="130px" 
                    onselectedindexchanged="ListBoxChanged" AutoPostBack="True" Height="110px" 
                    Font-Size="Small" SelectionMode="Multiple"></asp:ListBox> 
    <asp:TextBox ID="TextBox2" runat="server" Height="40px" Width="125px" CssClass="txtBox" 
                    TextMode="MultiLine"></asp:TextBox>
   </td><td style=" border-color:Blue; border:2px solid;"  align="center">
    <asp:Label ID="Label3" runat="server" Text="THROMBOCYTOPENIA" Height="40px" Width="130px" ForeColor="#CC3300"></asp:Label><br />
    <asp:ListBox ID="ListBox3" runat="server" Width="130px" 
                    onselectedindexchanged="ListBoxChanged" AutoPostBack="True" Height="110px" 
                    Font-Size="Small" SelectionMode="Multiple"></asp:ListBox><br />
    <asp:TextBox ID="TextBox3" runat="server" Height="40px" Width="125px" CssClass="txtBox" 
                    TextMode="MultiLine"></asp:TextBox>
    </td><td style=" border-color:Blue; border:2px solid;"  align="center">
    <asp:Label ID="Label4" runat="server" Text="ANEMIA" Height="40px" Width="130px" ForeColor="#CC3300" ></asp:Label><br />
    <asp:ListBox ID="ListBox4" runat="server" Width="130px" 
                    onselectedindexchanged="ListBoxChanged" AutoPostBack="True" Height="110px" 
                    Font-Size="Small" SelectionMode="Multiple"></asp:ListBox><br />
     <asp:TextBox ID="TextBox4" runat="server" Height="40px" Width="125px" CssClass="txtBox" 
                    TextMode="MultiLine"></asp:TextBox>
    </td>  
    <%--</tr> <tr>--%>
    <td style=" border-color:Blue; border:2px solid;"  align="center">
    <asp:Label ID="Label5" runat="server" Text="FEBRILE NEUTROPENIA" Height="40px" Width="130px" ForeColor="#CC3300"></asp:Label><br />
    <asp:ListBox ID="ListBox5" runat="server" Width="130px" 
            onselectedindexchanged="ListBoxChanged" AutoPostBack="True" Height="110px" 
            Font-Size="Small" SelectionMode="Multiple"></asp:ListBox><br />
    <asp:TextBox ID="TextBox5" runat="server" Height="40px" Width="125px"  CssClass="txtBox" TextMode="MultiLine"></asp:TextBox>
    </td><td style=" border-color:Blue; border:2px solid;"  align="center">
    <asp:Label ID="Label6" runat="server" Text="NAUSEA AND VOMITING" Height="40px" Width="130px" ForeColor="#CC3300"></asp:Label><br />
    <asp:ListBox ID="ListBox6" runat="server" Width="130px" onselectedindexchanged="ListBoxChanged" AutoPostBack="True" Height="110px" SelectionMode="Multiple"></asp:ListBox><br />
    <asp:TextBox ID="TextBox6" runat="server" Height="40px" Width="125px" CssClass="txtBox" TextMode="MultiLine"></asp:TextBox>
    </td><td style=" border-color:Blue; border:2px solid;"  align="center">
    <asp:Label ID="Label7" runat="server" Text="DIARRHEA" Height="40px" Width="130px" ForeColor="#CC3300"></asp:Label><br />
    <asp:ListBox ID="ListBox7" runat="server" Width="130px" onselectedindexchanged="ListBoxChanged" AutoPostBack="True" Height="110px" SelectionMode="Multiple"></asp:ListBox><br />
    <asp:TextBox ID="TextBox7" runat="server" Height="40px" Width="125px" CssClass="txtBox" TextMode="MultiLine"></asp:TextBox>
    </td><td style=" border-color:Blue; border:2px solid;"  align="center">
    <asp:Label ID="Label8" runat="server" Text="ORAL MUCOSITIS" Height="40px" Width="130px" ForeColor="#CC3300"></asp:Label><br />
    <asp:ListBox ID="ListBox8" runat="server" Width="130px" onselectedindexchanged="ListBoxChanged" AutoPostBack="True" Height="110px" SelectionMode="Multiple"></asp:ListBox><br />
    <asp:TextBox ID="TextBox8" runat="server" Height="40px" Width="125px" CssClass="txtBox" TextMode="MultiLine"></asp:TextBox>
    </td>
    </tr><tr> 
    <td style=" border-color:Blue; border:2px solid;"  align="center">
    <asp:Label ID="Label9" runat="server" Text="SKIN RASH" Height="22px" Width="130px" ForeColor="#CC3300"></asp:Label> <br />
    <asp:ListBox ID="ListBox9" runat="server" Width="130px" 
            onselectedindexchanged="ListBoxChanged" AutoPostBack="True" Height="110px" 
            SelectionMode="Multiple"></asp:ListBox> 
    <asp:TextBox ID="TextBox9" runat="server" Height="40px" Width="125px" CssClass="txtBox" TextMode="MultiLine"></asp:TextBox>
   </td><td style=" border-color:Blue; border:2px solid;"  align="center">
    <asp:Label ID="Label10" runat="server" Text="HAND FOOT SYNDROME" Height="40px" Width="130px" ForeColor="#CC3300"></asp:Label><br />
    <asp:ListBox ID="ListBox10" runat="server" Width="130px" 
                        onselectedindexchanged="ListBoxChanged" AutoPostBack="True" Height="110px" 
                        SelectionMode="Multiple"></asp:ListBox> 
    <asp:TextBox ID="TextBox10" runat="server" Height="40px" Width="125px" CssClass="txtBox" TextMode="MultiLine"></asp:TextBox>
   </td><td style=" border-color:Blue; border:2px solid;"  align="center">
    <asp:Label ID="Label11" runat="server" Text="HYPERTENSION" Height="40px" Width="130px" ForeColor="#CC3300"></asp:Label><br />
    <asp:ListBox ID="ListBox11" runat="server" Width="130px" 
                        onselectedindexchanged="ListBoxChanged" AutoPostBack="True" Height="110px" 
                        SelectionMode="Multiple"></asp:ListBox><br />
    <asp:TextBox ID="TextBox11" runat="server" Height="40px" Width="125px" CssClass="txtBox" TextMode="MultiLine"></asp:TextBox>
    </td><td style=" border-color:Blue; border:2px solid;"  align="center">
    <asp:Label ID="Label12" runat="server" Text="PERIPHERAL NEUROPATHY" Height="40px" Width="130px" ForeColor="#CC3300"></asp:Label><br />
    <asp:ListBox ID="ListBox12" runat="server" Width="130px" 
                        onselectedindexchanged="ListBoxChanged" AutoPostBack="True" Height="110px" 
                        SelectionMode="Multiple"></asp:ListBox><br />
     <asp:TextBox ID="TextBox12" runat="server" Height="40px" Width="125px" CssClass="txtBox" TextMode="MultiLine"></asp:TextBox>
    </td>
    <%--</tr><tr>--%>
    <td style=" border-color:Blue; border:2px solid;"  align="center">
    <asp:Label ID="Label13" runat="server" Text="CARDIAC LV FUNCTION" Height="40px" Width="130px" ForeColor="#CC3300"></asp:Label><br />
    <asp:ListBox ID="ListBox13" runat="server" Width="130px" onselectedindexchanged="ListBoxChanged" AutoPostBack="True" Height="110px" SelectionMode="Multiple"></asp:ListBox><br />
    <asp:TextBox ID="TextBox13" runat="server" Height="40px" Width="125px" CssClass="txtBox" TextMode="MultiLine"></asp:TextBox>
    </td><td style=" border-color:Blue; border:2px solid;"  align="center"> 
    <asp:Label ID="Label14" runat="server" Text="PULMONARY FUNCTION" Height="40px" Width="130px" ForeColor="#CC3300"></asp:Label><br />
    <asp:ListBox ID="ListBox14" runat="server" Width="130px" onselectedindexchanged="ListBoxChanged" AutoPostBack="True" Height="110px" SelectionMode="Multiple"></asp:ListBox><br />
    <asp:TextBox ID="TextBox14" runat="server" Height="40px" Width="125px" CssClass="txtBox" TextMode="MultiLine"></asp:TextBox>
    </td><td style=" border-color:Blue; border:2px solid;"  align="center">
    <asp:Label ID="Label15" runat="server" Text="RENAL FUNCTION " Height="40px" Width="130px" ForeColor="#CC3300"></asp:Label><br />
    <asp:ListBox ID="ListBox15" runat="server" Width="130px" onselectedindexchanged="ListBoxChanged" AutoPostBack="True" Height="110px" SelectionMode="Multiple"></asp:ListBox><br />
    <asp:TextBox ID="TextBox15" runat="server" Height="40px" Width="125px" CssClass="txtBox" TextMode="MultiLine"></asp:TextBox>
    </td><td style=" border-color:Blue; border:2px solid;"  align="center">
    <asp:Label ID="Label16" runat="server" Text="PROTEINURIA" Height="40px" Width="130px" ForeColor="#CC3300"></asp:Label><br />
    <asp:ListBox ID="ListBox16" runat="server" Width="130px" onselectedindexchanged="ListBoxChanged" AutoPostBack="True" Height="110px" SelectionMode="Multiple"></asp:ListBox><br />
    <asp:TextBox ID="TextBox16" runat="server" Height="40px" Width="125px" CssClass="txtBox" TextMode="MultiLine"></asp:TextBox>
    </td>
    </tr></table>
    </asp:Panel>
    
    </div>
    </ContentTemplate></asp:UpdatePanel>
</asp:Content>
