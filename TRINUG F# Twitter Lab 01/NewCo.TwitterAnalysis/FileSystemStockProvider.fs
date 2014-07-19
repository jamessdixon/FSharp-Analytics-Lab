namespace NewCo.TwitterAnalysis

open System
open System.Collections.Generic
open System.IO

type FileSystemStockProvider(filePath: string) = 
    interface IStockProvider with 
        member this.GetDailyVolumeOfTrades(stockSymbol: string) =
            let fileContents = File.ReadLines(filePath)
                                |> Seq.map(fun line -> line.Split([|'\t'|]))
                                |> Seq.map(fun values -> DateTime.Parse(values.[0]),int values.[1])
            fileContents

