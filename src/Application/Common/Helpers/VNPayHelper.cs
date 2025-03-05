using System.Globalization;
using System.Net;
using System.Net.Sockets;
using Application.Common.Models.Dtos;
using Microsoft.Extensions.Configuration;

namespace Application.Common.Helpers;

public class VNPayHelper
{
    public static VNPayConfig GetConfigData()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();

        var version = config["VNPay:Version"];
        var tmnCode = config["VNPay:TmnCode"];
        var currencyCode = config["VNPay:CurrencyCode"];
        var locale = config["VNPay:Locale"];
        var returnUrl = config["VNPay:ReturnUrl"];
        var baseUrl = config["VNPay:PaymentUrl"];
        var secretKey = config["VNPay:HashSecret"];

        return new VNPayConfig()
        {
            PaymentUrl = baseUrl,
            ReturnUrl = returnUrl,
            TmnCode = tmnCode,
            HashSecret = secretKey,
            Version = version,
            CurrencyCode = currencyCode,
            Locale = locale,
        };
    }
}

public class VNPayCompare : IComparer<string>
{
    public int Compare(string x, string y)
    {
        if (x == y) return 0;
        if (x == null) return -1;
        if (y == null) return 1;
        var vnpCompare = CompareInfo.GetCompareInfo("en-US");
        return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
    }
}

public class IPAddressHelper
{
    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
}







