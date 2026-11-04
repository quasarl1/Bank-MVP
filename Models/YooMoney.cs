using SystemBank.Models;
namespace SystemBank;
public class YooMoneyConfig
{
    public string ClientId { get; set; }
    public string SecretKey { get; set; }
    public string RedirectUri { get; set; }
}
public class YooMoneyPaymentRequest
{
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string TransactionId { get; set; }
}
public class YooMoneyPaymentResponse
{
    public string RequestId { get; set; }
    public string Status { get; set; }
    public string? ConfirmationUrl { get; set; }
    public bool IsSuccess => Status == "success";
}
public class YooMoneyWebhook
{
    public string NotificationType { get; set; }
    public string OperationId { get; set; }
    public decimal Amount { get; set; }
    public string Label { get; set; } 
    public string Sha1Hash { get; set; }
}