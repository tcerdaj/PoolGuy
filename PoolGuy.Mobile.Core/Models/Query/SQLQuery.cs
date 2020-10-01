using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PoolGuy.Mobile.Data.Models.Query
{
    public class SQLQuery
    {
        public static string BuildListQuery<T>(SQLControllerListCriteriaModel criteria)
        {
            StringBuilder sb = new StringBuilder();
            // Make sure all aggregate fields are added to the return fields so they can be set in the model
            foreach (var fld in criteria.AggregateFields)
            {
                criteria.FieldsToInclude.Add(fld.FieldName);
            }
            // ......................................
            // Build the "SELECT" clause
            // ......................................

            StringBuilder sbFields = new StringBuilder();
            if (criteria.FieldsToInclude.Count == 0)
            {
                sbFields.Append("*");
            }
            else
            {
                foreach (string fld in criteria.FieldsToInclude)
                {
                    if (sbFields.Length > 0)
                    {
                        sbFields.Append(", ");
                    }

                    sbFields.Append("[" + fld + "]");
                }
            }

            foreach (var fld in criteria.AggregateFields)
            {
                if (sbFields.Length > 0)
                {
                    sbFields.Append(", ");
                }
                switch (fld.AggregateType)
                {
                    case SQLControllerListAggregateField.AggregateEnum.Avg:
                        sbFields.Append("AVG([" + fld.FieldName + "]) AS " + fld.FieldName);
                        break;
                    case SQLControllerListAggregateField.AggregateEnum.Count:
                        sbFields.Append("COUNT(*) AS " + fld.FieldName);
                        break;
                    case SQLControllerListAggregateField.AggregateEnum.Max:
                        sbFields.Append("MAX([" + fld.FieldName + "]) AS " + fld.FieldName);
                        break;
                    case SQLControllerListAggregateField.AggregateEnum.Min:
                        sbFields.Append("MIN([" + fld.FieldName + "]) AS " + fld.FieldName);
                        break;
                    case SQLControllerListAggregateField.AggregateEnum.Sum:
                        sbFields.Append("SUM([" + fld.FieldName + "]) AS " + fld.FieldName);
                        break;
                }
            }

            sb.Append("SELECT ");
            sb.Append(sbFields.ToString() + " FROM [" + criteria.View + "]");

            // ......................................
            // Build the "WHERE" clause
            // ......................................
            StringBuilder sbFilter = new StringBuilder();
            int currentGroup = 0;

            foreach (var filter in criteria.Filter)
            {
                // If we are not currently in a group and this filter has a group specified, begin a group
                if (currentGroup == 0 && filter.ORGroup != 0)
                {
                    if (sbFilter.Length > 0)
                    {
                        sbFilter.Append(" AND ");
                    }

                    currentGroup = filter.ORGroup;
                    sbFilter.Append("(");
                }
                // If we are currently in a group and the group has changed, end the group
                else if (currentGroup != 0 && filter.ORGroup != currentGroup)
                {
                    currentGroup = filter.ORGroup;
                    sbFilter.Append(")");

                    // Add an "AND"
                    sbFilter.Append(" AND ");

                    // If we are re-entering a group, begin the new group
                    if (currentGroup != 0)
                    {
                        sbFilter.Append("(");
                    }
                }
                // We are in a group and the group hasn't changed, add "OR" between them
                else if (currentGroup != 0 && filter.ORGroup == currentGroup)
                {
                    if (sbFilter.Length > 0)
                    {
                        sbFilter.Append(" OR ");
                    }
                }
                // We aren't in a group.  Add an "AND"
                else
                {
                    if (sbFilter.Length > 0)
                    {
                        sbFilter.Append(" AND ");
                    }
                }

                Type fieldType = GetFieldType<T>(filter.FieldName.Replace(" ", ""));

                if (fieldType == null)
                {
                    throw new Exception("Field " + filter.FieldName + " could not be found in the model " + fieldType.Name);
                }

                if (fieldType == typeof(string))
                {
                    if (string.IsNullOrEmpty(filter.ValueUBound))
                    {
                        string szLBound = filter.ValueLBound;

                        if (szLBound.Equals("NULL"))
                        {
                            sbFilter.Append("[" + filter.FieldName + "] IS NULL");
                        }
                        else
                        {
                            switch (filter.CompareMethod)
                            {
                                case SQLControllerListFilterField.CompareMethodEnum.Normal:
                                    sbFilter.Append("[" + filter.FieldName + "] = '" + SQLEncode(szLBound) + "'");
                                    break;
                                case SQLControllerListFilterField.CompareMethodEnum.Partial:
                                    sbFilter.Append("[" + filter.FieldName + "] LIKE '" + SQLEncodeLike(szLBound) + "%'");
                                    break;
                                case SQLControllerListFilterField.CompareMethodEnum.ContainsValue:
                                case SQLControllerListFilterField.CompareMethodEnum.ContainsWord:
                                case SQLControllerListFilterField.CompareMethodEnum.FreeText:
                                    sbFilter.Append("[" + filter.FieldName + "] LIKE '%" + SQLEncodeLike(szLBound) + "%'");
                                    break;
                            }
                        }
                    }
                    else
                    {
                        string szLBound = filter.ValueLBound;
                        string szUBound = filter.ValueUBound + "zz";

                        sbFilter.Append("[" + filter.FieldName + "] BETWEEN '" + SQLEncode(szLBound) + "' AND '" + SQLEncode(szUBound) + "'");
                    }
                }
                else if (fieldType == typeof(Guid))
                {
                    Guid value = Guid.Parse(filter.ValueLBound);

                    sbFilter.Append("[" + filter.FieldName + "] = '" + SQLEncode(value.ToString()) + "'");
                }
                else if (fieldType == typeof(bool))
                {
                    string szField = filter.FieldName;
                    bool valueBool = Boolean.Parse(filter.ValueLBound);

                    int sqlValue = valueBool == true ? 1 : 0;

                    sbFilter.Append("[" + szField + "] = " + sqlValue.ToString());
                }
                else if (fieldType == typeof(DateTime) || fieldType == typeof(DateTime?))
                {
                    DateTime dtLBound = DateTime.Parse(filter.ValueLBound).ToUniversalTime();
                    DateTime dtUBound;

                    if (string.IsNullOrEmpty(filter.ValueUBound))
                    {
                        dtUBound = dtLBound.Date.AddDays(1).AddMinutes(-1).ToUniversalTime();
                    }
                    else
                    {
                        dtUBound = DateTime.Parse(filter.ValueUBound).ToUniversalTime();

                        if (dtUBound.Hour == 0 && dtUBound.Minute == 0)
                        {
                            dtUBound = dtUBound.Date.AddDays(1).AddMinutes(-1).ToUniversalTime();
                        }
                    }

                    sbFilter.Append("[" + filter.FieldName + "] BETWEEN '" + dtLBound.Ticks + "' AND '" + dtUBound.Ticks + "'");

                }
                else if (fieldType == typeof(decimal))
                {
                    if (string.IsNullOrEmpty(filter.ValueUBound))
                    {
                        decimal szLBound = decimal.Parse(filter.ValueLBound);
                        sbFilter.Append("[" + filter.FieldName + "] = " + szLBound);
                    }
                    else
                    {
                        decimal szLBound = decimal.Parse(filter.ValueLBound);
                        decimal szUBound = decimal.Parse(filter.ValueUBound);

                        sbFilter.Append("[" + filter.FieldName + "] BETWEEN " + szLBound + " AND " + szUBound);
                    }
                }
                else if (fieldType == typeof(int))
                {
                    if (string.IsNullOrEmpty(filter.ValueUBound))
                    {
                        int szLBound = int.Parse(filter.ValueLBound);

                        sbFilter.Append("[" + filter.FieldName + "] = " + szLBound);
                    }
                    else
                    {
                        int szLBound = int.Parse(filter.ValueLBound);
                        int szUBound = int.Parse(filter.ValueUBound);

                        sbFilter.Append("[" + filter.FieldName + "] BETWEEN " + szLBound + " AND " + szUBound);
                    }
                }
                else if (fieldType.GetTypeInfo().BaseType == typeof(System.Enum))
                {
                    // If fieldtype enum: Lower Bound Value is used for equal to and upper bound value is used for not equal to
                    if (!string.IsNullOrEmpty(filter.ValueLBound))
                    {
                        int equalValue = int.Parse(filter.ValueLBound);
                        sbFilter.Append("[" + filter.FieldName + "] = '" + SQLEncode(equalValue.ToString()) + "'");
                    }
                    else if (!string.IsNullOrEmpty(filter.ValueUBound))
                    {
                        int notEqualValue = int.Parse(filter.ValueUBound);
                        sbFilter.Append("[" + filter.FieldName + "] != '" + SQLEncode(notEqualValue.ToString()) + "'");
                    }
                }
            }

            // If we are currently in a group, end it
            if (currentGroup != 0)
            {
                sbFilter.Append(")");
            }

            // ......................................
            // Build the "Group by" clause
            // IF we have aggregate fields AND select fields
            // ......................................
            StringBuilder sbGroupBy = new StringBuilder();

            if (criteria.FieldsToInclude.Any() && criteria.AggregateFields.Any())
            {
                foreach (var fld in criteria.FieldsToInclude)
                {
                    if (sbGroupBy.Length > 0)
                    {
                        sbGroupBy.Append(", ");
                    }

                    if (fld.Equals("Active"))
                    {
                        sbGroupBy.Append("[Inactive]");
                    }
                    else
                    {
                        sbGroupBy.Append("[" + fld + "]");
                    }
                }
            }

            // ......................................
            // Build the "Order" clause
            // ......................................
            StringBuilder sbOrder = new StringBuilder();

            foreach (var order in criteria.Sort)
            {
                if (sbOrder.Length > 0)
                {
                    sbOrder.Append(", ");
                }

                sbOrder.Append("[" + order.FieldName + "]");

                Type fieldType = GetFieldType<T>(order.FieldName.Replace(" ", ""));

                if (fieldType == null)
                {
                    throw new Exception("Field " + order.FieldName + " could not be found in the model " + fieldType.Name);
                }

                if (fieldType == typeof(string))
                {
                    sbOrder.Append(" COLLATE NOCASE");
                }

                if (order.Descending)
                    sbOrder.Append(" DESC");
            }

            if (sbFilter.Length > 0)
            {
                sb.Append(" WHERE " + sbFilter.ToString());
            }

            if (sbGroupBy.Length > 0)
                sb.Append(" GROUP BY " + sbGroupBy.ToString());

            if (sbOrder.Length > 0)
                sb.Append(" ORDER BY " + sbOrder.ToString());

            if (criteria.RowsToReturn != 0)
            {
                // SELECT * FROM `table` LIMIT [row to start on], [how many to include] .
                sb.Append(" LIMIT " + criteria.TopRow.ToString() + ", " + criteria.RowsToReturn.ToString());
            }
            else if (criteria.TopNRecords != 0)
            {
                sb.Append(" LIMIT " + criteria.TopNRecords.ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Uses reflection to return the model's field type from a field name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        private static Type GetFieldType<T>(string FieldName)
        {
            Type targetType = typeof(T);

            var myPropInfo = targetType.GetRuntimeProperties();

            foreach (var propInfo in myPropInfo)
            {
                string sz = propInfo.Name;

                if (FieldName.Equals(sz))
                {
                    return propInfo.PropertyType;
                }
            }

            return null;
        }

        /// <summary>
        /// Encode the SQL string for special characters such as ' and %
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        private static string SQLEncode(string Value)
        {
            string szReturn = Value.Replace("'", "''");
            return szReturn;
        }

        private static string SQLEncodeLike(string Value)
        {
            string szReturn = Value.Replace("'", "''").Replace("%", "[%]");
            return szReturn;
        }
    }
}