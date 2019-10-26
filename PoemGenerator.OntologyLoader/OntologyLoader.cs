using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PoemGenerator.OntologyLoader.JsonModel;
using PoemGenerator.OntologyLoader.Model;
using PoemGenerator.OntologyLoader.Model.Collections;

namespace PoemGenerator.OntologyLoader
{
    public static class OntologyLoader
    {
        public static Ontology LoadByPath(string path)
        {
            var jsonOntology = JsonConvert.DeserializeObject<JsonOntology>(File.ReadAllText(path));
            var nodes = new NodeCollection(jsonOntology.Nodes.Select(x => new Node(x.Id, x.Name)));
            
            foreach (var jsonRelation in jsonOntology.Relations)
            {
                var fromNode = (Node)nodes[jsonRelation.SourceNodeId];
                var toNode = (Node)nodes[jsonRelation.DestinationNodeId];
                
                var relation = new Relation(jsonRelation.Name, fromNode, toNode);
                fromNode.From.Relations.Add(relation);
                toNode.To.Relations.Add(relation);
            }

            return new Ontology(nodes);
        }
    }
}