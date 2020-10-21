namespace OliWorkshop.AccountingSys.Data
{
    public  class CategoryEarnGroup
    {
        public uint Id { get; set; }
        public CountableGroup CountableGroup { get; set; }
        public uint CountableGroupId { get; set; }

        public EarnCategory EarnCategory { get; set; }
        public uint EarnCategoryId { get; set; }
    }
}