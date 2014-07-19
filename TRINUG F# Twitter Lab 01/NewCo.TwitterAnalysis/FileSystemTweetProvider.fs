namespace NewCo.TwitterAnalysis

open System
open System.Collections.Generic
open System.IO

type FileSystemTweetProvider(filePath: string) = 
    interface ITweetProvider with 
        member this.GetTweets(stockSymbol: string) =
            let fileContents = File.ReadLines(filePath)
                                |> Seq.map(fun line -> line.Split([|'\t'|]))
                                |> Seq.map(fun values -> DateTime.Parse(values.[0]),int values.[1], string values.[2])
            fileContents

        member this.GetDailyTotalOfTweets(stockSymbol: string)=
            let provider = new FileSystemTweetProvider(filePath)
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




