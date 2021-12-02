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
    /// Логика взаимодействия для CharacterEdit.xaml
    /// </summary>
    public partial class CharacterEdit : Window, IPlrCardWindow
    {
        private ApiClient _api;
        private Character _character;
        private RtbTextHandler _rtbTextHandler;
        private IWindowsManager _windowsManager;

        private ObservableCollection<Character> _avalibleParentLocs = new();
        private CollectionViewSource _avParentLocsView = new();

        private bool _addMode = true;

        public CharacterEdit(IApiClients apiClients, IWindowsManager windowsManager, Character location)
        {
            InitializeComponent();

            _rtbTextHandler = new RtbTextHandler(CharDescField);

            _api = apiClients.Default;
            _windowsManager = windowsManager;
            _character = location;
        }

        private async Task<List<Character>> GetAllCharacters()
        {
            List<Character> characters = await _api.Methods.Chars.List(null);

            if (_character is not null)
            {
                var selfLoc = characters.FirstOrDefault(chr => chr.Id == _character.Id);
                characters.Remove(selfLoc);
            }

            return characters;
        }

        private void CharacterEditWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateCardData();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var editedCharacter = new Character()
            {
                Name = CharNameTextBox.Text,
                Desc = _rtbTextHandler.GetAsString(),
                RaceId = 1,
                GenderId = 1
            };

            if (_addMode)
            {
                Task.Run(() => _api.Methods.Chars.Add(editedCharacter));
            }
            else
            {
                editedCharacter.Id = _character.Id;
                Task.Run(() => _api.Methods.Chars.Change(editedCharacter));
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
            return _character is not null ? _character.Id : null;
        }

        public void UpdateCardData()
        {
            if (_character is not null)
            {
                CharacterEditWindow.Title = _character.Name + " – изменение";
                _addMode = false;

                CharNameTextBox.Text = _character.Name;
                CharDescField.Document.Blocks.Clear();
                if (_rtbTextHandler.SetFromString(_character.Desc) is not null)
                    RtbTextHandler.ShowError(_rtbTextHandler.LastException);
            }
        }
    }
}
