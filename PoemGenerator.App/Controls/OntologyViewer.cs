using System.Linq;
using System.Windows.Forms;
using Microsoft.Msagl.Core.Geometry;
using Microsoft.Msagl.Core.Layout;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using Microsoft.Msagl.Routing;
using PoemGenerator.OntologyModel;
using PoemGenerator.OntologyModel.Abstractions;
using Node = Microsoft.Msagl.Drawing.Node;

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

        private Graph CreateGraph(IReadOnlyNodeCollection nodes = null, IReadOnlyRelationCollection relations = null)
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

            return graph;
        }

        private void UpdateGeometryGraph(Graph graph)
        {
            foreach (var ontologyNode in _ontology.Nodes)
            {
                var node = graph.FindGeometryNode(ontologyNode.Id.ToString());
                node.Center = new Point(ontologyNode.Position.X, -ontologyNode.Position.Y);
            }
            
            foreach (var edge in graph.Edges)
            {
                StraightLineEdges.CreateSimpleEdgeCurveWithUnderlyingPolyline(edge.GeometryEdge);
            }
            
            new EdgeLabelPlacement(graph.GeometryGraph).Run();
            graph.GeometryGraph.UpdateBoundingBox();
            _viewer.Transform = null;
            _viewer.DrawingPanel.Invalidate();
        }

        private void UpdateGraph(IReadOnlyNodeCollection nodes = null, IReadOnlyRelationCollection relations = null)
        {
            var graph = CreateGraph(nodes, relations);
            _viewer.Graph = graph;
            UpdateGeometryGraph(graph);
        }
    }
}