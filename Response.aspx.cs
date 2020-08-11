using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Open
{
	public partial class Response : System.Web.UI.Page
	{
		clsLayer layerAPI;

		protected void Page_Load(object sender, EventArgs e)
		{
			string status = verifyPayment();
			lblStatus.Text = status;
		}

		private string verifyPayment()
		{
			try
			{
				string accessKey = ConfigurationManager.AppSettings["accesskey"];
				string secretKey = ConfigurationManager.AppSettings["secretkey"];
				string environment = ConfigurationManager.AppSettings["environment"];

				layerAPI = new clsLayer(accessKey, secretKey, environment);

				string status = "";

				string paymentId = Request.Params["layer_payment_id"];
				if (string.IsNullOrEmpty(paymentId))
				{
					status = "Invalid response.";
				}

				decimal amount = 0;
				if (status == "" && decimal.TryParse(Request.Params["layer_order_amount"], out amount) == false)
				{
					status = "Invalid response.";
				}

				string transactionId = Request.Params["tranid"];
				if (status == "" && string.IsNullOrEmpty(transactionId))
				{
					status = "Invalid response.";
				}

				string paymentTokenId = Request.Params["layer_pay_token_id"];
				if (status == "" && string.IsNullOrEmpty(transactionId))
				{
					status = "Invalid response.";
				}

				//calculate and compare hash
				string hashValue = layerAPI.CreateHash(paymentTokenId, amount, transactionId);
				if (status == "" && hashValue != Request.Params["hash"])
				{
					status = "Invalid hash";
				}

				if (status == "")
				{
					var result = layerAPI.GetPaymentDetails(paymentId);

					JObject jObject = JsonConvert.DeserializeObject<JObject>(result);

					if (jObject["error"] != null)
					{
						status = "Layer: an error occurred E14" + jObject.GetValue("error").ToString();
					}

					if (status == "" && (jObject["id"] == null || jObject.GetValue("id").ToString() == ""))
					{
						status = "Invalid payment data received E98";
					}

					if (status == "" && jObject["payment_token"]["id"].ToString() != paymentTokenId)
					{
						status = "Layer: received layer_pay_token_id and collected layer_pay_token_id doesnt match";
					}

					if (status == "" && Convert.ToDecimal(jObject.GetValue("amount")) != amount)
					{
						status = "Layer: received amount and collected amount doesnt match";
					}

					if (status == "")
					{
						string id = jObject.GetValue("id").ToString();

						switch (jObject.GetValue("status").ToString())
						{
							case "authorized":
							case "captured":
								status = "Payment captured: Payment ID " + id;
								break;
							case "failed":
							case "cancelled":
								status = "Payment cancelled/failed: Payment ID " + id;
								break;
							default:
								status = "Payment pending: Payment ID " + id;
								break;
						}
					}
				}

				return status;
			}
			catch (Exception ex)
			{
				return "Error: " + ex.Message;
			}
		}
	}
}