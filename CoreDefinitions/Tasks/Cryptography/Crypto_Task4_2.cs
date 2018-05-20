using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreDefinitions.Helpers;
using CoreDefinitions.Helpers.ConsoleHelper;
using System.Windows.Forms;
using System.Drawing;
using System.Numerics;

namespace CoreDefinitions.Tasks
{
    public class Crypto_Task4_2 : ITask<Crypto_Task4_2>, IBaseTask
    {
        TaskAppType _subSystemType;

        TextBox _inputValue;
        TextBox _inputMod;
        Button _addNew;
        NumericUpDown _cellsPerHash;
        Button _acceptFunction;
        Button _selectNewFunction;
        DataGridView _table;

        int curValue = 0;
        int size = 0;

        public TaskAppType SubSystemType
        {
            get
            {
                return _subSystemType;
            }
        }

        public Crypto_Task4_2()
        {
            _subSystemType = Helpers.TaskAppType.GUI;
        }

        public void LocateControls(Form form, ConsoleHandler console)
        {
            form.Text = "Задание № 2";
            form.SetDefaultVals(new System.Drawing.Size(810, 500));

            form.Controls.Add(BeautyfyForms.AddButton(" Суть ", new Point(170, 10), (o, k) =>
            {
                MessageBox.Show("Задача № 2 Алгоритм разрешения коллизий в хэш-таблицах методом открытой адресации. \r\n Создаётся таблица с заданным кол-вом слотов под каждое значение хеша.");
            }));

            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(270, 46), "Хеширующая функция", true, 125));

            _inputValue = BeautyfyForms.CreateTextBox(new Point(200, 66), false);
            form.Controls.Add(_inputValue);

            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(320, 66), "mod", true, 35));

            _inputMod = BeautyfyForms.CreateTextBox(new Point(360, 66), false);
            form.Controls.Add(_inputMod);

            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(480, 46), "Кол-во ячеек для 1 хеша", true, 140));

            _cellsPerHash = BeautyfyForms.CreateNumericUpDown(new Point(490, 66), 1, 50, false);
            form.Controls.Add(_cellsPerHash);

            _acceptFunction = BeautyfyForms.AddButton(" Подтвердить выбор ", new Point(620, 33), (o, k) =>
            {
                var value = 0;
                if (int.TryParse(_inputMod.Text, out value))
                {
                    curValue = value;
                    _inputMod.ReadOnly = true;
                    _cellsPerHash.ReadOnly = true;
                    _addNew.Visible = true;

                    size = (int)_cellsPerHash.Value;

                    _table.Rows.Clear();
                    _table.Rows.Add();
                    _table.Rows.AddCopies(0, (curValue * size) - 1);
                    _table.ReadOnly = true;
                }
                else
                {
                    MessageBox.Show("Invalid mod value");
                }
            });
            form.Controls.Add(_acceptFunction);

            _selectNewFunction = BeautyfyForms.AddButton("Выбрать другую функцию", new Point(615, 66), (o, k) =>
            {
                _inputMod.ReadOnly = false;
                _cellsPerHash.ReadOnly = false;
                _addNew.Visible = false;
                _table.Rows.Clear();
            });
            form.Controls.Add(_selectNewFunction);

            _addNew = BeautyfyForms.AddButton(" Добавить ", new Point(280, 99), (o, k) =>
            {
                var value = 0;
                if (int.TryParse(_inputValue.Text, out value))
                {
                    var hash = value % curValue;
                    var tableValue = _table.Rows[hash * size].Cells[0].Value;

                    //No value yet
                    if ((tableValue == null) || (tableValue == ""))
                    {
                        _table.Rows[hash * size].Cells[0].Value = hash;
                        _table.Rows[hash * size].Cells[1].Value = value;
                    }
                    else
                    {
                        if (_table.Rows[hash * size].Cells[1].Value.ToString() == value.ToString())
                        {
                            MessageBox.Show("Such value already exist in table");
                            return;
                        }

                        var firstIndx = hash * size + 1;
                        var lastIndx = firstIndx + size - 1;
                        for (; firstIndx < lastIndx; firstIndx++)
                        {
                            tableValue = _table.Rows[firstIndx].Cells[0].Value;
                            if ((tableValue == null) || (tableValue == ""))
                            {
                                _table.Rows[firstIndx].Cells[0].Value = hash;
                                _table.Rows[firstIndx].Cells[1].Value = value;
                                return;
                            }
                            else
                            {
                                if (_table.Rows[firstIndx].Cells[1].Value.ToString() == value.ToString())
                                {
                                    MessageBox.Show("Such value already exist in table");
                                    return;
                                }
                            }
                        }
                        MessageBox.Show("Max num of slots reached. Unable to insert hash.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid x value");
                }
            });
            _addNew.Visible = false;
            form.Controls.Add(_addNew);

            _table = BeautyfyForms.AddDataGridViewHeadersOnUp(new Point(0, 200), form.Size, new string[] { "Значение хеша", "Собственно значение" });
            form.Controls.Add(_table);
        }
    }
}
