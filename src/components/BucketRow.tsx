import type {Bucket} from "../types/Bucket.tsx";

interface BucketRowProps{
    bucket: Bucket
}
const BucketRow = ({bucket}:BucketRowProps) => {
    
    async function DeleteBucket(bucketId:number){
        try{
            const URL = `http://localhost:5286/api/v1/Buckets/${bucketId}`
            await fetch(URL, {
                    method:"Delete",
                    credentials: "include",
                    headers: {
                        "Content-type": "application/json; charset=UTF-8"
                    },
                }
                
                )
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