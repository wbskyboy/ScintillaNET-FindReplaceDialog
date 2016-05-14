﻿namespace ScintillaNET_FindReplaceDialog.FindAllResults
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    using ScintillaNET;

    public partial class FindAllResultsPanel : UserControl
    {
        #region Fields

        private List<ScintillaNET_FindReplaceDialog.CharacterRange> _findAllResults;
        private Scintilla _scintilla;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a new instance of FindAllResultsPanel
        /// </summary>
        public FindAllResultsPanel()
        {
            InitializeComponent();

            FindResultsScintilla.Styles[Style.Default].Font = "Consolas";
            FindResultsScintilla.Styles[Style.Default].Size = 10;

            FindResultsScintilla.ClearAll();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the Scintilla control that was searched to generate the find results.
        /// Allows the FindAllResults list to be double clicked and results indicated in the original Scintilla.
        /// </summary>
        public Scintilla Scintilla
        {
            get { return _scintilla; }
            set { _scintilla = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Updates the find all results panel
        /// </summary>
        /// <param name="FindReplace">The FindReplace instance used to generate the find results.</param>
        /// <param name="FindAllResults"></param>
        public void UpdateFindAllResults(FindReplace FindReplace, List<ScintillaNET_FindReplaceDialog.CharacterRange> FindAllResults)
        {
            if (Scintilla == null)
                return;

            _findAllResults = new List<ScintillaNET_FindReplaceDialog.CharacterRange>(FindAllResults);

            FindResultsScintilla.ClearAll();

            Indicator _indicator;
            _indicator = FindResultsScintilla.Indicators[16];
            _indicator.ForeColor = Color.Purple;
            _indicator.Style = IndicatorStyle.RoundBox;

            FindResultsScintilla.IndicatorCurrent = _indicator.Index;

            foreach (var item in _findAllResults)
            {
                int startLine = Scintilla.LineFromPosition(item.cpMin);
                int endLine = Scintilla.LineFromPosition(item.cpMax);

                if (startLine == endLine)
                {
                    FindResultsScintilla.AppendText(string.Format("Line {0}: {1}",
                        startLine + 1, Scintilla.Lines[startLine].Text));

                    int LinePos = Scintilla.Lines[startLine].Position;
                    int startPosInLine = item.cpMin - LinePos;

                    int lastLineStartPos = FindResultsScintilla.Lines[FindResultsScintilla.Lines.Count].Position;

                    //FindResultsScintilla.IndicatorFillRange(lastLineStartPos + startPosInLine, item.cpMax - item.cpMin);

                }
            }
        }

        private void FindResultsScintilla_KeyUp(object sender, KeyEventArgs e)
        {
            int pos = FindResultsScintilla.CurrentPosition;
            int selectedLine = FindResultsScintilla.LineFromPosition(pos);

            if (_findAllResults.Count > selectedLine)
            {
                ScintillaNET_FindReplaceDialog.CharacterRange CharRange = _findAllResults[selectedLine];
                Scintilla.SetSelection(CharRange.cpMin, CharRange.cpMax);
                Scintilla.ScrollCaret();
            }
        }

        private void FindResultsScintilla_MouseClick(object sender, MouseEventArgs e)
        {
            int pos = FindResultsScintilla.CharPositionFromPointClose((e.Location).X, (e.Location).Y);
            if (pos == -1)
                return;

            int selectedLine = FindResultsScintilla.LineFromPosition(pos);

            ScintillaNET_FindReplaceDialog.CharacterRange CharRange = _findAllResults[selectedLine];
            Scintilla.SetSelection(CharRange.cpMin, CharRange.cpMax);
            Scintilla.ScrollCaret();
        }

        private void FindResultsScintilla_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int pos = FindResultsScintilla.CharPositionFromPointClose((e.Location).X, (e.Location).Y);
            if (pos == -1)
                return;

            int selectedLine = FindResultsScintilla.LineFromPosition(pos);

            ScintillaNET_FindReplaceDialog.CharacterRange CharRange = _findAllResults[selectedLine];
            Scintilla.SetSelection(CharRange.cpMin, CharRange.cpMax);
            Scintilla.ScrollCaret();
        }

        #endregion Methods
    }
}