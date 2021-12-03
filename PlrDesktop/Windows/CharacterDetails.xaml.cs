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

        private ObservableCollection<Character> _childrenOc = new();
        private CollectionViewSource _childrenView = new();

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

            _childrenView.Source = _childrenOc;
            //_childrenView.Filter += 
            ChildrenList.ItemsSource = _childrenView.View;
            PlrWpfUtils.ClearDataGridSelection(ChildrenList);

            RaceLabel.MouseLeftButtonUp += RaceLabel_MouseLeftButtonUp;
            LocBirthLabel.MouseLeftButtonUp += LocBirthLabel_MouseLeftButtonUp;
            LocDeathLabel.MouseLeftButtonUp += LocDeathLabel_MouseLeftButtonUp;
            FatherLabel.MouseLeftButtonUp += FatherLabel_MouseLeftButtonUp;
            MotherLabel.MouseLeftButtonUp += MotherLabel_MouseLeftButtonUp;
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
                // Имя
                CharacterDetailsWindow.Title = _character.Name + " – карточка";
                CharacterNameLabel.Content = _character.Name;

                // Описание
                if (_character.Desc is not null)
                {
                    CharacterDescription.Document.Blocks.Clear();
                    if (_rtbTextHandler.SetFromString(_character.Desc) is not null)
                        RtbTextHandler.ShowError(_rtbTextHandler.LastException);
                }

                // Цвета глаз и волос
                if (_character.ColorHair is not null)
                {
                    var color = (Color)ColorConverter.ConvertFromString(ColorDecToHex(_character.ColorHair.Value));
                    ColorHairRect.Fill = new SolidColorBrush(color);
                }
                if (_character.ColorEyes is not null)
                {
                    var color = (Color)ColorConverter.ConvertFromString(ColorDecToHex(_character.ColorEyes.Value));
                    ColorEyesRect.Fill = new SolidColorBrush(color);
                }

                // Даты рождения и смерти
                if (_character.DateBirth is not null)
                {
                    DateBirthLabel.Content = "Дата рождения: " + _character.DateBirth;
                }
                if (_character.DateDeath is not null)
                {
                    DateDeathLabel.Content = "Дата смерти: " + _character.DateDeath;
                }

                // Рост
                if (_character.Growth is not null)
                {
                    GrowthLabel.Content = "Рост: " + _character.Growth.ToString();
                }

                // Раса
                RaceLabel.MouseLeftButtonUp -= RaceLabel_MouseLeftButtonUp;
                if (_character.RaceId is not null)
                {
                    RaceLabel.Content = "Раса: " + _character.Race.Name;
                    RaceLabel.MouseLeftButtonUp += RaceLabel_MouseLeftButtonUp;
                }
                
                // Отец и мать
                FatherLabel.MouseLeftButtonUp -= FatherLabel_MouseLeftButtonUp;
                if (_character.BioFatherId is not null)
                {
                    FatherLabel.Content = "Отец: " + _character.BioFather.Name;
                    FatherLabel.MouseLeftButtonUp += FatherLabel_MouseLeftButtonUp;
                }
                MotherLabel.MouseLeftButtonUp -= MotherLabel_MouseLeftButtonUp;
                if (_character.BioMotherId is not null)
                {
                    MotherLabel.Content = "Мать: " + _character.BioMother.Name;
                    MotherLabel.MouseLeftButtonUp += MotherLabel_MouseLeftButtonUp;
                }

                // Места рождения и смерти
                LocBirthLabel.MouseLeftButtonUp -= LocBirthLabel_MouseLeftButtonUp;
                if (_character.LocBirthId is not null)
                {
                    LocBirthLabel.Content = "Родился в: " + _character.LocBirth.Name;
                    LocBirthLabel.MouseLeftButtonUp += LocBirthLabel_MouseLeftButtonUp;
                }
                LocDeathLabel.MouseLeftButtonUp -= LocDeathLabel_MouseLeftButtonUp;
                if (_character.LocDeathId is not null)
                {
                    LocDeathLabel.Content = "Умер в: " + _character.LocDeath.Name;
                    LocDeathLabel.MouseLeftButtonUp += LocDeathLabel_MouseLeftButtonUp;
                }

                // Другие имена
                if (_character.AltNames is not null && _character.AltNames.Count() > 0)
                {
                    string altNames = "Другие имена: ";
                    foreach (var altName in _character.AltNames)
                    {
                        altNames += altName + ", ";
                    }
                    AltNamesLabel.Content = altNames.Remove(altNames.Length - 1);
                }

                // Титулы и звания
                if (_character.Titles is not null && _character.Titles.Count() > 0)
                {
                    string titles = "Титулы и звания: ";
                    foreach (var title in _character.Titles)
                    {
                        titles += title + ", ";
                    }
                    TitlesLabel.Content = titles.Remove(titles.Length - 1);
                }

                // Социальные формирования
                if (_character.SocFormsIds is not null)
                {
                    foreach (var socForm in _character.SocForms)
                    {
                        var label = new Label()
                        {
                            Content = socForm.Name,
                            Style = Application.Current.TryFindResource("ClickableDataLabel") as Style
                        };

                        label.MouseLeftButtonUp += (object sender, MouseButtonEventArgs e) =>
                        {
                            var window = _windowsManager.CreateSocFormDetailsWindow(socForm.Id.Value);
                            window.Show();
                        };

                        SocFormsPanel.Children.Add(label);
                    }
                    SocFormsPanel.UpdateLayout();
                }

                // Другие карточки личности
                if (_character.AltCharsIds is not null)
                {
                    foreach (var altChar in _character.AltChars)
                    {
                        var label = new Label()
                        {
                            Content = altChar.Name,
                            Style = Application.Current.TryFindResource("ClickableDataLabel") as Style
                        };

                        label.MouseLeftButtonUp += (object sender, MouseButtonEventArgs e) =>
                        {
                            var window = _windowsManager.CreateCharacterDetailsWindow(altChar.Id.Value);
                            window.Show();
                        };

                        AltCharsPanel.Children.Add(label);
                    }
                    AltCharsPanel.UpdateLayout();
                }

                // Дети
                if (_character.ChildrenIds is not null)
                {
                    _childrenOc.Clear();
                    foreach (var child in _character.Children)
                        _childrenOc.Add(child);
                }
            }
        }

        private void RaceLabel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var window = _windowsManager.CreateRaceDetailsWindow(_character.RaceId.Value);
            window.Show();
        }

        private void LocBirthLabel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var window = _windowsManager.CreateLocationDetailsWindow(_character.LocBirthId.Value);
            window.Show();
        }

        private void LocDeathLabel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var window = _windowsManager.CreateLocationDetailsWindow(_character.LocDeathId.Value);
            window.Show();
        }

        private void FatherLabel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var window = _windowsManager.CreateCharacterDetailsWindow(_character.BioFatherId.Value);
            window.Show();
        }

        private void MotherLabel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var window = _windowsManager.CreateCharacterDetailsWindow(_character.BioMotherId.Value);
            window.Show();
        }

        private void ChildrenList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = ChildrenList.SelectedCells[0].Item;
            var selectedCharacter = (Character)selectedItem;

            CharacterDetails characterDetails = (CharacterDetails)_windowsManager
                .CreateCharacterDetailsWindow(selectedCharacter.Id.Value);

            characterDetails.Show();
        }

        private string ColorDecToHex(int color)
        {
            char[] result = "000000".ToCharArray();

            string value = color.ToString("X");
            for (int i = value.Length - 1; i >= 0; i--)
                result[i] = value[i];

            return "#FF" + new string(result);
        }

        //private void ChildrenView_Filter(object sender, FilterEventArgs e)
        //{
        //    Character chr = e.Item as Character;

        //    if (chr is not null)
        //    {
        //        if (chr.Name.ToLower().Contains(CharFindTextBox.Text.ToLower()))
        //            e.Accepted = true;
        //        else
        //            e.Accepted = false;
        //    }
        //}
    }
}
