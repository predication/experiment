using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Predication.Experiment.Library;

namespace ExpConApp
{
    /* 
     * 
     * we will have several states 
     * a loading state (with some silly animation)
     * a quiting state (with a goodbye animation)
     * a menu page state (which is paged view 
     * a example page state 
     * 
     * the context would have the list of examples
     * the currently selected page
     * the dictionary of states
     * 
     */

    public interface IContext
    {
        IEnumerable<ExampleBase> Examples { get; set; }
        Dictionary<string, MenuState> States { get; set; }
        MenuState CurrentState { get; set; }
        ExampleBase CurrentExample { get; set; }
        int CurrentPage { get; set; }
    }

    public class ExampleMenu : IContext
    {
     
        public IEnumerable<ExampleBase> Examples { get; set; }
        public Dictionary<string, MenuState> States { get; set; }
        public MenuState CurrentState { get; set; }
        public ExampleBase CurrentExample { get; set; }
        public int CurrentPage { get; set; }

        public ExampleMenu(IEnumerable<ExampleBase> examples)
        {
            States = new Dictionary<string, MenuState>();
            Examples = examples;
            InitialiseState(new LoadingState(), true, this);
            InitialiseState(new QuitingState(), false, this);
            InitialiseState(new ListExamplesState(), false, this);
            InitialiseState(new RunExampleState(), false, this);

            if (CurrentState == null)
                Console.WriteLine("!!! No Inital State Defined !!!");

            while (CurrentState != null)
            {
                CurrentState.Display();
            }
        }

        private void InitialiseState(MenuState state, bool isInitial, IContext context)
        {
            state.Context = context;
            States.Add(state.StateName, state);
            if (isInitial)
                CurrentState = state;
        }
    }

    public enum StateNames
    {
        Loading, Quitting, ListExamples, RunExample
    }

    public class MenuState
    {
        public IContext Context { get; set; }
        public string StateName { get; private set; }
        public MenuState(string stateName)
        {
            StateName = stateName;
        }

        public virtual void Display()
        {
            Context.CurrentState = null;
        }

        protected void NextState(StateNames nextState)
        {
            Context.CurrentState = Context.States[nextState.ToString()];
        }

        protected void ClearState()
        {
            Context.CurrentState = null;
        }

        protected void DrawLogo()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(6, 3);
            Console.WriteLine("##");
            Console.SetCursorPosition(6, 4);
            Console.WriteLine("##");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(9, 3);
            Console.WriteLine("##");
            Console.SetCursorPosition(9, 4);
            Console.WriteLine("##");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(6, 6);
            Console.WriteLine("##");
            Console.SetCursorPosition(6, 7);
            Console.WriteLine("##");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(9, 6);
            Console.WriteLine("##");
            Console.SetCursorPosition(9, 7);
            Console.WriteLine("##");
        }
    }

    public class LoadingState : MenuState
    {
        private List<ConsoleColor> _colors = new List<ConsoleColor>{
            ConsoleColor.DarkGray, 
            ConsoleColor.DarkCyan, 
            ConsoleColor.DarkBlue, 
            ConsoleColor.Blue, 
            ConsoleColor.Cyan, 
            ConsoleColor.Yellow, 
            ConsoleColor.White
        };

        public LoadingState()
            : base(StateNames.Loading.ToString())
        {

        }

        public override void Display()
        {
            Context.CurrentPage = 0;
            LoadingAnimation();
            NextState(StateNames.ListExamples);
        }

        private void LoadingAnimation()
        {
            Console.Clear();

            DrawLogo();

            for (int i = 0; i < 1; i++)
            {
                foreach (ConsoleColor color in _colors)
                {
                    Console.CursorLeft = 5;
                    Console.CursorTop = 10;
                    Console.ForegroundColor = color;
                    Console.WriteLine(".NET 4.5 Experiments");
                    Thread.Sleep(100);
                }
            }
            Console.ResetColor();
        }
    }

    public class ListExamplesState : MenuState
    {
        public ListExamplesState()
            : base(StateNames.ListExamples.ToString())
        {

        }

        private Dictionary<int, List<ExampleBase>> _pages;
        private int _pageSize = 8;

        public override void Display()
        {
            Paginate();
            ShowChoices();
        }

        private void Paginate()
        {
            if (_pages == null)
            {
                _pages = new Dictionary<int, List<ExampleBase>>();
                int count = 0;
                int pageCount = 0;
                foreach (ExampleBase example in Context.Examples)
                {
                    if (count % _pageSize == 0)
                    {
                        _pages.Add(pageCount, new List<ExampleBase>());
                        pageCount++;
                    }
                    _pages[pageCount - 1].Add(example);
                    count++;
                }
            }
        }

        private void ShowChoices()
        {
            Console.Clear();
            int currentPage = Context.CurrentPage;
            Console.WriteLine("\n\n  Please Select an option:\n\n");
            // list the examples available
            int number = 1;
            List<ExampleBase> examples = _pages[currentPage];
            List<KeyAction> actions = new List<KeyAction>();
            foreach (ExampleBase example in examples)
            {
                actions.Add(new KeyAction { Character = number.ToString()[0], Text = example.Title, /*Example = example,*/ Activity = () => { Context.CurrentExample = example; NextState(StateNames.RunExample); } });
                number++;
            }
            // list the previous (if applicable)
            if(Context.CurrentPage > 0)
                actions.Add(new KeyAction { Foreground = ConsoleColor.Green, Character = 'P', Text = "Previous Page", Activity = () => { Context.CurrentPage--; } });
            // list the next (if applicable)
            if (Context.CurrentPage < (_pages.Count - 1))
                actions.Add(new KeyAction { Foreground = ConsoleColor.Cyan, Character = 'N', Text = "Next Page", Activity = () => { Context.CurrentPage++; } });
            // always show the quit option
            actions.Add(new KeyAction {Foreground = ConsoleColor.Red, Character = 'Q', Text = "Quit App", Activity = () => { NextState(StateNames.Quitting); } });
            foreach (KeyAction action in actions)
            {
                Console.ForegroundColor = action.Foreground;
                Console.WriteLine("     {0} - {1}", action.Character, action.Text);
            }
            Console.ResetColor();
            bool loop = true;
            while (loop)
            {
                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
                foreach (KeyAction action in actions)
                {
                    if (action.Character.ToString().ToLower() == consoleKeyInfo.KeyChar.ToString().ToLower())
                    {
                        loop = false;
                        action.Activity();
                    }
                }
            }
        }

        public class KeyAction
        {
            public char Character { get; set; }
            public string Text { get; set; }
            public Action Activity { get; set; }
            public ExampleBase Example { get; set; }
            public ConsoleColor Foreground { get; set; }

            public KeyAction()
            {
                Character = ' ';
                Foreground = ConsoleColor.White;
            }
        }

    }

    public class RunExampleState : MenuState
    {
        public RunExampleState()
            : base(StateNames.RunExample.ToString())
        {

        }

        public override void Display()
        {
            Console.Clear();
            Console.ForegroundColor = Context.CurrentExample.Foreground;
            Console.BackgroundColor = Context.CurrentExample.Background;
            Console.WriteLine(Context.CurrentExample.Title);
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(Context.CurrentExample.Description);
            Console.ResetColor();
            
            Context.CurrentExample.Execute();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n\n\nPress any key to return to the menu");
            Console.ReadKey(true);

            NextState(StateNames.ListExamples);
        }
    }

    public class QuitingState : MenuState
    {
        public QuitingState()
            : base(StateNames.Quitting.ToString())
        {

        }

        public override void Display()
        {
            Context.CurrentPage = 0;
            ClearState();
            LoadingAnimation();
        }

        private void LoadingAnimation()
        {
            Console.Clear();
            DrawLogo();
            Console.CursorLeft = 5;
            Console.CursorTop = 10;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("... Goodbye :)");
            Thread.Sleep(1000);
            Console.ResetColor();
        }
    }
}
