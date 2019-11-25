using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PoemGenerator.GeneratorComponent;
using PoemGenerator.OntologyModel;
using PoemGenerator.OntologyModel.Abstractions;

namespace PoemGenerator.App
{
    public partial class SafeSituationGroupBox : UserControl
    {
        private const string MainFrameNodeName = "*";
        
        private const string EmptyNodeName = "пусто";
        
        private const int EmptyNodeId = -1;

        private Generator _generator;
        
        private ComboBox _object;
        
        private ComboBox _action;
        
        private ComboBox _locative;

        private IEnumerable<IReadOnlyNode> ObjectDataSource
        {
            get => (IEnumerable<IReadOnlyNode>)_object.DataSource;
            set => _object.DataSource = new [] {new Node(EmptyNodeId, EmptyNodeName)}.Union(value).ToList();
        }

        private IEnumerable<IReadOnlyNode> ActionDataSource
        {
            get => (IEnumerable<IReadOnlyNode>)_action.DataSource;
            set => _action.DataSource = new [] {new Node(EmptyNodeId, EmptyNodeName)}.Union(value).ToList();
        }

        private IEnumerable<IReadOnlyNode> LocativeDataSource
        {
            get => (IEnumerable<IReadOnlyNode>)_locative.DataSource;
            set => _locative.DataSource = new [] {new Node(EmptyNodeId, EmptyNodeName)}.Union(value).ToList();
        }

        public IReadOnlyNode ObjectSelectedItem => ((IReadOnlyNode) _object.SelectedItem).Name == EmptyNodeName
            ? GetSelected(Relations.Object, ObjectDataSource, ref _objectIsSelected)
            : (IReadOnlyNode) _object.SelectedItem;

        public IReadOnlyNode ActionSelectedItem => ((IReadOnlyNode) _action.SelectedItem).Name == EmptyNodeName
            ? GetSelected(Relations.Action, ActionDataSource, ref _actionIsSelected)
            : (IReadOnlyNode) _action.SelectedItem;

        public IReadOnlyNode LocativeSelectedItem => ((IReadOnlyNode) _locative.SelectedItem).Name == EmptyNodeName
            ? GetSelected(Relations.Locative, LocativeDataSource, ref _locativeIsSelected)
            : (IReadOnlyNode) _locative.SelectedItem;

        #region Обвес для генерации, если все пусто
        
        /*
         * То что снизу это просто полная дичь, мне стыдно за этот код.
         *
         * По хорошему надо прегенерить значения для незаполненных комбобоксов,
         * тогда можно будет избежать кучи ненужных полей и использование ref.
        */
        
        private bool _actionIsSelected;

        private bool _objectIsSelected;

        private bool _locativeIsSelected;

        private IReadOnlyNode _frameNode;

        private bool IsEmpty =>
            ((IReadOnlyNode) _object.SelectedItem).Name == EmptyNodeName &&
            ((IReadOnlyNode) _action.SelectedItem).Name == EmptyNodeName &&
            ((IReadOnlyNode) _locative.SelectedItem).Name == EmptyNodeName;
        
        private IReadOnlyNode GetSelected(string relationName, IEnumerable<IReadOnlyNode> nodes, ref bool isSelected)
        {
            if (!IsEmpty)
                return nodes.Skip(1).ToNodeCollection().GetRandom();

            if (_frameNode == null)
            {
                _frameNode = GetSafeSituationFrameNode(nodes.Skip(1).ToNodeCollection().GetRandom());
            }

            isSelected = true;
            var result = GetRelevantNodes(_frameNode, relationName)
                .ToNodeCollection()
                .GetRandom();
            
            if (_actionIsSelected && _objectIsSelected && _locativeIsSelected)
            {
                _actionIsSelected = false;
                _objectIsSelected = false;
                _locativeIsSelected = false;
                _frameNode = null;
            }

            return result;
        }

        #endregion
        
        public Generator Generator
        {
            get => _generator;
            set
            {
                _generator = value;
                FillComboBoxes();
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
            comboBox.SelectionChangeCommitted += UpdateComboBoxes;

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

        private void FillComboBoxes()
        {
            var safeSituation = Generator.Ontology.Nodes.Get("безопасная ситуация").ToIsA();
            ActionDataSource = safeSituation.SelectMany(x => x.ToIsANestedFromAction());
            ObjectDataSource = safeSituation.SelectMany(x => x.ToIsANestedFromObject());
            LocativeDataSource = safeSituation.SelectMany(x => x.ToIsANestedFromLocative());
        }

        private static IReadOnlyNode GetSafeSituationFrameNode(IReadOnlyNode frameElementNode)
        {
            var objectRelation = frameElementNode.ToObject().FirstOrDefault(x => x.FromIsANested().Count(y => y.Name == "безопасная ситуация") > 0);
            var actionRelation = frameElementNode.ToAction().FirstOrDefault(x => x.FromIsANested().Count(y => y.Name == "безопасная ситуация") > 0);
            var locativeRelation = frameElementNode.ToLocative().FirstOrDefault(x => x.FromIsANested().Count(y => y.Name == "безопасная ситуация") > 0);

            return objectRelation ?? actionRelation ?? locativeRelation;
        }

        private static IEnumerable<IReadOnlyNode> GetRelevantNodes(IReadOnlyNode node, string relationName)
        {
            return node
                .From(relationName)
                .Union(node
                    .FromIsANested()
                    .Where(x => x.Name != MainFrameNodeName)
                    .SelectMany(x => x.From(relationName))
                    .Union(node.ToIsANestedFrom(relationName)));
        }

        private void UpdateComboBoxes(object obj, EventArgs args)
        {
            var actionSelectedItem = (IReadOnlyNode)_action.SelectedItem;
            var objectSelectedItem = (IReadOnlyNode)_object.SelectedItem;
            var locativeSelectedItem = (IReadOnlyNode)_locative.SelectedItem;

            var mainNode = actionSelectedItem != null && actionSelectedItem.Name != EmptyNodeName ? actionSelectedItem :
                objectSelectedItem != null && objectSelectedItem.Name != EmptyNodeName ? objectSelectedItem :
                locativeSelectedItem != null && locativeSelectedItem.Name != EmptyNodeName ? locativeSelectedItem :
                new Node(EmptyNodeId, EmptyNodeName);

            var frameNode = GetSafeSituationFrameNode(mainNode);
            
            if (frameNode != null)
            {
                ActionDataSource = GetRelevantNodes(frameNode, Relations.Action);
                ObjectDataSource = GetRelevantNodes(frameNode, Relations.Object);
                LocativeDataSource = GetRelevantNodes(frameNode, Relations.Locative);
                _action.SelectedItem = actionSelectedItem;
                _object.SelectedItem = objectSelectedItem;
                _locative.SelectedItem = locativeSelectedItem;
            }
            else
            {
                FillComboBoxes();
            }
        }
    }
}