using BuzzGUI.Common;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WDE.GuitarChordBox
{
    public class GuitarChordBoxCanvas : Canvas
    {
        public static double MARGIN_LEFT_PERCENT = 0.1;
        public static double MARGIN_RIGHT_PERCENT = 0.20;
        public static double MARGIN_TOP_PERCENT = 0.25;
        public static double MARGIN_BOTTOM_PERCENT = 0.10;

        public static int NUMBER_OF_STRINGS = 6;
        public static int NUMBER_OF_FRETS = 5;

        public static double NUT_HEIGHT = 20;

        public static double OPEN_OR_CLOSED_STRING_HEIGHT = 50;
        public static double CHORD_NAME_HEIGHT = 100;

        public static double TOP_FONT_SIZE = 35;
        public static double BOTTOM_FONT_SIZE = 30;
        public static double NOTE_FONT_SIZE = 25;

        public static double BOTTOM_NOTE_MARGIN = 20;

        public static double NOTE_CIRCLE_SIZE = 45;
        public static double NOTE_CIRCLE_STROKE_THIKNESS = 3;

        private static Brush FILL_BRUSH = Brushes.FloralWhite;
        private static Color FILL_COLOR = Colors.FloralWhite;
        private static Brush STROKE_BRUSH = Brushes.Black;

        private Ellipse[] ellipseStrings;
        private TextBlock[] tbStrings;
        private TextBlock[] tbTopMarks;
        private TextBlock[] tbBottomNotes;
        private TextBlock tbElevateFrets;
        private TextBlock tbChord;

        int[] noteBuffer = new int[NUMBER_OF_STRINGS];

        private double leftMostStringX;
        private double topString;
        private double rightMostStringX;
        private double bottomString;

        private double spaceBetweenStrings;
        private double spaceBetweenFrets;
        private int elevateFrets;
        private GuitarNotes notes;

        public GuitarChordBoxCanvas()
        {
            CreateUIElements();
            for (int i = 0; i < NUMBER_OF_STRINGS; i++)
            {
                noteBuffer[i] = BuzzNote.Off;
            }
        }

        public void SetNotes(GuitarNotes notes)
        {
            this.notes = notes;
            SetNoteForString(0, notes.StringE);
            SetNoteForString(1, notes.StringA);
            SetNoteForString(2, notes.StringD);
            SetNoteForString(3, notes.StringG);
            SetNoteForString(4, notes.StringB);
            SetNoteForString(5, notes.StringE2);

            SetFingerForString(0, notes.StringE, notes.StringE_Finger);
            SetFingerForString(1, notes.StringA, notes.StringA_Finger);
            SetFingerForString(2, notes.StringD, notes.StringD_Finger);
            SetFingerForString(3, notes.StringG, notes.StringG_Finger);
            SetFingerForString(4, notes.StringB, notes.StringB_Finger);
            SetFingerForString(5, notes.StringE2, notes.StringE2_Finger);
            this.Draw();
        }

        public void CreateUIElements()
        {
            ellipseStrings = new Ellipse[NUMBER_OF_STRINGS];
            tbStrings = new TextBlock[NUMBER_OF_STRINGS];
            tbTopMarks = new TextBlock[NUMBER_OF_STRINGS];
            tbBottomNotes = new TextBlock[NUMBER_OF_STRINGS];

            for (int i = 0; i < NUMBER_OF_STRINGS; i++)
            {
                ellipseStrings[i] = new Ellipse() { Width = NOTE_CIRCLE_SIZE, Height = NOTE_CIRCLE_SIZE, Fill = FILL_BRUSH, Stroke = STROKE_BRUSH, StrokeThickness = NOTE_CIRCLE_STROKE_THIKNESS, Opacity = 0 };
                tbStrings[i] = new TextBlock() { Text = "", FontSize = NOTE_FONT_SIZE, Foreground = STROKE_BRUSH, Opacity = 0 };
                tbTopMarks[i] = new TextBlock() { Text = "", FontSize = TOP_FONT_SIZE, Foreground = STROKE_BRUSH, Opacity = 0 };
                tbBottomNotes[i] = new TextBlock() { Text = "", FontSize = BOTTOM_FONT_SIZE, Foreground = STROKE_BRUSH, Opacity = 0 };
            }

            tbChord = new TextBlock() { Text = "", FontSize = TOP_FONT_SIZE, Foreground = STROKE_BRUSH, Opacity = 0 };
            tbElevateFrets = new TextBlock() { Text = "", FontSize = NOTE_FONT_SIZE, Foreground = STROKE_BRUSH, Opacity = 0 };
        }

        public void Draw()
        {
            this.Children.Clear();

            LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush();
            myLinearGradientBrush.StartPoint = new Point(0, 0);
            myLinearGradientBrush.EndPoint = new Point(0, 1);
            myLinearGradientBrush.GradientStops.Add(
                new GradientStop(ChangeColorBrightness(FILL_COLOR, -0.15f), 0.0));
            myLinearGradientBrush.GradientStops.Add(
                new GradientStop(ChangeColorBrightness(FILL_COLOR, 1), 0.10));
            myLinearGradientBrush.GradientStops.Add(
                new GradientStop(ChangeColorBrightness(FILL_COLOR, 1), 0.90));
            myLinearGradientBrush.GradientStops.Add(
                new GradientStop(ChangeColorBrightness(FILL_COLOR, -0.15f), 1.00));

            this.Background = myLinearGradientBrush;

            UpdateDrawingMarks();

            Rectangle mainRect = new Rectangle() { Height = NUT_HEIGHT + bottomString - topString, Width = rightMostStringX - leftMostStringX, Fill = Brushes.Transparent, Stroke = Brushes.Black, StrokeThickness = 2 };
            Canvas.SetLeft(mainRect, leftMostStringX);
            Canvas.SetTop(mainRect, topString - NUT_HEIGHT);
            this.Children.Add(mainRect);

            // Draw strings
            double stringX = leftMostStringX;
            for (int i = 1; i < NUMBER_OF_STRINGS - 1; i++)
            {
                DrawLine(this, stringX + i * spaceBetweenStrings, topString, stringX + i * spaceBetweenStrings, bottomString, Brushes.Black, 2);
            }

            // Draw nut
            Rectangle rectNut = new Rectangle() { Height = NUT_HEIGHT, Width = rightMostStringX - leftMostStringX, Fill = Brushes.Black };
            Canvas.SetLeft(rectNut, leftMostStringX);
            Canvas.SetTop(rectNut, topString - NUT_HEIGHT);
            this.Children.Add(rectNut);

            // Draw frets            
            double fretY = topString;

            for (int i = 1; i < NUMBER_OF_FRETS; i++)
            {
                DrawLine(this, leftMostStringX, topString + i * spaceBetweenFrets, rightMostStringX, topString + i * spaceBetweenFrets, Brushes.Black, 2);
            }

            // Draw top
            stringX = leftMostStringX;

            double noteY = topString + spaceBetweenFrets / 2.0;

            for (int i = 0; i < NUMBER_OF_STRINGS; i++)
            {
                this.Children.Add(ellipseStrings[i]);
                this.Children.Add(tbStrings[i]);
                this.Children.Add(tbTopMarks[i]);
                this.Children.Add(tbBottomNotes[i]);
            }

            this.Children.Add(tbChord);
            this.Children.Add(tbElevateFrets);

            UpdateNotes();
        }

        internal void UpdateDrawingMarks()
        {
            leftMostStringX = this.ActualWidth * MARGIN_LEFT_PERCENT;
            topString = this.ActualHeight * MARGIN_TOP_PERCENT;
            rightMostStringX = this.ActualWidth * (1.0 - MARGIN_RIGHT_PERCENT);
            bottomString = this.ActualHeight * (1.0 - MARGIN_BOTTOM_PERCENT);

            spaceBetweenStrings = (rightMostStringX - leftMostStringX) / (NUMBER_OF_STRINGS - 1.0);
            spaceBetweenFrets = (bottomString - topString) / (double)(NUMBER_OF_FRETS);

            elevateFrets = 0;
            if (notes.HighestFret() > NUMBER_OF_FRETS)
            {
                elevateFrets = notes.LowestFret() == -1 ? 0 : notes.LowestFret();
                elevateFrets = elevateFrets > 1 ? elevateFrets - 1 : 0;
                Global.Buzz.DCWriteLine("Elevate: " + elevateFrets);
            }
        }

        internal void UpdateNotes()
        {
            UpdateDrawingMarks();

            // Update only positions
            for (int i = 0; i < NUMBER_OF_STRINGS; i++)
            {
                double stringX = leftMostStringX + i * spaceBetweenStrings;
                double noteY = topString + spaceBetweenFrets / 2.0;

                UpdateTextCenter(this, tbChord, this.ActualWidth / 2.0, topString - CHORD_NAME_HEIGHT);
                UpdateTextCenter(this, tbTopMarks[i], stringX, topString - OPEN_OR_CLOSED_STRING_HEIGHT);
                UpdateTextCenter(this, tbBottomNotes[i], stringX, bottomString + BOTTOM_NOTE_MARGIN);

                if (this.noteBuffer[i] != BuzzNote.Off &&
                this.noteBuffer[i] != GuitarNotes.STRING_DEFAULT_VALS[i])
                {
                    int noteNum = BuzzNote.ToMIDINote(noteBuffer[i]) - BuzzNote.ToMIDINote(GuitarNotes.STRING_DEFAULT_VALS[i]) - elevateFrets;
                    if (noteNum <= NUMBER_OF_FRETS)
                    {
                        UpdateNoteCircle(this, ellipseStrings[i], stringX, noteY + (noteNum - 1.0) * spaceBetweenFrets);
                        UpdateTextCenter(this, tbStrings[i], stringX, noteY + (noteNum - 1.0) * spaceBetweenFrets);
                    }
                }
            }
            UpdateElevateFretText();
        }

        internal void SetFingerForString(int stringNumber, int stringNote, int fingerNumber)
        {
            UpdateDrawingMarks();

            double stringX = leftMostStringX + stringNumber * spaceBetweenStrings;
            double noteY = topString + spaceBetweenFrets / 2.0;

            StopAnimation(tbStrings[stringNumber]);
            tbStrings[stringNumber].Opacity = 0;

            if (stringNote != BuzzNote.Off &&
                stringNote != GuitarNotes.STRING_DEFAULT_VALS[stringNumber])
            {
                int noteNum = BuzzNote.ToMIDINote(stringNote) - BuzzNote.ToMIDINote(GuitarNotes.STRING_DEFAULT_VALS[stringNumber]) - elevateFrets;

                if (noteNum <= NUMBER_OF_FRETS)
                {
                    tbStrings[stringNumber].Text = fingerNumber > 0 ? "" + fingerNumber : "";
                    UpdateTextCenterFade(this, tbStrings[stringNumber], stringX, noteY + (noteNum - 1.0) * spaceBetweenFrets);
                }
            }
        }

        internal void SetNoteForString(int stringNumber, int stringNote)
        {
            noteBuffer[stringNumber] = stringNote;
            UpdateDrawingMarks();

            double stringX = leftMostStringX + stringNumber * spaceBetweenStrings;
            double noteY = topString + spaceBetweenFrets / 2.0;

            // Stop animation
            StopAnimation(tbStrings[stringNumber]);
            StopAnimation(tbTopMarks[stringNumber]);
            //StopAnimation(tbBottomNotes[stringNumber]);
            StopAnimation(ellipseStrings[stringNumber]);

            tbChord.Opacity = 0;
            tbChord.Text = notes.GetChord();
            UpdateTextCenterFade(this, tbChord, this.ActualWidth / 2.0, topString - CHORD_NAME_HEIGHT);

            //tbStrings[stringNumber].Text = "";
            tbStrings[stringNumber].Opacity = 0;
            tbTopMarks[stringNumber].Opacity = 0;
            tbBottomNotes[stringNumber].Text = "";
            tbBottomNotes[stringNumber].Opacity = 0;
            ellipseStrings[stringNumber].Opacity = 0;

            if (stringNote == GuitarNotes.STRING_DEFAULT_VALS[stringNumber])
            {
                tbTopMarks[stringNumber].Text = "O";
                UpdateTextCenterFade(this, tbTopMarks[stringNumber], stringX, topString - OPEN_OR_CLOSED_STRING_HEIGHT);

                tbBottomNotes[stringNumber].Text = ParseNote(BuzzNote.ToString(stringNote));
                UpdateTextCenterFade(this, tbBottomNotes[stringNumber], stringX, bottomString + BOTTOM_NOTE_MARGIN);
            }
            else if (stringNote == BuzzNote.Off)
            {
                tbTopMarks[stringNumber].Text = "X";
                UpdateTextCenterFade(this, tbTopMarks[stringNumber], stringX, topString - OPEN_OR_CLOSED_STRING_HEIGHT);
            }
            else
            {
                int noteNum = BuzzNote.ToMIDINote(stringNote) - BuzzNote.ToMIDINote(GuitarNotes.STRING_DEFAULT_VALS[stringNumber]) - elevateFrets;
                if (noteNum <= NUMBER_OF_FRETS)
                {
                    UpdateNoteCircleFade(this, ellipseStrings[stringNumber], stringX, noteY + (noteNum - 1.0) * spaceBetweenFrets);
                    UpdateTextCenterFade(this, tbStrings[stringNumber], stringX, noteY + (noteNum - 1.0) * spaceBetweenFrets);
                }

                tbBottomNotes[stringNumber].Text = ParseNote(BuzzNote.ToString(stringNote));
                UpdateTextCenterFade(this, tbBottomNotes[stringNumber], stringX, bottomString + BOTTOM_NOTE_MARGIN);
            }

            if (elevateFrets >= 0)
            {
                UpdateNotes(); // Reposition all note positions
            }
            else
            {
                StopAnimation(tbElevateFrets);
                tbElevateFrets.Opacity = 0;
            }

            this.InvalidateVisual();
        }

        public void UpdateElevateFretText()
        {
            if (elevateFrets > 0)
            {
                string fretStr = "";
                if (elevateFrets == 1)
                    fretStr = "2nd";
                else if (elevateFrets == 2)
                    fretStr = "3rd";
                else
                    fretStr = elevateFrets + 1 + "th";

                tbElevateFrets.Text = fretStr;
                UpdateTextCenterFade(this, tbElevateFrets, rightMostStringX + spaceBetweenStrings * 0.9, topString + spaceBetweenFrets / 2.0);
                //UpdateNotes(); // Reposition all note positions
            }
            else
            {
                StopAnimation(tbElevateFrets);
                tbElevateFrets.Opacity = 0;
            }
        }

        public static string ParseNote(string note)
        {
            string ret = note;
            if (ret.Contains("-"))
                ret = ret.Substring(0, 1);
            if (ret.Contains("#"))
                ret = ret.Substring(0, 2);

            return ret;
        }
        public static void StopAnimation(UIElement uie)
        {
            DoubleAnimation fadeInAnimation = new DoubleAnimation();
            fadeInAnimation.BeginTime = null;
            uie.BeginAnimation(UIElement.OpacityProperty, null);
        }

        public static void FadeIn(UIElement uie)
        {
            var fadeInAnimation = new DoubleAnimation(1d, new TimeSpan(0, 0, 0, 0, 200));
            uie.Opacity = 0;
            uie.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
        }

        public static void DrawNoteCircleFade(Canvas canvas, double X1, double Y1, double circleSize, string text, Brush brush, Brush brushBg, double strokeSize, double fontSize)
        {
            Ellipse ellipse = new Ellipse() { Width = circleSize, Height = circleSize, Fill = brushBg, Stroke = brush, StrokeThickness = strokeSize };
            Canvas.SetLeft(ellipse, X1 - circleSize / 2.0);
            Canvas.SetTop(ellipse, Y1 - circleSize / 2.0);
            canvas.Children.Add(ellipse);
            DrawTextCenterFade(canvas, X1, Y1, text, brush, fontSize);
            FadeIn(ellipse);
        }

        public static void UpdateNoteCircleFade(Canvas canvas, Ellipse ellipse, double X1, double Y1)
        {
            UpdateNoteCircle(canvas, ellipse, X1, Y1);
            FadeIn(ellipse);
        }

        public static void UpdateNoteCircle(Canvas canvas, Ellipse ellipse, double X1, double Y1)
        {
            Canvas.SetLeft(ellipse, X1 - ellipse.Width / 2.0);
            Canvas.SetTop(ellipse, Y1 - ellipse.Height / 2.0);
        }

        public static void DrawTextCenterFade(Canvas canvas, double X1, double Y1, string text, Brush brush, double fontSize)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.Foreground = brush;
            textBlock.FontSize = fontSize;

            textBlock.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            textBlock.Arrange(new Rect(textBlock.DesiredSize));

            Canvas.SetLeft(textBlock, X1 - textBlock.ActualWidth / 2.0);
            Canvas.SetTop(textBlock, Y1 - textBlock.ActualHeight / 2.0);
            canvas.Children.Add(textBlock);
            FadeIn(textBlock);
        }

        public static void UpdateTextCenterFade(Canvas canvas, TextBlock tb, double X1, double Y1)
        {
            UpdateTextCenter(canvas, tb, X1, Y1);
            FadeIn(tb);
        }

        public static void UpdateTextCenter(Canvas canvas, TextBlock tb, double X1, double Y1)
        {
            tb.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            tb.Arrange(new Rect(tb.DesiredSize));

            Canvas.SetLeft(tb, X1 - tb.ActualWidth / 2.0);
            Canvas.SetTop(tb, Y1 - tb.ActualHeight / 2.0);
        }

        public static void DrawTextCenter(Canvas canvas, double X1, double Y1, string text, Brush brush, double fontSize)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.Foreground = brush;
            textBlock.FontSize = fontSize;

            textBlock.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            textBlock.Arrange(new Rect(textBlock.DesiredSize));

            Canvas.SetLeft(textBlock, X1 - textBlock.ActualWidth / 2.0);
            Canvas.SetTop(textBlock, Y1 - textBlock.ActualHeight / 2.0);
            canvas.Children.Add(textBlock);
        }

        public static void DrawLine(Canvas canvas, double X1, double Y1, double X2, double Y2, Brush brush, double strokeWidth)
        {
            Line myLine = new Line();

            myLine.Stroke = brush;
            myLine.SnapsToDevicePixels = true;
            myLine.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            myLine.StrokeThickness = strokeWidth;

            myLine.X1 = X1;
            myLine.Y1 = Y1;
            myLine.X2 = X2;
            myLine.Y2 = Y2;

            canvas.Children.Add(myLine);
        }

        public static Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            red = red < 0 ? 0 : red;
            green = green < 0 ? 0 : green;
            blue = blue < 0 ? 0 : blue;

            red = red > 255 ? 255 : red;
            green = green > 255 ? 255 : green;
            blue = blue > 255 ? 255 : blue;

            return Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);
        }

    }
}
