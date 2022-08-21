using Buzz.MachineInterface;
using BuzzGUI.Common;
using System;

namespace WDE.GuitarChordBox
{
    public class GuitarNotes
    {
        public static int LAST_FRET_IN_STRING = 17;

        public static int STRING_E_DEFAULT_VAL = BuzzNote.Parse("E-3");
        public static int STRING_A_DEFAULT_VAL = BuzzNote.Parse("A-3");
        public static int STRING_D_DEFAULT_VAL = BuzzNote.Parse("D-4");
        public static int STRING_G_DEFAULT_VAL = BuzzNote.Parse("G-4");
        public static int STRING_B_DEFAULT_VAL = BuzzNote.Parse("B-4");
        public static int STRING_E2_DEFAULT_VAL = BuzzNote.Parse("E-5");

        public static int[] STRING_DEFAULT_VALS = { STRING_E_DEFAULT_VAL, STRING_A_DEFAULT_VAL, STRING_D_DEFAULT_VAL, STRING_G_DEFAULT_VAL, STRING_B_DEFAULT_VAL, STRING_E2_DEFAULT_VAL };

        private int stringE = STRING_E_DEFAULT_VAL;
        private int stringA = STRING_A_DEFAULT_VAL;
        private int stringD = STRING_D_DEFAULT_VAL;
        private int stringG = STRING_G_DEFAULT_VAL;
        private int stringB = STRING_B_DEFAULT_VAL;
        private int stringE2 = STRING_E2_DEFAULT_VAL;

        private int stringE_Finger = 0;
        private int stringA_Finger = 0;
        private int stringD_Finger = 0;
        private int stringG_Finger = 0;
        private int stringB_Finger = 0;
        private int stringE2_Finger = 0;

        public int StringE_Finger
        {
            get { return stringE_Finger; }
            set
            {
                stringE_Finger = value > 4 ? 0 : value;
            }
        }
        public int StringA_Finger
        {
            get { return stringA_Finger; }
            set
            {
                stringA_Finger = value > 4 ? 0 : value;
            }
        }
        public int StringD_Finger
        {
            get { return stringD_Finger; }
            set
            {
                stringD_Finger = value > 4 ? 0 : value;
            }
        }
        public int StringG_Finger
        {
            get { return stringG_Finger; }
            set
            {
                stringG_Finger = value > 4 ? 0 : value;
            }
        }
        public int StringB_Finger
        {
            get { return stringB_Finger; }
            set
            {
                stringB_Finger = value > 4 ? 0 : value;
            }
        }
        public int StringE2_Finger
        {
            get { return stringE2_Finger; }
            set
            {
                stringE2_Finger = value > 4 ? 0 : value;
            }
        }


        public int StringE
        {
            get { return stringE; }
            set
            {
                if (value < STRING_E_DEFAULT_VAL || value == BuzzNote.Off)
                    stringE = BuzzNote.Off;
                else if (BuzzNote.ToMIDINote(value) > BuzzNote.ToMIDINote(STRING_E_DEFAULT_VAL) + LAST_FRET_IN_STRING)
                    stringE = BuzzNote.Off;
                else
                    stringE = value;

                if (stringE == STRING_E_DEFAULT_VAL)
                    StringE_Finger = 0;
            }
        }

        public int StringA
        {
            get { return stringA; }
            set
            {
                if (value < STRING_A_DEFAULT_VAL || value == BuzzNote.Off)
                    stringA = BuzzNote.Off;
                else if (BuzzNote.ToMIDINote(value) > BuzzNote.ToMIDINote(STRING_A_DEFAULT_VAL) + LAST_FRET_IN_STRING)
                    stringA = BuzzNote.Off;
                else
                    stringA = value;

                if (stringA == STRING_A_DEFAULT_VAL)
                    StringA_Finger = 0;
            }
        }

        public int StringD
        {
            get { return stringD; }
            set
            {
                if (value < STRING_D_DEFAULT_VAL || value == BuzzNote.Off)
                    stringD = BuzzNote.Off;
                else if (BuzzNote.ToMIDINote(value) > BuzzNote.ToMIDINote(STRING_D_DEFAULT_VAL) + LAST_FRET_IN_STRING)
                    stringD = BuzzNote.Off;
                else
                    stringD = value;

                if (stringD == STRING_D_DEFAULT_VAL)
                    StringD_Finger = 0;
            }
        }

        public int StringG
        {
            get { return stringG; }
            set
            {
                if (value < STRING_G_DEFAULT_VAL || value == BuzzNote.Off)
                    stringG = BuzzNote.Off;
                else if (BuzzNote.ToMIDINote(value) > BuzzNote.ToMIDINote(STRING_G_DEFAULT_VAL) + LAST_FRET_IN_STRING)
                    stringG = BuzzNote.Off;
                else
                    stringG = value;

                if (stringG == STRING_G_DEFAULT_VAL)
                    StringG_Finger = 0;
            }
        }

        public int StringB
        {
            get { return stringB; }
            set
            {
                if (value < STRING_B_DEFAULT_VAL || value == BuzzNote.Off)
                    stringB = BuzzNote.Off;
                else if (BuzzNote.ToMIDINote(value) > BuzzNote.ToMIDINote(STRING_B_DEFAULT_VAL) + LAST_FRET_IN_STRING)
                    stringB = BuzzNote.Off;
                else
                    stringB = value;

                if (stringB == STRING_B_DEFAULT_VAL)
                    StringB_Finger = 0;
            }
        }

        public int TransposeAmount { get; internal set; }

        public int StringE2
        {
            get { return stringE2; }
            set
            {
                if (value < STRING_E2_DEFAULT_VAL || value == BuzzNote.Off)
                    stringE2 = BuzzNote.Off;
                else if (BuzzNote.ToMIDINote(value) > BuzzNote.ToMIDINote(STRING_E2_DEFAULT_VAL) + LAST_FRET_IN_STRING)
                    stringE2 = BuzzNote.Off;
                else
                    stringE2 = value;

                if (stringE2 == STRING_E2_DEFAULT_VAL)
                    StringE2_Finger = 0;
            }
        }

        public GuitarNotes()
        {
            ClearNotes();
        }

        public void ClearNotes()
        {
            /*
            StringE = STRING_E_DEFAULT_VAL;
            StringA = STRING_A_DEFAULT_VAL;
            StringD = STRING_D_DEFAULT_VAL;
            StringG = STRING_G_DEFAULT_VAL;
            StringB = STRING_B_DEFAULT_VAL;
            StringE2 = STRING_E2_DEFAULT_VAL;
            */

            StringE = BuzzNote.Off;
            StringA = BuzzNote.Off;
            StringD = BuzzNote.Off;
            StringG = BuzzNote.Off;
            StringB = BuzzNote.Off;
            StringE2 = BuzzNote.Off;

            StringE_Finger = 0;
            StringA_Finger = 0;
            StringD_Finger = 0;
            StringG_Finger = 0;
            StringB_Finger = 0;
            StringE2_Finger = 0;
        }

        public void Copy(GuitarNotes gn)
        {
            StringE = gn.stringE;
            StringA = gn.StringA;
            StringD = gn.StringD;
            StringG = gn.StringG;
            StringB = gn.StringB;
            StringE2 = gn.StringE2;

            StringE_Finger = gn.StringE_Finger;
            StringA_Finger = gn.StringA_Finger;
            StringD_Finger = gn.StringD_Finger;
            StringG_Finger = gn.StringG_Finger;
            StringB_Finger = gn.StringB_Finger;
            StringE2_Finger = gn.StringE2_Finger;
        }

        private bool StringNotes(int noteE, int noteA, int noteD, int noteG, int noteB, int noteE2)
        {
            return (StringE == noteE) &&
                (StringA == noteA) &&
                (StringD == noteD) &&
                (StringG == noteG) &&
                (StringB == noteB) &&
                (StringE2 == noteE2);
        }

        private bool StringNotesTransposed(int noteE, int noteA, int noteD, int noteG, int noteB, int noteE2)
        {
            int tmpE = StringE == BuzzNote.Off ? BuzzNote.Off : BuzzNote.FromMIDINote(BuzzNote.ToMIDINote(StringE) - TransposeAmount);
            int tmpA = StringA == BuzzNote.Off ? BuzzNote.Off : BuzzNote.FromMIDINote(BuzzNote.ToMIDINote(StringA) - TransposeAmount);
            int tmpD = StringD == BuzzNote.Off ? BuzzNote.Off : BuzzNote.FromMIDINote(BuzzNote.ToMIDINote(StringD) - TransposeAmount);
            int tmpG = StringG == BuzzNote.Off ? BuzzNote.Off : BuzzNote.FromMIDINote(BuzzNote.ToMIDINote(StringG) - TransposeAmount);
            int tmpB = StringB == BuzzNote.Off ? BuzzNote.Off : BuzzNote.FromMIDINote(BuzzNote.ToMIDINote(StringB) - TransposeAmount);
            int tmpE2 = StringE2 == BuzzNote.Off ? BuzzNote.Off : BuzzNote.FromMIDINote(BuzzNote.ToMIDINote(StringE2) - TransposeAmount);

            return (tmpE == noteE) &&
                (tmpA == noteA) &&
                (tmpD == noteD) &&
                (tmpG == noteG) &&
                (tmpB == noteB) &&
                (tmpE2 == noteE2);
        }

        public int GetStringEPos()
        {
            int ret = -1;
            if (stringE != BuzzNote.Off)
                ret = BuzzNote.ToMIDINote(stringE) - BuzzNote.ToMIDINote(STRING_E_DEFAULT_VAL);

            return ret;
        }

        public int GetStringAPos()
        {
            int ret = -1;
            if (stringA != BuzzNote.Off)
                ret = BuzzNote.ToMIDINote(stringA) - BuzzNote.ToMIDINote(STRING_A_DEFAULT_VAL);

            return ret;
        }

        public int GetStringDPos()
        {
            int ret = -1;
            if (stringD != BuzzNote.Off)
                ret = BuzzNote.ToMIDINote(stringD) - BuzzNote.ToMIDINote(STRING_D_DEFAULT_VAL);

            return ret;
        }

        public int GetStringGPos()
        {
            int ret = -1;
            if (stringG != BuzzNote.Off)
                ret = BuzzNote.ToMIDINote(stringG) - BuzzNote.ToMIDINote(STRING_G_DEFAULT_VAL);

            return ret;
        }

        public int GetStringBPos()
        {
            int ret = -1;
            if (stringB != BuzzNote.Off)
                ret = BuzzNote.ToMIDINote(stringB) - BuzzNote.ToMIDINote(STRING_B_DEFAULT_VAL);

            return ret;
        }

        public int GetStringE2Pos()
        {
            int ret = -1;
            if (stringE2 != BuzzNote.Off)
                ret = BuzzNote.ToMIDINote(stringE2) - BuzzNote.ToMIDINote(STRING_E2_DEFAULT_VAL);

            return ret;
        }

        public int LowestFret()
        {
            int ret = int.MaxValue;

            if (GetStringEPos() != -1 && StringE != GuitarNotes.STRING_E_DEFAULT_VAL && GetStringEPos() < ret)
                ret = GetStringEPos();

            if (GetStringAPos() != -1 && StringA != GuitarNotes.STRING_A_DEFAULT_VAL && GetStringAPos() < ret)
                ret = GetStringAPos();

            if (GetStringDPos() != -1 && StringD != GuitarNotes.STRING_D_DEFAULT_VAL && GetStringDPos() < ret)
                ret = GetStringDPos();

            if (GetStringGPos() != -1 && StringG != GuitarNotes.STRING_G_DEFAULT_VAL && GetStringGPos() < ret)
                ret = GetStringGPos();

            if (GetStringBPos() != -1 && StringB != GuitarNotes.STRING_B_DEFAULT_VAL && GetStringBPos() < ret)
                ret = GetStringBPos();

            if (GetStringE2Pos() != -1 && StringE2 != GuitarNotes.STRING_E2_DEFAULT_VAL && GetStringE2Pos() < ret)
                ret = GetStringE2Pos();

            if (ret == int.MaxValue)
                ret = -1;

            return ret;
        }

        public int HighestFret()
        {
            int ret = 0;

            if (GetStringEPos() != -1 && StringE != GuitarNotes.STRING_E_DEFAULT_VAL && GetStringEPos() > ret)
                ret = GetStringEPos();

            if (GetStringAPos() != -1 && StringA != GuitarNotes.STRING_A_DEFAULT_VAL && GetStringAPos() > ret)
                ret = GetStringAPos();

            if (GetStringDPos() != -1 && StringD != GuitarNotes.STRING_D_DEFAULT_VAL && GetStringDPos() > ret)
                ret = GetStringDPos();

            if (GetStringGPos() != -1 && StringG != GuitarNotes.STRING_G_DEFAULT_VAL && GetStringGPos() > ret)
                ret = GetStringGPos();

            if (GetStringBPos() != -1 && StringB != GuitarNotes.STRING_B_DEFAULT_VAL && GetStringBPos() > ret)
                ret = GetStringBPos();

            if (GetStringE2Pos() != -1 && StringE2 != GuitarNotes.STRING_E2_DEFAULT_VAL && GetStringE2Pos() > ret)
                ret = GetStringE2Pos();

            if (ret == 0)
                ret = -1;

            return ret;
        }

        public Tuple<int, int> Finger1FirstLastString()
        {
            int first = -1;
            int last = -1;

            if (StringE_Finger == 1)
                first = 0;
            else if (StringA_Finger == 1)
                first = 1;
            else if (StringD_Finger == 1)
                first = 2;
            else if (StringG_Finger == 1)
                first = 3;
            else if (StringB_Finger == 1)
                first = 4;
            else if (StringE2_Finger == 1)
                first = 5;

            if (StringE2_Finger == 1)
                last = 5;
            else if (StringB_Finger == 1)
                last = 4;
            else if (StringG_Finger == 1)
                last = 3;
            else if (StringD_Finger == 1)
                last = 2;
            else if (StringA_Finger == 1)
                last = 1;
            else if (StringE_Finger == 1)
                last = 0;

            return new Tuple<int, int>(first, last);
        }


        public static int N(string note)
        {
            if (note == "-")
                return BuzzNote.Off;
            else
                return BuzzNote.Parse(note);
        }

        private int Off()
        {
            return BuzzNote.Off;
        }

        public static string[] Chords =
        {
"C,-,C-4,E-4,G-4,C-5,E-5",
"C,-,C-4,E-4,G-4,C-5,-",
"Cmaj7,-,C-4,E-4,G-4,B-4,-",
"Cmaj7,-,C-4,E-4,G-4,B-4,E-5",
"CM7/9,-,C-4,E-4,B-4,D-5,-",
"C6/9,-,C-4,E-4,A-4,D-5,-",
"C6/9,-,C-4,E-4,A-4,D-5,G-5",
"Cadd9,-,C-4,E-4,G-4,D-5,-",
"Cadd9,-,C-4,E-4,G-4,D-5,G-5",
"C6maj7,-,C-4,E-4,A-4,B-4,-",
"C6,-,C-4,E-4,A-4,C-5,-",
"Cm,-,C-4,-,G-4,D#5,G-5",
"Cm,-,C-4,G-4,C-5,D#5,G-5",
"Cm7,-,C-4,G-4,A#4,D#5,G-5",
"Cm7sus4,-,C-4,F-4,A#4,D#5,G-5",
"Cm7b5,-,C-4,F#4,A#4,D#5,-",
"Cm7M,-,C-4,D#4,G-4,B-4,-",
"Cm9,-,C-4,D#4,A#4,D-5,-",
"Cmadd9,-,C-4,D#4,G-4,D-5,-",
"Cm6,-,C-4,-,A-4,D#5,G-5",
"C9,-,C-4,E-4,A#4,D-5,-",
"C7,-,C-4,E-4,A#4,C-5,E-5",
"C7,-,C-4,E-4,A#4,C-5,-",
"C7#9,-,C-4,E-4,A#4,D#5,-",
"C7b9,-,C-4,E-4,A#4,C#5,-",
"C7b9/13,-,C-4,E-4,A#4,C#5,A-5",
"C7b9/13b,-,C-4,E-4,A#4,C#5,G#5",
"C9sus4,-,C-4,F-4,A#4,D-5,-",
"C11,-,C-4,-,A#4,C-5,F-5",
"Caug,-,C-4,E-4,G#4,C-5,-",
"Cmaj7#5,-,C-4,E-4,G#4,B-4,-",
"Cdim,-,C-4,F#4,A-4,D#5,-",
// Inversions
"C,G-3,C-4,E-4,G-4,C-5,E-5",
"C,E-3,-,-,G-4,C-5,E-5",
"Cmaj7,G-3,C-4,E-4,G-4,B-4,E-5",
"Cmaj7,-,-,E-4,B-4,C-5,G-5",
"C6/9,-,-,E-4,A-4,D-5,G-5",
"Cm7,G-3,-,D#4,A#4,C-5,-",
"C7,-,A#3,-,G-4,C-5,E-5",
"C7,-,-,E-4,A#4,C-5,G-5",

// G
"G,G-3,B-3,D-4,G-4,B-4,G-5",
"G,G-3,B-3,D-4,G-4,D-5,G-5",
"G,G-3,D-4,G-4,B-4,D-5,G-5",
"G6,G-3,B-3,D-4,G-4,B-4,E-5",
"G6,G-3,-,E-4,B-4,D-5,-",
"Gmaj7,G-3,-,F#4,B-4,D-5,-",
"Gmaj7,G-3,-,-,B-4,D-5,F#5",
"Gmaj7,G-3,-,D-4,A-4,B-4,F#5",
"Gmaj7,-,-,G-4,B-4,D-5,F#5",
"Gmaj7#11,G-3,-,-,B-4,C#5,F#5",
"Gm,G-3,D-4,G-4,A#4,D-5,G-5",
"Gm7,G-3,F-4,G-4,A#4,D-5,G-5",
"Gm6,G-3,-,E-4,A#4,D-5,-",
"Gm11,G-3,-,F-4,A#4,C-5,-",
"Gm7M,G-3,-,F#4,A#4,D-5,-",
"G7,G-3,B-3,D-4,G-4,B-4,F-5",
"G7,G-3,-,F-4,G-4,B-4,-",
"Gadd9,G-3,-,D-4,A-4,B-4,-",
"G9,G-3,-,F-4,A-4,B-4,-",
"G7b9,G-3,-,F-4,G#4,B-4,-",
"G7b9/11,G-3,-,F-4,G#4,C-5,-",
"G7/9/11,G-3,-,F-4,A-4,C-5,-",
"G7dim,G-3,-,E-4,A#4,C#5,-",
// inversions
"G,-,B-3,-,G-4,D-5,G-5",
"Gm,-,A#3,-,G-4,D-5,G-5",
"Gm,-,A#3,D-4,G-4,D-5,-",
"G7,-,B-3,-,G-4,D-5,F-5",
"G9,-,-,F-4,A-4,B-4,G-5",
// F
"F,F-3,C-4,F-4,A-4,C-5,F-5",
"Fsus4,F-3,C-4,F-4,A#4,C-5,F-5",
"Fmaj7b5,F-3,C-4,F-4,A-4,B-4,E-5",
"Fmaj7,-,-,F-4,A-4,C-5,E-5",
"Fmaj7,F-3,-,E-4,A-4,C-5,-",
"Fmaj7b5,F-3,-,E-4,A-4,B-4,-",
"Fmaj7#5,F-3,-,E-4,A-4,C#5,-",
"F6,F-3,-,D-4,A-4,C-5,-",
"Fm,F-3,C-4,F-4,G#4,C-5,F-5",
"Fm6,F-3,-,D-4,G#4,C-5,-",
"Fm7,F-3,-,D#4,G#4,C-5,-",
"Fm7M,F-3,-,E-4,G#4,C-5,-",
"Fm9,F-3,C-4,D#4,G#4,C-5,G-5",
"F7,F-3,-,D#4,A-4,C-5,-",
"F7,F-3,C-4,D#4,A-4,C-5,F-5",
"F7#5,F-3,-,D#4,A-4,C#5,-",
"F7b5,F-3,-,D#4,A-4,B-4,-",
"F7#9,F-3,-,D#4,A-4,D#5,G#5",
"F7sus4,F-3,C-4,D#4,A#4,C-5,F-5",
"F13,F-3,-,D#4,A-4,D-5,-",
"F7/4/13,F-3,-,D#4,A#4,D-5,-",
"Fdim,F-3,-,D-4,G#4,B-4,-",
"Fdim,-,-,F-4,B-4,D-5,G#5",
"Faug,-,-,F-4,A-4,C#5,F-5",
// Inversions-chroma
"F,-,A-3,F-4,A-4,C-5,-",
"F6,-,C-4,-,A-4,D-5,F-5",
"F6/9,-,A-3,D-4,G-4,C-5,-",
"Fm,-,-,F-4,G#4,C-5,F-5",
"Fm7M,-,-,E-4,G#4,C-5,F-5",
"Fm7,-,-,D#4,G#4,C-5,F-5",
"Fm6,-,-,D-4,G#4,C-5,F-5",
// E
"E,E-3,B-3,E-4,G#4,B-4,E-5",
"E,-,-,E-4,B-4,E-5,G#5",
"Eadd9,E-3,B-3,F#4,G#4,B-4,E-5",
"Emaj7,E-3,-,D#4,G#4,B-4,-",
"Emaj7,-,-,E-4,B-4,D#5,G#5",
"Em,E-3,B-3,E-4,G-4,B-4,E-5",
"Em7sus4,E-3,A-3,D-4,G-4,B-4,E-5",
"Esus4,E-3,B-3,E-4,A-4,B-4,E-5",
"Em7,E-3,B-3,D-4,G-4,B-4,E-5",
"Em7,E-3,B-3,E-4,G-4,D-5,G-5",
"Em7,-,-,E-4,B-4,D-5,G-5",
"Em6,E-3,B-3,E-4,G-4,C#5,E-5",
"Em7M,-,-,E-4,G-4,D#5,G-5",
"Em9,E-3,B-3,F#4,G-4,D-5,-",
"Em9,E-3,B-3,D-4,G-4,D-5,F#5",
"E7sus4,E-3,B-3,D-4,A-4,B-4,E-5",
"Em9sus4,E-3,B-3,G-4,A-4,D-5,F#5",
"E7,E-3,B-3,D-4,G#4,B-4,E-5",
"E7,E-3,B-3,E-4,G#4,D-5,E-5",
"E7,-,-,E-4,B-4,D-5,G#5",
"E7/9,-,-,E-4,G#4,D-5,F#5",
"E9,-,-,E-4,G#4,D-5,F#5",
"E13,E-3,B-3,E-4,G#4,C#5,E-5",
"E7b9,-,-,E-4,G#4,D-5,F-5",
"E7#9,E-3,-,-,G#4,D-5,G-5",
"Em7b5,-,-,E-4,A#4,D-5,G-5",
"Edim,-,-,E-4,A#4,C#5,G-5",
// inversions
"E,G#3,-,E-4,B-4,E-5,-",
"Emaj7,G#3,-,E-4,B-4,D#5,-",
"Em6,G-3,-,E-4,B-4,C#5,-",
"E7,G#3,-,E-4,B-4,D-5,-",
"E13,-,-,D-4,G#4,C#5,E-5",
// A
"A,-,A-3,E-4,A-4,C#5,E-5",
"A,-,A-3,E-4,A-4,C#5,-",
"A,A-3,C#4,E-4,A-4,C#5,-",
"Asus4,-,A-3,E-4,A-4,D-5,E-5",
"Amaj7,-,A-3,E-4,G#4,C#5,E-5",
"Amaj7,-,A-3,E-4,A-4,C#5,G#5",
"Aadd9,-,A-3,E-4,B-4,C#5,E-5",
"Amadd9,-,A-3,E-4,B-4,C-5,E-5",
"Am,-,A-3,E-4,A-4,C-5,E-5",
"Am6,A-3,-,F#4,C-5,E-5,-",
"Am6,-,A-3,E-4,A-4,C-5,F#5",
"Am7,-,A-3,E-4,G-4,C-5,E-5",
"AmM7,-,A-3,E-4,G#4,C-5,-",
"Am7#5,-,A-3,F-4,G-4,C-5,-",
"Am7m6,-,A-3,F-4,G-4,C-5,E-5",
"A7,-,A-3,E-4,G-4,C#5,E-5",
"A7sus4,-,A-3,E-4,G-4,D-5,E-5",
"A13,-,A-3,-,G-4,C#5,F#5",
"Ab13,-,A-3,-,G-4,C#5,F-5",
"A9,-,A-3,G-4,B-4,C#5,-",
"A7b9,-,A-3,G-4,A#4,C#5,-",
// inversions
"A,E-3,-,E-4,A-4,C#5,-",
"Am7,G-3,-,E-4,A-4,C-5,-",
"A7,G-3,-,E-4,A-4,C#5,-",
//B
"B,-,B-3,F#4,B-4,D#5,F#5",
"Bmaj7,-,B-3,F#4,A#4,D#5,F#5",
"B6,-,B-3,F#4,G#4,D#5,-",
"Bm,-,B-3,F#4,B-4,D-5,F#5",
"Bm6,-,B-3,-,G#4,D-5,F#5",
"Bm7,-,B-3,F#4,A-4,D-5,F#5",
"Bm7b5,-,B-3,F-4,A-4,D-5,-",
"Bm7b5,-,B-3,-,A-4,D-5,F-5",
"Bm9,-,B-3,D-4,A-4,C#5,-",
"B7,-,B-3,D#4,A-4,B-4,F#5",
"B7,-,B-3,F#4,A-4,D#5,F#5",
"B9,-,B-3,D#4,A-4,C#5,-",
"B7/4/9,-,B-3,E-4,A-4,C#5,-",
"B7/9/11,-,B-3,-,A-4,C#5,E-5",
"B7b9,-,B-3,D#4,A-4,C-5,F#5",
"B7b9,-,B-3,D#4,A-4,C-5,-",
"B7#9,-,B-3,D#4,A-4,D-5,-",
"B13,-,B-3,-,A-4,D#5,G#5",
"B7/4/13,-,B-3,-,A-4,E-5,G#5",
"Bdim,-,B-3,F-4,G#4,D-5,-",
// Inversions
"B,F#3,-,F#4,B-4,D#5,-",
"Bmaj7,F#3,-,D#4,A#4,B-4,-",
"B7,F#3,-,D#4,A-4,B-4,-",
"B7,-,-,D#4,A-4,B-4,F#5",
// D
"D,-,-,D-4,A-4,D-5,F#5",
"D,-,D-4,F#4,A-4,D-5,F#5",
"Dsus4,-,-,D-4,A-4,D-5,G-5",
"Dmaj7,-,D-4,F#4,A-4,C#5,F#5",
"Dmaj7,-,D-4,F#4,A-4,C#5,-",
"Dmaj7#5,-,D-4,F#4,A#4,C#5,-",
"Dm,-,-,D-4,A-4,D-5,F-5",
"Dm6,-,-,D-4,A-4,B-4,F-5",
"Dm7,-,-,D-4,A-4,C-5,F-5",
"Dm7M,-,-,D-4,A-4,C#5,F-5",
"Dm9,-,D-4,F-4,C-5,E-5,-",
"D7,-,-,D-4,A-4,C-5,F#5",
"D7,-,D-4,F#4,C-5,D-5,-",
"D9,-,D-4,F#4,C-5,E-5,-",
"D7b9,-,D-4,F#4,C-5,D#5,-",
"D7sus4,-,-,D-4,A-4,C-5,G-5",
"D6,-,-,D-4,A-4,B-4,F#5",
// Inversions
"D,F#3,-,D-4,A-4,D-5,-",
"Dm6,F-3,-,D-4,A-4,B-4,-",
"Dm,F-3,-,F-4,A-4,D-5,-",
"Dm,-,A-3,F-4,A-4,D-5,-",
"D7,F#3,-,D-4,A-4,C-5,-",
"D7,-,C-4,-,A-4,D-5,F#5",
"D13,-,C-4,F#4,B-4,D-5,-",
// Eb chroma
"Eb,-,-,D#4,A#4,D#5,G-5",
"Ebmaj7,-,-,D#4,A#4,D-5,G-5",
"Eb7,-,-,D#4,A#4,C#5,G-5",
"Eb6,-,-,D#4,A#4,C-5,G-5",
"Ebm,-,-,D#4,A#4,D#5,F#5",
"Ebm7M,-,-,D#4,A#4,D-5,F#5",
"Ebm7,-,-,D#4,A#4,C#5,F#5",
"Ebm6,-,-,D#4,A#4,C-5,F#5",
// Inversions
"Eb,G-3,-,D#4,A#4,D#5,-",
"Ebmaj7,G-3,-,D#4,A#4,D-5,-",
"Eb7,G-3,-,D#4,A#4,C#5,-",
// Bb
"Bb,-,A#3,F-4,A#4,D-5,F-5",
"Bb,-,A#3,F-4,A#4,D-5,-",
"Bb6,-,A#3,F-4,G-4,D-5,-",
"Bbmaj7,-,A#3,F-4,A-4,D-5,-",
"Bbmaj7,-,A#3,F-4,A-4,D-5,F-5",
"Bbm,-,A#3,F-4,A#4,C#5,F-5",
"Bbm,-,A#3,F-4,A#4,C#5,-",
"Bbm7,-,A#3,F-4,G#4,C#5,F-5",
"Bbm7,-,A#3,F-4,G#4,C#5,-",
"Bbm7b5,-,A#3,E-4,G#4,C#5,-",
"Bbm6,-,A#3,F-4,G-4,C#5,-",
"Bb7,-,A#3,F-4,G#4,D-5,-",
"Bb9,-,A#3,D-4,G#4,C-5,-",
"Bb13,-,A#3,-,G#4,D-5,G-5",
// F#-Gb
"Gb,F#3,C#4,F#4,A#4,C#5,F#5",
"Gbmaj7,F#3,-,F-4,A#4,C#5,-",
"Gbmaj7,-,-,F#4,A#4,C#5,F-5",
"F#m,F#3,C#4,F#4,A-4,C#5,F#5",
"F#m7,F#3,C#4,E-4,A-4,C#5,F#5",
"F#m7,F#3,-,E-4,A-4,C#5,-",
"F#m6,F#3,-,D#4,A-4,C#5,-",
"F#m9,F#3,C#4,E-4,A-4,E-5,G#5",
"F#m7b5,F#3,-,E-4,A-4,C-5,-",
"F#7,F#3,C#4,E-4,A#4,C#5,F#5",
"F#7,F#3,-,E-4,A#4,C#5,-",
"Gbdim,F#3,-,D#4,A-4,C-5,-",
"Gbdim13b,F#3,-,D#4,A-4,D-5,-",
// Ab
"Ab,G#3,C-4,D#4,G#4,C-5,-",
"Ab6,G#3,-,F-4,C-5,D#5,-",
"Abmaj7,G#3,-,G-4,C-5,D#5,-",
"Abadd9,G#3,-,D#4,A#4,C-5,-",
"Abm6,G#3,-,F-4,B-4,D#5,-",
"Abm7,G#3,-,F#4,B-4,D#5,-",
"Abm7b5,G#3,-,F#4,B-4,D-5,-",
"Ab7,G#3,-,F#4,C-5,D#5,-",
"Ab7#5,G#3,-,F#4,C-5,E-5,-",
"Ab9,G#3,-,F#4,A#4,C-5,-",
"Ab7#11,G#3,-,F#4,C-5,D-5,-",
"AbM7#5,G#3,-,G-4,C-5,E-5,-",
// Db
"C#,-,C#4,F-4,G#4,C#5,-",
"C#,-,C#4,F-4,G#4,C#5,F-5",
"Dbmaj7,-,C#4,F-4,G#4,C-5,-",
"C#7,-,C#4,F-4,B-4,C#5,-",
"C#9,-,C#4,F-4,B-4,D#5,-",
"C#9b,-,C#4,F-4,B-4,D-5,-",
"C#9#,-,C#4,F-4,B-4,E-5,-",
// 3&2 notes Rock 
// Power Chords Fift
"E,E-3,B-3,-,-,-,-",
"F,F-3,C-4,-,-,-,-",
"F#,F#3,C#4,-,-,-,-",
"G,G-3,D-4,-,-,-,-",
"A,-,A-3,E-4,-,-,-",
"Bb,-,A#3,F-4,-,-,-",
"B,-,B-3,F#4,-,-,-",
"C,-,C-4,G-4,-,-,-",
// Power Chords third tonic
"C,E-3,C-4,-,-,-,-",
"Db,F-3,C#4,-,-,-,-",
"D,F#3,D-4,-,-,-,-",
"F,-,A-3,F-4,-,-,-",
"Gb,-,A#3,F#4,-,-,-",
"G,-,B-3,G-4,-,-,-",
// 3 notes C
"Em,E-3,B-3,-,G-4,-,-",
"F,F-3,C-4,-,A-4,-,-",
"G,G-3,-,D-4,-,B-4,-",
"Am,-,A-3,E-4,-,C-5,-",
"Bdim,-,B-3,F-4,-,D-5,-",
"C,-,C-4,-,G-4,-,E-5",
"Dm,-,-,D-4,A-4,-,F-5",
"Em,-,-,E-4,-,B-4,G-5",
// 3 notes GmHarm
"F#dim,F#3,C-4,-,A-4,-,-",
"Gm,G-3,D-4,-,A#4,-,-",
"Adim,-,A-3,D#4,-,C-5,-",
"BbAug,-,A#3,F#4,-,D-5,-",
"Cm,-,C-4,-,G-4,D#5,-",
"D,-,-,D-4,A-4,-,F#5",
"Eb,-,-,D#4,A#4,-,G-5",
"F#dim,-,-,F#4,C-5,-,A-5",
// 3 notes Bm melo inversions
"E,-,B-3,E-4,G#4,-,-",
"F#,-,C#4,F#4,A#4,-,-",
"G#dim,-,-,D-4,G#4,B-4,-",
"A#dim,-,-,E-4,A#4,C#5,-",
"Bm,-,-,F#4,B-4,D-5,-",
"C#,-,-,-,G#4,C#5,E-5",
"Daug,-,-,-,A#4,D-5,F#5",
"E,-,-,-,B-4,E-5,G#5",
// 3 notes inversions
"G,-,B-3,-,G-4,D-5,-",
"D,-,A-3,F#4,-,D-5,-",
"F,-,A-3,F-4,-,C-5,-",
"C,G-3,-,E-4,-,C-5,-",
"Eb,G-3,-,D#4,A#4,-,-",
"Bb,F-3,-,D-4,A#4,-,-",
"Gb,-,A#3,F#4,-,C#5,-",
"Db,G#3,-,F-4,-,C#5,-",
"Ab,-,C-4,-,G#4,D#5,-",
"Eb,-,A#3,-,G-4,D#5,-",
"C,-,-,E-4,-,C-5,G-5",
"G,-,D-4,-,B-4,-,G-5",
// Dissonants
"F#m7b9/11,F#3,C#4,F#4,G-4,B-4,E-5",
"B7b9#11,-,B-3,D#4,A-4,C-5,F-5",
"A7/9/#11/13,-,A-3,G-4,B-4,D#5,F#5",
"A7/b9/#11/13,-,A-3,G-4,A#4,D#5,F#5",
"Abm7/9,G#3,-,F#4,A#4,B-4,F#5",
"Eadd9b,E-3,B-3,F-4,G#4,B-4,E-5",
"E11#,E-3,B-3,E-4,A#4,B-4,E-5",
"Gb7/11,F#3,C#4,F#4,A#4,B-4,E-5",
//polytonal
"C/F,F-3,-,-,G-4,C-5,E-5",
"C/D,-,-,D-4,G-4,C-5,E-5",
"A/F,F-3,-,-,A-4,C#5,E-5",
"A/G#,G#3,-,E-4,A-4,C#5,-",
"E/F#,F#3,-,-,G#4,B-4,E-5",
"E/F,F-3,-,-,G#4,B-4,E-5",
"E/C,-,C-3,-,G#4,B-4,E-5",
// fourth
"Fourth,E-3,A-3,D-4,G-4,C-5,F-5",
"C9sus4,-,C-4,F-4,A#4,D-5,G-5",
"Bm11,-,B-3,E-4,A-4,D-5,F#5",
"Am7sus4,-,A-3,D-4,G-4,C-5,E-5",
"Dm7sus4,-,-,D-4,G-4,C-5,F-5",
"Em7sus4,-,B-3,E-4,A-4,D-5,G-5",
//Andalusian cadence I VII VI V
//D
"Dmadd9,F-3,A-3,F-4,A-4,D-5,E-5",
"C9,-,C-4,E-4,A#4,D-5,E-5",
"Bb#11,-,A#3,D-4,A#4,D-5,E-5",
"Aadd9b,-,A-3,E-4,A#4,C#5,E-5",
//A
"Am,-,-,-,C-5,E-5,A-5",
"G,-,-,-,B-4,D-5,G-5",
"F,-,-,-,A-4,C-5,F-5",
"E,-,-,-,G#4,B-4,E-5",
//E
"Em9,-,-,E-4,G-4,B-4,F#5",
"D9,-,-,D-4,A-4,C-5,E-5",
"C9#11,-,C-4,E-4,A#4,-,F#5",
"Badd9b,-,B-3,F#4,C-5,B-4,-",
//B-rock
"B,-,B-3,F#4,B-4,B-4,-",
"A2,-,A-3,E-4,A-4,B-4,-",
"G,G-3,D-4,G-4,B-4,B-4,-",
"F#7,F#3,A#3,E-4,A#4,B-4,-",
// II V I min/maj
// progressions 4 chords
//more inversions

        };

        public string GetChord()
        {
            string ret = "";
            string retOpt = "";

            try
            {
                for (int i = 0; i < Chords.Length; i++)
                {
                    string[] chord = Chords[i].Split(',');

                    if (StringNotes(N(chord[1]), N(chord[2]), N(chord[3]), N(chord[4]), N(chord[5]), N(chord[6])))
                    {
                        ret = chord[0];
                        break; // Found
                    }
                    else if (StringNotesTransposed(N(chord[1]), N(chord[2]), N(chord[3]), N(chord[4]), N(chord[5]), N(chord[6])))
                    {
                        retOpt = chord[0] + " + " + TransposeAmount;
                    }
                }
            }
            catch { }

            if (ret != "")
                return ret;
            else
                return retOpt;
        }

        public static Note Transpose(string note, int change)
        {
            Note ret = new Note(Note.Off);

            if (note != "-")
            {
                int iNote = BuzzNote.Parse(note);
                int iMidiNote = BuzzNote.ToMIDINote(iNote);
                iMidiNote += change;
                iNote = BuzzNote.FromMIDINote(iMidiNote);
                ret = new Note(iNote);
            }
            return ret;
        }
    }
}
