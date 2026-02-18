import CreateTransactionForm from "./CreateTransactionForm";
import type {NewTransactionRow} from "../types/NewTransactionRow.tsx";
const CreateFormModal = ({ updateTable, SetShowModal,showModal, isUpdateForm, transactionID }: NewTransactionRow ) => {
    return (
    <>
        {
            showModal && SetShowModal !== undefined ?
                (
                <>
                    <div
                        className="modal fade show"
                        id="createTransaction"
                        aria-labelledby="createTransactionLabel"
                        aria-hidden="true"
                        style={{ display: "block" }}
                    >
                        <div className="modal-dialog">
                            <div className="modal-content">
                                <div className="modal-header">
                                <h1 className="modal-title fs-5" id="createTransactionLabel"> Transaction Details</h1>
                                    
                                    <button
                                        type="button"   
                                        className="btn-close"
                                        data-bs-dismiss="modal"
                                        aria-label="Close"
                                        onClick={() => SetShowModal(false)}
                                    ></button>
                                </div>
                                <div className="modal-body">
                                    <CreateTransactionForm updateTable={updateTable} SetShowModal={SetShowModal} showModal={showModal} isUpdateForm={isUpdateForm} transactionID={transactionID}/>
                                </div>
                                <div className="modal-footer">
                                    <button
                                        type="button"
                                        className="btn btn-secondary"
                                        data-bs-dismiss="modal"
                                        onClick={() => SetShowModal(false)}
                                    >
                                        Close
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </>
                ) :
                <>
                    
                </>
        }
      
    </>
  );
};

export default CreateFormModal;
