using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PoemGenerator.OntologyLoader.Model.Abstractions;

namespace PoemGenerator.OntologyLoader.Model.Collections
{
    public class RelationCollection: IReadOnlyRelationCollection
    {
        public List<IReadOnlyRelation> Relations { get; }

        public RelationCollection()
        {
            Relations = new List<IReadOnlyRelation>();
        }
        
        public RelationCollection(IEnumerable<IReadOnlyRelation> relations)
        {
            Relations = relations.ToList();
        }
        
        public IEnumerator<IReadOnlyRelation> GetEnumerator()
        {
            return Relations.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => Relations.Count;

        public IReadOnlyNodeCollection this[string name] =>
            new NodeCollection(Relations.Where(x => x.To.Name == name).Select(x => x.From));

        public override string ToString()
        {
            return $"Count = {Count}";
        }
    }
}