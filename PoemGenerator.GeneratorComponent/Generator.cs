using System;
using System.Collections.Generic;
using System.Linq;
using PoemGenerator.GeneratorComponent.Constants;
using PoemGenerator.GeneratorComponent.Extensions;
using PoemGenerator.GeneratorComponent.Helpers;
using PoemGenerator.GeneratorComponent.Situations;
using PoemGenerator.OntologyModel;
using PoemGenerator.OntologyModel.Abstractions;

namespace PoemGenerator.GeneratorComponent
{
    public class Generator
    {
        private readonly Ontology _ontology;
        
        public Generator(Ontology ontology)
        {
            _ontology = ontology;
        }

        /// <summary>
        /// Возвращает множество узлов, которые подходят по иерархии с заданными связями
        /// </summary>
        /// <param name="node">Узел части фрейма.</param>
        /// <param name="toRelation">Связь части фрейма.</param>
        /// <param name="fromRelation">Узлы со связями которые надо найти.</param>
        /// <returns>Множество узлов.</returns>
        private static IEnumerable<IReadOnlyNode> GetRelevant(IReadOnlyNode node, string toRelation,
            string fromRelation)
        {
            return node.To(toRelation)
                .SelectMany(x => x.From(fromRelation))
                .Union(node
                    .To(toRelation)
                    .SelectMany(x => x
                        .ToIsANestedFrom(fromRelation)
                        .Union(x
                            .FromIsANested()
                            .Where(y => y.Name != Nodes.MainFrameNode)
                            .SelectMany(y => y.From(fromRelation))
                        )
                    )
                );
        }

        private static IReadOnlyNode GetNodeByRelation(string relation, Situation situation)
        {
            switch (relation)
            {
                case Relations.Agent:
                    return situation.Agent;
                case Relations.Action:
                    return situation.Action;
                case Relations.Object:
                    return situation.Object;
                case Relations.Locative:
                    return situation.Locative;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static IReadOnlyNode GetParentAgent(IReadOnlyNode agent)
        {
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

        private static RelevantNodes GetRelevantNodes(RelevantNodes relevantNodes, Situation situation, string relation)
        {
            var node = GetNodeByRelation(relation, situation);
            if (relation == Relations.Agent)
            {
                node = GetParentAgent(node);
            }
            
            if (relation != Relations.Agent)
            {
                var relevantAgents = GetRelevant(node, relation, Relations.Agent).ToList();
                relevantNodes.Agents = relevantNodes.Agents
                    .Intersect(relevantAgents
                        .Union(relevantAgents.SelectMany(x => x.ToIsANested()))
                        .Distinct()
                    );
            }

            if (relation != Relations.Action)
            {
                relevantNodes.Actions = relevantNodes.Actions
                    .Intersect(GetRelevant(node, relation, Relations.Action));
            }

            if (relation != Relations.Object)
            {
                relevantNodes.Objects = relevantNodes.Objects
                    .Intersect(GetRelevant(node, relation, Relations.Object));
            }

            if (relation != Relations.Locative)
            {
                relevantNodes.Locatives = relevantNodes.Locatives
                    .Intersect(GetRelevant(node, relation, Relations.Locative));
            }

            return relevantNodes;
        }

        private RelevantNodes GetRelevantNodes(string situationName)
        {
            var relevantNodes = new RelevantNodes();
            var situation = _ontology.Nodes.Get(situationName).ToIsA();
            relevantNodes.Actions = situation.SelectMany(x => x.ToIsANestedFromAction()).ToNodeCollection();
            relevantNodes.Objects = situation.SelectMany(x => x.ToIsANestedFromObject()).ToNodeCollection();
            relevantNodes.Locatives = situation.SelectMany(x => x.ToIsANestedFromLocative()).ToNodeCollection();
            relevantNodes.Agents = situation
                .SelectMany(x => x.ToIsANestedFromAgent())
                .Union(situation
                    .SelectMany(x => x.ToIsANestedFromAgent())
                    .SelectMany(x => x.ToIsANested()))
                .Distinct()
                .ToNodeCollection();

            return relevantNodes;
        }

        public RelevantNodes GetRelevantNodes(Situation situation, string situationName)
        {
            var relevantNodes = GetRelevantNodes(situationName);
            
            if (situation.Agent.Name != EmptyOntologyNode.Name)
            {
                relevantNodes = GetRelevantNodes(relevantNodes, situation, Relations.Agent);
            }

            if (situation.Action.Name != EmptyOntologyNode.Name)
            {
                relevantNodes = GetRelevantNodes(relevantNodes, situation, Relations.Action);
            }

            if (situation.Object.Name != EmptyOntologyNode.Name)
            {
                relevantNodes = GetRelevantNodes(relevantNodes, situation, Relations.Object);
            }

            if (situation.Locative.Name != EmptyOntologyNode.Name)
            {
                relevantNodes = GetRelevantNodes(relevantNodes, situation, Relations.Locative);
            }

            return relevantNodes;
        }

        /// <summary>
        /// Генерирует ситуацию по неполному заполнению.
        /// </summary>
        /// <param name="situation">Исходная ситуация с пустыми полями.</param>
        /// <param name="situationName">Наименование ситуации в онтологии</param>
        /// <returns>Заполненная ситуация.</returns>
        public Situation GenerateSituation(Situation situation, string situationName)
        {
            var resultSituation = new SafeSituation();
            var relevantNodes = GetRelevantNodes(situation, situationName);

            if (situation.Agent.Name != EmptyOntologyNode.Name)
            {
                resultSituation.Agent = situation.Agent;
            }
            else if (relevantNodes.Agents.Any())
            {
                var agentItem = relevantNodes.Agents.ToNodeCollection().GetRandom();
                resultSituation.Agent = agentItem;
                relevantNodes = GetRelevantNodes(relevantNodes, resultSituation, Relations.Agent);
            }
            
            if (situation.Action.Name != EmptyOntologyNode.Name)
            {
                resultSituation.Action = situation.Action;
            }
            else if (relevantNodes.Actions.Any())
            {
                var actionItem = relevantNodes.Actions.ToNodeCollection().GetRandom();
                resultSituation.Action = actionItem;
                relevantNodes = GetRelevantNodes(relevantNodes, resultSituation, Relations.Action);
            }

            if (situation.Object.Name != EmptyOntologyNode.Name)
            {
                resultSituation.Object = situation.Object;
            }
            else if (relevantNodes.Objects.Any())
            {
                var objectItem = relevantNodes.Objects.ToNodeCollection().GetRandom();
                resultSituation.Object = objectItem;
                relevantNodes = GetRelevantNodes(relevantNodes, resultSituation, Relations.Object);
            }

            resultSituation.Locative = situation.Locative.Name != EmptyOntologyNode.Name
                ? situation.Locative
                : relevantNodes.Locatives.Any()
                    ? relevantNodes.Locatives.ToNodeCollection().GetRandom()
                    : new EmptyOntologyNode();
            
            return resultSituation;
        }

        public DangerousSituation GenerateDangerousSituation(SafeSituation safeSituation)
        {
            var dangerousSituation = new DangerousSituation();
            var nodes = safeSituation
                .GetNodes()
                .Where(x => 
                    x.FromRelations.Any(y => y.Name == Relations.Affects || y.Name == Relations.APartOf) || 
                    x.ToRelations.Any(y => y.Name == Relations.Affects || y.Name == Relations.APartOf)
                )
                .ToNodeCollection();
            if (nodes.Count > 0)
            {
                var node = nodes.GetRandom();
                var relation = node.FromRelations
                    .Union(node.ToRelations)
                    .Where(x => x.Name == Relations.Affects || x.Name == Relations.APartOf)
                    .ToRelationCollection()
                    .GetRandom();
                var dangerousNode = OntologyHelper.GetDangerousNode(relation);
                if (dangerousNode.ToRelations.Any(x => x.Name == Relations.Agent))
                {
                    dangerousSituation.Agent = dangerousNode;
                }
                
                if (dangerousNode.ToRelations.Any(x => x.Name == Relations.Action))
                {
                    dangerousSituation.Action = dangerousNode;
                }
                
                if (dangerousNode.ToRelations.Any(x => x.Name == Relations.Object))
                {
                    dangerousSituation.Object = dangerousNode;
                }
                
                if (dangerousNode.ToRelations.Any(x => x.Name == Relations.Locative))
                {
                    dangerousSituation.Locative = dangerousNode;
                }
            }
            return GenerateSituation(dangerousSituation, Nodes.DangerousSituation).ToDangerousSituation();
        }
    }
}