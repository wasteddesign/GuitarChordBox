using Buzz.MachineInterface;
using BuzzGUI.Common;
using BuzzGUI.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace WDE.GuitarChordBox
{

    [MachineDecl(Name = "GuitarChordBox", ShortName = "GuitarChord", Author = "WDE", MaxTracks = 1)]
    public class GuitarChordBoxMachine : IBuzzMachine, INotifyPropertyChanged
    {
        public IBuzzMachineHost host;
        private GuitarChordBoxGUI GuitarChordBoxGUI;
        public GuitarNotes GuitarChordBoxNotes { get; set; }

        private Dictionary<IMachine, string> MachineAssociations = new Dictionary<IMachine, string>();

        Random rnd = new Random();

        public string[] selMacTable = { };

        internal void LoadChordStates(Dictionary<Key, ChordState> chordStates)
        {
            if (MachineState.ChordStates != null)
            {
                foreach (ChordState cs in MachineState.ChordStates)
                {
                    if (!chordStates.ContainsKey(cs.key) && cs.key != Key.None)
                        chordStates[cs.key] = cs;
                }
            }
        }

        public GuitarChordBoxMachine(IBuzzMachineHost host)
        {
            this.host = host;
            GuitarChordBoxNotes = new GuitarNotes();

            Global.Buzz.MasterTap += Buzz_MasterTap;
            Global.Buzz.Song.MachineRemoved += Song_MachineRemoved;
            Global.Buzz.Song.MachineAdded += Song_MachineAdded;
        }

        private void Song_MachineAdded(IMachine obj)
        {
            foreach (IMachine mac in Global.Buzz.Song.Machines)
            {
                MachineAssociations[mac] = mac.Name;
            }

            if (obj == host.Machine)
            {
                foreach (IMachine mac in Global.Buzz.Song.Machines)
                {
                    mac.PropertyChanged += Machine_PropertyChanged;
                }
            }
            else
            {
                obj.PropertyChanged += Machine_PropertyChanged;
            }
        }

        private void Machine_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IMachine mac = (IMachine)sender;
            if (e.PropertyName == "Name")
            {
                try
                {
                    string prevName = MachineAssociations[mac];
                    MachineAssociations[mac] = mac.Name;

                    for (int i = 0; i < selMacTable.Length; i++)
                    {
                        if (selMacTable[i] == prevName)
                        {
                            selMacTable[i] = mac.Name;
                            UpdateMachineState();
                            break;
                        }
                    }

                    if (GuitarChordBoxGUI != null)
                    {
                        GuitarChordBoxGUI.CreateMachineList();
                        GuitarChordBoxGUI.UpdateSelection();
                    }
                }
                catch { }
            }
        }

        private void UpdateMachineState()
        {
            lock (thisLock)
            {
                string targetMachines = "";

                int i = 0;

                foreach (var item in selMacTable)
                {
                    selMacTable[i++] = item.ToString();
                    targetMachines += item.ToString() + "\n";
                }
                char[] rem = { '\n' };
                targetMachines = targetMachines.TrimEnd(rem);
                machineState.SelectedMachines = targetMachines;
            }
        }

        private void Song_MachineRemoved(IMachine obj)
        {
            if (MachineAssociations.ContainsKey(obj))
            {
                MachineAssociations.Remove(obj);
                RemoveSelectedMachine(obj);
            }

            if (obj == host.Machine)
            {
                Global.Buzz.MasterTap -= Buzz_MasterTap;
                Global.Buzz.Song.MachineRemoved -= Song_MachineRemoved;
                Global.Buzz.Song.MachineAdded -= Song_MachineAdded;

                foreach (IMachine mac in Global.Buzz.Song.Machines)
                {
                    mac.PropertyChanged -= Machine_PropertyChanged;
                }
            }
            else
            {
                obj.PropertyChanged -= Machine_PropertyChanged;
            }
        }

        private void RemoveSelectedMachine(IMachine obj)
        {
            List<string> retList = new List<string>();

            foreach (string macName in selMacTable)
            {
                if (macName != obj.Name)
                {
                    retList.Add(macName);
                }
            }
            selMacTable = retList.ToArray();
        }

        internal void SaveChordStates(Dictionary<Key, ChordState> chordStates)
        {
            MachineState.ChordStates = new ChordState[chordStates.Count];
            int i = 0;

            foreach (ChordState cs in chordStates.Values)
            {
                //ChordState csSave = new ChordState();
                //csSave.key = cs.key;
                //csSave.note = (Note[])cs.note.Clone();
                //csSave.transpose = cs.transpose;
                MachineState.ChordStates[i] = cs;
                i++;
            }
        }


        private void Buzz_MasterTap(float[] arg1, bool arg2, SongTime arg3)
        {
            if (timerStringE.active)
            {
                timerStringE.delay -= arg1.Length / 2;
                if (timerStringE.delay <= 0 || timerStringENote.Value == BuzzNote.Off)
                {
                    timerStringE.delay = 0;
                    timerStringE.active = false;
                    TimerStringE_Elapsed(this, null);
                }
            }

            if (timerStringA.active)
            {
                timerStringA.delay -= arg1.Length / 2;
                if (timerStringA.delay <= 0 || timerStringANote.Value == BuzzNote.Off)
                {
                    timerStringA.delay = 0;
                    timerStringA.active = false;
                    TimerStringA_Elapsed(this, null);
                }
            }

            if (timerStringD.active)
            {
                timerStringD.delay -= arg1.Length / 2;
                if (timerStringD.delay <= 0 || timerStringDNote.Value == BuzzNote.Off)
                {
                    timerStringD.delay = 0;
                    timerStringD.active = false;
                    TimerStringD_Elapsed(this, null);
                }
            }

            if (timerStringG.active)
            {
                timerStringG.delay -= arg1.Length / 2;
                if (timerStringG.delay <= 0 || timerStringGNote.Value == BuzzNote.Off)
                {
                    timerStringG.delay = 0;
                    timerStringG.active = false;
                    TimerStringG_Elapsed(this, null);
                }
            }

            if (timerStringB.active)
            {
                timerStringB.delay -= arg1.Length / 2;
                if (timerStringB.delay <= 0 || timerStringBNote.Value == BuzzNote.Off)
                {
                    timerStringB.delay = 0;
                    timerStringB.active = false;
                    TimerStringB_Elapsed(this, null);
                }
            }

            if (timerStringE2.active)
            {
                timerStringE2.delay -= arg1.Length / 2;
                if (timerStringE2.delay <= 0 || timerStringE2Note.Value == BuzzNote.Off)
                {
                    timerStringE2.delay = 0;
                    timerStringE2.active = false;
                    TimerStringE2_Elapsed(this, null);
                }
            }
        }

        public void Stop()
        {
            TrackParamStringE(new Note(Note.Off), 0);
            TrackParamStringA(new Note(Note.Off), 0);
            TrackParamStringD(new Note(Note.Off), 0);
            TrackParamStringG(new Note(Note.Off), 0);
            TrackParamStringB(new Note(Note.Off), 0);
            TrackParamStringE2(new Note(Note.Off), 0);
        }

        [ParameterDecl(ValueDescriptions = new[] { "No", "Yes" }, Description = "Enable/Disable sending notes.")]
        public bool SendNotes { get; set; }

        [ParameterDecl(DefValue = 0, MinValue = 0, MaxValue = 15, Description = "Send MIDI Note Channel")]
        public int MIDIChannel { get; set; }

        [ParameterDecl(DefValue = 60, MinValue = 0, MaxValue = 127, Description = "Send Velocity")]
        public int SendVelocity { get; set; }

        [ParameterDecl(DefValue = 0, MinValue = 0, MaxValue = 127, Description = "Randomize Volume")]
        public int VolRandom { get; set; }

        [ParameterDecl(DefValue = 0, MinValue = 0, MaxValue = 2000, Description = "Randomize Delay")]
        public int DelayRandom { get; set; }

        [ParameterDecl(DefValue = 0, MinValue = 0, MaxValue = 2000, Description = "E Delay (ms)")]
        public int StringEDelay { get; set; }

        [ParameterDecl(DefValue = 0, MinValue = 0, MaxValue = 2000, Description = "A Delay (ms)")]
        public int StringADelay { get; set; }

        [ParameterDecl(DefValue = 0, MinValue = 0, MaxValue = 2000, Description = "D Delay (ms)")]
        public int StringDDelay { get; set; }

        [ParameterDecl(DefValue = 0, MinValue = 0, MaxValue = 2000, Description = "G Delay (ms)")]
        public int StringGDelay { get; set; }

        [ParameterDecl(DefValue = 0, MinValue = 0, MaxValue = 2000, Description = "B Delay (ms)")]
        public int StringBDelay { get; set; }

        [ParameterDecl(DefValue = 0, MinValue = 0, MaxValue = 2000, Description = "E2 Delay (ms)")]
        public int StringE2Delay { get; set; }


        private int transpose = 12;
        [ParameterDecl(DefValue = 12, MinValue = 0, MaxValue = 24, Description = "Transpose", ValueDescriptions = new string[] { "-12", "-11", "-10", "-9", "-8", "-7", "-6", "-5", "-4", "-3", "-2", "-1", "0", "+1", "+2", "+3", "+4", "+5", "+6", "+7", "+8", "+9", "+10", "+11", "+12" })]
        public int Transpose
        {
            get { return transpose; }
            set
            {
                if (transpose != value)
                {
                    transpose = value;
                    GuitarChordBoxNotes.TransposeAmount = transpose - 12;
                }
            }
        }

        [ParameterDecl(Name = "Chord", Description = "Play Chord (Index)", IsStateless = true, MinValue = 0, MaxValue = 2000)]
        public void TrackParamChord(int v, int track)
        {
            if (v < GuitarNotes.Chords.Length)
            {

                string[] chords = GuitarNotes.Chords[v].Split(',');

                string strNote = chords[1];
                TrackParamStringE(GuitarNotes.Transpose(strNote, 0), 0);

                strNote = chords[2];
                TrackParamStringA(GuitarNotes.Transpose(strNote, 0), 0);

                strNote = chords[3];
                TrackParamStringD(GuitarNotes.Transpose(strNote, 0), 0);

                strNote = chords[4];
                TrackParamStringG(GuitarNotes.Transpose(strNote, 0), 0);

                strNote = chords[5];
                TrackParamStringB(GuitarNotes.Transpose(strNote, 0), 0);

                strNote = chords[6];
                TrackParamStringE2(GuitarNotes.Transpose(strNote, 0), 0);
            }
        }

        struct StringDelay
        {
            public int delay;
            public bool active;
        }

        private StringDelay timerStringE;
        private StringDelay timerStringA;
        private StringDelay timerStringD;
        private StringDelay timerStringG;
        private StringDelay timerStringB;
        private StringDelay timerStringE2;

        private Note timerStringENote;
        private Note timerStringANote;
        private Note timerStringDNote;
        private Note timerStringGNote;
        private Note timerStringBNote;
        private Note timerStringE2Note;

        private void TimerStringE_Elapsed(object sender, ElapsedEventArgs e)
        {
            Note v = timerStringENote;

            this.SendNoteToTargetMachines(GuitarChordBoxNotes.StringE, 0);
            GuitarChordBoxNotes.StringE = v.Value == Note.Off ? Note.Off : BuzzNote.FromMIDINote(v.ToMIDINote() + Transpose - 12);
            this.SendNoteToTargetMachines(GuitarChordBoxNotes.StringE, SendVelocity);

            if (GuitarChordBoxGUI != null)
            {
                Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Normal,
                  new Action(() =>
                  {
                      GuitarChordBoxGUI.GuitarChordBoxCanvas.SetNoteForString(0, GuitarChordBoxNotes.StringE);
                      GuitarChordBoxGUI.GuitarChordBoxCanvas.SetFingerForString(0, GuitarChordBoxNotes.StringE, GuitarChordBoxNotes.StringE_Finger);
                  }
                  ));
            }
        }

        private void TimerStringA_Elapsed(object sender, ElapsedEventArgs e)
        {
            Note v = timerStringANote;

            this.SendNoteToTargetMachines(GuitarChordBoxNotes.StringA, 0);
            GuitarChordBoxNotes.StringA = v.Value == Note.Off ? Note.Off : BuzzNote.FromMIDINote(v.ToMIDINote() + Transpose - 12);
            this.SendNoteToTargetMachines(GuitarChordBoxNotes.StringA, SendVelocity);

            if (GuitarChordBoxGUI != null)
            {
                Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Normal,
                  new Action(() =>
                  {
                      GuitarChordBoxGUI.GuitarChordBoxCanvas.SetNoteForString(1, GuitarChordBoxNotes.StringA);
                      GuitarChordBoxGUI.GuitarChordBoxCanvas.SetFingerForString(1, GuitarChordBoxNotes.StringA, GuitarChordBoxNotes.StringA_Finger);
                  }
                  ));
            }
        }

        private void TimerStringD_Elapsed(object sender, ElapsedEventArgs e)
        {
            Note v = timerStringDNote;

            this.SendNoteToTargetMachines(GuitarChordBoxNotes.StringD, 0);
            GuitarChordBoxNotes.StringD = v.Value == Note.Off ? Note.Off : BuzzNote.FromMIDINote(v.ToMIDINote() + Transpose - 12);
            this.SendNoteToTargetMachines(GuitarChordBoxNotes.StringD, SendVelocity);

            if (GuitarChordBoxGUI != null)
            {
                Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Normal,
                  new Action(() =>
                  {
                      GuitarChordBoxGUI.GuitarChordBoxCanvas.SetNoteForString(2, GuitarChordBoxNotes.StringD);
                      GuitarChordBoxGUI.GuitarChordBoxCanvas.SetFingerForString(2, GuitarChordBoxNotes.StringD, GuitarChordBoxNotes.StringD_Finger);
                  }
                  ));
            }
        }

        private void TimerStringG_Elapsed(object sender, ElapsedEventArgs e)
        {
            Note v = timerStringGNote;

            this.SendNoteToTargetMachines(GuitarChordBoxNotes.StringG, 0);
            GuitarChordBoxNotes.StringG = v.Value == Note.Off ? Note.Off : BuzzNote.FromMIDINote(v.ToMIDINote() + Transpose - 12);
            this.SendNoteToTargetMachines(GuitarChordBoxNotes.StringG, SendVelocity);

            if (GuitarChordBoxGUI != null)
            {
                Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Normal,
                  new Action(() =>
                  {
                      GuitarChordBoxGUI.GuitarChordBoxCanvas.SetNoteForString(3, GuitarChordBoxNotes.StringG);
                      GuitarChordBoxGUI.GuitarChordBoxCanvas.SetFingerForString(3, GuitarChordBoxNotes.StringG, GuitarChordBoxNotes.StringG_Finger);
                  }
                  ));
            }
        }

        private void TimerStringB_Elapsed(object sender, ElapsedEventArgs e)
        {
            Note v = timerStringBNote;

            this.SendNoteToTargetMachines(GuitarChordBoxNotes.StringB, 0);
            GuitarChordBoxNotes.StringB = v.Value == Note.Off ? Note.Off : BuzzNote.FromMIDINote(v.ToMIDINote() + Transpose - 12);
            this.SendNoteToTargetMachines(GuitarChordBoxNotes.StringB, SendVelocity);

            if (GuitarChordBoxGUI != null)
            {
                Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Normal,
                  new Action(() =>
                  {
                      GuitarChordBoxGUI.GuitarChordBoxCanvas.SetNoteForString(4, GuitarChordBoxNotes.StringB);
                      GuitarChordBoxGUI.GuitarChordBoxCanvas.SetFingerForString(4, GuitarChordBoxNotes.StringB, GuitarChordBoxNotes.StringB_Finger);
                  }
                  ));
            }
        }

        private void TimerStringE2_Elapsed(object sender, ElapsedEventArgs e)
        {
            Note v = timerStringE2Note;

            this.SendNoteToTargetMachines(GuitarChordBoxNotes.StringE2, 0);
            GuitarChordBoxNotes.StringE2 = v.Value == Note.Off ? Note.Off : BuzzNote.FromMIDINote(v.ToMIDINote() + Transpose - 12);
            this.SendNoteToTargetMachines(GuitarChordBoxNotes.StringE2, SendVelocity);

            if (GuitarChordBoxGUI != null)
            {
                Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Normal,
                  new Action(() =>
                  {
                      GuitarChordBoxGUI.GuitarChordBoxCanvas.SetNoteForString(5, GuitarChordBoxNotes.StringE2);
                      GuitarChordBoxGUI.GuitarChordBoxCanvas.SetFingerForString(5, GuitarChordBoxNotes.StringE2, GuitarChordBoxNotes.StringE2_Finger);
                  }
                  ));
            }
        }

        [ParameterDecl(Name = "String E", Description = "String E", IsStateless = true)]
        public void TrackParamStringE(Note v, int track)
        {
            timerStringENote = v;

            int delay = GetDelayAndRandom(StringEDelay);

            if (delay == 0)
            {
                TimerStringE_Elapsed(this, null);
            }
            else
            {
                timerStringE.delay = delay * Global.Buzz.SelectedAudioDriverSampleRate / 1000;
                timerStringE.active = true;
            }
        }


        [ParameterDecl(Name = "String E Finger", Description = "String E Finger", IsStateless = true, MaxValue = 4)]
        public void TrackParamFingerE(int v, int track)
        {
            GuitarChordBoxNotes.StringE_Finger = v;
            if (GuitarChordBoxGUI != null && !timerStringE.active)
            {
                Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Normal,
                  new Action(() =>
                  {
                      GuitarChordBoxGUI.GuitarChordBoxCanvas.SetFingerForString(0, GuitarChordBoxNotes.StringE, GuitarChordBoxNotes.StringE_Finger);
                  }
                  ));
            }
        }

        [ParameterDecl(Name = "String A", Description = "String A", IsStateless = true)]
        public void TrackParamStringA(Note v, int track)
        {
            timerStringANote = v;

            int delay = GetDelayAndRandom(StringADelay);

            if (delay == 0)
            {
                TimerStringA_Elapsed(this, null);
            }
            else
            {
                timerStringA.delay = delay * Global.Buzz.SelectedAudioDriverSampleRate / 1000;
                timerStringA.active = true;
            }
        }

        [ParameterDecl(Name = "String A Finger", Description = "String A Finger", IsStateless = true, MaxValue = 4)]
        public void TrackParamFingerA(int v, int track)
        {
            GuitarChordBoxNotes.StringA_Finger = v;
            if (GuitarChordBoxGUI != null && !timerStringA.active)
            {
                Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Normal,
                  new Action(() =>
                  {
                      GuitarChordBoxGUI.GuitarChordBoxCanvas.SetFingerForString(1, GuitarChordBoxNotes.StringA, GuitarChordBoxNotes.StringA_Finger);
                  }
                  ));
            }
        }

        [ParameterDecl(Name = "String D", Description = "String D", IsStateless = true)]
        public void TrackParamStringD(Note v, int track)
        {
            timerStringDNote = v;

            int delay = GetDelayAndRandom(StringDDelay);

            if (delay == 0)
            {
                TimerStringD_Elapsed(this, null);
            }
            else
            {
                timerStringD.delay = delay * Global.Buzz.SelectedAudioDriverSampleRate / 1000;
                timerStringD.active = true;
            }
        }

        [ParameterDecl(Name = "String D Finger", Description = "String D Finger", IsStateless = true, MaxValue = 4)]
        public void TrackParamFingerD(int v, int track)
        {
            GuitarChordBoxNotes.StringD_Finger = v;

            if (GuitarChordBoxGUI != null && !timerStringD.active)
            {
                Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Normal,
                  new Action(() =>
                  {
                      GuitarChordBoxGUI.GuitarChordBoxCanvas.SetFingerForString(2, GuitarChordBoxNotes.StringD, GuitarChordBoxNotes.StringD_Finger);
                  }
                  ));
            }
        }

        [ParameterDecl(Name = "String G", Description = "String G", IsStateless = true)]
        public void TrackParamStringG(Note v, int track)
        {
            timerStringGNote = v;

            int delay = GetDelayAndRandom(StringGDelay);

            if (delay == 0)
            {
                TimerStringG_Elapsed(this, null);
            }
            else
            {
                timerStringG.delay = delay * Global.Buzz.SelectedAudioDriverSampleRate / 1000;
                timerStringG.active = true;
            }
        }

        [ParameterDecl(Name = "String G Finger", Description = "String G Finger", IsStateless = true, MaxValue = 4)]
        public void TrackParamFingerG(int v, int track)
        {
            GuitarChordBoxNotes.StringG_Finger = v;
            if (GuitarChordBoxGUI != null && !timerStringG.active)
            {
                Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Normal,
                  new Action(() =>
                  {
                      GuitarChordBoxGUI.GuitarChordBoxCanvas.SetFingerForString(3, GuitarChordBoxNotes.StringG, GuitarChordBoxNotes.StringG_Finger);
                  }
                  ));
            }
        }

        [ParameterDecl(Name = "String B", Description = "String B", IsStateless = true)]
        public void TrackParamStringB(Note v, int track)
        {
            timerStringBNote = v;

            int delay = GetDelayAndRandom(StringBDelay);

            if (delay == 0)
            {
                TimerStringB_Elapsed(this, null);
            }
            else
            {
                timerStringB.delay = delay * Global.Buzz.SelectedAudioDriverSampleRate / 1000;
                timerStringB.active = true;
            }
        }

        [ParameterDecl(Name = "String B Finger", Description = "String B Finger", IsStateless = true, MaxValue = 4)]
        public void TrackParamFingerB(int v, int track)
        {
            GuitarChordBoxNotes.StringB_Finger = v;
            if (GuitarChordBoxGUI != null && !timerStringB.active)
            {
                Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Normal,
                  new Action(() =>
                  {
                      GuitarChordBoxGUI.GuitarChordBoxCanvas.SetFingerForString(4, GuitarChordBoxNotes.StringB, GuitarChordBoxNotes.StringB_Finger);
                  }
                  ));
            }
        }

        [ParameterDecl(Name = "String E2", Description = "String E2", IsStateless = true)]
        public void TrackParamStringE2(Note v, int track)
        {
            timerStringE2Note = v;

            int delay = GetDelayAndRandom(StringE2Delay);

            if (delay == 0)
            {
                TimerStringE2_Elapsed(this, null);
            }
            else
            {

                timerStringE2.delay = delay * Global.Buzz.SelectedAudioDriverSampleRate / 1000;
                timerStringE2.active = true;
            }
        }

        [ParameterDecl(Name = "String E2 Finger", Description = "String E2 Finger", IsStateless = true, MaxValue = 4)]
        public void TrackParamFingerE2(int v, int track)
        {
            GuitarChordBoxNotes.StringE2_Finger = v;
            if (GuitarChordBoxGUI != null && !timerStringE2.active)
            {
                Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Normal,
                  new Action(() =>
                  {
                      GuitarChordBoxGUI.GuitarChordBoxCanvas.SetFingerForString(5, GuitarChordBoxNotes.StringE2, GuitarChordBoxNotes.StringE2_Finger);
                  }
                  ));
            }
        }

        // actual machine ends here. the stuff below demonstrates some other features of the api.

        public class State : INotifyPropertyChanged
        {
            string selectedMachines = "";
            public ChordState[] ChordStates { get; set; }
            public string SelectedMachines
            {
                get
                {
                    return selectedMachines;
                }
                set
                {
                    selectedMachines = value;
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        State machineState = new State();
        public State MachineState           // a property called 'MachineState' gets automatically saved in songs and presets
        {
            get { return machineState; }
            set
            {
                machineState = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("MachineState"));
                string selMac = machineState.SelectedMachines;
                selMacTable = selMac.Split('\n');
            }
        }


        public IEnumerable<IMenuItem> Commands
        {
            get
            {
                yield return new MenuItemVM()
                {
                    Text = "About...",
                    Command = new SimpleCommand()
                    {
                        CanExecuteDelegate = p => true,
                        ExecuteDelegate = p => MessageBox.Show("GuitaTab 0.9.8 (C) 2020 WDE")
                    }
                };
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal void SetGUI(GuitarChordBoxGUI GuitarChordBoxGUI)
        {
            this.GuitarChordBoxGUI = GuitarChordBoxGUI;
        }

        public void SendNoteToTargetMachines(int note, int vol)
        {
            if (this.SendNotes)
            {
                foreach (string item in this.selMacTable)
                {
                    IMachine machine = Global.Buzz.Song.Machines.FirstOrDefault(m => m.Name == item);

                    if (vol > 0)
                    {

                        int rndNum = rnd.Next(-VolRandom / 2, VolRandom / 2);
                        vol += rndNum;
                        vol = vol < 0 ? 0 : vol;
                        vol = vol > 127 ? 127 : vol;
                    }

                    if (machine != null && note != BuzzNote.Off)
                        machine.SendMIDINote(this.MIDIChannel, BuzzNote.ToMIDINote(note), vol);
                }
            }
        }

        public int GetDelayAndRandom(int delay)
        {
            int rndNum = rnd.Next(-DelayRandom / 2, DelayRandom / 2);
            delay += rndNum;
            delay = delay < 0 ? 0 : delay;
            delay = delay > 2000 ? 2000 : delay;

            return delay;
        }

        static Object thisLock = new Object();

        internal void TargetMachinesChanged(IList selectedItems)
        {
            string targetMachines = "";

            lock (thisLock)
            {
                selMacTable = new string[selectedItems.Count];
                int i = 0;

                foreach (var item in selectedItems)
                {
                    selMacTable[i++] = item.ToString();
                    targetMachines += item.ToString() + "\n";
                }
                char[] rem = { '\n' };
                targetMachines = targetMachines.TrimEnd(rem);
                machineState.SelectedMachines = targetMachines;
            }
        }
    }

    public class MachineGUIFactory : IMachineGUIFactory { public IMachineGUI CreateGUI(IMachineGUIHost host) { return new GuitarChordBoxGUI(); } }
    public class GuitarChordBoxGUI : UserControl, IMachineGUI
    {
        IMachine machine;
        GuitarChordBoxMachine GuitarChordBoxMachine;
        public GuitarChordBoxCanvas GuitarChordBoxCanvas;

        TextBox tb;
        ListBox lb;

        // view model for machine list box items
        public class MachineVM
        {
            public IMachine Machine { get; private set; }
            public MachineVM(IMachine m) { Machine = m; }
            public override string ToString() { return Machine.Name; }
        }

        public ObservableCollection<MachineVM> Machines { get; private set; }

        public IMachine Machine
        {
            get { return machine; }
            set
            {
                if (machine != null)
                {
                }

                machine = value;

                if (machine != null)
                {
                    GuitarChordBoxMachine = (GuitarChordBoxMachine)machine.ManagedMachine;
                    GuitarChordBoxMachine.SetGUI(this);

                    GuitarChordBoxCanvas.SetNotes(GuitarChordBoxMachine.GuitarChordBoxNotes);

                    machine.Graph.MachineAdded += machine_Graph_MachineAdded;
                    machine.Graph.MachineRemoved += machine_Graph_MachineRemoved;

                    CreateMachineList();

                    lb.SetBinding(ListBox.ItemsSourceProperty, new Binding("Machines") { Source = this, Mode = BindingMode.OneWay });

                    UpdateSelection();

                }
            }
        }

        public void CreateMachineList()
        {
            lb.SelectionChanged -= Lb_SelectionChanged;
            Machines.Clear();
            foreach (var m in machine.Graph.Machines)
                machine_Graph_MachineAdded(m);
            lb.SelectionChanged += Lb_SelectionChanged;
        }

        public void UpdateSelection()
        {
            string[] machines_ = GuitarChordBoxMachine.selMacTable;
            foreach (var item in machines_)
            {
                foreach (var lbitem in lb.Items)
                {
                    if (item.ToString() == lbitem.ToString())
                        lb.SelectedItems.Add(lbitem);
                }
            }
        }

        public GuitarChordBoxGUI()
        {
            Machines = new ObservableCollection<MachineVM>();

            Button bt = new Button() { Content = "Chords Window", Height = 30, Margin = new Thickness(1, 2, 1, 6) };
            bt.Click += Bt_Click;

            tb = new TextBox() { Margin = new Thickness(0, 0, 0, 4), AllowDrop = true };
            GuitarChordBoxCanvas = new GuitarChordBoxCanvas() { VerticalAlignment = VerticalAlignment.Stretch, MinHeight = 500, HorizontalAlignment = HorizontalAlignment.Stretch };


            Label label = new Label();
            label.Content = "MIDI Send Targets:";

            lb = new ListBox() { Height = 80, Margin = new Thickness(0, 0, 0, 4) };
            lb.SelectionChanged += Lb_SelectionChanged;
            lb.SelectionMode = SelectionMode.Multiple;

            var sp = new StackPanel();

            sp.Children.Add(GuitarChordBoxCanvas);
            sp.Children.Add(label);
            sp.Children.Add(lb);
            sp.Children.Add(bt);

            this.Content = sp;

            Loaded += GuitarChordBoxGUI_Loaded;
            this.SizeChanged += GuitarChordBoxGUI_SizeChanged;

            this.Unloaded += GuitarChordBoxGUI_Unloaded;
        }

        private void GuitarChordBoxGUI_Unloaded(object sender, RoutedEventArgs e)
        {
            if (cw != null)
                cw.Close();
        }

        private ChordWindow cw;
        private void Bt_Click(object sender, RoutedEventArgs e)
        {
            if (cw != null)
                cw.Close();

            cw = new ChordWindow(GuitarChordBoxMachine, GuitarChordBoxMachine.host.Machine.ParameterWindow.Resources);
            cw.Closed += (sender2, e2) =>
            {
                cw = null;
            };
            cw.Show();
        }

        public void UpdateChordWindow(int transpose)
        {
            if (cw != null)
            {
                cw.UpdateChordsGridFromMachine();
            }
        }

        private void GuitarChordBoxGUI_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GuitarChordBoxCanvas.Draw();
        }

        private void Lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lbs = (ListBox)sender;
            GuitarChordBoxMachine.TargetMachinesChanged(lbs.SelectedItems);
        }

        void machine_Graph_MachineAdded(IMachine machine)
        {
            Machines.Add(new MachineVM(machine));
        }

        void machine_Graph_MachineRemoved(IMachine machine)
        {
            Machines.Remove(Machines.First(m => m.Machine == machine));
        }

        private static double PARAM_HEIGHT_SUB = 496;
        private void GuitarChordBoxGUI_Loaded(object sender, RoutedEventArgs e)
        {
            GuitarChordBoxMachine.host.Machine.ParameterWindow.MinWidth = GuitarChordBoxMachine.host.Machine.ParameterWindow.Width;
            GuitarChordBoxMachine.host.Machine.ParameterWindow.MinHeight = GuitarChordBoxMachine.host.Machine.ParameterWindow.Height;

            GuitarChordBoxMachine.host.Machine.ParameterWindow.SizeChanged += ParameterWindow_SizeChanged;
        }

        private void ParameterWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GuitarChordBoxCanvas.Width = e.NewSize.Width - 22;
            GuitarChordBoxCanvas.Height = e.NewSize.Height - PARAM_HEIGHT_SUB;
        }
    }
}
