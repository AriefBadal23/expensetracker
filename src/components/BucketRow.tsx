import type {Bucket} from "../types/Bucket.tsx";
import type {Dispatch, SetStateAction} from "react";

interface BucketRowProps{
    bucket: Bucket
    setBuckets: Dispatch<SetStateAction<Bucket[]>>
}
const BucketRow = ({bucket, setBuckets}:BucketRowProps) => {
    
    async function DeleteBucket(bucketId:number){
        try{
            const URL = `https://localhost:7118/api/v1/buckets/${bucketId}`
            await fetch(URL, {
                    method:"Delete",
                    credentials: "include",
                    headers: {
                        "Content-type": "application/json; charset=UTF-8"
                    },
                }
                
                )
            setBuckets(prevState => prevState.filter(b => b.bucket.id !== bucketId));
        }
        catch(error){
            console.log(error)
        }
    }
    
    
    return (
        <tr key={bucket.bucket.id}>
            <td>{bucket.bucket.name}</td>
            <td>{bucket.bucket.type}</td>
           
            <td>
                <button type="button"
                        aria-label="Delete transaction"
                        onClick={() => DeleteBucket(Number(bucket.bucket.id))}
                >
                    <img src="delete.png" alt="Delete transaction"/>
                    
                </button>
            </td>
            <td>
                <button type="button"
                        aria-label="Update transaction">
                    <img src="update.png" alt="Update transaction"/></button>
            </td>
        </tr>
    )
}

export default BucketRow;