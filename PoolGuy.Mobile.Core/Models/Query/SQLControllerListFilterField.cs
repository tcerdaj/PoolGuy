namespace PoolGuy.Mobile.Data.Models.Query
{
    public class SQLControllerListFilterField
    {
        /// <summary>
        /// Compare method.  See enumeration code for documentation on appropriate field types
        /// </summary>
        public enum CompareMethodEnum
        {
            Normal,             // All field types
            Partial,            // String
            ContainsValue,      // String
            GreaterThan,        // String, FieldName value, int, decimal
            LessThan,           // String, FieldName value, int, decimal
            ContainsWord,       // Uses the CONTAINS predicate
            FreeText,           // Uses the FREETEXT predicate
            NotEqual,           // Compare using NOT logic instead of equals
            GreaterThanOrEqual,
            LessThanOrEqual
        }

        public enum DateKindEnum
        {
            Localized,
            UTC
        }

        /// <summary>
        /// If we specify a FieldName type it will directly use the field name from ValueLBound for the value comparison
        /// </summary>
        public enum eFieldValueType
        {
            Normal,
            FieldName
        }

        public string FieldName { get; set; }

        public eFieldValueType FieldValueType { get; set; }

        /// <summary>
        /// Lower boundary of filter.  Specify string, integer, decimal, date
        /// </summary>
        public string ValueLBound { get; set; }

        /// <summary>
        /// Upper boundary of filter
        /// </summary>
        public string ValueUBound { get; set; }

        /// <summary>
        /// Specifies a filter group.  Fields within a group are logically handled with an "OR" i.e. Customer = '0000001' AND (Location = '00001' OR Location = '00002').
        /// </summary>
        public int ORGroup { get; set; }

        /// <summary>
        /// If true, we will partially compare a string ('SM' would return 'Smith' records)
        /// </summary>
        public CompareMethodEnum CompareMethod { get; set; }

        private DateKindEnum dateKind = DateKindEnum.Localized;

        public DateKindEnum DateKind
        {
            get { return dateKind; }
            set { dateKind = value; }
        }
    }
}