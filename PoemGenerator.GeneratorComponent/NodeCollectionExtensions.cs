﻿using System;
using System.Collections.Generic;
using System.Linq;
using PoemGenerator.OntologyModel.Abstractions;
using PoemGenerator.OntologyModel.Collections;

namespace PoemGenerator.GeneratorComponent
{
    public static class NodeCollectionExtensions
    {
        private static Random _random = new Random();
        
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
            var index = _random.Next(nodes.Count);
            return list[index];
        }
        
        public static IReadOnlyNodeCollection ToNodeCollection(this IEnumerable<IReadOnlyNode> nodes)
        {
            return new NodeCollection(nodes);
        }
    }
}