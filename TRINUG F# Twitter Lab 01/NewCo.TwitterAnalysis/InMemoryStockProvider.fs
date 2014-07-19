namespace NewCo.TwitterAnalysis

open System
open System.Collections.Generic

type InMemoryStockProvider() = 
    interface IStockProvider with 
        member this.GetDailyVolumeOfTrades(stockSymbol: string)=
            let list = new List<(DateTime*int)>()
            list.Add(DateTime.Parse("4/15/2014").Date,-10000)
            list.Add(DateTime.Parse("4/16/2014").Date,-20000)
            list.Add(DateTime.Parse("4/17/2014").Date,-30000)
            list.Add(DateTime.Parse("4/18/2014").Date,-40000)
            list.Add(DateTime.Parse("4/19/2014").Date,-50000)
            list.Add(DateTime.Parse("4/10/2014").Date,-60000)
            list :> IEnumerable<(DateTime*int)>

