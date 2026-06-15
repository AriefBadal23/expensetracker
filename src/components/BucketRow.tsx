import type {Bucket} from "../types/Bucket.tsx";

interface BucketRowProps{
    bucket: Bucket
}
const BucketRow = ({bucket}:BucketRowProps) => {
    
    async function DeleteBucket(){
        
        try{
            const URL = ""
            await fetch(URL,
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
                        aria-label="Delete transaction">
                    <img src="delete.png" alt="Delete transaction"/></button>
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