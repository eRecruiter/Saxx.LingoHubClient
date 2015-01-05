using NUnit.Framework;

namespace Saxx.LingoHubClient.Tests
{
    public class TestBase
    {
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            Client = new LingoHubClient(Configuration.Username, Configuration.Password);
        }


        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            Client.Dispose();
        }


        public LingoHubClient Client { get; set; }
    }
}