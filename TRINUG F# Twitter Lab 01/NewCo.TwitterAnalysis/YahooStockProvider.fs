namespace NewCo.TwitterAnalysis

open System
open System.Configuration
open FSharp.Data

type csvProvider = CsvProvider<"http://ichart.finance.yahoo.com/table.csv?s=MSFT">

type YahooStockProvider() =
    interface IStockProvider with 
        member this.GetDailyVolumeOfTrades(stockSymbol: string) =
            let stockInformation = csvProvider.Load("http://ichart.finance.yahoo.com/table.csv?s=" + stockSymbol)
            stockInformation.Rows
                                |> Seq.skip(1)
                                |> Seq.map(fun row -> row.Date, row.Volume)

            








