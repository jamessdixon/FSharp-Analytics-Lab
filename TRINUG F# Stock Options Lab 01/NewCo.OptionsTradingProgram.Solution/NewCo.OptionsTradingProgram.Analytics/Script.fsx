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