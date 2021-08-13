using System.Collections;

namespace SQLite4Unity3d
{
    public static class SQLExtensions
    {
        public static int InsertOrReplaceAll(this SQLiteConnection conn, IEnumerable objects)
        {
            var c = 0;
            conn.RunInTransaction(() => {
                foreach (var r in objects)
                {
                    c += conn.Insert(r, "OR REPLACE", r.GetType());
                }
            });
            return c;
        }

        public static uint GetLastId(this SQLiteConnection conn, string tableName, string key)
        {
            uint res = 0;
            conn.RunInTransaction(() => {
                res = conn.ExecuteScalar<uint>($"select coalesce(max({key}), 0) from {tableName}");
            });
            return res;
        }
    }
}
