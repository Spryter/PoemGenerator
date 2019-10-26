using System.Collections.Generic;
using PoemGenerator.OntologyLoader.Model.Abstractions;
using PoemGenerator.OntologyLoader.Model.Collections;

namespace PoemGenerator.OntologyLoader.Model
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