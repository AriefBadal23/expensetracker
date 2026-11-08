import ImportForm from "./ImportForm.tsx";
import type {Dispatch, SetStateAction} from "react";

interface ImportFormModalProps {
    setShowModal: Dispatch<SetStateAction<boolean>>

}


const ImportFormModal = ({setShowModal}: ImportFormModalProps) => {

    return (
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
                        <h1 className="modal-title fs-5" id="createTransactionLabel">Import from file</h1>
                        <button
                            type="button"
                            className="btn-close"
                            data-bs-dismiss="modal"
                            aria-label="Close"
                            onClick={() => setShowModal(false)}

                        ></button>
                    </div>
                    <div className="modal-body">
                        <ImportForm/>
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
    )
}

export default ImportFormModal