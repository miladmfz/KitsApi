using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;


namespace webapikits.Model
{
    public class DataBaseClass
    {
        private readonly IConfiguration _configuration;

        public DataBaseClass(IConfiguration configuration)
        {
            _configuration = configuration;
        }





        public byte[] Web_GetImageData(String query)
        {
            byte[] imageData = null;
            Console.WriteLine(query);
            DataTable dataTableImg = Web_ImageExecQuery(query);
            if (dataTableImg.Rows.Count > 0)
            {
                if (!Convert.IsDBNull(dataTableImg.Rows[0][0]))
                {
                    imageData = (byte[])dataTableImg.Rows[0]["IMG"];
                }
                else
                {
                    Console.WriteLine("IsDBNull");
                }
            }
            else
            {
                Console.WriteLine("Rows.Count > 0");
            }

            return imageData;
        }



        public DataTable Support_ExecQuery( HttpContext httpContext, String query)
        {
            if (httpContext.Request.Path != "/api/Web/GetWebLog")
            {
                LogQuery(httpContext, query);
            }
            string connectionString = _configuration.GetConnectionString("Support_Connection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                ad.Fill(dataTable);
                con.Close();
                return dataTable;
            }
        }


        public DataTable SupportApp_ExecQuery( HttpContext httpContext, String query)
        {
            if (httpContext.Request.Path != "/api/Web/GetWebLog")
            {
                LogQuery(httpContext, query);
            }
            string connectionString = _configuration.GetConnectionString("SupportApp_Connection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                ad.Fill(dataTable);
                con.Close();
                return dataTable;
            }
        }

        public DataTable Web_ExecQuery( HttpContext httpContext, String query)
        {
            if (httpContext.Request.Path != "/api/Web/GetWebLog")
            {
                LogQuery(httpContext, query);
            }
            string connectionString = _configuration.GetConnectionString("Web_Connection"); 
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                 ad.Fill(dataTable);
                con.Close();
                return dataTable;
            }
        }

        public DataTable Kowsar_ExecQuery( HttpContext httpContext, String query)
        {
            if (httpContext.Request.Path != "/api/Web/GetWebLog")
            {
                LogQuery(httpContext, query);
            }
            string connectionString = _configuration.GetConnectionString("Kowsar_Connection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                ad.Fill(dataTable);
                con.Close();
                return dataTable;
            }
        }




        public DataTable Support_ImageExecQuery(String query)
        {
            string connectionString = _configuration.GetConnectionString("Support_ImageConnection"); // استفاده از IConfiguration برای خواندن connection string
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                ad.Fill(dataTable);
                con.Close();
                return dataTable;
            }
        }

        public DataTable Web_ImageExecQuery(String query)
        {
            string connectionString = _configuration.GetConnectionString("ImageConnection"); // استفاده از IConfiguration برای خواندن connection string
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                ad.Fill(dataTable);
                con.Close();
                return dataTable;
            }
        }





        public DataTable Broker_ExecQuery( HttpContext httpContext, String query)
        {

            if (httpContext.Request.Path != "/api/Web/GetWebLog")
            {
                // Log the route and function name
                LogQuery(httpContext, query);
            }
            string connectionString = _configuration.GetConnectionString("Broker_Connection"); 
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                ad.Fill(dataTable);
                con.Close();
                return dataTable;
            }
        }


        public DataTable Ocr_ExecQuery(HttpContext httpContext, String query)
        {

            if (httpContext.Request.Path != "/api/Web/GetWebLog")
            {
                // Log the route and function name
                LogQuery(httpContext, query);
            }
            string connectionString = _configuration.GetConnectionString("Ocr_Connection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                ad.Fill(dataTable);
                con.Close();
                return dataTable;
            }
        }



        public DataTable Order_ExecQuery( HttpContext httpContext, String query)
        {

            if (httpContext.Request.Path != "/api/Web/GetWebLog")
            {
                // Log the route and function name
                LogQuery(httpContext, query);
            }
            string connectionString = _configuration.GetConnectionString("Order_Connection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                ad.Fill(dataTable);
                con.Close();
                return dataTable;
            }
        }


        public DataTable Company_ExecQuery( HttpContext httpContext, String query)
        {

            if (httpContext.Request.Path != "/api/Web/GetWebLog")
            {
                // Log the route and function name
                LogQuery(httpContext, query);
            }
            string connectionString = _configuration.GetConnectionString("Company_Connection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                ad.Fill(dataTable);
                con.Close();
                return dataTable;
            }
        }




        public DataTable Kits_ExecQuery( HttpContext httpContext, String query)
        {

            if (httpContext.Request.Path != "/api/Web/GetWebLog")
            {
                // Log the route and function name
                LogQuery(httpContext, query);
            }
            string connectionString = _configuration.GetConnectionString("Kits_Connection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                ad.Fill(dataTable);
                con.Close();
                return dataTable;
            }
        }




        public void LogQuery(HttpContext httpContext, String query)
        {


            var headersDictionary = new Dictionary<string, string>();

            var agent = "";
            var PersonInfoRef = "";
            var Referer = "";

            foreach (var header in httpContext.Request.Headers)
            {
                if (header.Key == "User-Agent") {
                    agent = header.Value;
                }
                if (header.Key == "PersonInfoRef")
                {
                    PersonInfoRef = header.Value;
                }
                if (header.Key == "Referer")
                {
                    Referer = header.Value;

                }
            }


            query = query.Replace("'", "''");

            string Log = $"exec sp_WebLogInsert @ClassName='{httpContext.Request.Path}',@TagName='',@LogValue='{query}' ,@IpAddress='{Referer}',@UserAgent='{agent}',@SessionId='{PersonInfoRef}'";
            

            string connectionString = _configuration.GetConnectionString("Web_Connection"); // استفاده از IConfiguration برای خواندن connection string
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter ad = new SqlDataAdapter(Log, con);
                DataTable dataTable = new DataTable();
                ad.Fill(dataTable);
            }
        }







        public string ConvertToPersianNumber(string number)
        {
            // Implement your own logic to convert the numerical value to Persian text
            // This could involve replacing each digit with its Persian equivalent
            // For example, replace 1 with "۱", 2 with "۲", and so on

            // Example implementation:
            string result = number.Replace("1", "۱")
                                  .Replace("2", "۲")
                                  .Replace("3", "۳")
                                  .Replace("4", "۴")
                                  .Replace("5", "۵")
                                  .Replace("6", "۶")
                                  .Replace("7", "۷")
                                  .Replace("8", "۸")
                                  .Replace("9", "۹")
                                  .Replace("0", "۰");
            return result;
        }














        public string TestImportedit()
        {

            int batchSize = 1000;
            int rowCount = 0;
            int batches = 0;



            DataTable BrokerCustomer_dataTable = new DataTable();
            DataTable CacheGoodGroup_dataTable = new DataTable();
            DataTable Central_dataTable = new DataTable();
            DataTable City_dataTable = new DataTable();
            DataTable Customer_dataTable = new DataTable();
            DataTable Good_dataTable = new DataTable();
            DataTable GoodGroup_dataTable = new DataTable();
            DataTable GoodsGrp_dataTable = new DataTable();
            DataTable Goodstack_dataTable = new DataTable();
            DataTable Job_dataTable = new DataTable();
            DataTable JobPerson_dataTable = new DataTable();
            DataTable JobPerson_Good_dataTable = new DataTable();
            DataTable KsrImage_dataTable = new DataTable();
            DataTable ReplicationTable_dataTable = new DataTable();
            DataTable Units_dataTable = new DataTable();
            DataTable Address_dataTable = new DataTable();
            




            string server_connection = _configuration.GetConnectionString("AppbrokerConnection"); // استفاده از IConfiguration برای خواندن connection string
            string lite_connection = _configuration.GetConnectionString("SqliteConnection"); // مسیر و نام فایل دیتابیس SQLite جدید

            try
            {
                // بررسی وجود فایل قبل از حذف
                if (File.Exists(lite_connection))
                {
                    // حذف فایل
                    File.Delete(lite_connection);
                    Console.WriteLine($"File '{lite_connection}' has been successfully deleted.");
                }
                else
                {
                    Console.WriteLine($"File '{lite_connection}' does not exist.");
                }
            }
            catch (IOException e)
            {
                Console.WriteLine($"An error occurred while deleting the file: {e.Message}");
            }


            using (SqlConnection sqlserver_con = new SqlConnection(server_connection))
            {
                sqlserver_con.Open();

                SqlDataAdapter ad_BrokerCustomer = new SqlDataAdapter("Select  BrokerCustomerCode, BrokerRef, CustomerRef From BrokerCustomer", sqlserver_con);
                SqlDataAdapter ad_CacheGoodGroup = new SqlDataAdapter(" Select GoodRef, GroupsWhitoutCode From CacheGoodGroup", sqlserver_con);
                SqlDataAdapter ad_Central = new SqlDataAdapter(" Select CentralCode, CentralPrivateCode, Title, FName, Name, Manager, Delegacy, D_CodeMelli From Central", sqlserver_con);
                SqlDataAdapter ad_City = new SqlDataAdapter("Select CityCode, Name From City", sqlserver_con);
                SqlDataAdapter ad_Customer = new SqlDataAdapter("Select CustomerCode, CentralRef, AddressRef, EtebarNaghd,EtebarCheck, Takhfif, PriceTip, Active, CustomerBestankar, CustomerBedehkar From Customer", sqlserver_con);
                SqlDataAdapter ad_Good = new SqlDataAdapter(
                    " Select GoodCode,GoodMainCode, GoodName, GoodType,GoodExplain1, GoodExplain2, GoodExplain3,GoodExplain4, GoodExplain5, GoodExplain6," +
                    " SellPriceType, MaxSellPrice, MinSellPrice,SellPrice1, SellPrice2, SellPrice3, SellPrice4,SellPrice5, SellPrice6, FirstBarCode, GoodUnitRef," +
                    " DefaultUnitValue, ISBN, Nvarchar1, Nvarchar2,Nvarchar3, Nvarchar4, Nvarchar5, Nvarchar6,Nvarchar7, Nvarchar8, Nvarchar9, Nvarchar10,Nvarchar11," +
                    " Nvarchar12, Nvarchar13, Nvarchar14,Nvarchar15, Nvarchar16, Nvarchar17, Nvarchar18,Nvarchar19, Nvarchar20, Int1, Int2, Int3, Int4,Int5, Int6, Int7," +
                    " Int8, Int9, Int10, Float1, Float2,Float3, Float4, Float5, Float6, Float7, Float8, Float9,Float10, Date1, Date2, Date3, Date4, Date5, Bit1, Bit2," +
                    " Bit3, Bit4, Bit5, Bit6, Bit7, Text1, Text2, Text3, Text4, Text5  From good " 
                    , sqlserver_con);
                SqlDataAdapter ad_GoodGroup = new SqlDataAdapter("Select GoodGroupCode, GoodRef, GoodGroupRef From GoodGroup", sqlserver_con);
                SqlDataAdapter ad_GoodsGrp = new SqlDataAdapter("Select GroupCode, L1, L2, L3, L4, L5, Name From GoodsGrp", sqlserver_con);
                SqlDataAdapter ad_Goodstack = new SqlDataAdapter("Select GoodStackCode, GoodRef, StackRef ,Amount,ReservedAmount,ActiveStack From Goodstack", sqlserver_con);
                SqlDataAdapter ad_Job = new SqlDataAdapter("select JobCode,Title,Explain from Job", sqlserver_con);
                SqlDataAdapter ad_JobPerson = new SqlDataAdapter("select JobPersonCode,JobPersonPrivateCode,JobRef,CentralRef,AddressRef,PersonAlias from JobPerson", sqlserver_con);
                SqlDataAdapter ad_JobPerson_Good = new SqlDataAdapter("select JobPerson_GoodCode,GoodRef,JobPersonRef from JobPerson_Good", sqlserver_con);
                SqlDataAdapter ad_KsrImage = new SqlDataAdapter("Select KsrImageCode,ObjectRef,IsDefaultImage From KsrImage", sqlserver_con);
                SqlDataAdapter ad_ReplicationTable = new SqlDataAdapter("select ReplicationCode,ServerTable,ClientTable,ServerPrimaryKey,ClientPrimaryKey,Condition,ConditionDelete,LastRepLogCode,LastRepLogCodeDelete from ReplicationTable", sqlserver_con);
                SqlDataAdapter ad_Units = new SqlDataAdapter(" Select UnitCode, UnitName From Units", sqlserver_con);
                SqlDataAdapter ad_Address = new SqlDataAdapter(" Select AddressCode, CentralRef, CityCode, Address, Phone, Mobile, MobileName, Email, Fax, ZipCode, PostCode From Address", sqlserver_con);

                ad_BrokerCustomer.Fill(BrokerCustomer_dataTable);
                ad_CacheGoodGroup.Fill(CacheGoodGroup_dataTable);
                ad_Central.Fill(Central_dataTable);
                ad_City.Fill(City_dataTable);
                ad_Customer.Fill(Customer_dataTable);
                ad_Good.Fill(Good_dataTable);
                ad_GoodGroup.Fill(GoodGroup_dataTable);
                ad_GoodsGrp.Fill(GoodsGrp_dataTable);
                ad_Goodstack.Fill(Goodstack_dataTable);
                ad_Job.Fill(Job_dataTable);
                ad_JobPerson.Fill(JobPerson_dataTable);
                ad_JobPerson_Good.Fill(JobPerson_Good_dataTable);
                ad_KsrImage.Fill(KsrImage_dataTable);
                ad_ReplicationTable.Fill(ReplicationTable_dataTable);
                ad_Units.Fill(Units_dataTable);
                ad_Address.Fill(Address_dataTable);
                
                sqlserver_con.Close();

            }
            

             using (SQLiteConnection SQLite_con = new SQLiteConnection($"Data Source={lite_connection};Version=3;"))
             {
                 SQLite_con.Open();

                 string createTableQuery = "CREATE TABLE [BrokerCustomer]([BrokerCustomerCode] INTEGER PRIMARY KEY NOT NULL,[BrokerRef] INTEGER NOT NULL,[CustomerRef] INTEGER NOT NULL); CREATE TABLE [CacheGoodGroup]([GoodRef] INTEGER PRIMARY KEY NOT NULL,[GroupsWhitoutCode] CHAR(500)); CREATE TABLE [Central]([CentralCode] INTEGER PRIMARY KEY NOT NULL,[CentralPrivateCode] INTEGER NOT NULL,[Title] CHAR(50) NOT NULL,[FName] CHAR(70) NOT NULL,[Name] CHAR(50) NOT NULL,[Manager] CHAR(50),[Delegacy] CHAR(50),[D_CodeMelli] CHAR(11)); CREATE TABLE [City]([CityCode] INTEGER PRIMARY KEY NOT NULL,[Name] CHAR(50)); CREATE TABLE [Customer]([CustomerCode] INTEGER PRIMARY KEY NOT NULL,[CentralRef] INTEGER NOT NULL,[AddressRef] INTEGER,[EtebarNaghd] FLOAT NOT NULL,[EtebarCheck] FLOAT NOT NULL,[Takhfif] FLOAT NOT NULL,[PriceTip] INTEGER NOT NULL,[Active] INTEGER NOT NULL,[CustomerBestankar] FLOAT NOT NULL,[CustomerBedehkar] FLOAT NOT NULL); CREATE TABLE [Good]([GoodCode] INTEGER PRIMARY KEY NOT NULL,[GoodMainCode] CHAR(15) NOT NULL,[GoodName] CHAR(200) NOT NULL,[GoodType] CHAR(35),[GoodExplain1] CHAR(100),[GoodExplain2] CHAR(100),[GoodExplain3] CHAR(100),[GoodExplain4] CHAR(100),[GoodExplain5] CHAR(100),[GoodExplain6] CHAR(100),[SellPriceType] INTEGER NOT NULL,[MaxSellPrice] FLOAT NOT NULL,[MinSellPrice] FLOAT,[SellPrice1] FLOAT NOT NULL,[SellPrice2] FLOAT NOT NULL,[SellPrice3] FLOAT NOT NULL,[SellPrice4] FLOAT NOT NULL,[SellPrice5] FLOAT NOT NULL,[SellPrice6] FLOAT NOT NULL,[FirstBarCode] CHAR(50),[GoodUnitRef] INTEGER NOT NULL,[DefaultUnitValue] INTEGER NOT NULL,[ISBN] CHAR(50),[Nvarchar1] CHAR(200),[Nvarchar2] CHAR(200),[Nvarchar3] CHAR(200),[Nvarchar4] CHAR(200),[Nvarchar5] CHAR(200),[Nvarchar6] CHAR(200),[Nvarchar7] CHAR(200),[Nvarchar8] CHAR(200),[Nvarchar9] CHAR(200),[Nvarchar10] CHAR(200),[Nvarchar11] CHAR(200),[Nvarchar12] CHAR(200),[Nvarchar13] CHAR(200),[Nvarchar14] CHAR(200),[Nvarchar15] CHAR(200),[Nvarchar16] CHAR(200),[Nvarchar17] CHAR(200),[Nvarchar18] CHAR(200),[Nvarchar19] CHAR(200),[Nvarchar20] CHAR(200),[Int1] INTEGER,[Int2] INTEGER,[Int3] INTEGER,[Int4] INTEGER,[Int5] INTEGER,[Int6] INTEGER,[Int7] INTEGER,[Int8] INTEGER,[Int9] INTEGER,[Int10] INTEGER,[Float1] FLOAT,[Float2] FLOAT,[Float3] FLOAT,[Float4] FLOAT,[Float5] FLOAT,[Float6] FLOAT,[Float7] FLOAT,[Float8] FLOAT,[Float9] FLOAT,[Float10] FLOAT,[Date1] CHAR(10),[Date2] CHAR(10),[Date3] CHAR(10),[Date4] CHAR(10),[Date5] CHAR(10),[Bit1] BOOLEAN,[Bit2] BOOLEAN,[Bit3] BOOLEAN,[Bit4] BOOLEAN,[Bit5] BOOLEAN,[Bit6] BOOLEAN,[Bit7] BOOLEAN,[Text1] CHAR,[Text2] CHAR,[Text3] CHAR,[Text4] CHAR,[Text5] CHAR); CREATE TABLE [GoodGroup]([GoodGroupCode] INTEGER PRIMARY KEY NOT NULL,[GoodRef] INTEGER NOT NULL,[GoodGroupRef] INTEGER NOT NULL); CREATE TABLE [GoodsGrp]([GroupCode] INTEGER PRIMARY KEY NOT NULL,[L1] INTEGER,[L2] INTEGER,[L3] INTEGER,[L4] INTEGER,[L5] INTEGER,[Name] CHAR(50)); CREATE TABLE [Goodstack]([GoodStackCode] INTEGER PRIMARY KEY NOT NULL,[GoodRef] INTEGER NOT NULL,[StackRef] INTEGER NOT NULL,[Amount] FLOAT NOT NULL,[ReservedAmount] FLOAT NOT NULL,[ActiveStack] BOOLEAN NOT NULL); CREATE TABLE [Job]([JobCode] INTEGER PRIMARY KEY NOT NULL,[Title] CHAR(255) NOT NULL,[Explain] CHAR(1000),[GroupCode] INTEGER,[KolCode] INTEGER,[MoeenCode] INTEGER,[Owner] INTEGER NOT NULL,[CreationDate] DATETIME NOT NULL,[Reformer] INTEGER NOT NULL,[ReformDate] DATETIME NOT NULL); CREATE TABLE [JobPerson]([JobPersonCode] INTEGER PRIMARY KEY NOT NULL,[JobPersonPrivateCode] INTEGER NOT NULL,[JobRef] INTEGER NOT NULL,[CentralRef] INTEGER NOT NULL,[AddressRef] INTEGER,[Explain] CHAR(255),[Active] BOOLEAN NOT NULL,[GroupCode] INTEGER,[KolCode] INTEGER,[MoeenCode] INTEGER,[Owner] INTEGER NOT NULL,[CreationDate] DATETIME NOT NULL,[Reformer] INTEGER NOT NULL,[ReformDate] DATETIME NOT NULL,[PersonAlias] CHAR(50) NOT NULL); CREATE TABLE [JobPerson_Good]([JobPerson_GoodCode] INTEGER PRIMARY KEY NOT NULL,[JobPersonRef] INTEGER NOT NULL,[GoodRef] INTEGER NOT NULL,[StartDate] CHAR(10),[AggregateSubCode] BOOLEAN NOT NULL,[CalcFlag] INTEGER NOT NULL,[BaseValue] FLOAT,[PriceFlag] INTEGER NOT NULL,[cPercent] FLOAT,[LastCalcDate] CHAR(10),[Owner] INTEGER NOT NULL,[CreationDate] DATETIME NOT NULL,[Reformer] INTEGER NOT NULL,[ReformDate] DATETIME NOT NULL); CREATE TABLE [KsrImage]([KsrImageCode] INTEGER PRIMARY KEY NOT NULL,[ObjectRef] INTEGER NOT NULL,[IsDefaultImage] BOOLEAN NOT NULL); CREATE TABLE [ReplicationTable]([ReplicationCode] INTEGER PRIMARY KEY NOT NULL,[ServerTable] CHAR(256),[ClientTable] CHAR(256),[ServerPrimaryKey] CHAR(256),[ClientPrimaryKey] CHAR(256),[Condition] CHAR(4000),[ConditionDelete] CHAR(4000),[LastRepLogCode] INTEGER,[LastRepLogCodeDelete] INTEGER); CREATE TABLE [Units]([UnitCode] INTEGER PRIMARY KEY NOT NULL,[UnitName] CHAR(50)); CREATE TABLE [Address]([AddressCode] INTEGER PRIMARY KEY NOT NULL,[CentralRef] INTEGER NOT NULL,[CityCode] INTEGER NOT NULL,[Address] CHAR(255) NOT NULL,[Phone] CHAR(50) NOT NULL,[Mobile] CHAR(20) NOT NULL,[MobileName] CHAR(50) NOT NULL,[Email] CHAR(50) NOT NULL,[Fax] CHAR(20) NOT NULL, [ZipCode] CHAR(20) NOT NULL,[PostCode] CHAR(20) NOT NULL);";
                 using (SQLiteCommand command = new SQLiteCommand(createTableQuery, SQLite_con))
                 {
                     command.ExecuteNonQuery();
                 }



                 


                Console.WriteLine("Good_dataTable");

                // استفاده از SQLiteDataAdapter برای وارد کردن داده‌های DataTable به دیتابیس SQLite
                batchSize = 1000; // Define your batch size
                rowCount = Good_dataTable.Rows.Count;
                batches = (rowCount + batchSize - 1) / batchSize;

                for (int i = 0; i < batches; i++)
                {
                    int start = i * batchSize;
                    int end = Math.Min(rowCount, (i + 1) * batchSize);

                    using (SQLiteTransaction transaction = SQLite_con.BeginTransaction())
                    {
                        using (SQLiteCommand cmd = SQLite_con.CreateCommand())
                        {

                            cmd.CommandText = "Insert INTO Good ( GoodCode,GoodMainCode, GoodName, GoodType,GoodExplain1, GoodExplain2, GoodExplain3,GoodExplain4, GoodExplain5, GoodExplain6," +
                    " SellPriceType, MaxSellPrice, MinSellPrice,SellPrice1, SellPrice2, SellPrice3, SellPrice4,SellPrice5, SellPrice6, FirstBarCode, GoodUnitRef," +
                    " DefaultUnitValue, ISBN, Nvarchar1, Nvarchar2,Nvarchar3, Nvarchar4, Nvarchar5, Nvarchar6,Nvarchar7, Nvarchar8, Nvarchar9, Nvarchar10,Nvarchar11," +
                    " Nvarchar12, Nvarchar13, Nvarchar14,Nvarchar15, Nvarchar16, Nvarchar17, Nvarchar18,Nvarchar19, Nvarchar20, Int1, Int2, Int3, Int4,Int5, Int6, Int7," +
                    " Int8, Int9, Int10, Float1, Float2,Float3, Float4, Float5, Float6, Float7, Float8, Float9,Float10, Date1, Date2, Date3, Date4, Date5, Bit1, Bit2," +
                    " Bit3, Bit4, Bit5, Bit6, Bit7, Text1, Text2, Text3, Text4, Text5   )  VALUES (@GoodCode,@GoodMainCode,@GoodName,@GoodType,@GoodExplain1,@GoodExplain2,@GoodExplain3,@GoodExplain4,@GoodExplain5,@GoodExplain6," +
                    " @SellPriceType,@MaxSellPrice,@MinSellPrice,@SellPrice1,@SellPrice2,@SellPrice3,@SellPrice4,@SellPrice5,@SellPrice6,@FirstBarCode,@GoodUnitRef," +
                    " @DefaultUnitValue,@ISBN,@Nvarchar1,@Nvarchar2,@Nvarchar3,@Nvarchar4,@Nvarchar5,@Nvarchar6,@Nvarchar7,@Nvarchar8,@Nvarchar9,@Nvarchar10,@Nvarchar11," +
                    " @Nvarchar12,@Nvarchar13,@Nvarchar14,@Nvarchar15,@Nvarchar16,@Nvarchar17,@Nvarchar18,@Nvarchar19,@Nvarchar20,@Int1,@Int2,@Int3,@Int4,@Int5,@Int6,@Int7," +
                    " @Int8,@Int9,@Int10,@Float1,@Float2,@Float3,@Float4,@Float5,@Float6,@Float7,@Float8,@Float9,@Float10,@Date1,@Date2,@Date3,@Date4,@Date5,@Bit1,@Bit2," +
                    " @Bit3,@Bit4,@Bit5,@Bit6,@Bit7,@Text1,@Text2,@Text3,@Text4,@Text5 )";


                            cmd.Parameters.Add("@GoodCode", DbType.Double);
                            cmd.Parameters.Add("@GoodMainCode", DbType.String);
                            cmd.Parameters.Add("@GoodName", DbType.String);
                            cmd.Parameters.Add("@GoodType", DbType.String);
                            cmd.Parameters.Add("@GoodExplain1", DbType.String);
                            cmd.Parameters.Add("@GoodExplain2", DbType.String);
                            cmd.Parameters.Add("@GoodExplain3", DbType.String);
                            cmd.Parameters.Add("@GoodExplain4", DbType.String);
                            cmd.Parameters.Add("@GoodExplain5", DbType.String);
                            cmd.Parameters.Add("@GoodExplain6", DbType.String);
                            cmd.Parameters.Add("@SellPriceType", DbType.Double);
                            cmd.Parameters.Add("@MaxSellPrice", DbType.Double);
                            cmd.Parameters.Add("@MinSellPrice", DbType.Double);
                            cmd.Parameters.Add("@SellPrice1", DbType.Double);
                            cmd.Parameters.Add("@SellPrice2", DbType.Double);
                            cmd.Parameters.Add("@SellPrice3", DbType.Double);
                            cmd.Parameters.Add("@SellPrice4", DbType.Double);
                            cmd.Parameters.Add("@SellPrice5", DbType.Double);
                            cmd.Parameters.Add("@SellPrice6", DbType.Double);
                            cmd.Parameters.Add("@FirstBarCode", DbType.String);
                            cmd.Parameters.Add("@GoodUnitRef", DbType.Double);
                            cmd.Parameters.Add("@DefaultUnitValue", DbType.Double);
                            cmd.Parameters.Add("@ISBN", DbType.String);
                            cmd.Parameters.Add("@Nvarchar1", DbType.String);
                            cmd.Parameters.Add("@Nvarchar2", DbType.String);
                            cmd.Parameters.Add("@Nvarchar3", DbType.String);
                            cmd.Parameters.Add("@Nvarchar4", DbType.String);
                            cmd.Parameters.Add("@Nvarchar5", DbType.String);
                            cmd.Parameters.Add("@Nvarchar6", DbType.String);
                            cmd.Parameters.Add("@Nvarchar7", DbType.String);
                            cmd.Parameters.Add("@Nvarchar8", DbType.String);
                            cmd.Parameters.Add("@Nvarchar9", DbType.String);
                            cmd.Parameters.Add("@Nvarchar10", DbType.String);
                            cmd.Parameters.Add("@Nvarchar11", DbType.String);
                            cmd.Parameters.Add("@Nvarchar12", DbType.String);
                            cmd.Parameters.Add("@Nvarchar13", DbType.String);
                            cmd.Parameters.Add("@Nvarchar14", DbType.String);
                            cmd.Parameters.Add("@Nvarchar15", DbType.String);
                            cmd.Parameters.Add("@Nvarchar16", DbType.String);
                            cmd.Parameters.Add("@Nvarchar17", DbType.String);
                            cmd.Parameters.Add("@Nvarchar18", DbType.String);
                            cmd.Parameters.Add("@Nvarchar19", DbType.String);
                            cmd.Parameters.Add("@Nvarchar20", DbType.String);
                            cmd.Parameters.Add("@Int1", DbType.Int32);
                            cmd.Parameters.Add("@Int2", DbType.Int32);
                            cmd.Parameters.Add("@Int3", DbType.Int32);
                            cmd.Parameters.Add("@Int4", DbType.Int32);
                            cmd.Parameters.Add("@Int5", DbType.Int32);
                            cmd.Parameters.Add("@Int6", DbType.Int32);
                            cmd.Parameters.Add("@Int7", DbType.Int32);
                            cmd.Parameters.Add("@Int8", DbType.Int32);
                            cmd.Parameters.Add("@Int9", DbType.Int32);
                            cmd.Parameters.Add("@Int10", DbType.Int32);
                            cmd.Parameters.Add("@Float1", DbType.Double);
                            cmd.Parameters.Add("@Float2", DbType.Double);
                            cmd.Parameters.Add("@Float3", DbType.Double);
                            cmd.Parameters.Add("@Float4", DbType.Double);
                            cmd.Parameters.Add("@Float5", DbType.Double);
                            cmd.Parameters.Add("@Float6", DbType.Double);
                            cmd.Parameters.Add("@Float7", DbType.Double);
                            cmd.Parameters.Add("@Float8", DbType.Double);
                            cmd.Parameters.Add("@Float9", DbType.Double);
                            cmd.Parameters.Add("@Float10", DbType.Double);
                            cmd.Parameters.Add("@Date1", DbType.String);
                            cmd.Parameters.Add("@Date2", DbType.String);
                            cmd.Parameters.Add("@Date3", DbType.String);
                            cmd.Parameters.Add("@Date4", DbType.String);
                            cmd.Parameters.Add("@Date5", DbType.String);
                            cmd.Parameters.Add("@Bit1", DbType.Boolean);
                            cmd.Parameters.Add("@Bit2", DbType.Boolean);
                            cmd.Parameters.Add("@Bit3", DbType.Boolean);
                            cmd.Parameters.Add("@Bit4", DbType.Boolean);
                            cmd.Parameters.Add("@Bit5", DbType.Boolean);
                            cmd.Parameters.Add("@Bit6", DbType.Boolean);
                            cmd.Parameters.Add("@Bit7", DbType.Boolean);
                            cmd.Parameters.Add("@Text1", DbType.String);
                            cmd.Parameters.Add("@Text2", DbType.String);
                            cmd.Parameters.Add("@Text3", DbType.String);
                            cmd.Parameters.Add("@Text4", DbType.String);
                            cmd.Parameters.Add("@Text5", DbType.String);


                            //cmd.Parameters.Add("@CustomerRef", DbType.Int32);
                            // cmd.Parameters.Add("@Name", DbType.String);
                            for (int j = start; j < end; j++)
                            {
                                Console.WriteLine(Good_dataTable.Rows[j]["GoodCode"].ToString());
                                cmd.Parameters["@GoodCode"].Value = (Good_dataTable.Rows[j]["GoodCode"].ToString().Length > 0) ? Convert.ToInt32(Good_dataTable.Rows[j]["GoodCode"]) : 0;
                                cmd.Parameters["@GoodMainCode"].Value = (Good_dataTable.Rows[j]["GoodMainCode"].ToString().Length > 0) ? Good_dataTable.Rows[j]["GoodMainCode"].ToString() : "";
                                cmd.Parameters["@GoodName"].Value = (Good_dataTable.Rows[j]["GoodName"].ToString().Length > 0) ? Good_dataTable.Rows[j]["GoodName"].ToString() : "";
                                cmd.Parameters["@GoodType"].Value = (Good_dataTable.Rows[j]["GoodType"].ToString().Length > 0) ? Good_dataTable.Rows[j]["GoodType"].ToString() : "";
                                cmd.Parameters["@GoodExplain1"].Value = (Good_dataTable.Rows[j]["GoodExplain1"].ToString().Length > 0) ? Good_dataTable.Rows[j]["GoodExplain1"].ToString() : "";
                                cmd.Parameters["@GoodExplain2"].Value = (Good_dataTable.Rows[j]["GoodExplain2"].ToString().Length > 0) ? Good_dataTable.Rows[j]["GoodExplain2"].ToString() : "";
                                cmd.Parameters["@GoodExplain3"].Value = (Good_dataTable.Rows[j]["GoodExplain3"].ToString().Length > 0) ? Good_dataTable.Rows[j]["GoodExplain3"].ToString() : "";
                                cmd.Parameters["@GoodExplain4"].Value = (Good_dataTable.Rows[j]["GoodExplain4"].ToString().Length > 0) ? Good_dataTable.Rows[j]["GoodExplain4"].ToString() : "";
                                cmd.Parameters["@GoodExplain5"].Value = (Good_dataTable.Rows[j]["GoodExplain5"].ToString().Length > 0) ? Good_dataTable.Rows[j]["GoodExplain5"].ToString() : "";
                                cmd.Parameters["@GoodExplain6"].Value = (Good_dataTable.Rows[j]["GoodExplain6"].ToString().Length > 0) ? Good_dataTable.Rows[j]["GoodExplain6"].ToString() : "";
                                cmd.Parameters["@SellPriceType"].Value = (Good_dataTable.Rows[j]["SellPriceType"].ToString().Length > 0) ? Convert.ToInt32(Good_dataTable.Rows[j]["SellPriceType"]) : 0;
                                cmd.Parameters["@MaxSellPrice"].Value = (Good_dataTable.Rows[j]["MaxSellPrice"].ToString().Length > 0) ? Convert.ToInt32(Good_dataTable.Rows[j]["MaxSellPrice"]) : 0;
                                cmd.Parameters["@MinSellPrice"].Value = (Good_dataTable.Rows[j]["MinSellPrice"].ToString().Length > 0) ? Convert.ToInt32(Good_dataTable.Rows[j]["MinSellPrice"]) : 0;
                                cmd.Parameters["@SellPrice1"].Value = (Good_dataTable.Rows[j]["SellPrice1"].ToString().Length > 0) ? Convert.ToInt32(Good_dataTable.Rows[j]["SellPrice1"]) : 0;
                                cmd.Parameters["@SellPrice2"].Value = (Good_dataTable.Rows[j]["SellPrice2"].ToString().Length > 0) ? Convert.ToInt32(Good_dataTable.Rows[j]["SellPrice2"]) : 0;
                                cmd.Parameters["@SellPrice3"].Value = (Good_dataTable.Rows[j]["SellPrice3"].ToString().Length > 0) ? Convert.ToInt32(Good_dataTable.Rows[j]["SellPrice3"]) : 0;
                                cmd.Parameters["@SellPrice4"].Value = (Good_dataTable.Rows[j]["SellPrice4"].ToString().Length > 0) ? Convert.ToInt32(Good_dataTable.Rows[j]["SellPrice4"]) : 0;
                                cmd.Parameters["@SellPrice5"].Value = (Good_dataTable.Rows[j]["SellPrice5"].ToString().Length > 0) ? Convert.ToInt32(Good_dataTable.Rows[j]["SellPrice5"]) : 0;
                                cmd.Parameters["@SellPrice6"].Value = (Good_dataTable.Rows[j]["SellPrice6"].ToString().Length > 0) ? Convert.ToInt32(Good_dataTable.Rows[j]["SellPrice6"]) : 0;
                                cmd.Parameters["@FirstBarCode"].Value = (Good_dataTable.Rows[j]["FirstBarCode"].ToString().Length > 0) ? Good_dataTable.Rows[j]["FirstBarCode"].ToString() : "";
                                cmd.Parameters["@GoodUnitRef"].Value = (Good_dataTable.Rows[j]["GoodUnitRef"].ToString().Length > 0) ? Convert.ToInt32(Good_dataTable.Rows[j]["GoodUnitRef"]) : 0;
                                cmd.Parameters["@DefaultUnitValue"].Value = (Good_dataTable.Rows[j]["DefaultUnitValue"].ToString().Length > 0) ? Convert.ToInt32(Good_dataTable.Rows[j]["DefaultUnitValue"]) : 0;
                                cmd.Parameters["@ISBN"].Value = (Good_dataTable.Rows[j]["ISBN"].ToString().Length > 0) ? Good_dataTable.Rows[j]["ISBN"].ToString() : "";
                                cmd.Parameters["@Nvarchar1"].Value = (Good_dataTable.Rows[j]["Nvarchar1"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Nvarchar1"].ToString() : "";
                                cmd.Parameters["@Nvarchar2"].Value = (Good_dataTable.Rows[j]["Nvarchar2"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Nvarchar2"].ToString() : "";
                                cmd.Parameters["@Nvarchar3"].Value = (Good_dataTable.Rows[j]["Nvarchar3"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Nvarchar3"].ToString() : "";
                                cmd.Parameters["@Nvarchar4"].Value = (Good_dataTable.Rows[j]["Nvarchar4"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Nvarchar4"].ToString() : "";
                                cmd.Parameters["@Nvarchar5"].Value = (Good_dataTable.Rows[j]["Nvarchar5"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Nvarchar5"].ToString() : "";
                                cmd.Parameters["@Nvarchar6"].Value = (Good_dataTable.Rows[j]["Nvarchar6"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Nvarchar6"].ToString() : "";
                                cmd.Parameters["@Nvarchar7"].Value = (Good_dataTable.Rows[j]["Nvarchar7"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Nvarchar7"].ToString() : "";
                                cmd.Parameters["@Nvarchar8"].Value = (Good_dataTable.Rows[j]["Nvarchar8"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Nvarchar8"].ToString() : "";
                                cmd.Parameters["@Nvarchar9"].Value = (Good_dataTable.Rows[j]["Nvarchar9"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Nvarchar9"].ToString() : "";
                                cmd.Parameters["@Nvarchar10"].Value = (Good_dataTable.Rows[j]["Nvarchar10"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Nvarchar10"].ToString() : "";
                                cmd.Parameters["@Nvarchar11"].Value = (Good_dataTable.Rows[j]["Nvarchar11"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Nvarchar11"].ToString() : "";
                                cmd.Parameters["@Nvarchar12"].Value = (Good_dataTable.Rows[j]["Nvarchar12"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Nvarchar12"].ToString() : "";
                                cmd.Parameters["@Nvarchar13"].Value = (Good_dataTable.Rows[j]["Nvarchar13"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Nvarchar13"].ToString() : "";
                                cmd.Parameters["@Nvarchar14"].Value = (Good_dataTable.Rows[j]["Nvarchar14"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Nvarchar14"].ToString() : "";
                                cmd.Parameters["@Nvarchar15"].Value = (Good_dataTable.Rows[j]["Nvarchar15"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Nvarchar15"].ToString() : "";
                                cmd.Parameters["@Nvarchar16"].Value = (Good_dataTable.Rows[j]["Nvarchar16"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Nvarchar16"].ToString() : "";
                                cmd.Parameters["@Nvarchar17"].Value = (Good_dataTable.Rows[j]["Nvarchar17"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Nvarchar17"].ToString() : "";
                                cmd.Parameters["@Nvarchar18"].Value = (Good_dataTable.Rows[j]["Nvarchar18"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Nvarchar18"].ToString() : "";
                                cmd.Parameters["@Nvarchar19"].Value = (Good_dataTable.Rows[j]["Nvarchar19"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Nvarchar19"].ToString() : "";
                                cmd.Parameters["@Nvarchar20"].Value = (Good_dataTable.Rows[j]["Nvarchar20"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Nvarchar20"].ToString() : "";
                                cmd.Parameters["@Int1"].Value = (Good_dataTable.Rows[j]["Int1"].ToString().Length > 0) ? Convert.ToInt32(Good_dataTable.Rows[j]["Int1"]) : 0;
                                cmd.Parameters["@Int2"].Value = (Good_dataTable.Rows[j]["Int2"].ToString().Length > 0) ? Convert.ToInt32(Good_dataTable.Rows[j]["Int2"]) : 0;
                                cmd.Parameters["@Int3"].Value = (Good_dataTable.Rows[j]["Int3"].ToString().Length > 0) ? Convert.ToInt32(Good_dataTable.Rows[j]["Int3"]) : 0;
                                cmd.Parameters["@Int4"].Value = (Good_dataTable.Rows[j]["Int4"].ToString().Length > 0) ? Convert.ToInt32(Good_dataTable.Rows[j]["Int4"]) : 0;
                                cmd.Parameters["@Int5"].Value = (Good_dataTable.Rows[j]["Int5"].ToString().Length > 0) ? Convert.ToInt32(Good_dataTable.Rows[j]["Int5"]) : 0;
                                cmd.Parameters["@Int6"].Value = (Good_dataTable.Rows[j]["Int6"].ToString().Length > 0) ? Convert.ToInt32(Good_dataTable.Rows[j]["Int6"]) : 0;
                                cmd.Parameters["@Int7"].Value = (Good_dataTable.Rows[j]["Int7"].ToString().Length > 0) ? Convert.ToInt32(Good_dataTable.Rows[j]["Int7"]) : 0;
                                cmd.Parameters["@Int8"].Value = (Good_dataTable.Rows[j]["Int8"].ToString().Length > 0) ? Convert.ToInt32(Good_dataTable.Rows[j]["Int8"]) : 0;
                                cmd.Parameters["@Int9"].Value = (Good_dataTable.Rows[j]["Int9"].ToString().Length > 0) ? Convert.ToInt32(Good_dataTable.Rows[j]["Int9"]) : 0;
                                cmd.Parameters["@Int10"].Value = (Good_dataTable.Rows[j]["Int10"].ToString().Length > 0) ? Convert.ToDouble(Good_dataTable.Rows[j]["Int10"]) : 0;
                                cmd.Parameters["@Float1"].Value = (Good_dataTable.Rows[j]["Float1"].ToString().Length > 0) ? Convert.ToDouble(Good_dataTable.Rows[j]["Float1"]) : 0;
                                cmd.Parameters["@Float2"].Value = (Good_dataTable.Rows[j]["Float2"].ToString().Length > 0) ? Convert.ToDouble(Good_dataTable.Rows[j]["Float2"]) : 0;
                                cmd.Parameters["@Float3"].Value = (Good_dataTable.Rows[j]["Float3"].ToString().Length > 0) ? Convert.ToDouble(Good_dataTable.Rows[j]["Float3"]) : 0;
                                cmd.Parameters["@Float4"].Value = (Good_dataTable.Rows[j]["Float4"].ToString().Length > 0) ? Convert.ToDouble(Good_dataTable.Rows[j]["Float4"]) : 0;
                                cmd.Parameters["@Float5"].Value = (Good_dataTable.Rows[j]["Float5"].ToString().Length > 0) ? Convert.ToDouble(Good_dataTable.Rows[j]["Float5"]) : 0;
                                cmd.Parameters["@Float6"].Value = (Good_dataTable.Rows[j]["Float6"].ToString().Length > 0) ? Convert.ToDouble(Good_dataTable.Rows[j]["Float6"]) : 0;
                                cmd.Parameters["@Float7"].Value = (Good_dataTable.Rows[j]["Float7"].ToString().Length > 0) ? Convert.ToDouble(Good_dataTable.Rows[j]["Float7"]) : 0;
                                cmd.Parameters["@Float8"].Value = (Good_dataTable.Rows[j]["Float8"].ToString().Length > 0) ? Convert.ToDouble(Good_dataTable.Rows[j]["Float8"]) : 0;
                                cmd.Parameters["@Float9"].Value = (Good_dataTable.Rows[j]["Float9"].ToString().Length > 0) ? Convert.ToDouble(Good_dataTable.Rows[j]["Float9"]) : 0;
                                cmd.Parameters["@Float10"].Value = (Good_dataTable.Rows[j]["Float10"].ToString().Length > 0) ? Convert.ToDouble(Good_dataTable.Rows[j]["Float10"]) : 0;
                                cmd.Parameters["@Date1"].Value = (Good_dataTable.Rows[j]["Date1"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Date1"].ToString() : "";
                                cmd.Parameters["@Date2"].Value = (Good_dataTable.Rows[j]["Date2"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Date2"].ToString() : "";
                                cmd.Parameters["@Date3"].Value = (Good_dataTable.Rows[j]["Date3"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Date3"].ToString() : "";
                                cmd.Parameters["@Date4"].Value = (Good_dataTable.Rows[j]["Date4"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Date4"].ToString() : "";
                                cmd.Parameters["@Date5"].Value = (Good_dataTable.Rows[j]["Date5"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Date5"].ToString() : "";
                                cmd.Parameters["@Bit1"].Value = (Good_dataTable.Rows[j]["Bit1"].ToString().Length > 0) ? Convert.ToBoolean(Good_dataTable.Rows[j]["Bit1"]) : 0;
                                cmd.Parameters["@Bit2"].Value = (Good_dataTable.Rows[j]["Bit2"].ToString().Length > 0) ? Convert.ToBoolean(Good_dataTable.Rows[j]["Bit2"]) : 0;
                                cmd.Parameters["@Bit3"].Value = (Good_dataTable.Rows[j]["Bit3"].ToString().Length > 0) ? Convert.ToBoolean(Good_dataTable.Rows[j]["Bit3"]) : 0;
                                cmd.Parameters["@Bit4"].Value = (Good_dataTable.Rows[j]["Bit4"].ToString().Length > 0) ? Convert.ToBoolean(Good_dataTable.Rows[j]["Bit4"]) : 0;
                                cmd.Parameters["@Bit5"].Value = (Good_dataTable.Rows[j]["Bit5"].ToString().Length > 0) ? Convert.ToBoolean(Good_dataTable.Rows[j]["Bit5"]) : 0;
                                cmd.Parameters["@Bit6"].Value = (Good_dataTable.Rows[j]["Bit6"].ToString().Length > 0) ? Convert.ToBoolean(Good_dataTable.Rows[j]["Bit6"]) : 0;
                                cmd.Parameters["@Bit7"].Value = (Good_dataTable.Rows[j]["Bit7"].ToString().Length > 0) ? Convert.ToBoolean(Good_dataTable.Rows[j]["Bit7"]) : 0;
                                cmd.Parameters["@Text1"].Value = (Good_dataTable.Rows[j]["Text1"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Text1"].ToString() : "";
                                cmd.Parameters["@Text2"].Value = (Good_dataTable.Rows[j]["Text2"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Text2"].ToString() : "";
                                cmd.Parameters["@Text3"].Value = (Good_dataTable.Rows[j]["Text3"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Text3"].ToString() : "";
                                cmd.Parameters["@Text4"].Value = (Good_dataTable.Rows[j]["Text4"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Text4"].ToString() : "";
                                cmd.Parameters["@Text5"].Value = (Good_dataTable.Rows[j]["Text5"].ToString().Length > 0) ? Good_dataTable.Rows[j]["Text5"].ToString() : "";


                                cmd.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                }

                 




                // استفاده از SQLiteDataAdapter برای وارد کردن داده‌های DataTable به دیتابیس SQLite
                batchSize = 1000; // Define your batch size
                rowCount = BrokerCustomer_dataTable.Rows.Count;
                batches = (rowCount + batchSize - 1) / batchSize;

                for (int i = 0; i < batches; i++)
                {
                    int start = i * batchSize;
                    int end = Math.Min(rowCount, (i + 1) * batchSize);

                    using (SQLiteTransaction transaction = SQLite_con.BeginTransaction())
                    {
                        using (SQLiteCommand cmd = SQLite_con.CreateCommand())
                        {

                            cmd.CommandText = "Insert INTO BrokerCustomer (BrokerCustomerCode, BrokerRef, CustomerRef) VALUES (@BrokerCustomerCode, @BrokerRef, @CustomerRef)";
                            cmd.Parameters.Add("@BrokerCustomerCode", DbType.Int32);
                            cmd.Parameters.Add("@BrokerRef", DbType.Int32);
                            cmd.Parameters.Add("@CustomerRef", DbType.Int32);
                            //cmd.Parameters.Add("@CustomerRef", DbType.Int32);
                            // cmd.Parameters.Add("@Name", DbType.String);
                            for (int j = start; j < end; j++)
                            {
                                cmd.Parameters["@BrokerCustomerCode"].Value = Convert.ToInt32(BrokerCustomer_dataTable.Rows[j]["BrokerCustomerCode"]);
                                cmd.Parameters["@BrokerRef"].Value = Convert.ToInt32(BrokerCustomer_dataTable.Rows[j]["BrokerRef"]);
                                cmd.Parameters["@CustomerRef"].Value = Convert.ToInt32(BrokerCustomer_dataTable.Rows[j]["CustomerRef"]);
                                //cmd.Parameters["@CustomerRef"].Value = Convert.ToInt32(BrokerCustomer_dataTable.Rows[j]["CustomerRef"]);
                                //cmd.Parameters["@Name"].Value = BrokerCustomer_dataTable.Rows[j]["GoodName"].ToString();

                                cmd.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                }


                Console.WriteLine("CacheGoodGroup");

                // استفاده از SQLiteDataAdapter برای وارد کردن داده‌های DataTable به دیتابیس SQLite
                batchSize = 1000; // Define your batch size
                rowCount = CacheGoodGroup_dataTable.Rows.Count;
                batches = (rowCount + batchSize - 1) / batchSize;

                for (int i = 0; i < batches; i++)
                {
                    int start = i * batchSize;
                    int end = Math.Min(rowCount, (i + 1) * batchSize);

                    using (SQLiteTransaction transaction = SQLite_con.BeginTransaction())
                    {
                        using (SQLiteCommand cmd = SQLite_con.CreateCommand())
                        {

                            cmd.CommandText = "Insert INTO CacheGoodGroup ( GoodRef,GroupsWhitoutCode) VALUES (@GoodRef, @GroupsWhitoutCode)";
                            cmd.Parameters.Add("@GoodRef", DbType.Int32);
                            cmd.Parameters.Add("@GroupsWhitoutCode", DbType.String);

                            for (int j = start; j < end; j++)
                            {
                                cmd.Parameters["@GoodRef"].Value = Convert.ToInt32(CacheGoodGroup_dataTable.Rows[j]["GoodRef"]);
                                cmd.Parameters["@GroupsWhitoutCode"].Value = CacheGoodGroup_dataTable.Rows[j]["GroupsWhitoutCode"].ToString();


                                //cmd.Parameters["@GoodRef"].Value = Convert.ToInt32(CacheGoodGroup_dataTable.Rows[j]["GoodRef"]);
                                //cmd.Parameters["@Name"].Value = BrokerCustomer_dataTable.Rows[j]["GoodName"].ToString();

                                cmd.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                }







                Console.WriteLine("Central_dataTable");


                // استفاده از SQLiteDataAdapter برای وارد کردن داده‌های DataTable به دیتابیس SQLite
                batchSize = 1000; // Define your batch size
                rowCount = Central_dataTable.Rows.Count;
                batches = (rowCount + batchSize - 1) / batchSize;

                for (int i = 0; i < batches; i++)
                {
                    int start = i * batchSize;
                    int end = Math.Min(rowCount, (i + 1) * batchSize);

                    using (SQLiteTransaction transaction = SQLite_con.BeginTransaction())
                    {
                        using (SQLiteCommand cmd = SQLite_con.CreateCommand())
                        {

                            cmd.CommandText = "Insert INTO Central ( CentralCode, CentralPrivateCode, Title, FName, Name, Manager, Delegacy, D_CodeMelli )  VALUES (@CentralCode,@CentralPrivateCode, @Title, @FName, @Name,@Manager, @Delegacy, @D_CodeMelli )";
                            cmd.Parameters.Add("@CentralCode", DbType.Int32);
                            cmd.Parameters.Add("@CentralPrivateCode", DbType.Int32);
                            cmd.Parameters.Add("@Title", DbType.String);
                            cmd.Parameters.Add("@FName", DbType.String);
                            cmd.Parameters.Add("@Name", DbType.String);
                            cmd.Parameters.Add("@Manager", DbType.String);
                            cmd.Parameters.Add("@Delegacy", DbType.String);
                            cmd.Parameters.Add("@D_CodeMelli", DbType.String);

                            //cmd.Parameters.Add("@CustomerRef", DbType.Int32);
                            // cmd.Parameters.Add("@Name", DbType.String);
                            for (int j = start; j < end; j++)
                            {
                                cmd.Parameters["@CentralCode"].Value = Convert.ToInt32(Central_dataTable.Rows[j]["CentralCode"]);
                                cmd.Parameters["@CentralPrivateCode"].Value = Convert.ToInt32(Central_dataTable.Rows[j]["CentralPrivateCode"]);
                                cmd.Parameters["@Title"].Value = Central_dataTable.Rows[j]["Title"].ToString();
                                cmd.Parameters["@FName"].Value = Central_dataTable.Rows[j]["FName"].ToString();
                                cmd.Parameters["@Name"].Value = Central_dataTable.Rows[j]["Name"].ToString();
                                cmd.Parameters["@Manager"].Value = Central_dataTable.Rows[j]["Manager"].ToString();
                                cmd.Parameters["@Delegacy"].Value = Central_dataTable.Rows[j]["Delegacy"].ToString();
                                cmd.Parameters["@D_CodeMelli"].Value = Central_dataTable.Rows[j]["D_CodeMelli"].ToString();

                                //cmd.Parameters["@Name"].Value = BrokerCustomer_dataTable.Rows[j]["GoodName"].ToString();

                                cmd.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                }



                Console.WriteLine("City_dataTable");




                // استفاده از SQLiteDataAdapter برای وارد کردن داده‌های DataTable به دیتابیس SQLite
                batchSize = 1000; // Define your batch size
                rowCount = City_dataTable.Rows.Count;
                batches = (rowCount + batchSize - 1) / batchSize;

                for (int i = 0; i < batches; i++)
                {
                    int start = i * batchSize;
                    int end = Math.Min(rowCount, (i + 1) * batchSize);

                    using (SQLiteTransaction transaction = SQLite_con.BeginTransaction())
                    {
                        using (SQLiteCommand cmd = SQLite_con.CreateCommand())
                        {

                            cmd.CommandText = "Insert INTO City ( CityCode, Name )  VALUES (@CityCode,@Name )";
                            cmd.Parameters.Add("@CityCode", DbType.Int32);
                            cmd.Parameters.Add("@Name", DbType.String);

                            //cmd.Parameters.Add("@CustomerRef", DbType.Int32);
                            // cmd.Parameters.Add("@Name", DbType.String);
                            for (int j = start; j < end; j++)
                            {
                                cmd.Parameters["@CityCode"].Value = Convert.ToInt32(City_dataTable.Rows[j]["CityCode"]);
                                cmd.Parameters["@Name"].Value = City_dataTable.Rows[j]["Name"].ToString();


                                //cmd.Parameters["@Name"].Value = BrokerCustomer_dataTable.Rows[j]["GoodName"].ToString();

                                cmd.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                }


                Console.WriteLine("Customer_dataTable");



                // استفاده از SQLiteDataAdapter برای وارد کردن داده‌های DataTable به دیتابیس SQLite
                batchSize = 1000; // Define your batch size
                rowCount = Customer_dataTable.Rows.Count;
                batches = (rowCount + batchSize - 1) / batchSize;

                for (int i = 0; i < batches; i++)
                {
                    int start = i * batchSize;
                    int end = Math.Min(rowCount, (i + 1) * batchSize);

                    using (SQLiteTransaction transaction = SQLite_con.BeginTransaction())
                    {
                        using (SQLiteCommand cmd = SQLite_con.CreateCommand())
                        {

                            cmd.CommandText = "Insert INTO Customer ( CustomerCode, CentralRef, AddressRef, EtebarNaghd,EtebarCheck, Takhfif, PriceTip, Active, CustomerBestankar, CustomerBedehkar )  VALUES (@CustomerCode,@CentralRef,@AddressRef,@EtebarNaghd,@EtebarCheck,@Takhfif,@PriceTip,@Active,@CustomerBestankar,@CustomerBedehkar )";
                            cmd.Parameters.Add("@CustomerCode", DbType.Int32);
                            cmd.Parameters.Add("@CentralRef", DbType.Int32);
                            cmd.Parameters.Add("@AddressRef", DbType.Int32);
                            cmd.Parameters.Add("@EtebarNaghd", DbType.Double);
                            cmd.Parameters.Add("@EtebarCheck", DbType.Double);
                            cmd.Parameters.Add("@Takhfif", DbType.Double);
                            cmd.Parameters.Add("@PriceTip", DbType.Int32);
                            cmd.Parameters.Add("@Active", DbType.Int32);
                            cmd.Parameters.Add("@CustomerBestankar", DbType.Double);
                            cmd.Parameters.Add("@CustomerBedehkar", DbType.Double);

                            //cmd.Parameters.Add("@CustomerRef", DbType.Int32);
                            // cmd.Parameters.Add("@Name", DbType.String);
                            for (int j = start; j < end; j++)
                            {
                                //(Good_dataTable.Rows[j]["Bit3"].ToString().Length > 0) ? Convert.ToBoolean(Good_dataTable.Rows[j]["Bit3"]) : 0;

                                cmd.Parameters["@CustomerCode"].Value = (Customer_dataTable.Rows[j]["CustomerCode"].ToString().Length > 0) ? Convert.ToInt32(Customer_dataTable.Rows[j]["CustomerCode"]) : 0;
                                cmd.Parameters["@CentralRef"].Value = (Customer_dataTable.Rows[j]["CentralRef"].ToString().Length > 0) ? Convert.ToInt32(Customer_dataTable.Rows[j]["CentralRef"]) : 0;
                                cmd.Parameters["@AddressRef"].Value = (Customer_dataTable.Rows[j]["AddressRef"].ToString().Length > 0) ? Convert.ToInt32(Customer_dataTable.Rows[j]["AddressRef"]) : 0;
                                cmd.Parameters["@EtebarNaghd"].Value = (Customer_dataTable.Rows[j]["EtebarNaghd"].ToString().Length > 0) ? Convert.ToDouble(Customer_dataTable.Rows[j]["EtebarNaghd"]) : 0;
                                cmd.Parameters["@EtebarCheck"].Value = (Customer_dataTable.Rows[j]["EtebarCheck"].ToString().Length > 0) ? Convert.ToDouble(Customer_dataTable.Rows[j]["EtebarCheck"]) : 0;
                                cmd.Parameters["@Takhfif"].Value = (Customer_dataTable.Rows[j]["Takhfif"].ToString().Length > 0) ? Convert.ToDouble(Customer_dataTable.Rows[j]["Takhfif"]) : 0;
                                cmd.Parameters["@PriceTip"].Value = (Customer_dataTable.Rows[j]["PriceTip"].ToString().Length > 0) ? Convert.ToInt32(Customer_dataTable.Rows[j]["PriceTip"]) : 0;
                                cmd.Parameters["@Active"].Value = (Customer_dataTable.Rows[j]["Active"].ToString().Length > 0) ? Convert.ToInt32(Customer_dataTable.Rows[j]["Active"]) : 0;
                                cmd.Parameters["@CustomerBestankar"].Value = (Customer_dataTable.Rows[j]["CustomerBestankar"].ToString().Length > 0) ? Convert.ToDouble(Customer_dataTable.Rows[j]["CustomerBestankar"]) : 0;
                                cmd.Parameters["@CustomerBedehkar"].Value = (Customer_dataTable.Rows[j]["CustomerBedehkar"].ToString().Length > 0) ? Convert.ToDouble(Customer_dataTable.Rows[j]["CustomerBedehkar"]) : 0;
                                

                                //cmd.Parameters["@Name"].Value = BrokerCustomer_dataTable.Rows[j]["GoodName"].ToString();

                                cmd.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                }







                Console.WriteLine("GoodGroup_dataTable");



                // استفاده از SQLiteDataAdapter برای وارد کردن داده‌های DataTable به دیتابیس SQLite
                batchSize = 1000; // Define your batch size
                rowCount = GoodGroup_dataTable.Rows.Count;
                batches = (rowCount + batchSize - 1) / batchSize;

                for (int i = 0; i < batches; i++)
                {
                    int start = i * batchSize;
                    int end = Math.Min(rowCount, (i + 1) * batchSize);

                    using (SQLiteTransaction transaction = SQLite_con.BeginTransaction())
                    {
                        using (SQLiteCommand cmd = SQLite_con.CreateCommand())
                        {

                            cmd.CommandText = "Insert INTO GoodGroup ( GoodGroupCode, GoodRef, GoodGroupRef )  VALUES (@GoodGroupCode,@GoodRef,@GoodGroupRef )";
                            cmd.Parameters.Add("@GoodGroupCode", DbType.Int32);
                            cmd.Parameters.Add("@GoodRef", DbType.Int32);
                            cmd.Parameters.Add("@GoodGroupRef", DbType.Int32);


                            //cmd.Parameters.Add("@CustomerRef", DbType.Int32);
                            // cmd.Parameters.Add("@Name", DbType.String);
                            for (int j = start; j < end; j++)
                            {
                                cmd.Parameters["@GoodGroupCode"].Value = Convert.ToInt32(GoodGroup_dataTable.Rows[j]["GoodGroupCode"]);
                                cmd.Parameters["@GoodRef"].Value = Convert.ToInt32(GoodGroup_dataTable.Rows[j]["GoodRef"]);
                                cmd.Parameters["@GoodGroupRef"].Value = Convert.ToInt32(GoodGroup_dataTable.Rows[j]["GoodGroupRef"]);


                                //cmd.Parameters["@Name"].Value = BrokerCustomer_dataTable.Rows[j]["GoodName"].ToString();

                                cmd.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                }







                Console.WriteLine("GoodsGrp_dataTable");

                // استفاده از SQLiteDataAdapter برای وارد کردن داده‌های DataTable به دیتابیس SQLite
                batchSize = 1000; // Define your batch size
                rowCount = GoodsGrp_dataTable.Rows.Count;
                batches = (rowCount + batchSize - 1) / batchSize;

                for (int i = 0; i < batches; i++)
                {
                    int start = i * batchSize;
                    int end = Math.Min(rowCount, (i + 1) * batchSize);

                    using (SQLiteTransaction transaction = SQLite_con.BeginTransaction())
                    {
                        using (SQLiteCommand cmd = SQLite_con.CreateCommand())
                        {

                            cmd.CommandText = "Insert INTO GoodsGrp (GroupCode, L1, L2, L3, L4, L5, Name  )  VALUES (@GroupCode,@L1,@L2,@L3,@L4,@L5,@Name )";
                            cmd.Parameters.Add("@GroupCode", DbType.Int32);
                            cmd.Parameters.Add("@L1", DbType.Int32);
                            cmd.Parameters.Add("@L2", DbType.Int32);
                            cmd.Parameters.Add("@L3", DbType.Int32);
                            cmd.Parameters.Add("@L4", DbType.Int32);
                            cmd.Parameters.Add("@L5", DbType.Int32);
                            cmd.Parameters.Add("@Name", DbType.String);

                            //cmd.Parameters.Add("@CustomerRef", DbType.Int32);
                            // cmd.Parameters.Add("@Name", DbType.String);
                            for (int j = start; j < end; j++)
                            {
                                cmd.Parameters["@GroupCode"].Value = (GoodsGrp_dataTable.Rows[j]["GroupCode"].ToString().Length > 0) ? Convert.ToInt32(GoodsGrp_dataTable.Rows[j]["GroupCode"]) : 0;
                                cmd.Parameters["@L1"].Value = (GoodsGrp_dataTable.Rows[j]["L1"].ToString().Length > 0) ? Convert.ToInt32(GoodsGrp_dataTable.Rows[j]["L1"]) : 0;
                                cmd.Parameters["@L2"].Value = (GoodsGrp_dataTable.Rows[j]["L2"].ToString().Length > 0) ? Convert.ToInt32(GoodsGrp_dataTable.Rows[j]["L2"]) : 0;
                                cmd.Parameters["@L3"].Value = (GoodsGrp_dataTable.Rows[j]["L3"].ToString().Length > 0) ? Convert.ToInt32(GoodsGrp_dataTable.Rows[j]["L3"]) : 0;
                                cmd.Parameters["@L4"].Value = (GoodsGrp_dataTable.Rows[j]["L4"].ToString().Length > 0) ? Convert.ToInt32(GoodsGrp_dataTable.Rows[j]["L4"]) : 0;
                                cmd.Parameters["@L5"].Value = (GoodsGrp_dataTable.Rows[j]["L5"].ToString().Length > 0) ? Convert.ToInt32(GoodsGrp_dataTable.Rows[j]["L5"]) : 0;
                                cmd.Parameters["@Name"].Value = (GoodsGrp_dataTable.Rows[j]["Name"].ToString().Length > 0) ? GoodsGrp_dataTable.Rows[j]["Name"].ToString() : "";


                                //cmd.Parameters["@Name"].Value = BrokerCustomer_dataTable.Rows[j]["GoodName"].ToString();

                                cmd.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                }







                Console.WriteLine("Goodstack_dataTable");


                // استفاده از SQLiteDataAdapter برای وارد کردن داده‌های DataTable به دیتابیس SQLite
                batchSize = 1000; // Define your batch size
                rowCount = Goodstack_dataTable.Rows.Count;
                batches = (rowCount + batchSize - 1) / batchSize;

                for (int i = 0; i < batches; i++)
                {
                    int start = i * batchSize;
                    int end = Math.Min(rowCount, (i + 1) * batchSize);

                    using (SQLiteTransaction transaction = SQLite_con.BeginTransaction())
                    {
                        using (SQLiteCommand cmd = SQLite_con.CreateCommand())
                        {

                            cmd.CommandText = "Insert INTO Goodstack (GoodStackCode, GoodRef, StackRef ,Amount,ReservedAmount,ActiveStack  )  VALUES (@GoodStackCode,@GoodRef,@StackRef ,@Amount,@ReservedAmount,@ActiveStack )";
                            cmd.Parameters.Add("@GoodStackCode", DbType.Int32);
                            cmd.Parameters.Add("@GoodRef", DbType.Int32);
                            cmd.Parameters.Add("@StackRef", DbType.Int32);
                            cmd.Parameters.Add("@Amount", DbType.Double);
                            cmd.Parameters.Add("@ReservedAmount", DbType.Double);
                            cmd.Parameters.Add("@ActiveStack", DbType.Boolean);

                            //cmd.Parameters.Add("@CustomerRef", DbType.Int32);
                            // cmd.Parameters.Add("@Name", DbType.String);
                            for (int j = start; j < end; j++)
                            {
                                cmd.Parameters["@GoodStackCode"].Value = (Goodstack_dataTable.Rows[j]["GoodStackCode"].ToString().Length > 0) ? Convert.ToInt32(Goodstack_dataTable.Rows[j]["GoodStackCode"]) : 0;
                                cmd.Parameters["@GoodRef"].Value = (Goodstack_dataTable.Rows[j]["GoodRef"].ToString().Length > 0) ? Convert.ToInt32(Goodstack_dataTable.Rows[j]["GoodRef"]) : 0;
                                cmd.Parameters["@StackRef"].Value = (Goodstack_dataTable.Rows[j]["StackRef"].ToString().Length > 0) ? Convert.ToInt32(Goodstack_dataTable.Rows[j]["StackRef"]) : 0;
                                cmd.Parameters["@Amount"].Value = (Goodstack_dataTable.Rows[j]["Amount"].ToString().Length > 0) ? Convert.ToDouble(Goodstack_dataTable.Rows[j]["Amount"]) : 0;
                                cmd.Parameters["@ReservedAmount"].Value = (Goodstack_dataTable.Rows[j]["ReservedAmount"].ToString().Length > 0) ? Convert.ToDouble(Goodstack_dataTable.Rows[j]["ReservedAmount"]) : 0;
                                cmd.Parameters["@ActiveStack"].Value = (Goodstack_dataTable.Rows[j]["ActiveStack"].ToString().Length > 0) ? Convert.ToBoolean(Goodstack_dataTable.Rows[j]["ActiveStack"]) : 0;


                                //cmd.Parameters["@Name"].Value = BrokerCustomer_dataTable.Rows[j]["GoodName"].ToString();

                                cmd.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                }








                Console.WriteLine("Job_dataTable");


                // استفاده از SQLiteDataAdapter برای وارد کردن داده‌های DataTable به دیتابیس SQLite
                batchSize = 1000; // Define your batch size
                rowCount = Job_dataTable.Rows.Count;
                batches = (rowCount + batchSize - 1) / batchSize;

                for (int i = 0; i < batches; i++)
                {
                    int start = i * batchSize;
                    int end = Math.Min(rowCount, (i + 1) * batchSize);

                    using (SQLiteTransaction transaction = SQLite_con.BeginTransaction())
                    {
                        using (SQLiteCommand cmd = SQLite_con.CreateCommand())
                        {

                            cmd.CommandText = "Insert INTO Job (JobCode,Title,Explain )  VALUES (@JobCode,@Title,@Explain)";
                            cmd.Parameters.Add("@JobCode", DbType.Int32);
                            cmd.Parameters.Add("@Title", DbType.String);
                            cmd.Parameters.Add("@Explain", DbType.String);


                            //cmd.Parameters.Add("@CustomerRef", DbType.Int32);
                            // cmd.Parameters.Add("@Name", DbType.String);
                            for (int j = start; j < end; j++)
                            {

                                cmd.Parameters["@JobCode"].Value = (Job_dataTable.Rows[j]["JobCode"].ToString().Length > 0) ? Convert.ToInt32(Job_dataTable.Rows[j]["JobCode"]) : 0;
                                cmd.Parameters["@Title"].Value = (Job_dataTable.Rows[j]["Title"].ToString().Length > 0) ? Job_dataTable.Rows[j]["Title"].ToString() : "";
                                cmd.Parameters["@Explain"].Value = (Job_dataTable.Rows[j]["Explain"].ToString().Length > 0) ? Job_dataTable.Rows[j]["Explain"].ToString() : "";


                                //cmd.Parameters["@Name"].Value = BrokerCustomer_dataTable.Rows[j]["GoodName"].ToString();

                                cmd.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                }





                Console.WriteLine("JobPerson_dataTable");

                // استفاده از SQLiteDataAdapter برای وارد کردن داده‌های DataTable به دیتابیس SQLite
                batchSize = 1000; // Define your batch size
                rowCount = JobPerson_dataTable.Rows.Count;
                batches = (rowCount + batchSize - 1) / batchSize;

                for (int i = 0; i < batches; i++)
                {
                    int start = i * batchSize;
                    int end = Math.Min(rowCount, (i + 1) * batchSize);

                    using (SQLiteTransaction transaction = SQLite_con.BeginTransaction())
                    {
                        using (SQLiteCommand cmd = SQLite_con.CreateCommand())
                        {

                            cmd.CommandText = "Insert INTO JobPerson (JobPersonCode,JobPersonPrivateCode,JobRef,CentralRef,AddressRef,PersonAlias )  VALUES (@JobPersonCode,@JobPersonPrivateCode,@JobRef,@CentralRef,@AddressRef,@PersonAlias )";
                            cmd.Parameters.Add("@JobPersonCode", DbType.Int32);
                            cmd.Parameters.Add("@JobPersonPrivateCode", DbType.Int32);
                            cmd.Parameters.Add("@JobRef", DbType.Int32);
                            cmd.Parameters.Add("@CentralRef", DbType.Int32);
                            cmd.Parameters.Add("@AddressRef", DbType.Int32);
                           
                            cmd.Parameters.Add("@PersonAlias", DbType.String);

                            //cmd.Parameters.Add("@CustomerRef", DbType.Int32);
                            // cmd.Parameters.Add("@Name", DbType.String);
                            for (int j = start; j < end; j++)
                            {
                                cmd.Parameters["@JobPersonCode"].Value = (JobPerson_dataTable.Rows[j]["JobPersonCode"].ToString().Length > 0) ? Convert.ToInt32(JobPerson_dataTable.Rows[j]["JobPersonCode"]) : 0;
                                cmd.Parameters["@JobPersonPrivateCode"].Value = (JobPerson_dataTable.Rows[j]["JobPersonPrivateCode"].ToString().Length > 0) ? Convert.ToInt32(JobPerson_dataTable.Rows[j]["JobPersonPrivateCode"]) : 0;
                                cmd.Parameters["@JobRef"].Value = (JobPerson_dataTable.Rows[j]["JobRef"].ToString().Length > 0) ? Convert.ToInt32(JobPerson_dataTable.Rows[j]["JobRef"]) : 0;
                                cmd.Parameters["@CentralRef"].Value = (JobPerson_dataTable.Rows[j]["CentralRef"].ToString().Length > 0) ? Convert.ToInt32(JobPerson_dataTable.Rows[j]["CentralRef"]) : 0;
                                cmd.Parameters["@AddressRef"].Value = (JobPerson_dataTable.Rows[j]["AddressRef"].ToString().Length > 0) ? Convert.ToInt32(JobPerson_dataTable.Rows[j]["AddressRef"]) : 0;
                               
                                cmd.Parameters["@PersonAlias"].Value = (JobPerson_dataTable.Rows[j]["PersonAlias"].ToString().Length > 0) ? JobPerson_dataTable.Rows[j]["PersonAlias"].ToString() :"";



                                //cmd.Parameters["@Name"].Value = BrokerCustomer_dataTable.Rows[j]["GoodName"].ToString();

                                cmd.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                }




                Console.WriteLine("JobPerson_Good_dataTable");

                // استفاده از SQLiteDataAdapter برای وارد کردن داده‌های DataTable به دیتابیس SQLite
                batchSize = 1000; // Define your batch size
                rowCount = JobPerson_Good_dataTable.Rows.Count;
                batches = (rowCount + batchSize - 1) / batchSize;

                for (int i = 0; i < batches; i++)
                {
                    int start = i * batchSize;
                    int end = Math.Min(rowCount, (i + 1) * batchSize);

                    using (SQLiteTransaction transaction = SQLite_con.BeginTransaction())
                    {
                        using (SQLiteCommand cmd = SQLite_con.CreateCommand())
                        {

                            cmd.CommandText = "Insert INTO JobPerson_Good (JobPerson_GoodCode,JobPersonRef,GoodRef )  VALUES (@JobPerson_GoodCode,@JobPersonRef,@GoodRef)";
                            cmd.Parameters.Add("@JobPerson_GoodCode", DbType.Int32);
                            cmd.Parameters.Add("@JobPersonRef", DbType.Int32);
                            cmd.Parameters.Add("@GoodRef", DbType.Int32);


                            //cmd.Parameters.Add("@CustomerRef", DbType.Int32);
                            // cmd.Parameters.Add("@Name", DbType.String);
                            for (int j = start; j < end; j++)
                            {
                                cmd.Parameters["@JobPerson_GoodCode"].Value = (JobPerson_Good_dataTable.Rows[j]["JobPerson_GoodCode"].ToString().Length > 0) ? Convert.ToInt32(JobPerson_Good_dataTable.Rows[j]["JobPerson_GoodCode"]) : 0;
                                cmd.Parameters["@JobPersonRef"].Value = (JobPerson_Good_dataTable.Rows[j]["JobPersonRef"].ToString().Length > 0) ? Convert.ToInt32(JobPerson_Good_dataTable.Rows[j]["JobPersonRef"]) : 0;
                                cmd.Parameters["@GoodRef"].Value = (JobPerson_Good_dataTable.Rows[j]["GoodRef"].ToString().Length > 0) ? Convert.ToInt32(JobPerson_Good_dataTable.Rows[j]["GoodRef"]) : 0;


                                //cmd.Parameters["@Name"].Value = BrokerCustomer_dataTable.Rows[j]["GoodName"].ToString();

                                cmd.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                }















                Console.WriteLine("KsrImage_dataTable");


                // استفاده از SQLiteDataAdapter برای وارد کردن داده‌های DataTable به دیتابیس SQLite
                batchSize = 1000; // Define your batch size
                rowCount = KsrImage_dataTable.Rows.Count;
                batches = (rowCount + batchSize - 1) / batchSize;

                for (int i = 0; i < batches; i++)
                {
                    int start = i * batchSize;
                    int end = Math.Min(rowCount, (i + 1) * batchSize);

                    using (SQLiteTransaction transaction = SQLite_con.BeginTransaction())
                    {
                        using (SQLiteCommand cmd = SQLite_con.CreateCommand())
                        {

                            cmd.CommandText = "Insert INTO KsrImage (KsrImageCode,ObjectRef,IsDefaultImage  )  VALUES (@KsrImageCode,@ObjectRef,@IsDefaultImage )";
                            cmd.Parameters.Add("@KsrImageCode", DbType.Int32);
                            cmd.Parameters.Add("@ObjectRef", DbType.Int32);
                            cmd.Parameters.Add("@IsDefaultImage", DbType.Boolean);

                            //cmd.Parameters.Add("@CustomerRef", DbType.Int32);
                            // cmd.Parameters.Add("@Name", DbType.String);
                            for (int j = start; j < end; j++)
                            {
                                cmd.Parameters["@KsrImageCode"].Value = Convert.ToInt32(KsrImage_dataTable.Rows[j]["KsrImageCode"]);
                                cmd.Parameters["@ObjectRef"].Value = Convert.ToInt32(KsrImage_dataTable.Rows[j]["ObjectRef"]);
                                cmd.Parameters["@IsDefaultImage"].Value = Convert.ToBoolean(KsrImage_dataTable.Rows[j]["IsDefaultImage"]);


                                //cmd.Parameters["@Name"].Value = BrokerCustomer_dataTable.Rows[j]["GoodName"].ToString();

                                cmd.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                }









                Console.WriteLine("ReplicationTable_dataTable");



                // استفاده از SQLiteDataAdapter برای وارد کردن داده‌های DataTable به دیتابیس SQLite
                batchSize = 1000; // Define your batch size
                rowCount = ReplicationTable_dataTable.Rows.Count;
                batches = (rowCount + batchSize - 1) / batchSize;

                for (int i = 0; i < batches; i++)
                {
                    int start = i * batchSize;
                    int end = Math.Min(rowCount, (i + 1) * batchSize);

                    using (SQLiteTransaction transaction = SQLite_con.BeginTransaction())
                    {
                        using (SQLiteCommand cmd = SQLite_con.CreateCommand())
                        {

                            cmd.CommandText = "Insert INTO ReplicationTable (ReplicationCode,ServerTable,ClientTable,ServerPrimaryKey,ClientPrimaryKey,Condition,ConditionDelete,LastRepLogCode,LastRepLogCodeDelete )  VALUES (@ReplicationCode,@ServerTable,@ClientTable,@ServerPrimaryKey,@ClientPrimaryKey,@Condition,@ConditionDelete,@LastRepLogCode,@LastRepLogCodeDelete )";
                            cmd.Parameters.Add("@ReplicationCode", DbType.Int32);
                            cmd.Parameters.Add("@ServerTable", DbType.String);
                            cmd.Parameters.Add("@ClientTable", DbType.String);
                            cmd.Parameters.Add("@ServerPrimaryKey", DbType.String);
                            cmd.Parameters.Add("@ClientPrimaryKey", DbType.String);
                            cmd.Parameters.Add("@Condition", DbType.String);
                            cmd.Parameters.Add("@ConditionDelete", DbType.String);
                            cmd.Parameters.Add("@LastRepLogCode", DbType.Int32);
                            cmd.Parameters.Add("@LastRepLogCodeDelete", DbType.Int32);

                            //cmd.Parameters.Add("@CustomerRef", DbType.Int32);
                            // cmd.Parameters.Add("@Name", DbType.String);
                            for (int j = start; j < end; j++)
                            {
                                cmd.Parameters["@ReplicationCode"].Value = Convert.ToInt32(ReplicationTable_dataTable.Rows[j]["ReplicationCode"]);
                                cmd.Parameters["@ServerTable"].Value = ReplicationTable_dataTable.Rows[j]["ServerTable"].ToString();
                                cmd.Parameters["@ClientTable"].Value = ReplicationTable_dataTable.Rows[j]["ClientTable"].ToString();
                                cmd.Parameters["@ServerPrimaryKey"].Value = ReplicationTable_dataTable.Rows[j]["ServerPrimaryKey"].ToString();
                                cmd.Parameters["@ClientPrimaryKey"].Value = ReplicationTable_dataTable.Rows[j]["ClientPrimaryKey"].ToString();
                                cmd.Parameters["@Condition"].Value = ReplicationTable_dataTable.Rows[j]["Condition"].ToString();
                                cmd.Parameters["@ConditionDelete"].Value = ReplicationTable_dataTable.Rows[j]["ConditionDelete"].ToString();
                                cmd.Parameters["@LastRepLogCode"].Value = Convert.ToInt32(ReplicationTable_dataTable.Rows[j]["LastRepLogCode"]);
                                cmd.Parameters["@LastRepLogCodeDelete"].Value = Convert.ToInt32(ReplicationTable_dataTable.Rows[j]["LastRepLogCodeDelete"]);

                                //cmd.Parameters["@Name"].Value = BrokerCustomer_dataTable.Rows[j]["GoodName"].ToString();

                                cmd.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                }















                Console.WriteLine("Units_dataTable");

                // استفاده از SQLiteDataAdapter برای وارد کردن داده‌های DataTable به دیتابیس SQLite
                batchSize = 1000; // Define your batch size
                rowCount = Units_dataTable.Rows.Count;
                batches = (rowCount + batchSize - 1) / batchSize;

                for (int i = 0; i < batches; i++)
                {
                    int start = i * batchSize;
                    int end = Math.Min(rowCount, (i + 1) * batchSize);

                    using (SQLiteTransaction transaction = SQLite_con.BeginTransaction())
                    {
                        using (SQLiteCommand cmd = SQLite_con.CreateCommand())
                        {

                            cmd.CommandText = "Insert INTO Units (UnitCode, UnitName )  VALUES (@UnitCode,@UnitName )";
                            cmd.Parameters.Add("@UnitCode", DbType.Int32);
                            cmd.Parameters.Add("@UnitName", DbType.String);

                            //cmd.Parameters.Add("@CustomerRef", DbType.Int32);
                            // cmd.Parameters.Add("@Name", DbType.String);
                            for (int j = start; j < end; j++)
                            {
                                cmd.Parameters["@UnitCode"].Value = Convert.ToInt32(Units_dataTable.Rows[j]["UnitCode"]);
                                cmd.Parameters["@UnitName"].Value = Units_dataTable.Rows[j]["UnitName"].ToString();


                                //cmd.Parameters["@Name"].Value = BrokerCustomer_dataTable.Rows[j]["GoodName"].ToString();

                                cmd.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                }







                Console.WriteLine("Address_dataTable");


                batchSize = 1000; // Define your batch size
                rowCount = Address_dataTable.Rows.Count;
                batches = (rowCount + batchSize - 1) / batchSize;

                for (int i = 0; i < batches; i++)
                {
                    int start = i * batchSize;
                    int end = Math.Min(rowCount, (i + 1) * batchSize);

                    using (SQLiteTransaction transaction = SQLite_con.BeginTransaction())
                    {
                        using (SQLiteCommand cmd = SQLite_con.CreateCommand())
                        {
                            cmd.CommandText = "INSERT INTO Address (AddressCode, CentralRef, CityCode, Address, Phone, Mobile, MobileName, Email, Fax, ZipCode, PostCode) VALUES (@AddressCode, @CentralRef, @CityCode, @Address, @Phone, @Mobile, @MobileName, @Email, @Fax, @ZipCode, @PostCode)";
                            cmd.Parameters.Add("@AddressCode", DbType.Int32);
                            cmd.Parameters.Add("@CentralRef", DbType.Int32);
                            cmd.Parameters.Add("@CityCode", DbType.Int32);
                            cmd.Parameters.Add("@Address", DbType.String);
                            cmd.Parameters.Add("@Phone", DbType.String);
                            cmd.Parameters.Add("@Mobile", DbType.String);
                            cmd.Parameters.Add("@MobileName", DbType.String);
                            cmd.Parameters.Add("@Email", DbType.String);
                            cmd.Parameters.Add("@Fax", DbType.String);
                            cmd.Parameters.Add("@ZipCode", DbType.String);
                            cmd.Parameters.Add("@PostCode", DbType.String);

                            for (int j = start; j < end; j++)
                            {

                                cmd.Parameters["@AddressCode"].Value = (Address_dataTable.Rows[j]["AddressCode"].ToString().Length > 0) ? Convert.ToInt32(Address_dataTable.Rows[j]["AddressCode"]) : 0;
                                cmd.Parameters["@CentralRef"].Value = (Address_dataTable.Rows[j]["CentralRef"].ToString().Length > 0) ? Convert.ToInt32(Address_dataTable.Rows[j]["CentralRef"]) : 0;
                                cmd.Parameters["@CityCode"].Value = (Address_dataTable.Rows[j]["CityCode"].ToString().Length > 0) ? Convert.ToInt32(Address_dataTable.Rows[j]["CityCode"]) : 0;
                                cmd.Parameters["@Address"].Value = (Address_dataTable.Rows[j]["Address"].ToString().Length > 0) ? Address_dataTable.Rows[j]["Address"].ToString() : "";
                                cmd.Parameters["@Phone"].Value = (Address_dataTable.Rows[j]["Phone"].ToString().Length > 0) ? Address_dataTable.Rows[j]["Phone"].ToString() : "";
                                cmd.Parameters["@Mobile"].Value = (Address_dataTable.Rows[j]["Mobile"].ToString().Length > 0) ? Address_dataTable.Rows[j]["Mobile"].ToString() : "";
                                cmd.Parameters["@MobileName"].Value = (Address_dataTable.Rows[j]["MobileName"].ToString().Length > 0) ? Address_dataTable.Rows[j]["MobileName"].ToString() : "";
                                cmd.Parameters["@Email"].Value = (Address_dataTable.Rows[j]["Email"].ToString().Length > 0) ? Address_dataTable.Rows[j]["Email"].ToString() : "";
                                cmd.Parameters["@Fax"].Value = (Address_dataTable.Rows[j]["Fax"].ToString().Length > 0) ? Address_dataTable.Rows[j]["Fax"].ToString() : "";
                                cmd.Parameters["@ZipCode"].Value = (Address_dataTable.Rows[j]["ZipCode"].ToString().Length > 0) ? Address_dataTable.Rows[j]["ZipCode"].ToString() : "";
                                cmd.Parameters["@PostCode"].Value = (Address_dataTable.Rows[j]["PostCode"].ToString().Length > 0) ? Address_dataTable.Rows[j]["PostCode"].ToString() : "";

                                cmd.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                }



                SQLite_con.Close();
             }
               
            
                return "Done";

        } 






}

}


