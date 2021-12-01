using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml;

namespace PlrDesktop.Lib
{
    public class RtbTextHandler
    {
        private RichTextBox _rtb = null;
        private string _dataFormat = DataFormats.Xaml;
        private TextRange _textRange;

        public struct RtbThError
        {
            public Exception Exception { get; set; }
            public string Message { get; set; }

            public override string ToString()
            {
                return Message;
            }
        }

        public RichTextBox Rtb
        { 
            get { return _rtb; } 
            set 
            { 
                _rtb = value;
                _textRange = new TextRange(_rtb.Document.ContentStart, _rtb.Document.ContentEnd);
            }
        }

        public string DataFormat
        {
            get { return _dataFormat; }
            set { _dataFormat = value; }
        }

        public RtbThError? LastException { get; set; } = null;

        public bool RemoveForegrounds { get; set; } = true;


        public RtbTextHandler()
        {
        }

        public RtbTextHandler(RichTextBox rtb)
        {
            Rtb = rtb;
        }

        public RtbTextHandler(RichTextBox rtb, string dataFormat) : this(rtb)
        {
            DataFormat = dataFormat;
        }

        public string GetAsString(string dataFormat)
        {
            string result = null;
            DoRemoveForegrounds();

            using (MemoryStream stream = new MemoryStream())
            {
                _textRange.Save(stream, dataFormat);
                stream.Seek(0, SeekOrigin.Begin);

                using (StreamReader streamReader = new(stream))
                {
                    result = streamReader.ReadToEnd();
                }
            }

            return result;
        }

        public string GetAsString()
        {
            return GetAsString(DataFormat);
        }

        public RtbThError? SetFromString(string data, string dataFormat)
        {
            LastException = null;

            byte[] dataBuffer = ASCIIEncoding.Default.GetBytes(data);
            using (MemoryStream stream = new MemoryStream(dataBuffer))
            {
                try
                {
                    _textRange.Load(stream, dataFormat);
                }
                catch (System.Windows.Markup.XamlParseException ex)
                {
                    _textRange.Load(stream, DataFormats.Text);

                    LastException = new RtbThError
                    {
                        Exception = ex,
                        Message = "Возникла ошибка при чтении Xaml. Данные будут загружены в виде простого текста."
                    };
                }
                catch (Exception ex)
                {
                    _textRange.Load(stream, DataFormats.Text);

                    LastException = new RtbThError
                    {
                        Exception = ex,
                        Message = "Возникла ошибка при считывании форматированных данных." +
                        " Будет предпринята попытка загрузить данные в виде простого текста."
                    };
                }
            }

            DoRemoveForegrounds();
            return LastException;
        }

        public RtbThError? SetFromString(string data)
        {
            return SetFromString(data, DataFormat);
        }

        public static void ShowError(RtbThError? error)
        {
            if (error is not null)
                MessageBox.Show(error.Value.Message, "Text parsing error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void DoRemoveForegrounds()
        {
            if (RemoveForegrounds)
            {
                foreach (var block in _rtb.Document.Blocks)
                {
                    block.ClearValue(Block.ForegroundProperty);
                }
            }
        }
    }
}
