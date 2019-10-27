using PoemGenerator.OntologyComponent.Model.Abstractions;
using PoemGenerator.OntologyComponent.Model.Collections;

namespace PoemGenerator.OntologyComponent.Model
{
    public class Ontology
    {
        public IReadOnlyNodeCollection Nodes { get; }

        public Ontology()
        {
            Nodes = new NodeCollection();
        }
        
        public Ontology(IReadOnlyNodeCollection nodes)
        {
            Nodes = nodes;
        }
    }
}