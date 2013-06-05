<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Organize.aspx.cs" Inherits="imagevault_mobile_upload.Organize" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" method="POST" runat="server">
    
          <div class="metadatafield">
              <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
          </div>
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Button" />
        

        
    </form>
</asp:Content>
