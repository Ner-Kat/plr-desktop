using PlrDesktop.ApiInteraction;
using PlrDesktop.Datacards;
using PlrDesktop.Datacards.Utils;
using PlrDesktop.Lib;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;

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

        private static string _allowedDateSymbols = "01234567890.";
        private delegate bool CheckOperation(char s, string text);
        private static List<CheckOperation> checker = new()
        {
            (char s, string text) => _allowedDateSymbols.Contains(s),
            (char s, string text) => !(text.EndsWith('.') && s == '.'),
            (char s, string text) => !(text.Length == 0 && s == '.'),
            (char s, string text) => !(s == '.' && ApiUtils.ContainsCount(text, '.') >= 2)
        };

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
            _fatherView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            _fatherView.Filter += (o, e) => DataCardView_NameFilter(FatherFindTextBox, e);
            FatherComboBox.ItemsSource = _fatherView.View;

            _altCharsView.Source = _charsOc;
            _altCharsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            _altCharsView.Filter += (o, e) => DataCardView_NameFilter(AltCharsFindTextBox, e);
            _altCharsView.Filter += (o, e) => DataPanels_AddedFilter(_addedAltChars, e);
            AltCharsFindComboBox.ItemsSource = _altCharsView.View;

            _childrenView.Source = _childrenOc;
            _childrenView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            ChildrenList.ItemsSource = _childrenView.View;

            _childAddView.Source = _charsOc;
            _childrenView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            //_childAddView.Filter += (o, e) => DataCardView_NameFilter(AddChildFindTextBox, e);
            _childAddView.Filter += Children_AddedFilter;
            //AddChildFindComboBox.ItemsSource = _childAddView.View;

            _motherView.Source = _charsOc;
            _motherView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            _motherView.Filter += (o, e) => DataCardView_NameFilter(MotherFindTextBox, e);
            MotherComboBox.ItemsSource = _motherView.View;

            _racesView.Source = _racesOc;
            _racesView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            _racesView.Filter += (o, e) => DataCardView_NameFilter(RaceFindTextBox, e);
            RaceFindComboBox.ItemsSource = _racesView.View;

            _locBirthView.Source = _locationsOc;
            _locBirthView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            _locBirthView.Filter += (o, e) => DataCardView_NameFilter(LocBirthFindTextBox, e);
            LocBirthFindComboBox.ItemsSource = _locBirthView.View;

            _locDeathView.Source = _locationsOc;
            _locDeathView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            _locDeathView.Filter += (o, e) => DataCardView_NameFilter(LocDeathFindTextBox, e);
            LocDeathFindComboBox.ItemsSource = _locDeathView.View;

            _socFormsView.Source = _socFormsOc;
            _socFormsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            _socFormsView.Filter += (o, e) => DataCardView_NameFilter(SocFormFindTextBox, e);
            _socFormsView.Filter += (o, e) => DataPanels_AddedFilter(_addedSocForms, e);
            SocFormFindComboBox.ItemsSource = _socFormsView.View;

            _gendersView.Source = _gendersOc;
            GenderSymbolComboBox.ItemsSource = _gendersView.View;

            DateBirthSign.ItemsSource = _signs;
            DateDeathSign.ItemsSource = _signs;

            GenderSymbolComboBox.SelectedIndex = 0;

            UpdateCardData();

            if (_racesOc.Count > 0 && RaceFindComboBox.SelectedIndex == -1)
                RaceFindComboBox.SelectedIndex = 0;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (CharacterNameTextBox.Text is null || CharacterNameTextBox.Text.Length == 0)
            {
                CharacterNameTextBox.BorderThickness = new Thickness(2);
                return;
            }

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
            if (DateBirthPicker.Text is not null)
            {
                if (DateBirthPicker.Text.Length == 0)
                    editedChar.DateBirth = "";
                else
                    editedChar.DateBirth = ApiUtils.StrDateToApiDate(DateBirthPicker.Text,
                        DateBirthSign.SelectedIndex == 0);
            }
            if (DateDeathPicker.Text is not null)
            {
                if (DateDeathPicker.Text.Length == 0)
                    editedChar.DateDeath = "";
                else
                    editedChar.DateDeath = ApiUtils.StrDateToApiDate(DateDeathPicker.Text,
                        DateDeathSign.SelectedIndex == 0);
            }

            // Пол и раса
            if (GenderSymbolComboBox.SelectedItem is not null)
            {
                editedChar.GenderId = (GenderSymbolComboBox.SelectedItem as Gender).Id;
            }
            else
            {
                GenderSymbolComboBox.BorderThickness = new Thickness(2);
                return;
            }

            if (RaceFindComboBox.SelectedItem is not null)
            {
                editedChar.RaceId = (RaceFindComboBox.SelectedItem as Race).Id;
            }
            else
            {
                RaceFindComboBox.BorderThickness = new Thickness(2);
                return;
            }

            // Локации смерти и рождения
            if (LocBirthFindComboBox.SelectedItem is not null)
                editedChar.LocBirthId = (LocBirthFindComboBox.SelectedItem as Location).Id;
            else
                editedChar.LocBirthId = -1;

            if (LocDeathFindComboBox.SelectedItem is not null)
                editedChar.LocDeathId = (LocDeathFindComboBox.SelectedItem as Location).Id;
            else
                editedChar.LocDeathId = -1;

            // Отец и мать
            if (FatherComboBox.SelectedItem is not null)
                editedChar.BioFatherId = (FatherComboBox.SelectedItem as Character).Id;
            else
                editedChar.BioFatherId = -1;
            if (MotherComboBox.SelectedItem is not null)
                editedChar.BioMotherId = (MotherComboBox.SelectedItem as Character).Id;
            else
                editedChar.BioMotherId = -1;

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
                    string date = ApiUtils.ApiDateToStrDate(_character.DateBirth);
                    if (date is not null)
                    {
                        DateBirthPicker.Text = date;
                        if (_character.DateBirth.StartsWith('-'))
                            DateBirthSign.SelectedIndex = 1;
                    }
                }
                if (_character.DateDeath is not null)
                {
                    string date = ApiUtils.ApiDateToStrDate(_character.DateDeath);
                    if (date is not null)
                    {
                        DateDeathPicker.Text = date;
                        if (_character.DateDeath.StartsWith('-'))
                            DateDeathSign.SelectedIndex = 1;
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
            e.Accepted = true;
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

            if (card is not null && e.Accepted)
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


        private void DateBirthPicker_TextChanged(object sender, TextChangedEventArgs e)
        {
            string newText = GetCheckedDateText(DateBirthPicker);

            DateBirthPicker.Text = newText;
            DateBirthPicker.CaretIndex = newText.Length;
        }

        private void DateDeathPicker_TextChanged(object sender, TextChangedEventArgs e)
        {
            string newText = GetCheckedDateText(DateDeathPicker);

            DateDeathPicker.Text = newText;
            DateDeathPicker.CaretIndex = newText.Length;
        }

        private bool IsValidDateSymbol(char s, string text)
        {
            foreach (var cond in checker)
            {
                if (!cond(s, text))
                    return false;
            }

            return true;
        }

        private string GetCheckedDateText(TextBox tb)
        {
            string newText = "";
            foreach (var s in tb.Text)
            {
                if (IsValidDateSymbol(s, newText))
                    newText += s;
            }

            return newText;
        }

        private void FatherSelectionDel_Click(object sender, RoutedEventArgs e)
        {
            FatherComboBox.SelectedIndex = -1;
        }

        private void MotherSelectionDel_Click(object sender, RoutedEventArgs e)
        {
            MotherComboBox.SelectedIndex = -1;
        }

        private void DateBirthSelectionDel_Click(object sender, RoutedEventArgs e)
        {
            DateBirthPicker.Text = "";
        }

        private void DateDeathSelectionDel_Click(object sender, RoutedEventArgs e)
        {
            DateDeathPicker.Text = "";
        }

        private void LocBirthSelectionDel_Click(object sender, RoutedEventArgs e)
        {
            LocBirthFindComboBox.SelectedIndex = -1;
        }

        private void LocDeathSelectionDel_Click(object sender, RoutedEventArgs e)
        {
            LocDeathFindComboBox.SelectedIndex = -1;
        }
    }
}
