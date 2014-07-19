namespace NewCo.TwitterAnalysis

open System
open System.Collections.Generic

type InMemoryTweetProvider() = 
    interface ITweetProvider with 
        member this.GetTweets(stockSymbol: string) =
            let list = new List<(DateTime*int*string)>()
            list.Add(DateTime.Now, 1,"Test1")
            list.Add(DateTime.Now, 0,"Test2")
            list :> IEnumerable<(DateTime*int*string)>

        member this.GetDailyTotalOfTweets(stockSymbol: string)=
            let list = new List<(DateTime*int*int)>()
            list.Add(DateTime.Parse("4/15/2014").Date, 1,1)
            list.Add(DateTime.Parse("4/16/2014").Date, 2,1)
            list.Add(DateTime.Parse("4/17/2014").Date, 3,1)
            list.Add(DateTime.Parse("4/18/2014").Date, 4,1)
            list.Add(DateTime.Parse("4/19/2014").Date, 5,1)
            list.Add(DateTime.Parse("4/20/2014").Date, 6,1)
            list :> IEnumerable<(DateTime*int*int)>

