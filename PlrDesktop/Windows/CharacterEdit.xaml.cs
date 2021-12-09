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
using PlrDesktop.Datacards.Utils;
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

        private ObservableCollection<Character> _charsOc = new();
        private CollectionViewSource _fatherView = new();
        private CollectionViewSource _motherView = new();
        private CollectionViewSource _altCharsView = new();
        private CollectionViewSource _childAddView = new();
        private ObservableCollection<Character> _childrenOc = new();
        private CollectionViewSource _childrenView = new();

        private ObservableCollection<Location> _locationsOc = new();
        private CollectionViewSource _locBirthView = new();
        private CollectionViewSource _locDeathView = new();

        private ObservableCollection<SocialFormation> _socFormsOc = new();
        private CollectionViewSource _socFormsView = new();

        private ObservableCollection<Race> _racesOc = new();
        private CollectionViewSource _racesView = new();

        private ObservableCollection<Gender> _gendersOc = new();
        private CollectionViewSource _gendersView = new();

        private List<IApiDataCard> _addedSocForms = new();
        private List<IApiDataCard> _addedAltChars = new();

        private List<object> _signs = new() { new { Id = 0, Name = "От 0" }, new { Id = 1, Name = "До 0" } };

        private bool _addMode = true;

        public CharacterEdit(IApiClients apiClients, IWindowsManager windowsManager, Character character)
        {
            InitializeComponent();

            _rtbTextHandler = new RtbTextHandler(CharDescField);

            _api = apiClients.Default;
            _windowsManager = windowsManager;
            _character = character;
        }

        private async Task<List<Location>> GetLocationsList()
        {
            List<Location> locations = await _api.Methods.Locs.List(null);
            return locations;
        }

        private async Task<List<Race>> GetRacesList()
        {
            List<Race> races = await _api.Methods.Races.List(null);
            return races;
        }

        private async Task<List<SocialFormation>> GetSocialFormationsList()
        {
            List<SocialFormation> socForm = await _api.Methods.SocForms.List(null);
            return socForm;
        }

        private async Task<List<Character>> GetCharactersList()
        {
            List<Character> characters = await _api.Methods.Chars.List(null);
            return characters;
        }

        private async Task<List<Gender>> GetGendersList()
        {
            List<Gender> genders = await _api.Methods.Genders.List();
            return genders;
        }

        public void UpdateLocationsList()
        {
            var locs = Task.Run(() => GetLocationsList()).Result;

            if (locs is not null)
            {
                _locationsOc.Clear();
                foreach (var loc in locs)
                {
                    _locationsOc.Add(loc);
                }
            }
        }

        public void UpdateSocFormsList()
        {
            var socForms = Task.Run(() => GetSocialFormationsList()).Result;

            if (socForms is not null)
            {
                _socFormsOc.Clear();
                foreach (var sf in socForms)
                {
                    _socFormsOc.Add(sf);
                }
            }
        }

        public void UpdateRacesList()
        {
            var races = Task.Run(() => GetRacesList()).Result;

            if (races is not null)
            {
                _racesOc.Clear();
                foreach (var race in races)
                {
                    _racesOc.Add(race);
                }
            }
        }

        public void UpdateCharactersList()
        {
            var chars = Task.Run(() => GetCharactersList()).Result;

            if (chars is not null)
            {
                _charsOc.Clear();
                foreach (var chr in chars)
                {
                    _charsOc.Add(chr);
                }

                if (_character is not null)
                {
                    var chr = _charsOc.FirstOrDefault(chr => chr.Id == _character.Id);
                    _charsOc.Remove(chr);
                }
            }
        }

        public void UpdateGendersList()
        {
            var genders = Task.Run(() => GetGendersList()).Result;

            if (genders is not null)
            {
                _gendersOc.Clear();
                foreach (var gender in genders)
                {
                    _gendersOc.Add(gender);
                }
            }
        }

        private void CharacterEditWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _fatherView.Source = _charsOc;
            _fatherView.Filter += (o, e) => DataCardView_NameFilter(FatherFindTextBox, e);
            FatherComboBox.ItemsSource = _fatherView.View;

            _altCharsView.Source = _charsOc;
            _altCharsView.Filter += (o, e) => DataCardView_NameFilter(AltCharsFindTextBox, e);
            _altCharsView.Filter += (o, e) => DataPanels_AddedFilter(_addedAltChars, e);
            AltCharsFindComboBox.ItemsSource = _altCharsView.View;

            _childrenView.Source = _childrenOc;
            ChildrenList.ItemsSource = _childrenView.View;

            _childAddView.Source = _charsOc;
            //_childAddView.Filter += (o, e) => DataCardView_NameFilter(AddChildFindTextBox, e);
            _childAddView.Filter += Children_AddedFilter;
            //AddChildFindComboBox.ItemsSource = _childAddView.View;

            _motherView.Source = _charsOc;
            _motherView.Filter += (o, e) => DataCardView_NameFilter(MotherFindTextBox, e);
            MotherComboBox.ItemsSource = _motherView.View;

            _racesView.Source = _racesOc;
            _racesView.Filter += (o, e) => DataCardView_NameFilter(RaceFindTextBox, e);
            RaceFindComboBox.ItemsSource = _racesView.View;

            _locBirthView.Source = _locationsOc;
            _locBirthView.Filter += (o, e) => DataCardView_NameFilter(LocBirthFindTextBox, e);
            LocBirthFindComboBox.ItemsSource = _locBirthView.View;

            _locDeathView.Source = _locationsOc;
            _locDeathView.Filter += (o, e) => DataCardView_NameFilter(LocDeathFindTextBox, e);
            LocDeathFindComboBox.ItemsSource = _locDeathView.View;

            _socFormsView.Source = _socFormsOc;
            _socFormsView.Filter += (o, e) => DataCardView_NameFilter(SocFormFindTextBox, e);
            _socFormsView.Filter += (o, e) => DataPanels_AddedFilter(_addedSocForms, e);
            SocFormFindComboBox.ItemsSource = _socFormsView.View;

            _gendersView.Source = _gendersOc;
            GenderSymbolComboBox.ItemsSource = _gendersView.View;

            DateBirthSign.ItemsSource = _signs;
            DateDeathSign.ItemsSource = _signs;

            UpdateCardData();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var editedChar = new Character()
            {
                Name = CharacterNameTextBox.Text
            };
            editedChar.Desc = _rtbTextHandler.GetAsString();

            if (GrowthTextBox.Text.Length > 0)
                editedChar.Growth = int.Parse(GrowthTextBox.Text);

            // Альтернативные имена и титулы/звания
            if (AltNamesTextBox.Text.Length > 0)
                editedChar.AltNames = ParseStringList(AltNamesTextBox.Text);
            if (TitlesTextBox.Text.Length > 0)
                editedChar.Titles = ParseStringList(TitlesTextBox.Text);

            // Даты рождения и смерти
            if (DateBirthPicker.SelectedDate.HasValue)
            {
                editedChar.DateBirth = DateBirthPicker.SelectedDate.Value.ToString("yyyyyy-MM-dd");
                if (DateBirthSign.SelectedIndex == 1)
                    editedChar.DateBirth = "-" + editedChar.DateBirth;
            }
            if (DateDeathPicker.SelectedDate.HasValue)
            {
                editedChar.DateDeath = DateDeathPicker.SelectedDate.Value.ToString("yyyyyy-MM-dd");
                if (DateDeathSign.SelectedIndex == 1)
                    editedChar.DateDeath = "-" + editedChar.DateDeath;
            }
            
            // Пол и раса
            if (GenderSymbolComboBox.SelectedItem is not null)
                editedChar.GenderId = (GenderSymbolComboBox.SelectedItem as Gender).Id;
            if (RaceFindComboBox.SelectedItem is not null)
                editedChar.RaceId = (RaceFindComboBox.SelectedItem as Race).Id;

            // Локации смерти и рождения
            if (LocBirthFindComboBox.SelectedItem is not null)
                editedChar.LocBirthId = (LocBirthFindComboBox.SelectedItem as Location).Id;
            if (LocDeathFindComboBox.SelectedItem is not null)
                editedChar.LocDeathId = (LocDeathFindComboBox.SelectedItem as Location).Id;

            // Отец и мать
            if (FatherComboBox.SelectedItem is not null)
                editedChar.BioFatherId = (FatherComboBox.SelectedItem as Character).Id;
            if (MotherComboBox.SelectedItem is not null)
                editedChar.BioMotherId = (MotherComboBox.SelectedItem as Character).Id;

            // Дети
            if (_childrenOc.Count > 0)
                editedChar.ChildrenIds = GetIdsArray<Character>(_childrenOc.ToList());

            // Альтернативные имена
            if (_addedAltChars.Count > 0)
                editedChar.AltCharsIds = GetIdsArray(_addedAltChars);

            // Социальные формирования
            if (_addedSocForms.Count > 0)
                editedChar.SocFormsIds = GetIdsArray(_addedSocForms);

            // Цвета волос и глаз
            if (ColorHairValueTextBox.Text.Length > 1)
                editedChar.ColorHair = ColorHairValueTextBox.Text;
            if (ColorEyesValueTextBox.Text.Length > 1)
                editedChar.ColorEyes = ColorEyesValueTextBox.Text;

            // Передача в API
            var result = false;
            if (_addMode)
            {
                result = Task.Run(() => _api.Methods.Chars.Add(editedChar)).Result;
            }
            else
            {
                editedChar.Id = _character.Id;
                result = Task.Run(() => _api.Methods.Chars.Change(editedChar)).Result;
            }

            if (!result)
                MessageBox.Show("Произошла ошибка, данные не добавлены");
            else
                this.Close();
        }

        private int[] GetIdsArray<T>(List<T> collection) where T : IApiDataCard
        {
            var array = new int[collection.Count];
            for (int i = 0; i < collection.Count; i++)
                array[i] = collection[i].Id.Value;
            return array;
        }

        private string[] ParseStringList(string text)
        {
            string[] res = text.Split(',');
            for (int i = 0; i < res.Length; i++)
                res[i] = res[i].Trim();

            return res;
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
            // Списки данных
            UpdateCharactersList();
            UpdateLocationsList();
            UpdateRacesList();
            UpdateSocFormsList();
            UpdateGendersList();

            if (_character is not null)
            {
                CharacterEditWindow.Title = _character.Name + " – изменение";
                _addMode = false;

                // Пол
                if (_character.GenderId is not null)
                {
                    var gender = _gendersOc.FirstOrDefault(g => g.Id == _character.GenderId);
                    if (gender is not null)
                        GenderSymbolComboBox.SelectedItem = gender;
                }

                // Имя и описание персонажа
                CharacterNameTextBox.Text = _character.Name;
                CharDescField.Document.Blocks.Clear();
                if (_rtbTextHandler.SetFromString(_character.Desc) is not null)
                    RtbTextHandler.ShowError(_rtbTextHandler.LastException);

                // Другие имена
                string altNames = "";
                foreach (var altName in _character.AltNames)
                    altNames += altName + ", ";
                AltNamesTextBox.Text = altNames.Length > 2 ? altNames[0..^2] : "";

                // Титулы и звания
                string titles = "";
                foreach (var title in _character.Titles)
                    titles += title + ", ";
                TitlesTextBox.Text = titles.Length > 2 ? titles[0..^2] : "";

                // Отец и мать
                if (_character.BioFatherId is not null)
                {
                    var fatherChar = _charsOc.FirstOrDefault(chr => chr.Id == _character.BioFatherId);
                    if (fatherChar != null)
                        FatherComboBox.SelectedItem = fatherChar;
                }
                if (_character.BioMotherId is not null)
                {
                    var motherChar = _charsOc.FirstOrDefault(chr => chr.Id == _character.BioMotherId);
                    if (motherChar != null)
                        MotherComboBox.SelectedItem = motherChar;
                }

                // Раса
                if (_character.RaceId is not null)
                {
                    var race = _racesOc.FirstOrDefault(r => r.Id == _character.RaceId);
                    if (race is not null)
                        RaceFindComboBox.SelectedItem = race;
                }

                // Локации рождения и смерти
                if (_character.LocBirthId is not null)
                {
                    var loc = _locationsOc.FirstOrDefault(loc => loc.Id == _character.LocBirthId);
                    if (loc is not null)
                        LocBirthFindComboBox.SelectedItem = loc;
                }
                if (_character.LocDeathId is not null)
                {
                    var loc = _locationsOc.FirstOrDefault(loc => loc.Id == _character.LocDeathId);
                    if (loc is not null)
                        LocDeathFindComboBox.SelectedItem = loc;
                }

                // Рост
                if (_character.Growth is not null)
                {
                    GrowthTextBox.Text = _character.Growth.Value.ToString();
                }

                // Дата рождения и смерти
                if (_character.DateBirth is not null)
                {
                    try
                    {
                        var date = ApiUtils.StrToDate(_character.DateBirth);
                        DateBirthPicker.SelectedDate = date;
                        if (_character.DateBirth.StartsWith('-'))
                            DateBirthSign.SelectedIndex = 1;
                    }
                    catch
                    {
                        DateBirthPicker.SelectedDate = null;
                    }
                }
                if (_character.DateDeath is not null)
                {
                    try
                    {
                        var date = ApiUtils.StrToDate(_character.DateDeath);
                        DateDeathPicker.SelectedDate = date;
                        if (_character.DateDeath.StartsWith('-'))
                            DateDeathSign.SelectedIndex = 1;
                    }
                    catch
                    {
                        DateDeathPicker.SelectedDate = null;
                    }
                }
                
                // Цвет волос и глаз
                if (_character.ColorHair is not null)
                {
                    var color = (Color)ColorConverter.ConvertFromString("#FF" + _character.ColorHair[1..]);
                    var brush = new SolidColorBrush(color);
                    ColorHairRect.Fill = brush;
                    ColorHairPicker.SelectedBrush = brush;

                }
                if (_character.ColorEyes is not null)
                {
                    var color = (Color)ColorConverter.ConvertFromString("#FF" + _character.ColorEyes[1..]);
                    var brush = new SolidColorBrush(color);
                    ColorEyesRect.Fill = brush;
                    ColorEyesPicker.SelectedBrush = brush;
                }

                // Дети
                if (_character.ChildrenIds is not null)
                {
                    _childrenOc.Clear();
                    foreach (var childId in _character.ChildrenIds)
                        _childrenOc.Add(_charsOc.FirstOrDefault(chr => chr.Id == childId));
                    _childAddView.View.Refresh();
                }
                PlrWpfUtils.ClearDataGridSelection(ChildrenList);

                // Социальные формирования
                if (_character.SocFormsIds is not null)
                {
                    foreach (var socFormId in _character.SocFormsIds)
                    {
                        AddSocFormNameLabel(_socFormsOc.FirstOrDefault(sf => sf.Id == socFormId));
                    }
                    SocFormsPanel.UpdateLayout();
                    _socFormsView.View.Refresh();
                }

                // Другие карточки личности
                if (_character.AltCharsIds is not null)
                {
                    foreach (var altCharId in _character.AltCharsIds)
                    {
                        AddAltCharNameLabel(_charsOc.FirstOrDefault(chr => chr.Id == altCharId));
                    }
                    AltCharsPanel.UpdateLayout();
                    _altCharsView.View.Refresh();
                }
            }
        }

        private WrapPanel WrapDataNameLabel(IApiDataCard dataCard)
        {
            var wrapPanel = new WrapPanel();
            wrapPanel.Margin = new Thickness(5, 0, 0, 0);

            var label = new Label();
            label.Content = dataCard.Name;
            label.Style = Application.Current.TryFindResource("ClickableDataLabel") as Style;
            label.BorderThickness = new Thickness(0.0);

            var btnClose = new Button();
            btnClose.Content = "🞫";
            btnClose.BorderThickness = new Thickness(0.0);
            btnClose.Background = Application.Current.TryFindResource("PlrBlackLite") as Brush;
            btnClose.Padding = new Thickness(4, 0, 4, 0);

            wrapPanel.Children.Add(label);
            wrapPanel.Children.Add(btnClose);

            return wrapPanel;
        }

        private void AddSocFormNameLabel(SocialFormation socForm)
        {
            var wrappPanel = WrapDataNameLabel(socForm);
            
            var label = wrappPanel.Children[0] as Label;
            label.MouseLeftButtonUp += (object sender, MouseButtonEventArgs e) =>
            {
                var window = _windowsManager.CreateSocFormDetailsWindow(socForm.Id.Value);
                window.Show();
            };
            var btn = wrappPanel.Children[1] as Button;
            btn.Click += (object sender, RoutedEventArgs e) =>
            {
                SocFormsPanel.Children.Remove(wrappPanel);
                SocFormsPanel.UpdateLayout();
                _addedSocForms.Remove(socForm);
                _socFormsView.View.Refresh();
            };

            _addedSocForms.Add(socForm);
            SocFormsPanel.Children.Add(wrappPanel);
        }

        private void AddAltCharNameLabel(Character altChar)
        {
            var wrappPanel = WrapDataNameLabel(altChar);

            var label = wrappPanel.Children[0] as Label;
            label.MouseLeftButtonUp += (object sender, MouseButtonEventArgs e) =>
            {
                var window = _windowsManager.CreateCharacterDetailsWindow(altChar.Id.Value);
                window.Show();
            };
            var btn = wrappPanel.Children[1] as Button;
            btn.Click += (object sender, RoutedEventArgs e) =>
            {
                AltCharsPanel.Children.Remove(wrappPanel);
                AltCharsPanel.UpdateLayout();
                _addedAltChars.Remove(altChar);
                _altCharsView.View.Refresh();
            };

            _addedAltChars.Add(altChar);
            AltCharsPanel.Children.Add(wrappPanel);
        }

        private void DataCardView_NameFilter(TextBox filterTextBox, FilterEventArgs e)
        {
            var card = e.Item as IApiDataCard;

            if (card is not null)
            {
                if (card.Name.ToLower().Contains(filterTextBox.Text.ToLower()))
                    e.Accepted = true;
                else
                    e.Accepted = false;
            }
        }

        private void DataPanels_AddedFilter(List<IApiDataCard> addedCards, FilterEventArgs e)
        {
            var card = e.Item as IApiDataCard;
            
            if (card is not null)
            {
                e.Accepted = !addedCards.Contains(card);
            }
        }

        private void Children_AddedFilter(object sender, FilterEventArgs e)
        {
            var card = e.Item as Character;

            if (card is not null)
            {
                e.Accepted = !_childrenOc.Contains(card);
            }
        }

        private void FatherFindTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _fatherView.View.Refresh();
        }

        private void MotherFindTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _motherView.View.Refresh();
        }

        private void SocFormFindTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _socFormsView.View.Refresh();
        }

        private void AltCharsFindTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _altCharsView.View.Refresh();
        }

        private void RaceFindTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _racesView.View.Refresh();
        }

        private void LocBirthFindTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _locBirthView.View.Refresh();
        }

        private void LocDeathFindTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _locDeathView.View.Refresh();
        }

        private void SocFormAdd_Click(object sender, RoutedEventArgs e)
        {
            var socForm = SocFormFindComboBox.SelectedItem as SocialFormation;
            if (socForm is not null)
            {
                AddSocFormNameLabel(socForm);
                SocFormsPanel.UpdateLayout();
                _socFormsView.View.Refresh();
            }
        }

        private void AltCharsAdd_Click(object sender, RoutedEventArgs e)
        {
            var altChar = AltCharsFindComboBox.SelectedItem as Character;
            if (altChar is not null)
            {
                AddAltCharNameLabel(altChar);
                AltCharsPanel.UpdateLayout();
                _altCharsView.View.Refresh();
            }
        }

        //private void AddChildFindTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    _childAddView.View.Refresh();
        //}

        //private void AddChildButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var selectedChild = AddChildFindComboBox.SelectedItem as Character;
        //    if (selectedChild is not null)
        //    {
        //        _childrenOc.Add(selectedChild);
        //        _childrenView.View.Refresh();
        //        _childAddView.View.Refresh();
        //    }
        //}

        //private void RemoveChildButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var selectedChild = ChildrenList.SelectedItem as Character;
        //    if (selectedChild is not null)
        //    {
        //        _childrenOc.Remove(selectedChild);
        //        _childrenView.View.Refresh();
        //        _childAddView.View.Refresh();

        //        PlrWpfUtils.ClearDataGridSelection(ChildrenList);
        //    }
        //}

        private void ColorHairRect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ColorHairPopup.IsOpen = !ColorHairPopup.IsOpen;
        }

        private void ColorHairPicker_SelectedColorChanged(object sender, HandyControl.Data.FunctionEventArgs<Color> e)
        {
            ColorHairRect.Fill = ColorHairPicker.SelectedBrush;
            ColorHairValueTextBox.Text = "#" + ColorHairPicker.SelectedBrush.Color.ToString()[3..];
        }

        private void ColorEyesRect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ColorEyesPopup.IsOpen = !ColorEyesPopup.IsOpen;
        }

        private void ColorEyesPicker_SelectedColorChanged(object sender, HandyControl.Data.FunctionEventArgs<Color> e)
        {
            ColorEyesRect.Fill = ColorEyesPicker.SelectedBrush;
            ColorEyesValueTextBox.Text = "#" + ColorEyesPicker.SelectedBrush.Color.ToString()[3..];
        }
    }
}
