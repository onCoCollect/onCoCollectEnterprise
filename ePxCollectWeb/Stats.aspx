<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Stats.aspx.cs" Inherits="ePxCollectWeb.Stats" %>

<%@ Register
    Assembly="hmlib.Web"
    Namespace="hmlib.Web.UI.Controls"
    TagPrefix="hmc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Statistics</title>
   
  
    <link href="Style/style.css" rel="stylesheet" />
   
</head>
<body>
    <form id="form1" runat="server">
         <script type="text/javascript">
             function CloseDialog() {
                 window.close();
             }
    </script>
         <script type="text/javascript">

             var prm = Sys.WebForms.PageRequestManager.getInstance();

             //Raised before processing of an asynchronous postback starts and the postback request is sent to the server.

             prm.add_beginRequest(BeginRequestHandler);

             // Raised after an asynchronous postback is finished and control has been returned to the browser.

             prm.add_endRequest(EndRequestHandler);

             function BeginRequestHandler(sender, args) {

                 //Shows the modal popup - the update progress

                 var popup = $find('<%= modalPopup.ClientID %>');

                 if (popup != null) {

                     popup.show();

                 }
             }

             function EndRequestHandler(sender, args) {

                 //Hide the modal popup - the update progress

                 var popup = $find('<%= modalPopup.ClientID %>');

            if (popup != null) {

                popup.hide();

            }

        }

       </script>
      <style type="text/css">
       .modalPopup {
           background-color: #696969;
           filter: alpha(opacity=40);
           opacity: 0.7;
           xindex: -1;
       }
        </style>
        <asp:ScriptManager ID="ScriptManager1" runat="server"/>
        <div>
            <asp:UpdatePanel ID="updPHControls" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div style="width: 100%; background: #006699; height: 25px; margin-left: 0px; color: White">
                        Total Records:
                    <asp:Label ID="lblTotal" runat="server"></asp:Label>
                    </div>

                    <br />
                   
                <div>

                    <asp:UpdateProgress ID="UpdateProgress" runat="server">

                        <ProgressTemplate>

                            <asp:Image ID="Image1" ImageUrl="~/images/fancybox_loading.gif" AlternateText="Processing" runat="server" />

                        </ProgressTemplate>

                    </asp:UpdateProgress>

                    <ajaxToolkit:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress"
                        PopupControlID="UpdateProgress" BackgroundCssClass="modalPopup" />
                </div>
                    <asp:Panel ID="Panel1" runat="server" BorderStyle="Solid" Height="25px" BorderColor="#CCCCCC"
                        BorderWidth="2px">
                        <asp:RadioButtonList ID="rdAnalysisType" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rdAnalysisType_SelectedIndexChanged">
                            <asp:ListItem Selected="True">Mean</asp:ListItem>
                            <asp:ListItem>Median</asp:ListItem>
                        </asp:RadioButtonList>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="dlstColumns" runat="server" Width="300px" AutoPostBack="True" CssClass="dllCss"
                                        OnSelectedIndexChanged="dlstColumns_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Panel
                                        ID="pnlProgress"
                                        runat="server"
                                        Style="display: none;">
                                        <span style="color: #006699;">Please Wait...</span>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>

                        <br />
                        <br />
                        <asp:Label ID="lblNoRecs" runat="server" ForeColor="Black" CssClass="LabelRight"
                            Text="No of Records Considered :"></asp:Label>
                        <br />
                        <asp:Label ID="lblRecsIgnored" runat="server" ForeColor="Black" CssClass="LabelRight"
                            Text="Records Ignored (no values) :"></asp:Label>
                        <br />
                        <br />
                        <asp:Label ID="lblMeanMed" runat="server" ForeColor="Black" CssClass="LabelRight"
                            Text="Mean :"></asp:Label>
                        <br />
                        <asp:Label ID="LblRange" runat="server" ForeColor="Black" CssClass="LabelRight"
                            Text="Range :"></asp:Label>
                    </asp:Panel>



                    <hmc:ProgressBar
                        ID="ProgressBar1"
                        runat="server"
                        Width="400px"
                        OnComplete="ProgressBar1_Complete"
                        ProgressControlId="pnlProgress" />

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
