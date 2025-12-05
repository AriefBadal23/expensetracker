import type { NewTransactionRow } from "../types/NewTransactionRow";
import CreateForm from "./CreateForm";

const CreateFormModal = ({
  updateTable,
  updateBucketAmount,
}: NewTransactionRow) => {
  return (
    <>
      <button
        type="button"
        className="btn btn-primary"
        data-bs-toggle="modal"
        data-bs-target="#createTransaction"
      >
        Add new transaction
      </button>

      <div
        className="modal fade"
        id="createTransaction"
        aria-labelledby="createTransactionLabel"
        aria-hidden="true"
      >
        <div className="modal-dialog">
          <div className="modal-content">
            <div className="modal-header">
              <h1 className="modal-title fs-5" id="createTransactionLabel">
                Create new Transaction
              </h1>
              <button
                type="button"
                className="btn-close"
                data-bs-dismiss="modal"
                aria-label="Close"
              ></button>
            </div>
            <div className="modal-body">
              <CreateForm
                updateTable={updateTable}
                updateBucketAmount={updateBucketAmount}
              />
            </div>
            <div className="modal-footer">
              <button
                type="button"
                className="btn btn-secondary"
                data-bs-dismiss="modal"
              >
                Close
              </button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default CreateFormModal;
