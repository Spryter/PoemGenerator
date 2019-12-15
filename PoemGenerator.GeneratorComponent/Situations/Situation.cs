using PoemGenerator.OntologyModel.Abstractions;

namespace PoemGenerator.GeneratorComponent.Situations
{
    public abstract class Situation
    {
        public IReadOnlyNode Action { get; set; }
        
        public IReadOnlyNode Object { get; set; }
        
        public IReadOnlyNode Locative { get; set; }

        public abstract IReadOnlyNodeCollection GetNodes();

        public abstract IReadOnlyRelationCollection GetRelations();
    }
}