namespace CSRFundDetails.Models
{
    public class CombinedCsrFundsViewModel
    {
        public CsrFund CsrFund { get; set; }  // For excess CSR Fund data
        public DeficitCsrFund DeficitCsrFund { get; set; }  // For deficit CSR Fund data
    }
}