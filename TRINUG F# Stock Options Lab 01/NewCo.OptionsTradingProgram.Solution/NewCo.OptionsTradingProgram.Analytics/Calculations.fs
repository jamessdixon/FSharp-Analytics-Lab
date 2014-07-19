namespace NewCo.OptionsTradingProgram.Analytics

open System.Collections.Generic

type PutCallFlag = Put | Call

type BlackScholesInputData = 
    {StockPrice:float;
    StrikePriceOfPotion:float;
    TimeToExpirationInYears:int;
    RiskFreeInterestRate:float;
    Volatility:float}

type Calculations() = 
    member this.Variance (source:IEnumerable<double>) = 
        let mean = Seq.average source
        let deltas = Seq.map(fun x -> pown(x-mean) 2) source
        Seq.average deltas

    member this.StandardDeviation(values:IEnumerable<double>) =
        sqrt(this.Variance(values))

    member this.MovingAverage(values:IEnumerable<double>, windowSize:int)=
        values 
            |> Seq.windowed (windowSize)
            |> Seq.map(fun window -> window |> Seq.average)

    member this.MovingStandardDeviation(values:IEnumerable<double>, windowSize:int)=
        values 
            |> Seq.windowed (windowSize)
            |> Seq.map(fun window -> window |> this.StandardDeviation)

    member this.BollingerBands (values:IEnumerable<double>, windowSize:int)=
        let movingAverage = this.MovingAverage(values,windowSize)
        let movingStandardDeviation = this.MovingStandardDeviation(values,windowSize)
        let movingStandardDeviation' = movingStandardDeviation |> Seq.map(fun window -> window * 2.)
        Seq.zip movingAverage movingStandardDeviation'
