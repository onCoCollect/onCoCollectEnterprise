<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StatRightPick.aspx.cs" Inherits="ePxCollectWeb.UserControl.StatRightPick" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <!--Style Sheets-->
    <link rel="stylesheet" type="text/css" href="~/Style/style.css" />
    <link rel="stylesheet" type="text/css" href="~/Style/menu.css" />
<script type="text/javascript">
  
    

function GetSelectedValue() {

    var chkBox = document.getElementById("<%=lstValues.ClientID%>");

    var checkbox = chkBox.getElementsByTagName("input");

   <%-- var objTextBox = document.getElementById("<%=txtChkValue.ClientID%>");--%>

    var counter = 0;

    objTextBox.value = "";

    for (var i = 0; i < checkbox.length; i++) {

        if (checkbox[i].checked) {

            var chkBoxText = checkbox[i].parentNode.getElementsByTagName('label');

            if (objTextBox.value == "") {

                objTextBox.value = chkBoxText[0].innerHTML;

               // alert(objTextBox.value);

            }

            else {

                objTextBox.value = objTextBox.value + ", " + chkBoxText[0].innerHTML;

            }

        }

    }
    //SelVals = SelVals.substring(0, SelVals.length - 1)
  //  alert(objTextBox.value);
    window.returnValue = objTextBox.value;
    window.close();

}
 
    function CloseDialog() {
        window.close();
    }
    function CloseDialogWithValue() {
        var dpt = document.getElementById("<%=lstValues.ClientID%>");
        var SelVals = "";
        //alert("hi");
        //alert(dpt.options[dpt.selectedIndex].value);
        SelVals = "";
        //alert(dpt.rows.length);
        for (var i = 0; i <= dpt.rows.length-1; i++) {          
            if (dpt.rows[i].innerHTML.indexOf("CHECKED")!=-1) {
                SelVals += dpt.rows[i].innerText + ","; //dpt.options[dpt.selectedIndex].value;
                //alert(SelVals);
            }
        }
   
        SelVals = SelVals.substring(0, SelVals.length - 1)
        //alert(SelVals);
        window.returnValue = SelVals;
        window.close();     
    }
    </script>
    <style type="text/css">
        #btnOk
        {
            width: 85px;
        }
    </style>
</head>
<body bgcolor="#c0c0c0">
    <form id="form1" runat="server">
     <div class="entryOuterDiv">
     <div class="entryArea">
    <div style="width:410px;">
    <div>

<%--<asp:CheckBoxList ID="CheckBoxList1" runat="server">

<asp:ListItem Value="1">Item1</asp:ListItem>

<asp:ListItem Value="2">Item2</asp:ListItem>

<asp:ListItem Value="3">Item3</asp:ListItem>

<asp:ListItem Value="4">Item4</asp:ListItem>

</asp:CheckBoxList>--%>

<%--<asp:Button ID="Button1" runat="server" Text="Button" OnClientClick="return GetSelectedValue();" />

<asp:TextBox ID="txtChkValue" runat="server"></asp:TextBox>
        <br />--%>
        <div  style="border-style: solid; border-color: inherit; border-width: thin; OVERFLOW: auto; WIDTH: 341px; HEIGHT: 262px">
        <asp:CheckBoxList ID="lstValues" runat="server"  
               >
        </asp:CheckBoxList>
        </div>
        <br />
    
    </div>
    <div>
     <%--   <asp:Button ID="Button1" runat="server" Text="Button" OnClientClick="CloseDialogWithValue();" />--%>
     <input id="btnOk" type="button" value="Ok" onclick="return GetSelectedValue();" />
        <input id="btnClose" type="button" value="Close" onclick="CloseDialog();" />
    </div>
    </div>
    </div>
    </form>
</body>
</html>