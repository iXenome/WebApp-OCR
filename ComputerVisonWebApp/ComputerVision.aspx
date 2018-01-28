<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ComputerVision.aspx.cs" Inherits="ComputerVisonWebApp.ComputerVision" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Image ID="MyImage" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/uploadImage.png" />
    <asp:TextBox ID="MyTextBox" runat="server" TextMode="MultiLine" Height="500px" Width="200px"></asp:TextBox>
    <br />
    <asp:FileUpload ID="ImageUpload" runat="server" />
    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
</asp:Content>
