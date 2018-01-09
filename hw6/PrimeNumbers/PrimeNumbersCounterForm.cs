using System;
using System.Windows.Forms;

namespace PrimeNumbers
{
    public partial class PrimeNumbersCounterForm : Form
    {
        public PrimeNumbersCounterForm()
        {
            InitializeComponent();
        }


        private void AddNewRowToInnerTableLayoutPanel()
        {
            innerTableLayoutPanel.RowCount++;
            innerTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        }

        private void InitializeGUIComponentsInAddedRow() => new PrimeNumberTaskControls(innerTableLayoutPanel, innerTableLayoutPanel.RowCount - 1);

        private void addNewTaskButton_Click(object sender, EventArgs e)
        {
            AddNewRowToInnerTableLayoutPanel();
            InitializeGUIComponentsInAddedRow();
        }
    }
}
