using PoemGenerator.OntologyModel;

namespace PoemGenerator.GeneratorComponent
{
    public class EmptyOntologyNode: OntologyNode
    {
        public new const int Id = -1;

        public new const string Name = "пусто";
        
        public EmptyOntologyNode() : base(Id, Name)
        {
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}