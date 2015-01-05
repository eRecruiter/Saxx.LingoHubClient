using System;

namespace Saxx.LingoHubClient.Exceptions
{
    public class TranslationDoesNotExistException : Exception
    {
        public TranslationDoesNotExistException(string projectTitle, string translationTitle)
            : base("A translation '" + translationTitle + "' does not exist in project '" + projectTitle + "'.")
        {
        }
    }
}