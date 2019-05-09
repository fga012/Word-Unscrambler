using System;
using System.Collections.Generic;
using System.Linq;
using WordUnscrambler.Data;
using WordUnscrambler.Workers;

namespace WordUnscrambler
{
    class Program
    {

        private static readonly FileReader fileReader = new FileReader();
        private static readonly WordMatcher wordMatcher = new WordMatcher();
        

        static void Main(string[] args)
        {

            try
            {
                bool continueWordUnscramble = true;
                do
                {

                    Console.WriteLine(Constants.OptionsOnHowToEnterScrambledWords);
                    var option = Console.ReadLine() ?? string.Empty;

                    switch (option.ToUpper())
                    {

                        case Constants.File:
                            Console.Write(Constants.EnterScrambledWordsViaFile);
                            ExecuteScrambledWordInFileScenario();
                            break;
                        case Constants.Manual:
                            Console.Write(Constants.EnterScrambledWordsManually);
                            ExecuteScrambledWordsManualEntryScenario();
                            break;
                        default:
                            Console.Write(Constants.EnterScrambledWordsOptionNotRecognized);
                            break;

                    }

                    var continueDecision = string.Empty;

                    do
                    {

                        Console.WriteLine(Constants.OptionsContinuingTheProgram);
                        continueDecision = (Console.ReadLine() ?? string.Empty);

                    } while (
                        !continueDecision.Equals(Constants.Yes, StringComparison.OrdinalIgnoreCase) &&
                        !continueDecision.Equals(Constants.No, StringComparison.OrdinalIgnoreCase));

                    continueWordUnscramble = continueDecision.Equals(Constants.Yes, StringComparison.OrdinalIgnoreCase);


                } while (continueWordUnscramble);

            }
            catch (Exception ex)
            {

                Console.WriteLine(Constants.ErrorProgramWillBeTerminated + ex.Message);
            }

        }

        private static void ExecuteScrambledWordsManualEntryScenario()
        {
            var manualInput = Console.ReadLine() ?? string.Empty;
            string[] scramabledWords = manualInput.Split(',');
            DisplayMatchedUnscrambledWords(scramabledWords);
        }

        
        private static void ExecuteScrambledWordInFileScenario()
        {

            try
            {
                var filename = Console.ReadLine() ?? string.Empty;
                string[] scramabledWords = fileReader.Read(filename);
                DisplayMatchedUnscrambledWords(scramabledWords);
            }
            catch (Exception ex)
            {

                Console.WriteLine(Constants.ErrorScrambledWordsCannotBeLoaded + ex.Message);
            }

            
        }

        private static void DisplayMatchedUnscrambledWords(string[] scramableWords)
        {
            string[] wordList = fileReader.Read(Constants.WordListFileName);

            List<MatchedWord> matchedWords = wordMatcher.Match(scramableWords, wordList);

            if (matchedWords.Any())
            {
                foreach (var matchedWord in matchedWords)
                {
                    Console.WriteLine(Constants.MatchFound, matchedWord.ScrambledWord, matchedWord.Word);
                }
            }
            else
            {
                Console.WriteLine(Constants.MatchNotFound);
            }

        }

    }
}
