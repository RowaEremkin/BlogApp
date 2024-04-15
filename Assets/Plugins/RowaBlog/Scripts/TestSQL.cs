using System.Data;
using UnityEngine;

namespace Rowa.Blog
{
    public class TestSql : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            SqlAccess sql = new SqlAccess();
            string[] items = { "authorId", "authorNameFirst", "authorNameLast"};//Change the element, corresponding to your own table
            string[] col = { };
            string[] op = { };
            string[] val = { };
            DataSet ds = sql.SelectWhere("authors", items, col, op, val);//Read the table, here change the table name to the table you need to query

            if (ds != null)
            {
                DataTable table = ds.Tables[0];
                foreach (DataRow row in table.Rows)
                {
                    string str = "";
                    foreach (DataColumn column in table.Columns)
                        str += row[column] + " ";
                    Debug.Log(str);
                }
            }
        }
    }
}