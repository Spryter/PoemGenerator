using System.Collections.Generic;
using System.Linq;
using PoemGenerator.GeneratorComponent.Constants;
using PoemGenerator.GeneratorComponent.Extensions;
using PoemGenerator.OntologyModel.Abstractions;

namespace PoemGenerator.GeneratorComponent
{
    public class Situation
    {
        public IReadOnlyNode Action { get; set; }
        
        public IReadOnlyNode Object { get; set; }
        
        public IReadOnlyNode Locative { get; set; }

        private static List<IReadOnlyRelation> GetRelations(IReadOnlyNode node, IReadOnlyNode nodeToFind, string relation, bool isFrom)
        {
            if (node.From(relation).Any(currentNode => currentNode == nodeToFind))
            {
                return new List<IReadOnlyRelation> {node.FromRelations.First(x => x.To == nodeToFind)};
            }

            foreach (var currentNode in isFrom ? node.FromIsA() : node.ToIsA())
            {
                var relations = GetRelations(currentNode, nodeToFind, relation, isFrom);
                if (relations.Count <= 0) continue;
                var toNode = node.ToRelations.FirstOrDefault(x => x.From == currentNode);
                var fromNode = node.FromRelations.FirstOrDefault(x => x.To == currentNode);
                relations.Add(isFrom ? fromNode : toNode);
                return relations;
            }
            
            return new List<IReadOnlyRelation>();
        }

        private static IEnumerable<IReadOnlyRelation> GetRelations(IReadOnlyNode node, string nodeRelation, IReadOnlyNode nodeToFind, string relation)
        {
            var result = new List<IReadOnlyRelation>();
            foreach (var currentNode in node.To(nodeRelation).Where(x => x.FromIsANested().All(y => y.Name != Nodes.DangerousSituation)))
            {
                var relationsFrom = GetRelations(currentNode, nodeToFind, relation, true);
                var relationsTo = GetRelations(currentNode, nodeToFind, relation, false);
                result.AddRange(relationsFrom.Union(relationsTo));
                result.AddRange(node.ToRelations.Where(x => x.From.FromIsANested().All(y => y.Name != Nodes.DangerousSituation)));
            }
            
            return result;
        }
        
        public IReadOnlyNodeCollection GetNodes()
        {
            return new[] {Action, Object, Locative}.ToNodeCollection();
        }

        public IReadOnlyRelationCollection GetRelations()
        {
            var result = new List<IReadOnlyRelation>();
            result.AddRange(GetRelations(Action, Relations.Action, Object, Relations.Object));
            result.AddRange(GetRelations(Object, Relations.Object, Locative, Relations.Locative));

            return result.ToRelationCollection();
        }
    }
}