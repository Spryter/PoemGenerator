using System.Collections.Generic;
using System.Linq;
using PoemGenerator.GeneratorComponent.Constants;
using PoemGenerator.GeneratorComponent.Extensions;
using PoemGenerator.OntologyModel.Abstractions;
using PoemGenerator.OntologyModel.Collections;

namespace PoemGenerator.GeneratorComponent.Situations
{
    public class SafeSituation: Situation
    {
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
        
        public override IReadOnlyNodeCollection GetNodes()
        {
            return new[] {Action, Object, Locative}.ToNodeCollection();
        }

        public override IReadOnlyRelationCollection GetRelations()
        {
            foreach (var currentNode in Action.To(Relations.Action)
                .Where(x => x.FromIsANested().Any(y => y.Name == Nodes.SafeSituation)))
            {
                var result = new List<IReadOnlyRelation> {Action.ToRelations.First(x => x.From == currentNode)};
                var objectRelationsFrom = GetRelations(currentNode, Object, Relations.Object, true);
                var objectRelationsTo = GetRelations(currentNode, Object, Relations.Object, false);
                var locativeRelationsFrom = GetRelations(currentNode, Locative, Relations.Locative, true);
                var locativeRelationsTo = GetRelations(currentNode, Locative, Relations.Locative, false);
                result.AddRange(objectRelationsFrom
                    .Union(objectRelationsTo)
                    .Union(locativeRelationsFrom)
                    .Union(locativeRelationsTo));
                if (result.Count > 1)
                    return result.ToRelationCollection();
            }
            
            return new RelationCollection();
        }
    }
}