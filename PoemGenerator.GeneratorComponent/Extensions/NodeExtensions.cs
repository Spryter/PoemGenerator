using System;
using System.Collections.Generic;
using System.Linq;
using PoemGenerator.OntologyModel.Abstractions;
using PoemGenerator.OntologyModel.Collections;

namespace PoemGenerator.GeneratorComponent.Extensions
{
    public static partial class NodeExtensions
    {
        /// <summary>
        /// Возвращает коллекцию узлов, которые входят в указанный узел по определенной связи.
        /// </summary>
        /// <param name="node">Узел.</param>
        /// <param name="relationName">Наименование связи узла.</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection To(this IReadOnlyNode node, string relationName)
        {
            return new NodeCollection(node.ToRelations.Where(x =>
                    string.Equals(x.Name.Trim(), relationName.Trim(), StringComparison.CurrentCultureIgnoreCase))
                .Select(x => x.From));
        }
        
        /// <summary>
        /// Возвращает коллекцию узлов, которые исходят из указанного узла по определенной связи.
        /// </summary>
        /// <param name="node">Узел.</param>
        /// <param name="relationName">Наименование связи узла.</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection From(this IReadOnlyNode node, string relationName)
        {
            return new NodeCollection(node.FromRelations.Where(x =>
                    string.Equals(x.Name.Trim(), relationName.Trim(), StringComparison.CurrentCultureIgnoreCase))
                .Select(x => x.To));
        }

        /// <summary>
        /// Возвращает коллекцию узлов, которые вложенно входят по указанной связи.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <param name="relationName"></param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection ToNested(this IReadOnlyNode node, string relationName)
        {
            var nodeCollection = node.To(relationName).ToList();
            var result = new List<IReadOnlyNode>(nodeCollection);
            foreach (var readOnlyNode in nodeCollection)
            {
                result.AddRange(readOnlyNode.ToNested(relationName));
            }
            return new NodeCollection(result);
        }
        
        /// <summary>
        /// Возвращает коллекцию узлов, которые вложенно исходят по указанной связи.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <param name="relationName"></param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection FromNested(this IReadOnlyNode node, string relationName)
        {
            var nodeCollection = node.From(relationName).ToList();
            var result = new List<IReadOnlyNode>(nodeCollection);
            foreach (var readOnlyNode in nodeCollection)
            {
                result.AddRange(readOnlyNode.FromNested(relationName));
            }
            return new NodeCollection(result);
        }

        /// <summary>
        /// Возвращает коллекцию узлов, которые исходят по определенной связи из всех нижележащих узов по определенной вложенной входящей связи.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <param name="relationNameTo">Наименование вложенной входящей связи.</param>
        /// <param name="relationNameFrom">Наименование исходящей связи.</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection ToNestedFrom(this IReadOnlyNode node, string relationNameTo, string relationNameFrom)
        {
            return new NodeCollection(node.ToNested(relationNameTo).SelectMany(x => x.From(relationNameFrom)));
        }
        
        /// <summary>
        /// Возвращает коллекцию узлов, которые входят по определенной связи из всех нижележащих узов по определенной вложенной входящей связи.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <param name="firstRelationNameTo">Наименование вложенной входящей связи.</param>
        /// <param name="secondRelationNameTo">Наименование входящей связи.</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection ToNestedTo(this IReadOnlyNode node, string firstRelationNameTo, string secondRelationNameTo)
        {
            return new NodeCollection(node.ToNested(firstRelationNameTo).SelectMany(x => x.To(secondRelationNameTo)));
        }
        
        /// <summary>
        /// Возвращает коллекцию узлов, которые исходят по определенной связи из всех нижележащих узов по определенной вложенной исходящей связи.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <param name="firstRelationNameFrom">Наименование вложенной исходящей связи.</param>
        /// <param name="secondRelationNameFrom">Наименование исходящей связи.</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection FromNestedFrom(this IReadOnlyNode node, string firstRelationNameFrom, string secondRelationNameFrom)
        {
            return new NodeCollection(node.FromNested(firstRelationNameFrom).SelectMany(x => x.From(secondRelationNameFrom)));
        }
        
        /// <summary>
        /// Возвращает коллекцию узлов, которые входят по определенной связи из всех нижележащих узов по определенной вложенной исходящей связи.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <param name="relationNameFrom">Наименование вложенной исходящей связи.</param>
        /// <param name="relationNameTo">Наименование входящей связи.</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection FromNestedTo(this IReadOnlyNode node, string relationNameFrom, string relationNameTo)
        {
            return new NodeCollection(node.FromNested(relationNameFrom).SelectMany(x => x.To(relationNameTo)));
        }
    }
}