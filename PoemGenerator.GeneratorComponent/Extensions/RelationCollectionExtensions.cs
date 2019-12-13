using System.Collections.Generic;
using PoemGenerator.OntologyModel.Abstractions;
using PoemGenerator.OntologyModel.Collections;

namespace PoemGenerator.GeneratorComponent.Extensions
{
    public static class RelationCollectionExtensions
    {
        public static IReadOnlyRelationCollection ToRelationCollection(this IEnumerable<IReadOnlyRelation> relations)
        {
            return new RelationCollection(relations);
        }
    }
}