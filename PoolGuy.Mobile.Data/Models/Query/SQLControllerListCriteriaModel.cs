using System.Collections.Generic;

namespace PoolGuy.Mobile.Data.Models.Query
{
    public class SQLControllerListCriteriaModel
    {
        public string View { get; set; }

        private List<string> _fieldsToInclude = new List<string>();
        public List<string> FieldsToInclude
        {
            get { return _fieldsToInclude; }
            set { _fieldsToInclude = value; }
        }

        private List<SQLControllerListFilterField> _filters = new List<SQLControllerListFilterField>();
        public List<SQLControllerListFilterField> Filter
        {
            get { return _filters; }
            set { _filters = value; }
        }

        private List<SQLControllerListSortField> _sort = new List<SQLControllerListSortField>();
        public List<SQLControllerListSortField> Sort
        {
            get { return _sort; }
            set { _sort = value; }
        }

        private List<SQLControllerListAggregateField> _aggregateFields = new List<SQLControllerListAggregateField>();
        public List<SQLControllerListAggregateField> AggregateFields
        {
            get { return _aggregateFields; }
            set { _aggregateFields = value; }
        }

        private int _topNRecords = 0;
        public int TopNRecords
        {
            get { return _topNRecords; }
            set { _topNRecords = value; }
        }

        private int _topRow = 0;
        public int TopRow
        {
            get { return _topRow; }
            set { _topRow = value; }
        }

        private int _rowsToReturn = 0;
        public int RowsToReturn
        {
            get { return _rowsToReturn; }
            set { _rowsToReturn = value; }
        }

        public bool Distinct { get; set; }

        private bool _disableBusUnitPermissions = false;

        public bool DisableBusUnitPermissions
        {
            get { return _disableBusUnitPermissions; }
            set { _disableBusUnitPermissions = value; }
        }

        /// <summary>
        /// In some instances we don't want to include sorting, for instance when using the query for counting records
        /// </summary>
        public bool SupportSorting { get; set; } = true;

    }
}