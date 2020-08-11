using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Open.layer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Open
{
	public partial class Default : System.Web.UI.Page
	{
		clsPaymentData data;
		clsLayer layerAPI;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Page.IsPostBack)
			{
				Server.Transfer("~/response.aspx", true);
			}
			else
			{
				setupPayment();

				lblName.Text = "Full Name: " + data.name;
				lblEmail.Text = "Email: " + data.email_id;
				lblMobile.Text = "Mobile: " + data.contact_number;
				lblAmount.Text = "Amount: " + data.amount.ToString("#0.00");
			}
		}

		private void setupPayment()
		{
			try
			{
				string accessKey = ConfigurationManager.AppSettings["accesskey"];
				string secretKey = ConfigurationManager.AppSettings["secretkey"];
				string environment = ConfigurationManager.AppSettings["environment"];

				layerAPI = new clsLayer(accessKey, secretKey, environment);

				data = new clsPaymentData();
				data.amount = Convert.ToDecimal(ConfigurationManager.AppSettings["sampleDataAmount"]);
				data.currency = ConfigurationManager.AppSettings["sampleDataCurrency"].ToString();
				data.name = ConfigurationManager.AppSettings["sampleDataName"].ToString();
				data.contact_number = ConfigurationManager.AppSettings["sampleDataContactNumber"].ToString();
				data.email_id = ConfigurationManager.AppSettings["sampleDataEmail"].ToString();
				data.mtx = DateTime.Now.ToString("yyyyMMdd") + "-" + new Random().Next(1000, 9999).ToString();

				JObject jObject = new JObject();
				JObject jObjectToken = new JObject();

				//create payment token
				var result = layerAPI.CreatePaymentToken(data);

				jObject = JsonConvert.DeserializeObject<JObject>(result);

				string errMessage = "";

				if (jObject["error"] != null)
				{
					errMessage = "E55 Payment error. " + jObject.GetValue("error").ToString();
					//add code for error list
					if (jObject["error_data"] != null)
					{
						foreach (JProperty p in jObject["error_data"])
						{
							string e = p.Value.ToString().Replace("\r", "").Replace("\n", "").Replace("[", "").Replace("]", "").Trim();
							errMessage += " " + e;
						} 
					}				
				}

				if (errMessage == "" && jObject["id"] != null && jObject.GetValue("id").ToString() == "")
				{
					errMessage = "Payment error. Layer token ID cannot be empty.";
				}

				if (errMessage == "")
				{
					//check that the payment is setup correctly and has not been pad
					var resultToken = layerAPI.GetPaymentToken(jObject.GetValue("id").ToString());

					jObjectToken = JsonConvert.DeserializeObject<JObject>(result);

					if (jObjectToken["error"] != null)
					{
						errMessage = "E56 Payment error. " + jObjectToken.GetValue("error").ToString();
						//add code for error list
					}

					if (errMessage == "" && jObjectToken["status"] != null && jObject.GetValue("status").ToString() == "paid")
					{
						errMessage = "Layer: this order has already been paid.";
					}

					if (errMessage == "" && jObjectToken["amount"] != null &&
						Convert.ToDecimal(jObjectToken.GetValue("amount")) != Convert.ToDecimal(jObject.GetValue("amount")))
					{
						errMessage = "Layer: an amount mismatch occurred";
					}
				}

				if (errMessage.Length > 0)
				{
					lblError.Text = errMessage;
					return;
				}

				//setup hidden values
				string paymentTokenId = jObjectToken.GetValue("id").ToString();
				string amount = (Convert.ToDecimal(jObjectToken.GetValue("amount"))).ToString("#0.00");

				//create hash
				string hashValue = layerAPI.CreateHash(paymentTokenId,
					Convert.ToDecimal(jObjectToken.GetValue("amount")), data.mtx);

				layer_pay_token_id.Value = paymentTokenId;
				tranid.Value = data.mtx;
				layer_order_amount.Value = amount;
				layer_payment_id.Value = "";
				fallback_url.Value = "";
				hash.Value = hashValue;
				key.Value = layerAPI.accessKey;

			}
			catch(Exception ex)
			{
				lblError.Text = "Error: " + ex.Message;
			}

		}

		protected void btnPay_Click(object sender, EventArgs e)
		{

		}
	}
}