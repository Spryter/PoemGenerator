using PoemGenerator.OntologyModel.Abstractions;
using PoemGenerator.OntologyModel.Collections;

namespace PoemGenerator.OntologyModel
{
    public class Node: IReadOnlyNode
    {
        public int Id { get; }
        
        public string Name { get; }

        public IReadOnlyRelationCollection ToRelations => To;
        
        public RelationCollection To { get; }

        public IReadOnlyRelationCollection FromRelations => From;

        public RelationCollection From { get; }
        
        public Node(int id, string name)
        {
            Id = id;
            Name = name;
            To = new RelationCollection();
            From = new RelationCollection();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}