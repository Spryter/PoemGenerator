using System.Linq;
using PoemGenerator.OntologyModel.Abstractions;
using PoemGenerator.OntologyModel.Collections;

namespace PoemGenerator.GeneratorComponent
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Возвращает узел с заданным именем, если его не существует - возвращает null.
        /// </summary>
        /// <param name="nodes">Список узлов.</param>
        /// <param name="name">Наименование узла</param>
        /// <returns>Узел.</returns>
        public static IReadOnlyNode Get(this IReadOnlyNodeCollection nodes, string name)
        {
            return nodes.FirstOrDefault(x => x.Name == name);
        }

        /// <summary>
        /// Возвращает коллекцию узлов, которые входят в указанный узел по определенной связи.
        /// </summary>
        /// <param name="node">Узел.</param>
        /// <param name="relationName">Наименование связи узла.</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection To(this IReadOnlyNode node, string relationName)
        {
            return new NodeCollection(node.ToRelations.Where(x => x.Name == relationName).Select(x => x.From));
        }
        
        /// <summary>
        /// Возвращает коллекцию узлов, которые исходят из указанного узла по определенной связи.
        /// </summary>
        /// <param name="node">Узел.</param>
        /// <param name="relationName">Наименование связи узла.</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection From(this IReadOnlyNode node, string relationName)
        {
            return new NodeCollection(node.FromRelations.Where(x => x.Name == relationName).Select(x => x.To));
        }
    }
}