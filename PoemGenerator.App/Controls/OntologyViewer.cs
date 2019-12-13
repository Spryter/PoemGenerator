using System.Linq;
using System.Windows.Forms;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using PoemGenerator.OntologyModel;
using PoemGenerator.OntologyModel.Abstractions;

namespace PoemGenerator.App.Controls
{
    public partial class OntologyViewer : UserControl
    {
        private GViewer _viewer;
        
        private Ontology _ontology;

        public Ontology Ontology
        {
            get => _ontology;
            set 
            { 
                _ontology = value;
                UpdateGraph();
            }
        }

        public OntologyViewer(OntologyViewModel ontologyViewModel)
        {
            InitializeComponent();
            InitializeGraphViewer();
            ontologyViewModel.ColorizeGraph += UpdateGraph;
        }

        private void InitializeGraphViewer()
        {
            _viewer = new GViewer
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(_viewer);
        }

        private void UpdateGraph(IReadOnlyNodeCollection nodes = null, IReadOnlyRelationCollection relations = null)
        {
            var graph = new Graph();
            foreach (var ontologyNode in _ontology.Nodes)
            {
                var node = new Node(ontologyNode.Id.ToString())
                {
                    LabelText = ontologyNode.Name,
                };
                
                if (nodes != null && nodes.Contains(ontologyNode))
                {
                    node.Attr.FillColor = Color.Green;
                }
                
                graph.AddNode(node);
            }

            foreach (var ontologyRelation in _ontology.Relations)
            {
                var edge = graph.AddEdge(ontologyRelation.From.Id.ToString(), ontologyRelation.Name,
                    ontologyRelation.To.Id.ToString());
                if (relations != null && relations.Contains(ontologyRelation))
                {
                    edge.Attr.Color = Color.Red;
                }
            }

            _viewer.Graph = graph;
        }
    }
}