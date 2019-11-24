using System;

namespace PoemGenerator.OntolisAdapter.Exceptions
{
    public class OntologyPathNotInitializedException : Exception
    {
        private const string ErrorMessage = "Невозможно открыть онтологию, путь не проинициализирован.";

        public OntologyPathNotInitializedException(): base(ErrorMessage)
        {

        }
    }
}
