using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Saxx.LingoHubClient.Exceptions;

namespace Saxx.LingoHubClient.Tests
{
    public class ProjectTests : TestBase
    {
        [Test]
        public async Task Load_Projects()
        {
            var projects = (await Client.GetProjects()).ToList();

            Assert.Greater(projects.Count(), 0);
            Assert.IsTrue(projects.Any(x => x.Title == Configuration.ProjectTitle));
        }


        [Test]
        public async Task Load_Project_Details()
        {
            var projectDetails = await Client.GetProjectDetails(Configuration.ProjectTitle);

            Assert.AreEqual(Configuration.ProjectTitle, projectDetails.Title);
            Assert.IsTrue(projectDetails.ProjectLocales.Contains("en"));
        }


        [Test]
        [ExpectedException(typeof (ProjectDoesNotExistException))]
        public async Task Invalid_Project_Throws_Exception()
        {
            await Client.GetProjectDetails("Some_Random_Name");
        }
    }
}