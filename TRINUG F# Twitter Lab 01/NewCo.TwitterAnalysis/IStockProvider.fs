namespace NewCo.TwitterAnalysis

open System
open System.Collections.Generic

type IStockProvider =
   abstract member GetDailyVolumeOfTrades : string -> IEnumerable<DateTime * int>

