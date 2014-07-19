namespace NewCo.TwitterAnalysis

open System
open System.Configuration
open Tweetinvi

type TwitterTweetProvider() = 
    interface ITweetProvider with 
        member this.GetTweets(stockSymbol: string) =
            let consumerKey = ConfigurationManager.AppSettings.["consumerKey"]
            let consumerSecret = ConfigurationManager.AppSettings.["consumerSecret"]
            let accessToken = ConfigurationManager.AppSettings.["accessToken"]
            let accessTokenSecret = ConfigurationManager.AppSettings.["accessTokenSecret"]
        
            TwitterCredentials.SetCredentials(accessToken, accessTokenSecret, consumerKey, consumerSecret)
            let tweets = Search.SearchTweets(stockSymbol);
            tweets 
                |> Seq.map(fun t -> t.CreatedAt, t.RetweetCount,t.Creator.Name)

        member this.GetDailyTotalOfTweets(stockSymbol: string)=
            let provider = new TwitterTweetProvider()
            let currentProvider = (provider :> ITweetProvider)
            let retweetSum = currentProvider.GetTweets(stockSymbol)
                                    |> Seq.map(fun (createdAt,retweetCount,text) -> createdAt.Date,retweetCount)
                                    |> Seq.groupBy(fun (date,retweetCount) -> date)
                                    |> Seq.map(fun(date,list) -> (date,list |> Seq.sumBy snd))
            let tweetCount = currentProvider.GetTweets(stockSymbol)
                                    |> Seq.map(fun (createdAt,retweetCount,text) -> createdAt.Date,retweetCount)
                                    |> Seq.groupBy(fun (date,retweetCount) -> date)
                                    |> Seq.map(fun(date,list) -> (date,list |> Seq.countBy snd))
                                    |> Seq.map(fun(date,list) -> (date,list |> Seq.sumBy snd))
            Seq.zip retweetSum tweetCount
                                    |> Seq.map(fun ((date,retweetSum),(date,tweetCount)) -> date,retweetSum,tweetCount)


