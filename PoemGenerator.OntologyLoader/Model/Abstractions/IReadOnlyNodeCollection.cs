using System.Collections.Generic;

namespace PoemGenerator.OntologyLoader.Model.Abstractions
{
    public interface IReadOnlyNodeCollection: IReadOnlyCollection<IReadOnlyNode>
    {
        IReadOnlyNode this[string name] { get; }
        
        IReadOnlyNode this[int id] { get; }
    }
}