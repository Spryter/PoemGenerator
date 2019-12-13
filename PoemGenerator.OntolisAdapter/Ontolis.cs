using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Msagl.Core.Geometry;
using Newtonsoft.Json;
using PoemGenerator.OntolisAdapter.Exceptions;
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
            var nodes = new NodeCollection(jsonOntology.Nodes.Select(x =>
                new OntologyNode(x.Id, x.Name, new Point(x.PositionX, x.PositionY))));
            
            var ontologyRelations = new List<OntologyRelation>();
            
            foreach (var jsonRelation in jsonOntology.Relations)
            {
                var fromNode = (OntologyNode)nodes[jsonRelation.SourceNodeId];
                var toNode = (OntologyNode)nodes[jsonRelation.DestinationNodeId];
                
                var relation = new OntologyRelation(jsonRelation.Name, fromNode, toNode);
                fromNode.From.Relations.Add(relation);
                toNode.To.Relations.Add(relation);
                ontologyRelations.Add(relation);
            }
            
            var relations = new RelationCollection(ontologyRelations);
            return new Ontology(nodes, relations);
        }

        public Ontology Reload()
        {
            if (_ontologyPath == null)
            {
                throw new OntologyPathNotInitializedException();
            }

            return LoadByPath(_ontologyPath);
        }
        
        public void Open()
        {
            if (_ontologyPath == null)
            {
                throw new OntologyPathNotInitializedException();
            }

            var process = Process.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, OntolisBuildPath), _ontologyPath);
            process?.WaitForExit();
        }
    }
}