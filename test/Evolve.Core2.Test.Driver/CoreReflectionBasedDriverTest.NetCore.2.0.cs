using System.Data;
using Evolve.Driver;
using Evolve.Test.Utilities;
using Xunit;

namespace Evolve.Core2.Test.Driver
{
    [Collection("Database collection")]
    public class CoreReflectionBasedDriverTest
    {
        private readonly MySQLFixture _mySqlfixture;
        private readonly PostgreSqlFixture _pgFixture;
        private readonly SQLServerFixture _sqlServerFixture;

        public CoreReflectionBasedDriverTest(MySQLFixture mySqlfixture, PostgreSqlFixture pgFixture, SQLServerFixture sqlServerFixture)
        {
            _mySqlfixture = mySqlfixture;
            _pgFixture = pgFixture;
            _sqlServerFixture = sqlServerFixture;

            if (!TestContext.AppVeyor)
            { // AppVeyor and Windows 2016 does not support linux docker images
                _mySqlfixture.Start();
                _pgFixture.Start();
                _sqlServerFixture.Start();
            }
        }

        [Fact(DisplayName = "MicrosoftDataSqliteDriver_NET_Core_2_0_works")]
        public void MicrosoftDataSqliteDriver_NET_Core_2_0_works()
        {
            var driver = new CoreMicrosoftDataSqliteDriver(TestContext.NetCore20DepsFile, TestContext.NugetPackageFolder);
            var cnn = driver.CreateConnection("Data Source=:memory:");
            cnn.Open();

            Assert.True(cnn.State == ConnectionState.Open);
        }

        [Fact(DisplayName = "NpgsqlDriver_NET_Core_2_0_works")]
        public void NpgsqlDriver_NET_Core_2_0_works()
        {
            
            var driver = new CoreNpgsqlDriver(TestContext.NetCore20DepsFile, TestContext.NugetPackageFolder);
            var cnn = driver.CreateConnection($"Server=127.0.0.1;Port={_pgFixture.HostPort};Database={_pgFixture.DbName};User Id={_pgFixture.DbUser};Password={_pgFixture.DbPwd};");
            cnn.Open();

            Assert.True(cnn.State == ConnectionState.Open);
        }

        [Fact(DisplayName = "MySqlDriver_NET_Core_2_0_works")]
        public void MySqlDriver_NET_Core_2_0_works()
        {

            var driver = new CoreMySqlDataDriver(TestContext.NetCore20DepsFile, TestContext.NugetPackageFolder);
            var cnn = driver.CreateConnection($"Server=127.0.0.1;Port={_mySqlfixture.HostPort};Database={_mySqlfixture.DbName};Uid={_mySqlfixture.DbUser};Pwd={_mySqlfixture.DbPwd};SslMode=none;");
            cnn.Open();

            Assert.True(cnn.State == ConnectionState.Open);
        }

        [Fact(DisplayName = "SqlClientDriver_NET_Core_2_0_works")]
        public void SqlClientDriver_NET_Core_2_0_works()
        {
            var driver = new CoreSqlClientDriver(TestContext.NetCore20DepsFile, TestContext.NugetPackageFolder);
            var cnn = driver.CreateConnection($"Server=127.0.0.1;Database=master;User Id={_sqlServerFixture.DbUser};Password={_sqlServerFixture.DbPwd};");
            cnn.Open();

            Assert.True(cnn.State == ConnectionState.Open);
        }
    }
}
