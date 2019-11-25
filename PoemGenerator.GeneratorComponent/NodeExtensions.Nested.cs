using PoemGenerator.OntologyModel.Abstractions;

namespace PoemGenerator.GeneratorComponent
{
    public static partial class NodeExtensions
    {
        
        /// <summary>
        /// Возвращает коллекцию узлов, которые исходят по связи <see cref="Relations.Object"/> из всех нижележащих узов по вложенной входящей связи <see cref="Relations.IsA"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection ToIsANestedFromObject(this IReadOnlyNode node)
        {
            return node.ToNestedFrom(Relations.IsA, Relations.Object);
        }
        
        /// <summary>
        /// Возвращает коллекцию узлов, которые исходят по связи <see cref="Relations.Action"/> из всех нижележащих узов по вложенной входящей связи <see cref="Relations.IsA"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection ToIsANestedFromAction(this IReadOnlyNode node)
        {
            return node.ToNestedFrom(Relations.IsA, Relations.Action);
        }
        
        /// <summary>
        /// Возвращает коллекцию узлов, которые исходят по связи <see cref="Relations.Locative"/> из всех нижележащих узов по вложенной входящей связи <see cref="Relations.IsA"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection ToIsANestedFromLocative(this IReadOnlyNode node)
        {
            return node.ToNestedFrom(Relations.IsA, Relations.Locative);
        }
        
        /// <summary>
        /// Возвращает коллекцию узлов, которые вложенно входят по связи <see cref="Relations.IsA"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection ToIsANested(this IReadOnlyNode node)
        {
            return node.ToNested(Relations.IsA);
        }

        /// <summary>
        /// Возвращает коллекцию узлов, которые исходят по определенной связи из всех нижележащих узов по вложенной входящей связи <see cref="Relations.IsA"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <param name="relationName">Наименование связи.</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection ToIsANestedFrom(this IReadOnlyNode node, string relationName)
        {
            return node.ToNestedFrom(Relations.IsA, relationName);
        }
        
        /// <summary>
        /// Возвращает коллекцию узлов, которые исходят по связи <see cref="Relations.Action"/> из всех нижележащих узов по вложенной входящей связи <see cref="Relations.IsA"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection FromIsANestedFromAction(this IReadOnlyNode node)
        {
            return node.FromNestedFrom(Relations.IsA, Relations.Action);
        }
        
        /// <summary>
        /// Возвращает коллекцию узлов, которые исходят по связи <see cref="Relations.Object"/> из всех нижележащих узов по вложенной входящей связи <see cref="Relations.IsA"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection FromIsANestedFromObject(this IReadOnlyNode node)
        {
            return node.FromNestedFrom(Relations.IsA, Relations.Object);
        }
        
        /// <summary>
        /// Возвращает коллекцию узлов, которые исходят по связи <see cref="Relations.Locative"/> из всех нижележащих узов по вложенной входящей связи <see cref="Relations.IsA"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection FromIsANestedFromLocative(this IReadOnlyNode node)
        {
            return node.FromNestedFrom(Relations.IsA, Relations.Locative);
        }
        
        
        /// <summary>
        /// Возвращает коллекцию узлов, которые вложенно исходят по связи <see cref="Relations.IsA"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection FromIsANested(this IReadOnlyNode node)
        {
            return node.FromNested(Relations.IsA);
        }
    }
}