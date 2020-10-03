namespace PoolGuy.Mobile.Data.Models.Query
{
    public class SQLControllerListAggregateField
    {
        public enum AggregateEnum
        {
            Avg,
            Count,
            Max,
            Min,
            Sum
        }

        public string FieldName { get; set; }
        public AggregateEnum AggregateType { get; set; }
    }
}