using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PoemGenerator.GeneratorComponent;
using PoemGenerator.GeneratorComponent.Constants;
using PoemGenerator.GeneratorComponent.Extensions;
using PoemGenerator.GeneratorComponent.Situations;
using PoemGenerator.OntologyModel.Abstractions;

namespace PoemGenerator.App.Controls
{
    public partial class SafeSituationGroupBox : UserControl
    {
        private Generator _generator;

        private SafeSituation _situation;
        
        private ComboBox _agent;

        private ComboBox _object;

        private ComboBox _action;

        private ComboBox _locative;
        
        private IEnumerable<IReadOnlyNode> AgentDataSource
        {
            set => _agent.DataSource = GetDataSourceWithEmptyNode(value);
        }

        private IEnumerable<IReadOnlyNode> ObjectDataSource
        {
            set => _object.DataSource = GetDataSourceWithEmptyNode(value);
        }

        private IEnumerable<IReadOnlyNode> ActionDataSource
        {
            set => _action.DataSource = GetDataSourceWithEmptyNode(value);
        }

        private IEnumerable<IReadOnlyNode> LocativeDataSource
        {
            set => _locative.DataSource = GetDataSourceWithEmptyNode(value);
        }
        
        public Generator Generator
        {
            get => _generator;
            set
            {
                _generator = value;
                _situation = new SafeSituation();
                UpdateDataSource();
            }
        }

        public SafeSituationGroupBox()
        {
            InitializeComponent();
            InitializeGroupBox();
            _situation = new SafeSituation();
        }

        private static List<IReadOnlyNode> GetDataSourceWithEmptyNode(IEnumerable<IReadOnlyNode> nodes) =>
            new[] {new EmptyOntologyNode()}.Union(nodes).ToList();

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
            var relevantNodes = _generator.GetRelevantNodes(_situation, Nodes.SafeSituation);
            ActionDataSource = relevantNodes.Actions;
            AgentDataSource = relevantNodes.Agents;
            ObjectDataSource = relevantNodes.Objects;
            LocativeDataSource = relevantNodes.Locatives;
        }
        
        private void SelectedItemChanged(object obj, EventArgs args)
        {
            _situation = new SafeSituation
            {
                Action = (IReadOnlyNode) _action.SelectedItem,
                Agent = (IReadOnlyNode) _agent.SelectedItem,
                Object = (IReadOnlyNode) _object.SelectedItem,
                Locative = (IReadOnlyNode) _locative.SelectedItem
            };

            UpdateDataSource();

            _agent.SelectedItem = _situation.Agent;
            _action.SelectedItem = _situation.Action;
            _object.SelectedItem = _situation.Object;
            _locative.SelectedItem = _situation.Locative;
        }

        public SafeSituation GetSituation()
        {
            return _generator.GenerateSituation(_situation, Nodes.SafeSituation).ToSafeSituation();
        }
    }
}