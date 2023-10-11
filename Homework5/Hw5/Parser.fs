﻿module Hw5.Parser

open System
open Hw5.Calculator
open Hw5.MaybeBuilder

let isArgLengthSupported (args:string[]): Result<'a,'b> =
    let isSupported = args.Length = 3
    match isSupported with
    | true -> Ok args
    | false -> Error Message.WrongArgLength
    
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isOperationSupported (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    match operation with
    | CalculatorOperation.Plus -> Ok (arg1, operation, arg2)
    | CalculatorOperation.Minus -> Ok (arg1, operation, arg2)
    | CalculatorOperation.Multiply -> Ok (arg1, operation, arg2)
    | CalculatorOperation.Divide -> Ok (arg1, operation, arg2)
    | _ -> Error Message.WrongArgFormatOperation


let parseOperation(arg:string):Result<CalculatorOperation,Message> =
    match arg with
    | "+" -> Ok CalculatorOperation.Plus
    | "-" -> Ok CalculatorOperation.Minus
    | "*" -> Ok CalculatorOperation.Multiply
    | "/" -> Ok CalculatorOperation.Divide
    | _ -> Error Message.WrongArgFormatOperation


let tryChangeType<'T> (value : obj) = 
    try 
        Some (Convert.ChangeType(value, typeof<'T>) :?> 'T)
    with
    | _ -> None

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isDividingByZero (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    match tryChangeType<'b> 0 with
    | None -> Ok (arg1,operation,arg2)
    | Some zero ->
        match operation with  
        | CalculatorOperation.Divide ->
            match arg2 with
            | _ when arg2 = zero -> Error Message.DivideByZero
            | _ -> Ok (arg1,operation,arg2)
        | _ -> Ok (arg1,operation,arg2)


let convertValue (arg:string) : Result<double,Message> = 
    match Double.TryParse arg with
        | (true,value) -> Ok value
        | _ -> Error Message.WrongArgFormat

let convertValues (args:string[]) : Result<('a * CalculatorOperation * 'b), Message> = 
    maybe
        {
        let! firstValue = convertValue args[0]
        let! secondValue = convertValue args[2]
        let! operation = parseOperation args[1]
        return (firstValue,operation,secondValue)
        }
    

let parseArgs (args: string[]): Result<('a * CalculatorOperation * 'b), Message> = 
    maybe
        {
        let! supportedArgs = isArgLengthSupported args           
        let! convertedArgs = convertValues supportedArgs        
        return convertedArgs
        }    
        
let parseCalcArguments (args: string[]): Result<'a, 'b>  =    
    maybe
        {        
        let! parsedArgs = parseArgs args
        let! parsedArgs = isOperationSupported parsedArgs
        let! parsedArgs = isDividingByZero parsedArgs
        return parsedArgs
        }    