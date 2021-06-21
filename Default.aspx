<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Open.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Kit for Layer Payment</title>
	<link rel="stylesheet" type="text/css" href="Style.css"/>
	<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" />
	<script src="https://sandbox-payments.open.money/layer"></script>

	 <!-- For live use , <script src="https://sandbox-payments.open.money/layer"></script> -->
</head>
<body>
	<div class="wrapper">
    <form id="paymentForm" runat="server">
		<div class="divLogo">
			<img alt="logo" src="logo.png" />
		</div>
        <div class="divSection">
            <asp:Label ID="lblName" CssClass="label" runat="server" Text="Label"></asp:Label>
			<br />
			<asp:Label ID="lblEmail" CssClass="label" runat="server" Text="Label"></asp:Label>
			<br />
			<asp:Label ID="lblMobile" CssClass="label" runat="server" Text="Label"></asp:Label>
			<br />
			<asp:Label ID="lblAmount" CssClass="label" runat="server" Text="Label"></asp:Label>
			<br />
			<asp:Label ID="lblError" CssClass="error" runat="server"></asp:Label>
			<br />
        </div>
		<div class="divSection">
            <asp:HiddenField ID="layer_pay_token_id"  runat="server" ClientIDMode="Static" />
			<asp:HiddenField ID="tranid" runat="server" />
			<asp:HiddenField ID="layer_order_amount"  runat="server" />
			<asp:HiddenField ID="layer_payment_id"  runat="server" />
			<asp:HiddenField ID="fallback_url" runat="server" />
			<asp:HiddenField ID="hash" runat="server" />
			<asp:HiddenField ID="key" runat="server" />
		</div>
		<div class="divSection">
			<input type="button" class="button" value="Pay" onclick="doPayment(); return false"/>
		</div>
    </form>
	</div>
	<script>
        function doPayment() {

            Layer.checkout(
                {
                    token: document.getElementById("layer_pay_token_id").value,
					accesskey: document.getElementById("key").value
                },
				function (response) {
					console.log(response);
					if (response !== null || response.length > 0) {
                        if (response.payment_id !== undefined) {
                            document.getElementById('layer_payment_id').value = response.payment_id;
                        }
                    }
                    document.forms.paymentForm.submit();
                },
                function (err) {
                    alert(err.message);
                }
            );
        }
	</script>
</body>
</html>
