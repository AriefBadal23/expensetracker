import {useEffect, useState} from "react";
import type {Bucket} from "../types/Bucket.tsx";
import BucketRow from "./BucketRow.tsx";

const BucketTable = () => {
    
   const [buckets, setBuckets] = useState<Bucket[]>([])
    // TODO: Error handling for failing api call
    
    useEffect(
        // call fetch function HERE
        () => {
            async function fetchUserBuckets(){
                const URL = "https://localhost:7118/api/v1/buckets/user"
                try{
                    const response = await fetch(URL, {
                        method: "GET",
                        credentials: "include"
                    })
                    const data = await response.json()
                    setBuckets(data.value)
                }
                catch (err){
                    console.log(err)
                }
            }
         fetchUserBuckets()
        },
        []
    )
    return (
        <div className="container mt-4">
            <table className="table">
                <thead>
                <tr>
                    <th scope="col">Bucket</th>
                    <th scope="col">Type</th>
                    <th scope="col">Delete</th>
                    <th scope="col">Update</th>
                </tr>
                </thead>
                <tbody>
                {
                    buckets.map((bucket:Bucket) => (
                        <BucketRow bucket={bucket} key={bucket.bucket.id}/>
                    ))
                }
                </tbody>
            </table>
        </div>
    )
}

export default BucketTable;