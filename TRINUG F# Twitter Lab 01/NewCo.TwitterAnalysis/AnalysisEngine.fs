namespace NewCo.TwitterAnalysis

open System

type AnalysisEngine(twitterProvider: ITweetProvider, stockProvider: IStockProvider) = 
    member this.CalculateCorrelation(stockSymbol: string) =
        let tweets = twitterProvider.GetDailyTotalOfTweets(stockSymbol) 
                                            |> Seq.map(fun (date,tweetCount,reTweetCount) -> double tweetCount )
        let stockVolume = stockProvider.GetDailyVolumeOfTrades(stockSymbol)
                                            |> Seq.map(fun (date,volume) -> double volume)

        let meanX = Seq.average tweets
        let meanY = Seq.average stockVolume
        let a = Seq.map(fun x -> x-meanX) tweets
        let b = Seq.map(fun y -> y-meanY) stockVolume
        let ab = Seq.zip a b
        let abProduct = Seq.map(fun (a,b) -> a * b) ab
        let aSquare = Seq.map(fun a -> a * a) a
        let bSquare = Seq.map(fun b -> b * b) b
        let abSum = Seq.sum abProduct
        let aSquareSum = Seq.sum aSquare
        let bSquareSum = Seq.sum bSquare
        let sums = aSquareSum * bSquareSum
        let squareRootOfSums = sqrt(sums)
 
        abSum/squareRootOfSums        

