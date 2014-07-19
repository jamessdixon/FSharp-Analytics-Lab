namespace NewCo.TwitterAnalysis

open System
open System.Collections.Generic

type ITweetProvider =
   abstract member GetTweets : string -> IEnumerable<DateTime * int * string>
   abstract member GetDailyTotalOfTweets : string -> IEnumerable<DateTime * int * int>

