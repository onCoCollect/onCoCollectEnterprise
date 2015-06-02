<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="True"
    CodeBehind="Toxicity.aspx.cs" Inherits="ePxCollectWeb.Toxicity" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Always">
        <ContentTemplate>
            <asp:Panel runat="server" ID="Panel3">
                <div id="Div1" runat="server" class="entryArea">
                    <br />
                    <center> <asp:Label ID="lblTitle" runat="server" Font-Size="Medium" Text="Worst Toxicity" ></asp:Label></center>
                    <br />
                </div>
            </asp:Panel>
            <div style="width: 100%;">
                <table style="width: 100%;">
                    <tr>
                        <td align="left">
                            <asp:DropDownList ID="lstLines" runat="server" OnSelectedIndexChanged="lstLines_SelectedIndexChanged"
                                Width="170px" AutoPostBack="True">
                            </asp:DropDownList>
                            &nbsp;&nbsp;<asp:Label runat="server" ID="lblErrorMsg" Text="" ForeColor="Red"></asp:Label>
                            <asp:ListBox ID="lstLines1" runat="server" Width="130px" AutoPostBack="True" Height="45px"
                                OnSelectedIndexChanged="lstLines_SelectedIndexChanged" Visible="False" Font-Size="Smaller"
                                SelectionMode="Multiple"></asp:ListBox>
                            <asp:RadioButtonList ID="lstLinesR" runat="server" OnSelectedIndexChanged="lstLines_SelectedIndexChanged"
                                AutoPostBack="True" BorderColor="#CCCCCC" BorderStyle="None" Font-Size="Small"
                                RepeatLayout="OrderedList" Visible="false ">
                            </asp:RadioButtonList>
                        </td>
                        <td align="right">
                            <asp:Button ID="btnMoreToxixcity" runat="server" Text="Add More Toxicity" OnClick="btnMoreToxixcity_Click" Width="150px" Visible="true"
                                CssClass="button" />
                        </td>
                    </tr>
                </table>

            </div>
            <asp:UpdatePanel runat="server" ID="updMain">
                <ContentTemplate>
                    <div style="width: 100%">
                        <br />
                        <asp:Panel ID="pnlControls" runat="server" Height="380px" Width="100%" BackColor="AliceBlue"
                            Style="border: 1px solid;" ScrollBars="None">
                            <table width="100%">
                                <tr>
                                    <td style="border-color: Blue; border: 2px solid;" align="center">
                                        <asp:Label ID="Label1" runat="server" Text="NEUTROPENIA" Width="130px" ForeColor="#CC3300"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ListBox1" runat="server" OnSelectedIndexChanged="ListBoxChanged"
                                            Width="130px" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="TextBox1" runat="server" Height="28px" Width="125px" CssClass="txtBox"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td style="border-color: Blue; border: 2px solid;" align="center">
                                        <asp:Label ID="Label2" runat="server" Text="LEUKOCYTOPENIA" Width="130px" ForeColor="#CC3300"></asp:Label><br />
                                        <asp:DropDownList ID="ListBox2" runat="server" OnSelectedIndexChanged="ListBoxChanged"
                                            Width="130px" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="TextBox2" runat="server" Height="28px" Width="125px" CssClass="txtBox"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td style="border-color: Blue; border: 2px solid;" align="center">
                                        <asp:Label ID="Label3" runat="server" Text="THROMBOCYTOPENIA" Width="130px" ForeColor="#CC3300"></asp:Label><br />
                                        <asp:DropDownList ID="ListBox3" runat="server" OnSelectedIndexChanged="ListBoxChanged"
                                            Width="130px" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="TextBox3" runat="server" Height="28px" Width="125px" CssClass="txtBox"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td style="border-color: Blue; border: 2px solid;" align="center">
                                        <asp:Label ID="Label4" runat="server" Text="ANEMIA" Width="130px" ForeColor="#CC3300"></asp:Label><br />
                                        <asp:DropDownList ID="ListBox4" runat="server" OnSelectedIndexChanged="ListBoxChanged"
                                            Width="130px" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="TextBox4" runat="server" Height="28px" Width="125px" CssClass="txtBox"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-color: Blue; border: 2px solid;" align="center">
                                        <asp:Label ID="Label5" runat="server" Text="FEBRILE NEUTROPENIA" Width="130px" ForeColor="#CC3300"></asp:Label><br />
                                        <asp:DropDownList ID="ListBox5" runat="server" OnSelectedIndexChanged="ListBoxChanged"
                                            Width="130px" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:TextBox ID="TextBox5" runat="server" Height="28px" Width="125px" CssClass="txtBox"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td style="border-color: Blue; border: 2px solid;" align="center">
                                        <asp:Label ID="Label6" runat="server" Text="NAUSEA AND VOMITING" Width="130px" ForeColor="#CC3300"></asp:Label><br />
                                        <asp:DropDownList ID="ListBox6" runat="server" OnSelectedIndexChanged="ListBoxChanged"
                                            Width="130px" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:TextBox ID="TextBox6" runat="server" Height="28px" Width="125px" CssClass="txtBox"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td style="border-color: Blue; border: 2px solid;" align="center">
                                        <asp:Label ID="Label7" runat="server" Text="DIARRHEA" Width="130px" ForeColor="#CC3300"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ListBox7" runat="server" OnSelectedIndexChanged="ListBoxChanged"
                                            Width="130px" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:TextBox ID="TextBox7" runat="server" Height="28px" Width="125px" CssClass="txtBox"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td style="border-color: Blue; border: 2px solid;" align="center">
                                        <asp:Label ID="Label8" runat="server" Text="ORAL MUCOSITIS" Width="130px"
                                            ForeColor="#CC3300"></asp:Label>
                                        <asp:DropDownList ID="ListBox8" runat="server" OnSelectedIndexChanged="ListBoxChanged"
                                            Width="130px" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:TextBox ID="TextBox8" runat="server" Height="28px" Width="125px" CssClass="txtBox"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-color: Blue; border: 2px solid;" align="center">
                                        <asp:Label ID="Label9" runat="server" Text="SKIN RASH" Width="130px"
                                            ForeColor="#CC3300"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ListBox9" runat="server" OnSelectedIndexChanged="ListBoxChanged"
                                            Width="130px" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:TextBox ID="TextBox9" runat="server" Height="28px" Width="125px" CssClass="txtBox"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td style="border-color: Blue; border: 2px solid;" align="center">
                                        <asp:Label ID="Label11" runat="server" Text="HYPERTENSION" Width="130px" ForeColor="#CC3300"></asp:Label><br />
                                        <asp:DropDownList ID="ListBox11" runat="server" OnSelectedIndexChanged="ListBoxChanged"
                                            Width="130px" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:TextBox ID="TextBox11" runat="server" Height="28px" Width="125px" CssClass="txtBox"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td style="border-color: Blue; border: 2px solid;" align="center">
                                        <asp:Label ID="Label15" runat="server" Text="RENAL FUNCTION " Width="130px" ForeColor="#CC3300"></asp:Label><br />
                                        <asp:DropDownList ID="ListBox15" runat="server" OnSelectedIndexChanged="ListBoxChanged"
                                            Width="130px" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:TextBox ID="TextBox15" runat="server" Height="28px" Width="125px" CssClass="txtBox"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td style="border-color: Blue; border: 2px solid;" align="center">
                                        <asp:Label ID="Label16" runat="server" Text="PROTEINURIA" Width="130px" ForeColor="#CC3300"></asp:Label><br />
                                        <asp:DropDownList ID="ListBox16" runat="server" OnSelectedIndexChanged="ListBoxChanged"
                                            Width="130px" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:TextBox ID="TextBox16" runat="server" Height="28px" Width="125px" CssClass="txtBox"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-color: Blue; border: 2px solid;" align="center">
                                        <asp:Label ID="Label13" runat="server" Text="CARDIAC LV FUNCTION" Width="130px" ForeColor="#CC3300"></asp:Label><br />
                                        <asp:DropDownList ID="ListBox13" runat="server" OnSelectedIndexChanged="ListBoxChanged"
                                            Width="130px" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:TextBox ID="TextBox13" runat="server" Height="28px" Width="125px" CssClass="txtBox"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td style="border-color: Blue; border: 2px solid;" align="center">
                                        <asp:Label ID="Label14" runat="server" Text="PULMONARY FUNCTION" Width="130px" ForeColor="#CC3300"></asp:Label><br />
                                        <asp:DropDownList ID="ListBox14" runat="server" OnSelectedIndexChanged="ListBoxChanged"
                                            Width="130px" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:TextBox ID="TextBox14" runat="server" Height="28px" Width="125px" CssClass="txtBox"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td style="border-color: Blue; border: 2px solid;" align="center">
                                        <asp:Label ID="Label10" runat="server" Text="HAND FOOT SYNDROME" Width="130px"
                                            ForeColor="#CC3300"></asp:Label><br />
                                        <asp:DropDownList ID="ListBox10" runat="server" OnSelectedIndexChanged="ListBoxChanged"
                                            Width="130px" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:TextBox ID="TextBox10" runat="server" Height="28px" Width="125px" CssClass="txtBox"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td style="border-color: Blue; border: 2px solid;" align="center">
                                        <asp:Label ID="Label12" runat="server" Text="PERIPHERAL NEUROPATHY" Width="130px"
                                            ForeColor="#CC3300"></asp:Label><br />
                                        <asp:DropDownList ID="ListBox12" runat="server" OnSelectedIndexChanged="ListBoxChanged"
                                            Width="130px" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:TextBox ID="TextBox12" runat="server" Height="28px" Width="125px" CssClass="txtBox"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>


                        <asp:Panel ID="pnlMoreToxicity" runat="server" Height="557px" Width="540px" BackColor="white" HorizontalAlign="Center" BorderWidth="2"
                            BorderColor="#2a75a9" Style="display: none; text-align: center; vertical-align: top;">
                            <div style="height: 100%; vertical-align: top; border: 2px solid #2a75a9" onkeydown="onKeyPressIn();">
                                <div style="width: 100%; background: #0099c8; height: 25px; color: White; font-size: large; ">
                                    <center>
                                        <b>More Toxicity</b></center>
                                </div>
                                <table align="center" style="color: Black; vertical-align: top; padding: 0px; width: 100%; position: relative">
                                </table>
                                <div style="width: 100%; padding: 0px; border: none; color: White; vertical-align: top; text-align: center; position: relative">
                                    <table>
                                        <tr>

                                            <td align="left">
                                                <asp:ListBox ID="lstLinesMoreToxicity" runat="server" Width="300px" Height="100px" CssClass="dllCss" SelectionMode="Single"></asp:ListBox>
                                                <asp:Label runat="server" ID="Label17" Text="*" ForeColor="Red"></asp:Label>

                                            </td>
                                        </tr>
                                        <tr>

                                            <td align="left">
                                                <asp:DropDownList ID="ddlToxicity" runat="server" OnSelectedIndexChanged="lstLinesMoreToxicity_SelectedIndexChanged"
                                                    Width="300px">
                                                </asp:DropDownList>
                                                <asp:Label runat="server" ID="Label18" Text="*" ForeColor="Red"></asp:Label>

                                            </td>

                                        </tr>

                                        <tr>
                                            <td align="left">
                                                <asp:ListBox ID="ListBoxGradeMoreToxixcity" runat="server" Width="300px" Height="100px" CssClass="dllCss" AutoPostBack="true" OnSelectedIndexChanged="ListBoxGradeMoreToxixcity_SelectedIndexChanged"></asp:ListBox>
                                                <asp:Label runat="server" ID="Label20" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>

                                            <td align="left">
                                                <asp:TextBox ID="txtMoreToxicityValue" runat="server" Height="100px" Width="300px" CssClass="txtBox" ReadOnly="true"
                                                    TextMode="MultiLine"></asp:TextBox>

                                            </td>

                                        </tr>
                                    </table>

                                    <center>
                                        <asp:Label ID="lblError"  runat="server" ForeColor="Red" Visible="True" Text=""></asp:Label> 
                              <br />
                       
                          <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" OnClick="btnSave_Click" CssClass="button" />
                                  &nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="100px" OnClick="btnCancel_Click" CssClass="button" />
                             </center>
                                </div>
                            </div>
                        </asp:Panel>

                        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" TargetControlID="Hidden1" PopupControlID="pnlMoreToxicity"
                            BehaviorID="ModalBehaviour" CancelControlID="btnCancel" BackgroundCssClass="overlay_back"
                            DropShadow="false" runat="server">
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:HiddenField ID="Hidden1" runat="server" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
