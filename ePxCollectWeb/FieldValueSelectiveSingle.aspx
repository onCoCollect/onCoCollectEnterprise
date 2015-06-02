<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FieldValueSelectiveSingle.aspx.cs" Inherits="ePxCollectWeb.FieldValueSelectiveSingle" %>
    <%@ Register TagPrefix="SVP" TagName="SingleValuePick" Src="SingleSelectivePick.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
 
        <SVP:SingleValuePick runat="server" ID="SS"  />
    </div>
    </form>
</body>
</html>
