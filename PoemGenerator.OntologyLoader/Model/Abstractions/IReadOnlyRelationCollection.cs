using System.Collections.Generic;

namespace PoemGenerator.OntologyLoader.Model.Abstractions
{
    public interface IReadOnlyRelationCollection: IReadOnlyCollection<IReadOnlyRelation>
    {
        IReadOnlyNodeCollection this[string name] { get; }
    }
}