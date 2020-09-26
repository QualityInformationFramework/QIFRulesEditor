using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.Wpf
{
    /// <summary> Settings of the application </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class SettingsViewModel : INotifyPropertyChanged
    {
        /// <summary> Gets or sets a value indicating whether current line in the editor is highlighted. </summary>
        public bool HighlightCurrentLine
        {
            get { return mHighlightCurrentLine; }
            set
            {
                mHighlightCurrentLine = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HighlightCurrentLine"));
            }
        }

        /// <summary> Gets or sets a value indicating whether the editor uses word wrapping. </summary>
        public bool WordWrap
        {
            get { return mWordWrap; }
            set
            {
                mWordWrap = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WordWrap"));
            }
        }

        /// <summary> Gets or sets a value indicating whether the editor shows line numbers. </summary>
        public bool ShowLineNumbers
        {
            get { return mShowLineNumbers; }
            set
            {
                mShowLineNumbers = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ShowLineNumbers"));
            }
        }

        /// <summary> Gets or sets font name of the editor. </summary>
        public string FontName
        {
            get { return mFontName; }
            set
            {
                mFontName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FontName"));
            }
        }

        /// <summary> Gets or sets font size of the editor. </summary>
        public double FontSize
        {
            get { return mFontSize; }
            set
            {
                mFontSize = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FontSize"));
            }
        }

        /// <summary> Gets or sets a value indicating whether font of the editor is bold. </summary>
        public bool IsFontBold
        {
            get { return mIsFontBold; }
            set
            {
                mIsFontBold = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsFontBold"));
            }
        }

        /// <summary> Gets or sets a value indicating whether font of the editor is italic. </summary>
        public bool IsFontItalic
        {
            get { return mIsFontItalic; }
            set
            {
                mIsFontItalic = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsFontItalic"));
            }
        }

        /// <summary> Occurs when a property changed. </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary> Creates a copy of this settings. </summary>
        /// <returns> Created copy </returns>
        public SettingsViewModel Clone()
        {
            return new SettingsViewModel()
            {
                mFontName = mFontName,
                mFontSize = mFontSize,
                mHighlightCurrentLine = mHighlightCurrentLine,
                mWordWrap = mWordWrap,
                mShowLineNumbers = mShowLineNumbers,
                mIsFontBold = mIsFontBold,
                mIsFontItalic = mIsFontItalic
            };
        }

        /// <summary> Applies settings from the specified object. </summary>
        /// <param name="other"> Settings to apply </param>
        public void Apply(SettingsViewModel other)
        {
            FontName = other.FontName;
            FontSize = other.FontSize;
            IsFontBold = other.IsFontBold;
            IsFontItalic = other.IsFontItalic;

            WordWrap = other.WordWrap;
            HighlightCurrentLine = other.HighlightCurrentLine;
            ShowLineNumbers = other.ShowLineNumbers;
        }

        bool mHighlightCurrentLine = false;
        bool mWordWrap = false;
        bool mShowLineNumbers = true;
        string mFontName = "Courier New";
        bool mIsFontBold = false;
        bool mIsFontItalic = false;
        double mFontSize = 11 * 96.0 / 72.0;
    }
}
