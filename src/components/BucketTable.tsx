import {type Dispatch, type SetStateAction, useEffect, useState} from "react";
import type {Bucket} from "../types/Bucket.tsx";
import BucketRow from "./BucketRow.tsx";
import {getErrorMessage} from "../utils/utils.ts";


interface BucketTableProps{
    setShowBucketModal: Dispatch<SetStateAction<boolean>>
    setBuckets: Dispatch<SetStateAction<Bucket[]>>
    buckets: Bucket[]
}
const BucketTable = ({setShowBucketModal, setBuckets, buckets}:BucketTableProps) => {
    
   // const [buckets, setUserBuckets] = useState<Bucket[]>([])
    
    // TODO: Error handling for failing api call
    const [error, setError] = useState<string | null>(null)
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
                    
                    if(!response.ok){
                        let message = "Something went wrong."
                        
                        if(response.status === 401){
                            message="Unauthorized access."
                        }
                        else if(response.status == 404){
                            message="Unable to delete bucket."
                        }
                        setError(message)
                        return;
                    }
                    
                    const data = await response.json()
                    setBuckets(data.value)
                }
                catch (e){
                    const message = getErrorMessage(e);
                    console.error(message)
                    setError(message);
                }
            }
         fetchUserBuckets()
        },
        []
    )
    return (
        <div
            className="modal fade show"
            id="createTransaction"
            aria-labelledby="createTransactionLabel"
            aria-hidden="true"
            style={{display: "block"}}>

            <div className="modal-dialog">
                <div className="modal-content">
                    <div className="modal-header">
                        <h1 className="modal-title fs-5" id="createTransactionLabel"> Manage buckets</h1>

                        <button
                            type="button"
                            className="btn-close"
                            data-bs-dismiss="modal"
                            aria-label="Close"
                            onClick={() => setShowBucketModal(false)}
                        ></button>
                    </div>
                    <div className="modal-body">
                        <div className="container mt-4">
                            {error && <p style={{ color: "red", marginTop: "0.25rem" }}>{error}</p>}
                            
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
                                        <BucketRow bucket={bucket} key={bucket.bucket.id} setBuckets={setBuckets} setError={setError} error={error}/>
                                    ))
                                    
                                    
                                }
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div className="modal-footer">
                        <button
                            type="button"
                            className="btn btn-secondary"
                            data-bs-dismiss="modal"
                            onClick={() => setShowBucketModal(false)}
                        >
                            Close
                        </button>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default BucketTable;