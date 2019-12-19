using System;
using System.Collections.Generic;
using System.Linq;
using PoemGenerator.OntologyModel.Abstractions;
using PoemGenerator.OntologyModel.Collections;

namespace PoemGenerator.GeneratorComponent.Extensions
{
    public static class NodeCollectionExtensions
    {
        private static readonly Random Random = new Random();
        
        /// <summary>
        /// Возвращает узел с заданным именем, если его не существует - возвращает null.
        /// </summary>
        /// <param name="nodes">Список узлов.</param>
        /// <param name="name">Наименование узла</param>
        /// <returns>Узел.</returns>
        public static IReadOnlyNode Get(this IReadOnlyNodeCollection nodes, string name)
        {
            return nodes.FirstOrDefault(x => string.Equals(x.Name.Trim(), name.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }
        
        /// <summary>
        /// Возвращает случайный узел из коллекции.
        /// </summary>
        /// <param name="nodes">Список узлов.</param>
        /// <returns>Узел.</returns>
        public static IReadOnlyNode GetRandom(this IReadOnlyNodeCollection nodes)
        {
            var list = nodes.ToList();
            var index = Random.Next(nodes.Count);
            return list[index];
        }
        
        /// <summary> 
        /// Создает <see cref="T:IReadOnlyNodeCollection"/> из <see cref="T:System.Collections.Generic.IEnumerable`1" />
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns>Коллекция узлов</returns>
        public static IReadOnlyNodeCollection ToNodeCollection(this IEnumerable<IReadOnlyNode> nodes)
        {
            return new NodeCollection(nodes);
        }
    }
}