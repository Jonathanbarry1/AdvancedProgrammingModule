using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;


namespace BettingAssignment
{
    public class Program
    {
        /*
         * So I copied and pasted your historical bets into a text file which I then read out using a textwriter,
         * validated using ReGex,and then manipulate inside the menu. Since you asked for options to read and
         * write from a binary file I did not implement this to occur automatically, so you need to read the data
         * from the text file, then manually write to the binary file using the option in the menu. 
         * 
         * I've included this text file in the folder. 
         * 
         * This also applies for adding a new bet as it is added to the list which I am querying with linq,
         * but it is not added to the binary file automatically so again you have to write things to the binary
         * file yourself. Again I did this under the assumption that it's what you wanted. 
         * 
         * Furthermore, I ran into a huge headache trying to implement TDD with anonymous types
         * (it is impossible, right?), so I created unique classes for the popularity and yearly
         * total in order to test them. 
         * 
         * However, I've also included my versions of the methods with anonymous types commented out below 
         * just in case it's what you wanted and I was being foolish. Can't be too careful!
         * 
         * I also have 'showing' methods that write to console rather than using lots of Console.WriteLine 
         * in the menu, and I apologise if this isn't correct.
         * 
         * Also, since there doesn't seem to be any way of indicating it otherwise, I was granted an extension
         * by yourself and Shazia, thanks again for that.
         */ 



        //regex expressions for reading historical bets from text file
        public static readonly string BetPattern = @"[a-zA-Z]+\b,[\s]*\([\d]{4},[\s]*[\d]{2},[\s]*[\d]{2}\),[\s]*(\d)+.(\d){2}m,[\s]*(true|false)";
        static readonly string BetCoursePattern = @"^[a-zA-Z]{2,}"; 
        static readonly string BetDatePattern = @"\([\d]{4},[\s]*[\d]{2},[\s]*[\d]{2}\)";
        static readonly string BetAmountPattern = @"(\d)+.(\d){2}m";
        static readonly string BetVictoryPattern = @"true|false";

        //file paths for historical data from text file and binary file path
        static readonly string BinaryFilePath = @"D:\B8IT119_10370224\bet.txt";
        static readonly string TextFilePath = @"D:\B8IT119_10370224\historicalbets.txt";      


        static void Main(string[] args)
        {            
            bool exit = false;
            List<Bet> bets = new List<Bet>();

            while (!exit)
            {
                Console.WriteLine("\n\n\tWelcome to the betting app, \nPress 1 to access the historical bets which are stored in a text file.");
                Console.WriteLine("Press 2 to write the bets you are currently working with to a binary file.");
                Console.WriteLine("Press 3 to read out all bets saved in the binary file \n\n(N.B. You will will erase data you are currently working with if you have not written it to the binary file)\n");
                Console.WriteLine("Press 4 to add a bet. (You will need to write it to the binary file yourself).");
                Console.WriteLine("Press 5 for a report that lists years, total won and total lost."); 
                Console.WriteLine("Press 6 for a report that shows the most popular race course for bets.");
                Console.WriteLine("Press 7 for a report that shows the bets in date order.");
                Console.WriteLine("Press 8 for a report that displays the highest amount won for a bet laid and the most lost for a bet laid.");
                Console.WriteLine("Press 9 for a report to indicate how successful HotTipster is at horse betting.");
                Console.WriteLine("Press 0 to exit\n");

                int selection;
                if(int.TryParse(Console.ReadLine(), out selection))                
                switch (selection)
                {
                    case 1:
                            try
                            {
                                bets = GetHistoricalBets();
                                foreach (Bet bet in bets)
                                {
                                    Console.WriteLine(bet.ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }                        
                        break;
                    case 2:
                            try
                            {
                                bets = CheckPresenceOfBets(bets);
                                BinaryWriterMethod(bets);
                                Console.WriteLine("Bets are successfully written to the binary file!");
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }                        
                        break;
                    case 3:
                            try
                            {
                                bets = BinaryReaderMethod();
                                bets = CheckPresenceOfBets(bets);
                                foreach (Bet bet in bets)
                                {
                                    Console.WriteLine(bet.ToString());
                                }
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        
                        break;
                    case 4:
                        try
                        {
                            Bet newbet = BetAdder();
                            bets.Add(newbet);
                            Console.WriteLine("Bet added successfully!");
                        }
                        catch (SystemException)
                        {
                            Console.WriteLine("Wrong data entered!");
                        }                        
                        break;
                    case 5:
                        try
                        {
                            bets = CheckPresenceOfBets(bets);
                            ListTotalByYear(bets);
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }                        
                        break;
                    case 6:
                        try
                        {
                            bets = CheckPresenceOfBets(bets);
                            ShowMostPopular(bets); ;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case 7:
                        try
                        {
                            bets = CheckPresenceOfBets(bets);
                            ShowInOrderOfDate(bets);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }                        
                        break;
                    case 8:
                        try
                        {
                            bets = CheckPresenceOfBets(bets);
                            ShowHighestAndLowest(bets);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }                        
                        break;
                    case 9:
                        try
                        {
                            bets = CheckPresenceOfBets(bets);
                            ShowSuccess(bets);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }                        
                        break;
                    case 0: exit = true;
                        break;
                    default: Console.WriteLine("Invalid selection");
                        break;
                }
            }

        }

        public static List<Bet> CheckPresenceOfBets(List<Bet> bets)
        {
            if (bets.Any())
            {
                return bets;
            }
            else
            {
                throw new Exception("No bets were entered.");
            }

        }

        #region Fetching Historical bets from text file

        public static List<Bet> GetHistoricalBets()
        {
            string historicalbets = "";
            MatchCollection validhistoricalbets;
            List<Bet> bets = new List<Bet>();

            using (TextReader txtReader = OpenSourceTextFile())
            {
                historicalbets = txtReader.ReadToEnd();
                validhistoricalbets = Regex.Matches(historicalbets, BetPattern); //this fetches all bets that match the RegEx Pattern for a valid bet string
            }

            foreach (Match validhistoricalbet in validhistoricalbets)
            {
                try
                {
                    Bet bet = BetBuilder(validhistoricalbet.Value); //further splits valid bet string into different chunks that are used to create objects
                    bets.Add(bet);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return bets;
        }

        public static StreamReader OpenSourceTextFile()
        {
            StreamReader txtReader = null;
            try
            {
                FileStream fStream = File.Open(TextFilePath, FileMode.Open);
                txtReader = new StreamReader(fStream, Encoding.UTF8);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return txtReader;
        }        

        public static Bet BetBuilder(string betString)
        {
            string course = GetBetValue(BetCoursePattern, betString);

            string date = GetBetValue(BetDatePattern, betString);
            date = date.Replace(" ", ""); //getting rid of spaces for consistency
            DateTime dateproper = DateTime.ParseExact(date, "(yyyy,MM,dd)", null);
            decimal amount = decimal.Parse(GetBetValue(BetAmountPattern, betString).Trim('m'));
            bool won = bool.Parse(GetBetValue(BetVictoryPattern, betString));
            Bet bet = new Bet(course, dateproper, amount, won);
            return bet;
        }

        public static string GetBetValue(string pattern, string subject)
        {
            Regex reEngine = new Regex(pattern);
            Match match = reEngine.Match(subject);
            return match.Value;
        }

        #endregion

        #region Reading from binary file
        public static List<Bet> BinaryReaderMethod()
        {
            List<Bet> bets = new List<Bet>();
            using (Stream fileStream = new FileStream(BinaryFilePath, FileMode.Open))
            {
                BinaryReader br = new BinaryReader(fileStream);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                bets = (List<Bet>)(binaryFormatter.Deserialize(fileStream)); //reading out serialized list of bets
                return bets;
            }
        }

        #endregion

        #region Writing to Binary File
        public static void BinaryWriterMethod(List<Bet> bets)
        {

            using (Stream fileStream = new FileStream(BinaryFilePath, FileMode.OpenOrCreate))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, bets); //writing out serialized list of bets

            }
        }

#endregion        

        #region Adding Bets
        public static Bet BetAdder()
        {
            string course, date, choice, horse;
            decimal amount;
            bool won;
            
           Console.WriteLine("Please enter the name of the course that the bet happened on");
           course = Console.ReadLine();
           Console.WriteLine("Please enter the name of the horse that the bet happened on");
           horse = Console.ReadLine();            
            Console.WriteLine("Please enter the date that the bet happened on in YYYY,MM,DD format");
            date = Console.ReadLine(); //techincally you can enter this with slashes or commas, brackets or without as I tried to make this as defensive as possible
            date = date.Replace(" ", "");//getting rid of spaces for consistency for date entered
            date = date.Replace("(", "");//getting rid of brackets 
            date = date.Replace(")", "");//getting rid of brackets
            date = date.Replace("/", ","); //replacing slashes with commas
            DateTime dateproper = DateTime.ParseExact(date, "yyyy,MM,dd", null);
            
               
             Console.WriteLine("Did you win? Enter y if you won, enter n if not");
             choice = Console.ReadLine().ToUpper();
             if (choice == "Y")
             {
                 Console.WriteLine("Please enter the amount won on the bet");
                amount = decimal.Parse(Console.ReadLine());
                won = true;
             }
             else
             {
                 Console.WriteLine("Please enter the amount lost on the bet");
                 amount = decimal.Parse(Console.ReadLine());
                 won = false;
             }

             Bet bet = new Bet(course, horse, dateproper, amount, won);
             return bet;         
        }

        #endregion

        #region Listing By Year
        public static void ListTotalByYear(List<Bet> bets)
        {
            var listedTotalByYear = GetTotalByYear(bets);
            Console.WriteLine("Year \t Total Won \t Total Lost ");
            foreach (var bet in listedTotalByYear)
            {
                Console.WriteLine(bet.ToString());
            }

        }

        public static List<YearlyTotal> GetTotalByYear(List<Bet> bets) //I have this method written as an anonymous type commented out below if that's what you wanted
        {
            IEnumerable<YearlyTotal> listedTotalByYear = bets.GroupBy(b => b.Date.Year)
                 .Select(g => new YearlyTotal{
                     Year = g.Key,
                     AmountWon = g.Where(c => c.Won == true).Sum(c => c.Amount),
                     AmountLost = g.Where(c => c.Won == false).Sum(c => c.Amount)
                 });
            return listedTotalByYear.ToList();
        }

        #endregion

        #region Showing Most Popular
        public static void ShowMostPopular(List<Bet> bets)
        {

            var mostPopularCourse = GetPopularCourses(bets);
            Console.WriteLine("The most popular race course is:");
            Console.WriteLine(mostPopularCourse.First().ToString()); //it seems like you were asking for a single popular race course, so I provided this, but I can just as easily stick it in a foreach or something similar with a counter to show the five most popular etc.

        }

        public static List<PopularCourses> GetPopularCourses(List<Bet> bets) //I have this method written as an anonymous type commented out below if that's what you wanted
        {
            IEnumerable<PopularCourses> mostPopularCourses = bets.GroupBy(b => b.Course).OrderByDescending(grpBets => grpBets.Count())
                 .Select(g => new PopularCourses
                 {
                     CourseName = g.Key,
                     NumberOfTimes = g.Count()
                 });

            return mostPopularCourses.ToList(); //I  also send it back as a list as I feel that if the user wants to change the amount of popular race courses that are outputted, you only have to change the show method
        }

        #endregion

        #region Showing In Order of Date
        public static void ShowInOrderOfDate(List<Bet> bets)
        {
            var orderedBets = OrderBetsByDate(bets);
            
                Console.WriteLine("The bets in order of date are: ");
                foreach (var bet in orderedBets)
                    Console.WriteLine(bet.ToString());
            
        }
        public static List<Bet> OrderBetsByDate(List<Bet> bets)
        {
            return bets.OrderBy(bet => bet.Date).ToList();
        }

        #endregion

        #region Showing Highest And Lowest Bets
        public static void ShowHighestAndLowest(List<Bet> bets)
        {
            Bet won = GetHighestBetWon(bets);
            Bet lost = GetHighestBetLost(bets);
            Console.WriteLine("The highest bet won was at {0} and the amount was {1:0.00}", won.Course, won.Amount);
            Console.WriteLine("The highest bet lost was at {0} and the amount was {1:0.00}", lost.Course, lost.Amount);

        }

        public static Bet GetHighestBetWon(List<Bet> bets)
        {
            Bet maxBetWon = bets.OrderByDescending(bet => bet.Amount).Where(bet => bet.Won == true).First();
            return maxBetWon;
        }

        public static Bet GetHighestBetLost(List<Bet> bets)
        {
            Bet maxBetLost = bets.OrderByDescending(bet => bet.Amount).Where(bet => bet.Won == false).First();
            return maxBetLost;
        }

        #endregion

        #region Showing Success

        public static int CountSuccess(List<Bet> bets)
        {
            var success = from bet in bets
                          where bet.Won == true
                          select bets.Count();
            int successcount = success.ToArray().Count();
            return successcount;
        }

        public static void ShowSuccess(List<Bet> bets)
        {
            //display total number of races and the total amount of races that he's won on
                int success = CountSuccess(bets);
                Console.WriteLine($"The amount of bets won has been {success} while the total number of bets has been {bets.Count()}");
                      
        }

        #endregion

    }


    //Commented out code that shows some of the above methods using Anonymous types just in case you wanted it?
    /*public static void ListTotalByYear(List<Bet> bets)
       {
           //report that lists years, total won and total lost
           if (bets.Any())
           {
               var listedTotalByYear = bets.GroupBy(b => b.Date.Year)
                  .Select(g => new {
                 year = g.Key,
                 TotalWon = g.Where(c => c.Won == true).Sum(c => c.Amount),
                 TotalLost = g.Where(c => c.Won == false).Sum(c => c.Amount)
             });
               Console.WriteLine("Year \t Total Won \t total lost ");
               foreach (var bet in listedTotalByYear)
               {
                   Console.WriteLine($"{bet.year} \t {bet.TotalWon} \t {bet.TotalLost}");
               }
           }
           else
           {
               Console.WriteLine("You haven't submitted any bets to be analyzed!");
           }                       
       }

       public static void ShowMostPopular(List<Bet> bets)
       {
           if (bets.Any())
           {
               var mostPopularCourse = bets.GroupBy(bet => bet.Course).OrderByDescending(grpBets => grpBets.Count())
              .Select(orderedBets => new
              {
                  Course = orderedBets.Key,
                  popularity = orderedBets.Count()
              }
              );

               Console.WriteLine("The five most popular courses are: ");
               foreach (var item in mostPopularCourse)
               {
                   Console.WriteLine($"{item.Course} which has been bet on {item.popularity} times");
               }
           }
           else
           {
               Console.WriteLine("You haven't submitted any bets to be analyzed yet!");
           }              
       }
            */


}
