namespace PoemGenerator.OntologyModel.Abstractions
{
    public interface IReadOnlyRelation
    { 
        string Name { get; }
        
        IReadOnlyNode From { get; }
        
        IReadOnlyNode To { get; }
    }
}