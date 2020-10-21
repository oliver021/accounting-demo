namespace OliWorkshop.AccountingSys.Data
{
    public class CategoryExpenseGroup
    {
        public uint Id { get; set; }
        public CountableGroup CountableGroup { get; set; }
        public uint CountableGroupId { get; set; }

        public ExpenseCategory ExpenseCategory { get; set; }
        public uint ExpenseCategoryId { get; set; }
    }
}