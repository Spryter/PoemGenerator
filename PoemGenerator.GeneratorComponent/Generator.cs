using System;
using System.Collections.Generic;
using System.Linq;
using PoemGenerator.GeneratorComponent.Constants;
using PoemGenerator.GeneratorComponent.Extensions;
using PoemGenerator.GeneratorComponent.Situations;
using PoemGenerator.OntologyModel;

namespace PoemGenerator.GeneratorComponent
{
    public class Generator
    {
        public Ontology Ontology { get; }
        
        public Generator(Ontology ontology)
        {
            Ontology = ontology;
        }

        public Situation GenerateDangerousSituation()
        {
            var situations = Ontology.Nodes
                .Get(Nodes.DangerousSituation)
                .ToIsA()
                .ToNodeCollection();

            var randomObject = situations
                .SelectMany(x => x.ToIsANestedFromObject())
                .ToNodeCollection()
                .GetRandom();
			
            var randomAction = situations
                .SelectMany(x => x.ToIsANestedFromAction())
                .ToNodeCollection()
                .GetRandom();
			
            var randomLocative = situations
                .SelectMany(x => x.ToIsANestedFromLocative())
                .ToNodeCollection()
                .GetRandom();
            
            return new DangerousSituation
            {
                Action = randomAction,
                Object = randomObject,
                Locative = randomLocative
            };
        }

        public string GenerateChild()
        {
            var child = Ontology.Nodes
                .Get(Nodes.Situation)
                .FromAgent()
                .FirstOrDefault()
                .ToIsA()
                .Get(Nodes.Child)
                .ToIsANested()
                .GetRandom();

            return child.Name;
        }

        public string GeneratePoem()
        {
			var elements = Ontology.Nodes
				.Get("стихотворение")
				.ToAPartOf()
				.Get("1 строка")
				.ToAPartOf()
                .FirstOrDefault(x => x.FromOrder().Get("1") != null)
                .FromElement()
                .FirstOrDefault()
                .ToIsA();
            var rnd = new Random();
            var index = rnd.Next(elements.Count);
            var element = elements.ElementAt(index);
            var poemStrings = Ontology.Nodes
                .Get("стихотворение")
                .ToAPartOf()
                .OrderBy(s => s.FromOrder().FirstOrDefault().Name);
            foreach (var poemString in poemStrings)
            {
                var stringParts = poemString
                    .ToAPartOf()
                    .OrderBy(se => se.FromOrder().FirstOrDefault().Name);
                foreach (var stringPart in stringParts)
                {
                    var partElement = stringPart.FromElement().FirstOrDefault();
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