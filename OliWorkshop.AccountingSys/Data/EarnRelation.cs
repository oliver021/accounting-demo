using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OliWorkshop.AccountingSys.Data
{
    public class EarnRelation
    {
        public uint Id { get; set; }

        public string Note { get; set; }

        public uint ExpenseCategoryId { get; set; }

        public ExpenseCategory ExpenseCategory { get; set; }

        public uint EarnCategoryId { get; set; }

        public EarnCategory EarnCategory { get; set; }
    }
}
