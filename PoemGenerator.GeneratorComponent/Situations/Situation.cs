using PoemGenerator.OntologyModel.Abstractions;

namespace PoemGenerator.GeneratorComponent.Situations
{
    public abstract class Situation
    {
        public IReadOnlyNode Agent { get; set; } = new EmptyOntologyNode();
        
        public IReadOnlyNode Action { get; set; } = new EmptyOntologyNode();
        
        public IReadOnlyNode Object { get; set; } = new EmptyOntologyNode();
        
        public IReadOnlyNode Locative { get; set; } = new EmptyOntologyNode();

        public abstract IReadOnlyNodeCollection GetNodes();

        public abstract IReadOnlyRelationCollection GetRelations();
    }
}