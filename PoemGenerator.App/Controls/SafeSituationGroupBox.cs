using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PoemGenerator.GeneratorComponent;
using PoemGenerator.GeneratorComponent.Constants;
using PoemGenerator.GeneratorComponent.Extensions;
using PoemGenerator.GeneratorComponent.Situations;
using PoemGenerator.OntologyModel;
using PoemGenerator.OntologyModel.Abstractions;

namespace PoemGenerator.App.Controls
{
    public partial class SafeSituationGroupBox : UserControl
    {
        private const string EmptyNodeName = "пусто";

        private const int EmptyNodeId = -1;

        private Generator _generator;
        
        private ComboBox _agent;

        private ComboBox _object;

        private ComboBox _action;

        private ComboBox _locative;

        private IEnumerable<IReadOnlyNode> _agentDataSource;

        private IEnumerable<IReadOnlyNode> _objectDataSource;

        private IEnumerable<IReadOnlyNode> _actionDataSource;

        private IEnumerable<IReadOnlyNode> _locativeDataSource;
        
        private IEnumerable<IReadOnlyNode> AgentDataSource
        {
            get => ((IEnumerable<IReadOnlyNode>)_agent.DataSource).Skip(1);
            set => _agent.DataSource = new[] {new OntologyNode(EmptyNodeId, EmptyNodeName)}.Union(value).ToList();
        }

        private IEnumerable<IReadOnlyNode> ObjectDataSource
        {
            get => ((IEnumerable<IReadOnlyNode>)_object.DataSource).Skip(1);
            set => _object.DataSource = new[] {new OntologyNode(EmptyNodeId, EmptyNodeName)}.Union(value).ToList();
        }

        private IEnumerable<IReadOnlyNode> ActionDataSource
        {
            get => ((IEnumerable<IReadOnlyNode>)_action.DataSource).Skip(1);
            set => _action.DataSource = new[] {new OntologyNode(EmptyNodeId, EmptyNodeName)}.Union(value).ToList();
        }

        private IEnumerable<IReadOnlyNode> LocativeDataSource
        {
            get => ((IEnumerable<IReadOnlyNode>)_locative.DataSource).Skip(1);
            set => _locative.DataSource = new[] {new OntologyNode(EmptyNodeId, EmptyNodeName)}.Union(value).ToList();
        }
        
        public Generator Generator
        {
            get => _generator;
            set
            {
                _generator = value;
                UpdateDataSource();
            }
        }

        public SafeSituationGroupBox()
        {
            InitializeComponent();
            InitializeGroupBox();
        }

        private static Label CreateSituationLabel(string text)
        {
            var label = new Label
            {
                Text = text,
                Dock = DockStyle.Fill
            };

            return label;
        }

        private ComboBox CreateSituationComboBox()
        {
            var comboBox = new ComboBox
            {
                Dock = DockStyle.Fill,
                DisplayMember = "Name"
            };
            comboBox.SelectionChangeCommitted += SelectedItemChanged;

            return comboBox;
        }

        private TableLayoutPanel CreateSituationElement(string name, out ComboBox comboBox)
        {
            var labelObject = CreateSituationLabel(name);
            comboBox = CreateSituationComboBox();

            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill
            };
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            table.Controls.Add(labelObject, 0, 0);
            table.Controls.Add(comboBox, 0, 1);

            return table;
        }

        private void InitializeGroupBox()
        {
            var agentElement = CreateSituationElement("Актор", out _agent);
            var actionElement = CreateSituationElement("Действие", out _action);
            var objectElement = CreateSituationElement("Предмет", out _object);
            var locativeElement = CreateSituationElement("Локатив", out _locative);

            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill
            };
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));

            table.Controls.Add(agentElement, 0, 0);
            table.Controls.Add(actionElement, 1, 0);
            table.Controls.Add(objectElement, 2, 0);
            table.Controls.Add(locativeElement, 3, 0);

            var groupBox = new GroupBox
            {
                Dock = DockStyle.Fill,
                Text = @"Безопасная ситуация"
            };
            groupBox.Controls.Add(table);

            Controls.Add(groupBox);
        }

        private void UpdateDataSource()
        {
            var safeSituation = _generator.Ontology.Nodes.Get(Nodes.SafeSituation).ToIsA();
            _actionDataSource = safeSituation.SelectMany(x => x.ToIsANestedFromAction()).ToNodeCollection();
            _objectDataSource = safeSituation.SelectMany(x => x.ToIsANestedFromObject()).ToNodeCollection();
            _locativeDataSource = safeSituation.SelectMany(x => x.ToIsANestedFromLocative()).ToNodeCollection();
            _agentDataSource = safeSituation
                .SelectMany(x => x.ToIsANestedFromAgent())
                .Union(safeSituation
                    .SelectMany(x => x.ToIsANestedFromAgent())
                    .SelectMany(x => x.ToIsANested()))
                .Distinct()
                .ToNodeCollection();
            ActionDataSource = _actionDataSource;
            ObjectDataSource = _objectDataSource;
            LocativeDataSource = _locativeDataSource;
            AgentDataSource = _agentDataSource;
        }
        
        private void SelectedItemChanged(object obj, EventArgs args)
        {
            var agentSelectedItem = (IReadOnlyNode) _agent.SelectedItem;
            var actionSelectedItem = (IReadOnlyNode)_action.SelectedItem;
            var objectSelectedItem = (IReadOnlyNode)_object.SelectedItem;
            var locativeSelectedItem = (IReadOnlyNode)_locative.SelectedItem;

            UpdateDataSource();

            if (agentSelectedItem.Name != EmptyNodeName)
            {
                var agent = GetParentAgent(agentSelectedItem);
                ActionDataSource = _actionDataSource
                    .Intersect(GetRelevant(agent, Relations.Agent, Relations.Action))
                    .ToNodeCollection();
                _actionDataSource = ActionDataSource;
                ObjectDataSource = _objectDataSource
                    .Intersect(GetRelevant(agent, Relations.Agent, Relations.Object))
                    .ToNodeCollection();
                _objectDataSource = ObjectDataSource;
                LocativeDataSource = _locativeDataSource
                    .Intersect(GetRelevant(agent, Relations.Agent, Relations.Locative))
                    .ToNodeCollection();
                _locativeDataSource = LocativeDataSource;
            }

            if (actionSelectedItem.Name != EmptyNodeName)
            {
                var relevantAgents = GetRelevant(actionSelectedItem, Relations.Action, Relations.Agent).ToList();
                AgentDataSource = _agentDataSource
                    .Intersect(relevantAgents
                        .Union(relevantAgents
                            .SelectMany(x => x.ToIsANested()))
                        .Distinct())
                    .ToNodeCollection();
                _agentDataSource = AgentDataSource;
                ObjectDataSource = _objectDataSource
                    .Intersect(GetRelevant(actionSelectedItem, Relations.Action, Relations.Object))
                    .ToNodeCollection();
                _objectDataSource = ObjectDataSource;
                LocativeDataSource = _locativeDataSource
                    .Intersect(GetRelevant(actionSelectedItem, Relations.Action, Relations.Locative))
                    .ToNodeCollection();
                _locativeDataSource = LocativeDataSource;
            }

            if (objectSelectedItem.Name != EmptyNodeName)
            {
                var relevantAgents = GetRelevant(objectSelectedItem, Relations.Object, Relations.Agent).ToList();
                AgentDataSource = _agentDataSource
                    .Intersect(relevantAgents
                        .Union(relevantAgents
                            .SelectMany(x => x.ToIsANested()))
                        .Distinct())
                    .ToNodeCollection();
                _agentDataSource = AgentDataSource;
                ActionDataSource = _actionDataSource
                    .Intersect(GetRelevant(objectSelectedItem, Relations.Object, Relations.Action))
                    .ToNodeCollection();
                _actionDataSource = ActionDataSource;
                LocativeDataSource = _locativeDataSource
                    .Intersect(GetRelevant(objectSelectedItem, Relations.Object, Relations.Locative))
                    .ToNodeCollection();
                _locativeDataSource = LocativeDataSource;
            }

            if (locativeSelectedItem.Name != EmptyNodeName)
            {
                var relevantAgents = GetRelevant(locativeSelectedItem, Relations.Locative, Relations.Agent).ToList();
                AgentDataSource = _agentDataSource
                    .Intersect(relevantAgents
                        .Union(relevantAgents
                            .SelectMany(x => x.ToIsANested()))
                        .Distinct())
                    .ToNodeCollection();
                _agentDataSource = AgentDataSource;
                ActionDataSource = _actionDataSource
                    .Intersect(GetRelevant(locativeSelectedItem, Relations.Locative, Relations.Action))
                    .ToNodeCollection();
                _actionDataSource = ActionDataSource;
                ObjectDataSource = _objectDataSource
                    .Intersect(GetRelevant(locativeSelectedItem, Relations.Locative, Relations.Object))
                    .ToNodeCollection();
                _objectDataSource = ObjectDataSource;
            }

            if (agentSelectedItem.Name != EmptyNodeName)
            {
                _agent.SelectedItem = agentSelectedItem;
            }

            if (actionSelectedItem.Name != EmptyNodeName)
            {
                _action.SelectedItem = actionSelectedItem;
            }

            if (objectSelectedItem.Name != EmptyNodeName)
            {
                _object.SelectedItem = objectSelectedItem;
            }

            if (locativeSelectedItem.Name != EmptyNodeName)
            {
                _locative.SelectedItem = locativeSelectedItem;
            }

            _agentDataSource = AgentDataSource;
            _actionDataSource = ActionDataSource;
            _objectDataSource = ObjectDataSource;
            _locativeDataSource = LocativeDataSource;
        }

        /// <summary>
        /// Возвращает множество узлов, которые подходят по иерархии с заданными связями
        /// </summary>
        /// <param name="node">Узел части фрейма.</param>
        /// <param name="toRelation">Связь части фрейма.</param>
        /// <param name="fromRelation">Узлы со связями которые надо найти.</param>
        /// <returns>Множество узлов.</returns>
        private static IEnumerable<IReadOnlyNode> GetRelevant(IReadOnlyNode node, string toRelation,
            string fromRelation)
        {
            return node.To(toRelation)
                .SelectMany(x => x.From(fromRelation))
                .Union(node
                    .To(toRelation)
                    .SelectMany(x => x
                        .ToIsANestedFrom(fromRelation)
                        .Union(x
                            .FromIsANested()
                            .Where(y => y.Name != Nodes.MainFrameNode)
                            .SelectMany(y => y.From(fromRelation))))
                );
        }

        private static IReadOnlyNode GetParentAgent(IReadOnlyNode agent)
        {
            var agents = new Queue<IReadOnlyNode>();
            agents.Enqueue(agent);
            while (agents.Count > 0)
            {
                agent = agents.Dequeue();
                if (agent.ToRelations.Any(x => x.Name == Relations.Agent))
                    return agent;
                foreach (var node in agent.FromIsA())
                {
                    agents.Enqueue(node);
                }
            }

            throw new ArgumentException();
        }

        public SafeSituation GetSituation()
        {
            var situation = new SafeSituation();
            var selectedAgent = (IReadOnlyNode) _agent.SelectedItem;
            var selectedAction = (IReadOnlyNode) _action.SelectedItem;
            var selectedObject = (IReadOnlyNode) _object.SelectedItem;
            var selectedLocative = (IReadOnlyNode) _locative.SelectedItem;

            if (selectedAgent.Name != EmptyNodeName)
            {
                situation.Agent = selectedAgent;
            }
            else
            {
                if (_agentDataSource.Any())
                {
                    var agentItem = _agentDataSource.ToNodeCollection().GetRandom();
                    situation.Agent = agentItem;
                    var agent = GetParentAgent(agentItem);
                    _actionDataSource = _actionDataSource
                        .Intersect(GetRelevant(agent, Relations.Agent, Relations.Action));
                    _objectDataSource = _objectDataSource
                        .Intersect(GetRelevant(agent, Relations.Agent, Relations.Object));
                    _locativeDataSource = _locativeDataSource
                        .Intersect(GetRelevant(agent, Relations.Agent, Relations.Locative));
                }
                else
                {
                    situation.Agent = new OntologyNode(-1, "");
                }
            }

            if (selectedAction.Name != EmptyNodeName)
            {
                situation.Action = selectedAction;
            }
            else
            {
                if (_actionDataSource.Any())
                {
                    var actionItem = _actionDataSource.ToNodeCollection().GetRandom();
                    situation.Action = actionItem;
                    _objectDataSource = _objectDataSource
                        .Intersect(GetRelevant(actionItem, Relations.Action, Relations.Object));
                    _locativeDataSource = _locativeDataSource
                        .Intersect(GetRelevant(actionItem, Relations.Action, Relations.Locative));
                }
                else
                {
                    situation.Action = new OntologyNode(-1, "");
                }
            }

            if (selectedObject.Name != EmptyNodeName)
            {
                situation.Object = selectedObject;
            }
            else
            {
                if (_objectDataSource.Any())
                {
                    var objectItem = _objectDataSource.ToNodeCollection().GetRandom();
                    situation.Object = objectItem;
                    _locativeDataSource = _locativeDataSource
                        .Intersect(GetRelevant(objectItem, Relations.Object, Relations.Locative));
                }
                else
                {
                    situation.Object = new OntologyNode(-1, "");
                }
            }

            situation.Locative = selectedLocative.Name != EmptyNodeName
                ? selectedLocative
                : _locativeDataSource.Any()
                    ? _locativeDataSource.ToNodeCollection().GetRandom()
                    : new OntologyNode(-1, "");

            _actionDataSource = ActionDataSource;
            _objectDataSource = ObjectDataSource;
            _locativeDataSource = LocativeDataSource;

            return situation;
        }
    }
}