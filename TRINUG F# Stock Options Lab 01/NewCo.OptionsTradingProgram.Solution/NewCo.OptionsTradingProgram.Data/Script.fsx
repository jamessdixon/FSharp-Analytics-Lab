#r "C:\Users\Jamie\Desktop\NewCo.OptionsTradingProgram.Solution\packages\FSharp.Data.2.0.8\lib\portable-net40+sl5+wp8+win8\FSharp.Data.dll"
#r "C:\Users\Jamie\Desktop\NewCo.OptionsTradingProgram.Solution\packages\Newtonsoft.Json.6.0.3\lib\\net45\Newtonsoft.Json.dll"

open System
open FSharp.Data
open Newtonsoft.Json
open System.IO

type csvProvider = CsvProvider<"http://ichart.finance.yahoo.com/table.csv?s=MSFT">

type YahooStockProvider() = 
    member this.GetData(stockSymbol: string) =
            csvProvider.Load("http://ichart.finance.yahoo.com/table.csv?s=" + stockSymbol).Rows
    
type FileSystemStockProvider() =
    member this.PutData(filePath:string, stockData) =
        let serializedData = stockData 
                                |> Seq.map(fun row -> JsonConvert.SerializeObject(row))
        File.WriteAllLines(filePath,serializedData)

    member this.GetData(filePath:string) =
        let serializedData = File.ReadAllLines(filePath)
        serializedData 
            |> Seq.map(fun row -> JsonConvert.DeserializeObject<(DateTime*float*float*float*float*int*float)>(row))

