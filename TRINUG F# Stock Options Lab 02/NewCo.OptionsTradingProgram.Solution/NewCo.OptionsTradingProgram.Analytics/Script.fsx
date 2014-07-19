open System.Collections.Generic
open System

let testData = [1.0 .. 6.0]
let sum = testData |> Seq.sum 
let average = testData |> Seq.average
let min = testData |> Seq.min
let max = testData |> Seq.max
let evens = testData |> Seq.filter(fun number -> number % 2. = 0.)
let addOne = testData |> Seq.map(fun number -> number + 1.)


//http://www.mathsisfun.com/data/standard-deviation.html
let variance (source:IEnumerable<double>) =
    let mean = Seq.average source
    let deltas = Seq.map(fun x -> pown(x-mean) 2) source
    Seq.average deltas

let standardDeviation(values:IEnumerable<double>) =
    sqrt(variance(values))

let movingAverage(values:IEnumerable<double>, windowSize:int)=
    values 
        |> Seq.windowed (windowSize)
        |> Seq.map(fun window -> window |> Seq.average)

let movingStandardDeviation(values:IEnumerable<double>, windowSize:int)=
    values 
        |> Seq.windowed (windowSize)
        |> Seq.map(fun window -> window |> standardDeviation)

let bollingerBands (values:IEnumerable<double>, windowSize:int)=
    let movingAverage = movingAverage(values,windowSize)
    let movingStandardDeviation = movingStandardDeviation(values,windowSize)
    let movingStandardDeviation' = movingStandardDeviation |> Seq.map(fun window -> window * 2.)
    Seq.zip movingAverage movingStandardDeviation'


let power baseNumber exponent = exp(exponent * log(baseNumber))
//let pow x n = exp(n*log(x))
//http://www.mathsisfun.com/definitions/power.html
//base is a reserved word
//also could have used pown
//http://msdn.microsoft.com/en-us/library/ee370371.aspx


//http://en.wikipedia.org/wiki/Cumulative_distribution_function
//let cnd(x) =
//        let a1 = 0.3198153
//        let a2 = -0.356563782
//        let a3 = 1.781477937
//        let a4 = -1.821255978
//        let a5 = 1.330274429
//        let pi = 3.141592654
//        let l = abs(x)
//        let k = 1.0/(1.0 + 0.2316419 * l)
//        let w = (1.0-1.0/sqrt(2.0*pi)*exp(-l*l/2.0)*(a1*k+a2*k*a3+k*(power k 3.0)+a4*(power k 4.0)+a5*(power k 5.0)))
//        if x < 0.0 then 1.0 - w else w

let cumulativeDistribution (x) =
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
        let a3' = a3*(power k 3.0)
        let a4' = a4*(power k 4.0)
        let a5' = a5*(power k 5.0)
        let w1 = 1.0/sqrt(2.0*pi)
        let w2 = exp(-l*l/2.0)
        let w3 = a1'+a2'+a3'+a4'+a5'
        let w  = 1.0-w1*w2*w3
        if x < 0.0 then 1.0 - w else w


type putCallFlag = Put | Call

type blackScholesInputData = 
    {stockPrice:float;
    strikePrice:float;
    timeToExpiry:float;
    interestRate:float;
    volatility:float}

let blackScholes(inputData:blackScholesInputData, putCallFlag:putCallFlag)=
   let sx = log(inputData.stockPrice / inputData.strikePrice)
   let rv = inputData.interestRate+inputData.volatility*inputData.volatility*0.5
   let rvt = rv*inputData.timeToExpiry
   let vt = (inputData.volatility*sqrt(inputData.timeToExpiry))
   let d1=(sx + rvt)/vt
   let d2=d1-vt
    
   match putCallFlag with
    | Put -> 
        let xrt = inputData.strikePrice*exp(-inputData.interestRate*inputData.timeToExpiry)
        let cdD1 = xrt*cumulativeDistribution(-d2)
        let cdD2 = inputData.stockPrice*cumulativeDistribution(-d1)
        cdD1-cdD2
    | Call ->
        let xrt = inputData.strikePrice*exp(-inputData.interestRate*inputData.timeToExpiry)
        let cdD1 = inputData.stockPrice*cumulativeDistribution(d1)
        let cdD2 = xrt*cumulativeDistribution(d2)
        cdD1-cdD2

let inputData = {stockPrice=58.60;strikePrice=60.;timeToExpiry=0.5;interestRate=0.01;volatility=0.3}
let runBSCall = blackScholes(inputData,Call)
let runBSPut = blackScholes(inputData,Put)

let daysToYears day = (float day) / 365.25
let thirityDayEquiv = daysToYears 30 

let inputData2 = {stockPrice=58.60;strikePrice=60.;timeToExpiry=(daysToYears 20);interestRate=0.01;volatility=0.3}
let runBSCall2 = blackScholes(inputData2,Call)
let runBSPut2 = blackScholes(inputData2,Put)

#r @"E:\Documents\TriNug\FSharpLab_20140615\NewCo.OptionsTradingProgram.Solution\packages\MathNet.Numerics.3.0.0\lib\net40\MathNet.Numerics.dll"
#r @"E:\Documents\TriNug\FSharpLab_20140615\NewCo.OptionsTradingProgram.Solution\packages\MathNet.Numerics.FSharp.3.0.0\lib\net40\MathNet.Numerics.FSharp.dll"
open MathNet.Numerics.Distributions;
let normalDistribution = new Normal(0.0, 1.0)
normalDistribution.Density(100.0)

let blackScholesDelta (inputData:blackScholesInputData, putCallFlag:putCallFlag) =
    let sx = log(inputData.stockPrice / inputData.strikePrice)
    let rv = inputData.interestRate+inputData.volatility*inputData.volatility*0.5
    let rvt = rv*inputData.timeToExpiry
    let vt = (inputData.volatility*sqrt(inputData.timeToExpiry))
    let d1=(sx + rvt)/vt
    match putCallFlag with
    | Put -> cumulativeDistribution(d1) - 1.0
    | Call -> cumulativeDistribution(d1)

let deltaPut = blackScholesDelta(inputData, Put)
let deltaCall = blackScholesDelta(inputData, Call)

let blackScholesGamma (inputData:blackScholesInputData) =
    let sx = log(inputData.stockPrice / inputData.strikePrice)
    let rv = inputData.interestRate+inputData.volatility*inputData.volatility*0.5
    let rvt = rv*inputData.timeToExpiry
    let vt = (inputData.volatility*sqrt(inputData.timeToExpiry))
    let d1=(sx + rvt)/vt
    normalDistribution.Density(d1)

let gamma = blackScholesGamma(inputData)

let blackScholesVega (inputData:blackScholesInputData) =
    let sx = log(inputData.stockPrice / inputData.strikePrice)
    let rv = inputData.interestRate+inputData.volatility*inputData.volatility*0.5
    let rvt = rv*inputData.timeToExpiry
    let vt = (inputData.volatility*sqrt(inputData.timeToExpiry))
    let d1=(sx + rvt)/vt   
    inputData.stockPrice*normalDistribution.Density(d1)*sqrt(inputData.timeToExpiry)

let vega = blackScholesVega(inputData)

let blackScholesTheta (inputData:blackScholesInputData, putCallFlag:putCallFlag) =
    let sx = log(inputData.stockPrice / inputData.strikePrice)
    let rv = inputData.interestRate+inputData.volatility*inputData.volatility*0.5
    let rvt = rv*inputData.timeToExpiry
    let vt = (inputData.volatility*sqrt(inputData.timeToExpiry))
    let d1=(sx + rvt)/vt   
    let d2=d1-vt
    match putCallFlag with
    | Put -> 
        let ndD1 = inputData.stockPrice*normalDistribution.Density(d1)*inputData.volatility
        let ndD1' = ndD1/(2.0*sqrt(inputData.timeToExpiry))
        let rx = inputData.interestRate*inputData.strikePrice
        let rt = exp(-inputData.interestRate*inputData.timeToExpiry)
        let cdD2 = rx*rt*cumulativeDistribution(-d2) 
        -(ndD1')+cdD2
    | Call -> 
        let ndD1 = inputData.stockPrice*normalDistribution.Density(d1)*inputData.volatility
        let ndD1' = ndD1/(2.0*sqrt(inputData.timeToExpiry))
        let rx = inputData.interestRate*inputData.strikePrice
        let rt = exp(-inputData.interestRate*inputData.timeToExpiry)
        let cdD2 = cumulativeDistribution(d2)
        -(ndD1')-rx*rt*cdD2

let thetaPut = blackScholesTheta(inputData, Put)
let thetaCall = blackScholesTheta(inputData, Call)

let blackScholesRho (inputData:blackScholesInputData, putCallFlag:putCallFlag) =
    let sx = log(inputData.stockPrice / inputData.strikePrice)
    let rv = inputData.interestRate+inputData.volatility*inputData.volatility*0.5
    let rvt = rv*inputData.timeToExpiry
    let vt = (inputData.volatility*sqrt(inputData.timeToExpiry))
    let d1=(sx + rvt)/vt   
    let d2=d1-vt
    match putCallFlag with
    | Put ->
        let xt = inputData.strikePrice*inputData.timeToExpiry
        let rt = exp(-inputData.interestRate*inputData.timeToExpiry)  
        -xt*rt*cumulativeDistribution(-d2)
    | Call -> 
        let xt = inputData.strikePrice*inputData.timeToExpiry
        let rt = exp(-inputData.interestRate*inputData.timeToExpiry)          
        xt*rt*cumulativeDistribution(d2)

let rhoPut = blackScholesRho(inputData, Put)
let rhoCall = blackScholesRho(inputData, Call)

type monteCarloInputData = 
    {stockPrice:float;
    strikePrice:float;
    timeToExpiry:float;
    interestRate:float;
    volatility:float}

let priceAtMaturity (inputData:monteCarloInputData, randomValue:float) =
    let s = inputData.stockPrice
    let rv = (inputData.interestRate-inputData.volatility*inputData.volatility/2.0)
    let rvt = rv*inputData.timeToExpiry
    let vr = inputData.volatility*randomValue
    let t = sqrt(inputData.timeToExpiry)
    s*exp(rvt+vr*t)
    
let maturityPriceInputData = {stockPrice=58.60;strikePrice=60.0;timeToExpiry=0.5;interestRate=0.01;volatility=0.3}
priceAtMaturity(maturityPriceInputData, 10.0)

let monteCarlo(inputData: monteCarloInputData, randomValues:seq<float>) = 
    randomValues 
        |> Seq.map(fun randomValue -> priceAtMaturity(inputData,randomValue) - inputData.strikePrice )
        |> Seq.average


let random = new System.Random()
let rnd() = random.NextDouble()
let data = [for i in 1 .. 1000 -> rnd() * 1.0]

let monteCarloInputData = {stockPrice=58.60;strikePrice=60.0;timeToExpiry=0.5;interestRate=0.01;volatility=0.3;}
monteCarlo(monteCarloInputData,data)
