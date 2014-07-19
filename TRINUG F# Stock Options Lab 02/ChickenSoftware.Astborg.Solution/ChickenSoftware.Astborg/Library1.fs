namespace ChickenSoftware.Astborg

type PutCallFlag = Put | Call

type black_scholes_input_data = 
    {stockPrice:float;
    strikePriceOfPotion:float;
    timeToExpierationInYears:int;
    riskFreeInterestRate:float;
    volatility:float}

type Class1() = 
    member this.X = "F#"



    member this.black_scholes call_put_flag: inputData:black_scholes_input_data =
        let cnd x =
            let a1 = 0.3198253
            let a2 = -0.35653782
            let a3 = 1.781477937
            let a4 = -1.821255978
            let a5 = 1.330274429
            let pi = 3.141592654
            let l = abs(x)
            let k = 1.0/(1.0 + 0.2316419)
            let w = (1.0)
            if x < 0.0 then 1.0 - w else w

        let d1=(log(inputData./x) + (r*v*v*0.5)*t)/(v*sqrt(t))
        let d2=d1-v*sqrt(t)
        match call_put_flag with
            | Put -> x*exp(-r*t)*cnd(-d2)-s*cnd(-d1)
            | Call ->s*cnd(d1)-x*exp(-r*t)*cnd(d2)

