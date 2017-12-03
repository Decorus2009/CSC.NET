open System;

let random = (new System.Random(int System.DateTime.Now.Ticks)).Next

// 1.
printfn "1. Fibonacci numbers %s" Environment.NewLine

let n = 10

let fibSequence = Seq.unfold (fun (prev, cur) -> Some(prev, (cur, prev + cur))) (0, 1)
printfn "%d'th Fib number using sequence is: %d" n (Seq.item n fibSequence) // 55

let nthFibNumber n =
    let mutable counter = 0

    let rec computeFibNumberFrom prev cur =
        if counter = n then prev
        else
            counter <- counter + 1
            computeFibNumberFrom cur (prev + cur)
    computeFibNumberFrom 0 1

printfn "%d'th Fib number using rec fun is: %d" n (nthFibNumber 10) // 55


// 2.
printfn "%s2. Reversing list %s" Environment.NewLine Environment.NewLine

let reverse list =
    let rec reverseHelper ls acc =
        match ls with
        | [] -> acc
        | [x] -> x :: acc
        | h :: t -> reverseHelper t (h :: acc)
    reverseHelper list []

let randomListToReverse = List.init 10 random

printfn "Before: %A" randomListToReverse
printfn "After: %A" (reverse randomListToReverse)


// 3.
printfn "%s3. Merge sort %s" Environment.NewLine Environment.NewLine

let split list =
    let rec splitHelper ls acc1 acc2 =
        match ls with
        | [] -> (acc1, acc2)
        | [x] -> (acc1, x :: acc2)
        | h :: t ->
            if List.length acc1 >= List.length list / 2 then
                splitHelper t acc1 (h :: acc2)
            else
                splitHelper t (h :: acc1) acc2
    splitHelper list [] []

let rec merge list1 list2 =
    match list1, list2 with
        | h1 :: t1, h2 :: t2 ->
            if h1 < h2 then
                h1 :: merge t1 list2
            else
                h2 :: merge list1 t2
        | h1 :: t1, [] -> h1 :: t1
        | [], h2 :: t2 -> h2 :: t2
        | [], [] -> []

let rec mergeSort list =
    match list with
    | [] -> []
    | [x] -> [x]
    | h :: t ->
        let (list1, list2) = split list
        merge (mergeSort list1) (mergeSort list2)

let randomListToSort = List.init 30 random

printfn "Before: %A" randomListToSort
printfn "After: %A" (mergeSort randomListToSort)


// 4.
printfn "%s4. Expression evaluation %s" Environment.NewLine Environment.NewLine

type Expr =
    | Const of int
    | Add of Expr * Expr
    | Sub of Expr * Expr
    | Mul of Expr * Expr
    | Div of Expr * Expr

let rec eval expr =
    match expr with
    | Const x -> x
    | Add (x, y) -> (eval x) + (eval y)
    | Sub (x, y) -> (eval x) - (eval y)
    | Mul (x, y) -> (eval x) * (eval y)
    | Div (x, y) -> (eval x) / (eval y)

let expression = Mul(
                    Add(Sub(Const 5, Const 1),
                        Mul(Const 5, Const 2)),
                    Div(Const 35, Const 7))

printfn "((5 - 1) + (5 * 2)) * (35 / 7) = %d" (eval expression)


// 5.
printfn "%s5. Prime numbers %s" Environment.NewLine Environment.NewLine

let isPrime n =
    let sqrt_ = float >> sqrt >> int
    List.forall (fun x -> n % x <> 0) [2 .. sqrt_ n]

let limit = 1000

// в Seq.initInfinite нельзя без else ветви.
// Поэтому приходится что-то вставлять в последовательность, если даже число не является простым
// А потом это отфильтровывается. Нельзя ли написать как-то более прилично через Seq.initInfinite?
// (преподаватель сказал, тут надо использовать Seq.initInfinite)
let rec primesUsingInitInfinite  =
    Seq.initInfinite (fun x -> if isPrime x then x else -1) |> Seq.filter (fun x -> x >= 2)

printfn "Using initInfinite %A" (primesUsingInitInfinite |> Seq.takeWhile (fun x -> x < limit) |> Seq.toList)

let rec primes =
    let rec getPrimesStartingFrom n =
        seq {
            if isPrime n then
                yield n
            yield! getPrimesStartingFrom (n + 1)
        }
    getPrimesStartingFrom 2

printfn "Using rec fun %A" (primes |> Seq.takeWhile (fun x -> x < limit) |> Seq.toList)
