using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PoemGenerator.OntologyModel.Abstractions;

namespace PoemGenerator.OntologyModel.Collections
{
    public class NodeCollection: IReadOnlyNodeCollection
    {
        private readonly List<IReadOnlyNode> _nodes;

        public NodeCollection()
        {
            _nodes = new List<IReadOnlyNode>();
        }

        public NodeCollection(IEnumerable<IReadOnlyNode> nodes)
        {
            _nodes = nodes.ToList();
        }

        public IEnumerator<IReadOnlyNode> GetEnumerator()
        {
            return _nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _nodes.Count;

        public IReadOnlyNode this[string name] => _nodes.FirstOrDefault(x => x.Name == name);

        public IReadOnlyNode this[int id] => _nodes.FirstOrDefault(x => x.Id == id);

        public override string ToString()
        {
            return $"Count = {Count}";
        }
    }
}