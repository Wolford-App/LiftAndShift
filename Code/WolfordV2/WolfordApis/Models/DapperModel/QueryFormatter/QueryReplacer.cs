using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WolfordApis.Models.DapperModel.QueryFormatter
{
    public class QueryReplacer
    {
        public string ReplaceComments(Hashtable commentPatterns, string query)
        {
            string newQuery = query;
            foreach (string key in commentPatterns.Keys)
            {
                newQuery = newQuery.Replace($"/**{key}**/", commentPatterns[key].ToString());
            }
            return newQuery;
        }

        public string ConvertValues(IEnumerable<string> toConvert, string pattern = null, string replacement = "','", string start = null, string end = null)
        {
            if (pattern != null)
            {
                //return $"('{Regex.Replace(string.Join(",", toConvert), pattern, replacement)}')";
                var matches = Regex.Matches(string.Join(",", toConvert), pattern);
                var result = new List<string>();
                foreach (Match match in matches)
                {
                    if (match.Groups[1].Success)
                    {
                        result.Add(match.Groups[1].Value);
                    }
                    else
                    {
                        result.Add("'" + match.Groups[2].Value.Replace("'", "''") + "'");
                    }
                }
                return start + string.Join(",", result) + end;
            }
            else
                return start + String.Join(", ", toConvert).Trim() + end;
        }


        public string CleanQuery(string sqlQuery, IEnumerable<string> columns, IEnumerable<string> values)
        {
            string regrexPattern = @"###(\d+)###|([^,]+)";
            Hashtable pattern = new Hashtable() {
                {"columns",ConvertValues(columns,null, "','","(",")")},
                {"values",ConvertValues(values,regrexPattern, "','","(",")")}
            };
            return ReplaceComments(pattern, sqlQuery);

        }
        public string CleanQuery(string sqlQuery, IEnumerable<string> columns)
        {
            Hashtable pattern = new Hashtable() {
                {"columns",ConvertValues(columns,null, "','","(",")")}
            };
            return ReplaceComments(pattern, sqlQuery);

        }
        public string CleanQuery(string sqlQuery, IEnumerable<string> columnsEqValues, bool colAndVal)
        {
            Hashtable pattern = new Hashtable() {
                {"columns=values",ConvertValues(columnsEqValues)}
            };
            return ReplaceComments(pattern, sqlQuery);

        }

    }
}
