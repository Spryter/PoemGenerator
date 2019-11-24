using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PoemGenerator.GeneratorComponent;
using PoemGenerator.OntolisAdapter;
using PoemGenerator.OntologyModel;
using PoemGenerator.OntologyModel.Abstractions;

namespace PoemGenerator.App
{
	public partial class MainForm : Form
	{
		private readonly Ontolis _ontolis;
		
		private Generator _generator;
		
		private Action _enableEditOntologyMenuItem;
		
		private Action<bool> _setGenerateButtonEnabled;

		private Action<string> _updatePoemTextBox;

		private Action<IEnumerable<IReadOnlyNode>> _updateObjectComboBox;

		private Action<IEnumerable<IReadOnlyNode>> _updateActionComboBox;

		private Action<IEnumerable<IReadOnlyNode>> _updateLocativeComboBox;

		private Func<IReadOnlyNode> _getSelectedObject;

		private Func<IReadOnlyNode> _getSelectedAction;

		private Func<IReadOnlyNode> _getSelectedLocative;
		
		public MainForm()
		{
			InitializeComponent();
            InitializeMenu();
            InitializeForm();
            _ontolis = new Ontolis();
		}

        private void InitializeMenu()
        {
            var openOntologyMenuItem = new ToolStripMenuItem("Открыть");
            openOntologyMenuItem.Click += OpenOntologyClick;

            var editOntologyMenuItem = new ToolStripMenuItem("Редактировать")
            {
                Enabled = false
            };
            _enableEditOntologyMenuItem = () => editOntologyMenuItem.Enabled = true;
            editOntologyMenuItem.Click += EditOntologyClick;

            var ontologyMenuItem = new ToolStripMenuItem("Онтология");
            ontologyMenuItem.DropDownItems.Add(openOntologyMenuItem);
            ontologyMenuItem.DropDownItems.Add(editOntologyMenuItem);

            var menu = new MenuStrip();
            menu.Items.Add(ontologyMenuItem);

            Controls.Add(menu);
        }

        private Button CreateGenerateButton()
        {
	        var buttonGenerate = new Button
	        {
		        Text = @"Сгенерировать стихотворение",
		        Enabled = false,
		        Dock = DockStyle.Fill
	        };
	        buttonGenerate.Click += ButtonGenerateClick;
	        _setGenerateButtonEnabled = enabled => buttonGenerate.Enabled = enabled;

	        return buttonGenerate;
        }

        private RichTextBox CreatePoemTextBox()
        {
	        var poemTextBox = new RichTextBox
	        {
		        Dock = DockStyle.Fill
	        };
	        _updatePoemTextBox = poem => poemTextBox.Text = poem;

	        return poemTextBox;
        }

        private Label CreateSituationLabel(string text)
        {
	        var label = new Label
	        {
		        Text = text,
		        Dock = DockStyle.Fill
	        };

	        return label;
        }

        private ComboBox CreateSiatuationComboBox(out Action<IEnumerable<IReadOnlyNode>> updateSourceAction,
	        out Func<IReadOnlyNode> getSelectedItemFunc)
        {
	        var comboBox = new ComboBox
	        {
		        Dock = DockStyle.Fill,
		        DisplayMember = "Name"
	        };
	        updateSourceAction = nodes => comboBox.DataSource = nodes.ToList();
	        getSelectedItemFunc = () => (IReadOnlyNode) comboBox.SelectedItem;

	        return comboBox;
        }

        private TableLayoutPanel CreateSituationElement(string name,
	        out Action<IEnumerable<IReadOnlyNode>> updateSourceAction, out Func<IReadOnlyNode> getSelectedItemFunc)
        {
	        var labelObject = CreateSituationLabel(name);
	        var objectComboBox = CreateSiatuationComboBox(out updateSourceAction, out getSelectedItemFunc);

	        var table = new TableLayoutPanel
	        {
		        Dock = DockStyle.Fill
	        };
	        table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
	        table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
	        table.Controls.Add(labelObject, 0, 0);
	        table.Controls.Add(objectComboBox, 0, 1);

	        return table;
        }

        private GroupBox CreateSituationGroupBox()
        {
			var objectElement = CreateSituationElement("Предмет", out _updateObjectComboBox, out _getSelectedObject);
			var actionElement = CreateSituationElement("Действие", out _updateActionComboBox, out _getSelectedAction);
			var locativeElement = CreateSituationElement("Локатив", out _updateLocativeComboBox, out _getSelectedLocative);

			var table = new TableLayoutPanel
	        {
				Dock = DockStyle.Fill
	        };
	        table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
	        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
	        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
	        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));

	        table.Controls.Add(objectElement, 0, 0);
	        table.Controls.Add(actionElement, 1, 0);
	        table.Controls.Add(locativeElement, 2, 0);

	        var groupBox = new GroupBox
	        {
		        Dock = DockStyle.Fill,
		        Text = @"Безопасная ситуация"
	        };
	        groupBox.Controls.Add(table);

	        return groupBox;
        }

        private void InitializeForm()
        {
	        var table = new TableLayoutPanel
	        {
				Dock = DockStyle.Fill
	        };
	        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
	        table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
	        table.RowStyles.Add(new RowStyle(SizeType.Percent, 80));
	        table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
	        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

	        table.Controls.Add(CreateSituationGroupBox(), 0, 1);
	        table.Controls.Add(CreatePoemTextBox(), 0, 2);
	        table.Controls.Add(CreateGenerateButton(), 0, 3);

	        Controls.Add(table);
        }
        
        private void FillComboBoxes()
        {
	        var safeSituationNode = _generator.Ontology.Nodes.Get("безопасная ситуация");
	        var safeSituationDotNode = safeSituationNode.ToIsA().FirstOrDefault();
	        _updateObjectComboBox(safeSituationDotNode.ToIsANestedFromObject());
	        _updateActionComboBox(safeSituationDotNode.ToIsANestedFromAction());
	        _updateLocativeComboBox(safeSituationDotNode.ToIsANestedFromLocative());
        }

		private void ButtonGenerateClick(object sender, EventArgs e)
		{
			var child = _generator.GenerateChild();
			
			var builder = new StringBuilder();
			builder.Append($"{child} {_getSelectedAction()} {_getSelectedObject()} {_getSelectedLocative()}");
			builder.Append(Environment.NewLine);
			builder.Append($"{child} {_generator.GenerateDangerSituation()}");
			
			_updatePoemTextBox(builder.ToString());
		}

		private void OpenOntologyClick(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog
            {
                Filter = @"Файлы онтологий (*.ont)|*.ont",
                Title = @"Открыть файл онтологии"
            })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var ontology = _ontolis.LoadByPath(openFileDialog.FileName);
                    _generator = new Generator(ontology);
                    _setGenerateButtonEnabled(true);
                    _enableEditOntologyMenuItem();
                    FillComboBoxes();
				}
            }
        }
		
		private void EditOntologyClick(object sender, EventArgs e)
		{
			Visible = false;
			_ontolis.Open();
			Visible = true;
			var ontology = _ontolis.Reload();
			_generator = new Generator(ontology);
			FillComboBoxes();
		}
	}
}
