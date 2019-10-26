namespace PoemGenerator.OntologyLoader.Model.Abstractions
{
    public interface IReadOnlyNode
    {
        int Id { get; }
        
        string Name { get; }
        
        /// <summary>
        /// Связи, которые идут в узел.
        /// </summary>
        IReadOnlyRelationCollection ToRelations { get; }
        
        /// <summary>
        /// Связи, которые исходят из узла.
        /// </summary>
        IReadOnlyRelationCollection FromRelations { get; }
    }
}