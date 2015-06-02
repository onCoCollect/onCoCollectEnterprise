<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="True" CodeBehind="ProjectForm.aspx.cs" Inherits="ePxCollectWeb.ProjectForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function DoPostBack() {
            __doPostBack("ctl00_MainContent_2nd_-Surgery_RTX_Y", "TextChanged");
        }

        function DoPostBack(input) {
            __doPostBack(input, "TextChanged");
        }
        function checkRedirectPage(txt) {
            alert(txt);
            window.location("ProjectForm.aspx");
        }
        $(document).ready(function () {
            var ypos = $("#divMastercenter").offset().top;
            var yposchange1 = $("#divMastercenter").scrollTop();
            $("#ctl00_MainContent_Hidden1").val(0);
        });

        $("#divMastercenter").scroll(function () {
            var yposchange1 = $("#ctl00_MainContent_Hidden1").val();
            var yposchange2 = $("#divMastercenter").scrollTop();
            var newchange = yposchange1 - yposchange2;
            var ycurtop = $("#calendar").offset().top;
            var newchange = 0;
            var tops = 0;

            if (yposchange1 > yposchange2) {
                newchange = yposchange1 - yposchange2;
                tops = eval(ycurtop) + eval(newchange);
                if (tops <= 112)
                    tops = 112;
            }
            if (yposchange1 < yposchange2) {
                newchange = yposchange2 - yposchange1;
                tops = eval(ycurtop) - eval(newchange);
                if (tops <= 112)
                    tops = 112;

            }
            //code modified on 28 April, 2015 - Thiru
            if (tops != 0) {
                var lefts = eval($("#calendar").offset().left);
                $("#ctl00_MainContent_Hidden1").val(yposchange2);
                $("#calendar").offset({ top: tops, left: lefts });
            }
        });

        $(function () {
            $('#ctl00_MainContent_btnEdit').on('click', function (e) {
                $('#myScript').html("$('a').on('mousedown', function (e) { var tmpvar = $(this).closest('div').attr('id'); if(tmpvar != 'calendar') { alert('Please save the changes.'); }});");
            });
            $('#ctl00_MainContent_btnSaveProjectForm').on('click', function (e) {
                $('#myScript').html(" ");
            });
            $('#ctl00_MainContent_btnReset').on('click', function (e) {
                $('#myScript').html(" ");
            });
        });

        function doSaveAndCloseCompositeForm(retVal) {
            if (retVal == undefined)
                retVal = "";

            if (retVal == "123123123")
                retVal = "";
            else {
                if (cur_ColName == "Total Cost of Primary Treatment(in INR)") {
                    document.getElementById(cur_ctrlName).value = retVal;
                }
                else
                    document.getElementById(cur_ctrlName).value = "";
            }
            //$("#onCoRightPickDiag").dialog('close');
        }
    </script>
    <script id="myScript" type="text/javascript"></script>
    <div class="entryOuterDiv">
        <div class="entryArea">

            <asp:UpdatePanel ID="updPHControls" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="Panel3">
                        <%-- <div id="Div1" runat="server" class="entryArea" >                           
                        <center> <asp:Label ID="lblTitle" runat="server" Font-Size="Medium"  ></asp:Label></center>
                        </div>--%>
                    </asp:Panel>
                    <asp:Panel ID="Panel1" runat="server" Width="100%" Visible="false" BorderStyle="None">
                        <div style="text-align:center;">
                            <asp:Button ID="btnEdit" runat="server" OnClick="btnAdd_Click" CssClass="button" Text="Edit Details" />
                        </div>
                        <div>
                            <asp:Label ID="lblError" runat="server" Text=""></asp:Label>

                        </div>
                        <br />
                    </asp:Panel>
                    <asp:Panel ID="Panel2" runat="server" Width="100%" Visible="false" BorderStyle="None"
                        HorizontalAlign="Center">

                        <div style="text-align:center;">
                            <asp:Button ID="btnSaveProjectForm" Text="Save Changes" runat="server" OnClick="btnSave_Click" CssClass="button" Width="120px" />
                            &nbsp; &nbsp;
                            <asp:Button ID="btnReset" Text="Cancel Changes" runat="server"
                                CssClass="button" OnClick="btnReset_Click" Width="120px" />
                        </div>
                        <br />
                    </asp:Panel>

                    <asp:Panel runat="server" ID="pnlControls">
                        <div id="DivPH" runat="server" class="entryArea">
                            <br />
                            <asp:PlaceHolder ID="phProject" runat="server" ViewStateMode="Disabled"></asp:PlaceHolder>
                        </div>
                    </asp:Panel>
                    <asp:HiddenField ID="Hidden1" runat="server" />


                    <%-- <asp:Panel ID="Panel2" runat="server" Width="100%" Visible="false" BorderStyle="None" 
             HorizontalAlign="Center"> 

    <div class="btn" >
    <asp:Button ID="btnSave" Text="Save Changes" runat="server" OnClick="btnSave_Click" CssClass="button" />
    &nbsp; &nbsp; <asp:Button ID="btnReset" Text="Cancel Changes" runat="server" 
            CssClass="button" onclick="btnReset_Click" />   </div></asp:Panel>--%>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%; top: 0; left: 0; background-color: Black; filter: alpha(opacity=60); opacity: 0.6; -moz-opacity: 0.8; display: none"></div>
</asp:Content>
