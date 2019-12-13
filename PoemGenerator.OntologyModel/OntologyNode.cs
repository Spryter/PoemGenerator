using Microsoft.Msagl.Core.Geometry;
using PoemGenerator.OntologyModel.Abstractions;
using PoemGenerator.OntologyModel.Collections;

namespace PoemGenerator.OntologyModel
{
    public class OntologyNode: IReadOnlyNode
    {
        public int Id { get; }
        
        public string Name { get; }
        
        public Point Position { get; }

        public IReadOnlyRelationCollection ToRelations => To;
        
        public RelationCollection To { get; }

        public IReadOnlyRelationCollection FromRelations => From;

        public RelationCollection From { get; }
        
        public OntologyNode(int id, string name, Point position = default)
        {
            Id = id;
            Name = name;
            Position = position;
            To = new RelationCollection();
            From = new RelationCollection();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}