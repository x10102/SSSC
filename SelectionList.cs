using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallSimpleSerialConsole
{

    public struct SLOption
    {
        public string text;
        public bool alignRight;
        public int index;
        public SLOption(string text, bool alignRight, int index)
        {
            this.text = text;
            this.alignRight = alignRight;
            this.index = index;
        }
        public override string ToString()
        {
            return this.text;
        }
    };
    class SelectionList
    {
        private string title = "";
        private List<SLOption> options = new List<SLOption>();

        public SelectionList(string title = "")
        {
            this.title = title;
        }

        public SelectionList(string[] items, string title = "")
        {
            foreach(string item in items)
            {
                this.addOption(item);
            }
            this.title = title;
        }

        public SLOption DisplayList()
        {
            int selectedIdx = 0;
            while(true)
            {
                Console.Clear();

                if (title.Length > 0)
                {
                    Console.WriteLine(title);
                }

                foreach (SLOption opt in options)
                {
                    drawOption(opt, selectedIdx == opt.index);
                }

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.DownArrow:
                        if (selectedIdx < options.Count - 1) { selectedIdx++; } 
                        break;

                    case ConsoleKey.UpArrow:
                        if (selectedIdx > 0) { selectedIdx--; }
                        break;

                    case ConsoleKey.Enter:
                        Console.Clear();
                        return options[selectedIdx];
                }
            }
        }
        public void addOption(string text, bool alignRight = false, int index = -1)
        {
            index = index == -1 ? options.Count : index;
            options.Add(new SLOption(text, alignRight, index));
        }

        private void drawOption(SLOption opt, bool check)
        {
            if(opt.alignRight)
            {
                Console.WriteLine("{0} [ {1} ]", opt.text, check ? "X" : " ");
            } else
            {
                Console.WriteLine("[ {1} ] {0}", opt.text, check ? "X" : " ");
            }
        }

    }
}
