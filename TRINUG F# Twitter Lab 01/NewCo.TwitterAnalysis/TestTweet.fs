namespace NewCo.TwitterAnalysis

open System
open System.Configuration
open Tweetinvi

type TestTweetProvider() = 
    member this.GetTweets(stockSymbol: string) =
        let consumerKey = ConfigurationManager.AppSettings.["consumerKey"]
        let consumerSecret = ConfigurationManager.AppSettings.["consumerSecret"]
        let accessToken = ConfigurationManager.AppSettings.["accessToken"]
        let accessTokenSecret = ConfigurationManager.AppSettings.["accessTokenSecret"]
        
        TwitterCredentials.SetCredentials(accessToken, accessTokenSecret, consumerKey, consumerSecret)
        let tweets = Search.SearchTweets(stockSymbol)
        tweets 
            

