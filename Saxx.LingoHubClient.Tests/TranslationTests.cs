using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Saxx.LingoHubClient.Tests
{
    public class TranslationTests : TestBase
    {
        [Test]
        public async Task Load_Translations()
        {
            var translations = (await Client.GetTranslations(Configuration.ProjectTitle)).ToList();

            Assert.AreEqual(2, translations.Count());
            Assert.IsTrue(translations.Any(x => x.Title == "Key_One"));
            Assert.IsTrue(translations.Any(x => x.Title == "Key_Two"));
        }
    }
}