import type {Bucket} from "../types/Bucket.tsx";
import {type Dispatch, type SetStateAction, useState} from "react";
import {getErrorMessage} from "../utils/utils.ts";

interface BucketRowProps{
    bucket: Bucket
    setBuckets: Dispatch<SetStateAction<Bucket[]>>
    setError: Dispatch<SetStateAction<string | null>>
    error: string | null
}
const BucketRow = ({bucket, setBuckets, setError, error}:BucketRowProps) => {
    
    const [modalIsShown, setShowModal] = useState<boolean>(false)
    
    async function DeleteBucket(bucketId:number | undefined){
        try{
            const URL = `https://localhost:7118/api/v1/buckets/${bucketId}`
            
            const response = await fetch(URL, {
                    method:"Delete",
                    credentials: "include",
                    headers: {
                        "Content-type": "application/json; charset=UTF-8"
                    },
                })
            
            
            let message = "Something went wrong."
            if(response.ok){
                setBuckets(prevState => prevState.filter(b => b.bucket.id !== bucketId));
                
            }
            if(!response.ok){
                console.error(`DELETE /buckets/${bucketId} failed`, {
                status: response.status,
                statusText: response.statusText
                });
                if(response.status === 400){
                    message = "Failed to delete bucket"
                    console.error(response.statusText)
                }
                if(response.status === 401){
                    message =  "Unauthorized access."
                }
                else if(response.status === 404){
                    message=  "Unable to delete the bucket"
                    console.error(response.statusText)


                }
                setError(message)
                console.error(error)
                return;
            }
        }
        catch(error){
            const message = getErrorMessage(error)
            console.log(error)
            setError(message)
        }
    }
    
        
    // const confirmDelete = (): boolean => {
    //    
    // }
    return (
        <>
            <div>
                {
                    modalIsShown ? (
                        <div
                            className="modal fade bd-example-modal-sm show"
                            role="dialog"
                            aria-labelledby="confirmDeletionLabel"
                            aria-hidden="false"
                            style={{ display: "block" }}
                        >
                            <div className="modal-dialog modal-sm" role="document">
                                <div className="modal-content">
                                    <div className="modal-header">
                                        <h5 className="modal-title" id="confirmDeletionLabel">Confirm deletion</h5>
                                        <button
                                            type="button"
                                            className="btn-close"
                                            aria-label="Close"
                                            onClick={() => setShowModal(false)}
                                        />
                                    </div>

                                    <div className="modal-body">
                                        <p>Are you sure you want to delete the bucket "{bucket.bucket.name}"?</p>
                                    </div>

                                    <div className="modal-footer">
                                        <button
                                            type="button"
                                            className="btn btn-secondary"
                                            onClick={() => setShowModal(false)}
                                        >
                                            Cancel
                                        </button>
                                        <button
                                            type="button"
                                            className="btn btn-danger"
                                            onClick={async () => {
                                                await DeleteBucket(bucket.bucket.id);
                                                setShowModal(false);
                                            }}
                                        >
                                            Delete
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    ) : null
                }
                
            </div>
            <tr key={bucket.bucket.id}>
                <td>{bucket.bucket.name}</td>
                <td>{bucket.bucket.type}</td>

                <td>
                    <button type="button"
                            aria-label="Delete transaction"
                            onClick={() => {
                                setShowModal(true);
                            }}
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
        </>
    )
}

export default BucketRow;