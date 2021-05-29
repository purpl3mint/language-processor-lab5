using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_5
{
    public class Commands
    {
        private string checkExpression(string source)
        {
            string result = "";
            string keyword = "";
            int status = 0;
            

            for (int i = 0; i < source.Length; i++)
            {
                if ((status == 0 || status == 1) && char.IsDigit(source[i]))
                {
                    status = 1;
                }
                else if ((status == 0 || status == 11) && char.IsLetter(source[i]))
                {
                    status = 11;
                    keyword += source[i];
                }
                else if (status == 11 && char.IsDigit(source[i]))
                {
                    status = 2;
                    keyword = "";
                }
                else if (status == 2 && char.IsLetterOrDigit(source[i]))
                {
                    status = 2;
                }
                else if (status == 0 && source[i] == '+')
                {
                    result += " 3";
                }
                else if (status == 0 && source[i] == '-')
                {
                    result += " 4";
                }
                else if (status == 0 && source[i] == '/')
                {
                    result += " 5";
                }
                else if (status == 0 && source[i] == '*')
                {
                    status = 6;
                }
                else if (status == 6 && source[i] == '*')
                {
                    result += " 7";
                    status = 0;
                }
                else if (status == 0 && source[i] == '(')
                {
                    result += " 8";
                }
                else if (status == 0 && source[i] == ')')
                {
                    result += " 9";
                }
                else if (status == 0 && source[i] == '=')
                {
                    result += " 10";
                }
                else if (status == 0 && source[i] == ';')
                {
                    result += " 15";
                }
                else if (source[i] == ' ' && status != 0)
                {
                    if (keyword == "int") status = 11;
                    else if (keyword == "char") status = 12;
                    else if (keyword == "float") status = 13;
                    else if (keyword == "double") status = 14;
                    else if (status == 11) status = 2;

                    keyword = "";
                    result += " " + status;
                    status = 0;
                }
                else if (status != 0)
                {
                    if (keyword == "int") status = 11;
                    else if (keyword == "char") status = 12;
                    else if (keyword == "float") status = 13;
                    else if (keyword == "double") status = 14;
                    else if (status == 11) status = 2;

                    keyword = "";
                    result += " " + status;
                    status = 0;
                    i -= 1;
                }
            }



            if (keyword == "int") status = 11;
            else if (keyword == "char") status = 12;
            else if (keyword == "float") status = 13;
            else if (keyword == "double") status = 14;
            else if (status == 11) status = 2;

            if (status != 0)
                result += " " + status;

            return result;
            /*
            int state = 1;
            int position = 1;

            foreach (char symbol in source)
            {
                switch (state)
                {
                    case 1:
                        {
                            if (symbol == '0')
                                state = 2;
                            else
                                return position;
                            break;
                        }
                    case 2:
                        {
                            if (symbol == '1')
                                state = 3;
                            else
                                return position;
                            break;
                        }
                    case 3:
                        {
                            if (symbol == '1')
                                state = 4;
                            else if (symbol == '2')
                                state = 7;
                            else if (symbol == '3')
                                state = 6;
                            else
                                return position;
                            break;
                        }
                    case 4:
                        {
                            if (symbol == '2')
                                state = 5;
                            else
                                return position;
                            break;
                        }
                    case 5:
                        {
                            if (symbol == '1')
                                state = 4;
                            else if (symbol == '3')
                                state = 6;
                            else
                                return position;
                            break;
                        }
                    case 6:
                        {
                            if (symbol == '2')
                                state = 6;
                            else
                                return position;
                            break;
                        }
                    default: return -2;
                }

                position++;
            }

            if (state == 6 || state == 7)
            {
                return -1;
            }

            return position;
            */
        }

        public void CommandCreate()
        {
            if (StaticData.unsaved)
            {
                StaticData.currentData = StaticData.mainForm.TextBox.Text;
                var saveBeforeCloseWindow = new SaveBeforeCloseForm();
                saveBeforeCloseWindow.ShowDialog();
            }

            StaticData.dialogService.FilePath = "";
            StaticData.currentData = "";
            StaticData.mainForm.TextBox.Text = StaticData.currentData;
            StaticData.mainForm.Heading = "Language Processor - unnamed";
        }

        public void CommandOpen()
        {
            if (StaticData.unsaved)
            {
                StaticData.currentData = StaticData.mainForm.TextBox.Text;
                var saveBeforeCloseWindow = new SaveBeforeCloseForm();
                saveBeforeCloseWindow.ShowDialog();
            }

            StaticData.dialogService.OpenFileDialog();
            StaticData.currentData = StaticData.fileService.ReadFile(StaticData.dialogService.FilePath);

            StaticData.mainForm.TextBox.Text = StaticData.currentData;

            StaticData.mainForm.Heading = "Language Processor";
            if (StaticData.dialogService.FilePath != null || StaticData.dialogService.FilePath != "")
                StaticData.mainForm.Heading += " - " + StaticData.dialogService.FilePath;
            else
                StaticData.mainForm.Heading += " - unnamed";

            StaticData.unsaved = false;
        }

        public void CommandSave()
        {
            StaticData.currentData = StaticData.mainForm.TextBox.Text;

            if (StaticData.dialogService.FilePath == null)
            {
                StaticData.dialogService.SaveFileDialog();
                StaticData.fileService.SaveFile(StaticData.dialogService.FilePath, StaticData.currentData);
            }
            else
            {
                StaticData.fileService.SaveFile(StaticData.dialogService.FilePath, StaticData.currentData);
            }

            StaticData.unsaved = false;
            StaticData.mainForm.Heading = "Language Processor - " + StaticData.dialogService.FilePath;
        }

        public void CommandSaveAs()
        {
            StaticData.currentData = StaticData.mainForm.TextBox.Text;
            StaticData.dialogService.SaveFileDialog();
            StaticData.fileService.SaveFile(StaticData.dialogService.FilePath, StaticData.currentData);
            StaticData.mainForm.Heading = "Language Processor - " + StaticData.dialogService.FilePath;
            StaticData.unsaved = false;
        }

        public void CommandUndo()
        {
            if (StaticData.undoStack.Count > 0)
            {
                StaticData.redoStack.Push(StaticData.mainForm.TextBox.Text);
                string newValue = StaticData.undoStack.Pop();
                StaticData.mainForm.TextBox.Text = newValue;
            }
        }

        public void CommandRedo()
        {
            if (StaticData.redoStack.Count > 0)
            {
                StaticData.undoStack.Push(StaticData.mainForm.TextBox.Text);
                string newValue = StaticData.redoStack.Pop();
                StaticData.mainForm.TextBox.Text = newValue;
            }
        }

        public void CommandCopy()
        {
            if (StaticData.mainForm.TextBox.SelectionLength > 0)
                StaticData.mainForm.TextBox.Copy();
        }
        public void CommandPaste()
        {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
            {
                if (StaticData.mainForm.TextBox.SelectionLength > 0)
                {
                    StaticData.mainForm.TextBox.SelectionStart = StaticData.mainForm.TextBox.SelectionStart + StaticData.mainForm.TextBox.SelectionLength;
                }
                StaticData.mainForm.TextBox.Paste();
            }
        }

        public void CommandCut()
        {
            if (StaticData.mainForm.TextBox.SelectedText != "")
                StaticData.mainForm.TextBox.Cut();
        }

        public void CommandDelete()
        {
            int StartPosDel = StaticData.mainForm.TextBox.SelectionStart;
            int LenSelection = StaticData.mainForm.TextBox.SelectionLength;
            StaticData.mainForm.TextBox.Text = StaticData.mainForm.TextBox.Text.Remove(StartPosDel, LenSelection);
        }

        public void CommandSelectAll()
        {
            StaticData.mainForm.TextBox.SelectAll();
        }

        public void CommandHelp()
        {
            Help.ShowHelp(null, "../../heeelp/help1.html");
        }

        public void CommandCheck()
        {
            string[] strings = StaticData.mainForm.TextBox.Text.Split('\n');

            for (int i = 0; i < strings.Length; i++)
            {
                strings[i] = strings[i].TrimEnd('\r');
            }

            StaticData.mainForm.ResultsTextBox.Text = "";

            for (int i = 0; i < strings.Length; i++)
            {
                string lineResult = checkExpression(strings[i]);

                StaticData.mainForm.ResultsTextBox.Text += "Line " + (i + 1) + ": " + lineResult + Environment.NewLine;
                /*
                int lineStatus = checkExpression(strings[i]);
                if (lineStatus == -1)
                {
                    StaticData.mainForm.ResultsTextBox.Text += "Line " + (i + 1) + ": CORRECT" + Environment.NewLine;
                }
                else if (lineStatus == -2)
                {
                    StaticData.mainForm.ResultsTextBox.Text += "Line " + (i + 1) + ": PROCESSING ERROR, very big expression" + Environment.NewLine;
                }
                else
                {
                    StaticData.mainForm.ResultsTextBox.Text += "Line " + (i + 1) + ": SYNTAX ERROR, wrong command at position " + lineStatus + Environment.NewLine;
                }
                */
            }

        }
    }
}
