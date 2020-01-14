using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Common;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Framework.Extensions
{
    public static class TableExtensions
    {
        public static T DeserializeTableAsObject<T>(this Table table)
        {
            var tableDic = table.CreateSet<FieldValueDto>().ToDictionary(row => row.Field, row => row.Value);
            var jObject = JsonConvert.SerializeObject(tableDic);
            return JsonConvert.DeserializeObject<T>(jObject);
        }

        public static List<T> GetFirstColumnValues<T>(this Table table)
        {
            return table.Rows.Select(row => Convert.ChangeType(row[0], typeof(T))).Cast<T>().ToList();
        }
    }
}
