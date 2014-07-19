namespace NewCo.OptionsTradingProgram.Data

open System
open FSharp.Data
open Newtonsoft.Json
open System.IO

type csvProvider = CsvProvider<"http://ichart.finance.yahoo.com/table.csv?s=MSFT">

type YahooStockProvider() = 
    member this.GetData(stockSymbol: string) =
            csvProvider.Load("http://ichart.finance.yahoo.com/table.csv?s=" + stockSymbol).Rows
    
type FileSystemStockProvider(filePath:string) =
    member this.PutData(stockData) =
        let serializedData = stockData 
                                |> Seq.map(fun row -> JsonConvert.SerializeObject(row))
        File.WriteAllLines(filePath,serializedData)

    member this.GetData() =
        let serializedData = File.ReadAllLines(filePath)
        serializedData 
            |> Seq.map(fun row -> JsonConvert.DeserializeObject<(DateTime*float*float*float*float*int*float)>(row))

