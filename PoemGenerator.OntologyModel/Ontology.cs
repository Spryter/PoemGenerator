using PoemGenerator.OntologyModel.Abstractions;
using PoemGenerator.OntologyModel.Collections;

namespace PoemGenerator.OntologyModel
{
    public class Ontology
    {
        public IReadOnlyNodeCollection Nodes { get; }
        
        public IReadOnlyRelationCollection Relations { get; }

        public Ontology()
        {
            Nodes = new NodeCollection();
        }
        
        public Ontology(IReadOnlyNodeCollection nodes)
        {
            Nodes = nodes;
        }
        
        public Ontology(IReadOnlyNodeCollection nodes, IReadOnlyRelationCollection relations)
        {
            Nodes = nodes;
            Relations = relations;
        }
    }
}