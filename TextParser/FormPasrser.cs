using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TextParser
{
    public partial class FormPasrser : Form
    {
        private List<string> Files = new List<string>();
        private readonly string[] Extensions = new string[2] { "*.cs", "*.xaml" };


        public FormPasrser()
        {
            InitializeComponent();
        }

        private bool OpenFolder(string path)
        {
            foreach (string extension in Extensions)
            {
                var directoryWithAllFiles = Directory.GetFiles(path, extension, SearchOption.AllDirectories).ToList();

                foreach (var file in directoryWithAllFiles)
                {
                    Files.Add(file);
                }
            }

            if (Files.Count > 0)
                return true;

            return false;
        }

        private bool SaveFile(string path)
        {
            var streamWriter = new StreamWriter(path, true);

            for (int file = 0; file < checkedListFiles.Items.Count; file++)
            {
                if (checkedListFiles.GetItemChecked(file))
                {
                    var lines = File.ReadAllLines(checkedListFiles.GetItemText(checkedListFiles.Items[file]));
                    foreach (string line in lines)
                    {
                        streamWriter.WriteLine(line);
                    };
                }
            }
            streamWriter.Close();

            return true;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(fbd.SelectedPath))
                    MessageBox.Show("Папка не выбрана", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (OpenFolder(fbd.SelectedPath))
                {
                    foreach (var file in Files)
                    {
                        checkedListFiles.Items.Add(file);
                    }

                    for (int @checked = 0; @checked < Files.Count; @checked++)
                    {
                        checkedListFiles.SetItemChecked(@checked, true);
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка при выгрузке всех файлов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var svf = new SaveFileDialog();
            svf.Filter = "All texts documents(*.txt)|*.txt";

            if (svf.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(svf.FileName))
                    MessageBox.Show("Путь не указан", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (SaveFile(svf.FileName))
                    MessageBox.Show("Файл сохранен по пути: " + svf.FileName);
                else
                    MessageBox.Show("Ошибка при сохранении файла", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clearListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var element in Files)
            {
                checkedListFiles.Items.Remove(element);
            }
        }
    }
}
