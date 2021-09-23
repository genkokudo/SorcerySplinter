using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SorcerySplinter.Modules.Common.ViewModels
{
    public class EditViewModel : BindableBase
    {
        public List<SampleData> Variables { get; set; }

        /// <summary>削除するコマンド</summary>
        public DelegateCommand DeleteCommand { get; private set; }

        public EditViewModel()
        {
            Variables = new List<SampleData>
            {
                new SampleData{SubName = "1郎", SubDescription = "説明1", SubDefValue = "aaaa", SubIsClassName = true },
                new SampleData{SubName = "2郎", SubDescription = "説明2", SubDefValue = "bbbb", SubIsClassName = false },
                new SampleData{SubName = "3郎", SubDescription = "説明3", SubDefValue = "cccc", SubIsClassName = true }
            };
            DeleteCommand = new DelegateCommand(DeleteRow);
        }

        private void DeleteRow()
        {
            MessageBox.Show($"del");
        }

    }

    public class SampleData
    {
        public string SubName { get; set; }
        public string SubDescription { get; set; }
        public string SubDefValue { get; set; }
        public bool SubIsClassName { get; set; }
    }
}
