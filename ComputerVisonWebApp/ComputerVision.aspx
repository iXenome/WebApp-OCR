<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ComputerVision.aspx.cs" Inherits="ComputerVisonWebApp.ComputerVision" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Image ID="MyImage" runat="server" Height="500px" ImageAlign="Middle" Width="500px" />
    <br />
    <asp:TextBox ID="MyTextBox" runat="server" Width="1000px" Height="500px" TextMode="MultiLine"></asp:TextBox>
    <br />
    <asp:FileUpload ID="ImageUpload" runat="server" />
    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
</asp:Content>
