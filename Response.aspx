<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Response.aspx.cs" Inherits="Open.Response" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Kit for Layer Payment</title>
    <link rel="stylesheet" type="text/css" href="Style.css" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" >
</head>
<body>
    <div class="wrapper">
        <form id="receiptForm" runat="server">
            <div class="divLogo">
                <img alt="logo" src="logo.png" />
            </div>
            <div class="divSection">
                <asp:Label ID="lblStatus" CssClass="label" runat="server" Text=""></asp:Label>
                <br />
            </div>
            <div class="divSection">
                <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl="~/Default.aspx">Another Payment</asp:LinkButton>
            </div>
        </form>
    </div>
</body>
</html>
