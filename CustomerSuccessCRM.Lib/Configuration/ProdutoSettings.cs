namespace CustomerSuccessCRM.Lib.Configuration
{
    public class ProdutoSettings
    {
        public bool EnableAutomaticPricing { get; set; } = false;
        public decimal MaxDiscountPercentage { get; set; } = 30;
        public bool RequireApprovalForDiscounts { get; set; } = true;
        public string[] ProdutoAdministrators { get; set; } = Array.Empty<string>();
    }
} 