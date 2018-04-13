using System;
using System.Windows.Forms;
using FX.Test.Core;

namespace FX.TestTask
{
    public partial class MainForm : Form
    {
        private readonly Manager _manager;
        public MainForm()
        {
            InitializeComponent();

            _manager = new Manager();
            europianTradeOptionBindingSource.DataSource = _manager.Data;
            propertyGrid1.SelectedObject = _manager;
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog theDialog = new OpenFileDialog
                {
                    Title = @"Open Text File",
                    Filter = @"CSV files|*.csv|XML files|*.xml"
                };
                if (theDialog.ShowDialog() == DialogResult.OK)
                {
                    _manager.LoadAndSetData(theDialog.FileName);
                    totalText.Text = _manager.TotalPorfolio;
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                MessageBox.Show(this, exception.Message, @"Error");
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dataGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            var row = dataGridView.Rows[e.RowIndex].DataBoundItem as ITrade;
            var column = dataGridView.Columns[e.ColumnIndex];
            if(row==null)
                return;

            var pair = row.CCY + column.Name;
            if(!_manager.CurrencyPair.ContainsKey(pair))
                return;
            e.Value = _manager.CurrencyPair[pair] * row.StrikePrice;
        }

        private void matrixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _manager.CreateMatrix();
        }
    }
}
