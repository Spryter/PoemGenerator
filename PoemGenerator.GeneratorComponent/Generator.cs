using System;
using System.Linq;
using PoemGenerator.OntologyComponent.Model;

namespace PoemGenerator.GeneratorComponent
{
    public class Generator
    {
        private readonly Ontology _ontology;
        
        public Generator(Ontology ontology)
        {
            _ontology = ontology;
        }
        
        public string GeneratePoem()
        {
            var elements = _ontology.Nodes
                .Get("стихотворение")
                .To("a_part_of")
                .Get("1 строка")
                .To("a_part_of")
                .FirstOrDefault(x => x.From("order").Get("1") != null)
                .From("element")
                .FirstOrDefault()
                .To("is_a");
            var rnd = new Random();
            var index = rnd.Next(elements.Count);
            var element = elements.ElementAt(index);
            var poemStrings = _ontology.Nodes
                .Get("стихотворение")
                .To("a_part_of")
                .OrderBy(s => s.From("order").FirstOrDefault().Name);
            foreach (var poemString in poemStrings)
            {
                var stringParts = poemString
                    .To("a_part_of")
                    .OrderBy(se => se.From("order").FirstOrDefault().Name);
                foreach (var stringPart in stringParts)
                {
                    var partElement = stringPart.From("element").FirstOrDefault();
                    switch (partElement.Name)
                    {
                        //case для обработки разных типов элементов
                    }
                    // конкатенация частей строк
                }
            }

            return element.Name;
        }
    }
}