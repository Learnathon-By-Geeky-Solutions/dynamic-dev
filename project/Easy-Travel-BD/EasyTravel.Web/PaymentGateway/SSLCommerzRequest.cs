namespace EasyTravel.Web
{
    public class SslCommerzRequest
    {
        public required string StoreId { get; set; }
        public required string StorePassword { get; set; }
        public required int TotalAmount { get; set; }
        public string Currency { get; set; } = "BDT";
        public required string TranId { get; set; }
        public required string SuccessUrl { get; set; }
        public required string FailUrl { get; set; }
        public required string CancelUrl { get; set; }
        public required string CusName { get; set; }
        public required string CusEmail { get; set; }
        public required string CusPhone { get; set; }
        public required string CusAdd1 { get; set; }
        public required string CusCity { get; set; }
        public string CusCountry { get; set; } = "Bangladesh";
    }
}
