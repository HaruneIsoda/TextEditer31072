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
            FormTextChange();
        }

        //終了メニュー
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) {
            //if(SaveMessageBox(sender, e) == 1) {
            //    return;
            //}
            Application.Exit();
        }

        //開くメニュー
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e) {

            if(SaveMessageBox(sender, e) == 1) {
                return;
            }

            if(ofdFileOpen.ShowDialog() == DialogResult.OK) {
                using(StreamReader sr = new StreamReader(ofdFileOpen.FileName, Encoding.GetEncoding("utf-8"), false)) {
                    rtbTextArea.Text = sr.ReadToEnd();
                }
                fileName = ofdFileOpen.FileName;
                FormTextChange();
            }

        }

        //保存
        private void FileSave(string filename) {
            using(StreamWriter sw = new StreamWriter(filename, false, Encoding.GetEncoding("utf-8"))) {
                sw.WriteLine(rtbTextArea.Text);

                fileName = sfdFileSave.FileName;
                
            }
        }

        //名前を付けて保存メニュー
        private void SaveNameToolStripMenuItem_Click(object sender, EventArgs e) {
            if(sfdFileSave.ShowDialog() == DialogResult.OK) {
                FileSave(sfdFileSave.FileName);
                FormTextChange();
            }
        }

        //上書き保存メニュー
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e) {
            if(fileName != "") {
                FileSave(fileName);
                FormTextChange();
            } else {
                SaveNameToolStripMenuItem_Click(sender, e);
                FormTextChange();
            }
        }


        //ファイル名表示
        private void FormTextChange() {
            this.Text = fileName + "：テキストエディタ";
        }

        //保存をするかしないかのメッセージボックス
        private int SaveMessageBox(object sender, EventArgs e) {

            //0だったら呼び出し後の処理を実行、1だったら呼び出し後の処理は実行されない。
            if(rtbTextArea.Text != "") {
                DialogResult result = MessageBox.Show("保存しますか？",
                                        "確認",
                                        MessageBoxButtons.YesNoCancel,
                                        MessageBoxIcon.Exclamation,
                                        MessageBoxDefaultButton.Button2);

                if(result == DialogResult.Yes) {
                    SaveToolStripMenuItem_Click(sender, e);
                    return 0;
                } else if(result == DialogResult.No) {
                    //処理を実行
                    return 0;
                } else if(result == DialogResult.Cancel) {
                    return 1;
                } else {
                    return 1;
                }
            } else {
                return 0;
            }
        }

        //新規作成メニュー
        private void NewToolStripMenuItem_Click(object sender, EventArgs e) {
            
            if(SaveMessageBox(sender, e) == 1) {
                return;
            }
            
            fileName = "";
            rtbTextArea.Text = "";

        }


        //元に戻すメニュー
        private void UndoToolStripMenuItem_Click(object sender, EventArgs e) {
            rtbTextArea.Undo();
        }

        //やり直しメニュー
        private void RedoToolStripMenuItem_Click(object sender, EventArgs e) {
            rtbTextArea.Redo();
        }

        //テキストを変更したときの処理
        private void rtbTextArea_TextChanged(object sender, EventArgs e) {

        }


        //切り取りメニュー
        private void TearingOffToolStripMenuItem_Click(object sender, EventArgs e) {
            rtbTextArea.Cut();

        }

        //コピーメニュー
        private void CopyToolStripMenuItem_Click(object sender, EventArgs e) {
            rtbTextArea.Copy();
        }

        //貼り付けメニュー
        private void PastingToolStripMenuItem_Click(object sender, EventArgs e) {
            rtbTextArea.Paste();
        }

        //削除メニュー
        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e) {
            rtbTextArea.SelectedText = "";
        }


        //色メニュー
        private void ColorToolStripMenuItem_Click(object sender, EventArgs e) {
            if(cdColor.ShowDialog() == DialogResult.OK) {
                rtbTextArea.SelectionColor = cdColor.Color;
            }

        }

        //フォントメニュー
        private void FontToolStripMenuItem_Click(object sender, EventArgs e) {
            if(fdFont.ShowDialog() == DialogResult.OK) {
                rtbTextArea.SelectionFont = fdFont.Font;
            }
        }

        //閉じるボタンを押したときの処理
        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            if(SaveMessageBox(sender, e) == 1) {
                e.Cancel = true;
            }
        }

        //編集メニュー
        private void EditToolStripMenuItem_Click(object sender, EventArgs e) {
            UndoToolStripMenuItem.Enabled = rtbTextArea.CanUndo ? true : false;
            RedoToolStripMenuItem.Enabled = rtbTextArea.CanRedo ? true : false;
            TearingOffToolStripMenuItem.Enabled = rtbTextArea.SelectionLength > 0 ? true : false;
            CopyToolStripMenuItem.Enabled = rtbTextArea.SelectionLength > 0 ? true : false;
            PastingToolStripMenuItem.Enabled = Clipboard.GetDataObject().GetDataPresent(DataFormats.Rtf);
        }
    }
}
