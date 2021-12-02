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

namespace PlrDesktop.Windows
{
    /// <summary>
    /// Логика взаимодействия для SocFormDetails.xaml
    /// </summary>
    public partial class SocFormDetails : Window, IPlrCardWindow
    {
        private ApiClient _api;
        private int _socFormId;
        private SocialFormation _socialFormation;
        private IWindowsManager _windowsManager;
        private RtbTextHandler _rtbTextHandler;

        public SocFormDetails(IApiClients apiClients, IWindowsManager windowsManager, int socFormId)
        {
            InitializeComponent();

            _rtbTextHandler = new(SocFormDescription);

            _api = apiClients.Default;
            _socFormId = socFormId;
            _windowsManager = windowsManager;
        }

        private async Task<SocialFormation> GetSocForm(int id)
        {
            if (id < 0)
                return null;

            SocialFormation socForm = await _api.Methods.SocForms.Get(id);
            return socForm;
        }

        private void SocFormDetailsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateCardData();
        }

        //private void SocFormCatLabel_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        //{
        //    if (_socialFormation.Category is not null)
        //    {
        //        var editWindow = _windowsManager.CreateSocFormDetailsWindow(_socialFormation.Category.Id);
        //        editWindow.Show();
        //    }
        //}

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = _windowsManager.CreateSocFormEditWindow(_socialFormation);
            editWindow.Show();
        }

        public int? GetId()
        {
            return _socialFormation.Id ?? null;
        }

        public void UpdateCardData()
        {
            _socialFormation = Task.Run(() => GetSocForm(_socFormId)).Result;

            if (_socialFormation is not null)
            {
                SocFormDetailsWindow.Title = _socialFormation.Name + " – карточка";
                SocFormNameLabel.Content = _socialFormation.Name;

                SocFormDescription.Document.Blocks.Clear();
                if (_rtbTextHandler.SetFromString(_socialFormation.Desc) is not null)
                    RtbTextHandler.ShowError(_rtbTextHandler.LastException);

                //if (_socialFormation.Category is not null)
                //{
                //    SocFormCatLabel.Content = "Входит в категорию " + _socialFormation.Category.Name;
                //    SocFormCatLabel.MouseLeftButtonUp += SocFormCatLabel_MouseLeftButtonUp;
                //}
                //else
                //{
                //    SocFormCatLabel.Content = "Не имеет категории";
                //}
            }
        }
    }
}
