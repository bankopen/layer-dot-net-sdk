using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Open.layer;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Reflection.Emit;

namespace Open
{
	public class clsLayer
	{
		public string accessKey;
		private string secretKey = "";
		private string environment = "";

		private string createTokenURI;
		private string getpaymentURI;

		private string baseURIUAT = "https://icp-api.bankopen.co/api";
		private string baseURISandbox = "https://sandbox-icp-api.bankopen.co/api";

		public clsLayer(string accesskey, string secretkey, string environmentToUse)
		{
			accessKey = accesskey;
			secretKey = secretkey;
			environment = environmentToUse;

			//url's
			if (environment == "test")
			{
				createTokenURI = baseURISandbox + "/payment_token";
				getpaymentURI = baseURISandbox + "/payment";
			}
			else
			{
				createTokenURI = baseURIUAT + "/payment_token";
				getpaymentURI = baseURIUAT + "/payment";

			}
		}

		public string CreatePaymentToken(clsPaymentData data)
		{
			try
			{
				List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>>();
				headers.Add(new KeyValuePair<string, string>("Authorization", "Bearer " + accessKey + ":" + secretKey));

				string jsonData = JsonConvert.SerializeObject(data);
				var result = clsHTTPRequest.HttpPost(createTokenURI, jsonData, headers);
			
				return result;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public string GetPaymentToken(string paymentTokenId)
		{
			try
			{				
				List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>>();
				headers.Add(new KeyValuePair<string, string>("Authorization", "Bearer "  + accessKey + ":" + secretKey));
				
				var result = clsHTTPRequest.HttpGet(createTokenURI + "/" + paymentTokenId, headers);

				return result;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public string GetPaymentDetails(string paymentId)
		{
			try
			{
				List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>>();
				headers.Add(new KeyValuePair<string, string>("Authorization", "Bearer " + accessKey + ":" + secretKey));

				var result = clsHTTPRequest.HttpGet(getpaymentURI + "/" + paymentId, headers);

				return result;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public string CreateHash(string id, decimal amount, string transactionId)
		{
			string data = accessKey + "|" + amount.ToString("#0.00") + "|" + id + "|" + transactionId;

			return hashHmac(data, secretKey);
		}

		private string hashHmac(string message, string secret)
		{
			Encoding encoding = Encoding.UTF8;
			using (HMACSHA256 hmac = new HMACSHA256(encoding.GetBytes(secret)))
			{
				var msg = encoding.GetBytes(message);
				var hash = hmac.ComputeHash(msg);
				return BitConverter.ToString(hash).ToLower().Replace("-", string.Empty);
			}
		}

	}
}