namespace WebLabBudgetTool.DataTransferObjects
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? CategoryId { get; set; }
        public string Date { get; set; }
        public double Amount { get; set; }
        public bool IsCleared { get; set; }
        public PaymentType Type { get; set; }
        public string Note { get; set; }
    }
}
