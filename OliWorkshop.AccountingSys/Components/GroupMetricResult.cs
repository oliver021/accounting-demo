using System;

namespace OliWorkshop.AccountingSys.Components
{
    public class GroupMetricResult
    {
        public uint Id { get; }
        public string Name { get; }
        public int EarnCategories { get; }
        public int ExpenseCategories { get; }
        public int EarnTransactionTotal { get; }
        public int ExpenseTransactionTotal { get; }
        public decimal EarnTotal { get; }
        public decimal ExpenseTotal { get; }

        public GroupMetricResult(uint id, string name, int earnCategories, int expenseCategories, int earnTransactionTotal, int expenseTransactionTotal, decimal earnTotal, decimal expenseTotal)
        {
            Id = id;
            Name = name;
            EarnCategories = earnCategories;
            ExpenseCategories = expenseCategories;
            EarnTransactionTotal = earnTransactionTotal;
            ExpenseTransactionTotal = expenseTransactionTotal;
            EarnTotal = earnTotal;
            ExpenseTotal = expenseTotal;
        }

        public override bool Equals(object obj)
        {
            return obj is GroupMetricResult other &&
                   Id == other.Id &&
                   Name == other.Name &&
                   EarnCategories == other.EarnCategories &&
                   ExpenseCategories == other.ExpenseCategories &&
                   EarnTransactionTotal == other.EarnTransactionTotal &&
                   ExpenseTransactionTotal == other.ExpenseTransactionTotal &&
                   EarnTotal == other.EarnTotal &&
                   ExpenseTotal == other.ExpenseTotal;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, EarnCategories, ExpenseCategories, EarnTransactionTotal, ExpenseTransactionTotal, EarnTotal, ExpenseTotal);
        }
    }
}
