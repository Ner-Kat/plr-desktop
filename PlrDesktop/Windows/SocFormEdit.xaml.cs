using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PlrDesktop.Lib;
using PlrDesktop.ApiInteraction;
using PlrDesktop.Datacards;
using System.Collections.ObjectModel;
using System.IO;

namespace PlrDesktop.Windows
{
    /// <summary>
    /// Логика взаимодействия для SocFormEdit.xaml
    /// </summary>
    public partial class SocFormEdit : Window, IPlrCardWindow
    {
        private ApiClient _api;
        private SocialFormation _socialFormation;
        private RtbTextHandler _rtbTextHandler;
        private IWindowsManager _windowsManager;

        private bool _addMode = true;

        public SocFormEdit(IApiClients apiClients, IWindowsManager windowsManager, SocialFormation socForm)
        {
            InitializeComponent();

            _rtbTextHandler = new RtbTextHandler(SocFormDescField);

            _api = apiClients.Default;
            _windowsManager = windowsManager;
            _socialFormation = socForm;
        }

        private async Task<List<SocialFormation>> GetAllSocForms()
        {
            List<SocialFormation> socForms = await _api.Methods.SocForms.List(null);

            if (_socialFormation is not null)
            {
                var selfLoc = socForms.FirstOrDefault(loc => loc.Id == _socialFormation.Id);
                socForms.Remove(selfLoc);
            }

            return socForms;
        }

        private void SocFormEditWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateCardData();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var editedSocForm = new SocialFormation()
            {
                Name = SocFormNameTextBox.Text,
                Desc = _rtbTextHandler.GetAsString()
            };

            if (_addMode)
            {
                Task.Run(() => _api.Methods.SocForms.Add(editedSocForm));
            }
            else
            {
                editedSocForm.Id = _socialFormation.Id;
                Task.Run(() => _api.Methods.SocForms.Change(editedSocForm));
            }
            
            this.Close();
        }

        private void TextEditingToolbar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }

            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness(0);
            }
        }

        public int? GetId()
        {
            return _socialFormation.Id ?? null;
        }

        //private void ClearSocFormCatSelection_Click(object sender, RoutedEventArgs e)
        //{
        //    SocFormCatComboBox.SelectedIndex = -1;
        //}

        public void UpdateCardData()
        {
            if (_socialFormation is not null)
            {
                SocFormEditWindow.Title = _socialFormation.Name + " – изменение";
                _addMode = false;

                SocFormNameTextBox.Text = _socialFormation.Name;
                SocFormDescField.Document.Blocks.Clear();
                if (_rtbTextHandler.SetFromString(_socialFormation.Desc) is not null)
                    RtbTextHandler.ShowError(_rtbTextHandler.LastException);
            }
        }
    }
}
