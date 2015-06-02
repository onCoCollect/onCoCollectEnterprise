<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="True" CodeBehind="AnalysisResult.aspx.cs" Inherits="ePxCollectWeb.AnalysisResult" EnableEventValidation="false" %>

<%@ Register Assembly="DatePickerControl" Namespace="DatePickerControl" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        DIV.dd_chk_select {
            border-color: #CCCCCC;
            border-style: solid;
            border-width: 1px;
            height: 18px;
            padding: 0px 0px 0px 0px;
            text-align: left;
            vertical-align: middle;
            font-size: 12px;
            text-decoration: none;
            overflow: visible;
            color: Black;
            background-color: white;
            background-image: url("WebResource.axd?d=61EFMEfrfoV68kXieJ5eJ-7_ywU29XTbwVkf8KYBBoUjQ1IBB8tLl2rmkcMzGM2Fqvd_v4w5AVVe1Vn8JUqukqCEVm5okPmKqmcXqatqoEqzrF9Ax2Nt5npdUTe-tJW1vUwU1IA2pnjHB3Ctc0uYznnrmcXG-e-1C_gQmKUUEbM1&t=635519086320000000");
            background-position: right center;
            background-repeat: no-repeat;
            width: 200px;
            display:block;
        }

            DIV.dd_chk_select DIV#caption {
                width: auto;
                /*overflow:hidden;*/
                display: block;
                position: relative;
                /*top: 0px;
            left: 0px;
            z-index: 9999;
            display:inline-block;
            position:relative;*/
            }

        #dvListLoading {
            background: url(../images/fancybox_loading.gif) no-repeat center center;
            position: fixed;
            z-index: 1000;
            top: 0%;
            left: 0%;
            margin: 0;
            height: 100%;
            width: 100%;
            background-color: gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }
    </style>
    
<script type="text/javascript">

    function showProgress() {
        $('#dvListLoading').show();
    }

    function HideProgress() {
        $('#dvListLoading').fadeOut(1000);
    }

</script>
    <script type="text/javascript">

        function modalpopupshow() {
            $find('ModalPopupExtender2').show();

        }
        //function showExportExcelPopup()
        //{

        //    //var x = screen.width / 2 - 100 / 2;
        //    //var y = screen.height / 2 - 50 / 2;
        //    window.open("ExportData.aspx", "Export", "width=376, height=296,left=400,top=200");
        //    //function popupwindow(url, title, w, h) {
        //        //var left = (screen.width / 2) - (w / 2);
        //        //var top = (screen.height / 2) - (h / 2);
        //        //return window.open("ExportData.aspx", "ExportExcel", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);

        //       // ("ExportData.aspx", "ExportData", 'status:no;directories=no;addressbar=0;titlebar=no;menu=no;toolbar=no;location=0;status=no;menubar=no;scrollbars=no;resizable=no;dialogWidth:590px;dialogHeight150px;dialogHide:true;help:no;scroll:no');  //, null, 'status:no;dialogWidth:600px;dialogHeight:300px;dialogHide:true;help:no;scroll:yes');
        //  //  window.showModalDialog("ExportData.aspx", "ExportData", 'status:no;directories=no;addressbar=0;titlebar=no;menu=no;toolbar=no;location=0;status=no;menubar=no;scrollbars=no;resizable=no;dialogWidth:590px;dialogHeight150px;dialogHide:true;help:no;scroll:no');  //, null, 'status:no;dialogWidth:600px;dialogHeight:300px;dialogHide:true;help:no;scroll:yes');

        //}
        function showAccessPopup2() {

            //window.open("GroupAcess.aspx", "AccessGroup", 'status:no;directories=no;addressbar=0;titlebar=no;menu=no;toolbar=no;location=0;status=no;menubar=no;scrollbars=no;resizable=no;dialogWidth:1500px;dialogHeight150px;dialogHide:true;help:no;scroll:no');  //, null, 'status:no;dialogWidth:600px;dialogHeight:300px;dialogHide:true;help:no;scroll:yes');
            window.open("GroupAcess.aspx", "AccessGroup", "width=1200, height=400,left=70,top=100");  //, null, 'status:no;dialogWidth:600px;dialogHeight:300px;dialogHide:true;help:no;scroll:yes');
        }
        function frequencyPopup() {
            var ddlReport = document.getElementById("<%=dpFrequency.ClientID%>");


            var Text = ddlReport.options[ddlReport.selectedIndex].text;
            var Value = ddlReport.options[ddlReport.selectedIndex].value;
            if (Text == '') {
                alert('Please select a Frequency.');
            }

            else {
                //     window.Open("FrequencyPopup.aspx?filterText=" + Text, "Frequency", 'toolbar=0,scrollbars=0,location=0,statusbar=0,menubar=0,resizable=0,width=200,height=400');  //, null, 'status:no;dialogWidth:600px;dialogHeight:300px;dialogHide:true;help:no;scroll:yes');
                //window.showModalDialog("FrequencyPopup.aspx?filterText=" + Text, "Frequency", 'status:no;directories=no;addressbar=0;titlebar=no;menu=no;toolbar=no;location=0;status=no;menubar=no;scrollbars=no;resizable=no;dialogWidth:590px;dialogHeight150px;dialogHide:true;help:no;scroll:no');  //, null, 'status:no;dialogWidth:600px;dialogHeight:300px;dialogHide:true;help:no;scroll:yes');               
                $("#Frequencydiag")
                    .html('<iframe src="/FrequencyPopup.aspx?filterText=' + Text + '" height="400px" width="100%"></iframe>')
                    .dialog(
                    {
                        model: true,
                        width: '60%',
                        minHeight: '400',
                        resizable: false,
                        title: 'Frequency',
                    }
                    );
            }

        }
        function ValidateDropDown() {
            var ddlReport = document.getElementById("<%=ddlTemplates.ClientID%>");


            var Text = ddlReport.options[ddlReport.selectedIndex].text;

            if (Text == '') {
                alert('Please select a Template');
            }



        }

        function isNumberKey(evt) {

            var k;
            document.all ? k = e.keyCode : k = event.which;

            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57) || k > 31 && (k < 48 || k > 57))
                return false;
            // (charCode >= 48 && charCode <= 53) || (charCode >= 65 && charCode <= 69) || (charCode >= 97 && charCode <= 101)
            return true;
        }

        function numericvalidation() {
            var key = event.keyCode;
            if ((key >= 48) && (key <= 58) || (key == 46)) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>

    <script type="text/javascript">        
        function OnRadioSelection(control) {

            var button = null;
            //var selection = -1;


            if (control) {

                for (var i = 0; i < control.cells.length; i++) {

                    button = document.getElementById(control.id + "_" + i);
                    selection = i

                    if (button && button.checked) {



                        if (selection == "0") {
                            document.getElementById("<%=ddlTemplates.ClientID%>").disabled = true;
                            document.getElementById("<%=lblExportMessage.ClientID%>").innerText = "";
                            document.getElementById("<%=ddlTemplates.ClientID%>").value = "";
                            //$find("ModalPopupExtender1").show();
                        }

                        if (selection == "1") {
                            document.getElementById("<%=ddlTemplates.ClientID%>").disabled = false;
                            // $find("ModalPopupExtender1").show();
                        }

                        break;
                    }


                }


            }

        }


        function validateDropDown2() {
           
            var dpc = document.getElementById("<%=dpColumns.ClientID%>");

            var dpcText = dpc.options[dpc.selectedIndex].value;
            // alert(dpcText);

            var dpo = document.getElementById("<%=dpOperator.ClientID%>");

            var dpoText = dpo.options[dpo.selectedIndex].value;
            // alert(dpoText);

            if ((dpcText == 'Select') || (dpoText == 'Select')) {
                alert('Please select required Filter Values.');
            }
            return false;


        }

        window.onfocus = focusPopup;
        window.onunload = function () { void (0) }
        //window.onload = function () {
        //    window.opener.document.body.disabled = true;
        //}

        //window.onunload = function () {
        //    window.opener.document.body.disabled = false;
        //}

        var popupWindow = null;
        function OpenStats() {
            $("#statsdiag").dialog(
                {
                    model: true,
                    width: '60%',
                    minHeight: '400',
                    resizable: false,
                    title: 'Stats',
                }
                );
            //var opt = 'dialogWidth:800px; dialogHeight:300px;location=0;menu=no; center:yes; scroll:no; status:no';
            //window.showModalDialog("Stats.aspx", "Statistrics", opt, true);
        }


        function showExportExcelPopup() {
          
            var control = document.getElementById("<%=ddlTemplates.ClientID%>")
            document.getElementById("<%=ddlTemplates.ClientID%>").value = "";

            control.cells[0].selection = true;

            $find("ModalPopupExtender1").show();
            // var opt = "top=200,left=500,height=297,width=376,location=no,directories=no,menubar=0,resizable=no ,center=yes, scroll=no, status=no";
            /* Commented Feb05
               
                 //  var opt = 'status:no;directories=no;addressbar=0;titlebar=no;menu=no;toolbar=no;location=0;status=no;menubar=no;scrollbars=no;resizable=no;dialogWidth:590px;dialogHeight:350px;dialogHide:true;help:no;scroll:no'
               // window.showModalDialog("ExportData.aspx", "ExportData",opt,true);
               // 'status:no;directories=no;addressbar=0;titlebar=no;menu=no;toolbar=no;location=0;status=no;menubar=no;scrollbars=no;resizable=no;dialogWidth:590px;dialogHeight350px;dialogHide:true;help:no;scroll:no');  //, null, 'status:no;dialogWidth:600px;dialogHeight:300px;dialogHide:true;help:no;scroll:yes');
               */
            // popupWindow = window.open("ExportData.aspx", "ExportData", opt, true);

            //popupWindow.focus();
            //document.onmousedown = focusPopup;    
            //document.onkeyup = focusPopup;
            //document.onmousemove = focusPopup;
            //document.onclick = focusPopup;
            //LoadModalDiv();

        }

        function HideModalPopup() {

            $find("ModalPopupExtender1").hide();

            return false;

        }

        function focusPopup() {
            if (popupWindow && !popupWindow.closed) {
                popupWindow.focus();


            }
        }

        function DisableRightClick(e) {
            if (e.which == 1 || e.which == 2) {
                alert(message);
                return false;
            }

        }
        function checkTemplate() {
            
            var dpoText = document.getElementById("<%=ddlTemplates.ClientID%>").value;
            var control = document.getElementById("<%=rdExportType.ClientID%>");
            for (var i = 0; i < control.cells.length; i++) {

                button = document.getElementById(control.id + "_" + i);
                selection = i

                if (button && button.checked) {
                    if ((dpoText == '') && (selection == 1)) {
                        document.getElementById("<%=lblExportMessage.ClientID%>").innerText = "Please select a Template to Export with Grouping.";
                        document.getElementById("<%=ddlTemplates.ClientID%>").disabled = false;
                        return false;

                    }
                    else {
                        document.getElementById("<%=lblExportMessage.ClientID%>").innerText = "";
                        return true;
                    }

                }


            }
        }
        function showAccessPopup() {
            //var opt = "dialogWidth:1200px;dialogHeight:400px,top=150;left=75;edge: Raised;location=no;directories=no;menubar=no;resizable=no;center=yes;scroll=no;status=no";
            //window.showModalDialog("GroupAcess.aspx", "AccessGroup", opt, true);
            ////popupWindow = window.open("GroupAcess.aspx", "AccessGroup", opt, true);  //, null, 'status:no;dialogWidth:600px;dialogHeight:300px;dialogHide:true;help:no;scroll:yes');
            ////document.onmousedown = focusPopup;
            ////document.onkeyup = focusPopup;
            ////document.onmousemove = focusPopup;
            //// LoadModalDiv();
            $("#Analysisaccessdiag").dialog(
                {
                    model: true,
                    width: '60%',
                    minHeight: '400',
                    resizable: false,
                    title: 'Analysis access',
                    buttons: {
                        'close': function () {
                            $(this).dialog('close');
                        }
                    }
                }
                );
        }

        function LoadModalDiv() {
            var bcgDiv = document.getElementById("divBackground");
            bcgDiv.style.display = "block";

        }

        function HideModalDiv() {
            var bcgDiv = document.getElementById("divBackground");
            bcgDiv.style.display = "none";
            popUpObj = null;
        }
    </script>

    <asp:UpdatePanel runat="server" ID="updREsult" UpdateMode="Conditional">

        <ContentTemplate>
            <ajax:ModalPopupExtender ID="ModalPopupExtender1" TargetControlID="Hidden1" PopupControlID="pnlConfirm"
                CancelControlID="btnNo" BackgroundCssClass="modalBackground" DropShadow="true" BehaviorID="ModalPopupExtender1"
                runat="server">
            </ajax:ModalPopupExtender>
            <ajax:ModalPopupExtender ID="ModalPopupExtender2" TargetControlID="Hidden2" PopupControlID="Panel1"
                CancelControlID="btnNo" BackgroundCssClass="modalBackground" DropShadow="true"
                runat="server">
            </ajax:ModalPopupExtender>


            <div style="border-width: 1px; border-style: solid;">
                <div style="margin-top: 10px; height: 160px; border-bottom: solid 1px gray; border: none;">
                    
       <div id="dvListLoading" style="display: none;">
                </div>
                    <center> 
                        <table>
                            <tr style="margin: 2px; padding: 1px;"><td >
                                 <asp:DropDownList ID="dpColumns" runat="server"  height="20px" OnSelectedIndexChanged="dpColumns_SelectedIndexChanged" CssClass="dllCss" AutoPostBack="true"></asp:DropDownList>
                                </td>
                                <td style="margin: 0px; padding: 0px;">   
                                    <asp:DropDownList ID="dpOperator" runat="server" height="20px" OnSelectedIndexChanged="dpOperator_SelectedIndexChanged" CssClass="dllCss" AutoPostBack="true">
                                        <asp:ListItem>Select</asp:ListItem>
                                        <asp:ListItem>=</asp:ListItem>
                                        <asp:ListItem>&lt;&gt;</asp:ListItem>
                                        <asp:ListItem>Contains</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownCheckBoxes ID="dpValues" runat="server"  UseSelectAllNode="true" CssClass="dllCss" height="20px" OnSelectedIndexChanged="dpValues_SelectedIndexChanged" UseButtons="True" ></asp:DropDownCheckBoxes>
                                </td>
                            </tr>
                        
                     
                     
                        <%--<asp:DropDownList ID="dpValues" runat="server" Width="180px" 
                        onselectedindexchanged="dpValues_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>--%>
                            <table>
                                <tr style="margin: 0px; padding: 0px;">
                                    <td>
                                        <asp:DropDownList ID="ddlValue1" runat="server" AutoPostBack="true"  CssClass="dllCss" OnSelectedIndexChanged="ddlValue1_SelectedIndexChanged" Visible="False">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblAnd" runat="server" Text=" and " Visible="false"></asp:Label>
                                        <asp:DropDownList ID="ddlValue2" runat="server" AutoPostBack="true" CssClass="dllCss" OnSelectedIndexChanged="ddlValue2_SelectedIndexChanged" Visible="False">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <tr >
                               

                            </tr>
                        </table>
                        <asp:TextBox ID="txtDataType" runat="server" Visible="False"></asp:TextBox>
                        <asp:TextBox ID="txtValue" runat="server" autocomplete="off" MaxLength="3" Visible="false" Width="200px" onkeypress="javascript:return numericvalidation();" ></asp:TextBox>
                        <asp:TextBox runat="server" ID="dtDate1" autocomplete="off" ViewStateMode="Enabled" Visible="false" onkeypress="return isNumberKey(event)"  
                            Width="100" />
                        <ajaxToolkit:CalendarExtender ID="Calendar1" runat="server" TargetControlID="dtDate1"
                            CssClass="MyCalendar" Format="dd-MM-yyyy" PopupButtonID="dtDate1" PopupPosition="BottomRight" />
                        <ajaxToolkit:CalendarExtender ID="Calendar2" runat="server" TargetControlID="txtValue2"
                            CssClass="MyCalendar" Format="dd-MM-yyyy"  PopupButtonID="dtDate1" PopupPosition="BottomRight" />

                      <%--  <ajaxToolkit:CalendarExtender ID="calDate" runat="server" TargetControlID="dtDate1"
                            CssClass="MyCalendar" Format="MMMM d, yyyy" PopupButtonID="dtDate1" PopupPosition="BottomRight" />--%>
                      
                        <asp:TextBox ID="txtValue2" runat="server" Visible="False" autocomplete="off" MaxLength="3" onkeypress="javascript:return numericvalidation();"></asp:TextBox>
                        <asp:TextBox runat="server" ID="dtDate2" ViewStateMode="Enabled" autocomplete="off" Visible="false" onkeypress="return isNumberKey(event)"
                            Width="100" />
                        <ajaxToolkit:CalendarExtender ID="CalDate1" runat="server" TargetControlID="dtDate2"
                            CssClass="MyCalendar" Format="dd-MM-yyyy" PopupButtonID="dtDate2" PopupPosition="BottomRight" />

                        <div class="btn-group btn-group-lg">
                            <asp:Button ID="Button2" runat="server" Text="Filter" OnClick="btnFilter_Click" OnClientClick="showProgress();"  CssClass="btn btn-primary"/>
                            <asp:Button ID="btnClear" runat="server" Text="Clear Filters" OnClick="btnClear_Click" CssClass="btn btn-primary" />
                            <asp:Button ID="btnStats" runat="server" Text="Stats" OnClientClick="OpenStats(); return false;"  CssClass="btn btn-primary" />
                            <asp:Button ID="btnExportExcel" runat="server" Text="Export to Excel" CssClass="btn btn-primary" OnClick="btnExportExcel_Click"   OnClientClick="showExportExcelPopup();return false;"  Visible="true"  />
                            <asp:Button ID="btnSaveSearch" runat="server" Text="Save your Search" CssClass="btn btn-primary" OnClientClick="fnPopupSaveQuery(); return false;" />
                            <asp:Button ID="btnPermissions" runat="server" Text="Analysis Access" OnClick="btnPermissions_Click" CssClass="btn btn-primary" OnClientClick="showAccessPopup();return false;"/>
                            <asp:Button ID="Button1" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="btnClose_Click" />
                        </div>
                         
                       <%-- <div style="margin-top: 5px">
                            <asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" CssClass="button" Height="25px"
                                Width="72px" />
                            &nbsp;
                            <asp:Button ID="btnClear" runat="server" Text="Clear Filters" Width="85px" OnClick="btnClear_Click" CssClass="button" />
                           &nbsp;<asp:Button ID="btnStats" runat="server" Text="Stats" OnClientClick="OpenStats(); return false;"  CssClass="button"/>
                           &nbsp;<asp:Button ID="btnExportExcel" Width="120px" runat="server" Text="Export to Excel" CssClass="button" 
                                OnClick="btnExportExcel_Click" Visible="true" />
                            &nbsp;<asp:Button ID="btnSaveSearch" runat="server" Text="Save your Search" Width="130px" CssClass="button"
                                OnClientClick="fnPopupSaveQuery(); return false;" />
                           &nbsp;  <asp:Button ID="btnPermissions" runat="server" Text="Analysis Access" OnClick="btnPermissions_Click" CssClass="button" Width="150px"  />
                            &nbsp; <asp:Button ID="Button1" runat="server" Text="Close" CssClass="button" OnClick="btnClose_Click" />
                        </div>--%>
                    </center>

                    <table style="margin-left: 45px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblFrequency" Visible="True" Text="Frequency :" runat="server" Style="font-family: Verdana; font-size: 12px; font-weight: bold;"></asp:Label></td>
                            <td>
                                <asp:DropDownList ID="dpFrequency" runat="server" Height="20px" Width="300px" CssClass="dllCss"
                                    AutoPostBack="true">
                                </asp:DropDownList></td>
                            <td>
                                <asp:Button ID="btnFreqencyGene" runat="server" Height="25px" Width="100px" Text="Generate" CssClass="button" OnClientClick="frequencyPopup();return false;"
                                    OnClick="btnFreqencyGene_Click" /></td>

                        </tr>
                    </table>
                </div>
                <%-- OnClientClick="javascript:modalpopupshow();return true;"</asp:Panel>                
                --%>
            </div>
            &nbsp;<asp:Panel ID="GridViewPanel" runat="server" Wrap="False">
                <center>
                        <div >&nbsp;<asp:Label runat="server" ID="lblCaption" Text="Total No. Of Records : " Font-Bold="True" Font-Size="Medium"></asp:Label>
                            <asp:Label runat="server" ID="lblCaptionCount" Text="" Font-Bold="True" Font-Size="Medium"></asp:Label>
               <asp:HiddenField ID="Hidden1" runat="server"  />
                             <asp:HiddenField ID="Hidden2" runat="server"   />
                <asp:Label Text="NO" ID="lblFilterStatus" runat="server" Visible="false"></asp:Label>
            </div>
            </center>
                <br />

            </asp:Panel>

            <asp:TextBox ID="lblMessage" runat="server" ForeColor="black" Height="260px" Font-Size="11px"
                ReadOnly="True" TextMode="MultiLine" Width="798px" Font-Names="verdana">No Filters applied!</asp:TextBox>

            <br />

            <br />

            <asp:Label ID="lblFilter" runat="server" ForeColor="#004080"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>


    <asp:UpdatePanel runat="server" ID="updConfirm" ChildrenAsTriggers="true">
        <ContentTemplate>

            <asp:Panel ID="pnlConfirm" runat="server" Height="300px" Width="500px" BackColor="white"
                BorderColor="Black" Style="border: 1px solid; display: none;">
                <div style="width: 100%; background: #006699; height: 25px; color: White">
                    <h4 style="font-family: Verdana; font-size: 11px; margin-left: 10px;">Export to Excel</h4>
                </div>

                <%--  <table align="center" style="margin-top: 20px; color: Black;" width="450px">
                    <tr>
                    </tr>
                    <tr>
                   
                    <tr>
                        <td colspan="2" style="display: inline-flex; font-size: 11px; font-family: Verdana; OnSelectedIndexChanged="rdExportType_SelectedIndexChanged" ">--%>
                <div style="margin-left: 20px; margin-top: 10px; font-family: Verdana; font-size: 11px; width: 450px;">
                    <fieldset>
                        <legend style="font-style: italic; font-size:14px;">Export Options</legend>
                        <asp:RadioButtonList ID="rdExportType" runat="server" CssClass="exportCSS" onclick="javascript: OnRadioSelection(this);">
                            <%-- OnClientClick="JavaScript: return false;"  >--%>
                            <asp:ListItem Value="export" Selected="True">&nbsp;Export Data</asp:ListItem>
                            <asp:ListItem Value="WithGrouping">&nbsp;Export with Grouping</asp:ListItem>
                            <asp:ListItem Value="WithCoding" Enabled="false">&nbsp;Export with Grouping and Coding for Multivariate Analysis</asp:ListItem>

                        </asp:RadioButtonList>

                    </fieldset>

                </div>
                <div style="margin-left: 20px; margin-top: 10px; width: auto;">
                    Select Template :
                        
                            <asp:DropDownList ID="ddlTemplates" runat="server" CssClass="dllCss" OnSelectedIndexChanged="ddlTemplates_SelectedIndexChanged"></asp:DropDownList>
                </div>
                <br />
                <div style="margin-left: 20px; font-family: Verdana; font-size: 11px; color: red;">

                    <asp:Label ID="lblExportMessage" runat="server" Text=""></asp:Label>
                </div>
                <%--  </td>
                    </tr--%>
                <%--  <tr>
                        <td> &nbsp;
                        </td>
                   <%-- </tr>OnClientClick="return ValidateDropDown();return false;"--%>
                <%-- <tr>
                        <td colspan="2" align="center">
                --%>

                <div style="text-align:center">
                    <asp:Button ID="btnYes" runat="server" Text="OK" Width="50px" OnClick="btnYes_Click" CssClass="button" OnClientClick="return checkTemplate();" />
                    &nbsp;
                    <%-- <input type="button" id="btnNo"  onclick="btnNo_Click" value="Export To Excel"    />--%>

                    <asp:Button ID="btnNo" runat="server" Text="Close" Width="50px" CssClass="button" OnClick="btnNo_Click" UseSubmitBehavior="true" />


                    <%-- <asp:Button ID="btnNo" runat="server" Text="Close" Width="50px" Height="25px" CssClass="button" OnClick="return HideModalPopup();"  />--%>
                    <%--<asp:Button ID="Button3" runat="server" Text="Close" Width="50px" Height="25px" CssClass="button" OnClientClick="return HideModalPopup()"   OnClick="btnNo_Click" />--%>

                </div>
            </asp:Panel>


        </ContentTemplate>
        <Triggers>

            <%-- <asp:AsyncPostBackTrigger ControlID="rdExportType"  />--%>
            <%--<asp:AsyncPostBackTrigger ControlID="rdExportType" />--%>
            <%-- <asp:AsyncPostBackTrigger ControlID="rdExportType" EventName="SelectedIndexChanged" /> --%>
            <asp:PostBackTrigger ControlID="rdExportType" />
        </Triggers>
    </asp:UpdatePanel>


    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional" ChildrenAsTriggers="true">

        <ContentTemplate>

            <asp:Panel ID="Panel1" runat="server" Height="600px" Width="1200px" BackColor="white"
                BorderColor="Black" Style="border: 1px solid; display: none">
                <div style="width: 100%; background: #006699; height: 25px; color: White">
                    <h4 style="font-family: Verdana; font-size: 11px; margin-left: 10px;">Access Details</h4>
                </div>
                <div style="margin-left: 20px; margin-top: 10px; width: 300px;">
                </div>
                <%--  <table align="center" style="margin-top: 20px; color: Black;" width="450px">
                    <tr>
                    </tr>
                    <tr>
                   
                    <tr>
                        <td colspan="2" style="display: inline-flex; font-size: 11px; font-family: Verdana; ">--%>
                <div style="margin-left: 20px; margin-top: 10px; font-family: Verdana; font-size: 11px; width: 350px;">
                    <%-- <asp:GridView ID="grdAccess" runat="server">
                        <Columns></Columns>
                    </asp:GridView>--%>
                    <asp:GridView ID="grdAccess" runat="server" BackColor="White" BorderColor="#CCCCCC" ShowFooter="true" Width="1162px" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" AllowPaging="true" PageSize="10" HeaderStyle-Width="200px" OnPageIndexChanging="grdAccess_PageIndexChanging">
                        <FooterStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />

                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" Height="30px" />
                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                        <RowStyle ForeColor="#000066" />
                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                        <EmptyDataTemplate>
                            No Data Found.
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
                <br />
                <div style="margin-left: 20px; font-family: Verdana; font-size: 11px; color: red;">

                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                </div>
                <%--  </td>
                    </tr--%>
                <%--  <tr>
                        <td> &nbsp;
                        </td>
                   <%-- </tr>OnClientClick="return ValidateDropDown();return false;"--%>
                <%-- <tr>
                        <td colspan="2" align="center">
                --%>

                <div style="margin-left: 20px; margin-top: 10px;">
                    <center>
                    <%--<asp:Button ID="Close" runat="server" Text="Close" Width="50px"  Height="25px"                         CssClass="button" OnClick="Close_Click" />--%>
                    &nbsp;
                          <%--  <asp:Button ID="Button2" runat="server" Text="Close" Width="50px" Height="25px" CssClass="button" />--%>
                    </center>
                </div>
            </asp:Panel>


        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="grdAccess" />
            <%-- <asp:AsyncPostBackTrigger ControlID="grdAccess" EventName="PageIndexChanging" />--%>
        </Triggers>
    </asp:UpdatePanel>

    <div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%; top: 0; left: 0; background-color: Black; filter: alpha(opacity=60); opacity: 0.6; -moz-opacity: 0.8; display: none"></div>
    <div id="statsdiag" title="Stats" style="display:none;">
        <iframe id="Stats" src="Stats.aspx" height="400px" width="100%"></iframe>
    </div>
    <div id="SaveQuerydiag" title="Save your search" style="display:none;">
        <iframe id="SaveMyQuery" src="SaveMyQuery.aspx" height="400px" width="100%"></iframe>
    </div>
    <div id="Analysisaccessdiag" title="Analysis access" style="display:none;">
        <iframe id="GroupAcess" src="GroupAcess.aspx" height="400px" width="100%"></iframe>
    </div>
    <div id="Frequencydiag" title="Analysis access" style="display:none;">
         <!--  src set @ js -->
    </div>

    <link href="Scripts/jquery-ui-1.11.2.custom/jquery-ui.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.10.2.js"></script>
    <script src="Scripts/jquery-ui-1.11.2.custom/jquery-ui.js"></script>
</asp:Content>
