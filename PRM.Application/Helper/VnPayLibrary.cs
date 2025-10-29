using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Helper
{
	public class VnPayLibrary
	{
		private readonly SortedList<string, string> _requestData = new SortedList<string, string>(new VnPayCompare());
		private readonly SortedList<string, string> _responseData = new SortedList<string, string>(new VnPayCompare());

		public void AddRequestData(string key, string value)
		{
			if (!string.IsNullOrEmpty(value))
				_requestData[key] = value;
		}

		public void AddResponseData(string key, string value)
		{
			if (!string.IsNullOrEmpty(value))
				_responseData[key] = value;
		}

		public string GetResponseData(string key)
		{
			return _responseData.TryGetValue(key, out string value) ? value : string.Empty;
		}

		public string CreateRequestUrl(string baseUrl, string hashSecret)
		{
			var data = new StringBuilder();

			foreach (var kv in _requestData)
			{
				if (!string.IsNullOrEmpty(kv.Value))
					data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
			}

			var queryString = data.ToString();
			var signData = queryString.TrimEnd('&');
			var vnp_SecureHash = Utils.HmacSHA512(hashSecret, signData);

			return $"{baseUrl}?{queryString}vnp_SecureHash={vnp_SecureHash}";
		}

		public bool ValidateSignature(string inputHash, string hashSecret)
		{
			if (_responseData.ContainsKey("vnp_SecureHash")) _responseData.Remove("vnp_SecureHash");
			if (_responseData.ContainsKey("vnp_SecureHashType")) _responseData.Remove("vnp_SecureHashType");

			var rawData = new StringBuilder();
			foreach (var kv in _responseData)
			{
				if (!string.IsNullOrEmpty(kv.Value))
					rawData.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
			}

			if (rawData.Length > 0) rawData.Remove(rawData.Length - 1, 1);
			var myChecksum = Utils.HmacSHA512(hashSecret, rawData.ToString());

			return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
		}

		public string GetIpAddress(HttpContext context)
		{
			var ipAddress = context.Connection.RemoteIpAddress;

			if (ipAddress == null)
				return "127.0.0.1";

			// Nếu là IPv6 thì convert sang IPv4
			if (ipAddress.IsIPv4MappedToIPv6)
				ipAddress = ipAddress.MapToIPv4();

			// Nếu là ::1 thì cũng trả về 127.0.0.1
			if (ipAddress.ToString() == "::1")
				return "127.0.0.1";

			return ipAddress.ToString();
		}

		public class VnPayCompare : IComparer<string>
		{
			public int Compare(string x, string y)
			{
				if (x == y) return 0;
				if (x == null) return -1;
				if (y == null) return 1;
				var comparer = CompareInfo.GetCompareInfo("en-US");
				return comparer.Compare(x, y, CompareOptions.Ordinal);
			}
		}
	}
}
