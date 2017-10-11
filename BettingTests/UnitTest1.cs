using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BettingAssignment;
using System.Collections.Generic;

namespace BettingTests
{
    [TestClass]
    public class UnitTest1
    {
        
        Bet bet1 = new Bet("Carlow", "Oscar the horse", new DateTime(2013, 12, 1), 700m, true);
        Bet bet2 = new Bet("Dublin", "Billy the horse", new DateTime(2013, 12, 1), 600m, true);
        Bet bet3 = new Bet("Wicklow", "Michael the horse", new DateTime(2013, 12, 1), 300m, false);
        Bet bet4 = new Bet("Meath", "Myles the horse", new DateTime(2014, 12, 1), 300m, false);
        Bet bet5 = new Bet("Dublin", "Boris the horse", new DateTime(2014, 12, 1), 300m, false);


        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestExceptionForHistoricalBet()
        {
            DateTime futureDateTime = new DateTime(2200, 12, 1);
            
            Bet bet = new Bet("Carlow", "Oscar the horse", futureDateTime, 500m, true);
        }

        [TestMethod]
        public void BinaryWriterAndReaderMethods()
        {
            List<Bet> bets = new List<Bet>();
            List<Bet> bets2 = new List<Bet>();
            DateTime date = new DateTime(2012, 12, 1);
            Bet bet1 = new Bet("Carlow", "Oscar the horse", date, 500m, true);
            Bet bet2 = new Bet("Dublin", "Billy the horse", date, 600m, false);
            bets.Add(bet1);
            bets.Add(bet2);
            Program.BinaryWriterMethod(bets);
            
            bets2 = Program.BinaryReaderMethod();
            Assert.AreEqual(bets2[0].Course, bets[0].Course);
            Assert.AreEqual(bets2[0].Amount, bets[0].Amount);
            Assert.AreEqual(bets2[0].Horse, bets[0].Horse);
            Assert.AreEqual(bets2[0].Date, bets[0].Date);
            Assert.AreEqual(bets2[0].Won, bets[0].Won);
            Assert.AreEqual(bets2[1].Course, bets[1].Course);
            Assert.AreEqual(bets2[1].Amount, bets[1].Amount);
            Assert.AreEqual(bets2[1].Horse, bets[1].Horse);
            Assert.AreEqual(bets2[1].Date, bets[1].Date);
            Assert.AreEqual(bets2[1].Won, bets[1].Won);            

        }

        [TestMethod]
        public void BinaryWriterAndReaderSerializable()
        {
            List<Bet> bets = new List<Bet>();
            List<Bet> bets2 = new List<Bet>();
            DateTime date = new DateTime(2012, 12, 1);
            Bet bet1 = new Bet("Carlow", "Oscar the horse", date, 500m, true);
            Bet bet2 = new Bet("Dublin", "Billy the horse", date, 600m, false);
            bets.Add(bet1);
            bets.Add(bet2);
            Program.BinaryWriterMethod(bets);

            bets2 = Program.BinaryReaderMethod();

            Assert.AreEqual(bets2[0].Course, bets[0].Course);
            CollectionAssert.AreNotEqual(bets, bets2); 
            //proves that while the objects have the same values, they are not the same objects so they've been successfully serialized and reserialized
        }

        [TestMethod]
        public void TestingRegexPatternMatch()
        {
            string pattern = "a";
            string subject = "blah";
            string value = Program.GetBetValue(pattern, subject);
            Assert.AreEqual(pattern, value);
        }

        [TestMethod]
        public void TestingRegexPatternMatchFail()
        {
            string pattern = "a";
            string subject = "blop";
            string value = Program.GetBetValue(pattern, subject);
            Assert.AreNotEqual(pattern, value);

        }
        [TestMethod]
        public void TestingRegexPatternMatchBets()
        {
            string pattern = Program.BetPattern;
            string subject = "blop";
            string value = Program.GetBetValue(pattern, subject);
            Assert.AreNotEqual(pattern, value);

        }
        [TestMethod]
        public void TestingBetBuilder()
        {
            string pattern = "Aintree, (2017, 05, 12), 11.58m, true";
            Bet bet = Program.BetBuilder(pattern);
            DateTime date = DateTime.ParseExact("(2017,05,12)", "(yyyy,MM,dd)", null);

            Assert.AreEqual(bet.Course, "Aintree");
            Assert.AreEqual(bet.Date,date);
            Assert.AreEqual(bet.Amount, 11.58m);
            Assert.AreEqual(bet.Won, true);

        }

        [TestMethod]
        public void TestingBetBuilder2()
        {
            string pattern = "Punchestown,  (2016, 12, 22), 122.52m, true";
            Bet bet = Program.BetBuilder(pattern);
            DateTime date = DateTime.ParseExact("(2016,12,22)", "(yyyy,MM,dd)", null);
            Assert.AreEqual(bet.Course, "Punchestown");
            Assert.AreEqual(bet.Date, date);
            Assert.AreEqual(bet.Amount, 122.52m);
            Assert.AreEqual(bet.Won, true);
        }

        [TestMethod]
        public void HistoricalBetsTester()
        {
            List<Bet> bets = Program.GetHistoricalBets();
            string pattern = "Aintree, (2017, 05, 12), 11.58m, true";
            Bet bet = Program.BetBuilder(pattern);    
            Assert.AreEqual(bets[0].Course, bet.Course);
            Assert.AreEqual(bets[0].Date, bet.Date);
            Assert.AreEqual(bets[0].Amount, bet.Amount);
            Assert.AreEqual(bets[0].Won, bet.Won);

        }

        [TestMethod]
        public void OrderBetsByDate()
        {
            DateTime date1 = new DateTime(2013, 12, 1);
            DateTime date2 = new DateTime(2012, 12, 1);
            Bet bet1 = new Bet("Carlow", "Oscar the horse", date1, 500m, true);
            Bet bet2 = new Bet("Dublin", "Billy the horse", date2, 600m, false);

            List<Bet> bets = new List<Bet>();
            bets.Add(bet1); //this time the order is being changed
            bets.Add(bet2);
            List<Bet> bets2 = new List<Bet>();
            bets2 = Program.OrderBetsByDate(bets);
            Assert.AreEqual(bets2[0], bet2);

        }

        [TestMethod]
        public void OrderBetsByDatePreserve()
        {
            DateTime date1 = new DateTime(2013, 12, 1);
            DateTime date2 = new DateTime(2012, 12, 1);
            Bet bet1 = new Bet("Carlow", "Oscar the horse", date1, 500m, true);
            Bet bet2 = new Bet("Dublin", "Billy the horse", date2, 600m, false);

            List<Bet> bets = new List<Bet>();
            bets.Add(bet2); //this time the order is being preserved rather than changed
            bets.Add(bet1);
            List<Bet> bets2 = new List<Bet>();
            bets2 = Program.OrderBetsByDate(bets);
            Assert.AreEqual(bets2[0], bet2);

        }

        [TestMethod]
        public void GetHighestBetWon()
        {
            DateTime date1 = new DateTime(2013, 12, 1);
            DateTime date2 = new DateTime(2012, 12, 1);
            Bet bet1 = new Bet("Carlow", "Oscar the horse", date1, 500m, true);
            Bet bet2 = new Bet("Dublin", "Billy the horse", date2, 600m, true);
            Bet bet3 = new Bet("Wicklow", "Michael the horse", date1, 300m, true);

            List<Bet> bets = new List<Bet>();
            bets.Add(bet1);
            bets.Add(bet2);
            bets.Add(bet3);
            Bet bet = Program.GetHighestBetWon(bets);
            Assert.AreEqual(bet2, bet);

        }
        [TestMethod]
        public void GetHighestBetWonDifferentInput()
        {
            DateTime date1 = new DateTime(2013, 12, 1);
            DateTime date2 = new DateTime(2012, 12, 1);
            Bet bet1 = new Bet("Carlow", "Oscar the horse", date1, 700m, true); //new highest value
            Bet bet2 = new Bet("Dublin", "Billy the horse", date2, 600m, true);
            Bet bet3 = new Bet("Wicklow", "Michael the horse", date1, 300m, true);

            List<Bet> bets = new List<Bet>();
            bets.Add(bet1); 
            bets.Add(bet2);
            bets.Add(bet3);
            Bet bet = Program.GetHighestBetWon(bets);
            Assert.AreEqual(bet1, bet);

        }        

        [TestMethod]
        public void GetHighestBetLost()
        {
            DateTime date1 = new DateTime(2013, 12, 1);
            DateTime date2 = new DateTime(2012, 12, 1);
            Bet bet1 = new Bet("Carlow", "Oscar the horse", date1, 500m, false);
            Bet bet2 = new Bet("Dublin", "Billy the horse", date2, 600m, false);
            Bet bet3 = new Bet("Wicklow", "Michael the horse", date1, 300m, false);

            List<Bet> bets = new List<Bet>();
            bets.Add(bet1); 
            bets.Add(bet2);
            bets.Add(bet3);
            Bet bet = Program.GetHighestBetLost(bets);
            Assert.AreEqual(bet2, bet);

        }
        [TestMethod]
        public void GetHighestBetLostDifferentInput()
        {
            DateTime date1 = new DateTime(2013, 12, 1);
            DateTime date2 = new DateTime(2012, 12, 1);
            Bet bet1 = new Bet("Carlow", "Oscar the horse", date1, 700m, false);
            Bet bet2 = new Bet("Dublin", "Billy the horse", date2, 600m, false);
            Bet bet3 = new Bet("Wicklow", "Michael the horse", date1, 300m, false);

            List<Bet> bets = new List<Bet>();
            bets.Add(bet1); 
            bets.Add(bet2);
            bets.Add(bet3);
            Bet bet = Program.GetHighestBetLost(bets);
            Assert.AreEqual(bet1, bet);

        }

        [TestMethod]
        public void SuccessCounter()
        {
            DateTime date1 = new DateTime(2013, 12, 1);
            DateTime date2 = new DateTime(2012, 12, 1);
            Bet bet1 = new Bet("Carlow", "Oscar the horse", date1, 700m, false);
            Bet bet2 = new Bet("Dublin", "Billy the horse", date2, 600m, true);
            Bet bet3 = new Bet("Wicklow", "Michael the horse", date1, 300m, false);

            List<Bet> bets = new List<Bet>();
            bets.Add(bet1);
            bets.Add(bet2);
            bets.Add(bet3);
            int success = Program.CountSuccess(bets);
            Assert.AreEqual(1, success);

        }

        [TestMethod]
        public void SuccessCounter2()
        {
            DateTime date1 = new DateTime(2013, 12, 1);
            DateTime date2 = new DateTime(2012, 12, 1);
            Bet bet1 = new Bet("Carlow", "Oscar the horse", date1, 700m, true);
            Bet bet2 = new Bet("Dublin", "Billy the horse", date2, 600m, true);
            Bet bet3 = new Bet("Wicklow", "Michael the horse", date1, 300m, false);

            List<Bet> bets = new List<Bet>();
            bets.Add(bet1);
            bets.Add(bet2);
            bets.Add(bet3);
            int success = Program.CountSuccess(bets);
            Assert.AreEqual(2, success);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestExceptionForHighBetLost()
        {
            List<Bet> bets = new List<Bet>();
            Program.GetHighestBetLost(bets);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestExceptionForHighBetWon()
        {
            List<Bet> bets = new List<Bet>();
            Program.GetHighestBetWon(bets);
        }

        [TestMethod]
        public void CheckYearlyTotalNotEmpty()
        {
            DateTime date1 = new DateTime(2013, 12, 1);
            Bet bet1 = new Bet("Carlow", "Oscar the horse", date1, 700m, true);
            Bet bet2 = new Bet("Dublin", "Billy the horse", date1, 600m, true);
            Bet bet3 = new Bet("Wicklow", "Michael the horse", date1, 300m, false);

            List<Bet> bets = new List<Bet>();
            bets.Add(bet1);
            bets.Add(bet2);
            bets.Add(bet3);
            List<YearlyTotal> yearlyTotal = Program.GetTotalByYear(bets);
            CollectionAssert.AllItemsAreNotNull(yearlyTotal);

        }

        [TestMethod]
        public void GetYearlyTotal()
        {
            DateTime date1 = new DateTime(2013, 12, 1);
            Bet bet1 = new Bet("Carlow", "Oscar the horse", date1, 700m, true);
            Bet bet2 = new Bet("Dublin", "Billy the horse", date1, 600m, true);
            Bet bet3 = new Bet("Wicklow", "Michael the horse", date1, 300m, false);

            List<Bet> bets = new List<Bet>();
            bets.Add(bet1);
            bets.Add(bet2);
            bets.Add(bet3);
            List<YearlyTotal> yearlyTotals =  Program.GetTotalByYear(bets);
            
            Assert.AreEqual(yearlyTotals[0].Year, 2013);
            Assert.AreEqual(yearlyTotals[0].AmountWon, 1300);
            Assert.AreEqual(yearlyTotals[0].AmountLost, 300);
        
           
               
        }

        [TestMethod]
        public void GetYearlyTotalMultipleYears()
        {           
            List<Bet> bets = new List<Bet>();
            bets.Add(bet1);
            bets.Add(bet2);
            bets.Add(bet3);
            bets.Add(bet4);
            List<YearlyTotal> yearlyTotals = Program.GetTotalByYear(bets);
            
            Assert.AreEqual(yearlyTotals[0].Year, 2013);
            Assert.AreEqual(yearlyTotals[0].AmountWon, 1300);
            Assert.AreEqual(yearlyTotals[0].AmountLost, 300);
            Assert.AreEqual(yearlyTotals[1].Year, 2014);
            Assert.AreEqual(yearlyTotals[1].AmountWon, 0);
            Assert.AreEqual(yearlyTotals[1].AmountLost, 300);
        }

        [TestMethod]
        public void GetMostPopularCoursesOneInput()
        {
            List<Bet> bets = new List<Bet>();
            bets.Add(bet1);            
            List<PopularCourses> popularCourses = Program.GetPopularCourses(bets);

            Assert.AreEqual(popularCourses[0].CourseName, "Carlow");
            Assert.AreEqual(popularCourses[0].NumberOfTimes, 1);
        }

        [TestMethod]
        public void GetMostPopularCoursesMultipleInputs()
        {
            List<Bet> bets = new List<Bet>();
            bets.Add(bet1);
            bets.Add(bet2);
            bets.Add(bet3);
            bets.Add(bet5);
            List<PopularCourses> popularCourses = Program.GetPopularCourses(bets);

            Assert.AreEqual(popularCourses[0].CourseName, "Dublin");
            Assert.AreEqual(popularCourses[0].NumberOfTimes, 2);
        }

        [TestMethod]
        public void CheckPresenceOfBets()
        {
            List<Bet> bets = new List<Bet>();
            bets.Add(bet1);
            bets.Add(bet2);
            bets.Add(bet3);
            bets.Add(bet5);
            List<Bet>bets2 = Program.CheckPresenceOfBets(bets);

            CollectionAssert.AreEqual(bets, bets2);
        }
        
        [TestMethod]
        [ExpectedException(typeof (Exception))]
        public void CheckPresenceOfBetsExceptionHandling()
        {
            List<Bet> bets = new List<Bet>();
             Program.CheckPresenceOfBets(bets);
        }
        


    }
}
