using System;
using System.Collections.Generic;
using System.Linq;
using PoemGenerator.GeneratorComponent.Constants;
using PoemGenerator.GeneratorComponent.Extensions;
using PoemGenerator.OntologyModel.Abstractions;
using PoemGenerator.OntologyModel.Collections;

namespace PoemGenerator.GeneratorComponent.Situations
{
    public abstract class Situation
    {
        public IReadOnlyNode Agent { get; set; } = new EmptyOntologyNode();
        
        public IReadOnlyNode Action { get; set; } = new EmptyOntologyNode();
        
        public IReadOnlyNode Object { get; set; } = new EmptyOntologyNode();
        
        public IReadOnlyNode Locative { get; set; } = new EmptyOntologyNode();

        private static IReadOnlyNode GetParentAgent(IReadOnlyNode agent)
        {
            if (agent.Name == EmptyOntologyNode.Name)
                return agent;
            
            var agents = new Queue<IReadOnlyNode>();
            agents.Enqueue(agent);
            while (agents.Count > 0)
            {
                agent = agents.Dequeue();
                if (agent.ToRelations.Any(x => x.Name == Relations.Agent))
                    return agent;
                foreach (var node in agent.FromIsA())
                {
                    agents.Enqueue(node);
                }
            }

            throw new ArgumentException();
        }

        private static List<IReadOnlyRelation> GetAgentRelations(IReadOnlyNode agent, IReadOnlyNode parentAgent)
        {
            if (agent == parentAgent)
                return new List<IReadOnlyRelation>();
            
            if (agent.FromIsA().Any(currentNode => currentNode == parentAgent))
            {
                return new List<IReadOnlyRelation> {agent.FromRelations.Where(x => x.Name == Relations.IsA).First(x => x.To == parentAgent)};
            }

            foreach (var node in agent.FromIsA())
            {
                var relations = GetAgentRelations(node, parentAgent);
                if (relations.Count <= 0) continue;
                relations.Add(node.FromRelations.Where(x => x.Name == Relations.IsA).FirstOrDefault(x => x.To == agent));
                return relations;
            }
            
            return new List<IReadOnlyRelation>();
        }
        
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
        
        public virtual IReadOnlyNodeCollection GetNodes()
        {
            return new[] {Agent, Action, Object, Locative}.ToNodeCollection();
        }

        public virtual IReadOnlyRelationCollection GetRelations()
        {
            var parentAgent = GetParentAgent(Agent);
            var agentRelations = GetAgentRelations(Agent, parentAgent);
            foreach (var currentNode in GetRelevantToSituationActions())
            {
                var result = new List<IReadOnlyRelation> {Action.ToRelations.First(x => x.From == currentNode)};
                var objectRelationsFrom = GetRelations(currentNode, Object, Relations.Object, true);
                var objectRelationsTo = GetRelations(currentNode, Object, Relations.Object, false);
                var locativeRelationsFrom = GetRelations(currentNode, Locative, Relations.Locative, true);
                var locativeRelationsTo = GetRelations(currentNode, Locative, Relations.Locative, false);
                var agentRelationsFrom = GetRelations(currentNode, parentAgent, Relations.Agent, true);
                var agentRelationsTo = GetRelations(currentNode, parentAgent, Relations.Agent, false);
                result.AddRange(objectRelationsFrom
                    .Union(objectRelationsTo)
                    .Union(locativeRelationsFrom)
                    .Union(locativeRelationsTo)
                    .Union(agentRelationsFrom)
                    .Union(agentRelationsTo)
                    .Union(agentRelations));
                if ((Action.Name == EmptyOntologyNode.Name || result.Any(x => x.To == Action)) &&
                    (Object.Name == EmptyOntologyNode.Name || result.Any(x => x.To == Object)) &&
                    (Locative.Name == EmptyOntologyNode.Name || result.Any(x => x.To == Locative)) &&
                    (Agent.Name == EmptyOntologyNode.Name || result.Any(x => x.To == parentAgent)))
                    return result.ToRelationCollection();
            }
            
            return new RelationCollection();
        }

        protected abstract IEnumerable<IReadOnlyNode> GetRelevantToSituationActions();
    }
}