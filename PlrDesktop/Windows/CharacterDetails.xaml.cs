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
    /// Логика взаимодействия для CharacterDetails.xaml
    /// </summary>
    public partial class CharacterDetails : Window, IPlrCardWindow
    {
        private ApiClient _api;
        private int _charId;
        private Character _character;
        private IWindowsManager _windowsManager;
        private RtbTextHandler _rtbTextHandler;

        public CharacterDetails(IApiClients apiClients, IWindowsManager windowsManager, int charId)
        {
            InitializeComponent();

            _rtbTextHandler = new(CharacterDescription);

            _api = apiClients.Default;
            _charId = charId;
            _windowsManager = windowsManager;
        }

        private async Task<Character> GetCharacter(int id)
        {
            if (id < 0)
                return null;
            
            Character character = await _api.Methods.Chars.Get(id);
            return character;
        }

        private void CharacterDetailsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateCardData();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = _windowsManager.CreateCharacterEditWindow(_character);
            editWindow.Show();
        }

        public int? GetId()
        {
            return _character.Id ?? null;
        }

        public void UpdateCardData()
        {
            _character = Task.Run(() => GetCharacter(_charId)).Result;

            if (_character is not null)
            {
                CharacterDetailsWindow.Title = _character.Name + " – карточка";
                CharacterNameLabel.Content = _character.Name;

                CharacterDescription.Document.Blocks.Clear();
                if (_rtbTextHandler.SetFromString(_character.Desc) is not null)
                    RtbTextHandler.ShowError(_rtbTextHandler.LastException);
            }
        }
    }
}
