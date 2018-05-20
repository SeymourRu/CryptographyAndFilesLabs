using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace CoreDefinitions.Helpers
{
    public static class BeautyfyForms
    {
        public static void SetDefaultVals(this Form form, Size formSize)
        {
            form.MaximizeBox = false;
            form.Size = formSize;
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        public static Button AddButton(string name, Point pos, EventHandler clickEvent)
        {
            var btn = new Button();
            btn.Text = name;
            btn.Click += clickEvent;
            btn.Location = pos;
            btn.Size = new Size(name.Length * 8, 24);
            return btn;
        }

        public static DataGridView AddDataGridViewHeadersOnLeft(Point pos, Size formSize, int rows, string[] headers)
        {
            var dgv = new DataGridView();
            dgv.Location = pos;
            dgv.RowCount = rows;
            dgv.Size = new Size(formSize.Width - 20, rows * 30);
            dgv.AllowUserToAddRows = false;

            for (int i = 0; i < rows - 1; i++)
            {
                dgv.Rows[i].HeaderCell.Value = headers[i];
            }

            for (int i = 0; i < dgv.Columns.Count - 1; i++)
            {
                dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            dgv.Columns[dgv.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                int colw = dgv.Columns[i].Width;
                dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgv.Columns[i].Width = colw;
            }

            return dgv;
        }

        public static DataGridView AddDataGridViewHeadersOnUp(Point pos, Size formSize, string[] headers, bool readonlyMode = true)
        {
            var dgv = new DataGridView();
            dgv.Location = pos;
            dgv.Size = new Size(formSize.Width - 20, formSize.Height - pos.Y - 40);
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = readonlyMode;
            dgv.ScrollBars = ScrollBars.Both;

            int counter = 0;
            foreach (var value in headers)
            {
                dgv.Columns.Add("value_" + counter.ToString(), value);
                counter += 1;
            }

            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            return dgv;
        }


        public static ListBox AddListBox(Point pos, Size formSize)
        {
            var listBox = new ListBox();
            listBox.Location = pos;
            listBox.Size = new Size(formSize.Width - 20, formSize.Height - pos.Y - 40);
            return listBox;
        }

        public static ProgressBar AddProgressBar(Point pos, Size formSize, int min, int max)
        {
            var progressBar = new ProgressBar();
            progressBar.Location = pos;
            //progressBar.Size = new Size(formSize.Width - 20, formSize.Height - pos.Y - 40);
            progressBar.Minimum = min;
            progressBar.Maximum = max;
            return progressBar;
        }

        public static Timer CreateTimer(EventHandler onTick, int tickInterval = 1000)
        {
            var tim = new Timer();
            tim.Interval = tickInterval;
            tim.Tick += onTick;
            return tim;
        }

        public static TextBox CreateTextBox(Point pos, bool onlyread)
        {
            var textBox = new TextBox();
            textBox.Location = pos;
            if (onlyread)
            {
                textBox.ReadOnly = true;
            }

            return textBox;
        }

        public static Label CreateLabel(Point pos, string text, bool custom = false, int len = 0)
        {
            var lbl = new Label();
            lbl.Location = pos;
            lbl.Text = text;
            if (custom)
            {
                lbl.Width = len;
            }
            else
            {
                lbl.Width = text.Length * 35;
            }

            lbl.Height = 20;
            return lbl;
        }

        public static TextBox CreateMLTextBox(Point pos, int width, int height)
        {
            var txtbx = new TextBox();
            txtbx.Location = pos;
            txtbx.Multiline = true;
            txtbx.Width = width;
            txtbx.Height = height;
            txtbx.ScrollBars = ScrollBars.Both;
            return txtbx;
        }

        public static CheckBox CreateCheckBox(Point pos, bool state)
        {
            var chkbx = new CheckBox();
            chkbx.Location = pos;

            if (state)
            {
                chkbx.CheckState = CheckState.Checked;
            }

            return chkbx;
        }

        public static NumericUpDown CreateNumericUpDown(Point pos, int min, int max, bool onlyread)
        {
            var numericUpDown = new NumericUpDown();
            numericUpDown.Location = pos;

            numericUpDown.Minimum = min;
            numericUpDown.Maximum = max;

            if (onlyread)
            {
                numericUpDown.ReadOnly = true;
            }

            return numericUpDown;
        }

        public static TreeView AddTreeView(Point pos, Size formSize)
        {
            var treeView = new TreeView();
            treeView.Location = pos;
            treeView.Size = formSize;
            return treeView;
        }
    }
}