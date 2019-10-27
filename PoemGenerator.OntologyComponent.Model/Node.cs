using PoemGenerator.OntologyComponent.Model.Abstractions;
using PoemGenerator.OntologyComponent.Model.Collections;

namespace PoemGenerator.OntologyComponent.Model
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