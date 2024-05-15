using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace IngresoDinero.Helpers
{
    public static class SqlHelpers
    {
        public static T CheckNull<T>(this object obj)
        {
            return obj == DBNull.Value ? default : (T)Convert.ChangeType(obj, typeof(T));
        }

        /// <summary>
        /// Determina si existe la columna con su nombre
        /// </summary>
        public static bool ColumnExists(this IDataReader reader, string columnName)
        {
            return reader.GetSchemaTable()
                         .Rows
                         .OfType<DataRow>()
                         .Any(row => row["ColumnName"].ToString() == columnName);
        }

        /// <summary>
        /// Mapea un objeto DataReader a una lista
        /// </summary>
        public static List<T> ToList<T>(this IDataReader reader)
        {
            List<T> list = new List<T>();
            while (reader.Read())
            {
                T obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!reader.ColumnExists(prop.Name)) continue;
                    if (Equals(reader[prop.Name], DBNull.Value)) continue;

                    var field = reader[prop.Name];

                    if (prop.PropertyType == typeof(string))
                        prop.SetValue(obj, field.ToString(), null);
                    else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
                        prop.SetValue(obj, int.Parse(field.ToString()), null);
                    else if (prop.PropertyType == typeof(long) || prop.PropertyType == typeof(long?))
                        prop.SetValue(obj, long.Parse(field.ToString()), null);
                    else if (prop.PropertyType == typeof(float) || prop.PropertyType == typeof(float?))
                        prop.SetValue(obj, float.Parse(field.ToString()), null);
                    else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
                        prop.SetValue(obj, double.Parse(field.ToString()), null);
                    else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
                        prop.SetValue(obj, field.ToString() == "1", null);
                    else
                        prop.SetValue(obj, field, null);
                }
                list.Add(obj);
            }
            return list;
        }
    }
}