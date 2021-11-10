using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace viselitsa
{
    public class Game
    {
        //private static string[] words = new string[] { "Выставка", "Икона", "Камень", "Кактус", "Пакет", "Автор", "Парашют", "Колосс" };
        private readonly string Word;
        public List<char> Letters { get; private set; }
        public int Attempts { get; private set; }
        public bool Finished { get => Attempts == 0 || AllLettersCollected(); }

        public Game(string[] words)
        {
            Word = words[new Random().Next(0, words.Length)].ToUpper();
            Attempts = 5;
            Letters = new List<char>();
        }

        public string WriteWord()
        {
            var words = Word.ToCharArray();
            string str = "";
            foreach(var e in words)
            {
                if (Letters.Contains(e))
                {
                    str += $"{e} ";
                }
                else
                    str += "_ ";
            }
            return str;
        }

        public bool CheckLetter(string letter, out char? let)
        {
            let = null;

            if (letter.Length > 1)
                return false;

            char converted = letter.ToUpper()[0];

            if (Letters.Contains(converted))
                return false;
            if (converted < 'А' || converted > 'Я')
                return false;

            let = converted;
            return true;
        }

        public void AddChar(char letter)
        {
            Letters.Add(letter);
        }

        public void CheckLetterInWord(char letter)
        {
            AddChar(letter);

            if (Word.Contains(letter))
            {
                WriteWord();
            }
            else
                Attempts -= 1;
        }

        public bool AllLettersCollected()
        {
            foreach(var e in Word)
            {
                if (Letters.Contains(e))
                    continue;
                else
                    return false;
            }
            return true;
        }

        public string LoseOrWin()
        {
            if (AllLettersCollected())
                return "YOU WIN!!";
            else
                return "you lost";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string path = Environment.CurrentDirectory; //path - путь до папки
            while (!File.Exists(path + "\\dictionary.txt")) 
            {
                Console.WriteLine($"файл dictionary.txt не найден. положите файл в папку {path} и нажмите любую клавишу");
                Process.Start("explorer", path); //Process.Start - запуск определенного процесса - проводника
                Console.ReadKey(false); //чтобы дал понять когда закончил
            }

            var game = new Game(File.ReadAllLines(path + "\\dictionary.txt"));
            Console.WriteLine($"Слово: {game.WriteWord()}");

            do
            {
                char? let;
                do
                {
                    Console.WriteLine($"Oсталось попыток:{game.Attempts} \nВведите букву: ");
                }
                while (!game.CheckLetter(Console.ReadLine(), out let));

                game.CheckLetterInWord(let.Value);
                Console.WriteLine(game.WriteWord());

            }
            while (!game.Finished);
            Console.WriteLine(game.LoseOrWin());
        }
    }
}
