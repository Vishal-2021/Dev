using Npgsql;
using System;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            string strConnString = "Server=localhost;Port=5432;User Id=postgres;Password=approva1@;Database=Sample";


            string filePath = @"C:\Work-Station\TimeToValueProject\New_5_5\New folder\inforgrc.dbupdatesvc\approvaos.databasescript\postgres\infrastructuredb";


            DirectoryInfo d = new DirectoryInfo(filePath); // File Path
            FileInfo[] Files = d.GetFiles("*.sql"); // Getting Text files

            int executeCount = 0;

            foreach (FileInfo file in Files)
            {

                string NewString = filePath.Replace(@"\", @"\\") + "\\";

                FileInfo files = new FileInfo(NewString + file.Name);

                string sCommandText = files.OpenText().ReadToEnd();


                NpgsqlConnection objConn = new NpgsqlConnection(strConnString);
                objConn.Open();
                NpgsqlCommand sqlCmd = new NpgsqlCommand(sCommandText, objConn);

                try
                {
                    sqlCmd.ExecuteNonQuery();
                    executeCount++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" \t File Name : { file.Name } , \n  Exeption : {ex.Message} \n \n"  );
                }
                finally
                {
                    sqlCmd.Dispose();
                    objConn.Close();
                }
            }

            Console.WriteLine($"No of executed sql scripts : { executeCount }");
        }



    } // class end
}