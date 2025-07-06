using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Monopolizers.Common.DTO;
using System.Net.Http.Headers;

public class PayOSService
{
    private readonly HttpClient _httpClient;
    private readonly PayOSConfig _config;

    public PayOSService(HttpClient httpClient, IOptions<PayOSConfig> config)
    {
        _httpClient = httpClient;
        _config = config.Value;
    }

    public async Task<string> CreatePaymentAsync(decimal amount, string orderIdIgnored, string description)
    {
        var returnUrl = "https://monopolizers.vercel.app/wallet";
        var cancelUrl = "https://monopolizers.vercel.app/wallet?cancel=true";

        var orderCode = new Random().Next(100000, 999999);
        var amountMinor = (int)(amount); 

        if (description.Length > 25)
        {
            description = description.Substring(0, 25);
        }

        var rawData = $"amount={amountMinor}&cancelUrl={cancelUrl}&description={description}&orderCode={orderCode}&returnUrl={returnUrl}";
        var signature = GenerateSignature(rawData);

        var payload = new
        {
            orderCode = orderCode,
            amount = amountMinor,
            description = description,
            cancelUrl = cancelUrl,
            returnUrl = returnUrl,
            signature = signature
        };

        var json = JsonSerializer.Serialize(payload);
        var request = new HttpRequestMessage(HttpMethod.Post, "https://api-merchant.payos.vn/v2/payment-requests");

        request.Headers.Add("x-client-id", _config.ClientId);
        request.Headers.Add("x-api-key", _config.ApiKey);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"❌ PayOS error: {response.StatusCode} – {responseContent}");
        }

        var jsonDoc = JsonDocument.Parse(responseContent);
        if (jsonDoc.RootElement.TryGetProperty("data", out var dataElement) &&
            dataElement.ValueKind == JsonValueKind.Object &&
            dataElement.TryGetProperty("checkoutUrl", out var checkoutUrlElement))
        {
            return checkoutUrlElement.GetString();
        }

        throw new Exception($"❌ Không tìm thấy 'checkoutUrl' hợp lệ trong phản hồi từ PayOS. Response: {responseContent}");
    }

    private string GenerateSignature(string rawData)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_config.ChecksumKey));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}
