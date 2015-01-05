using System;

namespace Saxx.LingoHubClient.Exceptions
{
    public class ProjectDoesNotExistException : Exception
    {
        public ProjectDoesNotExistException(string projectTitle)
            : base("A project '" + projectTitle + "' does not exist.")
        {
        }
    }
}
