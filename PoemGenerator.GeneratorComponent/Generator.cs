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

        public DangerousSituation GenerateDangerousSituation()
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
    }
}