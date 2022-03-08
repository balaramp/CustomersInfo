using System.Data.SqlClient;
namespace CustomersInfo
{
    public class CustomerInfo
    {
        Dictionary<int, CustomerPackageInfo> Customers = new Dictionary<int, CustomerPackageInfo>();

        public void Init()
        {
            string ConnectionString =
                System.Configuration.ConfigurationManager.ConnectionStrings["CustomersDb"].ConnectionString;

            var conn = new SqlConnection(ConnectionString);
            conn.Open();

            string sql = "Select Customers.CustomerId, Customers.CustomerName,  OrderType, EligibleToWin, Won, Day, Sale from Customers, Orders where Customers.CustomerId = Orders.CustomerId";
            SqlCommand cmd = new SqlCommand(sql, conn);

            SqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                CustomerPackageInfo packageInfo = new CustomerPackageInfo();
                packageInfo.CustomerId = Convert.ToInt32(dataReader.GetValue(0));
                packageInfo.Name = dataReader.GetValue(1).ToString();
                packageInfo.Type = (CustomerPackageType)Convert.ToInt32(dataReader.GetValue(2));
                packageInfo.EligableToWin = (bool)dataReader.GetValue(3);
                packageInfo.Won = (bool)dataReader.GetValue(4);
                packageInfo.Day = (DateTime)dataReader.GetValue(5);
                packageInfo.Sale = Convert.ToInt32(dataReader.GetValue(6));
                Customers.Add(packageInfo.CustomerId, packageInfo);
            }
        }

        public CustomerPackageInfo? GetCustomerWinInfo(int customerId)
        {
            if (Customers.ContainsKey(customerId))
            {
                CustomerPackageInfo customerWinInfo = Customers[customerId];
                return customerWinInfo;
            }
            else
            {
                return null;
            }
        }

        public Dictionary<int, CustomerPackageInfo> GetCustomers()
        {
            return Customers;
        }
    }

    public enum CustomerPackageType
    {
        prints = 1,
        panoramas,
        strips
    }
    public class CustomerPackageInfo
    {
        public int CustomerId { get; set; }

        public string? Name { get; set; }
        public CustomerPackageType Type { get; set; }
        public bool EligableToWin { get; set; }
        public bool Won { get; set; }
        public DateTime Day { get; set; }
        public int Sale { get; set; }
    }
}