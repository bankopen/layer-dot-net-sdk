using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Open.layer
{

	public class clsPaymentData
	{
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public decimal amount { get; set; }

		[DefaultValue("")]
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string currency { get; set; }

		[DefaultValue("")]
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string name { get; set; }

		[DefaultValue("")]
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string email_id { get; set; }

		[DefaultValue("")]
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string contact_number { get; set; }

		[DefaultValue("")]
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string mtx { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string udf { get; set; }
	}
}