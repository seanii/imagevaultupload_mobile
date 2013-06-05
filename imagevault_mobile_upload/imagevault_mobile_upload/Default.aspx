<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="imagevault_mobile_upload._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

		<form method="POST" enctype="multipart/form-data" runat="server" data-ajax="false">	
        	        	
            <asp:DropDownList ID="DropDownList1" runat="server" data-corners="false">
            </asp:DropDownList>        
            <input type="file" ID="FileUpload1" name="myFile"/>
            <asp:Button ID="Button1" runat="server" onclick="Button1_Click1" Text="Upload it!" data-icon="plus" data-role="button" data-iconpos="right" />
			
             
		</form>
</asp:Content>
