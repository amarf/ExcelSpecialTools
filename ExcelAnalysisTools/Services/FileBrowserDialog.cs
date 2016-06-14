using System.Windows.Forms;
using Core.Interfaces;

namespace ExcelAnalysisTools.Services
{
    public class FileBrowserDialog : IFileBrowserDialog
    {
        #region Fields

        private FileDialog _preview_dialog;
        private FileDialog _dialog = new OpenFileDialog();

        #endregion

        #region IFileBrowserDialog

        bool isSaveFileDialog;
        public bool IsSaveFileDialog
        {
            get
            {
                return isSaveFileDialog;
            }
            set
            {
                if (isSaveFileDialog == value) return;
                isSaveFileDialog = value;
                var temp = _dialog;
                if (value)
                    _dialog = _preview_dialog != null ? _preview_dialog : new SaveFileDialog();
                else
                    _dialog = _preview_dialog != null ? _preview_dialog : new OpenFileDialog();
                _preview_dialog = temp;
            }
        }

        public bool CheckFileExists
        {
            get { return _dialog.CheckFileExists; }
            set { _dialog.CheckFileExists = value; }
        }

        public string SelectedPath
        {
            get { return _dialog.FileName; }
            set { _dialog.FileName = value; }
        }

        public string StartFolder
        {
            get { return _dialog.InitialDirectory; }
            set { _dialog.InitialDirectory = value; }
        }

        public string[] SelectedPaths
        {
            get { return _dialog.FileNames; }
        }

        public string Filter
        {
            get { return _dialog.Filter; }
            set { _dialog.Filter = value; }
        }

        public void Reset()
        {
            _dialog.Reset();
        }

        public bool ShowDialog()
        {
            return _dialog.ShowDialog() == DialogResult.OK;
        }

        public bool Multiselect
        {
            get
            {
                if (_dialog is OpenFileDialog)
                    return (_dialog as OpenFileDialog).Multiselect;
                return false;
            }
            set
            {
                if (_dialog is OpenFileDialog)
                    (_dialog as OpenFileDialog).Multiselect = value;
            }
        }

        #endregion
    }
}