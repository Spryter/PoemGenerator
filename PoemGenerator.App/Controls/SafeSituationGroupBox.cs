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

        private ComboBox _object;

        private ComboBox _action;

        private ComboBox _locative;

        private IEnumerable<IReadOnlyNode> _objectDataSource;

        private IEnumerable<IReadOnlyNode> _actionDataSource;

        private IEnumerable<IReadOnlyNode> _locativeDataSource;

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
            var actionElement = CreateSituationElement("Действие", out _action);
            var objectElement = CreateSituationElement("Предмет", out _object);
            var locativeElement = CreateSituationElement("Локатив", out _locative);

            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill
            };
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));

            table.Controls.Add(actionElement, 0, 0);
            table.Controls.Add(objectElement, 1, 0);
            table.Controls.Add(locativeElement, 2, 0);

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
            ActionDataSource = _actionDataSource;
            ObjectDataSource = _objectDataSource;
            LocativeDataSource = _locativeDataSource;
        }
        
        private void SelectedItemChanged(object obj, EventArgs args)
        {
            var actionSelectedItem = (IReadOnlyNode)_action.SelectedItem;
            var objectSelectedItem = (IReadOnlyNode)_object.SelectedItem;
            var locativeSelectedItem = (IReadOnlyNode)_locative.SelectedItem;

            UpdateDataSource();

            if (actionSelectedItem.Name != EmptyNodeName)
            {
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
                ActionDataSource = _actionDataSource
                    .Intersect(GetRelevant(locativeSelectedItem, Relations.Locative, Relations.Action))
                    .ToNodeCollection();
                _actionDataSource = ActionDataSource;
                ObjectDataSource = _objectDataSource
                    .Intersect(GetRelevant(locativeSelectedItem, Relations.Locative, Relations.Object))
                    .ToNodeCollection();
                _objectDataSource = ObjectDataSource;
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

        public Situation GetSituation()
        {
            var situation = new SafeSituation();
            var selectedAction = (IReadOnlyNode) _action.SelectedItem;
            var selectedObject = (IReadOnlyNode) _object.SelectedItem;
            var selectedLocative = (IReadOnlyNode) _locative.SelectedItem;

            if (selectedAction.Name != EmptyNodeName)
            {
                situation.Action = selectedAction;
            }
            else
            {
                var actionItem = _actionDataSource.ToNodeCollection().GetRandom();
                situation.Action = actionItem;
                _objectDataSource = _objectDataSource
                    .Intersect(GetRelevant(actionItem, Relations.Action, Relations.Object));
                _locativeDataSource = _locativeDataSource
                    .Intersect(GetRelevant(actionItem, Relations.Action, Relations.Locative));
            }

            if (selectedObject.Name != EmptyNodeName)
            {
                situation.Object = selectedObject;
            }
            else
            {
                var objectItem = _objectDataSource.ToNodeCollection().GetRandom();
                situation.Object = objectItem;
                _locativeDataSource = _locativeDataSource
                    .Intersect(GetRelevant(objectItem, Relations.Object, Relations.Locative));
            }

            situation.Locative = selectedLocative.Name != EmptyNodeName
                ? selectedLocative
                : _locativeDataSource.ToNodeCollection().GetRandom();

            _objectDataSource = ObjectDataSource;
            _locativeDataSource = LocativeDataSource;

            return situation;
        }
    }
}