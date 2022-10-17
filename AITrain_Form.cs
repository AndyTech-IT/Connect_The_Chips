using Connect_The_Chips.Game;
using Connect_The_Chips.Players;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Connect_The_Chips
{
    public partial class AITrain_Form : Form
    {
        public bool Training
        {
            get { return _training; }
            private set
            {
                Start_B.Enabled = value == false;
                Stop_B.Enabled = Abort_B.Enabled = value;
                _training = value;
            }
        }

        public uint Iteration
        {
            get => _iteration;
            private set
            {
                _iteration = value;
                CurentIteration_TB.Text = value.ToString();
            }
        }

        public int Iterations_to_End
        {
            get => _iterations_to_end;
            private set
            {
                if (value == -1)
                    _infinity_repeats = true;
                else
                {
                    _iterations_to_end = value;
                    _infinity_repeats = false;
                }

                IterationsToEnd_TB.Text = _infinity_repeats ? "infinity" : _iterations_to_end.ToString();
            }
        }

        public int Round_Iterations
        {
            get => _round_iterations;
            private set
            {
                if (Training == false)
                    _round_iterations = value;
                else
                    throw new Exception("Blocked while trainig!");
            }
        }


        private Game_Controller[] _games;
        private AI_Player[] _aI_players;
        private Thread[] _training_threads;
        private bool[] _finished;

        private uint _iteration;
        private int _iterations_to_end;
        private bool _infinity_repeats;
        private int _round_iterations;

        private int _saving_training;
        private int _saving_trained;

        private bool _training;

        private object _locker = new object();

        public AITrain_Form()
        {
            InitializeComponent();
            _training = false;
        }

        private void AITrain_Form_Load(object sender, EventArgs e)
        {
        }

        private void Start_B_Click(object sender, EventArgs e)
        {
            Iteration = 0;

            if (InfinityRepiats_CB.Checked)
                Iterations_to_End = -1;
            else
                Iterations_to_End = (int)Repiats_NUD.Value;
            Round_Iterations = (int)RoundIterations_NUD.Value;

            Training = true;

            new Thread(Start_Training).Start();
        }

        private void Stop_B_Click(object sender, EventArgs e)
        {
            Iterations_to_End = (int)(Round_Iterations - Iteration % Round_Iterations);
        }

        private void Abort_B_Click(object sender, EventArgs e)
        {
            foreach (var thread in _training_threads)
                thread.Abort();
            Iterations_to_End = 0;

            Training = false;
        }

        private void Start_Training()
        {
            int count = (int)AIsCount_NUD.Value;
            _aI_players = new AI_Player[count];
            _games = new Game_Controller[count];
            _training_threads = new Thread[count];
            _finished = new bool[count];
            _saving_training = (int)TrainingSave_NUD.Value;
            _saving_trained = (int)TrainedSave_NUD.Value;

            for (int i = 0; i < count; i++)
            {
                if (i == 0)
                    _games[i] = new Game_Controller();
                else
                    _games[i] = new Game_Controller(_games[0]);

                _games[i].Indexed_Game_Finished += On_AI_Finished;
                _aI_players[i] = new AI_Player(training: true);
            }
            Invoke((MethodInvoker)Start_Iteration);
        }

        private void StartGame(object index)
        {
            if (index is int i)
                _games[i].Start_Game(_aI_players[i], i);
            else
                throw new ArgumentException(nameof(index));
        }

        private void Start_Iteration()
        {
            for (int i = _training_threads.Length - 1; i >= 0; i--)
            {
                _finished[i] = false;
                _training_threads[i] = new Thread(StartGame);
                _training_threads[i].Start(i);
            }

            Iteration++;
            if (_infinity_repeats == false)
                if (Iterations_to_End <= 0)
                    Iterations_to_End = (int)(Round_Iterations - Iteration % Round_Iterations);
                else
                    Iterations_to_End--;
        }

        private void On_AI_Finished(int index)
        {
            lock (_locker)
            {
                _finished[index] = true;
                _aI_players[index].Unsubscribe();

                if (_finished.Any(f => f == false) == false)
                {
                    if (Iteration % Round_Iterations == 0)
                    {
                        _aI_players = _aI_players.OrderBy(ai => -ai.Score).ToArray();

                        Invoke((MethodInvoker)Show_Stats);

                        if (_infinity_repeats == false && Iterations_to_End <= 0)
                        {
                            Invoke((MethodInvoker)Training_Finish);
                            return;
                        }

                        Mutate();

                        foreach (var ai in _aI_players)
                            ai.ClearScore();
                    }

                    Invoke((MethodInvoker)Start_Iteration);
                }
            }
        }

        private void Show_Stats()
        {
            int top_len = 10;
            int[] max = _aI_players.Take(top_len).Select(ai => (int)ai.Score).ToArray();
            int[] min = _aI_players.Skip(_aI_players.Length - top_len).Select(ai => (int)ai.Score).ToArray();
            int sum = 0;
            foreach (var ai in _aI_players)
                sum += (int)ai.Score;

            int avg = sum / _aI_players.Length;

            Result_RTB.Text =
                $"Max scores:\n" +
                $"{string.Join("\n", max)}\n" +
                $"Avarage scores:\n" +
                $"{avg}\n" +
                $"Min scores:\n" +
                $"{string.Join("\n", min)}";
        }

        private void Mutate()
        {
            int group_len = _aI_players.Length / 5;
            for (int i = 0; i < group_len; i++)
            {
                _aI_players[i + group_len * 1].Union(_aI_players[i], .0003);

                _aI_players[i + group_len * 2].Union(_aI_players[i], .003);

                _aI_players[i + group_len * 3].Union(_aI_players[i], .003);
                _aI_players[i + group_len * 3].Mutate(.003, .0003);

                _aI_players[i + group_len * 4].Union(_aI_players[i], .03);
                _aI_players[i + group_len * 4].Mutate(.03, .0003);
            }
        }

        private void Training_Finish()
        {
            AI_Player.Clearing_Before_Saving();

            int count = _saving_trained < _aI_players.Length ? _saving_trained : _aI_players.Length;
            for (int i = 0; i < count; i++)
            {
                new Thread(Save_Trained_AI).Start(i);
            }

            count = _saving_training < _aI_players.Length ? _saving_training : _aI_players.Length;
            for (int i = 0; i < count; i++)
            {
                new Thread(Save_Training_AI).Start(i);
            }

            Training = false;
        }

        private void Save_Training_AI(object index)
        {
            if (index is int i)
                _aI_players[i].Save(i.ToString());
            else
                throw new ArgumentException(nameof(index));
        }

        private void Save_Trained_AI(object index)
        {
            if (index is int i)
                _aI_players[i].Save(i.ToString(), true);
            else
                throw new ArgumentException(nameof(index));
        }
    }
}
