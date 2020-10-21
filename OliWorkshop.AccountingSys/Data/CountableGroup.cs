using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OliWorkshop.AccountingSys.Data
{
    public class CountableGroup
    {
        public uint Id { get; set; }

        public string Name { get; set; }

        public User User { get; set; }

        public uint EarnCategoryId { get; set; }

        public List<CategoryEarnGroup> CategoryEarnGroups { get; set; }
        public List<CategoryExpenseGroup> CategoryExpenseGroups { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
