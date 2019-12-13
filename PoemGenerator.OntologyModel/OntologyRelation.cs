using PoemGenerator.OntologyModel.Abstractions;

namespace PoemGenerator.OntologyModel
{
    public class OntologyRelation: IReadOnlyRelation
    {
        public string Name { get; }
        
        public IReadOnlyNode From { get; }
        
        public IReadOnlyNode To { get; }

        public OntologyRelation(string name, IReadOnlyNode from, IReadOnlyNode to)
        {
            Name = name;
            From = from;
            To = to;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}