(* Copyright (c) 2013 MongoDB, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *)

namespace FSharp.MongoDB.Bson.Tests.Serialization

open MongoDB.Bson
open FsUnit
open NUnit.Framework

module FSharpListSerialization =

    type Record = { Ints : int list }

    [<Test>]
    let ``test serialize an empty list``() =
        let value = { Ints = [] }

        let result = serialize value
        let expected = BsonDocument("Ints", BsonArray List.empty<int>)

        result |> should equal expected

    [<Test>]
    let ``test deserialize an empty list``() =
        let doc = BsonDocument("Ints", BsonArray List.empty<int>)

        let result = deserialize<Record> doc
        let expected = { Ints = [] }

        result |> should equal expected

    [<Test>]
    let ``test serialize a list of one element``() =
        let value = { Ints = [ 0 ] }

        let result = serialize value
        let expected = BsonDocument("Ints", BsonArray [ 0 ])

        result |> should equal expected

    [<Test>]
    let ``test deserialize a list of one element``() =
        let doc = BsonDocument("Ints", BsonArray [ 0 ])

        let result = deserialize<Record> doc
        let expected = { Ints = [ 0 ] }

        result |> should equal expected

    [<Test>]
    let ``test serialize a list of multiple elements``() =
        let value = { Ints = [ 1; 2; 3 ] }

        let result = serialize value
        let expected = BsonDocument("Ints", BsonArray [ 1; 2; 3 ])

        result |> should equal expected

    [<Test>]
    let ``test deserialize a list of multiple elements``() =
        let doc = BsonDocument("Ints", BsonArray [ 1; 2; 3 ])

        let result = deserialize<Record> doc
        let expected = { Ints = [ 1; 2; 3 ] }

        result |> should equal expected

    module OptionType =

        type Record = { MaybeStrings : string option list }

        [<Test>]
        let ``test serialize a list of optional strings``() =
            let value = { MaybeStrings = [ Some "a"; None; Some "z" ] }

            let result = serialize value
            let expected =
                let values: (string|null) array = [| "a"; null; "z" |]
                BsonDocument("MaybeStrings", BsonArray values)

            result |> should equal expected

        [<Test>]
        let ``test deserialize a list of optional strings``() =
            let doc =
                let values: (string | null) array = [| "a"; null; "z" |]
                BsonDocument("MaybeStrings", BsonArray values)

            let result = deserialize<Record> doc
            let expected = { MaybeStrings = [ Some "a"; None; Some "z" ] }

            result |> should equal expected

    module RecordType =

        type KeyValuePair =
            { Key : string
              Value : int }

        type Record = { Elements : KeyValuePair list }

        [<Test>]
        let ``test serialize a list of record types``() =
            let value = { Elements = [ { Key = "a"; Value = 1 }
                                       { Key = "b"; Value = 2 }
                                       { Key = "c"; Value = 3 } ] }

            let result = serialize value
            let expected =
                BsonDocument(
                    "Elements",
                    BsonArray [ BsonDocument [ BsonElement("Key", BsonString "a")
                                               BsonElement("Value", BsonInt32 1) ]
                                BsonDocument [ BsonElement("Key", BsonString "b")
                                               BsonElement("Value", BsonInt32 2) ]
                                BsonDocument [ BsonElement("Key", BsonString "c")
                                               BsonElement("Value", BsonInt32 3) ] ])

            result |> should equal expected

        [<Test>]
        let ``test deserialize a list of record types``() =
            let doc =
                BsonDocument(
                    "Elements",
                    BsonArray [ BsonDocument [ BsonElement("Key", BsonString "a")
                                               BsonElement("Value", BsonInt32 1) ]
                                BsonDocument [ BsonElement("Key", BsonString "b")
                                               BsonElement("Value", BsonInt32 2) ]
                                BsonDocument [ BsonElement("Key", BsonString "c")
                                               BsonElement("Value", BsonInt32 3) ] ])

            let result = deserialize<Record> doc
            let expected = { Elements = [ { Key = "a"; Value = 1 }
                                          { Key = "b"; Value = 2 }
                                          { Key = "c"; Value = 3 } ] }

            result |> should equal expected
