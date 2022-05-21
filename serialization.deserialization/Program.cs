
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;

namespace serialization.deserilization
{
    public class BlogSites
    {
        public string Name { get; set; }
        public string Description { get; set; }


        public static string EncodeServerName(string serverName)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(serverName));
        }

        public static string DecodeServerName(string encodedServername)
        {
            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedServername));
        }
    }


    class Program
    {

        static void Main(string[] args)
        {


            string strConnString = "Server=localhost;Port=5432;User Id=postgres;Password=approva1@;Database=Sample";


            string filePath = @"C:\Work-Station\Learning\FileExecuteFolder";


            DirectoryInfo d = new DirectoryInfo(filePath); // File Path
            FileInfo[] Files = d.GetFiles("*.txt"); // Getting Text files

            int executeCount = 0;
             bool StormOut = true;
            foreach (FileInfo file in Files)
            {

                string NewString = filePath.Replace(@"\", @"\\") + "\\";

                FileInfo files = new FileInfo(NewString + file.Name);

                string sCommandText = files.OpenText().ReadToEnd();

                var bsObj2s = (JObject)JsonConvert.DeserializeObject(sCommandText);

             

                var result = bsObj2s["id"].Value<string>();


                     
                       





                sCommandText = @"insert into approvaos_mgmt_tenantinfo
 (tenantid,logicalid,displayname,description,productname,productshortname,productversion,dbversion,ismingleenabled,statusid,currentstatusreason,creationdate,modificationdate,idmtemplateversion, appFarmId)" +
" values (@TenantID,@LogicalID,@TenantDisplayName,@TenantDescription,@ProductName,@ProductShortName,@ProductVersion,'0.0.0.0',@IsMingleEnabled,@StatusID,'Newly Added Tenant',now(),now(),'0.0.0.0', @appFarmId);" + Environment.NewLine;

                       




                NpgsqlConnection objConn = new NpgsqlConnection(strConnString);







                objConn.Open();
                NpgsqlCommand sqlCmd = new NpgsqlCommand(sCommandText, objConn);


             
                sqlCmd.Parameters.AddWithValue("@TenantID", bsObj2s["id"].Value<string>());
                sqlCmd.Parameters.AddWithValue("@LogicalID", bsObj2s["logicalId"].Value<string>());
                sqlCmd.Parameters.AddWithValue("@TenantDisplayName", bsObj2s["displayName"].Value<string>());
                sqlCmd.Parameters.AddWithValue("@TenantDescription",  "");
                sqlCmd.Parameters.AddWithValue("@ProductName", bsObj2s.SelectToken("product.name").ToString());
                sqlCmd.Parameters.AddWithValue("@ProductShortName", bsObj2s.SelectToken("product.shortName").ToString());
                sqlCmd.Parameters.AddWithValue("@ProductVersion", bsObj2s.SelectToken("product.version").ToString());
                sqlCmd.Parameters.AddWithValue("@IsMingleEnabled", '1');
                sqlCmd.Parameters.AddWithValue("@StatusID",  1);
                sqlCmd.Parameters.AddWithValue("@appFarmId", 1);



                //string rslt1 = bsObj2s["id"].Value<string>();
                //string rslt2 = bsObj2s["logicalId"].Value<string>();
                //string rslt3 = bsObj2s["displayName"].Value<string>();
                ////     string rslt4   = bsObj2s["description"].Value<string>();
                //string rslt5 = bsObj2s.SelectToken("product.name").ToString();
                //string rslt6 = bsObj2s.SelectToken("product.shortName").ToString();
                //string rslt7 = bsObj2s.SelectToken("product.version").ToString();
                bool rslt8 = bsObj2s["mingleEnabled"].Value<bool>();


                try
                {
                    sqlCmd.ExecuteNonQuery();
                    executeCount++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" \t File Name : { file.Name } , \n  Exeption : {ex.Message} \n \n");
                }
                finally
                {
                    sqlCmd.Dispose();
                    objConn.Close();
                }
            }

            Console.WriteLine($"No of executed sql scripts : { executeCount }");









            BlogSites bsObj = new BlogSites()
            {
                Name = "C-sharpcorner",
                Description = "Share Knowledge"
            };

            // Convert BlogSites object to JOSN string format
            string jsonData = JsonConvert.SerializeObject(bsObj);

        //  Response.Write(jsonData);


     




            Console.WriteLine(jsonData);


            var bsObj2 = JsonConvert.DeserializeObject(jsonData);


            Console.WriteLine(bsObj2);



            string txt = BlogSites.EncodeServerName("vishal");
            Console.WriteLine(txt);

            Console.WriteLine(BlogSites.DecodeServerName(txt));



        }



    } // class end
}