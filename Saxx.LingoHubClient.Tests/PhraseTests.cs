using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Saxx.LingoHubClient.Tests
{
    public class PhraseTests : TestBase
    {
        [Test]
        public async Task Load_Phrases()
        {
            var phrases = (await Client.GetPhrases(Configuration.ProjectTitle, "Key_One")).ToList();

            Assert.AreEqual(3, phrases.Count());
            Assert.IsTrue(phrases.Any(x => x.Locale == "en" && x.Content == "Value of Key One"));
            Assert.IsTrue(phrases.Any(x => x.Locale == "es" && x.Content == null));
        }
    }
}