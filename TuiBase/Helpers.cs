using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TuiBase.Console;

namespace TuiBase
{


    public static class StringAdditions
    {
        public static string PadCenter(this string s, int totalWidth)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            return s.PadLeft((totalWidth + s.Length) / 2).PadRight(totalWidth);

        }
    }


    public class Helpers
    {

        internal static bool IntersectsWith(Coordinates locationA, Coordinates sizeA, Coordinates locationB, Coordinates sizeB)
        {
            return ((((locationA.X < (locationB.X + sizeB.X)) && (locationB.X < (locationA.X + sizeA.X))) && (locationA.Y < (locationB.Y + sizeB.Y))) && (locationB.Y < (locationA.Y + sizeA.Y)));
        }


        public static string DataTableToString(System.Data.DataTable table)
        {
            Dictionary<int, int> dummy;
            return DataTableToString(table, null,out dummy);

        }

        public static string DataTableToString<EntityType>(System.Data.DataTable table, Dictionary<DataRow, EntityType> entityByRow, out Dictionary<int, EntityType> entityByLineIndex)
        {
            if (entityByRow != null)
                entityByLineIndex = new Dictionary<int, EntityType>();
            else
                entityByLineIndex = null;

            StringBuilder sb = new StringBuilder();

            int[] columWidths = new int[table.Columns.Count];
            bool[] rigthtAlign = new bool[table.Columns.Count];

            bool checkForHLine = table.Columns.Contains("hl");
            int startColumn = checkForHLine ? 1 : 0;

            for (int i = startColumn; i < table.Columns.Count; i++)
            {
                columWidths[i] = table.Columns[i].ColumnName.ToString().Length + 2;
                rigthtAlign[i] = table.Columns[i].ExtendedProperties.ContainsKey("r");
            }

            foreach (System.Data.DataRow row in table.Rows)
            {

                object[] items = row.ItemArray;
                for (int i = startColumn; i < items.Length; i++)
                {
                    if (items[i] != null)
                    {
                        int lenght = (items[i].ToString()).Length + 2;
                        if (lenght > columWidths[i])
                            columWidths[i] = lenght;
                    }
                }
            }



            sb.Append("|");

            for (int i = startColumn; i < table.Columns.Count; i++)
            {
                sb.Append(table.Columns[i].ColumnName.PadCenter(columWidths[i]) + "|");
            }
            sb.AppendLine();

            sb.Append("+");
            for (int i = startColumn; i < columWidths.Length; i++)
            {
                sb.Append(new string('=', columWidths[i]));
                sb.Append("+");
            }
            sb.AppendLine();

            int lineIndex = 2;


            foreach (System.Data.DataRow row in table.Rows)
            {
                EntityType entity;
                if (entityByRow != null && entityByRow.TryGetValue(row, out entity))
                    entityByLineIndex.Add(lineIndex++, entity);


                object[] items = row.ItemArray;

                if (checkForHLine && items[0] != null && items[0] != DBNull.Value && (bool)items[0])
                {
                    sb.Append("+");
                    for (int i = startColumn; i < columWidths.Length; i++)
                    {
                        sb.Append(new string('-', columWidths[i]));
                        sb.Append("+");
                    }
                    sb.AppendLine();
                }

                sb.Append("|");
                for (int i = startColumn; i < items.Length; i++)
                {
                    if (items[i] != null && items[i] != DBNull.Value)
                    {
                        sb.Append(rigthtAlign[i] ? ((string)items[i]).PadLeft(columWidths[i] - 1) + " |" : " " + ((string)items[i]).PadRight(columWidths[i] - 1) + "|");
                    }
                    else
                        sb.Append(new string(' ', columWidths[i]) + "|");
                }
                sb.AppendLine();


            }

            return sb.ToString();

        }


    }
}
