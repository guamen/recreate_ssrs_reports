using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecreateReports
{
    class Program
    {
        static void Main(string[] args)
        {
            //Coloca aqui tu string de coneccion a SQL Server.
            var connstr = @"server=xxxx;database=xxx;uid=xxxx;pwd=xxxxx";

            //No es necesario cambiar estas variables
            var query = "SELECT C.NAME, CONVERT(NVARCHAR(MAX),CONVERT(XML,CONVERT(VARBINARY(MAX),C.CONTENT))) AS REPORTXML FROM  REPORTSERVER.DBO.CATALOG C WHERE  C.CONTENT IS NOT NULL AND C.TYPE = 2";
            var da = new SqlDataAdapter(query, connstr);
            var dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow item in dt.Rows)
            {
                //Esta es la ruta donde quieres que se creen los archivos .rldc
                var path = @"C:\Users\Guaroa Mendez\Documents\Visual Studio 2017\Projects\RecreateReports\RecreateReports\REPORTS\";

                path += item["NAME"].ToString() +".rdl";

                // Delete the file if it exists.
                if (File.Exists(path))
                {
                    // Note that no lock is put on the
                    // file and the possibility exists
                    // that another process could do
                    // something with it between
                    // the calls to Exists and Delete.
                    File.Delete(path);
                }

                using (FileStream fs = File.Create(path))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(item["REPORTXML"].ToString());

                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }

                Console.WriteLine("Proceso concluido.");

            }
        }
    }
}
