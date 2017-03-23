using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.View.Miscellaneous
{
    /// <summary>
    /// Interaction logic for MergePage.xaml
    /// </summary>
    public partial class MergePage : Page, INotifyPropertyChanged
    {
        public MergePage()
        {
            InitializeComponent();
        }

        #region ObservableObjects

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _mergeSuccess;
        public bool MergeSuccess
        {
            get { return _mergeSuccess; }
            set { _mergeSuccess = value; OnPropertyChanged(nameof(MergeSuccess)); }
        }

        private int _conflictCount;
        public int ConflictCount
        {
            get { return _conflictCount; }
            set { _conflictCount = value; OnPropertyChanged(nameof(ConflictCount)); }
        }

        #endregion


        public ProgramData Merge(ProgramData target, ProgramData mergeData)
        {
            List<ProductCategory> pcConflicts = ImportPc(target, mergeData.AllProductCategories);
            if (pcConflicts.Any())
            {
                
            }
            
            ImportRc(target, mergeData.AllRecipeCategories);
            ImportGpc(target, mergeData.AllGroupedProductCategories);
            ImportPc(target, mergeData.AllGroupedRecipeCategories);
            ImportPc(target, mergeData.AllProductCategories);
            ImportPc(target, mergeData.AllProductCategories);
            ImportPc(target, mergeData.AllProductCategories);


            ProgramData conflicts;
        }
    }
}
