using System;
using System.Collections.Generic;
using System.Linq;
using PoemGenerator.OntologyModel;

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
                .To(Relations.APartOf)
                .Get("1 строка")
                .To(Relations.APartOf)
                .FirstOrDefault(x => x.From(Relations.Order).Get("1") != null)
                .From(Relations.Element)
                .FirstOrDefault()
                .To(Relations.IsA);
            var rnd = new Random();
            var index = rnd.Next(elements.Count);
            var element = elements.ElementAt(index);
            var poemStrings = _ontology.Nodes
                .Get("стихотворение")
                .To(Relations.APartOf)
                .OrderBy(s => s.From(Relations.Order).FirstOrDefault().Name);
            foreach (var poemString in poemStrings)
            {
                var stringParts = poemString
                    .To(Relations.APartOf)
                    .OrderBy(se => se.From(Relations.Order).FirstOrDefault().Name);
                foreach (var stringPart in stringParts)
                {
                    var partElement = stringPart.From(Relations.Element).FirstOrDefault();
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