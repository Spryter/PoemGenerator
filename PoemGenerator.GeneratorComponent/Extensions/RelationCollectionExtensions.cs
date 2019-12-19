using System;
using System.Collections.Generic;
using System.Linq;
using PoemGenerator.OntologyModel.Abstractions;
using PoemGenerator.OntologyModel.Collections;

namespace PoemGenerator.GeneratorComponent.Extensions
{
    public static class RelationCollectionExtensions
    {
        private static readonly Random Random = new Random();

        public static IReadOnlyRelationCollection ToRelationCollection(this IEnumerable<IReadOnlyRelation> relations)
        {
            return new RelationCollection(relations);
        }
        
        /// <summary>
        /// Возвращает случайную связь из коллекции.
        /// </summary>
        /// <param name="relations">Список связей.</param>
        /// <returns>Связь.</returns>
        public static IReadOnlyRelation GetRandom(this IReadOnlyRelationCollection relations)
        {
            var list = relations.ToList();
            var index = Random.Next(relations.Count);
            return list[index];
        }
    }
}