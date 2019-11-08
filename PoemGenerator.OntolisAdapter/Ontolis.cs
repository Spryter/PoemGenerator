using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PoemGenerator.OntolisAdapter.JsonModel;
using PoemGenerator.OntologyModel;
using PoemGenerator.OntologyModel.Collections;

namespace PoemGenerator.OntolisAdapter
{
    public class Ontolis
    {
        private const string OntolisBuildPath = "ontolis/ontolis.exe";
        
        private string _ontologyPath;

        public Ontology LoadByPath(string path)
        {
            _ontologyPath = path;
            
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

        public Ontology Reload()
        {
            return LoadByPath(_ontologyPath);
        }
        
        public void Open()
        {
            var process = Process.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, OntolisBuildPath), _ontologyPath);
            process?.WaitForExit();
        }
    }
}