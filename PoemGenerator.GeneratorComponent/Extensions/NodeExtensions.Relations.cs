using PoemGenerator.GeneratorComponent.Constants;
using PoemGenerator.OntologyModel.Abstractions;

namespace PoemGenerator.GeneratorComponent.Extensions
{
    public static partial class NodeExtensions
    {
        /// <summary>
        /// Возвращает коллекцию узлов, которые входят в указанный узел по связи <see cref="Relations.APartOf"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection ToAPartOf(this IReadOnlyNode node)
        {
            return node.To(Relations.APartOf);
        }

        /// <summary>
        /// Возвращает коллекцию узлов, которые входят в указанный узел по связи <see cref="Relations.Element"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection ToElement(this IReadOnlyNode node)
        {
            return node.To(Relations.Element);
        }

        /// <summary>
        /// Возвращает коллекцию узлов, которые входят в указанный узел по связи <see cref="Relations.IsA"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection ToIsA(this IReadOnlyNode node)
        {
            return node.To(Relations.IsA);
        }

        /// <summary>
        /// Возвращает коллекцию узлов, которые входят в указанный узел по связи <see cref="Relations.Order"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection ToOrder(this IReadOnlyNode node)
        {
            return node.To(Relations.Order);
        }

        /// <summary>
        /// Возвращает коллекцию узлов, которые входят в указанный узел по связи <see cref="Relations.Object"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection ToObject(this IReadOnlyNode node)
        {
            return node.To(Relations.Object);
        }

        /// <summary>
        /// Возвращает коллекцию узлов, которые входят в указанный узел по связи <see cref="Relations.Action"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection ToAction(this IReadOnlyNode node)
        {
            return node.To(Relations.Action);
        }

        /// <summary>
        /// Возвращает коллекцию узлов, которые входят в указанный узел по связи <see cref="Relations.Locative"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection ToLocative(this IReadOnlyNode node)
        {
            return node.To(Relations.Locative);
        }
        
        /// <summary>
        /// Возвращает коллекцию узлов, которые входят в указанный узел по связи <see cref="Relations.Agent"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection ToAgent(this IReadOnlyNode node)
        {
            return node.To(Relations.Agent);
        }
        
        /// <summary>
        /// Возвращает коллекцию узлов, которые входят в указанный узел по связи <see cref="Relations.Has"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection ToHas(this IReadOnlyNode node)
        {
            return node.To(Relations.Has);
        }

        /// <summary>
        /// Возвращает коллекцию узлов, которые исходят из указанного узла по связи <see cref="Relations.APartOf"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection FromAPartOf(this IReadOnlyNode node)
        {
            return node.From(Relations.APartOf);
        }

        /// <summary>
        /// Возвращает коллекцию узлов, которые исходят из указанного узла по связи <see cref="Relations.Element"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection FromElement(this IReadOnlyNode node)
        {
            return node.From(Relations.Element);
        }

        /// <summary>
        /// Возвращает коллекцию узлов, которые исходят из указанного узла по связи <see cref="Relations.IsA"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection FromIsA(this IReadOnlyNode node)
        {
            return node.From(Relations.IsA);
        }

        /// <summary>
        /// Возвращает коллекцию узлов, которые исходят из указанного узла по связи <see cref="Relations.Order"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection FromOrder(this IReadOnlyNode node)
        {
            return node.From(Relations.Order);
        }

        /// <summary>
        /// Возвращает коллекцию узлов, которые исходят из указанного узла по связи <see cref="Relations.Object"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection FromObject(this IReadOnlyNode node)
        {
            return node.From(Relations.Object);
        }

        /// <summary>
        /// Возвращает коллекцию узлов, которые исходят из указанного узла по связи <see cref="Relations.Action"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection FromAction(this IReadOnlyNode node)
        {
            return node.From(Relations.Action);
        }

        /// <summary>
        /// Возвращает коллекцию узлов, которые исходят из указанного узла по связи <see cref="Relations.Locative"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection FromLocative(this IReadOnlyNode node)
        {
            return node.From(Relations.Locative);
        }

        /// <summary>
        /// Возвращает коллекцию узлов, которые исходят из указанного узла по связи <see cref="Relations.Agent"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection FromAgent(this IReadOnlyNode node)
        {
            return node.From(Relations.Agent);
        }
        
        /// <summary>
        /// Возвращает коллекцию узлов, которые исходят из указанного узла по связи <see cref="Relations.Has"/>.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Коллекция узлов.</returns>
        public static IReadOnlyNodeCollection FromHas(this IReadOnlyNode node)
        {
            return node.From(Relations.Has);
        }
    }
}