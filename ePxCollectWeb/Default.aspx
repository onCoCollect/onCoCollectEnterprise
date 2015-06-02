<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/MasterPage/ePxWebEmpty.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="ePxCollectWeb._Default" %>

<%@ Register assembly="DatePickerControl" namespace="DatePickerControl" tagprefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<script type="text/jscript">
    function ShowCal() {
        if (document.getElementById("Cal").style.display == "none") {
            document.getElementById("Cal").style.display = "";
        } else {
            document.getElementById("Cal").style.display = "none";
        }
        //alert(document.getElementById("Cal").style.display);
    }
</script>
    <h2>
        Welcome to ePxWeb Portal 
    </h2>
    <br />
    <br/>
    
    <div>
        One stop shop for all prescription data.
        
        &nbsp;</div><div id="Cal" style=" ">
    </div>
    <p>
        Some more data would go here
    </p>
</asp:Content>
