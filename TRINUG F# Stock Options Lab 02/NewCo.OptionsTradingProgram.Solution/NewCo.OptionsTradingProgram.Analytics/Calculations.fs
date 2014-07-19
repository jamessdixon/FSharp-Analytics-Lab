namespace NewCo.OptionsTradingProgram.Analytics

open System.Collections.Generic
open MathNet.Numerics.Distributions;

type PutCallFlag = Put | Call

type BlackScholesInputData = 
    {StockPrice:float;
    StrikePrice:float;
    TimeToExpiry:float;
    InterestRate:float;
    Volatility:float}

type MonteCarloInputData = 
    {StockPrice:float;
    StrikePrice:float;
    TimeToExpiry:float;
    InterestRate:float;
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

    
    member this.Power baseNumber exponent = exp(exponent * log(baseNumber))
    
    member this.CumulativeDistribution (x) =
            let a1 =  0.31938153
            let a2 = -0.356563782
            let a3 =  1.781477937
            let a4 = -1.821255978
            let a5 =  1.330274429
            let pi = 3.141592654
            let l  = abs(x)
            let k  = 1.0 / (1.0 + 0.2316419 * l)

            let a1' = a1*k
            let a2' = a2*k*k
            let a3' = a3*(this.Power k 3.0)
            let a4' = a4*(this.Power k 4.0)
            let a5' = a5*(this.Power k 5.0)
            let w1 = 1.0/sqrt(2.0*pi)
            let w2 = exp(-l*l/2.0)
            let w3 = a1'+a2'+a3'+a4'+a5'
            let w  = 1.0-w1*w2*w3
            if x < 0.0 then 1.0 - w else w


    member this.BlackScholes(inputData:BlackScholesInputData, putCallFlag:PutCallFlag)=
           let sx = log(inputData.StockPrice / inputData.StrikePrice)
           let rv = inputData.InterestRate+inputData.Volatility*inputData.Volatility*0.5
           let rvt = rv*inputData.TimeToExpiry
           let vt = (inputData.Volatility*sqrt(inputData.TimeToExpiry))
           let d1=(sx + rvt)/vt
           let d2=d1-vt
    
           match putCallFlag with
            | Put -> 
                let xrt = inputData.StrikePrice*exp(-inputData.InterestRate*inputData.TimeToExpiry)
                let cdD1 = xrt*this.CumulativeDistribution(-d2)
                let cdD2 = inputData.StockPrice*this.CumulativeDistribution(-d1)
                cdD1-cdD2
            | Call ->
                let xrt = inputData.StrikePrice*exp(-inputData.InterestRate*inputData.TimeToExpiry)
                let cdD1 = inputData.StockPrice*this.CumulativeDistribution(d1)
                let cdD2 = xrt*this.CumulativeDistribution(d2)
                cdD1-cdD2

    member this.DaysToYears day = (float day) / 365.25

    member this.NormalDistribution = new Normal(0.0, 1.0)

    member this.BlackScholesDelta (inputData:BlackScholesInputData, PutCallFlag:PutCallFlag) =
        let sx = log(inputData.StockPrice / inputData.StrikePrice)
        let rv = inputData.InterestRate+inputData.Volatility*inputData.Volatility*0.5
        let rvt = rv*inputData.TimeToExpiry
        let vt = (inputData.Volatility*sqrt(inputData.TimeToExpiry))
        let d1=(sx + rvt)/vt
        match PutCallFlag with
        | Put -> this.CumulativeDistribution(d1) - 1.0
        | Call -> this.CumulativeDistribution(d1)

    member this.BlackScholesGamma (inputData:BlackScholesInputData) =
        let sx = log(inputData.StockPrice / inputData.StrikePrice)
        let rv = inputData.InterestRate+inputData.Volatility*inputData.Volatility*0.5
        let rvt = rv*inputData.TimeToExpiry
        let vt = (inputData.Volatility*sqrt(inputData.TimeToExpiry))
        let d1=(sx + rvt)/vt
        this.NormalDistribution.Density(100.0) |> ignore
        this.NormalDistribution.Density(d1)

    member this.BlackScholesVega (inputData:BlackScholesInputData) =
        let sx = log(inputData.StockPrice / inputData.StrikePrice)
        let rv = inputData.InterestRate+inputData.Volatility*inputData.Volatility*0.5
        let rvt = rv*inputData.TimeToExpiry
        let vt = (inputData.Volatility*sqrt(inputData.TimeToExpiry))
        let d1=(sx + rvt)/vt   
        this.NormalDistribution.Density(100.0) |> ignore
        inputData.StockPrice*this.NormalDistribution.Density(d1)*sqrt(inputData.TimeToExpiry)

    member this.BlackScholesTheta (inputData:BlackScholesInputData, PutCallFlag:PutCallFlag) =
        let sx = log(inputData.StockPrice / inputData.StrikePrice)
        let rv = inputData.InterestRate+inputData.Volatility*inputData.Volatility*0.5
        let rvt = rv*inputData.TimeToExpiry
        let vt = (inputData.Volatility*sqrt(inputData.TimeToExpiry))
        let d1=(sx + rvt)/vt   
        let d2=d1-vt
        match PutCallFlag with
        | Put -> 
            let ndD1 = inputData.StockPrice*this.NormalDistribution.Density(d1)*inputData.Volatility
            let ndD1' = ndD1/(2.0*sqrt(inputData.TimeToExpiry))
            let rx = inputData.InterestRate*inputData.StrikePrice
            let rt = exp(-inputData.InterestRate*inputData.TimeToExpiry)
            let cdD2 = rx*rt*this.CumulativeDistribution(-d2) 
            -(ndD1')+cdD2
        | Call -> 
            let ndD1 = inputData.StockPrice*this.NormalDistribution.Density(d1)*inputData.Volatility
            let ndD1' = ndD1/(2.0*sqrt(inputData.TimeToExpiry))
            let rx = inputData.InterestRate*inputData.StrikePrice
            let rt = exp(-inputData.InterestRate*inputData.TimeToExpiry)
            let cdD2 = this.CumulativeDistribution(d2)
            -(ndD1')-rx*rt*cdD2

    member this.BlackScholesRho (inputData:BlackScholesInputData, PutCallFlag:PutCallFlag) =
        let sx = log(inputData.StockPrice / inputData.StrikePrice)
        let rv = inputData.InterestRate+inputData.Volatility*inputData.Volatility*0.5
        let rvt = rv*inputData.TimeToExpiry
        let vt = (inputData.Volatility*sqrt(inputData.TimeToExpiry))
        let d1=(sx + rvt)/vt   
        let d2=d1-vt
        match PutCallFlag with
        | Put ->
            let xt = inputData.StrikePrice*inputData.TimeToExpiry
            let rt = exp(-inputData.InterestRate*inputData.TimeToExpiry)  
            -xt*rt*this.CumulativeDistribution(-d2)
        | Call -> 
            let xt = inputData.StrikePrice*inputData.TimeToExpiry
            let rt = exp(-inputData.InterestRate*inputData.TimeToExpiry)          
            xt*rt*this.CumulativeDistribution(d2)


    member this.PriceAtMaturity (inputData:MonteCarloInputData, randomValue:float) =
        let s = inputData.StockPrice
        let rv = (inputData.InterestRate-inputData.Volatility*inputData.Volatility/2.0)
        let rvt = rv*inputData.TimeToExpiry
        let vr = inputData.Volatility*randomValue
        let t = sqrt(inputData.TimeToExpiry)
        s*exp(rvt+vr*t)
    
    member this.MonteCarlo(inputData:MonteCarloInputData, randomValues:seq<float>) = 
        randomValues 
            |> Seq.map(fun randomValue -> this.PriceAtMaturity(inputData,randomValue) - inputData.StrikePrice )
            |> Seq.average




