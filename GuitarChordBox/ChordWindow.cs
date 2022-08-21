using Buzz.MachineInterface;
using BuzzGUI.Common;
using BuzzGUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace WDE.GuitarChordBox
{
    public struct ChordState
    {
        public Key key;
        public Note[] note;
        public int transpose;
    }

    class ChordWindow : Window
    {
        private GuitarChordBoxMachine gcbm;
        private Grid chordsGrid;
        public NumericUpDown nudTranspose;
        private ScrollViewer scrollViewer;
        private Note[] previousNotes = new Note[6];

        private Dictionary<Key, ChordState> chordStates = new Dictionary<Key, ChordState>();

        public ChordWindow(GuitarChordBoxMachine gcb, ResourceDictionary resources)
        {
            gcbm = gcb;

            gcbm.LoadChordStates(chordStates);

            Resources = resources;
            Style = TryFindResource("ThemeWindowStyle") as Style;

            this.WindowStyle = WindowStyle.ToolWindow;
            //this.ResizeMode = ResizeMode.NoResize;

            this.Width = 640;
            this.Height = 500;
            this.MinWidth = 640;
            this.MinHeight = 340;

            new WindowInteropHelper(this).Owner = BuzzGUI.Common.Global.MachineViewHwndSource.Handle;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            Grid mainGrid = new Grid() { Margin = new Thickness(8, 0, 8, 0) };
            mainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50) });
            mainGrid.RowDefinitions.Add(new RowDefinition());

            Grid transposeGrid = new Grid() { Margin = new Thickness(8, 0, 6, 0) };
            transposeGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50) });
            transposeGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100) });
            transposeGrid.ColumnDefinitions.Add(new ColumnDefinition());
            transposeGrid.ColumnDefinitions.Add(new ColumnDefinition());
            Grid.SetRow(transposeGrid, 0);

            TextBlock tbTranspose = new TextBlock() { Text = "Transpose: ", HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(8, 16, 8, 0) };
            nudTranspose = new NumericUpDown()
            {
                Width = 80,
                Height = 20,
                DecimalPlaces = 0,
                Change = (decimal)1,
                Minimum = (decimal)-12,
                Maximum = (decimal)12,
                Margin = new Thickness(4, 4, 4, 4),
                Value = gcbm.Transpose - 12,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            EnableEvents();
            Grid.SetColumn(tbTranspose, 0);
            Grid.SetColumn(nudTranspose, 1);

            Button btStop = new Button()
            {
                Content = "◼",
                FontSize = 14,
                HorizontalAlignment = HorizontalAlignment.Right,
                Width = 60,
                Height = 30,
                Margin = new Thickness(1, 1, SystemParameters.VerticalScrollBarWidth, 1)
            };
            btStop.Click += BtStop_Click;
            Grid.SetColumn(btStop, 2);

            transposeGrid.Children.Add(tbTranspose);
            transposeGrid.Children.Add(nudTranspose);
            transposeGrid.Children.Add(btStop);

            mainGrid.Children.Add(transposeGrid);

            scrollViewer = new ScrollViewer() { VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Stretch };

            // Create Grid
            chordsGrid = new Grid() { Margin = new Thickness(8, 0, 8, 0) };

            chordsGrid.ColumnDefinitions.Add(new ColumnDefinition()); // Index
            chordsGrid.ColumnDefinitions.Add(new ColumnDefinition()); // Chord
            chordsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            chordsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            chordsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            chordsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            chordsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            chordsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            // chordsGrid.ColumnDefinitions.Add(new ColumnDefinition()); // Play
            // chordsGrid.ColumnDefinitions.Add(new ColumnDefinition()); // Copy

            for (int i = 0; i < GuitarNotes.Chords.Length; i++)
                chordsGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });

            scrollViewer.Content = chordsGrid;

            UpdateChordGridWithTimer();

            Grid.SetRow(scrollViewer, 1);

            mainGrid.Children.Add(scrollViewer);

            this.Content = mainGrid;

            this.Closed += (sender, e) =>
            {
                if (timer != null)
                    timer.Stop();
            };

            for (int i = 0; i < 6; i++)
                previousNotes[i] = new Note(Note.Off);

            this.PreviewKeyDown += (sender, e) =>
            {
                if (e.Key == Key.Add)
                {
                    nudTranspose.Value++;
                    e.Handled = true;
                }
                else if (e.Key == Key.Subtract)
                {
                    nudTranspose.Value--;
                    e.Handled = true;
                }

                else if (SavingChord)
                {
                    if ((e.Key >= Key.A) && (e.Key <= Key.Z) && !e.IsRepeat)
                    {
                        ChordState cs = new ChordState();
                        cs.note = (Note[])previousNotes.Clone();
                        cs.transpose = (int)nudTranspose.Value;
                        cs.key = e.Key;
                        chordStates[e.Key] = cs;

                        gcbm.SaveChordStates(chordStates);
                    }
                }
                else if ((e.Key >= Key.A) && (e.Key <= Key.Z) && !e.IsRepeat)
                {
                    if (chordStates.ContainsKey(e.Key))
                    {
                        ChordState cs = chordStates[e.Key];

                        foreach (IParameter par in gcbm.host.Machine.ParameterGroups[1].Parameters)
                            if (par.Name == "Transpose")
                            {
                                par.SetValue(0, cs.transpose + 12);
                                if (par.Group.Machine.DLL.Info.Version >= 42)
                                    par.Group.Machine.SendControlChanges();
                                break;
                            }

                        SendNote("String E", cs.note[0], 0);
                        SendNote("String A", cs.note[1], 0);
                        SendNote("String D", cs.note[2], 0);
                        SendNote("String G", cs.note[3], 0);
                        SendNote("String B", cs.note[4], 0);
                        SendNote("String E2", cs.note[5], 0);
                    }
                    e.Handled = true;
                }
            };
        }

        private void BtStop_Click(object sender, RoutedEventArgs e)
        {
            gcbm.TrackParamStringE(new Note(Note.Off), 0);
            gcbm.TrackParamStringA(new Note(Note.Off), 0);
            gcbm.TrackParamStringD(new Note(Note.Off), 0);
            gcbm.TrackParamStringG(new Note(Note.Off), 0);
            gcbm.TrackParamStringB(new Note(Note.Off), 0);
            gcbm.TrackParamStringE2(new Note(Note.Off), 0);
        }

        DispatcherTimer timer;

        private void NudTranspose_ValueChanged(object sender, RoutedPropertyChangedEventArgs<decimal> e)
        {
            //gcbm.Transpose = (int)nudTranspose.Value + 12;

            /*
            foreach (IParameter par in gcbm.host.Machine.ParameterGroups[1].Parameters)
                if (par.Name == "Transpose")
                {
                    par.SetValue(0, (int)nudTranspose.Value + 12);
                    if (par.Group.Machine.DLL.Info.Version >= 42)
                        par.Group.Machine.SendControlChanges();
                    break;
                }
            */
            UpdateChordGridWithTimer();
        }

        public void UpdateChordsGridFromMachine()
        {
            DisableEvents();
            nudTranspose.Value = gcbm.Transpose - 12;
            UpdateChordGridWithTimer();
            EnableEvents();
        }

        private void EnableEvents()
        {
            nudTranspose.ValueChanged += NudTranspose_ValueChanged;
        }

        private void DisableEvents()
        {
            nudTranspose.ValueChanged -= NudTranspose_ValueChanged;
        }

        private void UpdateChordGridWithTimer()
        {
            chordsGrid.Children.Clear();

            currentLine = 0;
            if (timer != null)
                timer.Stop();

            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(0) };
            timer.Start();
            timer.Tick += (sender2, args) =>
            {
                //timer.Stop();
                UpdateChordsGrid();
            };
        }
        private int currentLine = 0;

        public bool SavingChord { get; private set; }

        private void UpdateChordsGrid()
        {
            //scrollViewer.Content = null;

            int counter = 0;
            for (int i = currentLine; i < GuitarNotes.Chords.Length; i++)
            {
                TextBlock tbIndex = new TextBlock() { Text = i.ToString("X"), FontSize = 16 };
                //tbIndex.MouseEnter += TbChord_MouseEnter;
                //tbIndex.MouseLeave += TbChord_MouseLeave;
                Grid.SetColumn(tbIndex, 0);
                Grid.SetRow(tbIndex, i);
                chordsGrid.Children.Add(tbIndex);

                string[] chord = GuitarNotes.Chords[i].Split(',');
                TextBlock tbChord = new TextBlock() { Text = chord[0], FontWeight = FontWeights.Bold, FontSize = 16 };
                tbChord.Tag = new Tuple<int, int>(i, 0);
                tbChord.MouseLeftButtonDown += TbChord_MouseLeftButtonDown;
                tbChord.MouseRightButtonDown += TbChord_MouseRightButtonDown;
                tbChord.MouseLeftButtonUp += TbChord_MouseLeftButtonUp;
                tbChord.MouseRightButtonUp += TbChord_MouseRightButtonUp;
                tbChord.MouseEnter += TbChord_MouseEnter;
                tbChord.MouseLeave += TbChord_MouseLeave;
                Grid.SetColumn(tbChord, 1);
                Grid.SetRow(tbChord, i);
                chordsGrid.Children.Add(tbChord);

                for (int j = 1; j < chord.Length; j++)
                {
                    string txt = chord[j];

                    if (txt == "-")
                    {
                        txt = "Off";
                    }
                    else
                    {
                        int iNote = BuzzNote.Parse(chord[j]);
                        int iMidiNote = BuzzNote.ToMIDINote(iNote);
                        iMidiNote += (int)nudTranspose.Value;
                        iNote = BuzzNote.FromMIDINote(iMidiNote);
                        txt = BuzzNote.TryToString(iNote);
                    }
                    TextBlock tbNote = new TextBlock { Text = txt, FontSize = 16 };
                    tbNote.Tag = new Tuple<int, int>(i, j);
                    tbNote.MouseLeftButtonDown += TbChord_MouseLeftButtonDown;
                    tbNote.MouseRightButtonDown += TbChord_MouseRightButtonDown;
                    tbNote.MouseLeftButtonUp += TbChord_MouseLeftButtonUp;
                    tbNote.MouseRightButtonUp += TbChord_MouseRightButtonUp;
                    tbNote.MouseEnter += TbChord_MouseEnter;
                    tbNote.MouseLeave += TbChord_MouseLeave;
                    Grid.SetColumn(tbNote, j + 1);
                    Grid.SetRow(tbNote, i);
                    chordsGrid.Children.Add(tbNote);
                }
                /*
                Button btPlay = new Button()
                {
                    Content = "▶",
                    Tag = i,
                    FontSize = 14,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Width = 60,
                    Height = 28,
                    Margin = new Thickness(1)
                };
                btPlay.Click += BtPlay_Click;
                Grid.SetColumn(btPlay, 8);
                Grid.SetRow(btPlay, i);
                chordsGrid.Children.Add(btPlay);
                
                Button btCopy = new Button() { Content = "Copy", Tag = i, FontSize = 14 };
                btCopy.Click += BtCopy_Click;
                Grid.SetColumn(btCopy, 8);
                Grid.SetRow(btCopy, i);
                // chordsGrid.Children.Add(btCopy);
                */
                counter++;
                currentLine++;
                if (currentLine >= GuitarNotes.Chords.Length)
                {
                    timer.Stop();
                }

                if (counter > 10)
                    break;
            }
        }

        private void TbChord_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.SavingChord = false;
        }

        private void TbChord_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SavingChord = false;
        }

        private void TbChord_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            SavingChord = true;

            foreach (IParameter par in gcbm.host.Machine.ParameterGroups[1].Parameters)
                if (par.Name == "Transpose")
                {
                    par.SetValue(0, (int)nudTranspose.Value + 12);
                    if (par.Group.Machine.DLL.Info.Version >= 42)
                        par.Group.Machine.SendControlChanges();
                    break;
                }

            SendNote("String E", previousNotes[0], 0);
            SendNote("String A", previousNotes[1], 0);
            SendNote("String D", previousNotes[2], 0);
            SendNote("String G", previousNotes[3], 0);
            SendNote("String B", previousNotes[4], 0);
            SendNote("String E2", previousNotes[5], 0);
        }

        private void TbChord_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SavingChord = true;

            var tb = (TextBlock)sender;
            int chordRow = ((Tuple<int, int>)tb.Tag).Item1;
            int chordCol = ((Tuple<int, int>)tb.Tag).Item2;
            if (chordCol == 0)
                PlayChord(chordRow);
            else
                PlayNote(chordRow, chordCol);
        }

        private void TbChord_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;

            SavingChord = false;

            var fadeInAnimation = new DoubleAnimation(0d, new TimeSpan(0, 0, 0, 0, 500));
            tb.Background = new SolidColorBrush(Colors.DarkSlateGray);
            tb.Background.Opacity = 1;
            tb.Background.BeginAnimation(SolidColorBrush.OpacityProperty, fadeInAnimation);

            Mouse.OverrideCursor = null;
        }

        private void TbChord_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;

            var fadeInAnimation = new DoubleAnimation(1d, new TimeSpan(0, 0, 0, 0, 500));
            tb.Background = new SolidColorBrush(Colors.DarkSlateGray);
            tb.Background.Opacity = 0;
            tb.Background.BeginAnimation(SolidColorBrush.OpacityProperty, fadeInAnimation);

            Mouse.OverrideCursor = Cursors.Hand;

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                int chordRow = ((Tuple<int, int>)tb.Tag).Item1;
                int chordCol = ((Tuple<int, int>)tb.Tag).Item2;
                if (chordCol == 0)
                    PlayChord(chordRow);
                else
                    PlayNote(chordRow, chordCol);
            }
        }

        private void BtCopy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SendNote(string parameter, Note note, int track)
        {
            foreach (IParameter par in gcbm.host.Machine.ParameterGroups[2].Parameters)
                if (par.Name == parameter)
                {
                    par.SetValue(track, note.Value);
                    if (par.Group.Machine.DLL.Info.Version >= 42)
                        par.Group.Machine.SendControlChanges();
                    break;
                }
        }

        private void PlayNote(int chordRow, int noteNumber)
        {
            string[] chords = GuitarNotes.Chords[chordRow].Split(',');

            //int newTranspose = (int)nudTranspose.Value - gcbm.Transpose;

            foreach (IParameter par in gcbm.host.Machine.ParameterGroups[1].Parameters)
                if (par.Name == "Transpose")
                {
                    par.SetValue(0, (int)nudTranspose.Value + 12);
                    if (par.Group.Machine.DLL.Info.Version >= 42)
                        par.Group.Machine.SendControlChanges();
                    break;
                }

            string strNote = chords[noteNumber];

            switch (noteNumber)
            {
                case 1:
                    Note note = GuitarNotes.Transpose(strNote, 0);
                    SendNote("String E", note, 0);
                    previousNotes[0] = note;
                    break;
                case 2:
                    note = GuitarNotes.Transpose(strNote, 0);
                    SendNote("String A", GuitarNotes.Transpose(strNote, 0), 0);
                    previousNotes[1] = note;
                    break;
                case 3:
                    note = GuitarNotes.Transpose(strNote, 0);
                    SendNote("String D", note, 0);
                    previousNotes[2] = note;
                    break;
                case 4:
                    note = GuitarNotes.Transpose(strNote, 0);
                    SendNote("String G", note, 0);
                    previousNotes[3] = note;
                    break;
                case 5:
                    note = GuitarNotes.Transpose(strNote, 0);
                    SendNote("String B", note, 0);
                    previousNotes[4] = note;
                    break;
                case 6:
                    note = GuitarNotes.Transpose(strNote, 0);
                    SendNote("String E2", note, 0);
                    previousNotes[5] = note;
                    break;
            }
        }

        private void PlayChord(int chordRow)
        {
            string[] chords = GuitarNotes.Chords[chordRow].Split(',');

            //int newTranspose = (int)nudTranspose.Value - gcbm.Transpose;

            foreach (IParameter par in gcbm.host.Machine.ParameterGroups[1].Parameters)
                if (par.Name == "Transpose")
                {
                    par.SetValue(0, (int)nudTranspose.Value + 12);
                    if (par.Group.Machine.DLL.Info.Version >= 42)
                        par.Group.Machine.SendControlChanges();
                    break;
                }

            string strNote = chords[1];
            // gcbm.TrackParamStringE(GuitarNotes.Transpose(strNote, (int)nudTranspose.Value), 0);
            // gcbm.TrackParamStringE(GuitarNotes.Transpose(strNote, 0), 0);
            Note note = GuitarNotes.Transpose(strNote, 0);
            SendNote("String E", note, 0);
            previousNotes[0] = note;

            strNote = chords[2];
            // gcbm.TrackParamStringA(GuitarNotes.Transpose(strNote, (int)nudTranspose.Value), 0);
            // gcbm.TrackParamStringA(GuitarNotes.Transpose(strNote, 0), 0);
            note = GuitarNotes.Transpose(strNote, 0);
            SendNote("String A", note, 0);
            previousNotes[1] = note;

            strNote = chords[3];
            // gcbm.TrackParamStringD(GuitarNotes.Transpose(strNote, (int)nudTranspose.Value), 0);
            // gcbm.TrackParamStringD(GuitarNotes.Transpose(strNote, 0), 0);
            note = GuitarNotes.Transpose(strNote, 0);
            SendNote("String D", note, 0);
            previousNotes[2] = note;

            strNote = chords[4];
            // gcbm.TrackParamStringG(GuitarNotes.Transpose(strNote, (int)nudTranspose.Value), 0);
            // gcbm.TrackParamStringG(GuitarNotes.Transpose(strNote, 0), 0);
            note = GuitarNotes.Transpose(strNote, 0);
            SendNote("String G", note, 0);
            previousNotes[3] = note;

            strNote = chords[5];
            // gcbm.TrackParamStringB(GuitarNotes.Transpose(strNote, (int)nudTranspose.Value), 0);
            // gcbm.TrackParamStringB(GuitarNotes.Transpose(strNote, 0), 0);
            note = GuitarNotes.Transpose(strNote, 0);
            SendNote("String B", note, 0);
            previousNotes[4] = note;

            strNote = chords[6];
            // gcbm.TrackParamStringE2(GuitarNotes.Transpose(strNote, (int)nudTranspose.Value), 0);
            // gcbm.TrackParamStringE2(GuitarNotes.Transpose(strNote, 0), 0);
            note = GuitarNotes.Transpose(strNote, 0);
            SendNote("String E2", note, 0);
            previousNotes[5] = note;
        }

    }

}
