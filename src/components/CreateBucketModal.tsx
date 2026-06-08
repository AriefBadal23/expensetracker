import {type Dispatch, type SetStateAction} from "react";
import CreateBucketForm from "./CreateBucketForm.tsx";
import type { Bucket as BucketType } from "../types/Bucket";


// type Bucket = {
//     name: string
//     icon: string
// }


interface ICreateBucketModal{
    showModal?:boolean
    setShowModal: Dispatch<SetStateAction<boolean>>
    setBuckets: Dispatch<SetStateAction<BucketType[]>>
    setErrorMessage: Dispatch<SetStateAction<Error | undefined>>
    
}

const CreateBucketModal = ({showModal, setShowModal, setBuckets, setErrorMessage}:ICreateBucketModal) => {
    console.log(`Modal state is ${showModal}`)
    return (
        <>
            {
                showModal === true && setShowModal !== undefined ? (
                    <div
                        className="modal fade show"
                        id="createTransaction"
                        aria-labelledby="createTransactionLabel"
                        aria-hidden="true"
                        style={{display: "block"}}
                    >
                        <div className="modal-dialog">
                            <div className="modal-content">
                                <div className="modal-header">
                                    <h1 className="modal-title fs-5" id="createTransactionLabel"> Create bucket</h1>

                                    <button
                                        type="button"
                                        className="btn-close"
                                        data-bs-dismiss="modal"
                                        aria-label="Close"
                                        onClick={() => setShowModal(false)}
                                    ></button>
                                </div>
                                <div className="modal-body">
                                    <CreateBucketForm setShowModal = {setShowModal} setBuckets={setBuckets} setErrorMessage={setErrorMessage}/>
                                </div>
                                <div className="modal-footer">
                                    <button
                                        type="button"
                                        className="btn btn-secondary"
                                        data-bs-dismiss="modal"
                                        onClick={() => setShowModal(false)}
                                    >
                                        Close
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>

                ) : null
            }
        </>
        
       
    )
}                           

 export default CreateBucketModal