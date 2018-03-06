using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreDefinitions.Helpers;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using CoreDefinitions.Helpers.ConsoleHelper;

namespace CoreDefinitions.Tasks
{
    public class Files_Task1_3 : ITask<Files_Task1_3>, IBaseTask
    {
        //system props
        TaskAppType _subSystemType;

        //controlls
        List<Button> _buttonsToHide = new List<Button>();
        ListBox logLstBox;
        ProgressBar _progress;
        TextBox _input;
        Label _info;
        Timer _timerProgress;
        Timer _timerBackup;

        //required vars
        string openedFile = "";
        SelfOrganizeIndexHeader _header;
        LinkedList<SelfOrganizeIndexNode> _linkedList = null;

        public TaskAppType SubSystemType
        {
            get
            {
                return _subSystemType;
            }
        }

        public Files_Task1_3()
        {
            _subSystemType = Helpers.TaskAppType.GUI;
        }

        public void LocateControls(Form form, ConsoleHandler console)
        {
            form.Text = "Задание № 3";
            form.SetDefaultVals(new System.Drawing.Size(800, 500));

            var btnOpen = BeautyfyForms.AddButton("Открыть index файл", new Point(0, 10), async (o, k) =>
            {
                OpenFileDialog openDic = new OpenFileDialog();
                openDic.Multiselect = false;
                openDic.Filter = string.Format("{0} (*.{1})|*.{1}", "Index файл", "index");
                if (openDic.ShowDialog() == DialogResult.OK)
                {
                    _buttonsToHide.ForEach(x => x.Enabled = false);
                    openedFile = System.IO.Path.ChangeExtension(openDic.FileName, null);
                    var indexFile = openedFile + ".index";
                    var baseFile = openedFile + ".base";

                    _timerProgress.Start();

                    logLstBox.Items.Add(DateTime.Now.ToString() + ": Загружаем index файл");

                    var state = await Task.Run(() => LoadIndexFile(indexFile, baseFile));

                    if (state)
                    {
                        logLstBox.Items.Add(DateTime.Now.ToString() + ": index файл загружен");
                    }
                    else
                    {
                        logLstBox.Items.Add(DateTime.Now.ToString() + ": ошибка загрузки");
                    }

                    _timerProgress.Stop();
                    _progress.Value = _progress.Maximum;
                    _buttonsToHide.ForEach(x => x.Enabled = true);
                }
            });

            _buttonsToHide.Add(btnOpen);
            form.Controls.Add(btnOpen);

            logLstBox = BeautyfyForms.AddListBox(new Point(0, 300 + 3), form.Size);
            form.Controls.Add(logLstBox);

            _progress = BeautyfyForms.AddProgressBar(new Point(150, 10), form.Size, 0, 10);
            form.Controls.Add(_progress);

            _timerProgress = BeautyfyForms.CreateTimer((sender, EventArgs) =>
            {
                if (_progress.Value >= _progress.Maximum)
                {
                    _progress.Value = 0;
                }
                else
                {
                    _progress.Value = _progress.Value + 1;
                }
            });

            _timerBackup = BeautyfyForms.CreateTimer(async (sender, EventArgs) =>
            {
                var indexFile = openedFile + ".index";
                var baseFile = openedFile + ".base";
                var state = await Task.Run(() => SaveIndexFile(indexFile, baseFile));
            });

            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(300, 80), "Слово для поиска"));

            _info = BeautyfyForms.CreateLabel(new Point(400, 100), " Поиск не производился ");
            form.Controls.Add(_info);

            _input = BeautyfyForms.CreateTextBox(new Point(300, 100), false);
            form.Controls.Add(_input);

            var btnSearch = BeautyfyForms.AddButton(" Поиск ", new Point(200, 100), async (o, k) =>
            {
                if (_linkedList == null)
                {
                    MessageBox.Show("Необходимо сначала загрузить index файл");
                    return;
                }

                if (_linkedList.Count <= 0)
                {
                    MessageBox.Show("Нет записей, которые можно было бы обработать. (Пустой файл?)");
                    return;
                }

                if (string.IsNullOrEmpty(_input.Text))
                {
                    MessageBox.Show("Нет текста для поиска");
                    return;
                }

                var text = _input.Text;
                var hashedText = Helper.GetHash(text, _header.hashType);
                _buttonsToHide.ForEach(x => x.Enabled = false);

                var state = await Task.Run(() => Search(hashedText));

                if (state)
                {
                    logLstBox.Items.Add(DateTime.Now.ToString() + ": Слово `" + text + "` найдено");
                }
                else
                {
                    logLstBox.Items.Add(DateTime.Now.ToString() + ": Слово `" + text + "` не найдено");
                }

                _buttonsToHide.ForEach(x => x.Enabled = true);
            });

            _buttonsToHide.Add(btnSearch);
            form.Controls.Add(btnSearch);
        }

        async Task<bool> LoadIndexFile(string indexFile, string baseFile)
        {
            Func<string, bool> log = AddLogRecord;
            _header = Helper.ReadFromBinaryFile<SelfOrganizeIndexHeader>(indexFile, 0);
            if (_header.version == 0)
            {
                logLstBox.BeginInvoke(log, new object[] { "Ошибка чтения заголовка" });
                return false;
            }

            logLstBox.BeginInvoke(log, new object[] { "Заголовок успешно прочитан" });

            logLstBox.BeginInvoke(log, new object[] { "Загрузка index файла займёт некоторое время, пожалуйста, подождите" });

            _linkedList = Helper.ReadFromBinaryFileLinkedList<SelfOrganizeIndexNode>(indexFile, _header.nodesBeginOffset, 0, true);

            return true;
        }

        private async Task<bool> SaveIndexFile(string indexFile, string baseFile)
        {
            Func<string, bool> log = AddLogRecord;

            logLstBox.BeginInvoke(log, new object[] { "Сохраняем index файл" });
            _buttonsToHide.ForEach(x => x.Enabled = false);

            //UNTESTED
            Helper.WriteToBinaryFileLinkedList<SelfOrganizeIndexNode>(indexFile, _linkedList, FileMode.Append, _header.nodesBeginOffset);

            _buttonsToHide.ForEach(x => x.Enabled = true);
            logLstBox.BeginInvoke(log, new object[] { "Сохранено" });
            return true;
        }

        private async Task<bool> Search(string hash)
        {
            Func<string, bool> log = UpdateSearchState;
            var st = _linkedList.First;
            long road = 1;
            while (true)
            {
                if (st.Value.hash == hash)
                {
                    var stVal = st.Value;

                    _linkedList.Remove(st);
                    _linkedList.AddBefore(_linkedList.First, st.Value);

                    _info.BeginInvoke(log, new object[] { "Слово найдено за кол-во переходов равное " + road });

                    return true;
                }
                if (st.Next != null)
                {
                    st = st.Next;
                    road += 1;
                }
                else
                {
                    break;
                }
            }

            _info.BeginInvoke(log, new object[] { "Искомое слово не найдено" });
            return false;
        }

        private bool AddLogRecord(string rec)
        {
            logLstBox.Items.Add(DateTime.Now.ToString() + ": " + rec);
            return true;
        }

        private bool UpdateSearchState(string rec)
        {
            _info.Text = rec;
            return true;
        }
    }
}