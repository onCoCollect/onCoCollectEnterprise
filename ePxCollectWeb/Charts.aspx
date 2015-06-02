<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Charts.aspx.cs" Inherits="ePxCollectWeb.Charts" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <asp:Chart ID="Chart1" runat="server" DataSourceID="SqlDataSource1" Height="360px" Width="455px">
        <Series>
            <asp:Series Name="Series1" XValueMember="PatientID" YValueMembers="Observed Value" ></asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="ChartArea1">
                <AxisX Title="PatientID"></AxisX>
                <AxisY Title="ObservedValue"></AxisY>
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
    <form id="form1" runat="server">
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:oCollect17NovConnectionString %>" SelectCommand="SELECT [PatientID], [Date of Investigation] AS Date_of_Investigation, [Investigation], [Observed Value] AS Observed_Value, [Measure] FROM [PatientTestsByLine]"></asp:SqlDataSource>
    <div>
    
    </div>
    </form>
</body>
</html>
