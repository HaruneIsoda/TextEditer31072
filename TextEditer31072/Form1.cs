using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditer31072 {
    public partial class Form1 : Form {

        private string fileName = "";

        public Form1() {
            InitializeComponent();
        }

        //フォームロード
        private void Form1_Load(object sender, EventArgs e) {
            EditEnabledTrueOrFalse();
        }

        //終了メニュー
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        //開くメニュー
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e) {

            if(ofdFileOpen.ShowDialog() == DialogResult.OK) {
                using(StreamReader sr = new StreamReader(ofdFileOpen.FileName, Encoding.GetEncoding("utf-8"), false)) {
                    rtbTextArea.Text = sr.ReadToEnd();
                }
                fileName = ofdFileOpen.FileName;
                FormTextChange();
            }

        }

        //名前を付けて保存
        private void FileSave(string filename) {
            using(StreamWriter sw = new StreamWriter(filename, false, Encoding.GetEncoding("utf-8"))) {
                sw.WriteLine(rtbTextArea.Text);

                fileName = sfdFileSave.FileName;
                FormTextChange();
            }
        }

        //名前を付けて保存メニュー
        private void SaveNameToolStripMenuItem_Click(object sender, EventArgs e) {
            if(sfdFileSave.ShowDialog() == DialogResult.OK) {
                FileSave(sfdFileSave.FileName);
            }
        }

        //上書き保存メニュー
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e) {
            if(fileName != "") {
                FileSave(fileName);
            } else {
                SaveNameToolStripMenuItem_Click(sender, e);
            }
        }

        //ファイル名表示
        private void FormTextChange() {
            this.Text = fileName + "：テキストエディタ";
        }

        //新規作成メニュー
        private void NewToolStripMenuItem_Click(object sender, EventArgs e) {
            if(fileName == "" && rtbTextArea.Text != "") {
                DialogResult result = MessageBox.Show("保存しますか？",
                                        "確認",
                                        MessageBoxButtons.YesNoCancel,
                                        MessageBoxIcon.Exclamation,
                                        MessageBoxDefaultButton.Button2);

                if(result == DialogResult.Yes) {
                    SaveNameToolStripMenuItem_Click(sender, e);
                } else if(result == DialogResult.No) {
                    fileName = "";
                    rtbTextArea.Text = "";
                } else if(result == DialogResult.Cancel) {
                    return;
                }
            } else {
                fileName = "";
                rtbTextArea.Text = "";
            }

        }

        //元に戻すメニュー
        private void UndoToolStripMenuItem_Click(object sender, EventArgs e) {
            rtbTextArea.Undo();
        }

        //やり直しメニュー
        private void RedoToolStripMenuItem_Click(object sender, EventArgs e) {
            rtbTextArea.Redo();
        }

        //編集メニューのコントロール可否
        private void EditEnabledTrueOrFalse() {

            if(rtbTextArea.CanUndo == false) {  //元に戻す
                UndoToolStripMenuItem.Enabled = false;
            } else {
                UndoToolStripMenuItem.Enabled = true;
            }

            if(rtbTextArea.CanRedo == false) {  //やり直し
                RedoToolStripMenuItem.Enabled = false;
            } else {
                RedoToolStripMenuItem.Enabled = true;
            }

        }

        //切り取りメニュー
        private void TearingOffToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        //テキストを変更したときの処理
        private void rtbTextArea_TextChanged(object sender, EventArgs e) {
            //元に戻す、やり直しのマスク判定
            EditEnabledTrueOrFalse();
        }
    }
}
