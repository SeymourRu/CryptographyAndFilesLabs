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

        public static DataGridView AddDataGridView(Point pos, Size formSize, int rows, string[] headers)
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

        public static Timer CreateTimer(EventHandler onTick,int tickInterval = 1000)
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

        public static Label CreateLabel(Point pos,string text)
        {
            var lbl = new Label();
            lbl.Location = pos;
            lbl.Text = text;
            lbl.Width = text.Length * 35;
            lbl.Height = 20;
            return lbl;
        }
    }
}